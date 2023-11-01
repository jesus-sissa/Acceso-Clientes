
Partial Public Class DetallesDeposito
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: DETALLE DE DEPOSITO")

        gv_Lista.DataSource = fn_CreaGridVacio("Id_Servicio,Remision,Fecha,Sesion,Cliente,Dice_Contener,ImporteReal,Diferencia,Envases,Envases SN")
        gv_Lista.DataBind()

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar los Clientes.")
            Exit Sub
        End If

        Dim dt_Cajabancaria As DataTable = cn.fn_SolicitudDotaciones_GetCajasBancarias_DetalleDeposito
        If dt_Cajabancaria Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar las Cajas Bancrias.")
            Exit Sub
        End If

        Dim dt_Monedas As DataTable = cn.fn_ConsultaDotacion_Monedas()
        If dt_Monedas Is Nothing Then
            fn_Alerta("Ocurrió un error al intentar consultar las Monedas.")
            Exit Sub
        End If

        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        Call fn_LlenarDropDown(ddl_CajaBancaria, dt_Cajabancaria, False)
        Call fn_LlenarDropDown(ddl_Moneda, dt_Monedas, False)

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If
        fst_Depositos.Style.Add("height", "205px")

        txt_Monedas.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        txt_Billetes.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        txt_ImporteTotal.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

    End Sub

    Protected Sub Btn_Consultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Consultar.Click
        Dim Cliente As String = "TODOS"

        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If Not Validar(FechaInicial, FechaFinal) Then Exit Sub

        If FechaInicial > FechaFinal Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False And ddl_Clientes.SelectedValue = "0" Then
                Fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim SesionDesde As Integer = 0
        Dim SesionHasta As Integer = 0

        Dim dt_Desde As DataTable = cn.ObtenerSesiones(FechaInicial)
        If dt_Desde Is Nothing Then
            Fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Desde.Rows.Count = 0 Then
            Fn_Alerta("No existen detalles para la fecha seleccionada")
            Exit Sub
        Else
            SesionDesde = dt_Desde.Rows(0)("Id_Sesion")
        End If

        Dim dt_Hasta As DataTable = cn.ObtenerSesiones(FechaFinal)
        If dt_Hasta Is Nothing Then
            Fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Hasta.Rows.Count = 0 Then
            Fn_Alerta("No existen detalles para la fecha seleccionada")
            Exit Sub
        Else
            SesionHasta = dt_Hasta.Rows(dt_Hasta.Rows.Count - 1)("Id_Sesion")
        End If

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text

        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL: " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                            "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & "; MONEDA: " & ddl_Moneda.SelectedItem.Text)

        Dim dt_Servicios As DataTable = cn.fn_CargarServicios(ddl_CajaBancaria.SelectedValue, SesionDesde, SesionHasta, ddl_Moneda.SelectedValue, pNivel, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked)

        If dt_Servicios Is Nothing Then
            Fn_Alerta("Ocurrió un error al generar la consulta.")
            Exit Sub
        End If

        pTabla("Resultado") = dt_Servicios

        gv_Lista.SelectedIndex = -1
        fst_Depositos.Style.Remove("height")

        gv_Lista.DataSource = fn_MostrarSiempre(dt_Servicios)
        gv_Lista.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Protected Sub gv_Lista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        Dim dt_Detalle As DataTable = pTabla("Resultado")

        gv_Lista.SelectedIndex = -1
        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(dt_Detalle)
        gv_Lista.DataBind()

        txt_Billetes.Text = ""
        txt_Monedas.Text = ""
        txt_ImporteTotal.Text = ""

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Lista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.SelectedIndexChanged
        If IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_Monedas As DataTable = cn.fn_Desglose_Servicios(gv_Lista.SelectedDataKey("Id_Servicio"), ddl_Moneda.SelectedValue)
        pTabla("TblDesglose") = dt_Monedas

        gv_Desglose.DataSource = fn_MostrarSiempre(dt_Monedas)
        gv_Desglose.DataBind()

        ViewState("RutaIndice") = gv_Lista.SelectedIndex
        ViewState("RutaPagina") = gv_Lista.PageIndex

        '-Calculo de monedas y billetes y total
        Dim impBillete As Decimal = 0
        Dim impMoneda As Decimal = 0
        Dim ImpTotalEfectivo As Decimal = 0

        If dt_Monedas IsNot Nothing AndAlso dt_Monedas.Rows.Count > 0 Then
            For Each fila As DataRow In dt_Monedas.Rows
                If fila("Presentacion").ToString.ToUpper = "BILLETE" Then
                    impBillete += fila("Importe")
                End If
                If fila("Presentacion").ToString.ToUpper = "MONEDA" Then
                    impMoneda += fila("Importe")
                End If
            Next
        End If

        txt_Monedas.Text = Format(impMoneda, "$ ###,###,###0.00##")
        txt_Billetes.Text = Format(impBillete, "$ ###,###,###0.00##")
        txt_ImporteTotal.Text = Format((impBillete + impMoneda), "$ ###,###,###0.00##")

    End Sub

    Sub Limpiar()
        txt_Billetes.Text = String.Empty
        txt_Monedas.Text = String.Empty
        txt_ImporteTotal.Text = String.Empty

        fst_Depositos.Style.Add("height", "205px")
        gv_Lista.DataSource = fn_CreaGridVacio("Id_Servicio,Remision,Fecha,Sesion,Cliente,Dice_Contener,ImporteReal,Diferencia,Envases,Envases SN")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()
    End Sub

    Protected Sub ddl_Moneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Moneda.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub Btn_Exp_Remision_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Exp_Remision.Click
        Dim Cliente As String = "TODOS"

        Dim dt_ReporteGeneral As DataTable

        Dim SesionDesde As Integer = 0
        Dim SesionHasta As Integer = 0
        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If Not Validar(FechaInicial, FechaFinal) Then Exit Sub

        Dim dt_Desde As DataTable = cn.ObtenerSesiones(FechaInicial)
        If dt_Desde Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Desde.Rows.Count = 0 Then
            Fn_Alerta("No existen registros para la fecha seleccionada")
            Exit Sub
        Else
            SesionDesde = dt_Desde.Rows(0)("Id_Sesion")
        End If

        Dim dt_Hasta As DataTable = cn.ObtenerSesiones(FechaFinal)
        If dt_Hasta Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Hasta.Rows.Count = 0 Then
            Fn_Alerta("No existen registros para la fecha seleccionada")
            Exit Sub
        Else
            SesionHasta = dt_Hasta.Rows(dt_Hasta.Rows.Count - 1)("Id_Sesion")
        End If

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text

        Call cn.fn_Crear_Log(pId_Login, "EXPORTO X REMISION DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                            "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & "; MONEDA: " & ddl_Moneda.SelectedItem.Text)

        dt_ReporteGeneral = fn_Convertir_Datatable_ReportexRemision(ddl_CajaBancaria.SelectedValue, ddl_Moneda.SelectedValue, SesionDesde, SesionHasta, 0, "T", pNivel, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked)

        If dt_ReporteGeneral.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        fn_Exportar_Excel(dt_ReporteGeneral, "REPORTE DE DEPOSITOS-EXPORTAR POR REMISION", "Desde: " & txt_FInicial.Text, "Hasta: " & txt_FFinal.Text, 0, 0, "", pNombre, 1)
    End Sub

    Protected Sub Btn_Exp_Ficha_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Btn_Exp_Ficha.Click
        Dim Cliente As String = "TODOS"

        Dim dt_ReporteGeneral As DataTable
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim SesionDesde As Integer = 0
        Dim SesionHasta As Integer = 0

        If Not Validar(FechaInicial, FechaFinal) Then Exit Sub
        Dim Dt_Desde As DataTable = cn.ObtenerSesiones(FechaInicial)
        If Dt_Desde Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf Dt_Desde.Rows.Count = 0 Then
            fn_Alerta("No existen detalles para la fecha seleccionada1")
            Exit Sub
        Else
            SesionDesde = Dt_Desde.Rows(0)("Id_Sesion")
        End If

        Dim dt_Hasta As DataTable = cn.ObtenerSesiones(FechaFinal)
        If dt_Hasta Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Hasta.Rows.Count = 0 Then
            fn_Alerta("No existen detalles para la fecha seleccionada2")
            Exit Sub
        Else
            SesionHasta = dt_Hasta.Rows(dt_Hasta.Rows.Count - 1)("Id_Sesion")
        End If

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text
        Call cn.fn_Crear_Log(pId_Login, "EXPORTO X FICHA DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                            "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & "; MONEDA: " & ddl_Moneda.SelectedItem.Text)

        dt_ReporteGeneral = fn_Convertir_Datatable_ReportexFicha(ddl_CajaBancaria.SelectedValue, ddl_Moneda.SelectedValue, SesionDesde, SesionHasta, 0, "T", pNivel, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked)

        If dt_ReporteGeneral.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        fn_Exportar_Excel(dt_ReporteGeneral, "REPORTE DE DEPOSITOS-EXPORTAR POR FICHA", "Desde: " & txt_FInicial.Text, "Hasta: " & txt_FFinal.Text, 0, 0, "", pNombre, 1)

    End Sub

    Private Function Validar(ByRef FechaInicial As Date, ByRef FechaFinal As Date) As Boolean

        If (Not Date.TryParse(txt_FInicial.Text, FechaInicial)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Return False
        End If

        If (Not Date.TryParse(txt_FFinal.Text, FechaFinal)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Return False
        End If

        If ddl_CajaBancaria.SelectedValue = "0" Then
            fn_Alerta("Seleccione la Caja Bancaria")
            Return False
        End If

        If ddl_Moneda.SelectedValue = "0" Then
            fn_Alerta("Seleccione el Tipo de Moneda.")
            Return False
        End If
        Return True
    End Function

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub gv_Lista_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.DataBound
        gv_Lista.SelectedIndex = -1

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()
    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
        Call Limpiar()
    End Sub

End Class