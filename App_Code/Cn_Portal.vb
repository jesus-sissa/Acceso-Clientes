Imports System.Data.SqlClient
Imports System.Data
Imports PortalSIAC.cn_Datos
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports SiacMovilModel

Public Class Cn_Portal

#Region "Variables Privadas"

    Private Session As HttpSessionState
    Private Request As HttpRequest
    Shared pdfw As PdfWriter

#End Region

#Region "Constructores"

    Public Sub New(ByVal MySession As HttpSessionState, ByVal MyRequest As HttpRequest)
        Session = MySession
        Request = MyRequest
    End Sub

#End Region

#Region "Propiedades"

    Public Property pId_Usuario() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("Id_Usuario"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("Id_Usuario") = value
        End Set
    End Property

    Public Property pId_Sucursal() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("Id_Sucursal"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("Id_Sucursal") = value
        End Set
    End Property

    Public Property pTipo_Contacto() As Short
        Get
            Dim res As Short = 0
            Short.TryParse(Session("TipoContacto"), res)
            Return res
        End Get
        Set(ByVal value As Short)
            Session("TipoContacto") = value
        End Set
    End Property

    Public Property pNombre() As String
        Get
            Return Session("Nombre")
        End Get
        Set(ByVal value As String)
            Session("Nombre") = value
        End Set
    End Property

    Public Property pNombre_Usuario() As String
        Get
            Return Session("NombreUsuario")
        End Get
        Set(ByVal value As String)
            Session("NombreUsuario") = value
        End Set
    End Property

    Public Property pId_Cliente() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("IdCliente"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("IdCliente") = value
        End Set
    End Property

    Public Property pId_Cia() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("IdCia"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("IdCia") = value
        End Set
    End Property

    Public ReadOnly Property pIp_Cliente() As String
        Get
            Return Request.UserHostAddress
        End Get
    End Property

    Public ReadOnly Property pEstacion_Cliente() As String
        Get
            Return Request.UserHostName
        End Get
    End Property

    Public Property pId_ClienteGrupo() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("Id_ClienteGrupo"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("Id_ClienteGrupo") = value
        End Set
    End Property

    Public Property pExiste_Grupo() As String
        Get
            Dim res As String = ""
            res = Session("ExisteGrupo")
            Return res
        End Get
        Set(value As String)
            Session("ExisteGrupo") = value
        End Set
    End Property

    Public Property pCiaTV() As String
        Get
            Dim res As String = ""
            res = Session("CiaTV")
            Return res
        End Get
        Set(value As String)
            Session("CiaTV") = value
        End Set
    End Property
    Public Property pClaveSIAC() As String
        Get
            Dim res As String = ""
            res = Session("ClaveSIAC")
            Return res
        End Get
        Set(value As String)
            Session("ClaveSIAC") = value
        End Set
    End Property

#End Region

#Region "Constantes"

    Public Const ClaveModulo As String = "25"

#End Region

#Region "Funciones Comunes"

    Public Sub TrataEx(ByVal Ex As Exception)
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim cmd As SqlCommand = Crea_Comando("Cli_Errores_Create", CommandType.StoredProcedure, cnn)

        CreaParametro(cmd, "@Id_Usuario", SqlDbType.Int, pId_Usuario)
        CreaParametro(cmd, "@Estacion", SqlDbType.VarChar, pEstacion_Cliente)
        CreaParametro(cmd, "@EstacionIP", SqlDbType.VarChar, pIp_Cliente)
        CreaParametro(cmd, "@EstacionMAC", SqlDbType.VarChar, "")
        CreaParametro(cmd, "@Donde", SqlDbType.VarChar, Ex.StackTrace.ToString())
        If TypeOf (Ex) Is SqlException Then
            CreaParametro(cmd, "@Numero_Error", SqlDbType.VarChar, CType(Ex, SqlException).ErrorCode)
        Else
            CreaParametro(cmd, "@Numero_Error", SqlDbType.VarChar, 0)
        End If
        CreaParametro(cmd, "@Descripcion", SqlDbType.VarChar, Ex.Message)

        Try
            EjecutaNonQuery(cmd)
        Finally

        End Try

    End Sub

    Public Function GetHoras(Intervalo As Integer) As DataTable

        Dim tbl As New DataTable
        tbl.Columns.Add("Hora", GetType(Date))

        Dim hora As Date = TimeSerial(0, 0, 0)

        While hora < TimeSerial(24, 0, 0)
            Dim row As DataRow = tbl.NewRow

            row("Hora") = hora

            tbl.Rows.Add(row)

            hora = DateAdd(DateInterval.Minute, Intervalo, hora)
        End While

        Return tbl
    End Function
#End Region

#Region "Base"
    Public Function fn_ValidaPermisos() As Boolean

        If pId_Usuario = 0 Then Return False
        'Se le asigno la conexion de settings 
        Dim cnx As New SqlConnection(Session("ConexionCentral"))
        Dim cmd As SqlCommand = Crea_Comando("web_Permisos_GetOpciones", CommandType.StoredProcedure, cnx)

        CreaParametro(cmd, "@IdUsuario", SqlDbType.Int, pId_Usuario)

        Dim tbl_Opciones As Data.DataTable = EjecutaConsulta(cmd)
        tbl_Opciones.CaseSensitive = False
        Dim dr_Enlaces As DataRow() = Nothing
        Dim rutaCompleta As String = String.Empty
        Dim carpetaAplicacion As String = Request.ApplicationPath.ToUpper
        Dim vacio As String = ""
        If carpetaAplicacion = "/" Then vacio = "/"
        rutaCompleta = Request.Url.AbsolutePath.Trim.ToUpper.Replace(carpetaAplicacion, vacio)
        dr_Enlaces = tbl_Opciones.Select("Enlace = '~" & rutaCompleta & "'")
        If rutaCompleta = "/TRASLADO/IMPRIMIRR.ASPX" Then
            Return 1
        End If
        Return dr_Enlaces.Count > 0

    End Function
#End Region

#Region "Master"

    Public Sub fn_LoadMenu(ByRef Mapa As Menu)
        Mapa.Items.Clear()

        'Se le agrega la cadena de Settings por que es la central
        Dim cnx As New SqlConnection(Session("ConexionCentral"))
        Dim cmd As SqlCommand = Crea_Comando("web_Usuarios_GetMenus", CommandType.StoredProcedure, cnx)
        CreaParametro(cmd, "@IdUsuario", SqlDbType.Int, pId_Usuario)
        Dim dt_Menus As Data.DataTable = EjecutaConsulta(cmd)

        cmd = Crea_Comando("web_Usuarios_GetOpciones", CommandType.StoredProcedure, cnx)
        CreaParametro(cmd, "@IdUsuario", SqlDbType.Int, pId_Usuario)
        Dim dt_Opciones As Data.DataTable = EjecutaConsulta(cmd)

        Dim mi_Principal As New MenuItem("Menu Inicio<br />")
        If dt_Menus.Rows.Count = 0 Then mi_Principal.Text = ""

        For Each dr_Menus As DataRow In dt_Menus.Rows
            Dim mi_menu As New MenuItem(dr_Menus("Descripcion"))
            mi_menu.NavigateUrl = dr_Menus("Enlace")

            For Each dr_Opciones As DataRow In dt_Opciones.Select("Id_Menu = " & dr_Menus("Id_Menu"))
                Dim mi_Opcion As New MenuItem(dr_Opciones("Descripcion"))
                mi_Opcion.NavigateUrl = dr_Opciones("Enlace")
                mi_menu.ChildItems.Add(mi_Opcion) 'agrega las opciones del menu
            Next
            mi_Principal.ChildItems.Add(mi_menu) 'agrega los menús
        Next

        Mapa.Items.Add(mi_Principal)
    End Sub

#End Region

#Region "Funciones de Login"

    'Public Function fn_Login_Validar(ByVal Usuario As String) As DataRow

    '    Try
    '        Dim cmd As SqlCommand = CreaComando("web_Login_validar")
    '        CreaParametro(cmd, "@Usuario", Usuario)

    '        Dim tbl As DataTable = EjecutaConsulta(cmd)

    '        If tbl.Rows.Count = 0 Then
    '            Return Nothing
    '        Else
    '            Return tbl.Rows(0)
    '        End If

    '    Catch ex As Exception
    '        TrataEx(ex)
    '        Return Nothing
    '    End Try

    'End Function

    'La funcion fn_Login_Validar se reemplazo por fn_Login_Validar_Central que es la que valida ya en la 
    'base de datos CTRSIAC...

    Public Function fn_Login_Validar_Central(ByVal Usuario As String, ByVal Clave_Unica As String) As DataTable
        Dim dt As DataTable = Nothing
        Dim tb = New DataTable()
        Try


            'Esto se esta probando ya que la consulta actual sta teniendo problemas a la hora de realizar la consulta
            '---------------------------------------------------------------
            'Dim conectionString As New SqlConnection("Data Source=DVL-MTY-V01; Initial Catalog=CTRSIAC; User ID=SIACDEVELOPER;Password=5iacDeveloper*1")
            'conectionString.Open()


            'Dim command As SqlCommand = New SqlCommand("Cli_Usuarios_Read", conectionString)

            'command.CommandType = CommandType.StoredProcedure
            'command.Parameters.Add(New SqlParameter("@Usuario", "IVANN"))
            'command.Parameters.Add(New SqlParameter("@Clave_Unica", "BCA9208108V6"))



            'Using dr As SqlDataReader = command.ExecuteReader()
            '    tb.Load(dr)
            'End Using

            'tb = EjecutaConsulta(command)
            'MsgBox(tb.Rows(0).ItemArray(2))

            '---------------------------------------------------------------

            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Cli_Usuarios_Read", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Usuario", SqlDbType.VarChar, Usuario, True)
            CreaParametro(cmd, "@Clave_Unica", SqlDbType.VarChar, Clave_Unica, True)
            dt = EjecutaConsulta(cmd)




        Catch ex As Exception
            Return Nothing
        End Try
        Return dt
        'Return tb
    End Function

    Public Function fn_Crear_Login(ByVal Id_Usuario As Integer) As Integer

        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Cli_Login_Create", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
            CreaParametro(cmd, "@Estacion", SqlDbType.VarChar, "")
            CreaParametro(cmd, "@EstacionIP", SqlDbType.VarChar, "")
            CreaParametro(cmd, "@EstacionMAC", SqlDbType.VarChar, "")

            Return EjecutaScalar(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return 0
        End Try

    End Function

    Public Function fn_Crear_Log(ByVal Id_Login As Integer, ByVal Descripcion As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Cli_Log_Create", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Login", SqlDbType.Int, Id_Login)
            CreaParametro(cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)

            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Function fn_Login_GetCajaBancaria(ByVal Id_Cliente As Integer) As Integer

        Try
            Dim cnn As New SqlConnection(Session("Conexionlocal"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Web_CajasBancarias_GetXCliente", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
            Dim dt As DataTable = EjecutaConsulta(cmd)
            If dt Is Nothing Then Return -1

            If dt.Rows.Count = 0 Then Return 0
            Return dt.Rows(0)("Id_CajaBancaria")
        Catch ex As Exception
            Return -1
        End Try

    End Function

    Public Function fn_Login_GetIdPadre(ByVal Id_Cliente As Integer, ByVal IdSucursal As Integer) As Integer

        Try
            Dim cnn As New SqlConnection(Session("Conexionlocal"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Web_Cat_Clientes_GetPadre", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, IdSucursal)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")

            Dim dt As DataTable = EjecutaConsulta(cmd)
            If dt Is Nothing Then Return -1

            If dt.Rows.Count = 0 Then Return 0

            Return dt.Rows(0)("Id_Padre")
        Catch ex As Exception
            Return -1
        End Try

    End Function

    Public Function fn_Login_GetHayGrupo(ByVal Id_Cliente As Integer) As DataTable

        Try
            Dim cnn As New SqlConnection(Session("Conexionlocal"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("web_Cat_ClientesGruposD_Get2", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.VarChar, Id_Cliente)
            Dim dt As DataTable = EjecutaConsulta(cmd)
            Return dt
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

#End Region

#Region "Conciliación 06/Enero/2017"

    Public Function fn_ConsultaBoveda_Conciliacion(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Id_ClienteCombo As Integer, ByVal TodosClientes As String, ByVal Nivel As Integer, ByVal Caja As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Boveda_Get")

            If Nivel = 1 Then
                If TodosClientes = "S" Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            CreaParametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            CreaParametro(cmd, "@Todos", SqlDbType.VarChar, TodosClientes)

            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Caja)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaProservicios_Conciliacion(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Id_ClienteCombo As Integer, ByVal TodosClientes As String, ByVal Nivel As Integer, ByVal Caja As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Servicios_Get2")

            If Nivel = 1 Then
                If TodosClientes = "S" Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            CreaParametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            CreaParametro(cmd, "@Todos", SqlDbType.VarChar, TodosClientes)
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Caja)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaArchivos_Conciliacion(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Id_ClienteCombo As Integer, ByVal TodosClientes As String, ByVal Nivel As Integer, ByVal Caja As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Archivos_Get")

            If Nivel = 1 Then
                If TodosClientes = "S" Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            CreaParametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            CreaParametro(cmd, "@Todos", SqlDbType.VarChar, TodosClientes)
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Caja)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
#End Region

#Region "ConsultaDepositos"

    Public Function fn_ConsultaClientes() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_Clientes_GetHijos")
            CreaParametro(cmd, "@AgregarPadre", SqlDbType.VarChar, "S")
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
            CreaParametro(cmd, "@Id_Padre", SqlDbType.Int, pId_Cliente)
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaDepositos_Dotaciones(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Status As String, ByVal Todos As Boolean, ByVal Id_ClienteCombo As Integer, ByVal TodosClientes As Boolean, ByVal Nivel As Integer, ByVal Caja As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Servicios_Get")

            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            CreaParametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            If Not Todos Then CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            If Not TodosClientes Then CreaParametro(cmd, "@Todos", SqlDbType.VarChar, "N")
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Caja)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaDepositos_XMoneda(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Status As String, ByVal Todos As Boolean, ByVal Id_ClienteCombo As Integer, ByVal TodosClientes As Boolean, ByVal Nivel As Integer, ByVal Caja As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Servicios_Get_Moneda")

            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            CreaParametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            If Not Todos Then CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            If Not TodosClientes Then CreaParametro(cmd, "@Todos", SqlDbType.VarChar, "N")
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Caja)
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaDepositos_Fichas(ByVal IdServicio As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = cn_Datos.CreaComando("web_Servicios_Fichas")
            CreaParametro(cmd, "@Id_Servicio", SqlDbType.Int, IdServicio)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaDepositos_Parciales(ByVal Id_Ficha As String) As DataTable
        Try '03/sep/2015
            Dim cmd As SqlCommand = CreaComando("Pro_Parciales_Get")
            CreaParametro(cmd, "@Id_Ficha", SqlDbType.Int, Id_Ficha)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaDepositos_ParcialesDetalle(ByVal Id_Parcial As String) As DataTable
        Try '03/sep/2015
            Dim cmd As SqlCommand = CreaComando("Pro_ParcialesD_Get")
            CreaParametro(cmd, "@Id_Parcial", SqlDbType.Int, Id_Parcial)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaDepositos_Efectivo(ByVal Id_Ficha As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Servicios_Efectivo")
            CreaParametro(cmd, "@Id_Ficha", SqlDbType.Int, Id_Ficha)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaDepositos_Cheques(ByVal Id_Ficha As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Sevicios_Cheques")
            CreaParametro(cmd, "@Id_Ficha", SqlDbType.Int, Id_Ficha)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaDepositos_Otros(ByVal Id_Ficha As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Servicios_Otros")
            CreaParametro(cmd, "@Id_Ficha", SqlDbType.Int, Id_Ficha)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaDepositos_Falsos(ByVal Id_Ficha As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Pro_FichasFalsos_Get")
            CreaParametro(cmd, "@Id_Ficha", SqlDbType.Int, Id_Ficha)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

#End Region

#Region "Detalle Depositos"

    Public Function ObtenerSesiones(ByVal Fecha As Date) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Pro_Sesion_GetByFecha")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Fecha", SqlDbType.DateTime, Fecha)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_CargarServicios(ByVal Id_Caja As Integer, ByVal S_Desde As Integer, ByVal S_Hasta As Integer, ByVal Moneda As Integer, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        Try

            Dim cmd As SqlCommand = CreaComando("web_Pro_Servicios_GetProceso")
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_Caja)
            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
                End If

            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If
            CreaParametro(cmd, "@S_Desde", SqlDbType.Int, S_Desde)
            CreaParametro(cmd, "@S_Hasta", SqlDbType.Int, S_Hasta)
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Moneda)
            CreaParametro(cmd, "@Dpto_Procesa", SqlDbType.VarChar, "T")
            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_Desglose_Servicios(ByVal Id_Servicio As Integer, ByVal Id_Moneda As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Pro_FichasEfectivo_GetFacturacion")
            CreaParametro(cmd, "@Id_Servicio", SqlDbType.Int, Id_Servicio)
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    'De aqui empiezan las funciones para el reporte de excel de detalle de depositos X REMISION

    Public Function fn_DetalleDepositos_GetDenominaciones(ByVal Id_Moneda As Integer) As DataTable
        Dim cmd As SqlCommand = CreaComando("Cat_DenominacionesMoneda_Get")
        CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_DetalleDepositos_GetServicios(ByVal Id_CajaBancaria As Integer, ByVal Id_Moneda As Integer, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal Id_GrupoF As Integer, ByVal Dpto As Char, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        Dim cmd As SqlCommand = CreaComando("web_Pro_Servicios_GetExportarRem")
        'se usaba este antes 28/11/2014 'Pro_Servicios_GetReporteConcePro
        CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
        If Nivel = 1 Then
            If TodosClientes Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            Else
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
            End If

        ElseIf Nivel = 2 Then
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
        End If

        If pExiste_Grupo = "S" Then
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If
        CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        CreaParametro(cmd, "@S_Desde", SqlDbType.Int, Desde)
        CreaParametro(cmd, "@S_Hasta", SqlDbType.Int, Hasta)
        CreaParametro(cmd, "@Dpto_Procesa", SqlDbType.VarChar, Dpto)
        CreaParametro(cmd, "@Id_GrupoDepo", SqlDbType.Int, Id_GrupoF)
        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_DetalleDepositos_GetDesglose(ByVal Id_CajaBancaria As Integer, ByVal Id_Moneda As Integer, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal Id_GrupoF As Integer, ByVal Dpto As Char, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        Dim cmd As SqlCommand = CreaComando("Pro_FichasEfectivo_GetReporteConcePro")
        CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
        If Nivel = 1 Then
            If TodosClientes Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            Else
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
            End If

        ElseIf Nivel = 2 Then
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
        End If

        If pExiste_Grupo = "S" Then
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If

        CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        CreaParametro(cmd, "@S_Desde", SqlDbType.Int, Desde)
        CreaParametro(cmd, "@S_Hasta", SqlDbType.Int, Hasta)
        CreaParametro(cmd, "@Dpto_Procesa", SqlDbType.VarChar, Dpto)
        CreaParametro(cmd, "@Id_GrupoDepo", SqlDbType.Int, Id_GrupoF)
        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function Fn_DetalleDepositos_Cheques(ByVal Id_CajaBancaria As Integer, ByVal S_Desde As Integer, ByVal S_Hasta As Integer, ByVal Id_Moneda As Integer, ByVal Dpto_procesa As String, ByVal Id_GrupoDepo As Integer, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable

        Dim Cmd As SqlCommand = CreaComando("Pro_FichasCheques_GetCheques")
        CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
        If Nivel = 1 Then
            If TodosClientes Then
                CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            Else
                CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
            End If

        ElseIf Nivel = 2 Then
            CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
        End If

        If pExiste_Grupo = "S" Then
            CreaParametro(Cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If

        CreaParametro(Cmd, "@S_Desde", SqlDbType.Int, S_Desde)
        CreaParametro(Cmd, "@S_Hasta", SqlDbType.Int, S_Hasta)
        CreaParametro(Cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        CreaParametro(Cmd, "@Dpto_procesa", SqlDbType.VarChar, Dpto_procesa)
        CreaParametro(Cmd, "@Id_GrupoDepo", SqlDbType.Int, Id_GrupoDepo)
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_DetalleDepositos_GetServiciosEsp(ByVal Id_CajaBancaria As Integer, ByVal Id_Moneda As Integer, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal Id_GrupoF As Integer, ByVal Dpto As Char, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        Dim cmd As SqlCommand = CreaComando("Pro_Servicios_GetReporteConcePro2")
        CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)

        If Nivel = 1 Then
            If TodosClientes Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            Else
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
            End If

        ElseIf Nivel = 2 Then
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
        End If

        If pExiste_Grupo = "S" Then
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If

        CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        CreaParametro(cmd, "@S_Desde", SqlDbType.Int, Desde)
        CreaParametro(cmd, "@S_Hasta", SqlDbType.Int, Hasta)
        CreaParametro(cmd, "@Dpto_Procesa", SqlDbType.VarChar, Dpto)
        CreaParametro(cmd, "@Id_GrupoDepo", SqlDbType.Int, Id_GrupoF)
        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_DetalleDepositos_GetDesgloseEsp(ByVal Id_CajaBancaria As Integer, ByVal Id_Moneda As Integer, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal Id_GrupoF As Integer, ByVal Dpto As Char, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        Dim cmd As SqlCommand = CreaComando("Pro_FichasEfectivo_GetReporteConcePro2")
        CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
        If Nivel = 1 Then
            If TodosClientes Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            Else
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
            End If

        ElseIf Nivel = 2 Then
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
        End If

        If pExiste_Grupo = "S" Then
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If
        CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        CreaParametro(cmd, "@S_Desde", SqlDbType.Int, Desde)
        CreaParametro(cmd, "@S_Hasta", SqlDbType.Int, Hasta)
        CreaParametro(cmd, "@Dpto_Procesa", SqlDbType.VarChar, Dpto)
        CreaParametro(cmd, "@Id_GrupoDepo", SqlDbType.Int, Id_GrupoF)
        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function Fn_DetalleDepositos_ChequesEsp(ByVal Id_CajaBancaria As Integer, ByVal S_Desde As Integer, ByVal S_Hasta As Integer, ByVal Id_Moneda As Integer, ByVal Dpto_procesa As String, ByVal Id_GrupoDepo As Integer, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable

        Dim Cmd As SqlCommand = CreaComando("Pro_FichasCheques_GetCheques2")
        CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
        If Nivel = 1 Then
            If TodosClientes Then
                CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            Else
                CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
            End If

        ElseIf Nivel = 2 Then
            CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
        End If
        If pExiste_Grupo = "S" Then
            CreaParametro(Cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If
        CreaParametro(Cmd, "@S_Desde", SqlDbType.Int, S_Desde)
        CreaParametro(Cmd, "@S_Hasta", SqlDbType.Int, S_Hasta)
        CreaParametro(Cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
        CreaParametro(Cmd, "@Dpto_procesa", SqlDbType.VarChar, Dpto_procesa)
        CreaParametro(Cmd, "@Id_GrupoDepo", SqlDbType.Int, Id_GrupoDepo)
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_SolicitudDotaciones_GetCajasBancarias_DetalleDeposito() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_SolicitudDotaciones_GetCajasBancarias")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Validación de Tripulación"

    Public Function fn_ValidacionTripulacion_GetLista() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Tripulacion_Validar")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ValidacionTripulacion_GetLista(ByVal Fecha As Date, ByVal IdClienteCombo As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Tripulacion_Validar")

            If IdClienteCombo = 0 Then
                CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)
                CreaParametro(cmd, "@Todos", SqlDbType.VarChar, "S")
            Else
                CreaParametro(cmd, "@IdCliente", SqlDbType.Int, IdClienteCombo)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If
            CreaParametro(cmd, "@Fecha", SqlDbType.DateTime, Fecha)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ValidacionTripulacion_GetNombres(ByVal IdPunto As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Tripulacion_GetNombres")
            CreaParametro(cmd, "@IdPunto", SqlDbType.Int, IdPunto)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ValidacionTripulacion_GetNombresFotos(ByVal IdPunto As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_TripulacionImagen")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Int, IdPunto)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function


    Public Function fn_ValidacionTripulacion_GetCustodios(ByVal IdPunto As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Tripulacion_GetCustodios")
            CreaParametro(cmd, "@IdPunto", SqlDbType.Int, IdPunto)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_Sucursales_GetLogo(ByVal SucursalId As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_Sucursales_GetDatos")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ValidacionTripulacionAcuse_Guardar(Id_Punto As Integer, FechaPunto As Date, Operador As Integer, Cajero As Integer, Custodio1 As Integer, Custodio2 As Integer, Id_Unidad As Integer) As String
        Try
            Dim cmd As SqlCommand = CreaComando("Tv_Validartripulacion_Create")
            CreaParametro(cmd, "@Nombre_Sesion", SqlDbType.VarChar, pNombre_Usuario)
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Int, Id_Punto)
            CreaParametro(cmd, "@Fecha_Punto", SqlDbType.Date, FechaPunto)
            CreaParametro(cmd, "@Operador", SqlDbType.Int, Operador)
            CreaParametro(cmd, "@Cajero", SqlDbType.Int, Cajero)
            CreaParametro(cmd, "@Custodio1", SqlDbType.Int, Custodio1)
            CreaParametro(cmd, "@Custodio2", SqlDbType.Int, Custodio2)
            CreaParametro(cmd, "@Id_Unidad", SqlDbType.Int, Id_Unidad)
            CreaParametro(cmd, "@Usuario_Nombre", SqlDbType.VarChar, pNombre)
            CreaParametro(cmd, "@CiaTV", SqlDbType.VarChar, pCiaTV)


            Dim Identificador As String = EjecutaScalar(cmd)
            Return Identificador
        Catch ex As Exception
            TrataEx(ex)
            Return ""
        End Try
    End Function

    Public Function fn_ValidacionTripulacionAcuse_GetTripulacion(Identificador As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Tv_Validartripulacion_Get")
            CreaParametro(cmd, "@Identificador", SqlDbType.VarChar, Identificador)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Fotos [Empleado, Firma, Unidad]"
    Public Function fn_FotoFirma_Get(ByVal Id_Empleado As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_EmpleadosI_Read")
            CreaParametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
            Dim tbl As DataTable = EjecutaConsulta(cmd)
            Return tbl
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function



    Public Function fn_FotoUnidad_Get(ByVal Id_Unidad As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_UnidadesI_Read")
            CreaParametro(cmd, "@Id_Unidad", SqlDbType.Int, Id_Unidad)
            Dim tbl As DataTable = EjecutaConsulta(cmd)

            Return tbl
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    'Public Function fn_Firma_Get(ByVal Id_Empleado As Integer) As DataTable
    '    Try
    '        Dim cmd As SqlCommand = CreaComando("Cat_EmpleadosI_Read")
    '        CreaParametro(cmd, "@Id_Empleado", SqlDbType.Int, Id_Empleado)
    '        Dim tbl As DataTable = EjecutaConsulta(cmd)
    '        Return tbl
    '    Catch ex As Exception
    '        TrataEx(ex)
    '        Return Nothing
    '    End Try
    'End Function
#End Region

#Region "Reporte de Anomalias"

    Public Function fn_ReporteAnomalias_Guardar(ByVal Fecha_RIA As Date, ByVal Hora As String, ByVal Minuto As String, ByVal Descripcion As String, ByVal NotasAdicionales As String) As String
        Const IncidenciaWebClientes As Short = 5
        Const ViaWebDeClientes As Short = 2
        Const TipoCliente As Short = 1
        Const StatusIniciado As Char = "I"

        Dim tr As SqlTransaction

        Try
            tr = CreaTransaccion()
            Dim cmd As SqlCommand = CreaComando(tr, "Cat_RIA_Create")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Tipo", SqlDbType.Int, IncidenciaWebClientes)
            CreaParametro(cmd, "@Id_Entidad", SqlDbType.Int, pId_Cliente)
            CreaParametro(cmd, "@Fecha_RIA", SqlDbType.Date, Fecha_RIA)
            CreaParametro(cmd, "@Hora_RIA", SqlDbType.Time, Hora & ":" & Minuto)
            CreaParametro(cmd, "@Descripcion", SqlDbType.Text, UCase(Descripcion))
            CreaParametro(cmd, "@Notas_Adicionales", SqlDbType.Text, UCase(NotasAdicionales))
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, ViaWebDeClientes)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, StatusIniciado)
            CreaParametro(cmd, "@Usuario_RegistroN", SqlDbType.VarChar, pNombre_Usuario)

            Dim ria As Integer = EjecutaScalar(cmd)
            If ria = 0 Then
                tr.Rollback()
                Return ""
            End If

            cmd = CreaComando(tr, "Cat_RIAD_Create")
            CreaParametro(cmd, "@Id_RIA", SqlDbType.Int, ria)
            CreaParametro(cmd, "@Tipo", SqlDbType.Int, TipoCliente)
            CreaParametro(cmd, "@Id_Entidad", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Descripcion", SqlDbType.Text, UCase(Descripcion))

            EjecutaNonQuery(cmd)

            cmd = CreaComando(tr, "Cat_RiaU_Create")
            CreaParametro(cmd, "@Id_RIA", SqlDbType.Int, ria)
            CreaParametro(cmd, "@Tipo", SqlDbType.Int, TipoCliente)
            CreaParametro(cmd, "@Id_Entidad", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pEstacion_Cliente)

            EjecutaNonQuery(cmd)

            cmd = CreaComando(tr, "Cat_RIA_Read")
            CreaParametro(cmd, "@Id_Ria", SqlDbType.Int, ria)

            Dim res As DataTable = EjecutaConsulta(cmd)

            If res.Rows.Count = 0 Then
                tr.Rollback()
                Return ""
            End If

            tr.Commit()
            Return res.Rows(0)("Numero_RIA")
        Catch ex As Exception
            TrataEx(ex)
            If tr IsNot Nothing _
                AndAlso tr.Connection IsNot Nothing _
                AndAlso tr.Connection.State = ConnectionState.Open Then tr.Rollback()
            Return ""
        End Try
    End Function

    Public Function fn_Get_Rubros(Nivel As Integer) As DataTable
        Dim Dt As DataTable
        Try

            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Cli_Cat_Get_Rubros", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Nivel", SqlDbType.Int, Nivel)
            Dt = EjecutaConsulta(cmd)

        Catch ex As Exception

        End Try
        Return Dt
    End Function

#End Region

#Region "Consulta de Diferencias"
    Public Function fn_ConsultaDiferencias() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("")

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
#End Region

#Region "Rastreo de Remisiones"

    Public Function fn_RastreoRemisiones_Buscar(ByVal Numero As Long) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_Remisiones_ExisteByNumero")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Numero_Remision", SqlDbType.BigInt, Numero)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_RastreoRemisiones_Detalle(ByVal Id_Remision As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("sp_RastreoRemisiones_GetRastreo")
            CreaParametro(cmd, "@IdRemision", SqlDbType.Int, Id_Remision)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_RastreoRemisiones_RastreoImporte(ByVal Id_Remision As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("sp_RastreoRemisiones_GetRastreoImporte")
            CreaParametro(cmd, "@IdRemision", SqlDbType.Int, Id_Remision)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_RestreoRemisiones_RastreoEnvase(ByVal Id_Remision As Integer)
        Try
            Dim cmd As SqlCommand = CreaComando("sp_RastreoRemisiones_GetRastreoEnvases")
            CreaParametro(cmd, "@IdRemision", SqlDbType.Int, Id_Remision)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_RastreoRemisiones_Log(ByVal Id_Remision As Integer)
        Try
            Dim cmd As SqlCommand = CreaComando("web_Traslado_GetLog")
            CreaParametro(cmd, "@IdRemision", SqlDbType.Int, Id_Remision)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Cheques"
    Public Function fn_Cheque_Get(ByVal Id_Cheque As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Pro_FichasChequesI_Read")
            CreaParametro(cmd, "@Id_Cheque", SqlDbType.Int, Id_Cheque)
            Dim tbl As DataTable = EjecutaConsulta(cmd)
            Return tbl
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Cambiar Contraseña"

    Public Function fn_CambiarContraseña_GetPassword() As String
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("web_CambiarContraseña_GetPassword", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@IdUsuario", SqlDbType.Int, pId_Usuario)

            Return EjecutaScalar(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return String.Empty
        End Try
    End Function

    Public Function fn_CambiarContraseña_Guardar(ByVal Password As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("web_CambiarContraseña_Guardar", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@IdUsuario", SqlDbType.Int, pId_Usuario)
            'El parametro lleva False por que sino convertiria a mayusculas.
            CreaParametro(cmd, "@Password", SqlDbType.VarChar, Password, False)

            Return EjecutaNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "SUCURSALES PROPIAS"
    Public Function fn_SucursalesPropias_Get() As DataTable
        Dim Dt As DataTable
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = Crea_Comando("SucursalesPropias_Get", CommandType.StoredProcedure, cnn)
        Try
            Dt = EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Descargas"

    Public Function fn_Descargas_GetLista() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_descargas_GetLista")
            CreaParametro(cmd, "@ClaveModulo", SqlDbType.VarChar, ClaveModulo)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_Descargas_GetBytes(ByVal Id As Integer) As Byte()
        Try
            Dim cmd As SqlCommand = CreaComando("web_Descargas_GetBitArray")
            CreaParametro(cmd, "@Id", SqlDbType.Int, Id)

            Return EjecutaScalar(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "EnviaCorreo"
    Public Shared Function obtenerNotificacion(ByVal IdPunto As Decimal) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("Tv_Puntos_ObtenerNotificacion")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, IdPunto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception

        End Try
    End Function
    Public Shared Function obtenerRemisionWebImporte(ByVal NumeroRemision As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("NotificacionImportesWebTraslado")
            CreaParametro(cmd, "@NumeroRemision", SqlDbType.VarChar, NumeroRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception

        End Try
    End Function
    Public Shared Function obtenerEnvases(ByVal NumeroRemision As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("NotificacionEnvasesTraslado")
            CreaParametro(cmd, "@NumeroRemision", SqlDbType.VarChar, NumeroRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception

        End Try
    End Function
    Public Shared Function obtenerImporteMoneda(ByVal NumeroRemision As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("NotificacionMonedaTraslado")
            CreaParametro(cmd, "@NumeroRemision", SqlDbType.VarChar, NumeroRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception

        End Try
    End Function

    Public Shared Function obtenerEnvases(ByVal dtEnvases As DataTable) As String
        Dim envases As String = String.Empty
        For Each envase As DataRow In dtEnvases.Rows
            envases += "[" + envase("Numero").ToString() + "]"
        Next
    End Function

    Public Shared Function obtenerEnvaseMoneda(ByVal dtEnvases As DataTable)
        Return (From envase In dtEnvases.AsEnumerable()
                Where envase("Tipo Envase") = "BILLETE"
                Select envase).Count()
    End Function

    Public Shared Function obtenerEnvaseMixto(ByVal dtEnvases As DataTable)
        Return (From envase In dtEnvases.AsEnumerable()
                Where envase("Tipo Envase") = "MIXTO"
                Select envase).Count()
    End Function

    Public Shared Function obtenerEnvaseMorralla(ByVal dtEnvases As DataTable)
        Return (From envase In dtEnvases.AsEnumerable()
                Where envase("Tipo Envase") = "MORRALLA"
                Select envase).Count()
    End Function

    Public Shared Function obtenerMonenadaNacional(ByVal datos As DataTable) As Decimal
        Dim monedaNacional As Decimal = 0
        For Each moneda As DataRow In datos.Rows
            If moneda("Moneda").ToString() = "PESOS" Then
                monedaNacional += Convert.ToDecimal(moneda("Efectivo"))
            End If
        Next
        Return monedaNacional
    End Function

    Public Shared Function obtenerMonenadaExtranjera(ByVal datos As DataTable) As Decimal
        Dim monedaExt As Decimal = 0
        For Each moneda As DataRow In datos.Rows
            If moneda("Moneda").ToString() <> "PESOS" Then
                monedaExt += Convert.ToDecimal(moneda("Efectivo")) * Convert.ToDecimal(moneda("Tipo Cambio"))
            End If
        Next
        Return monedaExt
    End Function

    Public Shared Function obtenerDocumentos(ByVal datos As DataTable) As Decimal
        Dim doc As Decimal = 0
        For Each moneda As DataRow In datos.Rows
            doc += Convert.ToDecimal(moneda("Documentos"))
        Next
        Return doc
    End Function

    Public Shared Function crearPDF(NumeroRemision As String, Fecha As String, Hora As String, CantidadEnvases As String,
                             NumeroEnvase As String, ImporteTotal As String,
                             Importe_le As String, NombreOrigen As String, ClaveClienteOrigen As String,
                             DireccionOrigen As String, NombreDestino As String,
                             DireccionDestino As String, Clave_Ruta As String, NombreTraslado As String,
                             Descripcion As String, NombreEmpleado As String, NombreFirma As String,
                             Mon_Na As String, Mon_Ex As String, Mon_Otros As String,
                             Env_B As String, Env_M As String, Env_MIX As String, Mil As String,
                             Quinientos As String, Doscientos As String, Cien As String,
                             Cincuenta As String, Veinte As String, VeinteM As String, Diez As String, Cinco As String,
                             Dos As String, Uno As String, PCincuenta As String, PVeinte As String,
                             PDiez As String, PCinco As String, Notas As String) As MemoryStream

        Dim ms As MemoryStream = New MemoryStream()
        Dim doc As New Document(PageSize.A4, 15.0F, 15.0F, 40.0F, 30.0F)

        pdfw = PdfWriter.GetInstance(doc, ms)
        Dim fontN As New Font(FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL))
        Dim fontB As New Font(FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.BOLD))
        Dim fontB12 As New Font(FontFactory.GetFont(FontFactory.HELVETICA, 12, iTextSharp.text.Font.BOLD))
        Dim cvacio As PdfPCell = New PdfPCell(New Phrase())
        cvacio.Border = 0
        doc.Open()
        Dim tabla As PdfPTable = New PdfPTable(4)
        Dim col1 As PdfPCell
        Dim col2 As PdfPCell
        Dim col3 As PdfPCell
        Dim col4 As PdfPCell

        ''Encabezado
        tabla.WidthPercentage = 95
        Dim ancho As Single() = New Single() {4.5F, 9.0F, 1.0F, 4.0F}
        tabla.SetWidths(ancho)
        Dim img As Image
        img = Image.GetInstance(Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAEYAAABGCAYAAABxLuKEAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAOxAAADsQBlSsOGwAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAw6SURBVHic7Zx7cFzVfcc/v7sryY4sO04l7X1hFB62ZQWH2kAy1MF2SNOOsUJCa5MZGg/NFPFwjRxoBwKBEt6mZGqgtsGdpINLOhSHkCJK0saOeRWwwU3oRDZGKRFY+5AtkLAelqW999c/9Npd7a6E9sqWB39n9o977jm/3/d+9+zvnHvO76wwBpqeXzez3+u/E+SrAlXA9LHaTGEkVOQl6fdvqv7Go835KhpjWUp6yacEqReo5uQWBcAU1dWEjZ//fteV0/JVDOe7ua/h6rOBPwmUGvgC/6vQhNI+7lYiFaCLgNMLp6Dzj3VP/yrwbK4aeYURkTPQwmkM04H/M3wumX/plgMTaq/I289dezNwb8FcfM4ljzD5f0o+RYUSSHcmN09UFAARdP7eyAagMQA6JflujhljgkS/x+5Cbcgdd/jAngDo5MXxFEY/d+nmlkAMQVsQdvLhuPYYkSAj1uTiuApzMuGUMDlwSpgcOCVMDuSd4J0o7Gu4+mwD4zYFK0eVswt2Inx7f8O1o2b1Aj2q+sCUFGZB7WNNbzbUXfMpCa0T5RZg5iS4MQc/qXhOk7qu+huPNk9JYQDOq93aA2w40FD3Q5/wbaBrgdBk+FLYL8j66trN/zVUNuVjzLzarW3VtZvrQyLnANsDNt8uyPrqo23npIoCUzTGZMPclZv3A6sbG9ZeHMJ/UOHcAsz5Aj8OhcI3nr3ikcPZKkz5HpOJmtpNO+ftjSwWYTXw3gRMvCA+fzi/dsuaXKLASdRjUjH4Irn9zYa6/xh/gNYWxbh1Qe3mbePxcdL1mFScV7u1Z8HKLRt8Q+ahbAWSWar1qOjtM45OnzteUQAk3823G66rV3TjxyWcC2oYVQsu2TSR7j8uHGiom+8TuhNYNVj0HIPD78e1lVOYXbvuCJtdra8DiydGM4szZcP8r225OSh7udDYsPZiMehfcMmmlyZqI6swTc/XnZn0QhuAP5swu+zwBbnHO8oDNas3dwVsO1DI/oZrP8wom8bk7wYkgbeBY4UaUuSpBbWbHyicUjrCwN6gjR5PCH7PieZwCqcwxnA9UTiWdS/o5ZNh+zihb7JmvodB3i3UiKAlg+9EZQFwygUfiAF96b6nPkKOaa5H2EDAyw6i/GNvMnlLW1tb5yinhRh2THPVrLKy6iNdXfsLsTMGtLOr67WZZaXFIBcFZ1Z+GU0krujp6ck6ZZjQu5LrVi50LHMXwlOK/sSxIq84kcgXCyOaHyGfHwVpT8V/EnLvc30sYebMmTPbtqyH1DP2AstG7sgfYchrjm02nFZZeeYEueZFWXl5ILuYI9CD+e6OVxjDMc01Xn/fAUGvJ9dyhbLSDxv7HSvymGmaFR+T6ZTCmMK4prnMMc1fIzwOjP2wShFIXUg44NrmTVVVVXkTdKYqcgrjOI7rWOY2FXYhLJyA7dmq3N9/rPeAY5prODlGwGGMEsZ13emubd6E7+0HvhWAjzkIjzuW+bplWV8KwN5xQZowth2pVS+5T5X7gRkB+7rAQF9ybLPBdSvOCth24DAAHMc517bMF0XlWQYyMycPykr1Q/scK/JYJBKpnFRfBSDsRCJfxvOuEoNWMvdtlM8DcwtxILBPJUtqmMjssKF3uW7F37e0HP5dIT4mA+Foa+uvgF9lu+nY5oMoNxbiQIWno7HE7YXYOBE4qXcJJhMn5b5SkFgMRYdM81xfNSJFRb9uaWmJwidcGMuyqhOi/4aqhyFR9ZLnO5b1w2g8fssnVhjD17Avul093YqILYinqk8jep1tm02TH2NUA5nxdnd3B8pVjNAZAn2hYu8nItwIHBbhEUW+Jz5XHIfgK+VBWOnt7T0jCDtD8PBLEToGLwX064I8GVJtV2HW8RiVlhDA6Bc29MIAuAwjBO+g1JAsOhPhDYzQX6hojS9SD7IjP2HVowFw+JxjRb6/mImfS3BNc5mq3BMAl2EkNfSBil7vo9tR3ame9zVVOgx0nuf7d+f9/buWdZmiTwfEpV3gtyokxt1CqVSYJ6Nz5QqF3+/59qFDh1oty6oOiV6uSkSFPVYs8cRe6M8rzFlnnVXS0921V6AmYGInFsLj0VjiyvxVxkBVRYXZHza+D3Ih4ATF7QSgU6EZ5Tkrkdi4F/pPNKFTOIVT+OQiW/ANnV5eXtlfXDxTVQ3DMA5Fo9EPgnIYiURKgcqiIi3z/XB7cXHx4ebm5t7xtK2pqSnu6OioDHneLD/s9fX2aiLb9moQGBbmNMu6yFf9GxGW6ujU0GMgLwv8oCUe/0XqDccytwDXpJaF+5P2e21t8aHr8vLyspKionpFvymwgNFfyEFgu4pxXywWyzzWJ7YdWY3KdQIXMJDxlYpOhF8goXuj0ehvcj1oVVXVp/uP9R5k1Fq23h2Nt96WWd8AsE1zrY++gFCbRRSAEtCvKPpz14rU53KeDa7rfqYkHH4F9K7B+VC2XnoacIOh/p7MdWDbijwmKk8KXMRoUQDKUFbhe6/bduUf5+LR19dbR9YFfrnGtu1PZZYap5eXWyL8II2wcJcoX1DRr4O8ldpAkfts2x7/i6Gf/G76vpQ0ibICw78Q5XYG0jAGbfPZkGHcOnTtmOZyQa5KfT5B6gzkfJA1StosukQ0tIkswi+GIlHW5WBYLqprMgvDflHRQtD0M8iiL7YkWvcAmKbZFBa2qXAE6Man2/A9i3GeZFXl/Iyi93r6+v77ww8/PAK85trmTIUlqHSBdgj+cMwQgws0fdu9m1By18GBxfM3XcvqAr1VkXaBbh+6q6qqZjU3N3ekNmq1rNWgbkrRfgb+kmGI5XpgKylfUhjP+x0hQ0lV2pcdjmnGVHS3IewWlRtaYvFXyZ55nV8Y9B1BlqaUfGX6tOI2xzJ/I8puVdmjIg/H4rHRm+wq72QkJMxWL9TkWFYT+LsRf49PqC4Wi/3PGCy+k3plIN9RdIvCZweL5tl25JJYrLVhqI4AOLZ5J8qoAJSBNtAnVEL3pAbIsYKv4zguvvcy+ferFGGPovfFYq3/nvoMrmX9VNFLx+DWLMKj4eJpD2WOcK5pLlXhhRRPsWgiMcexzb/LeOYXovHE8mHHANFY4nZRlqHsJKU7ZaAcZL2o/6Zt23PGIDqMaDTacvRY3+dBbgMO5agmKF8QlZ85lpW6vOC3xOOXqeg3M2NdBqoG9smP7sgMpAo3pDky+FfAE8P7F9K74zLbtheNEMqA67qfwfOW+qrLRVjCQA5cej3ln6OJxLdhfMN1Ki/HqVyIH1ou+EsVWQJkBnIfI1QdjUbfyWzsOI6L530ZWIbIEtBRZyNFtL4l1vrwYP25g3vww+tOoiw3iovfAvD6+3YAi0Za6xPReOu3AMKuHfkrVU4HZghGmXpeYzQe/wfgGQA3EjlHDdlJegrIuM4XDBBL/rmqzBRhlqJlniffSyTiG4GNNTU1xR+1f3C3Kn+b0sxQTS4G3nVt80ZVmaWqM8SgTHz/2ZZEYhuwDcA1zRUqPEtKypyqnDfynF49GauHKuzy+tPyEEcgcrnjON+NRqMtYVX5UwbPDOhAz2o3TfOZRCLRDNDS2rrPsSJRkBFhDN4fjzCqKoLcI4P9TRDCBh3AOkAbGxv7HNN8EUkTBgi9DyQVrgI9UwRQUNVF5eXlO4Zmuz19fa9MLyk+Str8RN8HcBznD/C9K8fDc6QpRep7fw3cHPaRhwz0MkZ+LrPDwluObT4DtKF8iYxjdorxyHj8xGKxA45lPg+sGG6rrHUs83zQVwWjDHRVRiLcG7FY7NUBP2wUGPElLCwpCjc6VuR5MHoRXYmmTdqOqoS2AuAnrwZJjTcewk+z0FyEMpweJ0JdRUXFwNKma0XqFXmQsTfg+lS5IZZIbBoqGCvGRCKRyrDBf4KM4wyjvGWE+1ccPNgWGywwbCvyaMYkL0dTPlL8VbHYoV/W1NQUd7R/0IymntuWHdF4fNTM2LWsKxR9Is2U6PUhgCNd3btnlZb+DKEXEYOBP50JMzBCtSH8VpUfe6p/mWhtTTttOrOsdKEgnxaID328cHhbZ2dnD0B3d3f33K7uH3XOnPF7UF9UihA1QKYJHEFoQdgpyP3RePz6I0d6Pkoxr51d3Q0zS0tfFjGSDPTqEgYW1o+BHkLlDTH0n8QoWhONxhsBig1jqYh8MZUTysOdXV37MoWZXlr6rmHIxQKtQ3VVpOz/AZN/nzGefICJAAAAAElFTkSuQmCC"))

        'img = Image.GetInstance("\\MVIRTUAL-PC\Users\MVirtual\Desktop\IVAN\PROYECTO REMISION DIGITAL\TV MOVIL 25-02-2020 Materiales\WsTv2012_33\Imagenes\Logosissa.png")
        'img = Image.GetInstance("C:\Users\LOGO\Logosissa.png")
        img.ScaleToFit(80.0F, 90.0F)
        img.SpacingBefore = 20.0F
        img.SpacingAfter = 10.0F
        img.SetAbsolutePosition(40.0F, 720.0F)
        doc.Add(img)
        tabla.AddCell(cvacio)
        col2 = New PdfPCell(New Phrase("Servicio Integral de Seguridad. S.A. de C.V.", fontB12))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)
        tabla.AddCell(cvacio)

        tabla.AddCell(cvacio)
        col2 = New PdfPCell(New Phrase("ALVAREZ NTE. 209 MONTERREY, N.L. C.P. 64000" + vbCrLf + "CONMUTADOR: 8047-4545, 8047-4546 FAX 8047 4550" + vbCrLf + "www.sissaseguridad.com", fontN))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)
        col3 = New PdfPCell(New Phrase("REMISION:" + vbCrLf + NumeroRemision + vbCrLf, fontB))
        col3.Border = 0
        tabla.AddCell(col3)

        tabla.AddCell(cvacio)
        col2 = New PdfPCell(iTextSharp.text.Image.GetInstance(Codigo_barras(NumeroRemision)))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)
        col3 = New PdfPCell(New Phrase("Ruta:" + vbCrLf + Clave_Ruta + vbCrLf + "Unidad:" + vbCr + Descripcion, fontN))
        col3.Border = 0
        tabla.AddCell(col3)
        doc.Add(tabla)
        ''Cuerpo1
        Dim tabla2 As PdfPTable = New PdfPTable(12)
        tabla2.WidthPercentage = 95
        'Dim ancho2 = New Single() {2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F}
        Dim ancho2 = New Single() {2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F, 2.0F}
        tabla2.SetWidths(ancho2)
        doc.Add(New Paragraph(" "))

        col1 = New PdfPCell(New Phrase("Billetes", fontB))
        col1.Colspan = 6
        col1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla2.AddCell(col1)

        col2 = New PdfPCell(New Phrase("Monedas", fontB))
        col2.Colspan = 6
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla2.AddCell(col2)

        tabla2.AddCell("1000")
        tabla2.AddCell("500")
        tabla2.AddCell("200")
        tabla2.AddCell("100")
        tabla2.AddCell("50")
        tabla2.AddCell("20")
        tabla2.AddCell("20")
        tabla2.AddCell("10")
        tabla2.AddCell("5")
        tabla2.AddCell("2")
        tabla2.AddCell("1")
        tabla2.AddCell(".50")
        'tabla2.AddCell(".20")
        'tabla2.AddCell(".10")
        'tabla2.AddCell(".05")

        col1 = New PdfPCell(New Phrase(FormatNumber(Mil, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Quinientos, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Doscientos, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Cien, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Cincuenta, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Veinte, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(VeinteM, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Diez, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Cinco, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Dos, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(Uno, 2), fontB))
        tabla2.AddCell(col1)

        col1 = New PdfPCell(New Phrase(FormatNumber(PCincuenta, 2), fontB))
        tabla2.AddCell(col1)

        'tabla2.AddCell(FormatNumber(Quinientos, 2))
        'tabla2.AddCell(FormatNumber(Doscientos, 2))
        'tabla2.AddCell(FormatNumber(Cien, 2))
        'tabla2.AddCell(FormatNumber(Cincuenta, 2))
        'tabla2.AddCell(FormatNumber(Veinte, 2))
        'tabla2.AddCell(FormatNumber(VeinteM, 2))
        'tabla2.AddCell(FormatNumber(Diez, 2))
        'tabla2.AddCell(FormatNumber(Cinco, 2))
        'tabla2.AddCell(FormatNumber(Dos, 2))
        'tabla2.AddCell(FormatNumber(Uno, 2))
        'tabla2.AddCell(FormatNumber(PCincuenta, 2))
        'tabla2.AddCell(PVeinte)
        'tabla2.AddCell(PDiez)
        'tabla2.AddCell(PCinco)
        doc.Add(tabla2)
        '
        'Cuerpo1
        doc.Add(New Paragraph(" "))
        tabla = New PdfPTable(4)
        tabla.WidthPercentage = 95
        ancho = New Single() {4.0F, 4.0F, 4.0F, 4.0F}
        tabla.SetWidths(ancho)

        col1 = New PdfPCell(New Phrase("VALORES RECIBIDOS DE :", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(NombreOrigen, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col3 = New PdfPCell(New Phrase("MONEDA NACIONAL:", fontB))
        col3.Border = 0
        tabla.AddCell(col3)
        col4 = New PdfPCell(New Phrase(FormatNumber(Mon_Na, 2), fontN))
        col4.Border = 0
        tabla.AddCell(col4)

        col1 = New PdfPCell(New Phrase("NUM. CLIENTE:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)

        col2 = New PdfPCell(New Phrase(ClaveClienteOrigen, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col3 = New PdfPCell(New Phrase("MONEDA EXTRANJERA:", fontB))
        col3.Border = 0
        tabla.AddCell(col3)
        col4 = New PdfPCell(New Phrase(FormatNumber(Mon_Ex, ), fontN))
        col4.Border = 0
        tabla.AddCell(col4)

        col1 = New PdfPCell(New Phrase("FECHA:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(Fecha, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col3 = New PdfPCell(New Phrase("OTROS:", fontB))
        col3.Border = 0
        tabla.AddCell(col3)
        col4 = New PdfPCell(New Phrase(FormatNumber(Mon_Otros, 2), fontN))
        col4.Border = 0
        tabla.AddCell(col4)

        col1 = New PdfPCell(New Phrase("DIRECCION:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(DireccionOrigen, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col3 = New PdfPCell(New Phrase("TOTAL:", fontB))
        col3.Border = 0
        col3.BackgroundColor = BaseColor.LIGHT_GRAY
        tabla.AddCell(col3)
        col4 = New PdfPCell(New Phrase(FormatNumber(ImporteTotal, 2), fontN))
        col4.Border = 0
        col4.BackgroundColor = BaseColor.LIGHT_GRAY 'iTextSharp.text.pdf.ExtendedColor.LIGHT_GRAY ' BaseColor.LIGHT_GRAY
        tabla.AddCell(col4)
        doc.Add(tabla)

        'Cuerpo2
        doc.Add(New Paragraph(" "))
        tabla = New PdfPTable(8)
        tabla.WidthPercentage = 95
        ancho = New Single() {4.0F, 2.0F, 4.0F, 2.0F, 4.0F, 2.0F, 4.0F, 2.0F}
        tabla.SetWidths(ancho)

        col1 = New PdfPCell(New Phrase("ENVASES CON BILLETES:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(Env_B, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col1 = New PdfPCell(New Phrase("ENVASES CON MORRALLA:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(Env_M, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col1 = New PdfPCell(New Phrase("ENVASES MIXTOS:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(Env_MIX, fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        col1 = New PdfPCell(New Phrase("ENVASES TOTALES:", fontB))
        col1.Border = 0
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(CantidadEnvases, fontN))
        col2.Border = 0
        tabla.AddCell(col2)

        col1 = New PdfPCell(New Phrase("IMPORTE EN LETRAS:", fontB))
        col1.Border = 0
        col1.Colspan = 2
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(Importe_le, fontN))
        col2.Border = 0
        col2.Colspan = 6
        tabla.AddCell(col2)

        col1 = New PdfPCell(New Phrase("ENTREGAR ENVASES EN:", fontB))
        col1.Border = 0
        col1.Colspan = 2
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(NombreDestino + "(" + DireccionDestino + ")", fontN))
        col2.Border = 0
        col2.Colspan = 6
        tabla.AddCell(col2)

        'col1 = New PdfPCell(New Phrase("DIRECCIÓN:", fontB))
        'col1.Border = 0
        'col1.Colspan = 2
        'tabla.AddCell(col1)
        'col2 = New PdfPCell(New Phrase(DireccionDestino, fontN))
        'col2.Border = 0
        'col2.Colspan = 6
        'tabla.AddCell(col2)

        col1 = New PdfPCell(New Phrase("SELLOS:", fontB))
        col1.Border = 0
        col1.Colspan = 2
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(NumeroEnvase, fontN))
        col2.Border = 0
        col2.Colspan = 6
        tabla.AddCell(col2)

        col1 = New PdfPCell(New Phrase("NOTAS:", fontB))
        col1.Border = 0
        col1.Colspan = 2
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(Notas, fontN))
        col2.Border = 0
        col2.Colspan = 6

        tabla.AddCell(col2)
        doc.Add(tabla)
        doc.Add(New Paragraph("         ___________________________________________________________________________"))
        'Cuerpo firmas
        doc.Add(New Paragraph(" "))
        'doc.Add(New Paragraph(" "))
        'doc.Add(New Paragraph(" "))
        tabla = New PdfPTable(4)
        tabla.WidthPercentage = 95
        ancho = New Single() {4.0F, 3.0F, 3.0F, 4.0F}
        tabla.SetWidths(ancho)
        tabla.AddCell(cvacio)
        col1 = New PdfPCell(New Phrase("FIRMA DE REMITENTE:", fontB))
        col1.Border = 0
        col1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase("TRANSPORTACION DE VALORES", fontB))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)

        tabla.AddCell(cvacio)
        col1 = New PdfPCell(iTextSharp.text.Image.GetInstance(Qr(NombreFirma)))
        col1.Border = 0
        col1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col1)
        col2 = New PdfPCell(iTextSharp.text.Image.GetInstance(Qr(NombreEmpleado)))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)

        tabla.AddCell(cvacio)
        col1 = New PdfPCell(New Phrase("FIRMA DE CONSIGNATORIO:", fontB))
        col1.Border = 0
        col1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase("FECHA Y HORA DE SERVICIO:", fontB))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)

        tabla.AddCell(cvacio)
        col1 = New PdfPCell(iTextSharp.text.Image.GetInstance(Qr(NombreTraslado)))
        col1.Border = 0
        col1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase(vbCrLf + " " + vbCrLf + Fecha + vbCrLf + Hora, fontN))
        col2.Border = 0
        col2.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        tabla.AddCell(col2)
        tabla.AddCell(cvacio)
        doc.Add(tabla)
        'Cuerpo fin
        doc.Add(New Paragraph(" "))
        'doc.Add(New Paragraph(" "))
        'doc.Add(New Paragraph(" "))
        tabla = New PdfPTable(1)
        tabla.WidthPercentage = 95
        ancho = New Single() {5.0F}
        tabla.SetWidths(ancho)
        doc.Add(New Paragraph("         ___________________________________________________________________________"))
        doc.Add(New Paragraph(" "))
        col1 = New PdfPCell(New Phrase("IMPORTANTE", fontB))
        col1.Border = 0
        col1.HorizontalAlignment = PdfPCell.ALIGN_CENTER
        'col1.HorizontalAlignment = HorizontalAlignment.Center
        tabla.AddCell(col1)
        col2 = New PdfPCell(New Phrase("° CUALQUIER ALTERACIÓN HACE NULO ESTE DOCUMENTO." + vbCrLf +
        "° LA COMPAÑIA  NO SERA RESPONSABLE POR INCUMPLIMIENTO DE ESTE SERVICIO EN CASOS FORTUITOS O FUERZA MAYOR." + vbCrLf +
        "° LA COMPAÑIA  NO ATENDERA RECLAMACION ALGUNA DESPUES DE 60 DIAS DE LA FECHA DE ESTE UNICO DOCUMENTO." + vbCrLf +
        "° NO ENTREGUE SUS VALORES SI EXISTE DUDA SOBRE LA IDENTIDAD DEL PERSONAL." + vbCrLf +
        "° PARA EFECTOS DE FACTURACION, CADA REMISION REPRESENTA UN SERVICIO.", fontN))
        col2.Border = 0
        tabla.AddCell(col2)
        doc.Add(tabla)

        doc.Close()


        Dim bytes() As Byte = ms.ToArray()
        ms = New MemoryStream()
        ms.Write(bytes, 0, bytes.Length)
        ms.Position = 0
        Return ms
    End Function
    Public Shared Function Qr(texto As String) As Image
        Dim Qqr As BarcodeQRCode = New BarcodeQRCode(texto, 1000, 1000, Nothing)
        'Dim datam As BarcodeQRCode = New BarcodeQRCode(cadena, dine, dine, Nothing)

        'Dim QqR As QRCoder.QRCodeGenerator = New QRCoder.QRCodeGenerator()
        'Dim ASSCII As ASCIIEncoding = New ASCIIEncoding()
        'Dim z = QqR.CreateQrCode(ASSCII.GetBytes(texto), QRCoder.QRCodeGenerator.ECCLevel.H)
        'Dim png As QRCoder.PngByteQRCode = New QRCoder.PngByteQRCode()
        'png.SetQRCodeData(z)
        'Dim arr = png.GetGraphic(10)
        'Dim ms As MemoryStream = New MemoryStream()
        'ms.Write(arr, 0, arr.Length)
        'Return System.Drawing.Image.FromStream(ms)
        'Dim img As System.Drawing.Bitmap = New System.Drawing.Bitmap(ms)
        'Dim codeBitmap = New System.Drawing.Bitmap(img)
        'Dim image As Image = CType(codeBitmap, Image)
        'pictureBox1.Image = b





        Dim img As Image = Qqr.GetImage()
        img.ScaleAbsolute(60, 60)
        Return img
    End Function
    Public Shared Function Codigo_barras(NumeroRemision) As Byte()
        Dim code As Barcode128 = New Barcode128()
        code.CodeType = Barcode128.CODE128
        code.Code = NumeroRemision
        code.AltText = NumeroRemision
        code.TextAlignment = Barcode128.UPCA
        Dim bitm As System.Drawing.Bitmap = New System.Drawing.Bitmap(code.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White))
        Using memory As MemoryStream = New MemoryStream()

            bitm.Save(memory, System.Drawing.Imaging.ImageFormat.Jpeg)
            Return memory.ToArray()
        End Using
    End Function

#End Region

#Region "22/enero/2015 Parametros Cajas Bancarias"
    '
    Public Function fn_CajasBancarias_ObtenerParametros(ByVal IdcajaBancaria As Integer) As DataTable
        Dim dt_ParamCajasBancarias As New DataTable
        Try

            Dim Cmd As SqlClient.SqlCommand = CreaComando("Pro_CajasBancarias_Read")
            CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, IdcajaBancaria)
            dt_ParamCajasBancarias = EjecutaConsulta(Cmd)
            Return dt_ParamCajasBancarias
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
#End Region

#Region "Consulta de Anomalias"
    Public Function fn_ConsultaAnomalias_GetAnomalias(ByVal FechaInicial As Date, ByVal FechaFinal As Date, ByVal Status As Char) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaAnomalias_GetAnomalias")
            CreaParametro(cmd, "@FechaInicial", SqlDbType.Date, FechaInicial)
            CreaParametro(cmd, "@FechaFinal", SqlDbType.Date, FechaFinal)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaAnomalias_GetDetalle(ByVal Id_RIA As Integer) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("Cat_RIAD_Reporte")
            CreaParametro(cmd, "@Id_RIA", SqlDbType.Int, Id_RIA)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

#End Region

#Region "Consulta de Remantes 20/enero/2015"

    Public Function fn_DotacionCajero_Read(ByVal Id_Cajero As Integer) As DataTable
        Dim Dt As New DataTable
        Try
            Dim Cmd As SqlClient.SqlCommand = CreaComando("Caj_Cajeros_Read")
            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, Id_Cajero)
            Dt = EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_RemanentesConsultar(ByVal Desde As Date, ByVal Hasta As Date, ByVal Id_CajaBancaria As Integer, ByVal Status As String) As DataTable
        Dim lc_tabla As New DataTable
        Try

            Dim Cmd As SqlClient.SqlCommand = cn_Datos.CreaComando("web_ConsultaServicios_GetByCajero")
            CreaParametro(Cmd, "@FDesde", SqlDbType.DateTime, Desde)
            CreaParametro(Cmd, "@FHasta", SqlDbType.DateTime, Hasta)
            CreaParametro(Cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
            CreaParametro(Cmd, "@Status", SqlDbType.VarChar, Status)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_RemanentesConsultar_RemisionesD(ByVal Id_Remision As Integer) As DataTable
        Dim cmd As SqlCommand = CreaComando("web_RemisionesD_GetId")
        CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, Id_Remision)
        Try

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_RemanentesConsultar_ServiciosD(ByVal Id_Servicio As Integer) As DataTable
        Dim cmd As SqlCommand = CreaComando("web_ServiciosD_Get")
        CreaParametro(cmd, "@Id_Servicio", SqlDbType.Int, Id_Servicio)

        Try
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Cajeros modificado 13/enero/2015"

    Public Function fn_CatalogoCajeroLlenalista(ByVal Id_CajaBancaria As Integer, ByVal Status As Char) As DataTable

        Try
            Dim Cnn As SqlClient.SqlConnection = cn_Datos.CreaConexion
            Dim Cmd As SqlClient.SqlCommand = cn_Datos.CreaComando("web_Cajeros_Get")

            cn_Datos.CreaParametro(Cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            cn_Datos.CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
            cn_Datos.CreaParametro(Cmd, "@Status", SqlDbType.VarChar, Status)
            Return EjecutaConsulta(Cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_CatalogoConfigLlenalista(ByVal Idcajero As Integer) As DataTable
        Try
            Dim Cnn As SqlClient.SqlConnection = cn_Datos.CreaConexion
            Dim Cmd As SqlClient.SqlCommand = cn_Datos.CreaComando("Caj_Configuracion_Get")
            cn_Datos.CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, Idcajero)
            Return EjecutaConsulta(Cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_LlenarCajasBancarias() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Pro_CajasBancarias_Get")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region " LLENAR COMBO CAJEROS Y GRIDVIEW-- CAPTURA DOTACIONES "
    '22JULIO 2013
    Public Function fn_CapturaDotaciones_GetCajeros(ByVal Id_CajaBancaria As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("caj_CajerosCajaBan_ComboGet")
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    '22 JULIO 2013
    Public Function fn_DotacionConfigConsulta(ByVal Idcajero As Integer, ByVal IdMoneda As Integer) As DataTable
        Dim lc_tabla As New DataTable
        Try

            Dim Cmd As SqlCommand = CreaComando("caj_ConfigDotacion_Get")

            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, Idcajero)
            CreaParametro(Cmd, "@Id_Moneda", SqlDbType.Int, IdMoneda)

            lc_tabla = EjecutaConsulta(Cmd)

            lc_tabla.Columns(2).ReadOnly = False
            lc_tabla.Columns(3).ReadOnly = False
            lc_tabla.Columns(4).ReadOnly = False

            Return lc_tabla
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ObtenerValoresCajeros(ByVal Idcajero As Integer) As DataTable
        Dim dt_cajeros As New DataTable
        Try

            Dim Cmd As SqlClient.SqlCommand = CreaComando("Caj_Cajeros_Read")
            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, Idcajero)
            dt_cajeros = EjecutaConsulta(Cmd)
            Return dt_cajeros
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

#End Region


#Region "Solicitud de Dotaciones-Proceso"

    Public Function fn_SolicitudDotaciones_GetCajasBancarias() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_SolicitudDotaciones_GetCajasBancarias")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_SolicitudDotaciones_GetMonedas() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Monedas_Get")

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_SolicitudDotaciones_GetDenominaciones(ByVal Id_Moneda As Integer, ByVal Presentacion As Char) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_DenominacionesP_Get")
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
            CreaParametro(cmd, "@Presentacion", SqlDbType.VarChar, Presentacion)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_SolicitudDotaciones_Guardar(ByVal Id_Cliente As Integer, ByVal IdCajaBancaria As Integer, ByVal Id_ClienteP As Integer,
                                                   ByVal Importe As Single, ByVal Fecha As Date, ByVal Id_Moneda As Integer, ByVal Monedas As DataTable,
                                                   ByVal Billetes As DataTable, ByVal Comentarios As String, Nombre_Moneda As String, Cliente As String, Hora_Entrega As String) As Boolean
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Try
            Dim cmd As SqlCommand = CreaComando(tr, "Cli_Dotaciones_Create")

            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Session("Clave_SucursalPropia"))
            CreaParametro(cmd, "@Clave_Sucursal", SqlDbType.VarChar, Session("ClaveSucursal"))
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, Session("Id_Sucursal"))
            CreaParametro(cmd, "@IdCajaBancaria", SqlDbType.Int, IdCajaBancaria)
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, Id_Cliente)
            CreaParametro(cmd, "@IdClienteP", SqlDbType.Int, Id_ClienteP)
            CreaParametro(cmd, "@Cliente", SqlDbType.VarChar, Cliente)
            CreaParametro(cmd, "@IdMoneda", SqlDbType.Int, Id_Moneda)
            CreaParametro(cmd, "@Nombre_Moneda", SqlDbType.VarChar, Nombre_Moneda)
            CreaParametro(cmd, "@Importe", SqlDbType.Money, Importe)
            CreaParametro(cmd, "@Fecha_Entrega", SqlDbType.DateTime, Fecha)
            CreaParametro(cmd, "@Hora_Entrega", SqlDbType.VarChar, Hora_Entrega)
            CreaParametro(cmd, "@Estacion_Captura", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Usuario_Captura", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")

            Dim IdDotacion As Integer = EjecutaScalar(cmd)

            For Each r As DataRow In Billetes.Rows
                If r("Cantidad") > 0 Then
                    cmd = CreaComando(tr, "Cli_DotacionesD_Create")
                    CreaParametro(cmd, "@Id_DotacionCli", SqlDbType.Int, IdDotacion)
                    CreaParametro(cmd, "@Id_Denominacion", SqlDbType.Int, r("Id_Denominacion"))
                    CreaParametro(cmd, "@Cantidad", SqlDbType.Int, r("Cantidad"))
                    CreaParametro(cmd, "@Denominacion", SqlDbType.Decimal, r("Denominacion"))
                    CreaParametro(cmd, "@Presentacion", SqlDbType.VarChar, "B")
                    EjecutaNonQuery(cmd)
                End If
            Next

            For Each r As DataRow In Monedas.Rows
                If r("Cantidad") > 0 Then
                    cmd = CreaComando(tr, "Cli_DotacionesD_Create")
                    CreaParametro(cmd, "@Id_DotacionCli", SqlDbType.Int, IdDotacion)
                    CreaParametro(cmd, "@Id_Denominacion", SqlDbType.Int, r("Id_Denominacion"))
                    CreaParametro(cmd, "@Cantidad", SqlDbType.Int, r("Cantidad"))
                    CreaParametro(cmd, "@Denominacion", SqlDbType.Decimal, r("Denominacion"))
                    CreaParametro(cmd, "@Presentacion", SqlDbType.VarChar, "M")
                    EjecutaNonQuery(cmd)
                End If
            Next

            tr.Commit()
            Return True
        Catch ex As Exception
            tr.Rollback()
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Function fn_SolicitudDotaciones_GetClientes(ByVal IdCliente As Integer, ByVal IdCajaBancaria As Integer) As DataTable
        Try
            '25Agosto2016
            Dim cmd As SqlCommand = CreaComando("web_SolicitudDotaciones_GetClientes")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, IdCliente)
            CreaParametro(cmd, "@IdCajaBancaria", SqlDbType.Int, IdCajaBancaria)
            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Reporte Metrorrey 06/06/2016"
    Public Function fn_GruposDeposito_Consultar(ByVal CajaBancaria As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_GrupoDeposito_Get0")
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, CajaBancaria)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_CargarServiciosMetrorrey(ByVal Id_Caja As Integer, ByVal S_Desde As Integer, ByVal S_Hasta As Integer, ByVal Moneda As Integer, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean, ByVal GrupoDeposito As Integer) As DataTable
        Try

            Dim cmd As SqlCommand = CreaComando("web_Pro_Fichas_Get")
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_Caja)

            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
                End If

            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If
            CreaParametro(cmd, "@S_Desde", SqlDbType.Int, S_Desde)
            CreaParametro(cmd, "@S_Hasta", SqlDbType.Int, S_Hasta)
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Moneda)
            CreaParametro(cmd, "@Id_GrupoDepo", SqlDbType.Int, GrupoDeposito) 'grupo depsoiito
            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region


#Region "Reporte Fichas de Morralla 11/06/2016"

    Public Function fn_ClientesGruposD_Consultar(ByVal Id_Padre As Integer) As DataTable
        Try
            'Nuevo proc clientes grupos
            Dim cmd As SqlCommand = CreaComando("web_Cat_ClientesGruposD_Get0")
            CreaParametro(cmd, "@Id_Padre", SqlDbType.Int, Id_Padre)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_MorFichasC_Consultar(ByVal Fecha_OperacionI As Date, ByVal Fecha_OperacionF As Date) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Mor_FichasC_Get0")
            CreaParametro(cmd, "@Fecha_OperacionI", SqlDbType.Date, Fecha_OperacionI)
            CreaParametro(cmd, "@Fecha_OperacionF", SqlDbType.Date, Fecha_OperacionF)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_CargarFichasdeMorralla(ByVal Fecha_OperacionI As Date, ByVal Fecha_OperacionF As Date, ByVal Id_Cierre As Integer, ByVal Id_CajaBancaria As Integer, ByVal ClienteGrupo As Integer, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        Try

            'verificar procedure d econsulta fichas morralla
            Dim cmd As SqlCommand = CreaComando(" ")

            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Cliente)
                End If

            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            CreaParametro(cmd, "@Fecha_OperacionI", SqlDbType.Date, Fecha_OperacionI)
            CreaParametro(cmd, "@Fecha_OperacionF", SqlDbType.Date, Fecha_OperacionF)
            CreaParametro(cmd, "@Id_Cierre", SqlDbType.Int, Id_Cierre)
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, ClienteGrupo)

            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "T")
            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Autorizar Dotaciones-Proceso"

    Public Function fn_AutorizarDotaciones_GetClientes() As DataTable
        Try
            '25/ago/2016
            Dim cmd As SqlCommand = CreaComando("web_AutorizarDotaciones_GetClientes")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)
            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarDotaciones_GetActivas(ByVal P_Clave_SucursalPropia As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Cli_Dotaciones_GetActivas", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, P_Clave_SucursalPropia)
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_AutorizarDotaciones_GetDetalle(ByVal Id_DotacionCli As Integer) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_DotacionesD_Get")
            CreaParametro(cmd, "@Id_DotacionCli", SqlDbType.Int, Id_DotacionCli)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarDoraciones_Status(ByVal Id_DotacionCli As Integer, ByVal Estatus As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_Dotaciones_Status")
            CreaParametro(cmd, "@Id_DotacionCli", SqlDbType.Int, Id_DotacionCli)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)

            Return EjecutaNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Function fn_AutorizarDotaciones_Autorizar(ByVal Id_DotacionCli As Integer, P_Id_Sucursal As Integer, P_Id_CajaBancaria As Integer,
                                                     P_Id_ClienteP As Integer, P_Id_Moneda As Integer, Importe As Decimal,
                                                     Cantidad_Sobres As Integer, Fecha_Entrega As Date, Comentarios As String,
                                                     Conexion As String, Tbl_Detalle As DataTable, Hora_Entrega As String, tipoDotacion As Byte) As Boolean

        Dim cnn As New SqlConnection(Conexion)
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Dim cmd As SqlCommand
        Try
            cmd = CreaComando(tr, "Web_Dotaciones_Create")
            CreaParametro(cmd, "@Id_DotacionCli", SqlDbType.Int, Id_DotacionCli)
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, P_Id_Sucursal)
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, P_Id_CajaBancaria)
            CreaParametro(cmd, "@Id_ClienteP", SqlDbType.Int, P_Id_ClienteP)
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, P_Id_Moneda)
            CreaParametro(cmd, "@Importe", SqlDbType.Decimal, Importe)
            CreaParametro(cmd, "@Cantidad_Sobres", SqlDbType.Int, Cantidad_Sobres)
            CreaParametro(cmd, "@Fecha_Entrega", SqlDbType.Date, Fecha_Entrega)
            CreaParametro(cmd, "@Hora_Entrega", SqlDbType.VarChar, Hora_Entrega)
            CreaParametro(cmd, "@Documentos", SqlDbType.VarChar, "N")
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, 2)
            CreaParametro(cmd, "@Tipo", SqlDbType.Int, tipoDotacion)
            CreaParametro(cmd, "@Subtipo", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Id_Usuario", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Estacion_Captura", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "SO")
            Dim Id_Dotacion As Integer = EjecutarScalarT(cmd)
            If Id_Dotacion = 0 Then
                tr.Rollback()
                Return False
            Else
                cmd.Parameters.Clear()
                For Each Fila As DataRow In Tbl_Detalle.Rows
                    cmd = CreaComando(tr, "Web_DotacionesD_Create")
                    CreaParametro(cmd, "@Id_Dotacion", SqlDbType.Int, Id_Dotacion)
                    CreaParametro(cmd, "@Id_Denominacion", SqlDbType.Int, Fila("IdD").ToString)
                    CreaParametro(cmd, "@Cantidad", SqlDbType.Int, Fila("Cantidad").ToString)
                    CreaParametro(cmd, "@CantidadA", SqlDbType.Int, 0)
                    CreaParametro(cmd, "@CantidadB", SqlDbType.Int, 0)
                    CreaParametro(cmd, "@CantidadC", SqlDbType.Int, 0)
                    CreaParametro(cmd, "@CantidadD", SqlDbType.Int, 0)
                    CreaParametro(cmd, "@CantidadE", SqlDbType.Int, 0)
                    EjecutarNonQueryT(cmd)
                    cmd.Parameters.Clear()
                Next
                tr.Commit()

                cnn = New SqlConnection(Session("ConexionCentral"))
                cmd = CreaComando(cnn, "Cli_Dotaciones_Status")
                CreaParametro(cmd, "@Id_DotacionCli", SqlDbType.Int, Id_DotacionCli)
                CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
                CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
                CreaParametro(cmd, "@Status", SqlDbType.VarChar, "V")

                Return EjecutaNonQuery(cmd) > 0

            End If
            'Return True
        Catch ex As Exception
            Try
                tr.Rollback()
            Catch
            End Try
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Consulta Dotaciones-Proceso"

    Public Function fn_Consulta_DotacionesProceso(ByVal FechaDesde As DateTime, ByVal FechaHasta As DateTime, ByVal Id_Moneda As Integer, ByVal Status As String, ByVal Id_Cliente_Combo As Integer, ByVal TodosClientes As Boolean, ByVal Nivel As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Dotaciones_Dotaciones_New")
            CreaParametro(cmd, "@FechaDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.Date, FechaHasta)
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, Id_Moneda)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            If Not TodosClientes Then CreaParametro(cmd, "@Todos", SqlDbType.VarChar, "N")
            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@IdCliente", SqlDbType.Int, Id_Cliente_Combo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaDotacion_Monedas() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Monedas_Get")
            'CreaParametro(cmd, "@Id_Sucursal", Id_Sucursal)
            'CreaParametro(cmd, "@Pista", "")
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaDotacion_Detalle(ByVal Id_Dotacion As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Pro_DotacionesD_GetSinTipos")
            CreaParametro(cmd, "@Id_Dotacion", SqlDbType.Int, Id_Dotacion)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Solicitud de Materiales-Traslado"

    Public Function fn_SolicitudMateriales_GetMateriales(ByVal Id_Cliente As Integer) As DataTable
        Const MaterialesParaVentaAClientes As Integer = 4

        Try
            Dim cmd As SqlCommand = CreaComando("Web_Materiales_ComboGetCS")
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
            CreaParametro(cmd, "@Tipo", SqlDbType.Int, MaterialesParaVentaAClientes)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_SolicitudMateriales_Guardar(ByVal FechaEntrega As DateTime, ByVal Tabla As DataTable, ByVal Id_Cliente As Integer, Cliente As String, Comentarios As String) As Boolean
        Const CapturadoDesdeWeb As Integer = 2
        Const SinDepartamento As Integer = 0
        Const No As String = "N"
        Const Solicitado As String = "SO"

        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Dim cmd As SqlCommand = Nothing
        Try

            cmd = CreaComando(tr, "Cli_Ventas_Create")
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Session("Clave_SucursalPropia"))
            CreaParametro(cmd, "@Clave_Sucursal", SqlDbType.VarChar, Session("ClaveSucursal"))
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
            CreaParametro(cmd, "@Cliente", SqlDbType.VarChar, Cliente)
            CreaParametro(cmd, "@Id_Departamento", SqlDbType.Int, SinDepartamento)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Fecha_Entrega", SqlDbType.DateTime, FechaEntrega)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, CapturadoDesdeWeb)
            CreaParametro(cmd, "@Interno", SqlDbType.VarChar, No)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Solicitado)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pIp_Cliente)
            Dim Id_Venta As Integer = EjecutarScalarT(cmd)
            cmd.Parameters.Clear()

            cmd = CreaComando(tr, "Cli_VentasD_Create")
            For Each row As DataRow In Tabla.Rows
                CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_Venta)
                CreaParametro(cmd, "@Material", SqlDbType.VarChar, row("Material"))
                CreaParametro(cmd, "@Id_Inventario", SqlDbType.Int, row("Id_Inventario"))
                CreaParametro(cmd, "@Cantidad", SqlDbType.Int, row("Cantidad"))
                CreaParametro(cmd, "@Id_CS", SqlDbType.Int, row("IdCS"))
                CreaParametro(cmd, "@Precio", SqlDbType.Decimal, row("Precio"))
                EjecutarNonQueryT(cmd)
                cmd.Parameters.Clear()
            Next

            tr.Commit()
            Return True
        Catch ex As Exception
            tr.Rollback()
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Function fn_SolicitudMateriales_CreateVenta(ByRef Tr As SqlTransaction, ByVal FechaEntrega As Date, ByVal Id_Cliente As Integer) As Integer
        Const CapturadoDesdeWeb As Integer = 2
        Const SinDepartamento As Integer = 0
        Const No As String = "N"
        Const Solicitado As String = "SO"

        Try

            Dim cmd As SqlCommand = CreaComando(Tr, "Mat_Ventas_Create")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
            CreaParametro(cmd, "@Id_Departamento", SqlDbType.Int, SinDepartamento)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Fecha_Entrega", SqlDbType.DateTime, FechaEntrega)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, CapturadoDesdeWeb)
            CreaParametro(cmd, "@Interno", SqlDbType.VarChar, No)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Solicitado)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pIp_Cliente)

            Return EjecutaScalar(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return 0
        End Try
    End Function

    Public Function fn_SolicitudMateriales_CreateDetalle(ByRef Tr As SqlTransaction, ByVal Id_Venta As Integer, ByVal Id_Inventario As Integer, ByVal Cantidad As Integer) As Boolean

        Try
            Dim cmd As SqlCommand = CreaComando(Tr, "Mat_VentasD_Create")
            CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_Venta)
            CreaParametro(cmd, "@Id_Inventario", SqlDbType.Int, Id_Inventario)
            CreaParametro(cmd, "@Cantidad", SqlDbType.Int, Cantidad)

            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

#End Region

#Region "Autorizar Materiales-Traslado"
    Public Function fn_AutorizarMateriales_Get(ByVal pId_Cliente As Integer, Estatus As String) As DataTable '(ByVal P_Clave_SucursalPropia As String, Estatus As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Cli_Ventas_Get", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            'CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, P_Clave_SucursalPropia)
            'CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_AutorizarMateriales_GetDetalle(ByVal Id_MatVenta As Integer) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_VentasD_Get")
            CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_MatVenta)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarMateriales_Status(ByVal Id_MatVenta As Integer, ByVal Estatus As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_Ventas_Status")
            CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_MatVenta)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)

            Return EjecutaNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function
    Public Function fn_ConsultarAutorizacion_Materiales(ByVal Id_MatVenta As Integer, ByVal Status As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Web_Ventas_Status", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_MatVenta", SqlDbType.VarChar, Id_MatVenta)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarMateriales_Autorizar(IdS As Integer, IdMV As Integer, ByVal FechaEntrega As DateTime,
                                                     ByVal IdC As Integer, Tbl_Detalle As DataTable, Conexion As String, Comentarios As String) As Boolean
        Const CapturadoDesdeWeb As Integer = 2
        Const SinDepartamento As Integer = 0
        Const No As String = "N"
        Const Solicitado As String = "SO"
        Dim cnn As New SqlConnection(Conexion)
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Dim cmd As SqlCommand
        Try
            cmd = CreaComando(tr, "Web_Ventas_Create") 'Se conecta al SIAC 
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, IdS)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, IdC)
            CreaParametro(cmd, "@Id_Departamento", SqlDbType.Int, SinDepartamento)
            CreaParametro(cmd, "@Fecha_Entrega", SqlDbType.DateTime, FechaEntrega)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, CapturadoDesdeWeb)
            CreaParametro(cmd, "@Interno", SqlDbType.VarChar, No)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pIp_Cliente)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Solicitado)

            Dim Id_Venta As Integer = EjecutarScalarT(cmd)
            If Id_Venta = 0 Then
                tr.Rollback()
                Return False
            Else
                cmd.Parameters.Clear()
                cmd = CreaComando(tr, "Web_VentasD_Create")
                For Each Fila As DataRow In Tbl_Detalle.Rows
                    CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_Venta)
                    CreaParametro(cmd, "@Id_Inventario", SqlDbType.Int, Fila("IdI"))
                    CreaParametro(cmd, "@Cantidad", SqlDbType.Int, Fila("Cantidad"))
                    CreaParametro(cmd, "@Id_CS", SqlDbType.Int, Fila("IdCS"))
                    CreaParametro(cmd, "@Precio", SqlDbType.Decimal, Fila("Precio"))
                    EjecutarNonQueryT(cmd)
                    cmd.Parameters.Clear()
                Next
                tr.Commit()

                cnn = New SqlConnection(Session("ConexionCentral"))
                cmd = CreaComando(cnn, "Cli_Ventas_Status")
                CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, IdMV)
                CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
                CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
                CreaParametro(cmd, "@Status", SqlDbType.VarChar, "V")

                Return EjecutaNonQuery(cmd) > 0

            End If
            'Return True
        Catch ex As Exception
            Try
                tr.Rollback()
            Catch
            End Try
            TrataEx(ex)
            Return False
        End Try
    End Function
#End Region

#Region "Consulta de Solicitudes de Materiales-Traslado"

    Public Function fn_ConsultaMateriales_GetVentas(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Id_Cliente_Combo As Integer,
                                                    ByVal TodosClientes As Boolean, ByVal Nivel As Integer) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaMateriales_GetVentasNew")
            Dim Todos As Char = "N"
            If TodosClientes Then Todos = "S"
            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@IdCliente", SqlDbType.Int, Id_Cliente_Combo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            CreaParametro(cmd, "@Todos", SqlDbType.VarChar, Todos)
            CreaParametro(cmd, "@FechaDesde", SqlDbType.DateTime, FechaDesde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.DateTime, FechaHasta)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaMateriales_GetDetalle(ByVal Id_MatVenta As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaMateriales_GetDetalle")
            CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_MatVenta)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaMateriales_Cancelar(ByVal Id_MatVenta As Integer) As Boolean
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaMatierales_Cancelar")
            CreaParametro(cmd, "@Id_MatVenta", SqlDbType.Int, Id_MatVenta)

            Return EjecutaNonQuery(cmd) > 0
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

    End Function

#End Region


#Region "Solicitud de Servicios-Traslado"

    Public Function fn_SolicitudServicios_GetServicios(ByVal Id_Cliente As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_PreciosXclienteCombo_Get")
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    'Public Function fn_SolicitudServicios_GetOrigenDestino(ByVal Id_Cliente As Integer) As DataSet
    '    Try
    '        'Esta funcion ya no se usa desde 25/ago 2016, se dividio en 2 funciones
    '        Dim cmd As SqlCommand = CreaComando("web_SolicitudServicios_GetOrigenDestino")
    '        CreaParametro(cmd, "@IdCliente", SqlDbType.Int, Id_Cliente)
    '        'If pExiste_Grupo = "S" Then
    '        '    CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
    '        'End If
    '        Dim ds As DataSet = EjecutaDataSet(cmd)
    '        ds.Tables(0).TableName = "TblOrigen"
    '        ds.Tables(1).TableName = "TblDestino"

    '        Return ds
    '    Catch ex As Exception
    '        TrataEx(ex)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function fn_SolicitudServicios_GetOrigen(ByVal Id_Cliente As Integer) As DataTable
        Try
            '25/ago2016
            Dim cmd As SqlCommand = CreaComando("web_SolicitudServicios_GetOrigen")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, Id_Cliente)
            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            Dim dt_Origen As DataTable = EjecutaConsulta(cmd)
            Return dt_Origen
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_SolicitudServicios_GetDestino(ByVal Id_Cliente As Integer) As DataTable
        Try
            '25/ago/2016
            Dim cmd As SqlCommand = CreaComando("web_SolicitudServicios_GetDestino")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, Id_Cliente)

            Dim dt_Destino As DataTable = EjecutaConsulta(cmd)
            Return dt_Destino

        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_SolicitudServicios_Save(ByVal FechaServicio As Date, Id_ClinteSolicita As Integer, Cliente As String, ByVal IdCs As Integer, Servicio As String, ByVal IdClienteOrigen As Integer, Nombre_ClienteOrigen As String,
                                               ByVal HoraRec As String, ByVal IdClienteDestino As Integer, Nombre_ClienteDestino As String, ByVal HoraEnt As String, Comentarios As String) As Boolean

        Const ViaWebDeClientes As Short = 2
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Try
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_ServiciosTel_Create")
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Session("Clave_SucursalPropia"))
            CreaParametro(cmd, "@Clave_Sucursal", SqlDbType.VarChar, Session("ClaveSucursal"))
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Id_Ruta", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Fecha_Servicio", SqlDbType.DateTime, FechaServicio)
            CreaParametro(cmd, "@Cliente_Solicita", SqlDbType.Int, Id_ClinteSolicita)
            CreaParametro(cmd, "@Cliente", SqlDbType.VarChar, Cliente)
            CreaParametro(cmd, "@Id_CS", SqlDbType.Int, IdCs)
            CreaParametro(cmd, "@Servicio", SqlDbType.VarChar, Servicio)
            CreaParametro(cmd, "@Cliente_Origen", SqlDbType.Int, IdClienteOrigen)
            CreaParametro(cmd, "@Nombre_ClienteOrigen", SqlDbType.VarChar, Nombre_ClienteOrigen)
            CreaParametro(cmd, "@H_Recoleccion", SqlDbType.VarChar, HoraRec)
            CreaParametro(cmd, "@Cliente_Destino", SqlDbType.Int, IdClienteDestino)
            CreaParametro(cmd, "@Nombre_ClienteDestino", SqlDbType.VarChar, Nombre_ClienteDestino)
            CreaParametro(cmd, "@H_Entrega", SqlDbType.VarChar, HoraEnt)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, ViaWebDeClientes)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pIp_Cliente)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "SO")
            EjecutaNonQuery(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try

        Return True
    End Function

#End Region

#Region "Autorizar Servicios-Traslado 03/12/2014"
    Public Function fn_AutorizarServicios_Get(ByVal P_Clave_SucursalPropia As String, Estatus As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Cli_SolicitudesTel_Get", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, P_Clave_SucursalPropia)
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarServicios_Autorizar(Id_ST As Integer, ByVal FechaServicio As Date, Id_ClienteSolicita As Integer,
                                                    ByVal IdCs As Integer, ByVal IdClienteOrigen As Integer,
                                                     ByVal HoraRecoleccion As String, ByVal IdClienteDestino As Integer,
                                                     ByVal HoraEntrega As String, conexion As String, Comentarios As String) As Boolean
        Const CapturadoDesdeWeb As Integer = 2
        Const Status As String = "A"
        Dim cnn As New SqlConnection(conexion)
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Dim cmd As SqlCommand
        Try
            cmd = CreaComando(tr, "web_ServiciosTel_Create") 'Se conecta al SIAC 
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            CreaParametro(cmd, "@Fecha_Servicio", SqlDbType.DateTime, FechaServicio)
            CreaParametro(cmd, "@Cliente_Solicita", SqlDbType.Int, Id_ClienteSolicita)
            CreaParametro(cmd, "@Id_CS", SqlDbType.Int, IdCs)
            CreaParametro(cmd, "@H_Recoleccion", SqlDbType.VarChar, HoraRecoleccion)
            CreaParametro(cmd, "@H_Entrega", SqlDbType.VarChar, HoraEntrega)
            CreaParametro(cmd, "@Cliente_Origen", SqlDbType.Int, IdClienteOrigen)
            CreaParametro(cmd, "@Cliente_Destino", SqlDbType.Int, IdClienteDestino)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, CapturadoDesdeWeb)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, pIp_Cliente)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            EjecutarNonQueryT(cmd)
            tr.Commit()

            cnn = New SqlConnection(Session("ConexionCentral"))
            cmd = CreaComando(cnn, "Cli_ServiciosTel_Status")

            CreaParametro(cmd, "@Id_ServicioTel", SqlDbType.Int, Id_ST)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "V")
            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception

            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Function fn_AutorizarServicios_Status(ByVal Id_ST As Integer, ByVal Estatus As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_ServiciosTel_Status")
            CreaParametro(cmd, "@Id_ServicioTel", SqlDbType.Int, Id_ST)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)

            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Consulta de Servicios-Traslado"
    Public Function fn_ConsultaTrasladoG_GetPuntos(ByVal FechaInicial As Date, ByVal FechaFinal As Date, ByVal Id_ClienteCombo As Integer, ByVal Todos As Boolean, ByVal Nivel As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaTrasladoG_GetPuntos")

            If Nivel = 1 Then
                If Todos Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            CreaParametro(cmd, "@FechaInicial", SqlDbType.Date, FechaInicial)
            CreaParametro(cmd, "@FechaFinal", SqlDbType.Date, FechaFinal)
            If Todos Then CreaParametro(cmd, "@Todos", SqlDbType.Bit, 1)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaTraslado_GetPuntos(ByVal FechaInicial As Date, ByVal FechaFinal As Date, ByVal Id_ClienteCombo As Integer, ByVal Todos As Boolean, ByVal Nivel As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaTraslado_GetPuntosNew")
            If Nivel = 1 Then
                If Todos Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_ClienteCombo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            CreaParametro(cmd, "@FechaInicial", SqlDbType.Date, FechaInicial)
            CreaParametro(cmd, "@FechaFinal", SqlDbType.Date, FechaFinal)
            If Todos Then CreaParametro(cmd, "@Todos", SqlDbType.Bit, 1)

            Dim dt_puntos As DataTable = EjecutaConsulta(cmd)
            Return dt_puntos
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaTraslado_GetRemisiones(ByVal Id_Punto As Long) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaTraslados_GetRemisiones")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Int, Id_Punto)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaTraslado_GetMonedas(ByVal Id_Remision As Long) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaTraslado_GetRemisionesD")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, Id_Remision)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaTraslado_GetEnvases(ByVal Id_Remision As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaTraslado_GetEnvases")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, Id_Remision)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region


#Region "Capturar Fallas/Custodias-Cajeros 20/enero/2015"
    Public Function fn_FallasCustodias_Crear(ByVal Idcajero As Integer, ByVal IdParteF As Integer, ByVal NumeroReporte As String,
                                                    ByVal FechaRequerida As Date, ByVal Tiempo_Respuesta As String, ByVal Tipo As Integer,
                                                    ByVal Hora_SolicitaBanco As String, ByVal Fecha_Alarma As Date, ByVal Hora_Alarma As String,
                                                    Comentarios As String, Numcajero As String, Descripcion As String, ParteFalla As String, TipoFalla As String) As Boolean
        Dim Cmd As SqlCommand
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim lc_Transaccion As SqlTransaction = CreaTransaccion(cnn)

        Try
            Cmd = CreaComando(lc_Transaccion, "Cli_Fallas_Create")
            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, Idcajero)

            CreaParametro(Cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(Cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Session("Clave_SucursalPropia"))
            CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Session("ClaveSucursal"))
            CreaParametro(Cmd, "@Id_Sucursal", SqlDbType.Int, Session("Id_Sucursal"))
            CreaParametro(Cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(Cmd, "@Id_ParteF", SqlDbType.Int, IdParteF)
            CreaParametro(Cmd, "@Numero_Reporte", SqlDbType.VarChar, NumeroReporte)
            CreaParametro(Cmd, "@Usuario_Captura", SqlDbType.Int, pId_Usuario)
            CreaParametro(Cmd, "@Fecha_Requerida", SqlDbType.DateTime, FechaRequerida)

            CreaParametro(Cmd, "@Tiempo_Respuesta", SqlDbType.Time, Tiempo_Respuesta)

            CreaParametro(Cmd, "@Tipo", SqlDbType.Int, Tipo)
            CreaParametro(Cmd, "@Hora_SolicitaBanco", SqlDbType.Time, Hora_SolicitaBanco)
            CreaParametro(Cmd, "@Numero_Vuelta", SqlDbType.TinyInt, 1) 'default
            CreaParametro(Cmd, "@Prioridad", SqlDbType.TinyInt, 1)
            CreaParametro(Cmd, "@Estacion_Captura", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(Cmd, "@Fecha_Alarma", SqlDbType.Date, Fecha_Alarma)
            CreaParametro(Cmd, "@Hora_Alarma", SqlDbType.Time, Hora_Alarma)
            CreaParametro(Cmd, "@Modo_Captura", SqlDbType.TinyInt, 2) 'web
            CreaParametro(Cmd, "@Status", SqlDbType.VarChar, "A")
            CreaParametro(Cmd, "@Numero_Cajero", SqlDbType.VarChar, Numcajero)
            CreaParametro(Cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)
            CreaParametro(Cmd, "@Parte_Falla", SqlDbType.VarChar, ParteFalla)
            CreaParametro(Cmd, "@Tipo_Falla", SqlDbType.VarChar, TipoFalla)

            EjecutaNonQuery(Cmd)
            lc_Transaccion.Commit()
            Return True
        Catch ex As Exception
            lc_Transaccion.Rollback()
            TrataEx(ex)
            Return False
        End Try

    End Function

    'Public Function fn_FallasCustodiasCorte_Crear(ByVal Id_Cajero As Integer, ByVal H_Entrega As String) As Boolean
    '    Try

    '        Dim Cmd As SqlClient.SqlCommand = CreaComando("Caj_Puntos_Create")

    '        CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, Id_Cajero)
    '        CreaParametro(Cmd, "@Id_Remision", SqlDbType.Int, 0)
    '        CreaParametro(Cmd, "@H_Entrega", SqlDbType.VarChar, H_Entrega)
    '        CreaParametro(Cmd, "@Prioridad", SqlDbType.Int, 1)
    '        CreaParametro(Cmd, "@Status_Boveda", SqlDbType.VarChar, "A")
    '        EjecutaNonQuery(Cmd)
    '        Return True
    '    Catch ex As Exception
    '        TrataEx(ex)
    '        Return False
    '    End Try
    'End Function

    Public Function fn_ConsultaPartes() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Caj_Partes_Get")
            CreaParametro(cmd, "@Pista", SqlDbType.VarChar, "")
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaPartes_TipoFalla(ByVal ID_ParteF As Integer) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("Caj_PartesF_Get")
            CreaParametro(cmd, "@Id_Parte", SqlDbType.Int, ID_ParteF)
            CreaParametro(cmd, "@Pista", SqlDbType.VarChar, "")
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
#End Region

#Region "Autorizar Fallas-Cajeros 20 / enero / 2015"
    Public Function fn_AutorizarFallas_GetActivas(ByVal P_Clave_SucursalPropia As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Cli_Fallas_GetActivas", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, P_Clave_SucursalPropia)
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_AutorizarFallas_Autorizar(Conexion As String, Id_FallaCli As Integer, IdCajero As Integer, IdParteF As Integer,
                                                 NumeroReporte As String, FechaRequerida As Date, TiempoRespuesta As String, Tipo As Integer,
                                                 Fecha_Alarma As Date, Hora_Alarma As String, Hora_SolicitaBanco As String, Comentarios As String) As Boolean

        Dim cnn As New SqlConnection(Conexion)
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Dim cmd As SqlCommand
        Try
            cmd = CreaComando(tr, "web_Fallas_Create")

            CreaParametro(cmd, "@Id_Cajero", SqlDbType.Int, IdCajero)
            CreaParametro(cmd, "@Id_ParteF", SqlDbType.Int, IdParteF)
            CreaParametro(cmd, "@Numero_Reporte", SqlDbType.VarChar, NumeroReporte)
            CreaParametro(cmd, "@Fecha_Requerida", SqlDbType.Date, FechaRequerida)

            CreaParametro(cmd, "@Tiempo_Respuesta", SqlDbType.Time, TiempoRespuesta)
            CreaParametro(cmd, "@Tipo", SqlDbType.Int, Tipo)
            CreaParametro(cmd, "@Fecha_Alarma", SqlDbType.Date, Fecha_Alarma)
            CreaParametro(cmd, "@Hora_Alarma", SqlDbType.Time, Hora_Alarma)
            CreaParametro(cmd, "@Hora_SolicitaBanco", SqlDbType.Time, Hora_SolicitaBanco)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Id_Programacion", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Numero_Vuelta", SqlDbType.TinyInt, 1)
            CreaParametro(cmd, "@Prioridad", SqlDbType.TinyInt, 1)
            CreaParametro(cmd, "@Usuario_Captura", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Estacion_Captura", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.TinyInt, 2) '2=web
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
            EjecutarNonQueryT(cmd)
            tr.Commit()

            cnn = New SqlConnection(Session("ConexionCentral"))
            cmd = CreaComando(cnn, "Cli_Fallas_Status")
            CreaParametro(cmd, "@Id_FallaCli", SqlDbType.Int, Id_FallaCli)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "V")
            Return EjecutaNonQuery(cmd) > 0
        Catch ex As Exception
            Try
                tr.Rollback()
            Catch
            End Try
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Function fn_AutorizarFallasCustodias_Status(ByVal Id_FallaCli As Integer, ByVal Estatus As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_Fallas_Status")
            CreaParametro(cmd, "@Id_FallaCli", SqlDbType.Int, Id_FallaCli)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)
            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function
#End Region

#Region "Consulta Fallas-Cajeros"

    Public Function fn_ConsultaFallasCajeros_LlenaGridview(ByVal FechaInicial As Date, ByVal FechaFinal As Date, IDCajaBancaria As Integer, IdCajero As Integer, Status As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_ConsultaFallas_GetFallas")
            CreaParametro(cmd, "@Fecha_Desde", SqlDbType.Date, FechaInicial)
            CreaParametro(cmd, "@Fecha_Hasta", SqlDbType.Date, FechaFinal)
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, IDCajaBancaria)
            CreaParametro(cmd, "@Id_Cajero", SqlDbType.Int, IdCajero)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaFallasCajeros_DetalleTarjetas(IdFalla As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Tarjetas_GetByFalla")
            CreaParametro(cmd, "@Id_Falla", SqlDbType.Int, IdFalla)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaPuntosCajeros_DetalleTarjetas(IdPuntoC As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_Tarjetas_GetByPuntoC")
            CreaParametro(cmd, "@Id_PuntoC", SqlDbType.Int, IdPuntoC)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaFallasCajeros_DetalleRemision(IdRemision As Integer) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_Remisiones_GetById")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, IdRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region


#Region "Solicitud Dotaciones-Cajeros 14/ENERO/2015"
    Public Function fn_DotacionCajeros_Nuevo(ByVal Numero_Reporte As String, ByVal dt As DataTable, ByVal IdCajero As Integer, ByVal IdMoneda As Integer,
                                                    ByVal Importe As Decimal, ByVal FechaEntrega As Date, ByVal HorarioEntrega As String, ByVal RequiereCorte As String,
                                                    ByVal Hora_SolicitaBanco As String, ByVal Prioridad_Banco As String, ByVal IdCajaBancaria As Integer,
                                                    ByVal UsuarioID As Integer, comentarios As String, Numero_Cajero As String, Descripcion As String, Nombre_Moneda As String, Especial As Char, Prioridad As Integer, IdRuta As Integer) As Boolean
        Dim Cmd As SqlCommand
        Dim Elemento As DataRow
        Dim IDotacioncaj As Integer
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim lc_Transaccion As SqlTransaction = CreaTransaccion(cnn)

        Try

            Cmd = CreaComando(lc_Transaccion, "Cli_DotacionesCaj_Create")

            CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, IdCajaBancaria) 'newPendiente
            CreaParametro(Cmd, "@Modo_Captura", SqlDbType.Int, 2)
            CreaParametro(Cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            CreaParametro(Cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Session("Clave_SucursalPropia"))
            CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Session("ClaveSucursal"))
            CreaParametro(Cmd, "@Id_Sucursal", SqlDbType.Int, Session("Id_Sucursal"))
            CreaParametro(Cmd, "@Comentarios", SqlDbType.VarChar, comentarios)
            CreaParametro(Cmd, "@Numero_Reporte", SqlDbType.VarChar, Numero_Reporte)
            CreaParametro(Cmd, "@Numero_Cajero", SqlDbType.VarChar, Numero_Cajero)
            CreaParametro(Cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)
            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, IdCajero)
            CreaParametro(Cmd, "@Id_Moneda", SqlDbType.Int, IdMoneda)
            CreaParametro(Cmd, "@Nombre_Moneda", SqlDbType.VarChar, Nombre_Moneda)
            CreaParametro(Cmd, "@Importe", SqlDbType.Money, Importe)
            CreaParametro(Cmd, "@Fecha_Entrega", SqlDbType.DateTime, FechaEntrega)
            CreaParametro(Cmd, "@Horario_Entrega", SqlDbType.VarChar, HorarioEntrega)
            CreaParametro(Cmd, "@Requiere_Corte", SqlDbType.VarChar, RequiereCorte)
            CreaParametro(Cmd, "@Prioridad_Banco", SqlDbType.VarChar, Prioridad_Banco)
            CreaParametro(Cmd, "@Especial", SqlDbType.VarChar, Especial)
            CreaParametro(Cmd, "@Prioridad", SqlDbType.Int, Prioridad)
            CreaParametro(Cmd, "@Hora_SolicitaBanco", SqlDbType.VarChar, Hora_SolicitaBanco)
            CreaParametro(Cmd, "@Estacion_Captura", SqlDbType.VarChar, pIp_Cliente) '*--
            CreaParametro(Cmd, "@Usuario_Captura", SqlDbType.Int, UsuarioID)
            CreaParametro(Cmd, "@Id_Ruta", SqlDbType.Int, IdRuta)
            CreaParametro(Cmd, "@Status", SqlDbType.VarChar, "A")

            IDotacioncaj = EjecutaScalar(Cmd)

            For Each Elemento In dt.Rows
                If Elemento("Piezas") > 0 Then
                    Cmd = CreaComando(lc_Transaccion, "Cli_DotacionesCajD_Create")
                    CreaParametro(Cmd, "@Id_Dotacioncaj", SqlDbType.Int, IDotacioncaj)
                    CreaParametro(Cmd, "@Id_Denominacion", SqlDbType.Int, Elemento("Id_Denominacion"))
                    CreaParametro(Cmd, "@Cantidad", SqlDbType.Int, Elemento("Piezas"))
                    CreaParametro(Cmd, "@Denominacion", SqlDbType.Decimal, Elemento("Denominacion"))
                    CreaParametro(Cmd, "@Presentacion", SqlDbType.VarChar, "B")
                    EjecutaNonQuery(Cmd)
                End If
            Next

            lc_Transaccion.Commit()
            Return True
        Catch ex As Exception
            lc_Transaccion.Rollback()
            TrataEx(ex)
            Return False
        End Try

    End Function

    Public Function fn_SolicitudDotaciones_GetCajasBancarias_Cajeros() As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("web_SolicitudDotaciones_GetCajasBancarias")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Autorizar Dotaciones-Cajeros 14/01/2015"
    Public Function fn_AutorizarDotacionesCajeros_GetActivas(ByVal P_Clave_SucursalPropia As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = Crea_Comando("Cli_DotacionesCaj_GetActivas", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, P_Clave_SucursalPropia)
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Session("Clave_Corporativo"))
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_AutorizarDotacionesCajeros_GetDetalle(ByVal Id_DotacionCaj As Integer) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_DotacionesCajD_Get")
            CreaParametro(cmd, "@Id_DotacionCaj", SqlDbType.Int, Id_DotacionCaj)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarDoracionesCajeros_Status(ByVal Id_DotacionCaj As Integer, ByVal Estatus As String) As Boolean
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand = CreaComando(cnn, "Cli_DotacionesCaj_Status")
            CreaParametro(cmd, "@Id_DotacionCaj", SqlDbType.Int, Id_DotacionCaj)
            CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
            CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Estatus)
            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Function fn_AutorizarDotacionesCajeros_Autorizar(ByVal Id_DotacionCaj As Integer, Conexion As String, Tbl_Detalle As DataTable, P_Id_Sucursal As Integer,
                                                            P_Id_Moneda As Integer, Importe As Decimal, Fecha_Entrega As Date, Hora_Entrega As String,
                                                            Comentarios As String, IdCajero As Integer, RequiereCorte As Char, Hora_SolicitaBanco As String,
                                                            Numero_Reporte As String, Prioridad_Banco As Char, Especial As Char, Prioridad As Integer, Id_Ruta As Integer) As Boolean
        Dim cnn As New SqlConnection(Conexion)
        Dim tr As SqlTransaction = CreaTransaccion(cnn)
        Dim cmd As SqlCommand
        Try
            cmd = CreaComando(tr, "web_DotacionesCaj_Create")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, P_Id_Sucursal)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Int, 2)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "SO")
            CreaParametro(cmd, "@Id_Cajero", SqlDbType.Int, IdCajero)
            CreaParametro(cmd, "@Id_Moneda", SqlDbType.Int, P_Id_Moneda)
            CreaParametro(cmd, "@Importe", SqlDbType.Decimal, Importe)
            CreaParametro(cmd, "@Estacion_Captura", SqlDbType.VarChar, pEstacion_Cliente)
            CreaParametro(cmd, "@Usuario_Captura", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Fecha_Entrega", SqlDbType.Date, Fecha_Entrega)
            CreaParametro(cmd, "@Horario_Entrega", SqlDbType.VarChar, Hora_Entrega)
            CreaParametro(cmd, "@Requiere_Corte", SqlDbType.VarChar, RequiereCorte)
            CreaParametro(cmd, "@Hora_SolicitaBanco", SqlDbType.VarChar, Hora_SolicitaBanco)
            CreaParametro(cmd, "@Numero_Reporte", SqlDbType.VarChar, Numero_Reporte)
            CreaParametro(cmd, "@Prioridad_Banco", SqlDbType.VarChar, Prioridad_Banco)
            CreaParametro(cmd, "@Comentarios", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@Especial", SqlDbType.VarChar, Especial)
            CreaParametro(cmd, "@Prioridad", SqlDbType.Int, Prioridad)
            CreaParametro(cmd, "@Id_Turno", SqlDbType.Int, 0)
            CreaParametro(cmd, "@Numero_Vuelta", SqlDbType.TinyInt, 1) 'pendient
            CreaParametro(cmd, "@Ingreso_Auditoria", SqlDbType.VarChar, "N") ' pendt
            CreaParametro(cmd, "@Id_Ruta", SqlDbType.Int, Id_Ruta)

            Dim Id_Dotacion As Integer = EjecutarScalarT(cmd)
            If Id_Dotacion = 0 Then
                tr.Rollback()
                Return False
            Else
                cmd.Parameters.Clear()
                For Each Fila As DataRow In Tbl_Detalle.Rows
                    cmd = CreaComando(tr, "Web_DotacionesCajD_Create")
                    CreaParametro(cmd, "@Id_Dotacion", SqlDbType.Int, Id_Dotacion)
                    CreaParametro(cmd, "@Id_Denominacion", SqlDbType.Int, Fila("IdD").ToString)
                    CreaParametro(cmd, "@Cantidad", SqlDbType.Int, Fila("Cantidad").ToString)
                    EjecutarNonQueryT(cmd)
                    cmd.Parameters.Clear()
                Next
                tr.Commit()

                cnn = New SqlConnection(Session("ConexionCentral"))
                cmd = CreaComando(cnn, "Cli_DotacionesCaj_Status")
                CreaParametro(cmd, "@Id_DotacionCaj", SqlDbType.Int, Id_DotacionCaj)
                CreaParametro(cmd, "@Usuario_Afecta", SqlDbType.Int, pId_Usuario)
                CreaParametro(cmd, "@Estacion_Afecta", SqlDbType.VarChar, pEstacion_Cliente)
                CreaParametro(cmd, "@Status", SqlDbType.VarChar, "V")
                Return EjecutaNonQuery(cmd) > 0

            End If
        Catch ex As Exception
            Try
                tr.Rollback()
            Catch
            End Try
            TrataEx(ex)
            Return False
        End Try
    End Function

#End Region

#Region "Consulta Dotaciones-Cajeros y detalleDotaciones 14/01/2015"
    Public Function fn_ConsultaDotacionesCajeros_LlenaGridview(ByVal FechaD As Date, ByVal FechaH As Date, ByVal IDCajaBancaria As Integer, ByVal IdCajero As Integer, Status As String) As DataTable
        Try
            Dim Cmd As SqlCommand = CreaComando("web_ConsultaDotacionesCaj_GetDotaciones")

            CreaParametro(Cmd, "@Fecha_Desde", SqlDbType.Date, FechaD)
            CreaParametro(Cmd, "@Fecha_Hasta", SqlDbType.Date, FechaH)
            CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, IDCajaBancaria)
            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.Int, IdCajero)
            CreaParametro(Cmd, "@Status", SqlDbType.VarChar, Status)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ReporteDotacionLlenalistaDetalle(ByVal Id_Dotacion As Integer) As DataTable
        Try

            Dim Cmd As SqlCommand = CreaComando("Caj_DotacionesD_GetCompleto")
            CreaParametro(Cmd, "@Id_Dotacion", SqlDbType.Int, Id_Dotacion)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Consulta de Tarjetas Retenidas 06/Febrero/2015"
    Public Function fn_ConsultaTarjetasRetenidas_llenarGridView(ByVal Desde As Date, ByVal Hasta As Date, ByVal Id_CajaBancaria As Integer, ByVal Clonada As Char) As DataTable
        Dim cmd As SqlCommand = CreaComando("web_Tarjetas_GetRetenidas")

        Try
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal) 'pdte
            CreaParametro(cmd, "@Desde", SqlDbType.Date, Desde)
            CreaParametro(cmd, "@Hasta", SqlDbType.Date, Hasta)
            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
            CreaParametro(cmd, "@Clonada", SqlDbType.VarChar, Clonada)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Notificaciones"
    Public Function fn_Consulta_Notificaciones() As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            ' Dim cnn As New SqlConnectio()
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Cli_Notificaciones_Read", CommandType.StoredProcedure, cnn)

            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function fn_CuentaNotificaciones() As Integer
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand
            cmd = Crea_Comando("Cli_Notificaciones_Count", CommandType.StoredProcedure, cnn)

            Return EjecutaScalar(cmd)

        Catch ex As Exception
            Return 0
        End Try
    End Function

#End Region

#Region "Cambio de Sucursal"
    Public Function fn_ConsultaCadendas_Conexion(ByVal Clave_Corporativo As String) As DataTable
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Cli_Sucursales_Combo", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Clave_Corporativo", SqlDbType.VarChar, Clave_Corporativo)

            Return EjecutaConsulta(cmd)

        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function fn_Consulta_Id_ClienteXSucursal(ByVal Id_Usuario As Integer, ByVal Clave_SucursalPropia As String) As Integer
        Try
            Dim cnn As New SqlConnection(Session("ConexionCentral"))
            Dim cmd As SqlCommand

            cmd = Crea_Comando("Cli_UsuariosD_Read", CommandType.StoredProcedure, cnn)
            CreaParametro(cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
            CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Clave_SucursalPropia)

            Return EjecutaScalar(cmd)

        Catch ex As Exception
            Return -1
        End Try

    End Function

#End Region

#Region "Consulta CV"
    Public Function fn_ConsultaCV_Get(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Id_Cliente_Combo As Integer, ByVal TodosClientes As Boolean, ByVal Nivel As Integer) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("Web_ComprobantesV_Get")

            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente_Combo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If
            CreaParametro(cmd, "@FDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FHasta", SqlDbType.Date, FechaHasta)
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
            If Not TodosClientes Then CreaParametro(cmd, "@Todos", SqlDbType.VarChar, "N")

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

#End Region

#Region "Consulta Actas Diferencia"
    Public Function fn_ConsultaActasDiferencias_CajasBancarias_Get() As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("web_SolicitudDotaciones_GetCajasBancarias")
            CreaParametro(cmd, "@IdCliente", SqlDbType.Int, pId_Cliente)

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_ConsultaActasDiferencia_Get(ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal Id_Cliente_Combo As Integer, ByVal TodosClientes As Boolean, ByVal Nivel As Integer, ByVal Id_CajaBancaria As Integer) As DataTable

        Try
            Dim cmd As SqlCommand = CreaComando("Web_Pro_Actas_Get")

            If Nivel = 1 Then
                If TodosClientes Then
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
                Else
                    CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente_Combo)
                End If
            ElseIf Nivel = 2 Then
                CreaParametro(cmd, "@Id_Cliente", SqlDbType.Int, pId_Cliente)
            End If

            CreaParametro(cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
            CreaParametro(cmd, "@FDesde", SqlDbType.Date, FechaDesde)
            CreaParametro(cmd, "@FHasta", SqlDbType.Date, FechaHasta)
            'Si Todosclientes viene falso quire decir que solo quiere a uno 
            If Not TodosClientes Then
                CreaParametro(cmd, "@Hijo", SqlDbType.VarChar, "S")
            Else
                CreaParametro(cmd, "@Padre", SqlDbType.VarChar, "S")
            End If

            If pExiste_Grupo = "S" Then
                CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
            End If

            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
#End Region

#Region "Crear Usuarios"

    Public Function fn_LocalidadCorporativo(Clave_SucursalPropia As String) As DataTable

        Dim Dt As DataTable
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim cmd As SqlCommand = Crea_Comando("Web_Cli_CorporativoLocalidad", CommandType.StoredProcedure, cnn)
        CreaParametro(cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Clave_SucursalPropia)
        Try
            Dt = EjecutaConsulta(cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function Fn_UsuariosClientes_ComboSucursales(ByVal ClaveCorp As String, ByVal Clave_SucPro As String) As DataTable
        Dim Dt As DataTable
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Web_Cli_Sucursales_ComboGet", CommandType.StoredProcedure, cnn)
        CreaParametro(Cmd, "@Clave_Corp", SqlDbType.VarChar, ClaveCorp)
        CreaParametro(Cmd, "@Clave_SucPro", SqlDbType.VarChar, Clave_SucPro)
        Try
            Dt = EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function Fn_Sucursal_Cliente(ByVal ClaveCorp As String, ByVal Clave_SucPro As String, ByVal Clave_Sucursal As String) As DataTable
        Dim Dt As DataTable
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Web_Cli_SucursalCliente", CommandType.StoredProcedure, cnn)
        CreaParametro(Cmd, "@Clave_Corp", SqlDbType.VarChar, ClaveCorp)
        CreaParametro(Cmd, "@Clave_SucPro", SqlDbType.VarChar, Clave_SucPro)
        CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
        Try
            Dt = EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function Fn_UsuarioAReasignar(ByVal ClaveCorp As String, ByVal Clave_SucPro As String, ByVal IdUsuario As Integer) As DataTable
        Dim Dt As DataTable
        Dim cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = Crea_Comando("Web_Cli_UsuarioAReasignarGet", CommandType.StoredProcedure, cnn)
        CreaParametro(Cmd, "@Clave_Corporativo", SqlDbType.VarChar, ClaveCorp)
        CreaParametro(Cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Clave_SucPro)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, IdUsuario)
        Try
            Dt = EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing

        End Try
    End Function
    Public Function Fn_UsuariosXSucursal_ComboConsulta(ByVal Id_Cliente As Integer) As DataTable
        Dim Dt As DataTable
        Dim cmd As SqlCommand = CreaComando("Cat_Clientes_GetHijos")
        CreaParametro(cmd, "@AgregarPadre", SqlDbType.VarChar, "S") 'tenia S
        CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
        CreaParametro(cmd, "@Id_Padre", SqlDbType.Int, Id_Cliente)
        CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
        If pExiste_Grupo = "S" Then
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If
        Try
            Dt = EjecutaConsulta(cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function Fn_UsuariosXSucursal_Combo(ByVal Conexion As String, ByVal AgregarPadre As String, ByVal Id_Cliente As Integer) As DataTable
        Dim Dt As DataTable
        Dim Cnx_Alterna As New SqlConnection(Conexion)

        Dim cmd As SqlCommand = Crea_Comando("Cat_Clientes_GetHijos", CommandType.StoredProcedure, Cnx_Alterna)
        CreaParametro(cmd, "@AgregarPadre", SqlDbType.VarChar, "S") 'tenia S
        CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
        CreaParametro(cmd, "@Id_Padre", SqlDbType.Int, Id_Cliente)
        CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, pId_Sucursal)
        If pExiste_Grupo = "S" Then
            CreaParametro(cmd, "@Id_ClienteGrupo", SqlDbType.Int, pId_ClienteGrupo)
        End If
        Try
            Dt = EjecutaConsulta(cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function Fn_UsuariosClientes_Existe(ByVal Clave_Corp As String, ByVal Nombre_Sesion As String) As DataTable
        Dim Dt As DataTable
        Dim cnx As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Cli_Usuarios_ClaveExiste", CommandType.StoredProcedure, cnx)

        CreaParametro(Cmd, "@Clave_Corp", SqlDbType.VarChar, Clave_Corp)
        CreaParametro(Cmd, "@Nombre_Sesion", SqlDbType.VarChar, Nombre_Sesion)
        Try
            Dt = EjecutaConsulta(Cmd)
            Return Dt
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    'Public Function fn_UsuariosClientes_Get(ByVal Clave_Corporativo As String, Clave_Sucursal As String) As DataTable
    '    Dim Cnn As New SqlConnection(Session("ConexionCentral"))
    '    Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Usuarios_Get", CommandType.StoredProcedure, Cnn)
    '    CreaParametro(Cmd, "@Clave_Corporativo", SqlDbType.VarChar, Clave_Corporativo)
    '    CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
    '    CreaParametro(Cmd, "@Visible", SqlDbType.VarChar, "S")

    '    Try
    '        Return EjecutaConsulta(Cmd)
    '    Catch ex As Exception
    '        TrataEx(ex)
    '        Return Nothing
    '    End Try

    'End Function
    '    @Clave_Corporativo  Varchar(4),  
    '@Clave_SucursalPropia Varchar(10),           
    '@Clave_Sucursal Varchar(10),
    '@Visible   Varchar(1)='T',
    '@TodasSucursales int
    Public Function fn_UsuariosSucursales(Clave_Corporativo As String, Clave_SucursalPropia As String, Clave_Sucursal As String) As DataTable
        Dim AllSucursales As Integer = 0
        If Clave_Sucursal = "0" Then
            AllSucursales = 1
        End If
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Usuarios_GetConsulta", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Clave_Corporativo", SqlDbType.VarChar, Clave_Corporativo)
        CreaParametro(Cmd, "@Clave_SucursalPropia", SqlDbType.VarChar, Clave_SucursalPropia)
        CreaParametro(Cmd, "@AllSucursales", SqlDbType.Int, AllSucursales)
        CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
        CreaParametro(Cmd, "@Visible", SqlDbType.VarChar, "S")
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, pId_Usuario)
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_UsuariosClientes_Get(ByVal Id_Cliente As Integer, Clave_Sucursal As String) As DataTable
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Usuarios_Get", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
        CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
        CreaParametro(Cmd, "@Visible", SqlDbType.VarChar, "S")
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    'Reasignar usuario-usuarios
    Public Function fn_UsuariosClientesReasignar_Get(ByVal Id_Cliente As Integer, Clave_Sucursal As String) As DataTable
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Usuarios_Reasignar_Get", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Cliente", SqlDbType.Int, Id_Cliente)
        CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
        CreaParametro(Cmd, "@Visible", SqlDbType.VarChar, "S")
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function

    Public Function fn_UsuariosClientesReasignar_Update(ByVal Id_Usuario As Integer, ByVal Clave_Sucursal As String, ByVal Clave_Sucursal_Nueva As String, Correo As String)
        Dim resp As Boolean = False
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Usuarios_Reasignar_Update", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)

        CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
        CreaParametro(Cmd, "@Clave_Sucursal_Nueva", SqlDbType.VarChar, Clave_Sucursal_Nueva)
        CreaParametro(Cmd, "@Correo", SqlDbType.VarChar, Correo)
        Try
            EjecutaConsulta(Cmd)
            resp = True
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
        Return resp
    End Function

    Public Function fn_UsuariosClienteCorreoUpdate(ByVal Id_Usuario As Integer, ByVal Correo As String)
        Dim resp As Boolean = False
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Usuarios_Correo_Update", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
        CreaParametro(Cmd, "@Correo", SqlDbType.VarChar, Correo)

        Try
            EjecutaConsulta(Cmd)
            resp = True
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
        Return resp
    End Function

    Public Function fn_UsuariosClientes_Agregar(ByVal Clave_Sucursal As String, ByVal Nombre_Usuario As String, ByVal Nombre_Sesion As String,
                                                      ByVal Tipo As Integer, ByVal Nivel As Short, ByVal Id_Cliente As Integer, ByVal Id_ClienteP As Integer,
                                                      ByVal Password As String, ByVal Valida_IP As String, ByVal Clave_SucursalP As String,
                                                      ByVal Nombre_Cliente As String, ByVal Usuario As Integer, ByVal ModoConsulta As Byte, ByVal Mail As String) As Boolean
        Dim Id_Usuario As String = ""
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Tr As SqlTransaction = CreaTransaccion(Cnn)
        Dim Cmd As SqlCommand
        Try
            Cmd = CreaComando(Tr, "web_Cli_Usuarios_Create")
            CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
            CreaParametro(Cmd, "@Nombre_Usuario", SqlDbType.VarChar, Nombre_Usuario)
            CreaParametro(Cmd, "@Nombre_Sesion", SqlDbType.VarChar, Nombre_Sesion)
            CreaParametro(Cmd, "@Tipo", SqlDbType.TinyInt, Tipo)
            CreaParametro(Cmd, "@Nivel", SqlDbType.TinyInt, Nivel)
            CreaParametro(Cmd, "@Id_ClienteP", SqlDbType.Int, Id_ClienteP)
            CreaParametro(Cmd, "@Password", SqlDbType.VarChar, Password)
            CreaParametro(Cmd, "@Validar_IP", SqlDbType.VarChar, Valida_IP)
            CreaParametro(Cmd, "@Direccion_IP", SqlDbType.VarChar, pIp_Cliente)
            CreaParametro(Cmd, "@Usuario_Registro", SqlDbType.Int, Usuario)
            CreaParametro(Cmd, "@Estacion_Registro", SqlDbType.VarChar, "")
            CreaParametro(Cmd, "@Status", SqlDbType.VarChar, "A")
            CreaParametro(Cmd, "@SucPro", SqlDbType.VarChar, Clave_SucursalP)
            CreaParametro(Cmd, "@Nombre_Cliente", SqlDbType.VarChar, Nombre_Cliente)
            CreaParametro(Cmd, "@Mail", SqlDbType.VarChar, Mail)
            CreaParametro(Cmd, "Modo_Consulta", SqlDbType.TinyInt, ModoConsulta)
            Id_Usuario = EjecutarScalarT(Cmd)

            Cmd = CreaComando(Tr, "Web_Cli_UsuariosD_Create")
            CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
            CreaParametro(Cmd, "@Id_SucursalP", SqlDbType.VarChar, Clave_SucursalP)
            CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
            EjecutarNonQueryT(Cmd)

            Tr.Commit()
            Cmd.Dispose()
            Cnn.Dispose()
            Return True
        Catch Ex As Exception
            Tr.Rollback()
            Cmd.Dispose()
            Cnn.Dispose()
            TrataEx(Ex)
            Return False
        End Try
    End Function

    Public Function fn_UsuariosClientesContra_Reiniciar(ByVal Id_Usuario As Integer, ByVal Password As String) As Integer
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Cli_UsuariosContra_Restart", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
        CreaParametro(Cmd, "@Password", SqlDbType.VarChar, Password)

        Return EjecutaNonQuery(Cmd)
    End Function

    Public Function fn_UsuariosClientes_Status(ByVal Id_Usuario As Integer, ByVal Status As String) As Integer
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Cli_Usuarios_Status", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
        CreaParametro(Cmd, "@Status", SqlDbType.VarChar, Status)

        Return EjecutaNonQuery(Cmd)
    End Function

    Public Function fn_UsuariosClientes_StatusBaja(ByVal Id_Usuario As Integer, ByVal Status As String, ByVal Usuario_Baja As Integer) As Integer
        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Cli_Usuarios_Status", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
        CreaParametro(Cmd, "@Status", SqlDbType.VarChar, Status)
        'CreaParametro(Cmd, "@Usuario_Baja", SqlDbType.Int, Usuario_Baja)
        'CreaParametro(Cmd, "@Estacion_Baja", SqlDbType.VarChar, "")
        Return EjecutaNonQuery(Cmd)
    End Function

    Public Function fn_Get_Session(ByVal Clave_Sucursal As String) As String
        Dim resp As String = ""

        Try
            Dim Cnn As New SqlConnection(Session("ConexionCentral"))
            Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Cli_Get_Session", CommandType.StoredProcedure, Cnn)
            CreaParametro(Cmd, "@Clave_Corp", SqlDbType.VarChar, Clave_Sucursal)
            Dim dt As DataTable = EjecutaConsulta(Cmd)

            resp = dt.Rows(0)("Session_ID")
        Catch ex As Exception

        End Try

        Return resp

    End Function

    Public Function fn_Upd_ContadorSession(ByVal Clave_Sucursal As String) As Boolean
        Dim resp As Boolean = False
        Try
            Dim Cnn As New SqlConnection(Session("ConexionCentral"))
            Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Cli_Upd_Session", CommandType.StoredProcedure, Cnn)
            CreaParametro(Cmd, "@Clave_Corp", SqlDbType.VarChar, Clave_Sucursal)
            EjecutaConsulta(Cmd)
            resp = True
        Catch ex As Exception

        End Try
        Return resp
    End Function
#End Region

#Region "Privilegios Usuarios"
    Public Function fn_Opciones_Consultar() As DataTable

        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Opciones_Get", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, pId_Usuario)
        CreaParametro(Cmd, "@Heredable", SqlDbType.VarChar, "S")
        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_OpcionesClientes_Consultar(ByVal Id_Menu As String, ByVal Id_Usuario_Seleccionado As Integer) As DataTable

        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("web_Permisos_GetOpciones", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Menu", SqlDbType.Int, Id_Menu)
        CreaParametro(Cmd, "@IdUsuario", SqlDbType.Int, Id_Usuario_Seleccionado)

        Try
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ClientesPermisos_Agregar(ByVal Id_Usuario As Integer, ByVal Id_Opcion As Integer, Optional ByVal Tipo As String = "") As Integer

        Dim Cnn As New SqlConnection(Session("ConexionCentral"))
        Dim Cmd As SqlClient.SqlCommand = cn_Datos.Crea_Comando("Web_Cli_Permisos_Delete_Create", CommandType.StoredProcedure, Cnn)
        CreaParametro(Cmd, "@Id_Usuario", SqlDbType.Int, Id_Usuario)
        CreaParametro(Cmd, "@Id_Opcion", SqlDbType.Int, Id_Opcion)
        If Tipo = "A" Then CreaParametro(Cmd, "@Tipo", SqlDbType.VarChar, Tipo)

        Try
            Return EjecutaNonQuery(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Alertas"

    Public Function fn_AlertasGeneradas_Guardar(ByVal SucursalId As Integer, ByVal UsuarioId As Integer, ByVal EstacioN As String, ByVal Clave_AlertaTipo As String, ByVal Detalles As String) As Boolean
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_AlertasGeneradas_Create")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
            CreaParametro(cmd, "@Clave_AlertaTipo", SqlDbType.VarChar, Clave_AlertaTipo)
            CreaParametro(cmd, "@Detalles", SqlDbType.Text, Detalles)
            CreaParametro(cmd, "@Id_EmpleadoGenera", SqlDbType.Int, UsuarioId)
            CreaParametro(cmd, "@Estacion_Genera", SqlDbType.VarChar, EstacioN)
            CreaParametro(cmd, "@Tipo_Alerta", SqlDbType.Int, 1)   'Alerta por Incidencia
            EjecutaNonQuery(cmd)
            Return True
        Catch ex As Exception
            TrataEx(ex)
            Return False
        End Try
    End Function

    Public Function fn_AlertasGeneradas_ObtenerMails(ByVal Clave_AlertaTipo As String) As DataTable
        Try

            Dim cmd As SqlCommand = CreaComando("Cat_AlertasDestinos_GetMail")
            CreaParametro(cmd, "@Clave_AlertaTipo", SqlDbType.VarChar, Clave_AlertaTipo)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            'TrataEx(ex, SucursalId, UsuarioID)
            Return Nothing
        End Try
    End Function
#End Region

#Region "Tickets"
    '   @Descripcion Text,
    '@Tipo Numeric =0,
    '@Id_Rubro int=0,
    '@Nombre_Sucursal Varchar(100)='',  
    '   @Usuario_Sucursal Varchar (60)=''  
    Public Function fn_Mardar_Ticket(ByVal Descripcion As String, ByVal Tipo As Integer, ByVal Id_Rubro As Integer, ByVal Nombre_Sucursal As String, ByVal Usuario_Sucursal As String) As Boolean
        Dim resp As Boolean = False
        Try
            Dim Cnn As New SqlConnection(Session("ConexionCentral"))
            Dim Cmd As SqlCommand = cn_Datos.Crea_Comando("Cli_Ticket_Create", CommandType.StoredProcedure, Cnn)
            CreaParametro(Cmd, "@Descripcion", SqlDbType.VarChar, Descripcion)
            CreaParametro(Cmd, "@Tipo", SqlDbType.Int, Tipo)
            CreaParametro(Cmd, "@Id_Rubro", SqlDbType.Int, Id_Rubro)
            CreaParametro(Cmd, "@Nombre_Sucursal", SqlDbType.VarChar, Nombre_Sucursal)
            CreaParametro(Cmd, "@Usuario_Sucursal", SqlDbType.VarChar, Usuario_Sucursal)
            Dim IdTicket As Integer = EjecutaScalar(Cmd)
            resp = True
        Catch ex As Exception

        End Try
        Return resp
    End Function

    Public Function fn_Lista_Ticket(ByVal Usuario_Sucursal As String, ByVal Clave_Sucursal As String, ByVal Clave_Corp As String) As DataTable
        Dim dt As DataTable = Nothing
        Try
            Dim Cnn As New SqlConnection(Session("ConexionCentral"))
            Dim Cmd As SqlCommand = cn_Datos.Crea_Comando("Cli_Ticket_GET", CommandType.StoredProcedure, Cnn)
            CreaParametro(Cmd, "@Usuario_Sucursal", SqlDbType.VarChar, Usuario_Sucursal)
            CreaParametro(Cmd, "@Clave_Sucursal", SqlDbType.VarChar, Clave_Sucursal)
            CreaParametro(Cmd, "@Clave_Corp", SqlDbType.VarChar, Clave_Corp)
            dt = EjecutaConsulta(Cmd)
        Catch ex As Exception

        End Try
        Return dt
    End Function

    Public Function fn_Detalle_Ticket(ByVal Ticket) As DataTable
        Dim dt As DataTable = Nothing
        Try
            Dim Cnn As New SqlConnection(Session("ConexionCentral"))
            Dim Cmd As SqlCommand = cn_Datos.Crea_Comando("Cli_Ticket_Descripcion", CommandType.StoredProcedure, Cnn)
            CreaParametro(Cmd, "@Id_Ticket", SqlDbType.VarChar, Ticket)
            dt = EjecutaConsulta(Cmd)
        Catch ex As Exception

        End Try
        Return dt
    End Function
#End Region

#Region "Proyecto Remisiones"
    'Proyecto remisiones 23/05/2020
    Public Function fn_ConsultaR1(ByVal folio As Integer) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Re_reporte1")
            CreaParametro(cmd, "@id_folio", SqlDbType.Int, folio)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaR2(ByVal Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Re_reporte2")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaR3(ByVal Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Re_reporte3")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaR4(ByVal Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Re_reporte4")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_ConsultaR5(ByVal Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Re_reporte5")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaR6(ByVal Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Re_reporte6")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaMonrdas() As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Cat_Monedas_Get")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, CInt(Session("Id_Sucursal")))
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaEnvases() As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("CAT_TipoEnvase_GET")
            'CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, CInt(Session("Id_Sucursal")))
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_PuntosActivosxC() As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("ObtenerPuntoWeb_Get")
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.VarChar, pId_Cliente)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaTipoCambio() As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Get_TipocambioActual")
            'CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Int, CInt(Session("Id_Sucursal")))
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_GuardarRemisionWeb(tbl_monedas As DataTable, tbl_envases As DataTable, Importe As Double, Id_punto As Long, Importes() As Double, Comentarios As String, EnvasesSN As Integer) As Boolean
        '  Dim cnn As New SqlConnection(Session("cnx_siac"))
        If tbl_envases Is Nothing Then tbl_envases = New DataTable
        Dim tr As SqlTransaction = CreaTransaccion(CreaConexion())

        Try
            Dim cmd As SqlCommand = CreaComando(tr, "NumeroRemisionNew")
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.VarChar, pClaveSIAC)
            Dim Remision As String = EjecutaConsulta(cmd).Rows(0)(0).ToString
            If Not IsNumeric(Remision) Then
                Throw New Exception()
            End If
            cmd = CreaComando(tr, "Insert_RemisionesWeb")
            CreaParametro(cmd, "@Cliente", SqlDbType.Int, pId_Cliente)
            CreaParametro(cmd, "@Envases", SqlDbType.Int, tbl_envases.Rows.Count)
            CreaParametro(cmd, "@Importe", SqlDbType.VarChar, Importe)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.VarChar, Id_punto)
            CreaParametro(cmd, "@NumeroR", SqlDbType.VarChar, Remision)
            CreaParametro(cmd, "@Comentarioas", SqlDbType.VarChar, Comentarios)
            CreaParametro(cmd, "@EnvasesSN", SqlDbType.Int, EnvasesSN)
            Dim Id_RemisionWeb As Long = EjecutaScalar(cmd)
            For Each r As DataRow In tbl_monedas.Rows
                cmd = CreaComando(tr, "Insert_RemisionesWebD")
                CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, Id_RemisionWeb)
                CreaParametro(cmd, "@Id_Moneda", SqlDbType.Decimal, r("Id_Moneda"))
                CreaParametro(cmd, "@Importe_E", SqlDbType.VarChar, r("Efectivo"))
                CreaParametro(cmd, "@Importe_D", SqlDbType.VarChar, r("Documentos"))
                EjecutaNonQuery(cmd)
            Next
            For Each r As DataRow In tbl_envases.Rows
                cmd = CreaComando(tr, "Insert_EnvasesWeb")
                CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, Id_RemisionWeb)
                CreaParametro(cmd, "@Tipo_E", SqlDbType.Int, CLng(r("Id_TipoE")))
                CreaParametro(cmd, "@Numero", SqlDbType.VarChar, r("Numero"))
                EjecutaNonQuery(cmd)

            Next
            cmd = CreaComando(tr, "Insert_RemisionesWebImportes")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, Id_RemisionWeb)
            CreaParametro(cmd, "@Mil", SqlDbType.VarChar, Importes(0))
            CreaParametro(cmd, "@Quinientos", SqlDbType.VarChar, Importes(1))
            CreaParametro(cmd, "@Docientos", SqlDbType.VarChar, Importes(2))
            CreaParametro(cmd, "@Cien", SqlDbType.VarChar, Importes(3))
            CreaParametro(cmd, "@Cincuenta", SqlDbType.VarChar, Importes(4))
            CreaParametro(cmd, "@Veinte", SqlDbType.VarChar, Importes(5))
            CreaParametro(cmd, "@MVeinte", SqlDbType.VarChar, Importes(6))
            CreaParametro(cmd, "@MDiez", SqlDbType.VarChar, Importes(7))
            CreaParametro(cmd, "@MCinco", SqlDbType.VarChar, Importes(8))
            CreaParametro(cmd, "@MDos", SqlDbType.VarChar, Importes(9))
            CreaParametro(cmd, "@MUno", SqlDbType.VarChar, Importes(10))
            CreaParametro(cmd, "@MPCincuenta", SqlDbType.VarChar, Importes(11))
            CreaParametro(cmd, "@Mixto", SqlDbType.VarChar, Importes(12))
            'CreaParametro(cmd, "@MPDiez", SqlDbType.VarChar, Importes(13))
            'CreaParametro(cmd, "@MPCinco", SqlDbType.VarChar, Importes(14))

            EjecutaNonQuery(cmd)
            tr.Commit()
            Return True
        Catch ex As Exception
            tr.Rollback()
            TrataEx(ex)
            Return False
        End Try
    End Function
    Public Function fn_ConsultaEnvasesWeb(Envase As String, Id_Punto As Integer) As Integer
        Try
            Dim cmd As SqlCommand = CreaComando("Envases_WebCheck")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Int, Id_Punto)
            CreaParametro(cmd, "@Envases", SqlDbType.VarChar, Envase)
            Return EjecutaConsulta(cmd).Rows(0)(0).ToString
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_AutorizarRemisionesGet(Desde As Date, Hasta As Date, Status As String, Id_Cliente As String) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Get_RemisionesValidar")
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.VarChar, Id_Cliente)
            CreaParametro(cmd, "@FechaDesde", SqlDbType.DateTime, Desde)
            CreaParametro(cmd, "@FechaHasta", SqlDbType.DateTime, Hasta)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, Status)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    Public Function fn_AutorizarR(ByVal Id_Remision As Integer) As Boolean
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionesWebValidar")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaNonQuery(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    Public Function fn_TV_Remisionesweb_GetEnvases(Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("TV_Remisionesweb_GetEnvases")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    Public Function fn_Tv_Remisionesweb_GetMonedas(Id_Remision As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_Remisionesweb_GetMonedas")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    Public Function fn_Tv_RemisionesWeb_Delete(ByVal Id_Remision As Long) As Boolean
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionWeb_Delete")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            Return EjecutaNonQuery(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    Public Function fn_Tv_Remisionesweb_GetImportes(Id_Remision As Long) As DataTable
        Try
            Dim tbl = New DataTable
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Obtener_ImportesWeb_Get")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            tbl = EjecutaConsulta(cmd)
            If (tbl IsNot Nothing AndAlso EjecutaConsulta(cmd).Rows.Count > 0) Then
                Return tbl
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
        Return Nothing
    End Function
    Public Function fn_Tv_ModificarNumeros(ByVal Id_Remision As Long, NumeorOld As String, NumeroNew As String) As Boolean
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionesWeb_Cambiar_NumeroEnvase")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            CreaParametro(cmd, "@Numero_Old", SqlDbType.VarChar, NumeorOld)
            CreaParametro(cmd, "@Numero_New", SqlDbType.VarChar, NumeroNew)
            Return EjecutaNonQuery(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try

    End Function
    Public Function fn_ImportesATMD(Id_Remision As Long) As DataTable
        Try
            Dim tbl = New DataTable
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Den_cajRemiciones")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, Id_Remision)
            tbl = EjecutaConsulta(cmd)
            If (tbl IsNot Nothing AndAlso EjecutaConsulta(cmd).Rows.Count > 0) Then
                Return tbl
            End If
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
        Return Nothing
    End Function
    Public Function fn_CortesConsultar(ByVal Desde As Date, ByVal Hasta As Date, ByVal Id_CajaBancaria As Integer, Id_Cajero As Long) As DataTable
        Dim lc_tabla As New DataTable
        Try
            Dim Cmd As SqlClient.SqlCommand = cn_Datos.CreaComando("Obtener_CortesTotales")
            CreaParametro(Cmd, "@Fecha_Desde", SqlDbType.Date, Desde)
            CreaParametro(Cmd, "@Fecha_Hasta", SqlDbType.Date, Hasta)
            CreaParametro(Cmd, "@Id_CajaBancaria", SqlDbType.Int, Id_CajaBancaria)
            CreaParametro(Cmd, "@Id_Cajero", SqlDbType.VarChar, Id_Cajero)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_ConsultaIDRemision(Numero As String) As DataTable
        Dim lc_tabla As New DataTable
        Try
            Dim Cmd As SqlClient.SqlCommand = cn_Datos.CreaComando("Obtener_IDRemision")
            CreaParametro(Cmd, "@Numero", SqlDbType.VarChar, Numero)
            Return EjecutaConsulta(Cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_DetalleEnvio(Id_Punto As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionesWeb_Envases")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.VarChar, Id_Punto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    Public Function fn_TvRemisionesWeb_V(Id_Punto As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionesWeb_V")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.VarChar, Id_Punto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_Tv_EnvasesWeb_Mod_Portal(Id_RemsionWeb As Decimal) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_EnvaseWeb_Mod_Portal")
            CreaParametro(cmd, "@Id_RemsionWeb", SqlDbType.VarChar, Id_RemsionWeb)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_Tv_RemisionesWebD_Mod_Portal(Id_RemsionWeb As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionesWebD_Mod_Portal")
            CreaParametro(cmd, "@Id_RemsionWeb", SqlDbType.VarChar, Id_RemsionWeb)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function
    Public Function fn_Tv_RemisionesWeb_Mod_Portal(Id_Punto As Long) As DataTable
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlCommand = CreaComando("Tv_RemisionesWeb_Mod_Portal")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.VarChar, Id_Punto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            TrataEx(ex)
            Return Nothing
        End Try
    End Function

    ''Funciones para remisiones Digitales capturadas desde el portal
    Public Shared Function EstatusPuntos(ByVal model As PuntoRequest) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Tv_Puntos_Read")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.IdPunto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            'TrataEx(ex)
            Throw ex
        End Try
    End Function
    Public Shared Function consultarOrigenDestino(ByVal model As RemisionRequest) As Boolean
        Try
            Dim cmd As SqlCommand = CreaComando("Tv_PuntosOrigenDestino_Get")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.PuntoRemision.IdPunto)
            CreaParametro(cmd, "@Id_Cia", SqlDbType.Int, model.Remision.IdCia)
            Dim dt As DataTable = EjecutaConsulta(cmd)

            If dt IsNot Nothing Then

                If dt.Rows.Count > 0 Then
                    model.Remision.ClienteOrigen = Convert.ToDecimal(dt.Rows(0)("Cliente_Origen"))
                    model.Remision.ClienteDestino = Convert.ToDecimal(dt.Rows(0)("Cliente_Destino"))
                    model.Remision.IdClientP = Convert.ToDecimal(dt.Rows(0)("Id_ClienteP"))
                End If
            End If

        Catch ex As Exception
            model.Remision.ClienteOrigen = 0
            model.Remision.ClienteDestino = 0
            model.Remision.IdClientP = 0
            Throw ex
        End Try

        Return True
    End Function

    Public Shared Function ExisteRemision(ByVal model As RemisionRequest) As Integer
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_Remisiones_Existe")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Decimal, model.IdSucursal)
            CreaParametro(cmd, "@Numero_Remision", SqlDbType.Decimal, model.Remision.NumeroRemision)
            CreaParametro(cmd, "@Id_Cia", SqlDbType.Decimal, model.Remision.IdCia)

            Return EjecutaConsulta(cmd).Rows.Count

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function Obtener_Comentarios(ByVal model As RemisionRequest) As String
        Dim Coments As String = ""
        Try

            Dim cmd As SqlCommand = CreaComando("Tv_RemisonesWebcomentarios_Get")
            CreaParametro(cmd, "@Id_RemisionWeb", SqlDbType.VarChar, model.Remision.IdRemisionWeb)
            Dim dt As DataTable = EjecutaConsulta(cmd)



            If dt.Rows.Count > 0 Then
                Coments = dt.Rows(0)("Comentarios").ToString()
            End If

            Return Coments
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function insertar(ByVal model As RemisionRequest, ByVal tr As SqlTransaction) As Decimal

        Dim IdRemision As Decimal = 0

        Try
            Dim cmd As SqlCommand = CreaComando(tr, "CAT_Remisiones_Create")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Decimal, model.IdSucursal)
            CreaParametro(cmd, "@Numero_Remision", SqlDbType.Decimal, model.Remision.NumeroRemision)
            CreaParametro(cmd, "@Envases", SqlDbType.Decimal, model.Remision.Envases)
            CreaParametro(cmd, "@EnvasesSN", SqlDbType.Decimal, model.Remision.EnvasesSN)
            CreaParametro(cmd, "@Moneda_Local", SqlDbType.Decimal, model.Remision.MonedaLocal)
            CreaParametro(cmd, "@Id_Cia", SqlDbType.Decimal, model.Remision.IdCia)
            CreaParametro(cmd, "@Usuario", SqlDbType.Decimal, model.IdUsuario)
            CreaParametro(cmd, "@ImporteTotal", SqlDbType.Decimal, model.Remision.ImporteTotal)
            CreaParametro(cmd, "@Morralla", SqlDbType.VarChar, model.Remision.Morralla)
            CreaParametro(cmd, "@Id_CiaProp", SqlDbType.Decimal, model.Remision.IdCiaPropia)
            CreaParametro(cmd, "@Hora_Remision", SqlDbType.VarChar, model.Remision.HoraRemision)
            CreaParametro(cmd, "@Cliente_Origen", SqlDbType.Decimal, model.Remision.ClienteOrigen)
            CreaParametro(cmd, "@Cliente_Destino", SqlDbType.Decimal, model.Remision.ClienteDestino)
            CreaParametro(cmd, "@Id_ClienteP", SqlDbType.Decimal, model.Remision.IdClientP)
            CreaParametro(cmd, "@Estacion_Registro", SqlDbType.VarChar, model.EstacionNombre)
            IdRemision = Convert.ToDecimal(EjecutaScalar(cmd))
            If IdRemision = 0 Then Throw New Exception("No se pudo guardar la remisión")
        Catch ex As Exception
            Throw ex
        End Try

        Return IdRemision
    End Function

    Public Shared Sub insertarRemisionD(ByVal model As RemisionRequest, ByVal tr As SqlTransaction)

        Try

            For Each remision As RemisionD In model.RemisionesD
                Dim cmd As SqlCommand = CreaComando(tr, "CAT_RemisionesD_Create")
                CreaParametro(cmd, "@Id_Remision", SqlDbType.Decimal, model.Remision.IdRemision)
                CreaParametro(cmd, "@Id_Moneda", SqlDbType.Decimal, remision.IdMoneda)
                CreaParametro(cmd, "@Importe_Efectivo", SqlDbType.Money, remision.ImporteEfectivo)
                CreaParametro(cmd, "@Importe_Documentos", SqlDbType.Money, remision.ImporteDocumentos)
                If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudieron guardar todas las remisiones.")
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Sub insertarFirmas(ByVal model As RemisionRequest, ByVal tr As SqlTransaction)
        Try
            Dim cmd As SqlCommand = CreaComando(tr, "Cat_Firmas_Create")
            CreaParametro(cmd, "@Id_Remision", SqlDbType.Int, model.Remision.IdRemision)
            CreaParametro(cmd, "@Usuario", SqlDbType.VarChar, model.ClienteSesion.IdUsuarioCliente)
            CreaParametro(cmd, "@Nombre", SqlDbType.VarChar, model.ClienteSesion.NombreUsuarioCliente)
            CreaParametro(cmd, "@Nombre_Sesion", SqlDbType.VarChar, model.ClienteSesion.Sesion)
            If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudo crear la Firma.")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Sub insertarEnvases(ByVal model As RemisionRequest, ByVal tr As SqlTransaction)
        Try

            For Each envase As Envase In model.Envases
                Dim cmd As SqlCommand = CreaComando(tr, "CAT_Envases_Create")
                CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Decimal, model.IdSucursal)
                CreaParametro(cmd, "@Id_Remision", SqlDbType.Decimal, model.Remision.IdRemision)
                CreaParametro(cmd, "@Id_TipoE", SqlDbType.Int, envase.IdTipoEnvase)
                CreaParametro(cmd, "@Numero", SqlDbType.VarChar, envase.Numero)
                CreaParametro(cmd, "@Modo_Captura", SqlDbType.TinyInt, model.ModoCaptura)
                CreaParametro(cmd, "@Usuario_Registro", SqlDbType.VarChar, model.IdUsuario)
                If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudieron guardar todas las remisiones")
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Sub EditarPuntoRemision(ByVal model As RemisionRequest, ByVal tr As SqlTransaction)
        Try
            Dim cmd As SqlCommand = CreaComando(tr, "Tv_Puntos_AfectarRem")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.PuntoRemision.IdPunto)
            CreaParametro(cmd, "@Importe", SqlDbType.Money, model.Remision.ImporteTotal)
            CreaParametro(cmd, "@Envases", SqlDbType.Decimal, model.Remision.Envases)
            CreaParametro(cmd, "@Remisiones", SqlDbType.Decimal, 1)
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Decimal, model.IdUsuario)
            CreaParametro(cmd, "@Id_Remision", SqlDbType.Decimal, model.Remision.IdRemision)
            CreaParametro(cmd, "@Modo_Afecta", SqlDbType.Decimal, 2)
            If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudo editar el punto")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Function EditarCantidadRemision(ByVal model As RemisionRequest, ByVal tr As SqlTransaction) As Integer
        Try
            Dim cmd As SqlCommand = CreaComando(tr, "Tv_Puntos_UpdateCantRemC")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.PuntoRemision.IdPunto)
            If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudo editar el punto")
        Catch ex As Exception
            Throw ex
        End Try

        Return 1
    End Function
    Public Shared Sub terminarRemisionWeb(ByVal model As RemisionRequest, ByVal Coments As String, ByVal tr As SqlTransaction)
        Dim cmd As SqlCommand = Nothing
        Try

            If model.Remision.IdRemisionWeb > 0 Then
                cmd = CreaComando(tr, "StatusRemisionWeb_Update")
                CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, model.Remision.IdRemision)
                CreaParametro(cmd, "@Id_RemisionWeb", SqlDbType.VarChar, model.Remision.IdRemisionWeb)
                If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudo editar la Remisión Web")
                cmd = CreaComando(tr, "Tv_RemisonesWebReal")
                CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, model.Remision.IdRemision)
                CreaParametro(cmd, "@Id_RemisionWeb", SqlDbType.VarChar, model.Remision.IdRemisionWeb)
                If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudo editar la Remisión Web")
                cmd = CreaComando(tr, "Tv_RemisonesWebcomentarios")
                CreaParametro(cmd, "@Id_Remision", SqlDbType.VarChar, model.Remision.IdRemision)
                CreaParametro(cmd, "@Coments", SqlDbType.VarChar, Coments)
                If EjecutaNonQuery(cmd) = 0 Then Throw New Exception("No se pudo editar la Remisión Web")
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Function obtenerMovimientos(ByVal model As RemisionRequest, ByVal tr As SqlTransaction) As DataTable
        Dim cmd As SqlCommand = Nothing

        Try
            cmd = CreaComando(tr, "Cat_Movimientos_GetDatos")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Int, model.PuntoRemision.IdPunto)
            Return (EjecutaConsulta(cmd))
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function insertar(ByVal model As RemisionRequest, ByVal tr As SqlTransaction, ByVal movimiento As DataTable, ByVal calculoMovimiento As CalculoMovimiento) As Decimal
        Dim cmd As SqlCommand = Nothing
        Dim IdMovimiento As Decimal = 0

        Try
            cmd = CreaComando(tr, "Cat_Movimientos_CreateMovil")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Decimal, model.IdSucursal)
            CreaParametro(cmd, "@Tipo_Mov", SqlDbType.VarChar, "R")
            CreaParametro(cmd, "@Hora", SqlDbType.VarChar, DateTime.Now.ToString("HH:mm"))
            CreaParametro(cmd, "@Usuario_Registro", SqlDbType.Decimal, model.IdUsuario)
            CreaParametro(cmd, "@Id_Cliente", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Id_Cliente")))
            CreaParametro(cmd, "@Id_Ruta", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Id_Ruta")))
            CreaParametro(cmd, "@Id_Precio", SqlDbType.Int, Convert.ToDecimal(movimiento.Rows(0)("Id_Precio")))
            CreaParametro(cmd, "@Id_CR", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Id_CR")))
            CreaParametro(cmd, "@Id_EE", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Id_EE")))
            CreaParametro(cmd, "@Id_KM", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Id_KM")))
            CreaParametro(cmd, "@Cantidad_Remisiones", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Cantidad_Remisiones")))
            CreaParametro(cmd, "@Cantidad_Envases", SqlDbType.Decimal, Convert.ToDecimal(movimiento.Rows(0)("Cantidad_Envases")))
            CreaParametro(cmd, "@Importe", SqlDbType.Money, calculoMovimiento.Importe)
            CreaParametro(cmd, "@Miles", SqlDbType.Money, calculoMovimiento.Miles)
            CreaParametro(cmd, "@Envases_Exceso", SqlDbType.Decimal, calculoMovimiento.EnvaseExceso)
            CreaParametro(cmd, "@Kilometros", SqlDbType.Decimal, calculoMovimiento.Kilometros)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.PuntoRemision.IdPunto)
            CreaParametro(cmd, "@Estacion_Captura", SqlDbType.VarChar, model.EstacionNombre)
            CreaParametro(cmd, "@Modo_Captura", SqlDbType.Decimal, model.ModoCaptura)
            IdMovimiento = Convert.ToDecimal(EjecutaScalar(cmd))
        Catch ex As Exception
            Throw ex
        End Try

        Return IdMovimiento
    End Function
    Public Shared Sub insertarD(ByVal model As RemisionRequest, ByVal tr As SqlTransaction, ByVal idMovimiento As Decimal)
        Dim cmd As SqlCommand = Nothing

        Try

            For Each remision As RemisionD In model.RemisionesD
                cmd = CreaComando(tr, "Cat_MovimientosD_Create")
                CreaParametro(cmd, "@Id_Movimiento", SqlDbType.Decimal, idMovimiento)
                CreaParametro(cmd, "@Id_Remision", SqlDbType.Decimal, model.Remision.IdRemision)
                CreaParametro(cmd, "@Status", SqlDbType.VarChar, "A")
                CreaParametro(cmd, "@Pago_Morralla", SqlDbType.VarChar, model.Remision.Morralla)
                If Convert.ToInt32(EjecutaNonQuery(cmd)) <= 0 Then Throw New Exception("No se pudo insertar el detalle del movimiento")
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Sub enRuta(ByVal model As RemisionRequest, ByVal tr As SqlTransaction)
        Dim cmd As SqlCommand = Nothing

        Try
            cmd = CreaComando(tr, "tv_Puntos_SoloStatus")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Int, model.PuntoRemision.IdPunto)
            CreaParametro(cmd, "@Status", SqlDbType.VarChar, "RU")
            CreaParametro(cmd, "@Comentarios_Recoleccion", SqlDbType.VarChar, model.ComentarioRecolleccion, True)
            If Convert.ToInt32(EjecutaNonQuery(cmd)) = 0 Then Throw New Exception("No se pudo afectar el punto")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Function obtenerPuntoUtilizaCasetRemisionDigital(ByVal model As PuntoRequest, ByVal tr As SqlTransaction) As DataTable
        Dim cmd As SqlCommand = Nothing

        Try
            cmd = CreaComando(tr, "Tv_Punto_UtilizaCasetRemisionDigital")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.IdPunto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function obtenerNotificacion(ByVal model As RemisionRequest) As DataTable
        Dim cmd As SqlCommand = Nothing
        Try
            cmd = CreaComando("Tv_Puntos_ObtenerNotificacion")
            CreaParametro(cmd, "@Id_Punto", SqlDbType.Decimal, model.PuntoRemision.IdPunto)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function obtenerRemisionWebImporte(ByVal model As RemisionRequest) As DataTable
        Dim cmd As SqlCommand = Nothing

        Try
            cmd = CreaComando("NotificacionImportesWebTraslado")
            CreaParametro(cmd, "@NumeroRemision", SqlDbType.VarChar, model.Remision.NumeroRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function obtenerEnvasess(ByVal model As RemisionRequest) As DataTable
        Dim cmd As SqlCommand = Nothing
        Try
            cmd = CreaComando("NotificacionEnvasesTraslado")
            CreaParametro(cmd, "@NumeroRemision", SqlDbType.VarChar, model.Remision.NumeroRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function obtenerImporteMoneda(ByVal model As RemisionRequest) As DataTable
        Dim cmd As SqlCommand = Nothing
        Try
            cmd = CreaComando("NotificacionMonedaTraslado")
            CreaParametro(cmd, "@NumeroRemision", SqlDbType.VarChar, model.Remision.NumeroRemision)
            Return EjecutaConsulta(cmd)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function obtenerSucursal() As DataTable
        Dim cmd As SqlCommand = Nothing

        Try

            cmd = CreaComando("Cat_ParametrosL_Read")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Decimal, 1)
            Dim dt As DataTable = EjecutaConsulta(cmd)
            If dt.Rows.Count = 0 Then Throw New Exception("Ocurrió un errro al consultar la sucursal")
            Return (dt)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function fn_ObtenTipoMoneda(ByVal IdMoneda As Integer) As String
        Dim cmd As SqlCommand = Nothing
        Try
            cmd = CreaComando("Cat_Monedas_ReadTipoCambio")
            Dim dt_lc As New DataTable

            CreaParametro(cmd, "@Id_Moneda", SqlDbType.BigInt, IdMoneda)

            dt_lc = cn_Datos.EjecutaConsulta(cmd)

            If dt_lc.Rows.Count > 0 Then
                Return (dt_lc.Rows(0)(1).ToString)
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region


End Class


Public Class CalculoMovimiento
    Public Property Importe As Decimal
    Public Property Miles As Decimal
    Public Property EnvaseExceso As Decimal
    Public Property Kilometros As Decimal
End Class
