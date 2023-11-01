Public Class ConsultaFallas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        '---llenar combo cajeros
        Dim dt_Cajeros As DataTable = cn.fn_CapturaDotaciones_GetCajeros(pId_CajaBancaria)
        fn_LlenarDropDown(ddl_Cajero, dt_Cajeros, False)

        Call MuestraGridVacio_Fallas()
        Call MuestraGridvacio_TarjetaFallas()
        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: CONSULTA DE FALLAS CAJEROS ATMS")

        tbx_Importe.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_Remision.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_Teoricas.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_Reales.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

        fst_fallas.Style.Add("height", "205px;")

    End Sub

    Private Sub MuestraGridvacio_TarjetaFallas()
        gv_TarjetasFallas.DataSource = fn_CreaGridVacio("Id_Tarjeta,Tarjeta,Banco,Titular,Vence,Encontrada_En,Clonada,Observaciones")
        gv_TarjetasFallas.DataBind()
    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click
        Dim CajeroLog As String = "TODOS"
        Dim StatusLog As String = "TODOS"

        If tbx_Fechainicio.Text = "" Then
            Fn_Alerta("Seleccione Fecha Inicio")
            Exit Sub
        End If

        If tbx_Fechafin.Text = "" Then
            Fn_Alerta("Seleccione Fecha Fin")
            Exit Sub
        End If

        If CDate(tbx_Fechainicio.Text) > CDate(tbx_Fechafin.Text) Then
            Fn_Alerta("La fecha Inicio debe ser menor que la fecha Fin")
            Exit Sub
        End If

        If ddl_Cajero.SelectedValue = "0" AndAlso Not chk_Cajeros.Checked Then
            Fn_Alerta("Seleccione un cajero ó marque la Casilla Todos")
            Exit Sub
        End If

        If ddl_Status.SelectedValue = "0" AndAlso Not chk_Todos.Checked Then
            Fn_Alerta("Seleccione un status ó marque la Casilla Todos")
            Exit Sub
        End If
        '-----------------------------------------

        pTabla("Fallas") = Nothing
        Dim status As String = "T"
        If ddl_Status.SelectedValue <> "0" Then status = ddl_Status.SelectedValue

        Dim dt_Fallas As DataTable = cn.fn_ConsultaFallasCajeros_LlenaGridview(CDate(tbx_Fechainicio.Text), CDate(tbx_Fechafin.Text), pId_CajaBancaria, ddl_Cajero.SelectedValue, status)
        fst_fallas.Style.Remove("height")

        If dt_Fallas IsNot Nothing AndAlso dt_Fallas.Rows.Count > 0 Then
            gv_Fallas.DataSource = dt_Fallas
            gv_Fallas.DataBind()
            pTabla("Fallas") = dt_Fallas
        Else
            Call MuestraGridVacio_Fallas()
        End If

        If Not chk_Cajeros.Checked Then
            CajeroLog = ddl_Cajero.SelectedItem.Text
        End If

        If Not chk_Todos.Checked Then
            StatusLog = ddl_Status.SelectedItem.Text
        End If

        Call cn.fn_Crear_Log(pId_Login, "CONSULTO: " & tbx_Fechainicio.Text & " AL: " & tbx_Fechafin.Text & "; CAJERO: " & CajeroLog & _
                                "; ESTATUS: " & StatusLog & "; REGISTROS: " & dt_Fallas.Rows.Count)

        If gv_Fallas.Rows.Count < 4 Then
            fst_fallas.Style.Add("height", "205px")
        End If
    End Sub

    Private Sub Limpiar()
        fst_fallas.Style.Add("height", "205px;")

        Call MuestraGridVacio_Fallas()
        Call MuestraGridvacio_TarjetaFallas()
        gv_Fallas.SelectedIndex = -1

        rdb_funcionaSI.Checked = False
        rdb_funcionaNO.Checked = False
        rdb_cambiopapelSI.Checked = False
        rdb_cambiopapelNO.Checked = False
        rdb_descargoinfoSI.Checked = False
        rdb_descargoinfoNO.Checked = False
        rdb_huboretiroSI.Checked = False
        rdb_huboretiroNO.Checked = False
        tbx_Observaciones.Text = String.Empty
        tbx_Remision.Text = String.Empty
        tbx_Importe.Text = Format(0, "$ ###,###,###0.00##")
        tbx_Teoricas.Text = 0
        tbx_Reales.Text = 0

    End Sub

    Private Sub MuestraGridVacio_Fallas()
        gv_Fallas.DataSource = fn_CreaGridVacio("Id_Falla,NumReporte,Caja,No. Cajero,Cajero,Fecha Captura,Fecha Requerida,Hora Requerida,Tipo,Status,QuedoFuncionando,Observaciones,TarjetasReales,TarjetasTeoricas,HuboRetiro,RolloNuevo,Modo_Afecta,DescargaInfo,RemisionRetiro")
        gv_Fallas.DataBind()

    End Sub

    Protected Sub gv_Fallas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Fallas.PageIndexChanging
        Call Limpiar()
        fst_fallas.Style.Remove("height")
        gv_Fallas.PageIndex = e.NewPageIndex
        gv_Fallas.DataSource = pTabla("Fallas")
        gv_Fallas.DataBind()
        gv_Fallas.SelectedIndex = -1

        If gv_Fallas.Rows.Count < 4 Then
            fst_fallas.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub chk_Cajeros_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chk_Cajeros.CheckedChanged
        ddl_Cajero.Enabled = Not chk_Cajeros.Checked

        Call Limpiar()
        ddl_Cajero.SelectedValue = 0
        gv_Fallas.SelectedIndex = -1

    End Sub

    Protected Sub ddl_Cajero_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Cajero.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub chk_Todos_CheckedChanged(sender As Object, e As EventArgs) Handles chk_Todos.CheckedChanged
        ddl_Status.Enabled = Not chk_Todos.Checked

        Call Limpiar()
        ddl_Status.SelectedValue = 0
        gv_Fallas.SelectedIndex = -1
    End Sub

    Protected Sub gv_Fallas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Fallas.SelectedIndexChanged
        If IsDBNull(gv_Fallas.DataKeys(0).Value) OrElse CStr(gv_Fallas.DataKeys(0).Value) = "" OrElse CStr(gv_Fallas.DataKeys(0).Value) = "0" Then Exit Sub

        tbx_Observaciones.Text = gv_Fallas.SelectedDataKey("Observaciones")
        Call MuestraGridvacio_TarjetaFallas()
        rdb_funcionaSI.Checked = False
        rdb_funcionaNO.Checked = False
        rdb_cambiopapelSI.Checked = False
        rdb_cambiopapelNO.Checked = False
        rdb_descargoinfoSI.Checked = False
        rdb_descargoinfoNO.Checked = False

        rdb_huboretiroSI.Checked = False
        rdb_huboretiroNO.Checked = False

        If gv_Fallas.SelectedDataKey("QuedoFuncionando") <> "" Then
            If gv_Fallas.SelectedDataKey("QuedoFuncionando") = "S" Then
                rdb_funcionaSI.Checked = True
            Else
                rdb_funcionaNO.Checked = True
            End If
        End If

        If gv_Fallas.SelectedDataKey("RolloNuevo") <> "" Then
            If gv_Fallas.SelectedDataKey("RolloNuevo") = "S" Then
                rdb_cambiopapelSI.Checked = True
            Else
                rdb_cambiopapelNO.Checked = True
            End If
        End If

        If gv_Fallas.SelectedDataKey("DescargaInfo") <> "" Then
            If gv_Fallas.SelectedDataKey("DescargaInfo") = "S" Then
                rdb_descargoinfoSI.Checked = True
            Else
                rdb_descargoinfoNO.Checked = True
            End If
        End If

        If gv_Fallas.SelectedDataKey("HuboRetiro") <> "" Then
            If gv_Fallas.SelectedDataKey("HuboRetiro") = "S" Then
                rdb_huboretiroSI.Checked = True
            Else
                rdb_huboretiroNO.Checked = True
            End If
        End If

        tbx_Importe.Text = Format(0, "$ ###,###,###0.00##")
        tbx_Remision.Text = String.Empty

        tbx_Teoricas.Text = gv_Fallas.SelectedDataKey("TarjetasTeoricas")
        tbx_Reales.Text = gv_Fallas.SelectedDataKey("TarjetasReales")

        Dim IdRemision As Integer = Integer.Parse(gv_Fallas.SelectedDataKey("RemisionRetiro"))
        If IdRemision > 0 Then
            'consultar detalle remision
            Dim dt_DetalleRemision As DataTable = cn.fn_ConsultaFallasCajeros_DetalleRemision(IdRemision)
            If dt_DetalleRemision IsNot Nothing AndAlso dt_DetalleRemision.Rows.Count > 0 Then
                Dim IMPORTE As Decimal = dt_DetalleRemision.Rows(0)("Importe")

                tbx_Importe.Text = Format(IMPORTE, "$ ###,###,###0.00##")
                tbx_Remision.Text = dt_DetalleRemision.Rows(0)("Remision")
            End If
        End If

        Dim IdFalla As Integer = Integer.Parse(gv_Fallas.SelectedDataKey("Id_Falla"))

        'consultar detalle Tarjetas
        Dim dt_DetalleTarjetas As DataTable = cn.fn_ConsultaFallasCajeros_DetalleTarjetas(IdFalla)

        If dt_DetalleTarjetas IsNot Nothing AndAlso dt_DetalleTarjetas.Rows.Count > 0 Then
            gv_TarjetasFallas.DataSource = dt_DetalleTarjetas
            gv_TarjetasFallas.DataBind()
        Else
            Call MuestraGridvacio_TarjetaFallas()
        End If

    End Sub

    Protected Sub tbx_Fechainicio_TextChanged(sender As Object, e As EventArgs) Handles tbx_Fechainicio.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub tbx_Fechafin_TextChanged(sender As Object, e As EventArgs) Handles tbx_Fechafin.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub gv_Fallas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Fallas.RowCommand
        If IsDBNull(gv_Fallas.DataKeys(0).Value) OrElse CStr(gv_Fallas.DataKeys(0).Value) = "" OrElse CStr(gv_Fallas.DataKeys(0).Value) = "0" Then Exit Sub
        Select Case e.CommandName.ToUpper
            Case "VERDES"
                gv_Fallas.SelectedIndex = e.CommandArgument
                Num_Remision = gv_Fallas.SelectedDataKey("RemisionRetiro").ToString
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
        Tipo_Remision = "ATMF"
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