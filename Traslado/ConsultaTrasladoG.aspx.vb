Partial Public Class ConsultaTrasladoG
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE TRASLADO GLOBAL")

        Call Limpiar()

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()

        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
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

    End Sub

    Protected Sub gv_Lista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.SelectedIndexChanged
        If IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Id_Remision As Integer = gv_Lista.SelectedDataKey("Id_Remision")
        Dim dt_Monedas As DataTable = cn.fn_ConsultaTraslado_GetMonedas(Id_Remision)
        pTabla("TablaMonedas") = dt_Monedas

        gvMonedas.DataSource = fn_MostrarSiempre(dt_Monedas)
        gvMonedas.DataBind()

        Dim dt_Envases As DataTable = cn.fn_ConsultaTraslado_GetEnvases(Id_Remision)
        pTabla("TablaEnvases") = dt_Envases

        gvEnvases.DataSource = fn_MostrarSiempre(dt_Envases)
        gvEnvases.DataBind()

        ViewState("RutaIndice") = gv_Lista.SelectedIndex
        ViewState("RutaPagina") = gv_Lista.PageIndex
    End Sub

    Sub Limpiar()
        fst_TrasladosG.Style.Add("height", "205px")

        gv_Lista.DataSource = fn_CreaGridVacio("Id_Remision,Remision,Fecha,Hora,Origen,Destino,Importe")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1

        gvMonedas.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Efectivo,Documentos,Tipo Cambio")
        gvMonedas.DataBind()

        gvEnvases.DataSource = fn_CreaGridVacio("Id_Envase,Tipo Envase,Numero,IDTE")
        gvEnvases.DataBind()
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click

        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If (Not Date.TryParse(txt_FechaInicial.Text, FechaInicial)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If (Not Date.TryParse(txt_FechaFinal.Text, FechaFinal)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If FechaInicial > FechaFinal Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = 0 Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim Cliente As String = If(cbx_Todos_Clientes.Checked, "TODOS", ddl_Clientes.SelectedItem.Text)
        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente)

        Dim dt_Traslados As DataTable = cn.fn_ConsultaTrasladoG_GetPuntos(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)
        If dt_Traslados Is Nothing Then
            fn_Alerta("Ocurrio un error al cargar los datos.")
            Exit Sub
        End If
        If dt_Traslados.Rows.Count = 0 Then
            fn_Alerta("No se encontró informacion con los filtros establecidos.")
        End If
        fst_TrasladosG.Style.Remove("height")

        pTabla("Lista") = dt_Traslados
        gv_Lista.SelectedIndex = -1

        gv_Lista.DataSource = fn_MostrarSiempre(dt_Traslados)
        gv_Lista.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_TrasladosG.Style.Add("height", "205px")
        End If

    End Sub

    Protected Sub gv_Lista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        Dim dt_Traslados As DataTable = pTabla("Lista")
        Dim RutaIndice As Integer = ViewState("RutaIndice")
        Dim RutaPagina As Integer = ViewState("RutaPagina")

        If RutaPagina = e.NewPageIndex Then
            gv_Lista.SelectedIndex = RutaIndice
        Else
            gv_Lista.SelectedIndex = -1
        End If

        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(dt_Traslados)
        gv_Lista.DataBind()

        fst_TrasladosG.Style.Remove("height")
        If gv_Lista.Rows.Count < 4 Then
            fst_TrasladosG.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Lista_DataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.DataBinding
        gv_Lista.SelectedIndex = -1

        gvEnvases.DataSource = fn_CreaGridVacio("Id_Envase,Tipo Envase,Numero,IDTE")
        gvEnvases.DataBind()

        gvMonedas.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Efectivo,Documentos,Tipo Cambio")
        gvMonedas.DataBind()

    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = 0
    End Sub

    Protected Sub btn_Exportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Exportar.Click

        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If (Not Date.TryParse(txt_FechaInicial.Text, FechaInicial)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If (Not Date.TryParse(txt_FechaFinal.Text, FechaFinal)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = 0 Then
                Fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim Cliente As String = If(cbx_Todos_Clientes.Checked, "TODOS", ddl_Clientes.SelectedValue)
        Call cn.fn_Crear_Log(pId_Login, "EXPORTO DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente)

        Dim dt_Temporal As DataTable = cn.fn_ConsultaTrasladoG_GetPuntos(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)

        If dt_Temporal.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        fn_Exportar_Excel(dt_Temporal, Page.Title, "Desde: " & FechaInicial, "Hasta: " & FechaFinal, 1)

    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FechaFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaFinal.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FechaInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaInicial.TextChanged
        Call Limpiar()
    End Sub
End Class