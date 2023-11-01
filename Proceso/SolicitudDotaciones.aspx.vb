Public Partial Class SolicitudDotaciones
    Inherits BasePage

    Private Const ImagenValidado As String = "~/Imagenes/HoraSi.png"
    Private Const ImagenNoValidado As String = "~/Imagenes/Eliminar16x16.png"
    Private Const TextoImporte As String = "Importe Real: "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: SOLICITUD DE DOTACIONES")

        Dim dt_Monedas As DataTable = cn.fn_SolicitudDotaciones_GetMonedas()
        If dt_Monedas Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Moneda, dt_Monedas, False)

        Dim dt_CajasBancarias As DataTable = cn.fn_SolicitudDotaciones_GetCajasBancarias()
        If dt_CajasBancarias Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        pTabla("tbl_CajasBancarias") = dt_CajasBancarias
        fn_LlenarDropDown(ddl_CajaBancaria, dt_CajasBancarias, False)

        If ddl_CajaBancaria.Items.Count.Equals(2) Then
            ddl_CajaBancaria.SelectedIndex = 1
        End If

        Call ActualizarClientes()
        Call ActualizarGrids()

        Dim Horas As DataTable = cn.GetHoras(15)

        Call fn_LlenarDropDown(ddl_A, Horas.Copy(), False)
        Call fn_LlenarDropDown(ddl_De, Horas.Copy(), False)

        txt_Total.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
    End Sub

    Protected Sub ddl_Moneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Moneda.SelectedIndexChanged
        Call ActualizarGrids()
    End Sub

    Private Sub ActualizarGrids()
        If (ddl_Moneda.SelectedValue > 0) Then

            Dim Id_Moneda As Integer = ddl_Moneda.SelectedValue
            Dim dt_Billetes As DataTable = cn.fn_SolicitudDotaciones_GetDenominaciones(Id_Moneda, "B")
            Dim dt_Monedas As DataTable = cn.fn_SolicitudDotaciones_GetDenominaciones(Id_Moneda, "M")

            pTabla("Tbl_Billetes") = dt_Billetes
            If dt_Billetes.Rows.Count.Equals(0) Then
                gv_Billetes.DataSource = Nothing
                gv_Billetes.DataBind()
            Else
                gv_Billetes.DataSource = fn_MostrarSiempre(dt_Billetes)
                gv_Billetes.DataBind()
            End If

            pTabla("Tbl_Monedas") = dt_Monedas
            If dt_Monedas.Rows.Count.Equals(0) Then
                gv_Monedas.DataSource = Nothing
                gv_Monedas.DataBind()
            Else
                gv_Monedas.DataSource = fn_MostrarSiempre(dt_Monedas)
                gv_Monedas.DataBind()
            End If

        Else
            pTabla("Tbl_Billetes") = Nothing
            pTabla("Tbl_Monedas") = Nothing

            gv_Billetes.DataSource = fn_CreaGridVacio("Id_Denominacion,Denominacion,Cantidad,Total")
            gv_Billetes.DataBind()

            gv_Monedas.DataSource = fn_CreaGridVacio("Id_Denominacion,Denominacion,Cantidad,Total")
            gv_Monedas.DataBind()

        End If
    End Sub

    Protected Sub gv_Billetes_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Billetes.RowEditing
        gv_Billetes.EditIndex = e.NewEditIndex

        Dim dt_Billetes As DataTable = pTabla("Tbl_Billetes")
        gv_Billetes.DataSource = dt_Billetes
        gv_Billetes.DataBind()
    End Sub

    Protected Sub gv_Monedas_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gv_Monedas.RowEditing
        gv_Monedas.EditIndex = e.NewEditIndex

        Dim dt_Monedas As DataTable = pTabla("Tbl_Monedas")
        gv_Monedas.DataSource = dt_Monedas
        gv_Monedas.DataBind()
    End Sub

    Protected Sub gv_Monedas_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Monedas.RowUpdating
        Try


            Dim Id_Denominacion As Integer = Integer.Parse(gv_Monedas.DataKeys(e.RowIndex).Value)


            Dim dt_Monedas As DataTable = pTabla("Tbl_Monedas")

            Dim Filas = From T As DataRow In dt_Monedas Where T("Id_Denominacion") = Id_Denominacion Select T

            If Filas.Count = 0 Then
                fn_Alerta("No se puede Actualizar la fila debido a un error.")
                Exit Sub
            End If

            Dim fila As DataRow = Filas(0)

            Dim Cantidad As Integer = CInt(CType(gv_Monedas.Rows(e.RowIndex).Cells(2).Controls(0), TextBox).Text)

            fila("Cantidad") = Cantidad
            fila("Total") = FormatNumber((Cantidad * CDec(fila("Denominacion"))), 2)

            dt_Monedas.AcceptChanges()

            gv_Monedas.EditIndex = -1
            gv_Monedas.DataSource = dt_Monedas
            gv_Monedas.DataBind()
            pTabla("Tbl_Monedas") = dt_Monedas
        Catch ex As Exception
            fn_Alerta("No se puede Actualizar la fila debido a un error.")
            Exit Sub
        End Try
    End Sub

    Protected Sub gv_Billetes_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gv_Billetes.RowUpdating
        Try

            Dim Id_Denominacion As Integer = gv_Billetes.DataKeys(e.RowIndex).Value
            Dim dt_Billetes As DataTable = pTabla("Tbl_Billetes")

            Dim Filas = From T As DataRow In dt_Billetes Where T("Id_Denominacion") = Id_Denominacion Select T

            If Filas.Count = 0 Then
                fn_Alerta("No se puede Actualizar la fila debido a un error.")
                Exit Sub
            End If

            Dim fila As DataRow = Filas(0)
            Dim Cantidad As Integer = CInt(CType(gv_Billetes.Rows(e.RowIndex).Cells(2).Controls(0), TextBox).Text)

            fila("Cantidad") = Cantidad
            fila("Total") = FormatNumber((Cantidad * CDec(fila("Denominacion"))), 2)

            dt_Billetes.AcceptChanges()

            gv_Billetes.EditIndex = -1
            gv_Billetes.DataSource = dt_Billetes
            gv_Billetes.DataBind()
            pTabla("Tbl_Billetes") = dt_Billetes
        Catch ex As Exception
            fn_Alerta("No se puede Actualizar la fila debido a un error.")
            Exit Sub
        End Try

    End Sub

    Protected Sub gv_Billetes_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Billetes.RowCancelingEdit
        gv_Billetes.EditIndex = -1
        gv_Billetes.DataSource = pTabla("Tbl_Billetes")
        gv_Billetes.DataBind()
    End Sub

    Protected Sub gv_Monedas_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gv_Monedas.RowCancelingEdit
        gv_Monedas.EditIndex = -1
        gv_Monedas.DataSource = pTabla("Tbl_Monedas")
        gv_Monedas.DataBind()
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click
        Dim fecha As Date

        If Not Date.TryParse(txt_FechaEntrega.Text, fecha) Then
            Fn_Alerta("Debe capturar una fecha valida.")
            Exit Sub
        End If

        If fecha < Today Then
            Fn_Alerta("Debe seleccionar una fecha mayor o igual a la actual.")
            Exit Sub
        End If

        If ddl_De.SelectedItem.Text = "" Then
            Fn_Alerta("Debe seleccionar una hora de entrega mínima.")
            Exit Sub
        End If

        If ddl_A.SelectedItem.Text = "" Then
            Fn_Alerta("Debe seleccionar una hora de entrega máxima.")
            Exit Sub
        End If

        If CDate(ddl_De.Text) >= CDate(ddl_A.Text) Then
            Fn_Alerta("Debe Capturar una hora de entrega válida.")
            Exit Sub
        End If

        If ddl_CajaBancaria.SelectedValue = 0 Then
            Fn_Alerta("Debe seleccionar una caja bancaria.")
            Exit Sub
        End If
        If ddl_Cliente.SelectedValue = 0 Then
            Fn_Alerta("Debe seleccionar un cliente.")
            Exit Sub
        End If

        Dim dt_Clientes As DataTable = pTabla("tbl_Clientes")
        Dim Id_CajaBancaria As Integer = ddl_CajaBancaria.SelectedValue
        Dim Id_Cliente As Integer = ddl_Cliente.SelectedValue

        Dim Filas = From t As DataRow In dt_Clientes Where t("Id_Cliente") = Id_Cliente Select t

        If Filas.Count = 0 Then
            Fn_Alerta("No se puede guardar debido a un error.")
            Exit Sub
        End If

        Dim fila As DataRow = Filas(0)

        Dim Id_ClienteP As Integer = fila("Id_ClienteP")
        Dim Tbl_Billetes As DataTable = If(pTabla("Tbl_Billetes") Is Nothing, fn_CreaGridVacio("Id_Denominacion,Denominacion,Cantidad,Total"), pTabla("Tbl_Billetes"))
        Dim Tbl_Monedas As DataTable = If(pTabla("Tbl_Monedas") Is Nothing, fn_CreaGridVacio("Id_Denominacion,Denominacion,Cantidad,Total"), pTabla("Tbl_Monedas"))
        Dim IdMoneda As Integer = ddl_Moneda.SelectedValue
        Dim Importe As Single = 0

        For Each r As DataRow In Tbl_Billetes.Rows
            Importe += CSng(r("Total"))
        Next

        For Each r As DataRow In Tbl_Monedas.Rows
            Importe += CSng(r("Total"))
        Next

        If Importe.Equals(0) Then
            Fn_Alerta("Debe capturar un importe valido.")
            Exit Sub
        End If

        Dim Hora_Entrega As String = ddl_De.SelectedItem.Text & "/" & ddl_A.SelectedItem.Text
        Dim Resultado As Boolean = cn.fn_SolicitudDotaciones_Guardar(Id_Cliente, Id_CajaBancaria, Id_ClienteP, Importe, _
                                                                     fecha, IdMoneda, Tbl_Monedas, Tbl_Billetes, UCase(txt_Comentarios.Text), _
                                                                     ddl_Moneda.SelectedItem.Text, ddl_Cliente.SelectedItem.Text, Hora_Entrega)

        If Resultado Then
            cn.fn_Crear_Log(pId_Login, "SOLICITUD DOTACION DIA : " & fecha & "/ CLIENTE: " & ddl_Cliente.SelectedItem.Text & _
                            "/ MONEDA: " & ddl_Moneda.SelectedItem.Text & "/ CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & "/ SOLICITA: " & pNombre_Cliente & "/" & pNombre_Usuario)

            Call Limpiar()
            Fn_Alerta("Se ha solicitado su dotación con exito")
        Else
            Fn_Alerta("No se puede guardar debido a un error")
        End If

    End Sub

    Protected Sub gv_Billetes_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Billetes.RowDataBound
     
        If e.Row.RowType = DataControlRowType.DataRow Then
            If IsDBNull(gv_Billetes.DataKeys(0).Value) OrElse CStr(gv_Billetes.DataKeys(0).Value) = "" OrElse CStr(gv_Billetes.DataKeys(0).Value) = "0" Then
                e.Row.Cells(1).Controls.Clear()
            Else
                Dim txt_Cantidad As TextBox = e.Row.Cells(1).FindControl("txt_Cantidad")
                txt_Cantidad.Text = If(IsDBNull(e.Row.DataItem("Cantidad")), 0, e.Row.DataItem("Cantidad"))
            End If
        End If
    End Sub

    Protected Sub gv_Monedas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_Monedas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If IsDBNull(gv_Monedas.DataKeys(0).Value) OrElse CStr(gv_Monedas.DataKeys(0).Value) = "" OrElse CStr(gv_Monedas.DataKeys(0).Value) = "0" Then

                e.Row.Cells(1).Controls.Clear()
            Else
                Dim txt_Cantidad As TextBox = e.Row.Cells(1).FindControl("txt_Cantidad")
                txt_Cantidad.Text = If(IsDBNull(e.Row.DataItem("Cantidad")), 0, e.Row.DataItem("Cantidad"))
            End If
        End If
    End Sub

    Protected Function ActualizarTxtBilletes() As Single
        Dim dt_Billetes As DataTable = pTabla("Tbl_Billetes")
        Dim Acumulado As Single = 0
        If ddl_Moneda.SelectedIndex > 0 Then
            For Each r As GridViewRow In gv_Billetes.Rows
                Dim txt_Cantidad As TextBox = r.Cells(1).FindControl("txt_Cantidad")
                Dim IdDenominacion As Integer = Integer.Parse(gv_Billetes.DataKeys(r.RowIndex).Value)
                Dim row As DataRow = dt_Billetes.Select("Id_Denominacion = '" & IdDenominacion & "'")(0)

                Dim Cantidad As Integer
                If Not Integer.TryParse(txt_Cantidad.Text, Cantidad) Then Cantidad = 0
                row("Cantidad") = Cantidad
                row("Total") = FormatNumber(CSng(row("Denominacion") * Cantidad), 2)
                Acumulado += row("Total")
            Next

            pTabla("Tbl_Billetes") = dt_Billetes

            gv_Billetes.DataSource = dt_Billetes
            gv_Billetes.DataBind()
        End If
        Return Acumulado

    End Function

    Protected Function ActualizarTxtMonedas() As Single
        Dim dt_Monedas As DataTable = pTabla("Tbl_Monedas")
        Dim Acumulado As Single = 0
        If ddl_Moneda.SelectedIndex > 0 Then
            For Each r As GridViewRow In gv_Monedas.Rows
                Dim txt_Cantidad As TextBox = r.Cells(1).FindControl("txt_Cantidad")
                Dim IdDenominacion As Integer = Integer.Parse(gv_Monedas.DataKeys(r.RowIndex).Value)
                Dim row As DataRow = dt_Monedas.Select("Id_Denominacion = '" & IdDenominacion & "'")(0)

                Dim Cantidad As Integer
                If Not Integer.TryParse(txt_Cantidad.Text, Cantidad) Then Cantidad = 0
                row("Cantidad") = Cantidad
                row("Total") = FormatNumber(CSng(row("Denominacion") * Cantidad), 2)
                Acumulado += row("Total")
            Next

            pTabla("Tbl_Monedas") = dt_Monedas

            gv_Monedas.DataSource = dt_Monedas
            gv_Monedas.DataBind()
        End If
        Return Acumulado

    End Function

    Protected Sub gv_Monedas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Monedas.PageIndexChanging
        Dim dt_Monedas As DataTable = pTabla("Tbl_Monedas")
        gv_Monedas.PageIndex = e.NewPageIndex
        gv_Monedas.DataSource = dt_Monedas
        gv_Monedas.DataBind()
    End Sub

    Protected Sub gv_Billetes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Billetes.PageIndexChanging
        Dim dt_Billetes As DataTable = pTabla("Tbl_Billetes")
        gv_Billetes.PageIndex = e.NewPageIndex
        gv_Billetes.DataSource = dt_Billetes
        gv_Billetes.DataBind()
    End Sub

    Protected Sub btn_Actualizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Actualizar.Click
        Dim TotalReal As Single = 0
        Dim TotalUsuario As Single = 0

        If (Not Single.TryParse(txt_Total.Text, TotalUsuario)) Then
            fn_Alerta("Debe capturar un total valido.")
            Exit Sub
        End If
        If ddl_Moneda.SelectedIndex = 0 Then
            Fn_Alerta("Seleccione un tipo de Moneda.")
            Exit Sub
        End If

        If gv_Billetes.Visible Then TotalReal += ActualizarTxtBilletes()
        If gv_Monedas.Visible Then TotalReal += ActualizarTxtMonedas()

        lbl_Total.Text = TextoImporte & FormatCurrency(TotalReal)
        If TotalReal = TotalUsuario Then
            ImgValidado.ImageUrl = ImagenValidado
            btn_Guardar.Enabled = (TotalReal > 0)
        Else
            ImgValidado.ImageUrl = ImagenNoValidado
            btn_Guardar.Enabled = False
        End If

    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        Call ActualizarClientes()
    End Sub

    Protected Sub ActualizarClientes()

        Dim dt_Clientes As DataTable = cn.fn_SolicitudDotaciones_GetClientes(pId_Cliente, ddl_CajaBancaria.SelectedValue)
        pTabla("tbl_Clientes") = dt_Clientes

        Call fn_LlenarDropDown(ddl_Cliente, dt_Clientes, False)

        If ddl_Cliente.Items.Count = 2 Then
            ddl_Cliente.SelectedValue = pId_Cliente
        End If

        ddl_Cliente.Enabled = (pNivel = 1)
    End Sub

    Protected Sub Limpiar()
        ActualizarGrids()
        txt_Total.Text = String.Empty
        ImgValidado.ImageUrl = ImagenNoValidado
        txt_Comentarios.Text = String.Empty
        btn_Guardar.Enabled = False
        lbl_Total.Text = TextoImporte & FormatCurrency(0)
    End Sub

    Protected Sub ddl_De_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_De.SelectedIndexChanged
        If ddl_De.SelectedIndex <= ddl_De.Items.Count - 3 Then
            ddl_A.SelectedIndex = ddl_De.SelectedIndex + 2
        Else
            ddl_A.SelectedIndex = ddl_De.SelectedIndex
        End If

    End Sub
End Class