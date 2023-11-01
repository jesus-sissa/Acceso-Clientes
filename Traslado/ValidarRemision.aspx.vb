Public Class ValidarRemision
    Inherits BasePage
    Dim FechaDesde As Date
    Dim FechaHasta As Date
    Dim Folio As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Return
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: AUTORIZAR MATERIALES")
        Dim dt_SucursalesPropias As DataTable = cn.fn_SucursalesPropias_Get

        If dt_SucursalesPropias Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        txt_FechaInicial.Text = Date.Now
        txt_FechaFinal.Text = Date.Now
        'Call fn_LlenarDropDown(ddl_SucursalPropia, dt_SucursalesPropias)
        Grid_vacio()
        ''fn_LlenarSolicitudes()

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se pudo consultar la informacion de la lista de clientes.")
            Exit Sub
        End If
        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Cli.Enabled = False
            ddl_Cli.SelectedValue = pId_ClienteOriginal
        End If
        Call fn_LlenarDropDown(ddl_Cli, dt_Clientes, False)
    End Sub
    Sub MostrarGrid_Vacios()
        gv_Solicitudes.DataSource = fn_CreaGridVacio("Folio,Fecha,Hora,Envases,Importe,Solicita")
        gv_Solicitudes.DataBind()
        gv_Solicitudes.SelectedIndex = -1
        'Call LimpiarDetalle()
    End Sub
    Sub Grid_vacio()
        Remision_det.Visible = False
        gv_Solicitudes.DataSource = fn_CreaGridVacio("Folio,Fecha,Hora,Envases,Importe,Solicita,Destino")
        gv_Solicitudes.DataBind()
        gv_Solicitudes.SelectedIndex = -1
    End Sub
    Protected Sub fn_LlenarSolicitudes()
        DateTime.TryParse(txt_FechaInicial.Text, FechaDesde)
        DateTime.TryParse(txt_FechaFinal.Text, FechaHasta)
        Dim dt_Solicitudes As DataTable = cn.fn_AutorizarRemisionesGet(FechaDesde, FechaHasta, ddl_Clientes.SelectedValue, ddl_Cli.SelectedValue)
        fst_AutorizarMateriales.Style.Remove("height")

        If dt_Solicitudes Is Nothing Then
            Remision_det.Visible = False
            gv_Solicitudes.DataSource = fn_CreaGridVacio("Folio,Fecha,Hora,Envases,Importe,Solicita,Destino")
            gv_Solicitudes.DataBind()
        Else
            gv_Solicitudes.DataSource = fn_MostrarSiempre(dt_Solicitudes)
            gv_Solicitudes.DataBind()
            pTabla("tablaSolicitudesR") = dt_Solicitudes
        End If
    End Sub

    Protected Sub gv_Solicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Solicitudes.RowCommand
        If IsDBNull(gv_Solicitudes.DataKeys(0).Value) OrElse CStr(gv_Solicitudes.DataKeys(0).Value) = "" OrElse CStr(gv_Solicitudes.DataKeys(0).Value) = "0" Then Exit Sub
        Mensaje.Text = ""
        Dim Indice As Integer = e.CommandArgument
        Select Case e.CommandName.ToUpper
            Case "VERDETALLE"
                ''>>
                If ddl_Clientes.SelectedValue = "T" Then
                    Btn_edit.Visible = False
                Else
                    Btn_edit.Visible = True
                    Btn_edit.Text = "Editar Envases"
                End If
                ''<<
                Remision_det.Visible = True
                gv_Solicitudes.SelectedIndex = Indice
                Folio = Integer.Parse(gv_Solicitudes.SelectedDataKey("Folio"))
                gvMonedas.DataSource = fn_MostrarSiempre(cn.fn_Tv_Remisionesweb_GetMonedas(Folio))
                gvMonedas.DataBind()

                gvEnvases.DataSource = fn_MostrarSiempre(cn.fn_TV_Remisionesweb_GetEnvases(Folio))
                gvEnvases.DataBind()
                gvEnvases.Columns(2).Visible = False
            Case "AUTORIZAR"
                If (ddl_Clientes.SelectedValue = "A") Then
                    gv_Solicitudes.SelectedIndex = Indice
                    If (cn.fn_AutorizarR(Integer.Parse(gv_Solicitudes.SelectedDataKey("Folio")))) Then
                        Remision_det.Visible = False
                        fn_Alerta("La Remision se ha creado con exito, y se ha enviado la peticion correctamente.")
                    End If
                Else
                    Mensaje.Text = "La remision ya se encuentra validada."
                    Exit Sub
                End If

            Case "CANCELAR"
                If (ddl_Clientes.SelectedValue = "A" Or ddl_Clientes.SelectedValue = "V") Then
                    gv_Solicitudes.SelectedIndex = Indice
                    If (cn.fn_Tv_RemisionesWeb_Delete(Integer.Parse(gv_Solicitudes.SelectedDataKey("Folio")))) Then
                        Remision_det.Visible = False
                        fn_Alerta("La Remisón se eliminó correctamente")
                    End If
                Else
                    Exit Sub
                End If
        End Select
        fn_LlenarSolicitudes()
        ' gv_Solicitudes.SelectedIndex = Indice
    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click
        Buscar()
    End Sub
    Sub Buscar()
        If Not IsDate(txt_FechaInicial.Text) OrElse Not DateTime.TryParse(txt_FechaInicial.Text, FechaDesde) Then
            fn_Alerta("Debe capturar una fecha inicial valida.")
            Exit Sub
        End If

        If Not IsDate(txt_FechaFinal.Text) OrElse Not DateTime.TryParse(txt_FechaFinal.Text, FechaHasta) Then
            fn_Alerta("Debe capturar una fecha final valida.")
            Exit Sub
        End If
        If ddl_Cli.SelectedValue = 0 Then
            fn_Alerta("Seleccione un cliente")
            Exit Sub
        End If
        If ddl_Clientes.SelectedValue = "0" Then
            fn_Alerta("Seleccione un status para realizar la busqueda.")
            Exit Sub
        End If

        fn_LlenarSolicitudes()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Grid_vacio()
    End Sub

    Protected Sub Btn_edit_Click(sender As Object, e As EventArgs) Handles Btn_edit.Click
        If Btn_edit.Text = "Guardar Cambios" Then
            GuardarEnvasesNuevos()
            ''>>
            gvEnvases.DataSource = fn_MostrarSiempre(cn.fn_TV_Remisionesweb_GetEnvases(gv_Solicitudes.SelectedDataKey("Folio")))
            gvEnvases.DataBind()
            ''<<
            gvEnvases.Columns(2).Visible = False
            Btn_edit.Text = "Editar Envases"
        Else
            Dim txt As TextBox = gvEnvases.Rows(0).Cells(2).FindControl("tbx_NEW")
            txt.Focus()
            gvEnvases.Columns(2).Visible = True
            Btn_edit.Text = "Guardar Cambios"
        End If       
        '' Response.Redirect("~/Soporte/Notificaciones.aspx")
    End Sub
    Sub GuardarEnvasesNuevos()
        Dim txt As TextBox
        For Each row In gvEnvases.Rows
            txt = row.Cells(2).FindControl("tbx_NEW")
            If txt.Text.Trim <> "" AndAlso txt.Text <> row.cells(1).text Then
                cn.fn_Tv_ModificarNumeros(gv_Solicitudes.SelectedDataKey("Folio"), row.Cells(1).text, txt.Text)
            End If
            'MsgBox(txt.Text)
        Next
    End Sub

    Protected Sub gv_Solicitudes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging
        Dim dt_solicitudes As DataTable = pTabla("tablaSolicitudesR")

        gv_Solicitudes.PageIndex = e.NewPageIndex
        gv_Solicitudes.DataSource = dt_solicitudes
        gv_Solicitudes.DataBind()
        gv_Solicitudes.SelectedIndex = -1

        If gv_Solicitudes.Rows.Count < 4 Then
            gv_Solicitudes.Style.Add("height", "205px")
        End If
    End Sub
End Class