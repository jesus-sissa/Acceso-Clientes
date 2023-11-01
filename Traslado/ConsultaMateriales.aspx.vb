Public Partial Class ConsultaMateriales
    Inherits BasePage
    Dim Folio As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE MATERIALES")

        gv_ConsultaMat.DataSource = fn_CreaGridVacio("Folio,FechaSolicita,HoraSolicita,FechaEntrega,Cliente,Status,Remision")
        gv_ConsultaMat.DataBind()


        gv_Detalle.DataSource = fn_CreaGridVacio("Material,Cantidad")
        gv_Detalle.DataBind()

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()
        If dt_Clientes Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        txt_FechaInicial.Text = Date.Now
        txt_FechaFinal.Text = Date.Now
        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If

        fst_Materiales.Style.Add("height", "205px")
    End Sub

    Protected Sub ActualizarVentas()
        Dim FechaDesde As Date
        Dim FechaHasta As Date

        If Not IsDate(txt_FechaInicial.Text) OrElse Not DateTime.TryParse(txt_FechaInicial.Text, FechaDesde) Then
            Fn_Alerta("Debe capturar una fecha inicial valida.")
            Exit Sub
        End If

        If Not IsDate(txt_FechaFinal.Text) OrElse Not DateTime.TryParse(txt_FechaFinal.Text, FechaHasta) Then
            Fn_Alerta("Debe capturar una fecha final valida.")
            Exit Sub
        End If

        If ddl_Clientes.SelectedValue = 0 And cbx_Todos_Clientes.Checked = False Then
            Fn_Alerta("Seleccione un cliente o marque la casilla «Todos».")
            Exit Sub
        End If
        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL : " & FechaDesde & " AL: " & FechaHasta)

        fst_Materiales.Style.Remove("height")

        Dim dt_Ventas As DataTable = cn.fn_ConsultaMateriales_GetVentas(FechaDesde, FechaHasta, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)
        pTabla("tablaVentas") = dt_Ventas

        gv_ConsultaMat.DataSource = fn_MostrarSiempre(dt_Ventas)
        gv_ConsultaMat.DataBind()
        gv_ConsultaMat.SelectedIndex = -1

        If gv_ConsultaMat.Rows.Count < 4 Then
            fst_Materiales.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Detalle_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Detalle.PageIndexChanging
        Dim dt_Detalle As DataTable = pTabla("tablaDetalle")
        gv_Detalle.PageIndex = e.NewPageIndex
        gv_Detalle.DataSource = dt_Detalle
        gv_Detalle.DataBind()
        gv_Detalle.SelectedIndex = -1
    End Sub

    Protected Sub gv_ConsultaMat_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv_ConsultaMat.DataBound
        gv_Detalle.DataSource = fn_CreaGridVacio("Material,Cantidad")
        gv_Detalle.DataBind()
        gv_Detalle.SelectedIndex = -1
    End Sub

    Protected Sub gv_ConsultaMat_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_ConsultaMat.RowCreated
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Indice As Integer = e.Row.RowIndex
            Dim Status As String = If(IsDBNull(gv_ConsultaMat.DataKeys(Indice).Values(1)), String.Empty, gv_ConsultaMat.DataKeys(Indice).Values(1))

            'If Status <> "SOLICITADA" Then
            '    e.Row.Cells(0).Controls.Clear()
            'End If
        End If

    End Sub

    Protected Sub gv_ConsultaMat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_ConsultaMat.SelectedIndexChanged
        If IsDBNull(gv_ConsultaMat.DataKeys(0).Value) OrElse CStr(gv_ConsultaMat.DataKeys(0).Value) = "" OrElse CStr(gv_ConsultaMat.DataKeys(0).Value) = "0" Then Exit Sub

        Folio = gv_ConsultaMat.SelectedDataKey("Folio")

        Dim dt_Detalle As DataTable = fn_MostrarSiempre(cn.fn_ConsultaMateriales_GetDetalle(Folio))
        gv_Detalle.DataSource = dt_Detalle
        gv_Detalle.DataBind()
        gv_Detalle.SelectedIndex = -1
        'btnImprimir.Enabled = True
        pTabla("tablaDetalle") = dt_Detalle
    End Sub

    Protected Sub gv_ConsultaMat_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_ConsultaMat.PageIndexChanging
        Dim dt_Ventas As DataTable = pTabla("tablaVentas")

        gv_ConsultaMat.PageIndex = e.NewPageIndex
        gv_ConsultaMat.DataSource = dt_Ventas
        gv_ConsultaMat.DataBind()
        gv_ConsultaMat.SelectedIndex = -1

        If gv_ConsultaMat.Rows.Count < 4 Then
            fst_Materiales.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click
        Call ActualizarVentas()
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Private Sub Limpiar()
        fst_Materiales.Style.Add("height", "205px")
        gv_ConsultaMat.DataSource = fn_CreaGridVacio("Folio,FechaSolicita,HoraSolicita,FechaEntrega,Cliente,Status,Remision")
        gv_ConsultaMat.DataBind()
        gv_ConsultaMat.SelectedIndex = -1
        gv_Detalle.DataSource = fn_CreaGridVacio("Material,Cantidad")
        gv_Detalle.DataBind()
        gv_Detalle.SelectedIndex = -1
        'btnImprimir.Enabled = False
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FechaInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FechaFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaFinal.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub gv_ConsultaMat_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_ConsultaMat.RowCommand
        If IsDBNull(gv_ConsultaMat.DataKeys(0).Value) OrElse CStr(gv_ConsultaMat.DataKeys(0).Value) = "" OrElse CStr(gv_ConsultaMat.DataKeys(0).Value) = "0" Then Exit Sub
        Select Case e.CommandName.ToUpper
            Case "VERDES"
                'Num_Remision = gv_ConsultaMat.SelectedDataKey("Folio").ToString()
                Num_Remision = gv_ConsultaMat.DataKeys(e.CommandArgument).Value
                Tipo_Remision = "MAT"
                'Num_Remision = gv_ConsultaMat.Rows(e.CommandArgument).Cells(1).Text
                Response.Redirect("ImprimirR.aspx")

        End Select
    End Sub

    'Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
    '    Response.Redirect("ImprimirR.aspx")
    'End Sub

    'Private Sub btn_Imprimir_ServerClick(sender As Object, e As EventArgs) Handles btn_Imprimir.ServerClick
    '    Response.Redirect("ImprimirR.aspx")
    'End Sub

End Class
