Public Class Conciliacion
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONCILIACION")

        Call Limpiar_Grids()

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()

        If dt_Clientes Is Nothing Then
            fn_Alerta("No se pudo consultar la informacion de la lista de clientes.")
            Exit Sub
        End If

        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

        Dim dt_CajasBancarias As DataTable = cn.fn_SolicitudDotaciones_GetCajasBancarias_DetalleDeposito

        If dt_CajasBancarias Is Nothing Then
            fn_Alerta("No se pudo consultar la informacion.")
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

    End Sub

    Protected Sub btn_Mostrar_Click(sender As Object, e As EventArgs) Handles btn_Mostrar.Click
        ' maximo 7 dias a consultar

        Dim Cliente As String = "TODOS"
        Dim Status As String = "TODOS"
        tbx_ImporteTotal.Text = "0.00"
        tbx_ImporteTotal.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            fn_Alerta("Seleccione la(s) fecha(s).")
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

        If DateDiff(DateInterval.Day, FechaInicial, FechaFinal) > 7 Then
            fn_Alerta("La consulta solo puede ser por 7 dias, es decir por 1 semana unicamente.")
            Exit Sub
        End If

        If Not Validar() Then Exit Sub

        Dim TodosClientes As String = "S"
        If cbx_Todos_Clientes.Checked = False Then TodosClientes = "N"

        fst_conciliacion.Style.Remove("height")
        '--Consulta para Servicio de Traslado ----
        Call gvTV_Vacio()
        Dim dt_conciliacionTV As DataTable = cn.fn_ConsultaBoveda_Conciliacion(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, TodosClientes, pNivel, ddl_CajaBancaria.SelectedValue)

        If dt_conciliacionTV Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar los Traslados.")
        Else
            If dt_conciliacionTV.Rows.Count > 0 Then
                pTabla("tbl_Traslado") = dt_conciliacionTV
                gv_Conciliacion.DataSource = dt_conciliacionTV
                gv_Conciliacion.DataBind()
                gv_Conciliacion.SelectedIndex = -1
                Dim importes As Decimal = 0
                For Each fila As DataRow In dt_conciliacionTV.Rows
                    importes += fila("Importe")
                Next
                tbx_ImporteTotal.Text = Format(importes, "$ ###,###,###0.00##")
            End If
        End If

        '--Consulta para Servicio de Proceso----
        Call gvProceso_Vacio()
        Dim dt_conciliacionPRO As DataTable = cn.fn_ConsultaProservicios_Conciliacion(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, TodosClientes, pNivel, ddl_CajaBancaria.SelectedValue)

        If dt_conciliacionPRO Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar los Servicios.")
        Else
            If dt_conciliacionPRO.Rows.Count > 0 Then
                pTabla("tbl_Proceso") = dt_conciliacionPRO
                gv_Proceso.DataSource = dt_conciliacionPRO
                gv_Proceso.DataBind()
                gv_Proceso.SelectedIndex = -1
            End If
        End If

        '--Consulta para Mostrar los archivos de c/u Remision en servicios----
        Call gvArchivos_Vacio()
        Dim dt_Archivos As DataTable = cn.fn_ConsultaArchivos_Conciliacion(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, TodosClientes, pNivel, ddl_CajaBancaria.SelectedValue)

        If dt_Archivos Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar los Archivos.")
        Else
            If dt_Archivos.Rows.Count > 0 Then
                pTabla("tbl_Archivos") = dt_Archivos
                gv_Archivos.DataSource = dt_Archivos
                gv_Archivos.DataBind()
                gv_Archivos.SelectedIndex = -1
            End If
        End If
    End Sub

    Private Sub Limpiar_Grids()
        tbx_ImporteTotal.Text = "$ 0.00"
        tbx_ImporteTotal.Attributes.CssStyle.Add("TEXT-ALIGN", "right")
        fst_conciliacion.Style.Add("height", "230px")
        Call gvTV_Vacio()
        Call gvProceso_Vacio()
        Call gvArchivos_Vacio()
    End Sub

    Private Sub gvTV_Vacio()
        gv_Conciliacion.DataSource = fn_CreaGridVacio("Id_Remision,Remision,FechaBoveda,Cliente,Importe,Status")
        gv_Conciliacion.DataBind()
    End Sub

    Private Sub gvProceso_Vacio()
        gv_Proceso.DataSource = fn_CreaGridVacio("Id_Remision,Remision,Fecha,Cliente,Status")
        gv_Proceso.DataBind()
    End Sub

    Private Sub gvArchivos_Vacio()
        gv_Archivos.DataSource = fn_CreaGridVacio("Id_Remision,Remision,Genera,Aplicacion,Archivo")
        gv_Archivos.DataBind()
    End Sub

    Private Function Validar() As Boolean
        If pNivel = 1 Then

            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Return False
            End If
        End If

        If ddl_CajaBancaria.SelectedValue = "0" Then
            fn_Alerta("Seleccione una Caja Bancaria")
            Return False
        End If
        Return True

    End Function

    Protected Sub cbx_Todos_Clientes_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar_Grids()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
    Call Limpiar_Grids()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
         Call Limpiar_Grids()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
      Call Limpiar_Grids()
    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
         Call Limpiar_Grids()
    End Sub

    Protected Sub gv_Conciliacion_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Conciliacion.PageIndexChanging
        gv_Conciliacion.PageIndex = e.NewPageIndex
        gv_Conciliacion.DataSource = pTabla("tbl_Traslado")
        gv_Conciliacion.DataBind()
        gv_Conciliacion.SelectedIndex = -1

    End Sub

    Protected Sub gv_Proceso_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Proceso.PageIndexChanging
        gv_Proceso.PageIndex = e.NewPageIndex
        gv_Proceso.DataSource = pTabla("tbl_Proceso")
        gv_Proceso.DataBind()
        gv_Proceso.SelectedIndex = -1
    End Sub

    Protected Sub gv_Archivos_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Archivos.PageIndexChanging
        gv_Archivos.PageIndex = e.NewPageIndex
        gv_Archivos.DataSource = pTabla("tbl_Archivos")
        gv_Archivos.DataBind()
        gv_Archivos.SelectedIndex = -1

    End Sub
    Protected Sub btn_Exportar_Click(sender As Object, e As EventArgs) Handles btn_Exportar.Click
        'fn que exporta a excel añadiendo 2 columnas nuevas
        ' los campos(fecha procesa y status de la segunda consulta)

        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            fn_Alerta("Seleccione la(s) fecha(s).")
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
        If Not Validar() Then Exit Sub
        'validar las fechas en un rango de 7 dias /ya que las consultas son pesadas

        Dim TodosClientes As String = "S"
        If cbx_Todos_Clientes.Checked = False Then TodosClientes = "N"

        '--Consulta para Servicio de Traslado ----
        Dim dt_conciliacionTV As DataTable = cn.fn_ConsultaBoveda_Conciliacion(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, TodosClientes, pNivel, ddl_CajaBancaria.SelectedValue)

        If dt_conciliacionTV Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar los Traslados.")
            Exit Sub
        End If

        If dt_conciliacionTV.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        dt_conciliacionTV.Columns.Add("FechaProcesa")
        dt_conciliacionTV.Columns.Add("StatusProcesa")
        dt_conciliacionTV.Columns.Add("FechaGenera")
        dt_conciliacionTV.Columns.Add("FechaAplicacion")
        dt_conciliacionTV.Columns.Add("Archivo")

        Dim dt_conciliacionPRO As DataTable = cn.fn_ConsultaProservicios_Conciliacion(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, TodosClientes, pNivel, ddl_CajaBancaria.SelectedValue)

        Dim dt_concilArchivo As DataTable = cn.fn_ConsultaArchivos_Conciliacion(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, TodosClientes, pNivel, ddl_CajaBancaria.SelectedValue)

        '---Agregar contenido a las columnas agregadas ---------
        For Each dr_tv As DataRow In dt_conciliacionTV.Rows

            ' agrega las columnas de proceso
            For Each dr_pro As DataRow In dt_conciliacionPRO.Rows
                If dr_pro("Id_Remision") = dr_tv("Id_Remision") Then
                    dr_tv("FechaProcesa") = dr_pro("Fecha")
                    dr_tv("StatusProcesa") = dr_pro("Status")
                    dt_conciliacionPRO.Rows.Remove(dr_pro)
                    Exit For
                End If
            Next

            ' agrega las columnas de archivo
            For Each dr_archivo As DataRow In dt_concilArchivo.Rows
                If dr_archivo("Id_Remision") = dr_tv("Id_Remision") Then
                    dr_tv("FechaGenera") = dr_archivo("Genera")
                    dr_tv("FechaAplicacion") = dr_archivo("Aplicacion")
                    dr_tv("Archivo") = dr_archivo("Archivo")
                    dt_concilArchivo.Rows.Remove(dr_archivo)
                    Exit For
                End If
            Next
        Next
        '----------------------

        Call fn_Exportar_Excel(dt_conciliacionTV, "CONSULTA DE CONCILIACIÓN", "Desde: " & txt_FInicial.Text, "Hasta: " & txt_FFinal.Text, 1)

    End Sub

End Class