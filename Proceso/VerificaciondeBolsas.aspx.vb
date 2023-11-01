
Public Class VerificaciondeBolsas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: FICHAS DE MORRALLA")

        gv_Lista.DataSource = fn_CreaGridVacio("Id_Ficha,FechaOperacion,FechaRemision,Remision,Cliente,Entidad,Folio,DiceContener,ImporteRealT,ImporteReal,Diferencia,Grupo")
        gv_Lista.DataBind()

        Dim dt_Cajabancaria As DataTable = cn.fn_SolicitudDotaciones_GetCajasBancarias_DetalleDeposito
        If dt_Cajabancaria Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error de consulta en la información.")
            Exit Sub
        End If

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        Dim dt_GrupoCliente As DataTable = cn.fn_ClientesGruposD_Consultar(pId_Cliente)
        If dt_GrupoCliente Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error de consulta en la información.")
            Exit Sub
        End If

        fn_LlenarDropDownVacio(ddl_FechaCierre)
        fn_LlenarDropDown(ddl_CajaBancaria, dt_Cajabancaria, False)
        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        fn_LlenarDropDown(ddl_GruposClientes, dt_GrupoCliente, False)

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Clientes.Enabled = False
        End If

        fst_Depositos.Style.Add("height", "205px")

    End Sub

    Protected Sub txt_FechaInicioOperacion_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaInicioOperacion.TextChanged
        Call FechaOperaciones()
    End Sub

    Protected Sub txt_FechaFinOperaciones_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaFinOperaciones.TextChanged
        Call FechaOperaciones()
    End Sub

    Private Sub FechaOperaciones()
        Call Limpiar_dgv()
        If txt_FechaInicioOperacion.Text.Trim <> "" AndAlso txt_FechaFinOperaciones.Text.Trim <> "" Then
            Dim dt_FechaCierre As DataTable = cn.fn_MorFichasC_Consultar(CDate(txt_FechaInicioOperacion.Text.Trim), CDate(txt_FechaFinOperaciones.Text.Trim))
            fn_LlenarDropDown(ddl_FechaCierre, dt_FechaCierre, False)
            If txt_FechaInicioOperacion.Text <> txt_FechaFinOperaciones.Text Then
                ddl_FechaCierre.Enabled = False
            Else
                ddl_FechaCierre.Enabled = True
            End If
        End If
    End Sub

    Protected Sub cbx_GruposClientes_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_GruposClientes.CheckedChanged
        ddl_GruposClientes.Enabled = Not cbx_GruposClientes.Checked
        Call Limpiar_dgv()
        ddl_GruposClientes.SelectedValue = "0"
    End Sub

    Sub Limpiar_dgv()

        fst_Depositos.Style.Add("height", "205px")
        gv_Lista.DataSource = fn_CreaGridVacio("Id_Ficha,FechaOperacion,FechaRemision,Remision,Cliente,Entidad,Folio,DiceContener,ImporteRealT,ImporteReal,Diferencia,Grupo")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1
        gv_Lista.PageSize = 35

    End Sub

    Protected Sub gv_Lista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        Dim dt_fichas As DataTable = pTabla("Resultado")

        gv_Lista.SelectedIndex = -1
        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(dt_fichas)
        gv_Lista.DataBind()
        Call ColorearFilas(gv_Lista)

        If gv_Lista.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If
    End Sub

    Private Function Validar(ByRef FechaOperacionInicial As Date, ByRef FechaOperacionFinal As Date) As Boolean

        If (Not Date.TryParse(txt_FechaInicioOperacion.Text, FechaOperacionInicial)) Then
            fn_Alerta("La fecha parece ser incorrecto.")
            Return False
        End If

        If (Not Date.TryParse(txt_FechaFinOperaciones.Text, FechaOperacionFinal)) Then
            fn_Alerta("La fecha parece ser incorrecto.")
            Return False
        End If

        If ddl_FechaCierre.SelectedValue = "0" AndAlso ddl_FechaCierre.Enabled = True Then
            fn_Alerta("Seleccione alguna fecha de cierre.")
            Return False
        End If

        If ddl_CajaBancaria.SelectedValue = "0" Then
            fn_Alerta("Seleccione la Caja Bancaria")
            Return False
        End If

        If ddl_GruposClientes.SelectedValue = "0" And Not cbx_GruposClientes.Checked Then
            fn_Alerta("Seleccione Cliente Grupo o marque la casilla «Todos»")
            Return False
        End If
        Return True

    End Function

    Protected Sub Btn_Consultar_Click(sender As Object, e As EventArgs) Handles Btn_Consultar.Click
        Dim Cliente As String = "TODOS"
        Dim GrupoCliente As String = "TODOS"
        Dim FechaOperacionI As Date
        Dim FechaOperacionF As Date

        If Not Validar(FechaOperacionI, FechaOperacionF) Then Exit Sub

        If FechaOperacionI > FechaOperacionF Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        If Not cbx_Clientes.Checked Then Cliente = ddl_Clientes.SelectedItem.Text
        If Not cbx_GruposClientes.Checked Then GrupoCliente = ddl_GruposClientes.SelectedItem.Text

        Dim dt_FichasMor As DataTable = cn.fn_CargarFichasdeMorralla(FechaOperacionI, FechaOperacionF, ddl_FechaCierre.SelectedValue, ddl_CajaBancaria.SelectedValue, ddl_GruposClientes.SelectedValue, pNivel, ddl_Clientes.SelectedValue, cbx_Clientes.Checked)

        If dt_FichasMor Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar la información.")
            Exit Sub
        End If

        pTabla("Resultado") = dt_FichasMor

        Call cn.fn_Crear_Log(pId_Login, "CONSULTO FECHA OPERACION INICIAL: " & FechaOperacionI & " - FECHA OPERACION FINAL:" & FechaOperacionF & "; FECHA CIERRE: " & ddl_FechaCierre.SelectedItem.Text & _
                                                   ";CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & ";CLIENTE: " & Cliente & "; GRUPO CLIENTE: " & GrupoCliente)

        If dt_FichasMor.Rows.Count = 0 Then
            fn_Alerta("No se encontró información con los filtros establecidos.")
        End If

        If dt_FichasMor.Rows.Count > 1000 Then
            gv_Lista.PageSize = 50
        Else
            gv_Lista.PageSize = 35
        End If

        gv_Lista.SelectedIndex = -1
        fst_Depositos.Style.Remove("height")

        gv_Lista.DataSource = fn_MostrarSiempre(dt_FichasMor)
        gv_Lista.DataBind()
        Call ColorearFilas(gv_Lista)

        If gv_Lista.Rows.Count < 4 Then
            fst_Depositos.Style.Add("height", "205px")
        End If

    End Sub

    Protected Sub btn_Exportar_Click(sender As Object, e As EventArgs) Handles btn_Exportar.Click

        Dim GrupoCliente As String = "TODOS"
        Dim Cliente As String = "TODOS"
        Dim FechaOperacionI As Date
        Dim FechaOperacionF As Date
        Dim dt_FichasMorralla As DataTable = Nothing

        If Not Validar(FechaOperacionI, FechaOperacionF) Then Exit Sub

        If FechaOperacionI > FechaOperacionF Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        If Not cbx_GruposClientes.Checked Then GrupoCliente = ddl_GruposClientes.SelectedItem.Text

        dt_FichasMorralla = cn.fn_CargarFichasdeMorralla(FechaOperacionI, FechaOperacionF, ddl_FechaCierre.SelectedValue, ddl_CajaBancaria.SelectedValue, ddl_GruposClientes.SelectedValue, pNivel, ddl_Clientes.SelectedValue, cbx_Clientes.Checked)

        If dt_FichasMorralla Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar la información.")
            Exit Sub
        End If

        If dt_FichasMorralla.Rows.Count > 0 Then
            Dim col_DerechaOmitir = 0
            If ddl_GruposClientes.SelectedValue > 0 Then col_DerechaOmitir = 1

            Call cn.fn_Crear_Log(pId_Login, "EXPORTÓ FECHA OPERACION INICIAL: " & FechaOperacionI & " - FECHA OPERACION FINAL:" & FechaOperacionF & "; FECHA CIERRE: " & ddl_FechaCierre.SelectedItem.Text & _
                                                   ";CAJA BANCARIA: " & ddl_CajaBancaria.SelectedItem.Text & ";CLIENTE: " & Cliente & "; GRUPO CLIENTE: " & GrupoCliente)

            fn_Exportar_Excel(dt_FichasMorralla, "REPORTE DE FICHAS DE MORRALLA", "Desde: " & txt_FechaInicioOperacion.Text, "Hasta: " & txt_FechaFinOperaciones.Text, 1, col_DerechaOmitir, pNombre_Cliente, pNombre_Usuario)

        End If

    End Sub

    Private Sub ColorearFilas(ByRef gv As GridView)
        Dim Diferencia As Integer = 0
        If IsDBNull(gv.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        For Each gvr As GridViewRow In gv.Rows
            If Double.Parse(gvr.Cells(10).Text) > 0 Then
                gvr.Cells(10).BackColor = Drawing.Color.LightSteelBlue
            ElseIf Double.Parse(gvr.Cells(10).Text) < 0 Then
                gvr.Cells(10).BackColor = Drawing.Color.Coral
            End If
        Next

    End Sub

    Protected Sub cbx_Clientes_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Clientes.Checked
        Call Limpiar_dgv()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Protected Sub ddl_FechaCierre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_FechaCierre.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub

    Protected Sub ddl_GruposClientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_GruposClientes.SelectedIndexChanged
        Call Limpiar_dgv()
    End Sub
End Class