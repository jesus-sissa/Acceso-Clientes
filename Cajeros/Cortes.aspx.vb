Public Class Cortes
    Inherits BasePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub
        Dim dt_Cajeros As DataTable = cn.fn_CapturaDotaciones_GetCajeros(pId_CajaBancaria)

        If dt_Cajeros Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar la información.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Cajero, dt_Cajeros, False)
        MuestraGridVacios()
    End Sub
    Sub MuestraGridVacios()
        fst_Cortes.Style.Add("height", "205px")

        gv_Cortes.DataSource = fn_CreaGridVacio("Remision,Cajero,Importe,Status,Fecha,IDRR")
        gv_Cortes.DataBind()
        gv_Cortes.SelectedIndex = -1
    End Sub
    Protected Sub Bnt_consultar_Click(sender As Object, e As EventArgs) Handles Bnt_consultar.Click
        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            fn_Alerta("Seleccione un rango de fechas correcto.")
            Exit Sub
        End If

        Dim Serial As String() = Split(txt_FInicial.Text, "/")
        Dim FechaInicial As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        Serial = Split(txt_FFinal.Text, "/")
        Dim FechaFinal As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        If DateDiff(DateInterval.Day, FechaFinal, FechaInicial) > 0 Then
            fn_Alerta("La fecha final no debe superar a la fecha inicial.")
            Exit Sub
        End If
        If ddl_Cajero.SelectedValue = "0" AndAlso Not chk_Cajeros.Checked Then
            fn_Alerta("Seleccione un cajero ó marque la Casilla Todos")
            Exit Sub
        End If

        Dim dt_Cortes As DataTable = cn.fn_CortesConsultar(FechaInicial, FechaFinal, pId_CajaBancaria, ddl_Cajero.SelectedValue)

        fst_Cortes.Style.Remove("height")

        If dt_Cortes IsNot Nothing AndAlso dt_Cortes.Rows.Count > 0 Then
            gv_Cortes.DataSource = dt_Cortes
            gv_Cortes.DataBind()
            pTabla("Cortess") = dt_Cortes
        Else
            Call MuestraGridVacios()
        End If
    End Sub

    Protected Sub chk_Cajeros_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Cajeros.CheckedChanged
        ddl_Cajero.Enabled = Not chk_Cajeros.Checked
        ddl_Cajero.SelectedValue = 0
        gv_Cortes.SelectedIndex = -1
        MuestraGridVacios()
    End Sub

    Protected Sub gv_Cortes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Cortes.PageIndexChanging

        fst_Cortes.Style.Remove("height")
        gv_Cortes.PageIndex = e.NewPageIndex
        gv_Cortes.DataSource = pTabla("Cortess")
        gv_Cortes.DataBind()

        gv_Cortes.SelectedIndex = -1

        If gv_Cortes.Rows.Count < 4 Then
            fst_Cortes.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Cortes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Cortes.RowCommand
        If IsDBNull(gv_Cortes.DataKeys(0).Value) OrElse CStr(gv_Cortes.DataKeys(0).Value) = "" OrElse CStr(gv_Cortes.DataKeys(0).Value) = "0" Then Exit Sub
        Select Case e.CommandName.ToUpper
            Case "VERDES"
                gv_Cortes.SelectedIndex = e.CommandArgument
                Num_Remision = gv_Cortes.SelectedDataKey("IDRR").ToString
                Numeros_Env(Num_Remision)
                Cant_M(cn.fn_ConsultaTraslado_GetMonedas(Num_Remision))
                Cant_Env(cn.fn_ConsultaTraslado_GetEnvases(Num_Remision))
                'Num_Remision = gv_ConsultaMat.SelectedDataKey("Folio").ToString()
                'Num_Remision = gv_ConsultaMat.DataKeys(e.CommandArgument).Value
                'Num_Remision = gv_ConsultaMat.Rows(e.CommandArgument).Cells(1).Text
                Response.Redirect("~/Traslado/ImprimirR.aspx")                
        End Select
    End Sub
    Sub Numeros_Env(Id_Remision As Long)
        Tipo_Remision = "ATMC"
        Dim dt_Envases As DataTable = cn.fn_ConsultaTraslado_GetEnvases(Id_Remision)
        Envases_Remision = Nothing
        For Each rows In dt_Envases.Rows
            Envases_Remision += "[" + rows("Numero").ToString() + "]"
        Next
    End Sub
    Sub Cant_M(Dt As DataTable)
        Mon_Na = 0
        Mon_Ex = 0
        Mon_Otros = 0
        For Each Row As DataRow In Dt.Rows
            If (Row("Moneda").ToString() = "PESOS") Then
                Mon_Na += CDbl(Row("Efectivo").ToString())
                Mon_Otros += CDbl(Row("Documentos").ToString())
            ElseIf (Row("Moneda").ToString() = "DOLARES" Or Row("Moneda").ToString() = "ORO ONZA" Or Row("Moneda").ToString() = "EUROS" Or Row("Moneda").ToString() = "PLATA" Or Row("Moneda").ToString() = "ORO") Then
                Mon_Ex += (CDbl(Row("Efectivo").ToString()) * CDbl(Row("Tipo Cambio").ToString()))
                Mon_Otros += CDbl(Row("Documentos").ToString())
            End If
        Next
    End Sub

    Sub Cant_Env(Dt As DataTable)
        Env_B = 0
        Env_M = 0
        Env_MIX = 0
        For Each Row As DataRow In Dt.Rows
            If (Row("Tipo Envase").ToString() = "BILLETE") Then
                Env_B += 1
            ElseIf (Row("Tipo Envase").ToString() = "MIXTO") Then
                Env_MIX += 1
            ElseIf (Row("Tipo Envase").ToString() = "MORRALLA") Then
                Env_M += 1
            End If
        Next
    End Sub
End Class