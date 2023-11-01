Public Partial Class Fallas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CAPTURAR FALLAS CAJEROS ATMS")

        Dim dt_Cajeros As DataTable = cn.fn_CapturaDotaciones_GetCajeros(pId_CajaBancaria)
        pTabla("Cajeros") = dt_Cajeros
        Call fn_LlenarDropDown(ddl_Cajero, dt_Cajeros, False)

        Dim dt_Partes As DataTable = cn.fn_ConsultaPartes()
        Call fn_LlenarDropDown(ddl_ParteFalla, dt_Partes, False)

        Dim dt_TipoFalla As DataTable = cn.fn_ConsultaPartes_TipoFalla(0)
        Call fn_LlenarDropDown(ddl_TipoFalla, dt_TipoFalla, False)

        Dim dt_ParamCajasBancarias As DataTable = cn.fn_CajasBancarias_ObtenerParametros(pId_CajaBancaria)
        pTabla("ParamCajasBancarias") = dt_ParamCajasBancarias

        Dim Horas As DataTable = cn.GetHoras(30)
        Call fn_LlenarDropDown(ddl_A, Horas.Copy(), False)
        Call fn_LlenarDropDown(ddl_De, Horas.Copy(), False)

    End Sub

    Protected Sub btn_GuardarFalla_Click(sender As Object, e As EventArgs) Handles btn_GuardarFalla.Click

        If ddl_Cajero.SelectedValue = 0 Then
            Fn_Alerta("Seleccione un Cajero.")
            Exit Sub
        End If

        If tbx_NumReporte.Text.Trim = "" Then
            Fn_Alerta("Capture el número de Reporte.")
            Exit Sub
        End If

        If tbx_Fecha.Text.Trim = "" Then
            Fn_Alerta("Capture la fecha requerida.")
            Exit Sub
        End If

        If tbx_FechaAlarma.Text.Trim = "" Then
            Fn_Alerta("Capture la fecha de alarma.")
            Exit Sub
        End If
        If CDate(tbx_Fecha.Text) < CDate(tbx_FechaAlarma.Text) Then
            Fn_Alerta("La fecha  requerida debe ser mayor a la fecha de alarma.")
            Exit Sub
        End If

        If ddl_ParteFalla.SelectedValue = 0 Then
            Fn_Alerta("Seleccione la parte que falla.")
            Exit Sub
        End If

        If ddl_TipoFalla.SelectedValue = 0 Then
            Fn_Alerta("Seleccione el tipo de falla.")
            Exit Sub
        End If
        Dim FallaCustodia As Integer = 1 'Falla
        If rdb_Custodia.Checked Then FallaCustodia = 2 'custodia

        Dim HoraSolicitaBanco As String = ddl_MinutosSolBanco.Text & ":" & ddl_MinutosSolg.Text
        Dim HoraAlarma As String = ddl_HoraAlarma.Text & ":" & ddl_MinutosAlarma.Text

        Dim Comentarios As String = txt_Comentarios.Text.Trim
        Dim NumCajero As String = pTabla("Cajeros").Rows(ddl_Cajero.SelectedIndex - 1)("Numero_Cajero")

        Dim FallaCust As String = "FALLA"
        If FallaCustodia = 2 Then FallaCust = "CUSTODIA"

        If cn.fn_FallasCustodias_Crear(ddl_Cajero.SelectedValue, ddl_ParteFalla.SelectedValue, tbx_NumReporte.Text, CDate(tbx_Fecha.Text), tbx_TiempoRespuesta.Text,
                                       FallaCustodia, HoraSolicitaBanco, CDate(tbx_FechaAlarma.Text), HoraAlarma, Comentarios, NumCajero, ddl_Cajero.SelectedItem.Text, ddl_ParteFalla.SelectedItem.Text, ddl_TipoFalla.SelectedItem.Text) Then
            Fn_Alerta("Falla registrada correctamente.")

            Call cn.fn_Crear_Log(pId_Login, "SE GUARDO LA: " & FallaCust & "; FECHA REQUERIDA: " & tbx_Fecha.Text & "; FECHA ALARMA: " & tbx_FechaAlarma.Text _
                                 & "; HORA SOLICITA BANCO: " & HoraSolicitaBanco & "; PARTE: " & ddl_ParteFalla.SelectedItem.Text & "; TIPO: " & ddl_TipoFalla.SelectedItem.Text)
            Call LimpiarControles()

        Else
            Fn_Alerta("Ocurrió un error al intentar registrar la Falla.")
        End If
    End Sub

    Sub LimpiarControles()
        txt_Comentarios.Text = String.Empty
        ddl_Cajero.SelectedValue = 0
        tbx_tipoCajero.Text = String.Empty
        tbx_TiempoRespuesta.Text = String.Empty
        tbx_NumReporte.Text = String.Empty
        tbx_Fecha.Text = String.Empty
        tbx_FechaAlarma.Text = String.Empty
        ddl_ParteFalla.SelectedValue = 0
        ddl_TipoFalla.SelectedValue = 0
    End Sub

    Protected Sub btn_GuardarCorte_Click(sender As Object, e As EventArgs) Handles btn_GuardarCorte.Click
        tbx_NumReporteCorte.Text = String.Empty
    End Sub

    Protected Sub ddl_Cajero_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Cajero.SelectedIndexChanged
        If ddl_Cajero.SelectedValue > 0 Then
            tbx_tipoCajero.Text = String.Empty

            Dim dt As DataTable
            dt = cn.fn_DotacionCajero_Read(ddl_Cajero.SelectedValue)
            If dt Is Nothing Then
                Fn_Alerta("Ocurrió un Error al intentar leer la información del Cajero.")
                Exit Sub
            End If
            If dt.Rows.Count = 0 Then
                Fn_Alerta("No se encontró la Información del Cajero Seleccionado.")
                Exit Sub
            End If
            If dt.Rows(0)("LocalForaneo") = "L" Then
                tbx_tipoCajero.Text = "LOCAL"
                tbx_TiempoRespuesta.Text = pTabla("ParamCajasBancarias").Rows(0)("TR_FallaLocal")
            ElseIf dt.Rows(0)("LocalForaneo") = "F" Then
                tbx_tipoCajero.Text = "FORANEO"
                tbx_TiempoRespuesta.Text = pTabla("ParamCajasBancarias").Rows(0)("TR_FallaForanea")
            End If
        End If

    End Sub

    Protected Sub ddl_ParteFalla_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_ParteFalla.SelectedIndexChanged
        Dim dt_tipoFalla As DataTable = cn.fn_ConsultaPartes_TipoFalla(ddl_ParteFalla.SelectedValue)
        Call fn_LlenarDropDown(ddl_TipoFalla, dt_tipoFalla, False)
    End Sub
End Class