Public Class ConsultaTarjetasRetenidas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: CONSULTA DE TARJETAS RETENIDAS")

        Call MuestraGridVacio_TarjetasRetenidas()
    End Sub

    Private Sub MuestraGridVacio_TarjetasRetenidas()
        txt_Observaciones.Text = String.Empty
        fst_TarjetasRetenidas.Style.Add("height", "205px")
        gv_tarjetasRetenidas.DataSource = fn_CreaGridVacio("NumCajero,Proveedor,Ciudad,Estado,Descripcion,NoTarjeta,BancoTarjeta,FechaRecoleccion,FechaRecepcion,Encontrada_En,Clonada,Observaciones")
        gv_tarjetasRetenidas.DataBind()

    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click
        If txt_FechaInicial.Text.Trim = "" Then
            Fn_Alerta("Seleccione Fecha Inicio")
            Exit Sub
        End If

        If txt_FechaFinal.Text.Trim = "" Then
            Fn_Alerta("Seleccione Fecha Fin")
            Exit Sub
        End If

        If CDate(txt_FechaInicial.Text) > CDate(txt_FechaFinal.Text) Then
            Fn_Alerta("La fecha Inicio debe ser menor que la fecha Fin")
            Exit Sub
        End If

        If ddl_TipoTarjetas.SelectedValue = "0" AndAlso Not chk_TodosTarjetas.Checked Then
            Fn_Alerta("Seleccione un tipo de tarjeta ó marque la casilla Todos")
            Exit Sub
        End If

        pTabla("TarjetasRetenidas") = Nothing
        Dim status As String = "T"

        If ddl_TipoTarjetas.SelectedValue <> "0" Then status = ddl_TipoTarjetas.SelectedValue

        Dim dt_tarjetasRetenidas As DataTable = cn.fn_ConsultaTarjetasRetenidas_llenarGridView(CDate(txt_FechaInicial.Text), CDate(txt_FechaFinal.Text), pId_CajaBancaria, status)
        fst_TarjetasRetenidas.Style.Remove("height")

        If dt_tarjetasRetenidas IsNot Nothing AndAlso dt_tarjetasRetenidas.Rows.Count > 0 Then
            gv_tarjetasRetenidas.DataSource = dt_tarjetasRetenidas
            gv_tarjetasRetenidas.DataBind()
            gv_tarjetasRetenidas.SelectedIndex = -1

            pTabla("TarjetasRetenidas") = dt_tarjetasRetenidas
        Else
            Call MuestraGridVacio_TarjetasRetenidas()
        End If

        Dim StatusLog As String = If(chk_TodosTarjetas.Checked, "TODOS", ddl_TipoTarjetas.SelectedItem.Text)
        Call cn.fn_Crear_Log(pId_Login, "CONSULTO: " & txt_FechaInicial.Text & " AL: " & txt_FechaFinal.Text & "; ESTATUS: " & StatusLog & "; REGISTROS: " & dt_tarjetasRetenidas.Rows.Count)

        If gv_tarjetasRetenidas.Rows.Count < 4 Then
            fst_TarjetasRetenidas.Style.Add("height", "205px")
        End If

    End Sub

    Protected Sub chk_TodosTarjetas_CheckedChanged(sender As Object, e As EventArgs) Handles chk_TodosTarjetas.CheckedChanged
        ddl_TipoTarjetas.Enabled = Not chk_TodosTarjetas.Checked

        Call MuestraGridVacio_TarjetasRetenidas()
        ddl_TipoTarjetas.SelectedValue = 0
        gv_tarjetasRetenidas.SelectedIndex = -1
    End Sub

    Protected Sub txt_FechaInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaInicial.TextChanged
        Call MuestraGridVacio_TarjetasRetenidas()
    End Sub

    Protected Sub txt_FechaFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaFinal.TextChanged
        Call MuestraGridVacio_TarjetasRetenidas()
    End Sub

    Protected Sub ddl_TipoTarjetas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_TipoTarjetas.SelectedIndexChanged
        Call MuestraGridVacio_TarjetasRetenidas()
    End Sub

    Protected Sub gv_tarjetasRetenidas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_tarjetasRetenidas.SelectedIndexChanged
        If IsDBNull(gv_tarjetasRetenidas.DataKeys(0).Value) OrElse CStr(gv_tarjetasRetenidas.DataKeys(0).Value) = "" OrElse CStr(gv_tarjetasRetenidas.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Observaciones As String = gv_tarjetasRetenidas.SelectedDataKey("Observaciones")
        txt_Observaciones.Text = Observaciones
    End Sub

    Protected Sub gv_tarjetasRetenidas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_tarjetasRetenidas.PageIndexChanging
        Dim dt_TarjetasRetenidas As DataTable = pTabla("TarjetasRetenidas")

        gv_tarjetasRetenidas.PageIndex = e.NewPageIndex
        gv_tarjetasRetenidas.DataSource = dt_TarjetasRetenidas
        gv_tarjetasRetenidas.DataBind()
        gv_tarjetasRetenidas.SelectedIndex = -1
        txt_Observaciones.Text = String.Empty
        If gv_tarjetasRetenidas.Rows.Count < 4 Then
            fst_TarjetasRetenidas.Style.Add("height", "205px")
        End If
    End Sub
End Class