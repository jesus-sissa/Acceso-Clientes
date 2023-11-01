Imports PortalSIAC
Imports System.Data
Imports System.IO

Public Class BasePage
    Inherits Page

#Region "Variables Privadas"
    Public cn As Cn_Portal
    Private _Alerta As Alerta
    Private _Form As Control
    Public re As Cls_Remision

#End Region

#Region "Eventos"

    Private Sub Page_PreLoad(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreLoad

        cn = New Cn_Portal(Session, Request)
        _Form = Me.Controls(0).FindControl("form1")
        _Alerta = Page.LoadControl("~\webControls\Alerta.ascx")
        _Form.Controls.Add(_Alerta)

        Session("ConexionCentral") = ConfigurationManager.ConnectionStrings("ConexionCentral").ConnectionString
        Session("cnx_siac") = ConfigurationManager.ConnectionStrings("ConexionSiac").ConnectionString
        If pId_Usuario = 0 Then
            If Request.Url.AbsolutePath.EndsWith("/Login/Login.aspx") Then
                Exit Sub
            Else
                System.Web.Security.FormsAuthentication.SignOut()
                System.Web.Security.FormsAuthentication.RedirectToLoginPage()
            End If
        End If

        If Request.Url.AbsolutePath.Contains("/CambiarContrasena.aspx") Then
            Exit Sub
        End If

        If Request.Url.AbsolutePath.Contains("/ValidarTripulacionAcuse.aspx") Then
            Exit Sub
        End If

        If Not cn.fn_ValidaPermisos() Then
            System.Web.Security.FormsAuthentication.SignOut()
            Session.Clear()
            'Session.Abandon()
            Response.Redirect("~\Login\Login.aspx")
        End If

    End Sub

#End Region

#Region "Propiedades"
    Public Property Data_Remision() As DataTable
        Get
            Return Session("Data_Remision")
        End Get
        Set(value As DataTable)
            Session("Data_Remision") = value
        End Set
    End Property

    Public Property Cnx_siac() As String
        Get
            Return Session("cnx_siac")
        End Get
        Set(value As String)
            Session("cnx_siac") = value
        End Set
    End Property
    Public Property pId_Punto() As Long
        Get
            Dim res As Long = 0
            Integer.TryParse(Session("pId_Punto"), res)
            Return res
        End Get
        Set(ByVal value As Long)
            Session("pId_Punto") = value
        End Set
    End Property
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

    Public Property pId_ClienteOriginal() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("IdClienteOriginal"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("IdClienteOriginal") = value
        End Set
    End Property

    Public Property pId_ClienteP() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("IdClienteP"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("IdClienteP") = value
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

    Public Property pClave_Corporativo() As String
        Get
            Return Session("Clave_Corporativo")

        End Get
        Set(ByVal value As String)
            Session("Clave_Corporativo") = value
        End Set
    End Property

    Public Property pClave_SucursalPropia() As String
        Get
            Return Session("Clave_SucursalPropia")

        End Get
        Set(ByVal value As String)
            Session("Clave_SucursalPropia") = value
        End Set
    End Property

    Public Property pId_Login() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("Id_Login"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("Id_Login") = value
        End Set
    End Property
    Public Property Num_Remision() As String
        Get
            Dim res As Long = 0
            Long.TryParse(Session("Num_r"), res)
            Return Session("Num_r")
        End Get
        Set(value As String)
            Session("Num_r") = value
        End Set
    End Property
    Public Property Tipo_Remision() As String
        Get
            Return Session("TipoR")
        End Get
        Set(value As String)
            Session("TipoR") = value
        End Set
    End Property
    Public Property Clave_Ruta() As String
        Get
            Return Session("Clave_Ruta")
        End Get
        Set(value As String)
            Session("Clave_Ruta") = value
        End Set
    End Property
    Public Property Unidad_Ruta() As String
        Get
            Return Session("Unidad_Ruta")
        End Get
        Set(value As String)
            Session("Unidad_Ruta") = value
        End Set
    End Property
    Public Property Envases_Remision() As String
        Get
            Return Session("Envases_Remision")
        End Get
        Set(value As String)
            Session("Envases_Remision") = value
        End Set
    End Property
    Public Property Env_B() As Integer
        Get
            Return Session("Env_B")
        End Get
        Set(value As Integer)
            Session("Env_B") = value
        End Set
    End Property
    Public Property Env_M() As Integer
        Get
            Return Session("Env_M")
        End Get
        Set(value As Integer)
            Session("Env_M") = value
        End Set
    End Property
    Public Property Env_MIX() As Integer
        Get
            Return Session("Env_MIX")
        End Get
        Set(value As Integer)
            Session("Env_MIX") = value
        End Set
    End Property
    Public Property Mon_Na() As Double
        Get
            Return Session("Mon_Na")
        End Get
        Set(value As Double)
            Session("Mon_Na") = value
        End Set
    End Property
    Public Property Mon_Ex() As Double
        Get
            Return Session("Mon_Ex")
        End Get
        Set(value As Double)
            Session("Mon_Ex") = value
        End Set
    End Property
    Public Property Mon_Otros() As Double
        Get
            Return Session("Mon_Otros")
        End Get
        Set(value As Double)
            Session("Mon_Otros") = value
        End Set
    End Property
    Public ReadOnly Property pIp_Cliente() As String
        Get
            Return Request.UserHostAddress
        End Get
    End Property
    Public Property Id_Cajero() As String
        Get
            Return Session("Id_Cajero")
        End Get
        Set(value As String)
            Session("Id_Cajero") = value
        End Set
    End Property
    Public Property Cliente_origen() As String
        Get
            Return Session("Cliente_origen")
        End Get
        Set(value As String)
            Session("Cliente_origen") = value
        End Set
    End Property
    Public Property Cliente_Destino() As String
        Get
            Return Session("Cliente_Destino")
        End Get
        Set(value As String)
            Session("Cliente_Destino") = value
        End Set
    End Property


    Public ReadOnly Property pEstacion_Cliente() As String
        Get
            Return Request.UserHostName
        End Get
    End Property

    Public Property pTabla(ByVal Clave As String) As DataTable
        Get
            Dim Ds As New DataSet
            If ViewState("TablaXML") = Nothing Then Return Nothing
            Ds.ReadXml(New System.IO.StringReader(ViewState("TablaXML")))
            Return Ds.Tables(Clave)
        End Get
        Set(ByVal value As DataTable)

            Dim Ds As New DataSet

            If (ViewState("TablaXML") <> "") Then Ds.ReadXml(New System.IO.StringReader(ViewState("TablaXML")))

            If Ds.Tables(Clave) IsNot Nothing Then Ds.Tables.Remove(Clave)

            If value Is Nothing Then Exit Property

            value.TableName = Clave
            Ds.Tables.Add(value.Copy)

            ViewState("TablaXML") = Ds.GetXml

        End Set
    End Property

    Public Property pNivel() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("Nivel"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("Nivel") = value
        End Set
    End Property

    Public Property pId_CajaBancaria() As Integer
        Get
            Dim res As Integer = 0
            Integer.TryParse(Session("Id_CajaBancaria"), res)
            Return res
        End Get
        Set(ByVal value As Integer)
            Session("Id_CajaBancaria") = value
        End Set
    End Property

    Public Property pMail_Usuario() As String
        Get
            Dim res As String = ""
            res = Session("MailUsuario")
            Return res
        End Get
        Set(ByVal value As String)
            Session("MailUsuario") = value
        End Set
    End Property

    Public Property pNombre_Cliente() As String
        Get
            Dim res As String = ""
            res = Session("NombreCliente")
            Return res
        End Get
        Set(ByVal value As String)
            Session("NombreCliente") = value
        End Set
    End Property

    Public Property pClave_Sucursal() As String
        Get
            Dim res As String = ""
            res = Session("ClaveSucursal")
            Return res
        End Get
        Set(ByVal value As String)
            Session("ClaveSucursal") = value
        End Set
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
        Set(ByVal value As String)
            Session("ClaveSIAC") = value
        End Set
    End Property
#End Region

#Region "Constantes"

    Public Const ClaveModulo As String = "25"

#End Region

#Region "Metodos"

    Public Sub fn_Alerta(ByVal Mensaje As String)
        _Alerta.Alerta(Mensaje, Theme.ToUpper)
    End Sub

    Public Function fn_MostrarSiempre(ByVal Tabla As DataTable) As DataTable
        For Each col As DataColumn In Tabla.Columns
            col.AllowDBNull = True
        Next

        If Tabla.Rows.Count = 0 Then Tabla.Rows.InsertAt(Tabla.NewRow(), 0)
        Return Tabla
    End Function

    Public Shared Function fn_CreaGridVacio(ByVal campos As String) As DataTable
        Dim dt As New DataTable
        Dim column As New DataColumn

        Dim arr() As String = campos.Split(",")
        For x As Integer = 0 To arr.Length - 1
            dt.Columns.Add(New DataColumn(arr(x).Trim, GetType(String)))
        Next

        Dim Dr As DataRow = dt.NewRow
        For x As Integer = 0 To arr.Length - 1
            Dr(arr(x).Trim) = ""
        Next

        dt.Rows.Add(Dr)

        Return dt
    End Function

    Public Function fn_LlenarDropDownVacio(ByRef ddl As DropDownList) As DataTable
        ddl.DataSource = Nothing

        Dim dt_Vacio As DataTable = New DataTable
        Dim dr_FilaNueva As DataRow = dt_Vacio.NewRow
        dt_Vacio.Columns.Add(ddl.DataValueField)
        dt_Vacio.Columns.Add(ddl.DataTextField)

        dr_FilaNueva(ddl.DataValueField) = "0"
        dr_FilaNueva(ddl.DataTextField) = "Seleccione..."

        dt_Vacio.Rows.Add(dr_FilaNueva)

        ddl.DataSource = dt_Vacio
        ddl.DataBind()
        Return dt_Vacio
    End Function

    Public Function fn_LlenarDropDown(ByRef ddl As DropDownList, ByVal Tbl As DataTable, ByVal Todas As Boolean) As DataTable
        Dim RowSeleccione As DataRow = Tbl.NewRow()
        Dim lengt As Integer = 0
        For Each c As DataColumn In Tbl.Columns
            If c.ColumnName = ddl.DataTextField And c.DataType Is GetType(String) Then
                If c.MaxLength > 0 Then
                    lengt = c.MaxLength
                End If
                If lengt > 0 Then

                    If Todas Then
                        RowSeleccione(c.ColumnName) = Left("Todas", lengt)
                    Else
                        RowSeleccione(c.ColumnName) = Left("Seleccione...", lengt)
                    End If
                Else
                    If Todas Then
                        RowSeleccione(c.ColumnName) = Left("Todas", lengt)
                    Else
                        RowSeleccione(c.ColumnName) = Left("Seleccione...", lengt)
                    End If

                    End If
                ElseIf c.ColumnName = ddl.DataValueField And c.DataType IsNot GetType(Date) Then
                    RowSeleccione(c.ColumnName) = 0
                ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(String) Then
                    RowSeleccione(c.ColumnName) = String.Empty
                ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(Decimal) Then
                    RowSeleccione(c.ColumnName) = 0
                ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(Date) Then
                    RowSeleccione(c.ColumnName) = Today
                ElseIf (Not c.AllowDBNull) And c.DataType Is GetType(Integer) Then
                    RowSeleccione(c.ColumnName) = 0
                Else
                    RowSeleccione(c.ColumnName) = DBNull.Value
            End If
        Next

        Tbl.Rows.InsertAt(RowSeleccione, 0)

        ddl.DataSource = Tbl
        ddl.DataBind()

        Return Tbl
    End Function

    Public Function fn_Convertir_Datatable_ReportexRemision(ByVal Id_CajaBancaria As Integer, ByVal Id_Moneda As Integer, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal Id_GrupoF As Integer, ByVal Dpto As Char, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        'MODIF 26AGO2016 ADD FILTRO Left Join	Cat_ClientesGruposD
        Dim Dt_Denominaciones As DataTable
        Dim Dt_Servicios As DataTable
        Dim Dt_Desglose As DataTable
        Dim Dt_Cheques As DataTable
        Dim Dt_Reporte_General As New DataTable

        'Aqui cargare todas las denominaciones del Id_Moneda=1(Pesos)
        Dt_Denominaciones = cn.fn_DetalleDepositos_GetDenominaciones(Id_Moneda)
        If Dt_Denominaciones Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar las Denominaciones.")
            Return Nothing
        ElseIf Dt_Denominaciones.Rows.Count = 0 Then
            fn_Alerta("No existen Denominaciones para la Moneda seleccionada.")
            Return Nothing
        End If
        '********************************************************************

        'Aqui cargo el detalle de todo el deposito del cliente
        Dt_Servicios = cn.fn_DetalleDepositos_GetServicios(Id_CajaBancaria, Id_Moneda, Desde, Hasta, Id_GrupoF, Dpto, Nivel, Cliente, TodosClientes)
        If Dt_Servicios Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar los Depósitos.")
            Return Nothing
        ElseIf Dt_Servicios.Rows.Count = 0 Then
            fn_Alerta("No se econtró información con los parámetros seleccionados.")
            Return Nothing
        End If
        '*********************************************************************

        'Aqui traigo todo el desglose de los billetes 
        Dt_Desglose = cn.fn_DetalleDepositos_GetDesglose(Id_CajaBancaria, Id_Moneda, Desde, Hasta, Id_GrupoF, Dpto, Nivel, Cliente, TodosClientes)
        If Dt_Desglose Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar el Desglose de Efectivo.")
            Return Nothing
        End If
        '**********************************************************************

        'Aqui cargo el desglose de todos los cheques
        Dt_Cheques = cn.Fn_DetalleDepositos_Cheques(Id_CajaBancaria, Desde, Hasta, Id_Moneda, Dpto, Id_GrupoF, Nivel, Cliente, TodosClientes)
        If Dt_Cheques Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar los Cheques.")
            Return Nothing
        End If

        If Dt_Desglose.Rows.Count = 0 And Dt_Cheques.Rows.Count = 0 Then
            fn_Alerta("No se econtró información con los parámetros seleccionados.")
            Return Nothing
        End If

        Dt_Reporte_General.Columns.Add("Fecha")
        Dt_Reporte_General.Columns.Add("Remision")
        Dt_Reporte_General.Columns.Add("Origen")

        Dt_Reporte_General.Columns.Add("Dice_Contener")
        Dt_Reporte_General.Columns.Add("ImporteReal")
        Dt_Reporte_General.Columns.Add("Diferencia")

        Dt_Reporte_General.Columns.Add("Envases")
        Dt_Reporte_General.Columns.Add("Bolsas")
        Dt_Reporte_General.Columns.Add("Mazos")
        Dim PrimerasColumnas As Boolean = True
        Dim PrimerasColumnasCheques As Boolean = True

        For Each Servicio As DataRow In Dt_Servicios.Rows

            Dim Row As DataRow = Dt_Reporte_General.NewRow()
            Row("Fecha") = Servicio("Fecha")
            Row("Remision") = Servicio("Remision")
            Row("Origen") = Servicio("Cliente")
            Row("Dice_Contener") = Servicio("Dice_Contener")
            Row("ImporteReal") = Servicio("ImporteReal")
            Row("Diferencia") = Servicio("Diferencia")

            Row("Envases") = Servicio("Envases") + Servicio("EnvasesSN")
            Row("Bolsas") = 0
            Row("Mazos") = 0
            Dt_Reporte_General.Rows.Add(Row)

            Dim EfectivoTemporal() As DataRow
            EfectivoTemporal = Dt_Desglose.Select("Id_Servicio=" & Servicio("Id_Servicio"))
            For IEfe As Integer = 0 To EfectivoTemporal.Length - 1
                For Each Denominacion As DataRow In Dt_Denominaciones.Rows
                    If PrimerasColumnas Then
                        Dt_Reporte_General.Columns.Add(Mid(Denominacion("Presentacion"), 1, 1) & " " & Denominacion("Denominacion"))
                    End If
                    If Denominacion("Id_Denominacion") = EfectivoTemporal(IEfe)("Id_Denominacion") Then
                        Row(Mid(Denominacion("Presentacion"), 1, 1) & " " & Denominacion("Denominacion")) = EfectivoTemporal(IEfe)("Cantidad")
                    End If
                Next
                PrimerasColumnas = False
            Next

            Dim ChequesTemporales() As DataRow
            ChequesTemporales = Dt_Cheques.Select("Id_Servicio=" & Servicio("Id_Servicio"))
            If PrimerasColumnasCheques Then
                Dt_Reporte_General.Columns.Add("Cantidad Cheques")
                Dt_Reporte_General.Columns.Add("Cantidad Propios")
                Dt_Reporte_General.Columns.Add("Cantidad Otros")
                Dt_Reporte_General.Columns.Add("Cheques Importe")
                Dt_Reporte_General.Columns.Add("Propios Importe")
                Dt_Reporte_General.Columns.Add("Otros Importe")
            End If

            If ChequesTemporales.Length > 0 Then
                If ChequesTemporales(0)("Cheques_Propios") > 0 Then
                    Row("Cantidad Propios") = ChequesTemporales(0)("Cheques_Propios")
                End If
                If ChequesTemporales(0)("Cheques_Otros") > 0 Then
                    Row("Cantidad Otros") = ChequesTemporales(0)("Cheques_Otros")
                End If
                If ChequesTemporales(0)("Importe_Cheques") > 0 Then
                    Row("Cheques Importe") = ChequesTemporales(0)("Importe_Cheques")
                End If
                If ChequesTemporales(0)("Cheques_PropiosImp") > 0 Then
                    Row("Propios Importe") = ChequesTemporales(0)("Cheques_PropiosImp")
                End If
                If ChequesTemporales(0)("Cheques_OtrosImp") > 0 Then
                    Row("Otros Importe") = ChequesTemporales(0)("Cheques_OtrosImp")
                End If
                Row("Cantidad Cheques") = ChequesTemporales(0)("Cheques_propios") + ChequesTemporales(0)("Cheques_Otros")

            End If
            PrimerasColumnasCheques = False
        Next

        Dt_Reporte_General.Rows.Add()
        Dim AgregoFila As Boolean = False
        Dim Valor As Decimal = 0
        Dim Valor2 As Decimal
        Dim Total As Decimal
        Dim TotalMazo As Decimal = 0
        Dim Bolsas As Decimal = 0
        Dim BolsasT As Decimal = 0
        'DE AQUI PARA ABAJO SON LAS SUMAS 
        For Each Ficha As DataRow In Dt_Reporte_General.Rows

            For Each Colum As DataColumn In Dt_Reporte_General.Columns

                If IsDBNull(Ficha("Remision")) Then Exit For

                If Colum.ColumnName = "Cheques Importe" OrElse Colum.ColumnName = "Propios Importe" OrElse _
                    Colum.ColumnName = "Otros Importe" OrElse Colum.ColumnName = "Importe" Then
                    If Not IsDBNull(Ficha(Colum.ColumnName)) Then
                        Valor = Ficha(Colum.ColumnName)
                        If IsDBNull(Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)) Then
                            Valor2 = 0
                        Else
                            Valor2 = Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)
                        End If

                        Total = Valor + Valor2

                        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName) = FormatCurrency(Total)
                        Valor = 0
                        Valor2 = 0
                        Total = 0
                    End If

                End If

                If Mid(Colum.ColumnName, 1, 2) = "B " OrElse Mid(Colum.ColumnName, 1, 2) = "M " Then

                    If IsDBNull(Ficha(Colum.ColumnName)) Then Continue For
                    Valor = Ficha(Colum.ColumnName)
                    TotalMazo += Valor
                    If IsDBNull(Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)) Then
                        Valor2 = 0
                    Else
                        Valor2 = Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)
                    End If

                    Valor = Valor * Mid(Colum.ColumnName, 3, Colum.ColumnName.Length)
                    Total = Valor + Valor2
                    Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName) = FormatCurrency(Total)
                    Valor = 0
                    Valor2 = 0
                    Total = 0

                    '----------->>Aqui sumo el total de monedas del cliente
                    If Mid(Colum.ColumnName, 1, 2) = "M " Then
                        If IsDBNull(Ficha(Colum.ColumnName)) Then Continue For
                        For Each Denominacion As DataRow In Dt_Denominaciones.Rows
                            If Not Denominacion("Presentacion") = "MONEDA" Then Continue For
                            If Mid(Colum.ColumnName, 3, Colum.ColumnName.Length) = Denominacion("Denominacion") Then
                                Bolsas = (Ficha(Colum.ColumnName) / Denominacion("CantidadXbolsa"))
                                BolsasT += Bolsas
                                Exit For
                            End If
                        Next
                    End If
                    '<<--------------------
                End If

                If Colum.ColumnName = "Cantidad Cheques" OrElse Colum.ColumnName = "Cantidad Propios" OrElse _
                   Colum.ColumnName = "Cantidad Otros" OrElse Colum.ColumnName = "Bolsas" OrElse Colum.ColumnName = "Mazos" OrElse _
                    Colum.ColumnName = "Envases" Then

                    If Not IsDBNull(Ficha(Colum.ColumnName)) Then
                        Valor = Ficha(Colum.ColumnName)
                        If IsDBNull(Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)) Then
                            Valor2 = 0
                        Else
                            Valor2 = Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)
                        End If

                        Total = Valor + Valor2

                        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName) = Total
                        Valor = 0
                        Valor2 = 0
                        Total = 0
                    End If

                End If
            Next
            Ficha("Bolsas") = BolsasT
            Ficha("Mazos") = (TotalMazo / 1000)
            TotalMazo = 0
            BolsasT = 0
        Next

        'TOTAL GRAL DE MASOS Y BOLSAS
        For Each Row As DataRow In Dt_Reporte_General.Rows
            TotalMazo += Row("Mazos")
            BolsasT += Row("Bolsas")
        Next
        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)("Mazos") = TotalMazo
        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)("Bolsas") = BolsasT

        Return Dt_Reporte_General

    End Function

    Public Function fn_Convertir_Datatable_ReportexFicha(ByVal Id_CajaBancaria As Integer, ByVal Id_Moneda As Integer, ByVal Desde As Integer, ByVal Hasta As Integer, ByVal Id_GrupoF As Integer, ByVal Dpto As Char, ByVal Nivel As Integer, ByVal Cliente As Integer, ByVal TodosClientes As Boolean) As DataTable
        'MODIF 26AGO2016 ADD FILTRO Left Join	Cat_ClientesGruposD
        Dim Dt_Servicios As DataTable
        Dim Dt_Desglose As DataTable
        Dim Dt_Cheques As DataTable
        Dim Dt_Reporte_General As New DataTable

        Dim Dt_Denominaciones As DataTable = cn.fn_DetalleDepositos_GetDenominaciones(Id_Moneda)
        If Dt_Denominaciones Is Nothing Then
            fn_Alerta("No existen Denominaciones para la Moneda seleccionada.")
            Return Nothing
        End If

        'Traer las Dotaciones
        Dt_Servicios = cn.fn_DetalleDepositos_GetServiciosEsp(Id_CajaBancaria, Id_Moneda, Desde, Hasta, Id_GrupoF, Dpto, Nivel, Cliente, TodosClientes)

        If Dt_Servicios Is Nothing Then
            fn_Alerta("Ocurrio un error al consultar los datos.")
            Return Nothing
        ElseIf Dt_Servicios.Rows.Count = 0 Then
            fn_Alerta("No Hay Informacion para los datos seleccionados.")
            Return Nothing
        End If

        'Traer el dsglose
        Dt_Desglose = cn.fn_DetalleDepositos_GetDesgloseEsp(Id_CajaBancaria, Id_Moneda, Desde, Hasta, Id_GrupoF, Dpto, Nivel, Cliente, TodosClientes)
        If Dt_Desglose Is Nothing Then
            fn_Alerta("Ocurrió un Error al intentar Consultar el Desglose de las Fichas.")
            Return Nothing
        ElseIf Dt_Desglose.Rows.Count = 0 Then
            fn_Alerta("No existe desglose para esta Ficha.")
            Return Nothing
        End If

        'Traer los Cheques
        Dt_Cheques = cn.Fn_DetalleDepositos_ChequesEsp(Id_CajaBancaria, Desde, Hasta, Id_Moneda, Dpto, Id_GrupoF, Nivel, Cliente, TodosClientes)
        If Dt_Cheques Is Nothing Then
            fn_Alerta("Ocurrió un Error al intentar Consultar los Cheques.")
            Return Nothing
        ElseIf Dt_Cheques.Rows.Count = 0 Then
            fn_Alerta("No existen cheques para esta Ficha")
            Return Nothing
        End If

        Dt_Reporte_General.Columns.Add("Fecha")
        Dt_Reporte_General.Columns.Add("Remision")
        Dt_Reporte_General.Columns.Add("Origen")
        Dt_Reporte_General.Columns.Add("Numero de Cuenta")
        Dt_Reporte_General.Columns.Add("Referencia")

        Dt_Reporte_General.Columns.Add("Referencia2") 'agregado 05-08-2017
        Dt_Reporte_General.Columns.Add("FolioFicha") 'agregado 05-08-2017

        Dt_Reporte_General.Columns.Add("Importe Efectivo")
        Dt_Reporte_General.Columns.Add("Importe Cheques")
        Dt_Reporte_General.Columns.Add("Importe Otros")
        Dt_Reporte_General.Columns.Add("Diferencia Efectivo")
        Dt_Reporte_General.Columns.Add("Diferencia Cheques")
        Dt_Reporte_General.Columns.Add("Diferencia Otros")
        Dt_Reporte_General.Columns.Add("Bolsas")
        Dt_Reporte_General.Columns.Add("Mazos")
        Dim PrimerasColumnas As Boolean = True
        Dim PrimerasColumnasCheques As Boolean = True

        For Each Ficha As DataRow In Dt_Servicios.Rows

            Dim Row As DataRow = Dt_Reporte_General.NewRow()
            Row("Fecha") = Ficha("Fecha")
            Row("Remision") = Ficha("Remision")
            Row("Origen") = Ficha("Cliente")
            Row("Numero de Cuenta") = Ficha("Num_Cuenta")
            Row("Referencia") = Ficha("Referen")
            Row("Referencia2") = Ficha("Referen2") 'Agregado 05-08-2017
            Row("FolioFicha") = Ficha("FolioFicha") 'Agregado 05-08-2017
            Row("Importe Efectivo") = Ficha("Imp_Efec")
            Row("Importe Cheques") = Ficha("Imp_Cheq")
            Row("Importe Otros") = Ficha("Imp_Otros")
            Row("Diferencia Efectivo") = Ficha("Dif_Efec")
            Row("Diferencia Cheques") = Ficha("Dif_Cheq")
            Row("Diferencia Otros") = Ficha("Dif_Otros")
            Row("Bolsas") = 0
            Row("Mazos") = 0


            Dt_Reporte_General.Rows.Add(Row)

            Dim EfectivoTemporal() As DataRow
            EfectivoTemporal = Dt_Desglose.Select("Id_Ficha=" & Ficha("Id_Ficha"))
            For IEfe As Integer = 0 To EfectivoTemporal.Length - 1
                For Each Denominacion As DataRow In Dt_Denominaciones.Rows
                    If PrimerasColumnas Then
                        Dt_Reporte_General.Columns.Add(Mid(Denominacion("Presentacion"), 1, 1) & " " & Denominacion("Denominacion"))
                    End If
                    If Denominacion("Id_Denominacion") = EfectivoTemporal(IEfe)("Id_Denominacion") Then
                        Row(Mid(Denominacion("Presentacion"), 1, 1) & " " & Denominacion("Denominacion")) = EfectivoTemporal(IEfe)("Cantidad")
                    End If
                Next
                PrimerasColumnas = False
            Next

            Dim ChequesTemporales() As DataRow
            ChequesTemporales = Dt_Cheques.Select("Id_Ficha=" & Ficha("Id_Ficha"))
            If PrimerasColumnasCheques Then
                Dt_Reporte_General.Columns.Add("Cantidad Cheques")
                Dt_Reporte_General.Columns.Add("Cantidad Propios")
                Dt_Reporte_General.Columns.Add("Cantidad Otros")
                Dt_Reporte_General.Columns.Add("Cheques Importe")
                Dt_Reporte_General.Columns.Add("Propios Importe")
                Dt_Reporte_General.Columns.Add("Otros Importe")
            End If

            If ChequesTemporales.Length > 0 Then
                If ChequesTemporales(0)("Cheques_Propios") > 0 Then
                    Row("Cantidad Propios") = ChequesTemporales(0)("Cheques_Propios")
                End If
                If ChequesTemporales(0)("Cheques_Otros") > 0 Then
                    Row("Cantidad Otros") = ChequesTemporales(0)("Cheques_Otros")
                End If
                If ChequesTemporales(0)("Importe_Cheques") > 0 Then
                    Row("Cheques Importe") = ChequesTemporales(0)("Importe_Cheques")
                End If
                If ChequesTemporales(0)("Cheques_PropiosImp") > 0 Then
                    Row("Propios Importe") = ChequesTemporales(0)("Cheques_PropiosImp")
                End If
                If ChequesTemporales(0)("Cheques_OtrosImp") > 0 Then
                    Row("Otros Importe") = ChequesTemporales(0)("Cheques_OtrosImp")
                End If
                Row("Cantidad Cheques") = ChequesTemporales(0)("Cheques_propios") + ChequesTemporales(0)("Cheques_Otros")

            End If
            PrimerasColumnasCheques = False
        Next
        Dt_Reporte_General.Rows.Add()
        Dim AgregoFila As Boolean = False
        Dim Valor As Decimal = 0
        Dim Valor2 As Decimal
        Dim Total As Decimal
        Dim TotalMazo As Decimal = 0
        Dim Bolsas As Decimal = 0
        Dim BolsasT As Decimal = 0

        'DE AQUI PARA ABAJO SON LAS SUMAS 
        For Each Ficha As DataRow In Dt_Reporte_General.Rows

            For Each Colum As DataColumn In Dt_Reporte_General.Columns

                If IsDBNull(Ficha("Remision")) Then Exit For

                If Colum.ColumnName = "Importe Efectivo" OrElse Colum.ColumnName = "Importe Cheques" OrElse _
                   Colum.ColumnName = "Importe Otros" OrElse Colum.ColumnName = "Diferencia Efectivo" OrElse _
                   Colum.ColumnName = "Diferencia Cheques" OrElse Colum.ColumnName = "Diferencia Otros" OrElse _
                   Colum.ColumnName = "Cheques Importe" OrElse Colum.ColumnName = "Propios Importe" OrElse _
                    Colum.ColumnName = "Otros Importe" Then
                    If Not IsDBNull(Ficha(Colum.ColumnName)) Then
                        Valor = Ficha(Colum.ColumnName)
                        If IsDBNull(Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)) Then
                            Valor2 = 0
                        Else
                            Valor2 = Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)
                        End If

                        Total = Valor + Valor2

                        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName) = FormatCurrency(Total)
                        Valor = 0
                        Valor2 = 0
                        Total = 0
                    End If

                End If

                If Mid(Colum.ColumnName, 1, 2) = "B " OrElse Mid(Colum.ColumnName, 1, 2) = "M " Then

                    If IsDBNull(Ficha(Colum.ColumnName)) Then Continue For
                    Valor = Ficha(Colum.ColumnName)
                    TotalMazo += Valor
                    If IsDBNull(Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)) Then
                        Valor2 = 0
                    Else
                        Valor2 = Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)
                    End If

                    Valor = Valor * Mid(Colum.ColumnName, 3, Colum.ColumnName.Length)
                    Total = Valor + Valor2
                    Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName) = FormatCurrency(Total)
                    Valor = 0
                    Valor2 = 0
                    Total = 0

                    '*********************Aqui sumo el total de monedas del cliente
                    If Mid(Colum.ColumnName, 1, 2) = "M " Then
                        If IsDBNull(Ficha(Colum.ColumnName)) Then Continue For
                        For Each Denominacion As DataRow In Dt_Denominaciones.Rows
                            If Not Denominacion("Presentacion") = "MONEDA" Then Continue For
                            If Mid(Colum.ColumnName, 3, Colum.ColumnName.Length) = Denominacion("Denominacion") Then
                                Bolsas = (Ficha(Colum.ColumnName) / Denominacion("CantidadXbolsa"))
                                BolsasT += Bolsas
                                Exit For
                            End If
                        Next
                    End If
                    '************************
                End If

                If Colum.ColumnName = "Cantidad Cheques" OrElse Colum.ColumnName = "Cantidad Propios" OrElse _
                   Colum.ColumnName = "Cantidad Otros" OrElse Colum.ColumnName = "Bolsas" OrElse Colum.ColumnName = "Mazos" Then

                    If Not IsDBNull(Ficha(Colum.ColumnName)) Then
                        Valor = Ficha(Colum.ColumnName)
                        If IsDBNull(Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)) Then
                            Valor2 = 0
                        Else
                            Valor2 = Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName)
                        End If

                        Total = Valor + Valor2

                        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)(Colum.ColumnName) = Total
                        Valor = 0
                        Valor2 = 0
                        Total = 0
                    End If

                End If
            Next
            Ficha("Bolsas") = BolsasT
            Ficha("Mazos") = (TotalMazo / 1000)
            TotalMazo = 0
            BolsasT = 0
        Next

        'TOTAL GRAL DE MASOS Y BOLSAS

        For Each Row As DataRow In Dt_Reporte_General.Rows
            TotalMazo += Row("Mazos")
            BolsasT += Row("Bolsas")
        Next
        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)("Mazos") = TotalMazo
        Dt_Reporte_General(Dt_Reporte_General.Rows.Count - 1)("Bolsas") = BolsasT

        Return Dt_Reporte_General
    End Function

#End Region

#Region "Funciones {Encripta y Desencripta}"

    Public Shared Function fn_Encode(ByVal data As String) As String
        Try
            Dim encyrpt(0 To data.Length - 1) As Byte
            encyrpt = System.Text.Encoding.UTF8.GetBytes(data)
            Dim encodedata As String = Convert.ToBase64String(encyrpt)
            Return encodedata
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function fn_Decode(ByVal data As String) As String
        Try
            Dim encoder As New UTF8Encoding()
            Dim decode As Decoder = encoder.GetDecoder()
            Dim bytes As Byte() = Convert.FromBase64String(data)
            Dim count As Integer = decode.GetCharCount(bytes, 0, bytes.Length)
            Dim decodechar(0 To count - 1) As Char
            decode.GetChars(bytes, 0, bytes.Length, decodechar, 0)
            Dim result As New String(decodechar)
            Return result
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function fn_EncryptToSHA1(ByVal password As String) As String
        Try
            Dim sha As New System.Security.Cryptography.SHA1CryptoServiceProvider
            Dim bytesToHash() As Byte
            bytesToHash = System.Text.Encoding.ASCII.GetBytes(password)
            bytesToHash = sha.ComputeHash(bytesToHash)

            Dim encPassword As String = ""

            For Each b As Byte In bytesToHash
                encPassword += b.ToString("X2")
            Next
            Return encPassword
        Catch ex As Exception
            Return ""
        End Try
    End Function

#End Region

#Region "Funciones Varias "

    Public Shared Function fn_Exportar_Excel(ByVal dt_Datos As DataTable, ByVal Titulo As String, ByVal Subtitulo1 As String, ByVal Subtitulo2 As String, _
                                        Optional ByVal Cols_Izquierda_Omitir As Integer = 0, Optional ByVal Cols_Derecha_Omitir As Integer = 0, _
                                          Optional ByVal NombreCliente As String = "", Optional ByVal NombreUsuario As String = "", Optional ByVal FilasRestar As Integer = 0) As Boolean
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)

        Dim gv_Datos As New GridView
        Dim cantColumnas As Integer = dt_Datos.Columns.Count
        Dim NombreArchivo = FechaExcel()
        Try

            ' ---- Manipular el dt antes de sourcesarlo ----
            If (Cols_Izquierda_Omitir + Cols_Derecha_Omitir) > cantColumnas Then
                Return False
            End If

            For i As Short = 0 To (Cols_Izquierda_Omitir - 1)
                dt_Datos.Columns.RemoveAt(0)
            Next
            cantColumnas = dt_Datos.Columns.Count ' Refrescar

            For i As Short = 1 To Cols_Derecha_Omitir
                dt_Datos.Columns.RemoveAt(cantColumnas - i)
            Next

            gv_Datos.DataSource = dt_Datos
            gv_Datos.DataBind()
            gv_Datos.AllowPaging = False
            gv_Datos.EnableViewState = False
            '----------------------------------

            gv_Datos.RenderControl(htw)
            With HttpContext.Current.Response
                .Clear()
                .Buffer = True
                .ContentType = "application/vnd.ms-excel" 'vnd.ms-word'exporta a word
                .AddHeader("Content-Disposition", "attachment;filename=" & NombreArchivo & ".xls")
                .Charset = ""
                .ContentEncoding = Encoding.Default
                .Output.Write("<br><b>" & Titulo & "</b>")
                .Output.Write("<br><b>" & Subtitulo1 & "</b>")
                .Output.Write("<br><b>" & Subtitulo2 & "</b>" & "<br>")
                .Output.Write(sb.ToString())
                .Output.Write("<br><b>" & "Cantidad de Registros: " & gv_Datos.Rows.Count - FilasRestar & "</b>")
                .Output.Write("<br><b>" & "Fecha: " & Format(Today.Date, "dd/MMM/yyyy") & " - " & Now.ToShortTimeString & "</b>")
                .Output.Write("<br><b>" & "Usuario: " & NombreUsuario & "</b>" & "<br>")

                .Flush()
                .End()
            End With
            gv_Datos.Dispose()
            dt_Datos.Dispose()
        Catch ex As Exception
            Return False
        End Try
        Return True

    End Function

    Public Shared Function FechaExcel() As String
        Dim FechaHora As String = ""
        FechaHora = Now.ToString("yyyyMMdd_hhmmss")
        Return FechaHora

    End Function

    Public Shared Function fn_ValidarMail(ByVal sMail As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(sMail, "^([\w-]+\.)*?[\w-]+@[\w-]+\.([\w-]+\.)*?[\w]+$")
    End Function

    Public Shared Function fn_DatatableToHTML(ByVal dt As DataTable, ByVal Titulo As String, ByVal Cols_Omitir_Izq As Integer, ByVal Cols_Omitir_Der As Integer) As String
        '"Prueba de Correo HTML.<Br><Table><Tr><Td>Celda 1</Td><Td>Celda 2</Td></Tr><Tr><Td>Celda 1</Td><Td>Celda 2</Td></Tr></Table>"
        Dim Cadena As String = ""
        Dim Fila As Integer = 0
        Dim Columna As Integer = 0
        'Titulo
        Cadena = "<Table style='border:solid 1px black; border-collapse:collapse' width='100%'>"
        Cadena &= "<CAPTION style='border:solid 1px black'><b>"
        Cadena &= Titulo
        Cadena &= "</b></CAPTION>"
        'Encabezados
        Cadena &= "<thead>"
        Cadena &= "<tr>"
        Dim indice As Integer = 0
        For Each cl As DataColumn In dt.Columns
            If indice >= Cols_Omitir_Izq Then
                If indice > (dt.Columns.Count - 1 - Cols_Omitir_Der) Then Exit For
                Cadena &= "<th style='border:solid 1px black'>"
                Cadena &= cl.Caption
                Cadena &= "</th>"
            End If
            indice += 1
        Next
        Cadena &= "</tr>"
        Cadena &= "<thead>"
        'Filas
        For Fila = 0 To dt.Rows.Count - 1
            Cadena &= "<Tr>"
            For Columna = 0 + Cols_Omitir_Izq To dt.Columns.Count - 1 - Cols_Omitir_Der
                If IsNumeric(dt.Rows(Fila)(Columna).ToString) Then
                    Cadena &= "<Td style='border:solid 1px black; text-align:right'>"
                Else
                    Cadena &= "<Td style='border:solid 1px black'>"
                End If

                Cadena &= dt.Rows(Fila)(Columna).ToString
                Cadena &= "</Td>"
            Next
            Cadena &= "</Tr>"
        Next Fila
        Cadena &= "</Table>"
        Return Cadena
    End Function

#End Region

End Class
