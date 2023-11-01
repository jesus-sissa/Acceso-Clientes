Partial Public Class ConsultaDotaciones
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE DOTACIONES")

        gv_Resultado.DataSource = fn_CreaGridVacio("Remision,Fecha,Moneda,Importe,Envases,Status,Id_Dotacion,Cliente,Id_Remision")
        gv_Resultado.DataBind()

        gv_Desglose.DataSource = fn_CreaGridVacio("Id_Dotacion,Denominacion,Cantidad,Id_Denominacion,Id_Moneda,Importe,Presentacion")
        gv_Desglose.DataBind()

        Dim dt_Monedas As DataTable = cn.fn_ConsultaDotacion_Monedas
        If dt_Monedas Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Moneda, dt_Monedas, False)

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        txt_FInicial.Text = Date.Now.ToShortDateString
        txt_FFinal.Text = Date.Now.ToShortDateString
        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If

        fst_Dotaciones.Style.Add("height", "205px")
    End Sub

    Protected Sub cbx_Moneda_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Moneda.CheckedChanged
        ddl_Moneda.Enabled = Not cbx_Moneda.Checked
        Call Limpiar()
        ddl_Moneda.SelectedValue = "0"
    End Sub

    Protected Sub cbx_Status_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Status.CheckedChanged
        ddl_Status.Enabled = Not cbx_Status.Checked
        Call Limpiar()
        ddl_Status.SelectedValue = "0"
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click
        Dim Cliente As String = "TODOS"
        Dim Moneda As String = "TODOS"
        Dim StatusLog As String = "TODOS"

        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            fn_Alerta("Seleccione la(s) fecha(s).")
            Exit Sub
        End If
        If Not Validar() Then Exit Sub

        Dim Serial As String() = Split(txt_FInicial.Text, "/")
        Dim FechaInicial As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        Serial = Split(txt_FFinal.Text, "/")
        Dim FechaFinal As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        If DateDiff(DateInterval.Day, FechaInicial, FechaFinal) > 60 Then
            fn_Alerta("No se permiten consultas de periodos mayores a 60 dias.")
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, FechaFinal, FechaInicial) > 0 Then
            fn_Alerta("La fecha final debe ser mayor a la fecha inicial.")
            Exit Sub
        End If

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text
        If Not cbx_Moneda.Checked Then Moneda = ddl_Moneda.SelectedItem.Text
        If Not cbx_Status.Checked Then StatusLog = ddl_Status.SelectedItem.Text

        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL: " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                           "; MONEDA: " & Moneda & "; ESTATUS: " & StatusLog)

        Dim Id_Moneda As Integer = If(cbx_Moneda.Checked, "0", ddl_Moneda.SelectedValue)
        Dim Status As String = If(cbx_Status.Checked, "T", ddl_Status.SelectedValue)

        Dim dt_DotacionesProceso As DataTable = cn.fn_Consulta_DotacionesProceso(FechaInicial, FechaFinal, Id_Moneda, Status, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)
        pTabla("Resultado") = dt_DotacionesProceso
        fst_Dotaciones.Style.Remove("height")

        gv_Resultado.DataSource = fn_MostrarSiempre(dt_DotacionesProceso)
        gv_Resultado.DataBind()
        gv_Resultado.SelectedIndex = -1

        If gv_Resultado.Rows.Count < 4 Then
            fst_Dotaciones.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Resultado_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Resultado.PageIndexChanging

        Dim dt_DotacionesProceso As DataTable = pTabla("Resultado")
        Dim RutaIndice As Integer = ViewState("RutaIndice")
        Dim RutaPagina As Integer = ViewState("RutaPagina")

        If RutaPagina = e.NewPageIndex Then
            gv_Resultado.SelectedIndex = RutaIndice
        Else
            gv_Resultado.SelectedIndex = -1
        End If

        gv_Resultado.PageIndex = e.NewPageIndex
        gv_Resultado.DataSource = fn_MostrarSiempre(dt_DotacionesProceso)
        gv_Resultado.DataBind()

        fst_Dotaciones.Style.Remove("height")
        If gv_Resultado.Rows.Count < 4 Then
            fst_Dotaciones.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub ddl_Moneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Moneda.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Sub Limpiar()
        fst_Dotaciones.Style.Add("height", "205px")
        gv_Resultado.DataSource = fn_CreaGridVacio("Remision,Fecha,Moneda,Importe,Envases,Status,Id_Dotacion,Cliente,Id_Remision")
        gv_Resultado.DataBind()
        gv_Resultado.SelectedIndex = -1
    End Sub

    Protected Sub ddl_Status_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Status.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub gv_Resultado_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Resultado.DataBound
        gv_Resultado.SelectedIndex = -1

        gv_Desglose.DataSource = fn_CreaGridVacio("Id_Dotacion,Denominacion,Cantidad,Id_Denominacion,Id_Moneda,Importe,Presentacion")
        gv_Desglose.DataBind()
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Protected Sub btn_Exportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Exportar.Click
        fst_Dotaciones.Style.Remove("height")

        Dim Cliente As String = "TODOS"
        Dim Moneda As String = "TODOS"
        Dim StatusLog As String = "TODOS"

        Dim Serial As String() = Split(txt_FInicial.Text, "/")
        Dim FechaInicial As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        Serial = Split(txt_FFinal.Text, "/")
        Dim FechaFinal As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        If DateDiff(DateInterval.Day, FechaInicial, FechaFinal) > 60 Then
            fn_Alerta("No se permiten consultas de periodos mayores a 60 dias.")
            Exit Sub
        End If

        If DateDiff(DateInterval.Day, FechaFinal, FechaInicial) > 0 Then
            fn_Alerta("La fecha final debe ser mayor a la fecha inicial.")
            Exit Sub
        End If

        If Not Validar() Then Exit Sub

        If Not cbx_Todos_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text
        If Not cbx_Moneda.Checked Then Moneda = ddl_Moneda.SelectedItem.Text
        If Not cbx_Status.Checked Then StatusLog = ddl_Status.SelectedItem.Text

        Call cn.fn_Crear_Log(pId_Login, "EXPORTO: " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente & _
                        "; MONEDA: " & Moneda & "; ESTATUS: " & StatusLog)

        Dim Id_Moneda As Integer = If(cbx_Moneda.Checked, "0", ddl_Moneda.SelectedValue)
        Dim Status As String = If(cbx_Status.Checked, "T", ddl_Status.SelectedValue)

        Dim dt_temporal As DataTable = cn.fn_Consulta_DotacionesProceso(FechaInicial, FechaFinal, Id_Moneda, Status, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)
        If dt_temporal.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        fn_Exportar_Excel(dt_temporal, "REPORTE DE DOTACIONES", "Desde: " & FechaInicial, "Hasta: " & FechaFinal, 1)

    End Sub

    Private Function Validar() As Boolean

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Return False
            End If
        End If

        If cbx_Moneda.Checked = False AndAlso ddl_Moneda.SelectedValue = "0" Then
            fn_Alerta("Seleccione una Moneda.")
            Return False
        End If

        If cbx_Status.Checked = False AndAlso ddl_Status.SelectedValue = "0" Then
            fn_Alerta("Seleccione un Status.")
            Return False
        End If
        Return True
    End Function

    Protected Sub gv_Resultado_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Resultado.SelectedIndexChanged
        If IsDBNull(gv_Resultado.DataKeys(0).Value) OrElse CStr(gv_Resultado.DataKeys(0).Value) = "" OrElse CStr(gv_Resultado.DataKeys(0).Value) = "0" Then Exit Sub
        Dim dt_Detalle As DataTable = cn.fn_ConsultaDotacion_Detalle(gv_Resultado.SelectedDataKey("Id_Dotacion"))      
        gv_Desglose.DataSource = dt_Detalle
        gv_Desglose.DataBind()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub gv_Resultado_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Resultado.RowCommand
        If IsDBNull(gv_Resultado.DataKeys(0).Value) OrElse CStr(gv_Resultado.DataKeys(0).Value) = "" OrElse CStr(gv_Resultado.DataKeys(0).Value) = "0" Then Exit Sub
        Select Case e.CommandName.ToUpper
            Case "VERDES"
                gv_Resultado.SelectedIndex = e.CommandArgument
                Num_Remision = gv_Resultado.SelectedDataKey("Id_Remision").ToString
                If Num_Remision <> "" Then
                    Numeros_Env(Num_Remision)
                    Cant_M(cn.fn_ConsultaTraslado_GetMonedas(Num_Remision))
                    Cant_Env(cn.fn_ConsultaTraslado_GetEnvases(Num_Remision))
                    Response.Redirect("~/Traslado//ImprimirR.aspx")
                End If
                
        End Select
    End Sub
    Sub Numeros_Env(Id_Remision As Long)
        Tipo_Remision = "DOT"
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