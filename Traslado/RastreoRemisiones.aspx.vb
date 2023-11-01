Partial Public Class RastreoRemisiones1
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: RASTREO DE REMISIONES")

        gv_Importes.DataSource = fn_CreaGridVacio("Moneda,Importe,Documentos")
        gv_Importes.DataBind()

        gv_Envases.DataSource = fn_CreaGridVacio("Tipo,Numero")
        gv_Envases.DataBind()

        gv_Log.DataSource = fn_CreaGridVacio("Concepto,Fecha,Hora")
        gv_Log.DataBind()

        txt_Remision.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        txt_NumeroRemision.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        txt_Importe.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        txt_EnvasesSN.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

    End Sub

    Protected Sub btn_Buscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Buscar.Click
        If (txt_Remision.Text.Trim = "") Then
            Fn_Alerta("Debe capturar un numero de remisión")
            Exit Sub
        End If

        Dim NumeroRemision As Long

        If Not Long.TryParse(txt_Remision.Text, NumeroRemision) Then
            fn_Alerta("Debe capturar un numero de remisión valido.")
            Exit Sub
        End If

        Dim dt_Remision As DataTable = cn.fn_RastreoRemisiones_Buscar(NumeroRemision)
        If dt_Remision Is Nothing Then
            Fn_Alerta("Ha ocurrido un error al buscar la remisión.")
            Exit Sub
        End If

        If dt_Remision.Rows.Count = 0 Then
            Fn_Alerta("No se ha encontrado la remisión solicitada.")
            Exit Sub
        End If

        Call cn.fn_Crear_Log(pId_Login, "RASTREO : " & NumeroRemision)

        Dim Id_Remision As Integer = dt_Remision.Rows(0)("Id_Remision")

        Call Actualizar(Id_Remision)

    End Sub

    Protected Sub Actualizar(ByVal Id_Remision As Integer)
        Dim dt_Remision As DataTable = cn.fn_RastreoRemisiones_Detalle(Id_Remision)

        If dt_Remision Is Nothing Then
            Fn_Alerta("No se pudo obtener el detalle de la remision.")
            Exit Sub
        End If

        If dt_Remision.Rows.Count > 0 Then
            Dim dr_Remision As DataRow = dt_Remision.Rows(0)

            txt_NumeroRemision.Text = dr_Remision("Numero_Remision")
            txt_FechaCaptura.Text = dr_Remision("Fecha_Registro")
            txt_HoraCaptura.Text = dr_Remision("Hora_Registro")
            txt_Importe.Text = dr_Remision("Importe")
            txt_EnvasesSN.Text = dr_Remision("EnvasesSN")

        End If

        Dim dt_Importe As DataTable = cn.fn_RastreoRemisiones_RastreoImporte(Id_Remision)

        pTabla("tbl_Importe") = dt_Importe

        gv_Importes.DataSource = fn_MostrarSiempre(dt_Importe)
        gv_Importes.DataBind()

        Dim dt_Envases As DataTable = cn.fn_RestreoRemisiones_RastreoEnvase(Id_Remision)
        pTabla("tbl_RastreoEnvases") = dt_Envases

        gv_Envases.DataSource = fn_MostrarSiempre(dt_Envases)
        gv_Envases.DataBind()

        Dim dt_Log As DataTable = cn.fn_RastreoRemisiones_Log(Id_Remision)

        gv_Log.DataSource = dt_Log
        gv_Log.DataBind()

    End Sub

    Protected Sub gv_Importes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Importes.PageIndexChanging
        gv_Importes.PageIndex = e.NewPageIndex
        gv_Importes.DataSource = pTabla("tbl_Importe")
        gv_Importes.DataBind()

    End Sub

    Protected Sub gv_Envases_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Envases.PageIndexChanging
        gv_Envases.PageIndex = e.NewPageIndex
        gv_Envases.DataSource = pTabla("tbl_RastreoEnvases")
        gv_Envases.DataBind()
    End Sub

End Class