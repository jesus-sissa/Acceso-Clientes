Partial Public Class ConsultaActasDiferencia
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE ACTAS DE DIFERENCIA")

        gv_CV.DataSource = fn_CreaGridVacio("Id_Acta,Fecha,Acta,Remision,Caja,Cliente,Tipo Diferencia,Nombre,Status,Comentarios,Comentarios_Valida")
        gv_CV.DataBind()

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

        Dim dt_CajasBancarias As DataTable = cn.fn_ConsultaActasDiferencias_CajasBancarias_Get
        If dt_CajasBancarias Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_CajaBancaria, dt_CajasBancarias, False)

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
        End If
        fst_ActasDiferencia.Style.Add("height", "205px")

    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Sub Limpiar()
        fst_ActasDiferencia.Style.Add("height", "205px")
        gv_CV.DataSource = fn_CreaGridVacio("Id_Acta,Fecha,Acta,Remision,Caja,Cliente,Tipo Diferencia,Nombre,Status,Comentarios,Comentarios_Valida")
        gv_CV.DataBind()
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click
        If txt_FInicial.Text = "" Or txt_FFinal.Text = "" Then
            Fn_Alerta("Seleccione un Rango de Fechas Correcto.")
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

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                Fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        If ddl_CajaBancaria.SelectedValue = "0" Then
            Fn_Alerta("Seleccione la Caja Bancaria.")
            Exit Sub
        End If

        Dim Cliente As String = If(cbx_Todos_Clientes.Checked, "TODOS", ddl_Clientes.SelectedItem.Text)

        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente)

        Dim dt_Diferencia As DataTable = cn.fn_ConsultaActasDiferencia_Get(FechaInicial, FechaFinal, CInt(ddl_Clientes.SelectedValue), cbx_Todos_Clientes.Checked, pNivel, CInt(ddl_CajaBancaria.SelectedValue))

        If dt_Diferencia Is Nothing Then
            fn_Alerta("Ocurrio un error al Cargar los Datos.")
            Exit Sub
        End If
        pTabla("Resultado") = dt_Diferencia

        fst_ActasDiferencia.Style.Remove("height")

        gv_CV.DataSource = fn_MostrarSiempre(dt_Diferencia)
        gv_CV.DataBind()
        gv_CV.Focus()

        If gv_CV.Rows.Count < 4 Then
            fst_ActasDiferencia.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_CV_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_CV.PageIndexChanging
        gv_CV.PageIndex = e.NewPageIndex
        gv_CV.DataSource = pTabla("Resultado")
        gv_CV.DataBind()

        gv_CV.SelectedIndex = -1

        If gv_CV.Rows.Count < 4 Then
            fst_ActasDiferencia.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub ddl_CajaBancaria_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_CajaBancaria.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FInicial_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_FInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FFinal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_FFinal.TextChanged
        Call Limpiar()
    End Sub
End Class