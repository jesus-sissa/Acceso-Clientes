Imports System.Data

Partial Public Class ConsultaAnomalias
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE ANOMALIAS")


        Dim dt_Localidad As DataTable = cn.fn_LocalidadCorporativo(pClave_SucursalPropia)
        Dim dt_LocalidadConsulta As DataTable = cn.fn_LocalidadCorporativo(pClave_SucursalPropia)
        Call fn_LlenarDropDown(ddl_Localidad, dt_Localidad, False)
        If dt_Localidad.Rows.Count > 0 Then
            ddl_Localidad.SelectedIndex = 1
        End If
        ddl_Localidad_SelectedIndexChanged(sender, e)
        Call fn_LlenarDropDown(ddl_Localidad, dt_LocalidadConsulta, False)
        If dt_LocalidadConsulta.Rows.Count > 0 Then
            ddl_Localidad.SelectedIndex = 1
        End If
        ddl_Localidad_SelectedIndexChanged(sender, e)
        'fn_CreaGridVacio()
    End Sub

    Private Sub limpiar()

        'dl_Detalle.DataSource = fn_CreaGridVacio("Id_RIAD,Id_RIA,Fecha,Tipo,Entidad,Id_Entidad,Descripcion")

    End Sub

    Protected Sub gvTickets_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvTickets.RowCommand
        If IsDBNull(gvTickets.DataKeys(0).Value) OrElse CStr(gvTickets.DataKeys(0).Value) = "" OrElse CStr(gvTickets.DataKeys(0).Value) = "0" Then Exit Sub

        Select Case e.CommandName.ToUpper
            Case "SELECT"
                Dim dtDetalles As DataTable = cn.fn_Detalle_Ticket(e.CommandArgument)
                fst_DetalleMateriales.Visible = True
                If dtDetalles.Rows.Count > 0 Then

                    gv_Detalle.DataSource = fn_MostrarSiempre(dtDetalles)
                Else
                    gv_Detalle.DataSource = fn_CreaGridVacio("Fecha,Hora,Descripcion")
                End If
                gv_Detalle.DataBind()
        End Select
    End Sub

    Protected Sub ddl_Localidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Localidad.SelectedIndexChanged
        Dim dt_Sucursales As DataTable = cn.Fn_UsuariosClientes_ComboSucursales(pClave_Corporativo, ddl_Localidad.SelectedValue)

        If ddl_Localidad.SelectedValue <> 0 Then
            ddl_Sucursales.Enabled = True
            If dt_Sucursales.Rows.Count > 0 Then

                Call fn_LlenarDropDown(ddl_Sucursales, dt_Sucursales, True)
                If pNivel = 2 Then
                    ddl_Sucursales.SelectedValue = pClave_Sucursal
                    ddl_Sucursales.Enabled = False
                Else
                    ddl_Sucursales.SelectedIndex = 0
                    ddl_Sucursales.Enabled = True
                End If
            Else
                Call fn_LlenarDropDownVacio(ddl_Sucursales)
                ddl_Sucursales.Enabled = False
                ddl_Sucursales.SelectedValue = 0

            End If
        End If
    End Sub

    Protected Sub ddl_Sucursales_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Sucursales.SelectedIndexChanged


    End Sub

    Protected Sub btn_Guardar_Click(sender As Object, e As EventArgs) Handles btn_Guardar.Click
        Dim dtTickets As DataTable = Nothing
        If ddl_Sucursales.SelectedIndex = 0 Then
            dtTickets = cn.fn_Lista_Ticket(pNombre_Usuario, 0, pClave_Corporativo)
        Else
            dtTickets = cn.fn_Lista_Ticket(pNombre_Usuario, ddl_Sucursales.SelectedValue, pClave_Corporativo)
        End If

        If dtTickets.Rows.Count > 0 Then
            gvTickets.DataSource = fn_MostrarSiempre(dtTickets)
            gvTickets.DataBind()
        Else
            gvTickets.DataSource = fn_CreaGridVacio("Ticket,Descripcion,Status,Nombre_Sucursal,Fecha")
            gvTickets.DataBind()
        End If

    End Sub




    'Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click

    '    If Not fn_validar() Then Exit Sub
    '    Dim StatusLog As String = "TODOS"
    '    Dim FechaInicial As Date = Date.Parse(txt_FInicial.Text)
    '    Dim FechaFinal As Date = Date.Parse(txt_FFinal.Text)
    '    Dim Status As Char = If(cbx_Todos.Checked, "T", ddl_Tipo.SelectedItem.Value)

    '    If Not cbx_Todos.Checked Then StatusLog = ddl_Tipo.SelectedItem.Text
    '    Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL : " & FechaInicial & " AL " & FechaFinal & "; STATUS: " & StatusLog)

    '    Dim dt_Anomalias As DataTable = cn.fn_ConsultaAnomalias_GetAnomalias(FechaInicial, FechaFinal, Status)

    '    pTabla("tblAnomalias") = dt_Anomalias
    '    fst_Anomalias.Style.Remove("height")

    '    gv_Anomalias.DataSource = fn_MostrarSiempre(dt_Anomalias)
    '    gv_Anomalias.DataBind()
    '    gv_Anomalias.SelectedIndex = -1

    '    If gv_Anomalias.Rows.Count < 4 Then
    '        fst_Anomalias.Style.Add("height", "205px")
    '    End If
    'End Sub

    'Protected Sub gv_Anomalias_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Anomalias.SelectedIndexChanged
    '    If IsDBNull(gv_Anomalias.DataKeys(0).Value) OrElse CStr(gv_Anomalias.DataKeys(0).Value) = "" OrElse CStr(gv_Anomalias.DataKeys(0).Value) = "0" Then Exit Sub

    '    Dim dt_Detalle As DataTable = cn.fn_ConsultaAnomalias_GetDetalle(gv_Anomalias.SelectedDataKey("Id_RIA"))

    '    pTabla("tblDetalle") = dt_Detalle

    '    dl_Detalle.DataSource = fn_MostrarSiempre(dt_Detalle)
    '    dl_Detalle.DataBind()
    'End Sub

    'Protected Sub gv_Anomalias_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Anomalias.PageIndexChanging
    '    gv_Anomalias.PageIndex = e.NewPageIndex
    '    gv_Anomalias.DataSource = pTabla("tblAnomalias")
    '    gv_Anomalias.DataBind()
    '    gv_Anomalias.SelectedIndex = -1

    '    If gv_Anomalias.Rows.Count < 4 Then
    '        fst_Anomalias.Style.Add("height", "205px")
    '    End If
    'End Sub

    'Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
    '    Call limpiar()
    'End Sub

    'Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
    '    Call limpiar()
    'End Sub

    'Protected Sub ddl_Tipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Tipo.SelectedIndexChanged
    '    Call limpiar()
    'End Sub

    'Protected Sub btn_Guardar_Click(sender As Object, e As EventArgs) Handles btn_Guardar.Click

    'End Sub
End Class