Public Class FichasdeMorralla
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: REPORTES METRORREY")

        gv_Lista.DataSource = fn_CreaGridVacio("Id_Servicio,Fecha,Estacion,Remision,Entidad,Folio,ImporteEsperado,ImporteReal,Diferencia")
        gv_Lista.DataBind()

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()

        Dim dt_Cajabancaria As DataTable = cn.fn_SolicitudDotaciones_GetCajasBancarias_DetalleDeposito
        If dt_Cajabancaria Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error de consulta en la información.")
            Exit Sub
        End If

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()
        If dt_Clientes Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        Dim dt_Monedas As DataTable = cn.fn_ConsultaDotacion_Monedas()
        If dt_Monedas Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        '----------------------
        fn_LlenarDropDown(ddl_CajaBancaria, dt_Cajabancaria, False)
        fn_LlenarDropDownVacio(ddl_GruposDeposito)
        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        fn_LlenarDropDown(ddl_Moneda, dt_Monedas, False)

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

    Protected Sub Btn_Consultar_Click(sender As Object, e As EventArgs) Handles Btn_Consultar.Click

        Dim Cliente As String = "TODOS"
        Dim GrupoDeposito As String = "TODOS"
        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If Not Validar(FechaInicial, FechaFinal) Then Exit Sub

        If FechaInicial > FechaFinal Then
            Fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim SesionDesde As Integer = 0
        Dim SesionHasta As Integer = 0

        Dim dt_Desde As DataTable = cn.ObtenerSesiones(FechaInicial)
        If dt_Desde Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Desde.Rows.Count = 0 Then
            fn_Alerta("No existen detalles para la fecha seleccionada")
            Exit Sub
        Else
            SesionDesde = dt_Desde(0)("Id_Sesion")
        End If

        Dim dt_Hasta As DataTable = cn.ObtenerSesiones(FechaFinal)
        If dt_Hasta Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Hasta.Rows.Count = 0 Then
            fn_Alerta("No existen detalles para la fecha seleccionada")
            Exit Sub
        Else
            SesionHasta = dt_Hasta.Rows(dt_Hasta.Rows.Count - 1)("Id_Sesion")
        End If

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text
        If Not cbx_GruposDeposito.Checked Then GrupoDeposito = ddl_GruposDeposito.SelectedItem.Text


        Dim dt_ServiciosMetro As DataTable = cn.fn_CargarServiciosMetrorrey(ddl_CajaBancaria.SelectedValue, SesionDesde, SesionHasta, ddl_Moneda.SelectedValue, pNivel, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, ddl_GruposDeposito.SelectedValue)

        If dt_ServiciosMetro Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar la información.")
            Exit Sub
        Else
            pTabla("Resultado") = dt_ServiciosMetro
        End If

        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL: " & FechaInicial & " AL: " & FechaFinal & "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & _
                                          "; GRUPO DE DEPOSITO: " & GrupoDeposito & " ;CLIENTE: " & Cliente & "; MONEDA: " & ddl_Moneda.SelectedItem.Text)

        If dt_ServiciosMetro.Rows.Count = 0 Then
            fn_Alerta("No se encontró infomación con los filtros establecidos.")
        End If

        If dt_ServiciosMetro.Rows.Count > 1000 Then
            gv_Lista.PageSize = 50
        Else
            gv_Lista.PageSize = 35
        End If

        gv_Lista.SelectedIndex = -1
        fst_Depositos.Style.Remove("height")

        gv_Lista.DataSource = fn_MostrarSiempre(dt_ServiciosMetro)
        gv_Lista.DataBind()
        Call ColorearFilas(gv_Lista)

        If gv_Lista.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If

    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        If ddl_CajaBancaria Is Nothing Then Exit Sub
        Call Limpiar_dgv()
        If ddl_CajaBancaria.SelectedValue > 0 Then
            Dim dt_GruposDeposito As DataTable = cn.fn_GruposDeposito_Consultar(ddl_CajaBancaria.SelectedValue)
            fn_LlenarDropDown(ddl_GruposDeposito, dt_GruposDeposito, False)
        Else
            Call fn_LlenarDropDownVacio(ddl_GruposDeposito)
        End If

    End Sub

    Private Function Validar(ByRef FechaInicial As Date, ByRef FechaFinal As Date) As Boolean

        If (Not Date.TryParse(txt_FInicial.Text, FechaInicial)) Then
            Fn_Alerta("El rango de fechas parece ser incorrecto.")
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

        If ddl_GruposDeposito.SelectedValue = "0" And Not cbx_GruposDeposito.Checked Then
            Fn_Alerta("Seleccione Grupo de depósito o marque la casilla «Todos»")
            Return False
        End If

        If ddl_Moneda.SelectedValue = "0" Then
            fn_Alerta("Seleccione el Tipo de Moneda.")
            Return False
        End If
        Return True

    End Function

    Protected Sub gv_Lista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging

        Dim dt_servicio As DataTable = pTabla("Resultado")

        gv_Lista.SelectedIndex = -1
        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(dt_servicio)
        gv_Lista.DataBind()
        Call ColorearFilas(gv_Lista)

        txt_Billetes.Text = ""
        txt_Monedas.Text = ""
        txt_ImporteTotal.Text = ""

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar_dgv()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Protected Sub cbx_GruposDeposito_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_GruposDeposito.CheckedChanged
        ddl_GruposDeposito.Enabled = Not cbx_GruposDeposito.Checked
        Call Limpiar_dgv()
        ddl_GruposDeposito.SelectedValue = "0"
    End Sub

    Sub Limpiar_dgv()
        txt_Billetes.Text = String.Empty
        txt_Monedas.Text = String.Empty
        txt_ImporteTotal.Text = String.Empty

        fst_Depositos.Style.Add("height", "205px")
        gv_Lista.DataSource = fn_CreaGridVacio("Id_Servicio,Fecha,Estacion,Remision,Entidad,Folio,ImporteEsperado,ImporteReal,Diferencia")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1
        gv_Lista.PageSize = 35

        gv_Desglose.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe")
        gv_Desglose.DataBind()

    End Sub

    Private Sub ColorearFilas(ByRef gv As GridView)
        Dim Diferencia As Integer = 0
        If IsDBNull(gv.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        For Each gvr As GridViewRow In gv.Rows
            If Double.Parse(gvr.Cells(8).Text) > 0 Then
                gvr.Cells(8).BackColor = Drawing.Color.LightSteelBlue
            ElseIf Double.Parse(gvr.Cells(8).Text) < 0 Then
                gvr.Cells(8).BackColor = Drawing.Color.Coral
            End If
        Next

    End Sub

    Protected Sub gv_Lista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Lista.SelectedIndexChanged
        'Al Mostrar GridVacio DK="" y al Sourcearlo si no trae nada DK=0
        If IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_Desglose As DataTable = cn.fn_Desglose_Servicios(gv_Lista.SelectedDataKey("Id_Servicio"), ddl_Moneda.SelectedValue)
        pTabla("TblDesglose") = dt_Desglose
        gv_Desglose.DataSource = fn_MostrarSiempre(dt_Desglose)
        gv_Desglose.DataBind()
        ViewState("RutaIndice") = gv_Lista.SelectedIndex
        ViewState("RutaPagina") = gv_Lista.PageIndex

        'Ejecuta el marcador1
        Dim jscript As String = "<script type=text/javascript>marca();</script>"
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "marca", jscript, False)

        '-Calculo de monedas y billetes y total
        Dim impBillete As Decimal = 0
        Dim impMoneda As Decimal = 0
        Dim ImpTotalEfectivo As Decimal = 0

        If dt_Desglose IsNot Nothing AndAlso dt_Desglose.Rows.Count > 0 Then
            For Each dr_Desglose As DataRow In dt_Desglose.Rows
                If dr_Desglose("Presentacion").ToString.ToUpper = "BILLETE" Then
                    impBillete += dr_Desglose("Importe")
                End If
                If dr_Desglose("Presentacion").ToString.ToUpper = "MONEDA" Then
                    impMoneda += dr_Desglose("Importe")
                End If
            Next
        End If

        txt_Monedas.Text = Format(impMoneda, "$ ###,###,###0.00##")
        txt_Billetes.Text = Format(impBillete, "$ ###,###,###0.00##")
        txt_ImporteTotal.Text = Format((impBillete + impMoneda), "$ ###,###,###0.00##")

    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub ddl_GruposDeposito_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_GruposDeposito.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub ddl_Moneda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Moneda.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub btn_Exportar_Click(sender As Object, e As EventArgs) Handles btn_Exportar.Click

        Dim Cliente As String = "TODOS"
        Dim GrupoDeposito As String = "TODOS"
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim SesionDesde As Integer = 0
        Dim SesionHasta As Integer = 0
        Dim dt_ServiciosMetro As DataTable = Nothing
        Dim dt_Sesiones As DataTable = Nothing

        If Not Validar(FechaInicial, FechaFinal) Then Exit Sub

        If FechaInicial > FechaFinal Then
            Fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        dt_Sesiones = cn.ObtenerSesiones(FechaInicial)
        If dt_Sesiones Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Sesiones.Rows.Count = 0 Then
            fn_Alerta("No existen registros para la fecha seleccionada")
            Exit Sub
        Else
            SesionDesde = dt_Sesiones.Rows(0)("Id_Sesion")
        End If
        dt_Sesiones = Nothing

        dt_Sesiones = cn.ObtenerSesiones(FechaFinal)
        If dt_Sesiones Is Nothing Then
            fn_Alerta("Ocurrio un error al generar la consulta")
            Exit Sub
        ElseIf dt_Sesiones.Rows.Count = 0 Then
            fn_Alerta("No existen registros para la fecha seleccionada")
            Exit Sub
        Else
            SesionHasta = dt_Sesiones.Rows(dt_Sesiones.Rows.Count - 1)("Id_Sesion")
        End If

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text
        If Not cbx_GruposDeposito.Checked Then GrupoDeposito = ddl_GruposDeposito.SelectedItem.Text

        dt_ServiciosMetro = cn.fn_CargarServiciosMetrorrey(ddl_CajaBancaria.SelectedValue, SesionDesde, SesionHasta, ddl_Moneda.SelectedValue, pNivel, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, ddl_GruposDeposito.SelectedValue)

        If dt_ServiciosMetro Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar la información.")
            Exit Sub
        End If

        If dt_ServiciosMetro.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        If dt_ServiciosMetro.Rows.Count > 0 Then
            Call cn.fn_Crear_Log(pId_Login, "EXPORTO A EXCEL DEL: " & FechaInicial & " AL: " & FechaFinal & "; CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & _
                                         "; GRUPO DE DEPOSITO: " & GrupoDeposito & " ;CLIENTE: " & Cliente & "; MONEDA: " & ddl_Moneda.SelectedItem.Text)
        
            fn_Exportar_Excel(dt_ServiciosMetro, "REPORTE DE DEPOSITOS", "Desde: " & txt_FInicial.Text, "Hasta: " & txt_FFinal.Text)
        End If

    End Sub

End Class