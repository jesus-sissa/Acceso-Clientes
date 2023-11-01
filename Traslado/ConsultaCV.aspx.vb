Public Partial Class ConsultaCV
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE COMPROBANTE VISITA")

        gv_CV.DataSource = fn_CreaGridVacio("Id_ComprobanteV,Numero,Fecha,Hora,HoraLlegada,HoraSalida,Cliente,Nombre,Status,Comentarios,idc,idcs,idpu")
        gv_CV.DataBind()

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        txt_FInicial.Text = Date.Now.ToShortDateString
        txt_FFinal.Text = Date.Now.ToShortDateString
        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If

        fst_CV.Style.Add("height", "205px")
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = "0"
    End Sub

    Sub Limpiar()
        gv_CV.DataSource = fn_CreaGridVacio("Id_ComprobanteV,Numero,Fecha,Hora,HoraLlegada,HoraSalida,Cliente,Nombre,Status,Comentarios,idc,idcs,idpu")
        gv_CV.DataBind()
        fst_CV.Style.Add("height", "205px")
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click

        If txt_FInicial.Text.Trim = "" Then
            fn_Alerta("Seleccione Fecha Inicio")
            Exit Sub
        End If

        If txt_FFinal.Text.Trim = "" Then
            fn_Alerta("Seleccione Fecha Fin")
            Exit Sub
        End If

        Dim Serial As String() = Split(txt_FInicial.Text, "/")
        Dim FechaInicial As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        Serial = Split(txt_FFinal.Text, "/")
        Dim FechaFinal As Date = DateSerial(Serial(2), Serial(1), Serial(0))

        If DateDiff(DateInterval.Day, FechaFinal, FechaInicial) > 0 Then
            fn_Alerta("Las fechas parece ser incorrecta.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = "0" Then
                Fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim Cliente As String = If(cbx_Todos_Clientes.Checked, "TODOS", ddl_Clientes.SelectedItem.Text)
        Call cn.fn_Crear_Log(pId_Login, "CONSULTO DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente)

        Dim dt_ComprovanteV As DataTable = cn.fn_ConsultaCV_Get(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)

        If dt_ComprovanteV Is Nothing Then
            Fn_Alerta("Ocurrio un error al Cargar los Datos.")
            Exit Sub
        End If

        pTabla("Resultado") = dt_ComprovanteV
        fst_CV.Style.Remove("height")

        gv_CV.DataSource = fn_MostrarSiempre(dt_ComprovanteV)
        gv_CV.DataBind()
        gv_CV.Focus()
        gv_CV.SelectedIndex = -1

        If gv_CV.Rows.Count < 4 Then
            fst_CV.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_CV_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_CV.PageIndexChanging
        gv_CV.PageIndex = e.NewPageIndex
        gv_CV.DataSource = pTabla("Resultado")
        gv_CV.DataBind()
        gv_CV.SelectedIndex = -1

        If gv_CV.Rows.Count < 4 Then
            fst_CV.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub txt_FInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FFinal.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub btn_Exportar_Click(sender As Object, e As EventArgs) Handles btn_Exportar.Click
        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If (Not Date.TryParse(txt_FInicial.Text, FechaInicial)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If (Not Date.TryParse(txt_FFinal.Text, FechaFinal)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = 0 Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim Cliente As String = If(cbx_Todos_Clientes.Checked, "TODOS", ddl_Clientes.SelectedValue)
        Call cn.fn_Crear_Log(pId_Login, "EXPORTO DEL : " & FechaInicial & " AL: " & FechaFinal & "; CLIENTE: " & Cliente)

        Dim dt_ComprovanteV As DataTable = cn.fn_ConsultaCV_Get(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)

        If dt_ComprovanteV.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        fn_Exportar_Excel(dt_ComprovanteV, Page.Title, "Desde: " & FechaInicial, "Hasta: " & FechaFinal, 0, 3)

    End Sub
End Class