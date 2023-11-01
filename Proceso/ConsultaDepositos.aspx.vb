Imports System.IO

Partial Public Class ConsultaDepositos
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Nota: Si se va a quitar o agregar una columna al gv, favor de reacomodar el item en el sub de [ColorearFilas]
        'que es el que se encarga de colorear los items cuando tiene moneda Extranjera.
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE DEPOSITOS")
        fst_Depositos.Style.Add("height", "205px")

        gv_Depositos.DataSource = fn_CreaGridVacio("Id_Servicio,Remision,Fecha,Cia,Fichas,Dice_Contener,ImporteReal,Envases,Status,Cliente,HoraFinaliza,Moneda Extranjera,Diferencia")
        gv_Depositos.DataBind()

        Call Limpiar_gvFichasVacio()

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()
        If dt_Clientes Is Nothing Then
            Fn_Alerta("No se pudo consultar la informacion de la lista de clientes.")
            Exit Sub
        End If
        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

        'Dim DTCLI As Integer = dt_Clientes.Rows.Count '=277 REGISTROS

        Dim dt_CajasBancarias As DataTable = cn.fn_SolicitudDotaciones_GetCajasBancarias_DetalleDeposito
        If dt_CajasBancarias Is Nothing Then
            Fn_Alerta("No se pudo consultar la informacion.")
            Exit Sub
        End If
        Call fn_LlenarDropDown(ddl_CajaBancaria, dt_CajasBancarias, False)

        If ddl_CajaBancaria.Items.Count = 2 Then
            ddl_CajaBancaria.Items(1).Selected = True
        End If

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If
        txt_FFinal.Text = Date.Now.ToShortDateString
        txt_FInicial.Text = Date.Now.ToShortDateString
        tbx_TotalMonedas.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_TotalBilletes.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_ImporteTotal.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

        tbx_TotalMonedasP.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_TotalBilletesP.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        tbx_ImporteTotalP.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

        lbl_Faltantes.BackColor = Drawing.Color.Coral
        lbl_Sobrantes.BackColor = Drawing.Color.LightSteelBlue
        lbl_MonedaExtranjera.BackColor = Drawing.Color.LightGreen

    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar_gvDepositosVacio()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Protected Sub cbx_Todos_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos.CheckedChanged
        ddl_Status.Enabled = Not cbx_Todos.Checked
        Call Limpiar_gvDepositosVacio()
        ddl_Status.SelectedValue = "0"
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click

        Dim Cliente As String = "TODOS"
        Dim Status As String = "TODOS"
        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            Fn_Alerta("Seleccione la(s) fecha(s).")
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
        If Not Validar() Then Exit Sub

        Dim dt_Servicios As DataTable = cn.fn_ConsultaDepositos_Dotaciones(FechaInicial, FechaFinal, ddl_Status.SelectedValue, cbx_Todos.Checked, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel, ddl_CajaBancaria.SelectedValue)

        If dt_Servicios Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar los Depósitos.")
            Exit Sub
        End If

        If dt_Servicios.Rows.Count > 0 Then
            dt_Servicios.Columns.Add("Moneda Extranjera")
            Dim dt_Temporal As DataTable = cn.fn_ConsultaDepositos_XMoneda(FechaInicial, FechaFinal, ddl_Status.SelectedValue, cbx_Todos.Checked, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel, ddl_CajaBancaria.SelectedValue)

            Call AgregarColumna(dt_Servicios, dt_Temporal)
            dt_Temporal.Dispose()

            pTabla("Resultado") = dt_Servicios
            gv_Depositos.DataSource = fn_MostrarSiempre(dt_Servicios)
            gv_Depositos.DataBind()
            gv_Depositos.SelectedIndex = -1

            Call ColorearFilas(gv_Depositos)

            If Not cbx_Todos_Clientes.Checked Then
                Cliente = ddl_Clientes.SelectedItem.Text
            End If

            If Not cbx_Todos.Checked Then
                Status = ddl_Status.SelectedItem.Text
            End If

            Call cn.fn_Crear_Log(pId_Login, "CONSULTO: " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                                 "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & "; ESTATUS: " & Status)
        End If

        fst_Depositos.Style.Remove("height")
        If gv_Depositos.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub btn_Exportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Exportar.Click

        Dim Cliente As String = "TODOS"
        Dim Status As String = "TODOS"

        If Not Validar() Then Exit Sub

        Dim Serial As String() = Split(txt_FInicial.Text, "/")
        Dim FechaInicial As Date = DateSerial(Serial(2), Serial(1), Serial(0))
        Serial = Split(txt_FFinal.Text, "/")
        Dim FechaFinal As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        If DateDiff(DateInterval.Day, FechaFinal, FechaInicial) > 0 Then
            Fn_Alerta("La fecha final no debe superar a la fecha inicial.")
            Exit Sub
        End If

        If Not cbx_Todos_Clientes.Checked Then
            Cliente = ddl_Clientes.SelectedItem.Text
        End If

        If Not cbx_Todos.Checked Then
            Status = ddl_Status.SelectedItem.Text
        End If

        Call cn.fn_Crear_Log(pId_Login, "EXPORTO DEL: " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                            "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & "; ESTATUS: " & Status)

        Dim dt_temporal As DataTable = cn.fn_ConsultaDepositos_Dotaciones(FechaInicial, FechaFinal, ddl_Status.SelectedValue, cbx_Todos.Checked, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel, ddl_CajaBancaria.SelectedValue)

        If dt_temporal.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If
        Call fn_Exportar_Excel(dt_temporal, Page.Title, "Desde: " & txt_FInicial.Text, "Hasta: " & txt_FFinal.Text, 1)
    End Sub

    Protected Sub gv_Fichas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Fichas.PageIndexChanging
        gv_Fichas.PageIndex = e.NewPageIndex
        gv_Fichas.DataSource = pTabla("Fichas")
        gv_Fichas.DataBind()
        gv_Fichas.SelectedIndex = -1
    End Sub

    Protected Sub gv_Efectivo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Efectivo.PageIndexChanging
        gv_Efectivo.PageIndex = e.NewPageIndex
        gv_Efectivo.DataSource = pTabla("Efectivo")
        gv_Efectivo.DataBind()
        gv_Efectivo.SelectedIndex = -1
    End Sub

    Private Sub gv_Parciales_DataBound(sender As Object, e As EventArgs) Handles gv_Parciales.DataBound
        pTabla("ParcialesD") = Nothing
        gv_ParcialD.SelectedIndex = -1
        gv_ParcialD.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_ParcialD.DataBind()
    End Sub

    Protected Sub gv_Parciales_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Parciales.PageIndexChanging
        gv_Parciales.PageIndex = e.NewPageIndex
        gv_Parciales.DataSource = pTabla("Parciales")
        gv_Parciales.DataBind()
        gv_Parciales.SelectedIndex = -1
    End Sub

    Protected Sub gv_Cheques_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Cheques.PageIndexChanging
        gv_Cheques.PageIndex = e.NewPageIndex
        gv_Cheques.DataSource = pTabla("Cheques")
        gv_Cheques.DataBind()
    End Sub

    Protected Sub gv_Fichas_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Fichas.DataBound
        Call Limpiar_gvPestanas()
    End Sub

    Protected Sub gv_Cheques_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Fichas.RowCommand, gv_Cheques.RowCommand
        If e.CommandName = "Numero" Then
            Dim Id_Cheque As Integer = gv_Cheques.DataKeys(e.CommandArgument).Value

            Dim str As String = "<script language=javascript> {window.open('Cheques.aspx?Id=" & Id_Cheque & "');}</script>"
            ClientScript.RegisterStartupScript(GetType(String), "Startup", str)
        End If
    End Sub

    Protected Sub gv_Depositos_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Depositos.DataBound
        gv_Depositos.SelectedIndex = -1
        pTabla("Fichas") = Nothing
        gv_Fichas.SelectedIndex = -1
        Call Limpiar_gvFichasVacio()
    End Sub

    Protected Sub gv_Depositos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Depositos.PageIndexChanging
        gv_Depositos.PageIndex = e.NewPageIndex
        gv_Depositos.DataSource = pTabla("Resultado")
        gv_Depositos.DataBind()
        gv_Depositos.SelectedIndex = -1

        Call ColorearFilas(gv_Depositos)

        pTabla("Fichas") = Nothing
        Call Limpiar_gvFichasVacio()

        fst_Depositos.Style.Remove("height")
        If gv_Depositos.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If

    End Sub

    Protected Sub gv_Depositos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Depositos.SelectedIndexChanged
        If IsDBNull(gv_Depositos.DataKeys(0).Value) OrElse CStr(gv_Depositos.DataKeys(0).Value) = "" _
            OrElse CStr(gv_Depositos.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Id_Servicio As Integer = 0
        Id_Servicio = gv_Depositos.SelectedDataKey("Id_Servicio")
        Call Limpiar_gvFichasVacio()

        Dim dt_Fichas As DataTable = cn.fn_ConsultaDepositos_Fichas(Id_Servicio)
        If dt_Fichas Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar las fichas del depósito.")
        Else
            pTabla("Fichas") = dt_Fichas
            gv_Fichas.DataSource = dt_Fichas
            gv_Fichas.DataBind()
            gv_Fichas.SelectedIndex = -1
        End If
    End Sub

#Region "Precedmientos Privados"
    Private Sub Limpiar_ImportesParcial()
        tbx_TotalMonedasP.Text = String.Empty
        tbx_TotalBilletesP.Text = String.Empty
        tbx_ImporteTotalP.Text = String.Empty
    End Sub

    Private Sub Limpiar_ImportesEfectivo()
        tbx_TotalMonedas.Text = String.Empty
        tbx_TotalBilletes.Text = String.Empty
        tbx_ImporteTotal.Text = String.Empty
        Call Limpiar_ImportesParcial()
    End Sub

    Private Sub Limpiar_gvFichasVacio()
        gv_Fichas.SelectedIndex = -1
        gv_Fichas.DataSource = fn_CreaGridVacio("Id_Ficha,Ficha,Moneda,Tipo,Efectivo,Cheques,Otros,Dif. Efectivo,Dif. Cheques,Dif. Otros")
        gv_Fichas.DataBind()
        Call Limpiar_gvPestanas() 'limpia gv de las pestañas y los Texboximportes
    End Sub

    Private Sub Limpiar_gvDepositosVacio()
        fst_Depositos.Style.Add("height", "205px")
        gv_Depositos.DataSource = fn_CreaGridVacio("Id_Servicio,Remision,Fecha,Cia,Fichas,Dice_Contener,ImporteReal,Envases,Status,Cliente,HoraFinaliza,Moneda Extranjera,Diferencia")
        gv_Depositos.DataBind()
        gv_Depositos.SelectedIndex = -1
        Call Limpiar_gvFichasVacio()
    End Sub

    Private Sub Limpiar_gvParcialesDvacio()
        pTabla("ParcialesD") = Nothing
        gv_ParcialD.SelectedIndex = -1
        gv_ParcialD.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_ParcialD.DataBind()
    End Sub

    Private Sub Limpiar_gvPestanas()

        Call Limpiar_gvParcialesDvacio()

        pTabla("Parciales") = Nothing
        gv_Parciales.SelectedIndex = -1
        gv_Parciales.DataSource = fn_CreaGridVacio("Id_Parcial,Parcial,Hora,Referencia,DiceContener,Direfencia")
        gv_Parciales.DataBind()

        pTabla("Efectivo") = Nothing
        gv_Efectivo.SelectedIndex = -1
        gv_Efectivo.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Efectivo.DataBind()

        pTabla("Cheques") = Nothing
        gv_Cheques.SelectedIndex = -1
        gv_Cheques.DataSource = fn_CreaGridVacio("Banco,Cuenta,Importe,Numero,Id_Cheque")
        gv_Cheques.DataBind()

        pTabla("Otros") = Nothing
        gv_Otros.SelectedIndex = -1
        gv_Otros.DataSource = fn_CreaGridVacio("Documento,Importe,Observaciones")
        gv_Otros.DataBind()

        pTabla("Falsos") = Nothing
        gv_Falsos.SelectedIndex = -1
        gv_Falsos.DataSource = fn_CreaGridVacio("Id_Falso,Denominacion,Cantidad,Importe,Serie,Observaciones")
        gv_Falsos.DataBind()

        Call Limpiar_ImportesEfectivo()
    End Sub

    Private Function Validar() As Boolean
        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Return False
            End If
        End If

        If cbx_Todos.Checked = False AndAlso ddl_Status.SelectedValue = "0" Then
            fn_Alerta("Seleccione el Status")
            Return False
        End If

        If ddl_CajaBancaria.SelectedValue = "0" Then
            fn_Alerta("Seleccione una Caja Bancaria")
            Return False
        End If
        Return True

    End Function

    Private Sub AgregarColumna(ByRef Dt_Consulta_Depositos As DataTable, ByRef Dt_Consulta_Monedas As DataTable)

        For Each Deposito As DataRow In Dt_Consulta_Depositos.Rows
            For Each Moneda As DataRow In Dt_Consulta_Monedas.Rows
                If Deposito("Id_Servicio") = Moneda("Id_Servicio") Then
                    Deposito("Moneda Extranjera") = Moneda("Moneda Extranjera")
                    Dt_Consulta_Monedas.Rows.Remove(Moneda)
                    Exit For
                End If
            Next
        Next

    End Sub

    Private Sub ColorearFilas(ByRef gv As GridView)
        Dim Diferencia As Integer = 0
        If IsDBNull(gv.DataKeys(0).Value) OrElse CStr(gv.DataKeys(0).Value) = "" OrElse CStr(gv.DataKeys(0).Value) = "0" Then Exit Sub

        For Each Elem As GridViewRow In gv.Rows
            If Elem.Cells(12).Text = "SI" Then
                Elem.Cells(12).BackColor = Drawing.Color.LightGreen
            End If
            If Double.Parse(Elem.Cells(8).Text) > 0 Then
                Elem.Cells(8).BackColor = Drawing.Color.LightSteelBlue
            ElseIf Double.Parse(Elem.Cells(8).Text) < 0 Then
                Elem.Cells(8).BackColor = Drawing.Color.Coral
            End If
        Next

    End Sub

#End Region

    Protected Sub ddl_Clientes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar_gvDepositosVacio()
    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        Call Limpiar_gvDepositosVacio()
    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call Limpiar_gvDepositosVacio()
    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
        Call Limpiar_gvDepositosVacio()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
        Call Limpiar_gvDepositosVacio()
    End Sub

    Protected Sub gv_Fichas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Fichas.SelectedIndexChanged
        If IsDBNull(gv_Fichas.DataKeys(0).Value) OrElse CStr(gv_Fichas.DataKeys(0).Value) = "" OrElse CStr(gv_Fichas.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Id_Ficha As Integer = gv_Fichas.SelectedDataKey("Id_Ficha")
        fst_Parciales.Style.Remove("height")
        fst_Efectivo.Style.Remove("height")
        fst_cheques.Style.Remove("height")
        fst_otros.Style.Remove("height")
        fst_BilletesFalsos.Style.Remove("height")
        Call Limpiar_gvPestanas() 'limpia todas las pestañas y los texboximportes

        '----------------
        Dim dt_Efectivo As DataTable = cn.fn_ConsultaDepositos_Efectivo(Id_Ficha)
        If dt_Efectivo Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar el desglose del efectivo de la Ficha.")
        Else
            pTabla("Efectivo") = dt_Efectivo
            gv_Efectivo.DataSource = fn_MostrarSiempre(dt_Efectivo)
            gv_Efectivo.DataBind()

            '-Calculo de monedas y billetes y total
            Dim impBillete As Decimal = 0
            Dim impMoneda As Decimal = 0
            Dim ImpTotalEfectivo As Decimal = 0
            For Each fila As DataRow In dt_Efectivo.Rows
                If fila("Presentacion").ToString.ToUpper = "BILLETE" Then
                    impBillete += fila("Importe")
                End If
                If fila("Presentacion").ToString.ToUpper = "MONEDA" Then
                    impMoneda += fila("Importe")
                End If
            Next
            tbx_TotalMonedas.Text = Format(impMoneda, "$ ###,###,###0.00##")
            tbx_TotalBilletes.Text = Format(impBillete, "$ ###,###,###0.00##")
            tbx_ImporteTotal.Text = Format((impBillete + impMoneda), "$ ###,###,###0.00##")
        End If
        '----------------

        Dim dt_Parciales As DataTable = cn.fn_ConsultaDepositos_Parciales(Id_Ficha)
        If dt_Parciales Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar el desglose de los parciales de la Ficha.")
        Else
            pTabla("Parciales") = dt_Parciales
            gv_Parciales.DataSource = fn_MostrarSiempre(dt_Parciales)
            gv_Parciales.DataBind()
            gv_Parciales.SelectedIndex = -1

        End If
        '----------------------

        Dim dt_Cheques As DataTable = cn.fn_ConsultaDepositos_Cheques(Id_Ficha)
        If dt_Cheques Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar cheques de la ficha seleccionada.")
        Else
            pTabla("Cheques") = dt_Cheques
            gv_Cheques.DataSource = fn_MostrarSiempre(dt_Cheques)
            gv_Cheques.DataBind()
            gv_Cheques.SelectedIndex = -1
        End If

        Dim dt_Otros As DataTable = cn.fn_ConsultaDepositos_Otros(Id_Ficha)
        If dt_Otros Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar otros documentos de la ficha.")
        Else
            pTabla("Otros") = dt_Otros
            gv_Otros.DataSource = fn_MostrarSiempre(dt_Otros)
            gv_Otros.DataBind()
        End If

        Dim dt_Falsos As DataTable = cn.fn_ConsultaDepositos_Falsos(Id_Ficha)
        If dt_Falsos Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar billetes falsos de la ficha seleccionada.")
        Else
            pTabla("Falsos") = dt_Falsos
            gv_Falsos.DataSource = fn_MostrarSiempre(dt_Falsos)
            gv_Falsos.DataBind()
        End If

    End Sub

    Protected Sub gv_Parciales_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Parciales.SelectedIndexChanged
        If IsDBNull(gv_Parciales.DataKeys(0).Value) OrElse CStr(gv_Parciales.DataKeys(0).Value) = "" OrElse CStr(gv_Parciales.DataKeys(0).Value) = "0" Then Exit Sub

        Dim Id_Parcial As Integer = gv_Parciales.SelectedDataKey("Id_Parcial")

        Call Limpiar_gvParcialesDvacio()
        Call Limpiar_ImportesParcial()

        Dim dt_ParcialD As DataTable = cn.fn_ConsultaDepositos_ParcialesDetalle(Id_Parcial)
        If dt_ParcialD Is Nothing Then
            Fn_Alerta("Ocurrió un error al consultar el detalle del Parcial seleccionado.")
        Else

            pTabla("ParcialesD") = dt_ParcialD
            gv_ParcialD.DataSource = fn_MostrarSiempre(dt_ParcialD)
            gv_ParcialD.DataBind()

            '-Calculo de monedas y billetes y total
            Dim impBillete As Decimal = 0
            Dim impMoneda As Decimal = 0
            Dim ImpTotalEfectivo As Decimal = 0
            For Each fila As DataRow In dt_ParcialD.Rows
                If fila("Presentacion").ToString.ToUpper = "BILLETE" Then
                    impBillete += fila("Importe")
                End If
                If fila("Presentacion").ToString.ToUpper = "MONEDA" Then
                    impMoneda += fila("Importe")
                End If
            Next
            tbx_TotalMonedasP.Text = Format(impMoneda, "$ ###,###,###0.00##")
            tbx_TotalBilletesP.Text = Format(impBillete, "$ ###,###,###0.00##")
            tbx_ImporteTotalP.Text = Format((impBillete + impMoneda), "$ ###,###,###0.00##")
        End If

    End Sub

End Class
