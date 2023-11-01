Imports System.IO
Imports System.Net
Imports System.Windows.Forms

Partial Public Class CrearUsuario
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then Exit Sub

        MultiView1.ActiveViewIndex = 0
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CREAR USUARIO")
        'If pNivel = 2 Then Response.Redirect("~/Default.aspx")
        Dim dt_Localidad As DataTable = cn.fn_LocalidadCorporativo(pClave_SucursalPropia)
        Dim dt_LocalidadConsulta As DataTable = cn.fn_LocalidadCorporativo(pClave_SucursalPropia)
        Call fn_LlenarDropDown(ddl_Localidad, dt_Localidad, False)
        If dt_Localidad.Rows.Count > 0 Then
            ddl_Localidad.SelectedIndex = 1
        End If
        ddl_Localidad_SelectedIndexChanged(sender, e)
        Call fn_LlenarDropDown(ddl_LocalidadConsulta, dt_LocalidadConsulta, False)
        If dt_LocalidadConsulta.Rows.Count > 0 Then
            ddl_LocalidadConsulta.SelectedIndex = 1
        End If
        ddl_LocalidadConsulta_SelectedIndexChanged(sender, e)



    End Sub

    Private Function LlenarUsuarios() As Integer
        Dim TotalUsuarios As Integer = 0

        'Dim dt_Usuarios As DataTable = cn.fn_UsuariosClientes_Get(pClave_Corporativo, pClave_Sucursal)
        Dim dt_Usuarios As DataTable = cn.fn_UsuariosClientes_Get(pId_Cliente, pClave_Sucursal)
        If dt_Usuarios Is Nothing Then
            fn_Alerta("Ocurrio un error al Cargar los Datos.")
            Return TotalUsuarios
        End If

        If dt_Usuarios.Rows.Count > 0 Then
            pTabla("Resultado") = dt_Usuarios
        End If

        gv_CrearUsuarios.DataSource = fn_MostrarSiempre(dt_Usuarios)
        gv_CrearUsuarios.DataBind()
        gv_CrearUsuarios.Focus()

        If gv_CrearUsuarios.Rows.Count < 4 Then
            ' fst_CrearUsuario.Style.Add("height", "205px")
        End If

        Return dt_Usuarios.Rows.Count
    End Function
    Private Function LlenarUsuarios(Id_ClienteS As Integer) As Integer
        Dim TotalUsuarios As Integer = 0

        'Dim dt_Usuarios As DataTable = cn.fn_UsuariosClientes_Get(pClave_Corporativo, pClave_Sucursal)
        Dim dt_Usuarios As DataTable = cn.fn_UsuariosClientes_Get(Id_ClienteS, pClave_Sucursal)
        If dt_Usuarios Is Nothing Then
            fn_Alerta("Ocurrio un error al Cargar los Datos.")
            Return TotalUsuarios
        End If

        If dt_Usuarios.Rows.Count > 0 Then
            pTabla("Resultado") = dt_Usuarios
        End If

        gv_CrearUsuarios.DataSource = fn_MostrarSiempre(dt_Usuarios)
        gv_CrearUsuarios.DataBind()
        gv_CrearUsuarios.Focus()

        If gv_CrearUsuarios.Rows.Count < 4 Then
            'fst_CrearUsuario.Style.Add("height", "205px")
        End If

        Return dt_Usuarios.Rows.Count
    End Function

    Private Function ContarUsuarios() As Integer
        Dim TotalUsuarios As Integer = 0
        Dim dt_Usuarios As DataTable = cn.fn_UsuariosClientes_Get(pId_Cliente, pClave_Sucursal)

        If dt_Usuarios Is Nothing Then
            fn_Alerta("Ocurrio un error al contabilizar Usuarios.")
            Return TotalUsuarios
        End If

        Return dt_Usuarios.Rows.Count
    End Function

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click

        If ddl_Localidad.SelectedValue = "0" Then
            fn_Alerta("Seleccione La Localidad.")
            ddl_Localidad.Focus()
            Exit Sub
        End If

        If ddl_Sucursales.SelectedValue = "0" Then
            fn_Alerta("Seleccione La Sucursal.")
            ddl_Sucursales.Focus()
            Exit Sub
        End If

        If ddl_Nivel.SelectedValue = "0" Then
            fn_Alerta("Seleccione el Nivel del Usuario.")
            ddl_Nivel.Focus()
            Exit Sub
        End If

        If tbx_Nombre.Text = "" Then
            fn_Alerta("Indique el Nombre completo del Usuario.")
            tbx_Nombre.Focus()
            Exit Sub
        End If

        If tbx_Sesion.Text = "" Then
            fn_Alerta("Nombre Usuario inexistente.")
            tbx_Sesion.Focus()
            Exit Sub
        End If

        If ddl_ModoConsulta.SelectedValue = "0" Then
            fn_Alerta("Seleccione el Modo de Consulta.")
            ddl_ModoConsulta.Focus()
            Exit Sub
        End If
        If tbx_mail.Text.Trim = "" Then
            fn_Alerta("Indique un correo ")
            'tbx_Sesion.Focus()
            Exit Sub
        End If
        Dim ContraHash As String
        Dim ValidarIP As String = "N"
        Dim dt_Existe As DataTable = cn.Fn_UsuariosClientes_Existe(pClave_Corporativo, tbx_Sesion.Text.ToUpper)
        'Dim dt_Sucursales As DataTable = cn.Fn_UsuariosClientes_ComboSucursales(pClave_Corporativo)
        ' Dim Clave_SP As String = dt_Sucursales.Rows(ddl_Sucursales.SelectedIndex - 1)("Clave_SucursalPropia").ToString

        If dt_Existe Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar si el nombre de Usuario ya existe.")
            Exit Sub
        End If
        If dt_Existe.Rows.Count > 0 Then
            fn_Alerta("El nombre de Usuario ya existe, se asignara uno nuevo.")

            Dim sessionActual As Integer = tbx_Sesion.Text.Substring(dt_Existe.Rows(0)(2).ToString, 5)
            Dim sessionBD As Integer
            Dim session_id As String = ""
            session_id = cn.fn_Get_Session(pClave_Corporativo)
            sessionBD = session_id.Substring(dt_Existe.Rows(0)(2).ToString, 5)
            If (sessionBD) >= sessionActual Then
                cn.fn_Upd_ContadorSession(pClave_Corporativo)
            End If
            tbx_Sesion.Text = cn.fn_Get_Session(pClave_Corporativo)
            Exit Sub
        Else
            'cn.fn_Crear_Log(pId_Login, "AGREGAR NUEVO USUARIO CLIENTE: " & tbx_Nombre.Text & " / CLIENTE: " & ddl_Clientes.Text)--comentado
            'asignar como clave su Nombre de Sesion
            ContraHash = FormsAuthentication.HashPasswordForStoringInConfigFile(tbx_Sesion.Text.Trim.ToUpper, "SHA1")

            If cn.fn_UsuariosClientes_Agregar(ddl_Sucursales.SelectedValue, tbx_Nombre.Text.Trim, tbx_Sesion.Text.Trim,
                                             pTipo_Contacto, ddl_Nivel.SelectedValue,
                                             pId_Cliente, 0, ContraHash,
                                             ValidarIP, pClave_SucursalPropia,
                                             ddl_Sucursales.SelectedItem.Text, pId_Usuario, ddl_ModoConsulta.SelectedValue, tbx_mail.Text.Trim) = False Then
                'Cn_Login.fn_Log_Create("ERROR AL AGREGAR NUEVO USUARIO CLIENTE: " & tbx_Nombre.Text & " / CLIENTE: " & cmb_Cliente.Text)}

                fn_Alerta("Ocurrió un error al intentar agregar el Nuevo Usuario.")
                Exit Sub
            Else
                fn_Alerta("El Usuario Registrado es: " & tbx_Sesion.Text & " y su contraseña : " & tbx_Sesion.Text)
                cn.fn_Upd_ContadorSession(pClave_Corporativo)
            End If
        End If

        'Call LlenarUsuarios()
        'Call ColorearBloqueados(gv_CrearUsuarios)
        Call BorrarDatos()
    End Sub

    Protected Sub ddl_Sucursales_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Sucursales.SelectedIndexChanged

        If ddl_Nivel.Enabled = False And ddl_Sucursales.SelectedValue <> 0 Then
            'ddl_Nivel.Enabled = True
            ddl_Nivel.SelectedIndex = 1
            tbx_Nombre.Enabled = True
            ddl_ModoConsulta.SelectedIndex = 1
            tbx_Nombre.Focus()
            Call Sugerencia()
        Else
            ddl_Nivel.Enabled = False
            ddl_Nivel.SelectedValue = 0
            tbx_Nombre.Enabled = False
            tbx_Nombre.Text = ""
            tbx_Sesion.Enabled = False
            tbx_Sesion.Text = ""
            ddl_ModoConsulta.Enabled = False
            ddl_ModoConsulta.SelectedIndex = 0
            tbx_mail.Enabled = False
            tbx_mail.Text = ""
            tbx_Nombre.Focus()
            Call Sugerencia()
        End If

    End Sub

    Protected Sub tbx_Nombre_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles tbx_Nombre.TextChanged
        'If tbx_Nombre.Text.Trim = "" Then Exit Sub
        'tbx_Nombre.Text = tbx_Nombre.Text.ToUpper 'convierte a mayusculas
        'tbx_mail.Enabled = True


    End Sub

    Protected Sub gv_CrearUsuarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_CrearUsuarios.PageIndexChanging

        gv_CrearUsuarios.PageIndex = e.NewPageIndex
        gv_CrearUsuarios.DataSource = pTabla("Resultado")
        gv_CrearUsuarios.DataBind()

        gv_CrearUsuarios.SelectedIndex = -1
        pTabla("Fichas") = Nothing
        'Call Limpiar()

        If gv_CrearUsuarios.Rows.Count < 4 Then
            ' fst_CrearUsuario.Style.Add("height", "205px")
        End If
        Call ColorearBloqueados(gv_CrearUsuarios)
    End Sub

    Protected Sub gv_CrearUsuarios_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_CrearUsuarios.RowCommand
        If IsDBNull(gv_CrearUsuarios.DataKeys(0).Value) OrElse CStr(gv_CrearUsuarios.DataKeys(0).Value) = "" OrElse CStr(gv_CrearUsuarios.DataKeys(0).Value) = "0" Then Exit Sub
        ' gv_CrearUsuarios.DataBind()
        Dim Indice As Integer = Convert.ToInt32(e.CommandArgument)
        'Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
        'If IdUsuario = pId_Usuario Then Exit Sub

        'Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(8).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(7).Text
        'Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(9).Text
        'Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text

        Select Case e.CommandName
            Case "Reiniciar"

                Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
                If IdUsuario = pId_Usuario Then Exit Sub

                Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(8).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(7).Text
                Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(9).Text
                Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text

                If UsuarioStatus = "BAJA" Or UsuarioStatus = "BLOQUEADO" Then Exit Sub
                'If MessageBox.Show("Se reiniciara la clave del usurio:" + gv_CrearUsuarios.Rows(Indice).Cells(8).Text + ", desea continuar ?", "Aviso Importante", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> Windows.Forms.DialogResult.Yes Then Exit Sub

                Dim ContraHash As String = FormsAuthentication.HashPasswordForStoringInConfigFile(gv_CrearUsuarios.Rows(Indice).Cells(8).Text, "SHA1")

                Dim conta As Integer = cn.fn_UsuariosClientesContra_Reiniciar(IdUsuario, ContraHash)
                If conta > 0 Then
                    cn.fn_Crear_Log(pId_Login, "REINICIAR CONTRASEÑA A USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    fn_Alerta("La Contraseña se reinició correctamente.")
                    BuscarUsuarios_Click(sender, e)
                Else
                    cn.fn_Crear_Log(pId_Login, "ERROR AL REINICIAR CONTRASEÑA A USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    fn_Alerta("Ocurrió un problema al intentar reiniciar la Contraseña.")
                End If
                Call ColorearBloqueados(gv_CrearUsuarios)

            Case "Desbloquear"
                Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
                If IdUsuario = pId_Usuario Then Exit Sub

                Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(8).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(7).Text
                Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(9).Text
                Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text
                If UsuarioStatus = "BLOQUEADO" Then
                    cn.fn_Crear_Log(pId_Login, "DESBLOQUEAR USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    cn.fn_UsuariosClientes_Status(IdUsuario, "A")
                ElseIf UsuarioStatus = "ACTIVO" Then
                    cn.fn_Crear_Log(pId_Login, "BLOQUEAR USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    cn.fn_UsuariosClientes_Status(IdUsuario, "B")
                End If
                BuscarUsuarios_Click(sender, e)
                Call ColorearBloqueados(gv_CrearUsuarios)

            Case "Suspender"
                Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
                If IdUsuario = pId_Usuario Then Exit Sub

                Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(8).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(7).Text
                Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(9).Text
                Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text
                If UsuarioStatus = "ACTIVO" Or UsuarioStatus = "BLOQUEADO" Then
                    cn.fn_Crear_Log(pId_Login, "SUSPENDER USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    cn.fn_UsuariosClientes_Status(IdUsuario, "S")

                ElseIf UsuarioStatus = "SUSPENDIDO" Then
                    cn.fn_Crear_Log(pId_Login, "REACTIVAR USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    cn.fn_UsuariosClientes_Status(IdUsuario, "A")

                End If
                BuscarUsuarios_Click(sender, e)
                Call ColorearBloqueados(gv_CrearUsuarios)

            Case "Baja"
                Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
                If IdUsuario = pId_Usuario Then Exit Sub

                Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(8).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(7).Text
                Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(9).Text
                Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text
                If UsuarioStatus = "ACTIVO" Or UsuarioStatus = "BLOQUEADO" Then
                    cn.fn_Crear_Log(pId_Login, "DAR DE BAJA A USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    cn.fn_UsuariosClientes_StatusBaja(IdUsuario, "C", pId_Usuario) 'Dando de Baja usuario
                ElseIf UsuarioStatus = "BAJA" Then
                    cn.fn_Crear_Log(pId_Login, "DAR DE ALTA A USUARIO : " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)
                    cn.fn_UsuariosClientes_StatusBaja(IdUsuario, "A", pId_Usuario) 'Dando de Baja usuario
                End If

                BuscarUsuarios_Click(sender, e)
                Call ColorearBloqueados(gv_CrearUsuarios)

            Case "Reasignar"
                Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
                'If IdUsuario = pId_Usuario Then Exit Sub

                Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(7).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(8).Text
                Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(10).Text
                Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text
                'cn.fn_Crear_Log(pId_Login, "Reasigno Usuario: " & UsuarioNombre & " / CLIENTE: " & ClienteNombre)

                If UsuarioStatus = "BAJA" Then Exit Sub
                popup_click(sender, e)
                MmodalPanel.Visible = True

                Dim dt_Sucursales As DataTable = cn.Fn_UsuariosClientes_ComboSucursales(pClave_Corporativo, pClave_SucursalPropia)
                Dim fila As DataRow

                tb_usuarioReasignar.Text = gv_CrearUsuarios.Rows(Indice).Cells(7).Text

                Dim dt As DataTable = cn.Fn_UsuarioAReasignar(pClave_Corporativo, pClave_SucursalPropia, IdUsuario)
                Dim correo As String
                Dim reasignar_sucursal As String
                Dim reasignar_Idusuario As Integer
                For Each rows As DataRow In dt.Rows
                    reasignar_Idusuario = CInt(rows("Id_Usuario"))
                    reasignar_sucursal = CStr(rows("Clave_Sucursal"))
                    Correo = CStr(rows("Mail"))
                Next
                headerModal.Text = "Reasignar Usuario"
                lblIdUsuarioReasginar.Text = reasignar_Idusuario
                lblClave_SucursalReasginar.Text = reasignar_sucursal
                dt_Sucursales.PrimaryKey = New DataColumn() {dt_Sucursales.Columns("Clave_Sucursal")}
                fila = dt_Sucursales.Rows.Find(reasignar_sucursal)
                fn_LlenarDropDown(ddl_ReasignarSucursal, dt_Sucursales, False)
                ddl_ReasignarSucursal.Visible = True
                lblSucursal.Visible = True
                If fila IsNot Nothing Then
                    ddl_ReasignarSucursal.SelectedValue = reasignar_sucursal
                End If
                tbxCorreo.Text = correo
                lblcorreo.Visible = True
                tbxCorreo.Visible = True
                lblResp.Text = "RU"
                MmodalPanel.Focus()

            Case "CambiarCorreo"
                Dim IdUsuario As Integer = Convert.ToInt32(gv_CrearUsuarios.DataKeys(e.CommandArgument).Value)
                If IdUsuario = pId_Usuario Then Exit Sub

                Dim UsuarioNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(8).Text & " - NOMBRE DE SESION: " & gv_CrearUsuarios.Rows(Indice).Cells(7).Text
                Dim ClienteNombre As String = gv_CrearUsuarios.Rows(Indice).Cells(9).Text
                Dim UsuarioStatus As String = gv_CrearUsuarios.Rows(Indice).Cells(13).Text

                If UsuarioStatus = "BAJA" Then Exit Sub

                popup_click(sender, e)
                MmodalPanel.Visible = True
                ddl_ReasignarSucursal.Visible = False
                tb_usuarioReasignar.Text = gv_CrearUsuarios.Rows(Indice).Cells(7).Text
                Dim dt As DataTable = cn.Fn_UsuarioAReasignar(pClave_Corporativo, pClave_SucursalPropia, IdUsuario)
                headerModal.Text = "Cambiar Correo"
                Dim correo As String
                Dim reasignar_sucursal As String
                Dim reasignar_Idusuario As Integer
                For Each rows As DataRow In dt.Rows
                    reasignar_Idusuario = CInt(rows("Id_Usuario"))
                    reasignar_sucursal = CStr(rows("Clave_Sucursal"))
                    correo = CStr(rows("Mail"))
                Next
                lblIdUsuarioReasginar.Text = reasignar_Idusuario
                lblClave_SucursalReasginar.Text = reasignar_sucursal
                tbxCorreo.Text = correo
                tbxCorreo.Visible = True
                lblcorreo.Visible = True
                btnReasginar.Text = "Cambiar"
                lblResp.Text = "CC"

        End Select

    End Sub

    Private Sub popup_click(sender As Object, e As EventArgs) Handles lblHiddenReasignar.Click

    End Sub

    Private Sub Limpiar()
        gv_CrearUsuarios.DataSource = fn_CreaGridVacio("Id_Usuario,Usuario,Sesion,Cliente,ExpiraContra,Tipo,Nivel,ValidarIP,IP,Status")
        gv_CrearUsuarios.DataBind()
        gv_CrearUsuarios.SelectedIndex = -1
        'fst_CrearUsuario.Style.Add("height", "205px")
    End Sub

    Private Sub Sugerencia()

        'Dim Nombres As String() = Split(tbx_Nombre.Text, " ")
        'Dim NumArreglo As Integer = Nombres.Count
        'If NumArreglo > 2 Then
        '    Dim Sugerencia As String = Microsoft.VisualBasic.Left(tbx_Nombre.Text, 2) & Nombres(1)
        '    If Sugerencia.Length > 10 Then Sugerencia = Mid(Sugerencia, 1, 10)

        '    tbx_Sesion.Text = Replace(Sugerencia, "Ñ", "N")
        'Else
        '    tbx_Sesion.Text = ""
        'End If
        Dim session_id As String = ""

        session_id = cn.fn_Get_Session(pClave_Corporativo)
        If session_id <> "" Then
            tbx_Sesion.Text = session_id
        Else
            tbx_Sesion.Text = ""
        End If

    End Sub

    Private Sub BorrarDatos()
        ddl_Sucursales.SelectedValue = "0"
        ddl_Nivel.SelectedValue = "0"
        'ddl_Clientes.SelectedValue = "0"
        'ddl_Clientes.Enabled = False--comentado
        ddl_ModoConsulta.SelectedValue = "0"
        tbx_Nombre.Text = ""
        tbx_Sesion.Text = ""
        tbx_mail.Text = ""
    End Sub

    Private Sub ColorearBloqueados(ByRef gv As GridView)
        For Each Elem As GridViewRow In gv.Rows
            If Elem.Cells(13).Text = "BLOQUEADO" Then
                'Elem.BackColor = Drawing.Color.Orange
                '                Elem.CssClass = "usBlock"
                Elem.BackColor = Drawing.Color.LightGray
            ElseIf Elem.Cells(13).Text = "BAJA" Then
                Elem.BackColor = Drawing.Color.Salmon
            ElseIf Elem.Cells(13).Text = "ACTIVO" Then
                'Elem.BackColor = Drawing.Color.Green
                Elem.Cells(13).CssClass = "usActive"
            End If
        Next
    End Sub

    Protected Sub tbx_Sesion_TextChanged(sender As Object, e As EventArgs) Handles tbx_Sesion.TextChanged
        tbx_Sesion.Text = tbx_Sesion.Text.ToUpper
    End Sub

    Protected Sub ddl_Localidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Localidad.SelectedIndexChanged
        Dim dt_Sucursales As DataTable = cn.Fn_UsuariosClientes_ComboSucursales(pClave_Corporativo, ddl_Localidad.SelectedValue)
        If ddl_Localidad.SelectedValue <> 0 Then
            ddl_Sucursales.Enabled = True
            Call fn_LlenarDropDown(ddl_Sucursales, dt_Sucursales, False)
        Else
            Call fn_LlenarDropDownVacio(ddl_Sucursales)
            ddl_Sucursales.Enabled = False
            ddl_Sucursales.SelectedValue = 0
            ddl_Nivel.Enabled = False
            ddl_Nivel.SelectedIndex = 0
            tbx_Nombre.Enabled = False
            tbx_Nombre.Text = ""
            tbx_Sesion.Enabled = False
            tbx_Sesion.Text = ""
            ddl_ModoConsulta.Enabled = False
            ddl_ModoConsulta.SelectedIndex = 0
            tbx_mail.Enabled = False
            tbx_mail.Text = ""
        End If

    End Sub

    Protected Sub btnAdUsuarios_Click(sender As Object, e As EventArgs) Handles btnAdUsuarios.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub btnConsUsuarios_Click(sender As Object, e As EventArgs) Handles btnConsUsuarios.Click
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub ddl_LocalidadConsulta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_LocalidadConsulta.SelectedIndexChanged
        Dim dt_Sucursales As DataTable = cn.Fn_UsuariosClientes_ComboSucursales(pClave_Corporativo, ddl_LocalidadConsulta.SelectedValue)

        If ddl_LocalidadConsulta.SelectedValue <> 0 Then
            ddl_SucursalesConsulta.Enabled = True
            Call fn_LlenarDropDown(ddl_SucursalesConsulta, dt_Sucursales, True)
        Else
            Call fn_LlenarDropDownVacio(ddl_SucursalesConsulta)
            ddl_SucursalesConsulta.Enabled = False
            ddl_SucursalesConsulta.SelectedValue = 0
        End If
    End Sub

    Protected Sub BuscarUsuarios_Click(sender As Object, e As EventArgs) Handles BuscarUsuarios.Click
        If ddl_LocalidadConsulta.SelectedValue = "0" Then
            fn_Alerta("Seleccione una Localidad")
            Exit Sub
        End If
        Dim dt_Usuarios As DataTable = cn.fn_UsuariosSucursales(pClave_Corporativo, ddl_LocalidadConsulta.SelectedValue, ddl_SucursalesConsulta.SelectedValue)
        If dt_Usuarios.Rows.Count > 0 Then
            gv_CrearUsuarios.DataSource = fn_MostrarSiempre(dt_Usuarios)
            gv_CrearUsuarios.DataBind()
            gv_CrearUsuarios.Focus()
            Call ColorearBloqueados(gv_CrearUsuarios)
            pTabla("Resultado") = dt_Usuarios
        Else
            fn_Alerta("No se encontraron Usuarios en la Sucursal:" + ddl_SucursalesConsulta.SelectedItem.Text)
        End If

    End Sub

    Protected Sub ddl_ModoConsulta_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_ModoConsulta.SelectedIndexChanged
        tbx_mail.Enabled = True
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        MmodalPanel.Visible = False
        tb_usuarioReasignar.Text = ""
        fn_LlenarDropDownVacio(ddl_ReasignarSucursal)
    End Sub

    Protected Sub btnReasginar_Click(sender As Object, e As EventArgs) Handles btnReasginar.Click
        MmodalPanel.Visible = False

        If lblResp.Text = "RU" Then
            tb_usuarioReasignar.Text = ""
            ' fn_LlenarDropDownVacio(ddl_ReasignarSucursal)
            If (lblIdUsuarioReasginar.Text = "0" Or lblIdUsuarioReasginar.Text = "") AndAlso lblIdUsuarioReasginar.Text = "" Then
                fn_Alerta("Usuario no Existe")
                Exit Sub
            End If
            If (tbxCorreo.Text.Trim = "") Then
                fn_Alerta("Indique un correo electronico")
                Exit Sub
            End If

            Try
                cn.fn_UsuariosClientesReasignar_Update(CInt(lblIdUsuarioReasginar.Text), lblClave_SucursalReasginar.Text, ddl_ReasignarSucursal.SelectedValue, tbxCorreo.Text)
                cn.fn_Crear_Log(pId_Login, "Se reasigno usuario : " & lblIdUsuarioReasginar.Text & " / A Sucursal: " & lblClave_SucursalReasginar.Text)
                BuscarUsuarios_Click(sender, e)
            Catch ex As Exception
                fn_Alerta("Ocurrió un problema al intentar Reasignar el Usuario")
            End Try
        ElseIf lblResp.Text = "CC" Then
            Try
                cn.fn_UsuariosClienteCorreoUpdate(CInt(lblIdUsuarioReasginar.Text), tbxCorreo.Text)
                cn.fn_Crear_Log(pId_Login, "Se cambio corre de usuario : " & lblIdUsuarioReasginar.Text & " / A : " & tbxCorreo.Text)
                BuscarUsuarios_Click(sender, e)
            Catch ex As Exception

            End Try
        End If


    End Sub

    Protected Sub ddl_ReasignarSucrsal_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_ReasignarSucursal.SelectedIndexChanged
        MmodalPanel.Visible = True
        mPopUpReasignar.Show()

    End Sub


End Class