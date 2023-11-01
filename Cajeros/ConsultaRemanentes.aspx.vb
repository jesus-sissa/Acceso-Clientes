Public Class ConsultaRemanentes
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE REMANENTES")

        Call MuestraGridVacios()
    End Sub

    Sub MuestraGridVacios()
        fst_Remanentes.Style.Add("height", "205px")

        gv_Remanentes.DataSource = fn_CreaGridVacio("Id_Servicio,Remision,Cajero,FechaEntrada,FechaVerifica,ImporteEfectivo,DiferenciaEfectivo,Status,IDR")
        gv_Remanentes.DataBind()
        gv_Remanentes.SelectedIndex = -1

        Call MuestraDetalleGridVacios()
    End Sub

    Sub MuestraDetalleGridVacios()
        gv_ImporteRemision.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Efectivo,Documentos,TipoCambio")
        gv_ImporteRemision.DataBind()

        gv_ImporteVerificado.DataSource = fn_CreaGridVacio("Id_Denominacion,Presentacion,Denominacion,Cantidad,Importe")
        gv_ImporteVerificado.DataBind()
    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click

        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            Fn_Alerta("Seleccione un rango de fechas correcto.")
            Exit Sub
        End If

        Dim Serial As String() = Split(txt_FInicial.Text, "/")
        Dim FechaInicial As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        Serial = Split(txt_FFinal.Text, "/")
        Dim FechaFinal As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        If DateDiff(DateInterval.Day, FechaFinal, FechaInicial) > 0 Then
            Fn_Alerta("La fecha final no debe superar a la fecha inicial.")
            Exit Sub
        End If

        If ddl_Status.Enabled = True And ddl_Status.SelectedIndex = 0 Then
            Fn_Alerta("Seleccione un Status.")
            Exit Sub
        End If

        Dim Status As String = "T"
        If ddl_Status.SelectedValue <> "0" Then Status = ddl_Status.SelectedValue

        Dim dt_Remanentes As DataTable = cn.fn_RemanentesConsultar(FechaInicial, FechaFinal, pId_CajaBancaria, Status)

        fst_Remanentes.Style.Remove("height")

        If dt_Remanentes IsNot Nothing AndAlso dt_Remanentes.Rows.Count > 0 Then
            gv_Remanentes.DataSource = dt_Remanentes
            gv_Remanentes.DataBind()
            pTabla("Remanentes") = dt_Remanentes
        Else
            Call MuestraGridVacios()
        End If

        Dim StatusLog As String = If(chk_Todos.Checked, "TODOS", ddl_Status.SelectedItem.Text)
        Call cn.fn_Crear_Log(pId_Login, "CONSULTO: " & txt_FInicial.Text & " AL: " & txt_FFinal.Text & "; ESTATUS: " & StatusLog & "; REGISTROS: " & dt_Remanentes.Rows.Count)

        If gv_Remanentes.Rows.Count < 4 Then
            fst_Remanentes.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
        Call MuestraGridVacios()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
        Call MuestraGridVacios()
    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call MuestraGridVacios()
    End Sub

    Protected Sub gv_Remanentes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Remanentes.PageIndexChanging
        gv_Remanentes.PageIndex = e.NewPageIndex
        gv_Remanentes.DataSource = pTabla("Remanentes")
        gv_Remanentes.DataBind()
        gv_Remanentes.SelectedIndex = -1

        Call MuestraDetalleGridVacios()

        If gv_Remanentes.Rows.Count < 4 Then
            fst_Remanentes.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Remanentes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Remanentes.SelectedIndexChanged
        If IsDBNull(gv_Remanentes.DataKeys(0).Value) OrElse CStr(gv_Remanentes.DataKeys(0).Value) = "" OrElse CStr(gv_Remanentes.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_RemisionD As DataTable = cn.fn_RemanentesConsultar_RemisionesD(gv_Remanentes.SelectedDataKey("IDR"))
        If dt_RemisionD IsNot Nothing AndAlso dt_RemisionD.Rows.Count > 0 Then
            gv_ImporteRemision.DataSource = dt_RemisionD
            gv_ImporteRemision.DataBind()

        Else
            gv_ImporteRemision.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Efectivo,Documentos,TipoCambio")
            gv_ImporteRemision.DataBind()

        End If

        Dim dt_ServiciosD As DataTable = cn.fn_RemanentesConsultar_ServiciosD(gv_Remanentes.SelectedDataKey("Id_Servicio"))
        If dt_ServiciosD IsNot Nothing AndAlso dt_ServiciosD.Rows.Count > 0 Then
            gv_ImporteVerificado.DataSource = dt_ServiciosD
            gv_ImporteVerificado.DataBind()
        Else
            gv_ImporteVerificado.DataSource = fn_CreaGridVacio("Id_Denominacion,Presentacion,Denominacion,Cantidad,Importe")
            gv_ImporteVerificado.DataBind()

        End If
    End Sub

    Protected Sub chk_Todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Todos.CheckedChanged
        ddl_Status.Enabled = Not chk_Todos.Checked
        Call MuestraGridVacios()
        ddl_Status.SelectedValue = 0
        gv_Remanentes.SelectedIndex = -1
    End Sub
End Class