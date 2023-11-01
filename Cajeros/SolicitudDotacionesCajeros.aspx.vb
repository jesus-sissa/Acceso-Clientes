Partial Public Class Dotaciones
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: SOLICITUD DE DOTACIONES DE CAJEROS ATMS")

        Call MuestraGridVacio()

        '----Llena combo monedas
        Dim dt_Monedas As DataTable = cn.fn_SolicitudDotaciones_GetMonedas()
        If dt_Monedas Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Moneda, dt_Monedas, False)

        '-----llena combo Cajeros
        Dim dt_Cajeros As DataTable = cn.fn_CapturaDotaciones_GetCajeros(pId_CajaBancaria)
        If dt_Cajeros Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        pTabla("Cajeros") = dt_Cajeros
        fn_LlenarDropDown(ddl_Cajero, dt_Cajeros, False)

    End Sub

    Protected Sub rdb_Siprioridad_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Siprioridad.CheckedChanged
        If rdb_Siprioridad.Checked Then lbl_msjPrioridad.Visible = True
    End Sub

    Protected Sub rdb_Noprioridad_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rdb_Noprioridad.CheckedChanged
        If rdb_Noprioridad.Checked Then lbl_msjPrioridad.Visible = False
    End Sub

    Protected Sub ddl_Cajero_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Cajero.SelectedIndexChanged
        If ddl_Cajero.SelectedValue > 0 Then
            Dim dt_valoresCajero As DataTable = cn.fn_ObtenerValoresCajeros(ddl_Cajero.SelectedValue)
            pTabla("ValoresCajero") = dt_valoresCajero
        End If
        If ddl_Moneda.SelectedIndex > 0 Then Call LLenar_GridDotaciones()
    End Sub

    Private Sub MuestraGridVacio()
        gv_Dotaciones.DataSource = fn_CreaGridVacio("Denominacion,Piezas,Total,Id_Denominacion")
        gv_Dotaciones.DataBind()
        lbl_Total.Text = "$ 0.00"
    End Sub

    Private Sub LLenar_GridDotaciones()

        Dim dt_Dotacion As DataTable = cn.fn_DotacionConfigConsulta(ddl_Cajero.SelectedValue, ddl_Moneda.SelectedValue)
        If dt_Dotacion Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar la información.")
            Exit Sub
        End If

        pTabla("tablaBilletesDot") = dt_Dotacion

        If dt_Dotacion.Rows.Count > 0 Then
            gv_Dotaciones.DataSource = fn_MostrarSiempre(dt_Dotacion)
            gv_Dotaciones.DataBind()
        Else
            Call MuestraGridVacio()
        End If

    End Sub

    Protected Sub ddl_Moneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Moneda.SelectedIndexChanged
        If ddl_Cajero.SelectedIndex > 0 Then Call LLenar_GridDotaciones()
    End Sub

    Protected Sub gv_Dotaciones_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Dotaciones.RowCancelingEdit
        gv_Dotaciones.EditIndex = -1
        gv_Dotaciones.DataSource = pTabla("tablaBilletesDot")
        gv_Dotaciones.DataBind()
    End Sub

    Protected Sub gv_Dotaciones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Dotaciones.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            If IsDBNull(gv_Dotaciones.DataKeys(0).Value) OrElse CStr(gv_Dotaciones.DataKeys(0).Value) = "" OrElse CStr(gv_Dotaciones.DataKeys(0).Value) = "0" Then
                e.Row.Cells(1).Controls.Clear()
            Else
                Dim txt_Cantidad As TextBox = e.Row.Cells(1).FindControl("tbx_Piezas")
                txt_Cantidad.Text = If(IsDBNull(e.Row.DataItem("Piezas")), 0, e.Row.DataItem("Piezas"))
            End If
        End If

    End Sub

    Protected Sub gv_Dotaciones_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Dotaciones.RowEditing
        gv_Dotaciones.EditIndex = e.NewEditIndex

        Dim dt_Dotaciones As DataTable = pTabla("tablaBilletesDot")
        gv_Dotaciones.DataSource = dt_Dotaciones
        gv_Dotaciones.DataBind()
    End Sub

    Protected Sub gv_Dotaciones_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Dotaciones.RowUpdating
        Try
            Dim Denominacion As Integer = gv_Dotaciones.SelectedRow.Cells(e.RowIndex).Text
            Dim dt_Dotacion As DataTable = pTabla("tablaBilletesDot")
            Dim Filas = From T As DataRow In dt_Dotacion Where T("Denominacion") = Denominacion Select T

            If Filas.Count = 0 Then
                Fn_Alerta("No se puede Actualizar la fila debido a un error.")
                Exit Sub
            End If

            Dim dr_fila As DataRow = Filas(0)
            Dim Cantidad As Integer = CInt(CType(gv_Dotaciones.Rows(e.RowIndex).Cells(3).Controls(0), TextBox).Text)

            dr_fila("Piezas") = Cantidad
            dr_fila("Total") = FormatNumber((Cantidad * CDec(dr_fila("Denominacion"))), 2)

            dt_Dotacion.AcceptChanges()

            gv_Dotaciones.EditIndex = -1
            gv_Dotaciones.DataSource = dt_Dotacion
            gv_Dotaciones.DataBind()
            pTabla("tablaDotaciones") = dt_Dotacion
        Catch ex As Exception
            fn_Alerta("No se puede actualizar la fila debido a un error.")
            Exit Sub
        End Try
    End Sub

    Protected Sub btn_Comprobar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Comprobar.Click

        If IsDBNull(gv_Dotaciones.DataKeys(0).Value) OrElse CStr(gv_Dotaciones.DataKeys(0).Value) = "" OrElse CStr(gv_Dotaciones.DataKeys(0).Value) = "0" Then
            Fn_Alerta("Debe llenar la Grilla antes de Comprobar.")
            Exit Sub
        End If

        Dim TotalReal As Single = 0

        If gv_Dotaciones.Visible Then TotalReal += ActualizarTxtBilletes()
        lbl_Total.Text = FormatCurrency(TotalReal)

    End Sub

    Protected Function ActualizarTxtBilletes() As Single
        Dim dt_Dotacion As DataTable = pTabla("tablaBilletesDot")
        Dim Acumulado As Single = 0

        For Each gvr_Dota As GridViewRow In gv_Dotaciones.Rows
            Dim txt_Cantidad As TextBox = gvr_Dota.Cells(1).FindControl("tbx_Piezas")
            Dim IdDenominacion As Integer = gv_Dotaciones.DataKeys(gvr_Dota.RowIndex).Value
            Dim dr As DataRow = dt_Dotacion.Select("Id_Denominacion = '" & IdDenominacion & "'")(0)

            Dim Cantidad As Integer
            If Not Integer.TryParse(txt_Cantidad.Text, Cantidad) Then Cantidad = 0
            dr("Piezas") = Cantidad
            dr("Total") = FormatNumber(CSng(dr("Denominacion") * Cantidad), 2)
            Acumulado += dr("Total")
        Next

        pTabla("tablaBilletesDot") = dt_Dotacion
        gv_Dotaciones.DataSource = dt_Dotacion
        gv_Dotaciones.DataBind()

        Return Acumulado

    End Function

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click
        Dim fecha As Date

        If ValidaCampos() = True Then

            Dim Prioridad_Banco As String = "N"
            Dim RCorte As String = "N"
            Dim Horario As String
            Dim Prioridad As Integer = 100
            'Tiene prioridad por parte del Banco
            Dim Especial As Char = "N"

            Dim IdRuta As Integer = pTabla("ValoresCajero").Rows(0)("Id_Ruta")

            If rdb_Siprioridad.Checked = True Then
                Prioridad_Banco = "S" 'verificar si es para hoy??
                Especial = "S"
                Prioridad = 1
            End If

            If rdb_Sicorte.Checked = True Then
                RCorte = "S"
            End If

            Dim HoraSolicita As DateTime = CDate(ddl_HorasSol.Text & ":" & ddl_MinutosSol.Text)
            Dim HoraDesde As DateTime = CDate(ddl_HorasInicio.Text & ":" & ddl_MinutosInicio.Text)
            Dim HoraHasta As DateTime = CDate(ddl_HorasFin.Text & ":" & ddl_MinutosFin.Text)

            Horario = Format(HoraDesde, "HH:mm") + "/" + Format(HoraHasta, "HH:mm")
            Dim NumCajero As String = pTabla("Cajeros").Rows(ddl_Cajero.SelectedIndex - 1)("Numero_Cajero")

            If cn.fn_DotacionCajeros_Nuevo(tbx_NoReporte.Text, pTabla("tablaBilletesDot"), ddl_Cajero.SelectedValue, ddl_Moneda.SelectedValue, lbl_Total.Text, _
                                                   CDate(tbx_Fecha.Text), Horario, RCorte, Format(HoraSolicita, "HH:mm"), _
                                                   Prioridad_Banco, pId_CajaBancaria, pId_Cliente, txt_Comentarios.Text.Trim, _
                                                   NumCajero, ddl_Cajero.SelectedItem.Text, ddl_Moneda.SelectedItem.Text, Especial, Prioridad, IdRuta) = True Then

                cn.fn_Crear_Log(pId_Login, "SOLICITUD DOTACION ATMS: DIA : " & fecha & "/ CAJERO: " & ddl_Cajero.SelectedItem.Text & _
                            "/ MONEDA: " & ddl_Moneda.SelectedItem.Text & "/ SOLICITA: " & pNombre_Cliente & "/" & pNombre_Usuario)

                fn_Alerta("Registro guardado correctamente.")
                ddl_Cajero.SelectedValue = "0"
                ddl_Moneda.SelectedValue = "0"

                lbl_Total.Text = "$ 0.00"
                tbx_NoReporte.Text = String.Empty
                tbx_Fecha.Text = String.Empty
                txt_Comentarios.Text = String.Empty
                tbx_NoReporte.Focus()
                rdb_Noprioridad.Checked = True
                rdb_NoCorte.Checked = True
                Call MuestraGridVacio()

            Else
                fn_Alerta("Ocurrió un Error al intentar guardar la Dotación.")
                Exit Sub
            End If
        End If
    End Sub

    Private Function ValidaCampos() As Boolean

        If ddl_Cajero.SelectedValue = "0" Then
            Fn_Alerta("Seleccione Cajero.")
            ddl_Cajero.Focus()
            Return False
        End If

        If ddl_Moneda.SelectedValue = "0" Then
            Fn_Alerta("Seleccione Una Moneda.")
            ddl_Moneda.Focus()
            Return False
        End If

        If Not rdb_Siprioridad.Checked And Not rdb_Noprioridad.Checked Then
            Fn_Alerta("Indique si la Dotación tiene Prioridad por parte del Banco.")
            Return False
        End If

        'If ddl_HorasSol.Text = "00" Then
        '    fn_Alerta("Seleccione una Hora valida.")
        '    Return False
        'End If

        'If ddl_HorasInicio.Text = "00" Then
        '    fn_Alerta("Seleccione una Hora valida.")
        '    Return False
        'End If

        'If ddl_HorasFin.Text = "00" Then
        '    fn_Alerta("Seleccione una Hora valida.")
        '    Return False
        'End If

        Dim HoraSolicita As DateTime = CDate(ddl_HorasSol.Text & ":" & ddl_MinutosSol.Text)
        Dim HoraDesde As DateTime = CDate(ddl_HorasInicio.Text & ":" & ddl_MinutosInicio.Text)
        Dim HoraHasta As DateTime = CDate(ddl_HorasFin.Text & ":" & ddl_MinutosFin.Text)

        If HoraDesde < HoraSolicita Then
            Fn_Alerta("Horario de Entrega Incorrecto.")
            ddl_HorasInicio.Focus()
            Return False
        End If

        If HoraDesde > HoraHasta Then
            Fn_Alerta("Horario de Entrega Incorrecto.")
            ddl_HorasInicio.Focus()
            Return False
        End If

        If HoraDesde = HoraHasta Then
            Fn_Alerta("Horario de Entrega Incorrecto.")
            ddl_HorasFin.Focus()
            Return False
        End If

        If (HoraSolicita = HoraHasta) Or (HoraSolicita > HoraHasta) Then
            Fn_Alerta("Horario de Entrega Incorrecto.")
            ddl_HorasFin.Focus()
            Return False
        End If

        If tbx_Fecha.Text = "" Then
            Fn_Alerta("Seleccione una Fecha para la entrega de la Dotacion.")
            tbx_Fecha.Focus()
            Return False
        End If

        If lbl_Total.Text = 0 Then
            Fn_Alerta("Debe comprobar la Dotacion antes de Guardar.")
            btn_Comprobar.Focus()
            Return False
        End If

        'If Id_Dotacion = 0 Then
        '    'Cuando se esta agregando una nueva Dotación
        '    If Dg_Dotaciones.RowCount = 0 Then
        '        MsgBox("Capture la Dotación", MsgBoxStyle.Critical, frm_MENU.Text)
        '        Dg_Dotaciones.Focus()
        '        Return False
        '    End If
        'End If
        Return True

    End Function
End Class