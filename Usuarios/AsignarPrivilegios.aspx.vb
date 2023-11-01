Public Partial Class PrivilegiosUsuarios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub

        'Se la agrege para que no guarde Cache
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: PRIVILEGIOS USUARIOS")
        '  If pNivel = 2 Then Response.Redirect("~/Default.aspx")
        Dim dt_LocalidadConsulta As DataTable = cn.fn_LocalidadCorporativo(pClave_SucursalPropia)
        Call fn_LlenarDropDown(ddl_LocalidadConsulta, dt_LocalidadConsulta, False)
        If dt_LocalidadConsulta.Rows.Count > 0 Then
            ddl_LocalidadConsulta.SelectedIndex = 1
        End If
        ddl_LocalidadConsulta_SelectedIndexChanged(sender, e)

        gv_PrivilegiosOtorgados.DataSource = fn_CreaGridVacio("Id_Menu,Descripcion,Enlace,Id_Opcion")
        gv_PrivilegiosOtorgados.DataBind()

    End Sub

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Agregar.Click

        If gv_Usuarios.SelectedIndex = -1 Then
            Fn_Alerta("Seleccione un Usuario.")
            Exit Sub
        End If

        Dim OpcionChecked As Boolean = False
        For Each Opcion As GridViewRow In gv_Opciones.Rows
            OpcionChecked = DirectCast(Opcion.FindControl("chk_Opciones"), CheckBox).Checked

            If Not OpcionChecked Then Continue For
            Dim Id_Opcion As Integer = gv_Opciones.DataKeys(Opcion.RowIndex).Value
            cn.fn_ClientesPermisos_Agregar(gv_Usuarios.SelectedDataKey.Value, Id_Opcion, "A")
        Next

        Call Desmarcar_OpcionesCheckBox()
        Call CargarPrivilegiosOtorgados()
    End Sub

    Protected Sub btn_Eliminar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Eliminar.Click

        If gv_Usuarios.SelectedIndex = -1 Then
            Fn_Alerta("Seleccione un Usuario.")
            Exit Sub
        End If

        If IsDBNull(gv_PrivilegiosOtorgados.DataKeys(0).Value) OrElse CStr(gv_PrivilegiosOtorgados.DataKeys(0).Value) = "" _
            OrElse CStr(gv_PrivilegiosOtorgados.DataKeys(0).Value) = "0" Then
            Fn_Alerta("El Usuario no Cuenta con Privilegios a Eliminar.")
            Exit Sub
        End If

        Dim OpcionChecked As Boolean = False
        For Each Opcion As GridViewRow In gv_PrivilegiosOtorgados.Rows
            OpcionChecked = DirectCast(Opcion.FindControl("chk_PrivilegioAsignado"), CheckBox).Checked

            If Not OpcionChecked Then Continue For
            Dim Id_Opcion As Integer = gv_PrivilegiosOtorgados.DataKeys(Opcion.RowIndex).Value
            cn.fn_ClientesPermisos_Agregar(gv_Usuarios.SelectedDataKey.Value, Id_Opcion)
        Next
        Call CargarPrivilegiosOtorgados()
        btn_Eliminar.Enabled = False
    End Sub

    Protected Sub gv_Usuarios_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Usuarios.PageIndexChanging

        gv_Usuarios.PageIndex = e.NewPageIndex
        gv_Usuarios.DataSource = pTabla("Usuarios")
        gv_Usuarios.DataBind()
        gv_Usuarios.SelectedIndex = -1

        If gv_Usuarios.Rows.Count < 4 Then
            fst_Usuarios.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub checkPrivilegio_CheckedChanged(sender As Object, e As EventArgs)
        If IsDBNull(gv_Opciones.DataKeys(0).Value) OrElse CStr(gv_Opciones.DataKeys(0).Value) = "" _
            OrElse CStr(gv_Opciones.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Marcado As Boolean = False

        For Each rowPrivilegio As GridViewRow In gv_Opciones.Rows
            Dim cb As CheckBox = CType(rowPrivilegio.FindControl("chk_Opciones"), CheckBox)

            If cb.Checked Then
                Marcado = True
                Exit For
            End If
        Next

        btn_Agregar.Enabled = Marcado

    End Sub

    Protected Sub checkPrivilegioAsignado_CheckedChanged(sender As Object, e As EventArgs)
        If IsDBNull(gv_PrivilegiosOtorgados.DataKeys(0).Value) OrElse CStr(gv_PrivilegiosOtorgados.DataKeys(0).Value) = "" _
         OrElse CStr(gv_PrivilegiosOtorgados.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Marcado As Boolean = False
        For Each rowPrivilegioAsignado As GridViewRow In gv_PrivilegiosOtorgados.Rows
            Dim cb As CheckBox = CType(rowPrivilegioAsignado.FindControl("chk_PrivilegioAsignado"), CheckBox)

            If cb.Checked Then
                Marcado = True
                Exit For
            End If
        Next
        btn_Eliminar.Enabled = Marcado

    End Sub

    Private Sub Desmarcar_OpcionesCheckBox()
        For Each rowPrivilegio As GridViewRow In gv_Opciones.Rows
            Dim cb As CheckBox = CType(rowPrivilegio.FindControl("chk_Opciones"), CheckBox)
            cb.Checked = False
        Next
        btn_Agregar.Enabled = False
    End Sub

    'Function RevisarMarcados(ByRef Grillas As GridView) As Boolean

    '    Dim Marcado As Boolean = False
    '    For Each Fila As GridViewRow In Grillas.Rows
    '        Marcado = DirectCast(Fila.FindControl("chk_Usuario"), CheckBox).Checked
    '        If Marcado Then
    '            Return True
    '        Else
    '            Continue For
    '        End If

    '    Next

    'End Function

    Sub CargarPrivilegiosOtorgados()
        Dim dt_Privilegios As DataTable = cn.fn_OpcionesClientes_Consultar(0, gv_Usuarios.SelectedDataKey.Value)

        If dt_Privilegios Is Nothing Then
            Fn_Alerta("No se pueden mostrar las opciones del Menú")
            Exit Sub
        End If

        pTabla("PrivilegiosOtorgados") = dt_Privilegios
        gv_PrivilegiosOtorgados.DataSource = fn_MostrarSiempre(dt_Privilegios)
        gv_PrivilegiosOtorgados.DataBind()

    End Sub

    Sub Limpiar_PrivilegiosOtorgados()

        gv_PrivilegiosOtorgados.DataSource = fn_CreaGridVacio("Id_Menu,Descripcion,Enlace,Id_Opcion")
        gv_PrivilegiosOtorgados.DataBind()
    End Sub

    Sub LlenarOpciones()

        Dim dt_Menus As DataTable = cn.fn_Opciones_Consultar()
        If dt_Menus Is Nothing Then
            Fn_Alerta("Ocurrió un error al cargar los datos.")
            Exit Sub
        End If

        pTabla("Opciones") = dt_Menus
        gv_Opciones.DataSource = fn_MostrarSiempre(dt_Menus)
        gv_Opciones.DataBind()
        gv_Opciones.Focus()

    End Sub

    Protected Sub gv_Usuarios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Usuarios.SelectedIndexChanged
        Call Limpiar_PrivilegiosOtorgados()
        If IsDBNull(gv_Usuarios.DataKeys(0).Value) OrElse CStr(gv_Usuarios.DataKeys(0).Value) = "" OrElse CStr(gv_Usuarios.DataKeys(0).Value) = "0" Then Exit Sub

        If gv_Usuarios.SelectedRow.Cells(6).Text = "ACTIVO" Then
            Call CargarPrivilegiosOtorgados()
            pPrivilegios.Visible = True
        Else
            fn_Alerta("Usuario Bloqueado")
            pPrivilegios.Visible = False
        End If

    End Sub

    Protected Sub BuscarUsuarios_Click(sender As Object, e As EventArgs) Handles BuscarUsuarios.Click
        If ddl_LocalidadConsulta.SelectedValue = "0" Then
            fn_Alerta("Seleccione una Localidad")
            Exit Sub
        End If
        Dim dt_Usuarios As DataTable = cn.fn_UsuariosSucursales(pClave_Corporativo, ddl_LocalidadConsulta.SelectedValue, ddl_SucursalesConsulta.SelectedValue)
        If dt_Usuarios.Rows.Count > 0 Then
            gv_Usuarios.DataSource = fn_MostrarSiempre(dt_Usuarios)
            gv_Usuarios.DataBind()
            gv_Usuarios.Focus()
            Call LlenarOpciones()
            pUsuarios.Visible = True
            pPrivilegios.Visible = False
            ColorearBloqueados(gv_Usuarios)
            pTabla("Usuarios") = dt_Usuarios
        Else
            fn_Alerta("No se encontraron Usuarios en la Sucursal:" + ddl_SucursalesConsulta.SelectedItem.Text)
        End If

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

    Private Sub ColorearBloqueados(ByRef gv As GridView)
        For Each Elem As GridViewRow In gv.Rows
            If Elem.Cells(6).Text = "BLOQUEADO" Then
                'Elem.BackColor = Drawing.Color.Orange
                Elem.CssClass = "usBlock"
            ElseIf Elem.Cells(6).Text = "SUSPENDIDO" Then
                Elem.BackColor = Drawing.Color.Salmon
            ElseIf Elem.Cells(6).Text = "ACTIVO" Then
                'Elem.BackColor = Drawing.Color.Green
                Elem.Cells(6).CssClass = "usActive"
            End If
        Next
    End Sub
End Class