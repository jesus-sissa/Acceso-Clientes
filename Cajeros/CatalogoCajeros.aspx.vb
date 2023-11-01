Partial Public Class CatalogoCajeros
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: CATALOGO DE CAJEROS ATMS")
        Call MostrargridVacio()

    End Sub

    Protected Sub gv_Lista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.SelectedIndexChanged
        If IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_Config As DataTable = cn.fn_CatalogoConfigLlenalista(gv_Lista.SelectedDataKey("Id_Cajero"))
        gv_Config.DataSource = fn_MostrarSiempre(dt_Config)
        gv_Config.DataBind()
        gv_Config.SelectedIndex = -1

    End Sub

    Sub MostrargridVacio()
        div_cajeros.Style.Add("height", "205px")
        gv_Lista.DataSource = fn_CreaGridVacio("Id_Cajero,Caja,No. cajero,Descripcion,Direccion,Ciudad,Status,Latitud,Longitud")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1

        gv_Config.DataSource = fn_CreaGridVacio("Id_Caset,Denominacion,Caset,Capacidad,Saldo,Status")
        gv_Config.DataBind()
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click

        If pId_CajaBancaria <= 0 Then Exit Sub

        If cbx_Todos.Checked = False AndAlso ddl_Status.SelectedValue = "0" Then
            Fn_Alerta("Seleccione un Status.")
            Exit Sub
        End If
        div_cajeros.Style.Remove("height")

        Dim Status As String = If(cbx_Todos.Checked, "T", ddl_Status.SelectedValue)

        Dim dt_Cajeros As DataTable = cn.fn_CatalogoCajeroLlenalista(pId_CajaBancaria, Status)
        pTabla("Lista") = dt_Cajeros
        gv_Lista.SelectedIndex = -1
        gv_Lista.DataSource = fn_MostrarSiempre(dt_Cajeros)
        gv_Lista.DataBind()

        Dim statusLog As String = If(cbx_Todos.Checked, "TODOS", ddl_Status.SelectedItem.Text)
        Call cn.fn_Crear_Log(pId_Login, "CONSULTO: ESTATUS: " & statusLog & "; REGISTROS: " & dt_Cajeros.Rows.Count)

        If gv_Lista.Rows.Count < 4 Then
            div_cajeros.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Lista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        Dim dt_Cajeros As DataTable = pTabla("Lista")
        Dim RutaIndice As Integer = ViewState("RutaIndice")
        Dim RutaPagina As Integer = ViewState("RutaPagina")

        If RutaPagina = e.NewPageIndex Then
            gv_Lista.SelectedIndex = RutaIndice
        Else
            gv_Lista.SelectedIndex = -1
        End If

        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(dt_Cajeros)
        gv_Lista.DataBind()

        gv_Config.DataSource = fn_CreaGridVacio("Id_Caset,Denominacion,Caset,Capacidad,Saldo,Status")
        gv_Config.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            div_cajeros.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos.CheckedChanged
        ddl_Status.Enabled = Not cbx_Todos.Checked
        ddl_Status.SelectedValue = 0
        Call MostrargridVacio()
    End Sub

    Protected Sub btn_Exportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Exportar.Click

        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If cbx_Todos.Checked = False AndAlso ddl_Status.SelectedValue = "0" Then
            Fn_Alerta("Seleccione un Status.")
            Exit Sub
        End If

        Dim Status As String = If(cbx_Todos.Checked, "T", ddl_Status.SelectedValue)
        Dim dt_Temporal As DataTable = cn.fn_CatalogoCajeroLlenalista(pId_CajaBancaria, Status)

        If dt_Temporal.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        Dim statusLog As String = If(cbx_Todos.Checked, "TODOS", ddl_Status.SelectedItem.Text)
        Call cn.fn_Crear_Log(pId_Login, "EXPORTO: ESTATUS: " & statusLog & "; REGISTROS: " & dt_Temporal.Rows.Count)
        Call fn_Exportar_Excel(dt_Temporal, "CATALOGO CAJEROS ATMS", "Desde: " & FechaInicial, "Hasta: " & FechaFinal)

    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call MostrargridVacio()
    End Sub

End Class