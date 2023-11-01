
Partial Public Class SolicitudMateriales
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: SOLICITUD DE MATERIALES")

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()

        If dt_Clientes Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

        Dim dt_MaterialCliente As New DataTable
        dt_MaterialCliente.Columns.Add("Id_Inventario")
        dt_MaterialCliente.Columns.Add("Clave")
        dt_MaterialCliente.Columns.Add("Descripcion")
        dt_MaterialCliente.Columns.Add("Existe")
        dt_MaterialCliente.Columns.Add("Precio")
        dt_MaterialCliente.Columns.Add("IDCS")
        Dim comilla As String = "  w""Hola Mundo"""
        'fn_CreaGridVacio("Id_Inventario,Clave,Descripcion,Existe,Precio,IDCS") ' = cn.fn_SolicitudMateriales_GetMateriales(0)
        fn_LlenarDropDown(ddl_Material, dt_MaterialCliente, False)

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
            dt_MaterialCliente = Nothing
            dt_MaterialCliente = cn.fn_SolicitudMateriales_GetMateriales(pId_Cliente)
            fn_LlenarDropDown(ddl_Material, dt_MaterialCliente, False)
        End If
        pTabla("tbl_MaterialCliente") = dt_MaterialCliente

        Dim dt_MaterialAgregar As DataTable = CreaTablaMaterialesAgregar()
        pTabla("tbl_MaterialAgregar") = dt_MaterialAgregar

        gv_MaterialesAgregados.DataSource = fn_MostrarSiempre(dt_MaterialAgregar)
        gv_MaterialesAgregados.DataBind()
        txt_Cantidad.Attributes.CssStyle.Add("TEXT-ALIGN", "right")

    End Sub

    Private Function CreaTablaMaterialesAgregar() As DataTable
        Dim dt_MaterialesAgregar As New DataTable
        dt_MaterialesAgregar = New Data.DataTable
        dt_MaterialesAgregar.Columns.Add("IdCliente", GetType(Integer))
        dt_MaterialesAgregar.Columns.Add("Fecha", GetType(DateTime))
        dt_MaterialesAgregar.Columns.Add("Id_Inventario", GetType(Integer))
        dt_MaterialesAgregar.Columns.Add("Material", GetType(String))
        dt_MaterialesAgregar.Columns.Add("Cantidad", GetType(Integer))
        dt_MaterialesAgregar.Columns.Add("IdCS", GetType(Integer))
        dt_MaterialesAgregar.Columns.Add("Precio", GetType(Decimal))
        Return dt_MaterialesAgregar
    End Function

    Protected Sub btn_Agregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Agregar.Click
        Dim dt_MaterialAgregar As DataTable = pTabla("tbl_MaterialAgregar")
        Dim dt_MaterialCliente As DataTable = pTabla("tbl_MaterialCliente")

        If Not fn_Validar() Then Exit Sub

        'Validar que la cantidad sea válida
        Dim Cantidad As Integer
        If Not Integer.TryParse(txt_Cantidad.Text, Cantidad) Then
            Fn_Alerta("Debe indicar una cantidad válida.")
            Exit Sub
        End If
        If Cantidad <= 0 Then
            Fn_Alerta("Debe indicar una cantidad válida.")
            Exit Sub
        End If

        'Crear la tabla en caso de que no exista ya
        If dt_MaterialAgregar Is Nothing Then
            dt_MaterialAgregar = New Data.DataTable
            dt_MaterialAgregar = CreaTablaMaterialesAgregar()
        End If

        'revisar si el material ya existe en la tbla que se va agregando
        For Each dr_Existe As DataRow In dt_MaterialAgregar.Rows
            If dr_Existe("Id_Inventario") = ddl_Material.SelectedValue Then
                Fn_Alerta("No se puede agragar el material porque ya ha sido agregado.")
                Exit Sub
            End If
        Next

        'MsgBox(dt_MaterialCliente.Rows(ddl_Material.SelectedIndex)("IdCS") + " " + dt_MaterialCliente.Rows(ddl_Material.SelectedIndex)("Precio") + " " + dt_MaterialCliente.Rows(ddl_Material.SelectedIndex)("Clave"))
        'agregar el material a la lista de detalle
        Dim dr_Agregar As DataRow = dt_MaterialAgregar.NewRow()
        dr_Agregar("IdCliente") = pId_Cliente
        dr_Agregar("Fecha") = Date.Parse(txt_Fecha.Text)
        dr_Agregar("Id_Inventario") = ddl_Material.SelectedValue
        dr_Agregar("Material") = ddl_Material.SelectedItem.Text
        dr_Agregar("Cantidad") = Cantidad '14/NOV/2014 PENDIENTE LA TABLA INDEX FALTA SELECCIONE
        dr_Agregar("IdCS") = Integer.Parse(dt_MaterialCliente.Rows(ddl_Material.SelectedIndex)("IdCS"))
        Dim precio As String = dt_MaterialCliente.Rows(ddl_Material.SelectedIndex)(4).ToString
        dr_Agregar("Precio") = Decimal.Parse(dt_MaterialCliente.Rows(ddl_Material.SelectedIndex)("Precio"))
        dt_MaterialAgregar.Rows.Add(dr_Agregar)

        pTabla("tbl_MaterialAgregar") = dt_MaterialAgregar
        gv_MaterialesAgregados.DataSource = dt_MaterialAgregar
        gv_MaterialesAgregados.DataBind()

        ddl_Material.SelectedValue = 0
        txt_Cantidad.Text = String.Empty
    End Sub

    Protected Function fn_Validar() As Boolean

        Dim Fecha As Date

        If Not Date.TryParse(txt_Fecha.Text, Fecha) Then
            Fn_Alerta("Debe capturar una fecha valida")
            Return False
        End If

        If Fecha < Today Then
            fn_Alerta("Debe capturar una fecha mayor a la actual")
            Return False
        End If

        If ddl_Material.SelectedValue = "0" Then
            Fn_Alerta("Debe Seleccionar un material")
            Return False
        End If
        If Not IsNumeric(txt_Cantidad.Text) Then
            Fn_Alerta("Debe capturar una cantidad valida")
            Return False
        End If
        Return True

    End Function

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click

        Dim dt_MaterialAgregar As DataTable = pTabla("tbl_MaterialAgregar")
        If dt_MaterialAgregar Is Nothing OrElse dt_MaterialAgregar.Rows.Count = 0 Then
            Fn_Alerta("Debe seleccionar al menos un Material")
            Exit Sub
        End If
        If ddl_Clientes.SelectedValue = 0 And ddl_Clientes.SelectedItem.Text = "SELECCIONE..." Then
            fn_Alerta("Selecciones el Cliente.")
            Exit Sub
        End If

        Dim fecha As Date = Date.Parse(txt_Fecha.Text)

        If Not cn.fn_SolicitudMateriales_Guardar(fecha, dt_MaterialAgregar, ddl_Clientes.SelectedValue, ddl_Clientes.SelectedItem.Text, txt_Comentarios.Text) Then
            Fn_Alerta("No se pudo crear la solicitud")
            Exit Sub
        Else
            Call cn.fn_Crear_Log(pId_Login, "SE GUARDO SOLICITUD DE MATERIAL PARA EL CLIENTE: " & ddl_Clientes.SelectedItem.Text & " / CON FECHA DE ENTREGA: " & fecha & " / SOLICITO: " & pNombre)
        End If

        Fn_Alerta("La solicitud de material se ha creado exitosamente")
        Call LimpiarTodo()

    End Sub

    Sub LimpiarTodo()
        txt_Fecha.Text = String.Empty
        txt_Cantidad.Text = String.Empty
        txt_Comentarios.Text = String.Empty
        ddl_Clientes.SelectedValue = 0
        ddl_Material.SelectedValue = 0

        Call Limpiar()
    End Sub

    Private Sub Limpiar()
        Dim dt_Materiales As New DataTable
        pTabla("tbl_MaterialAgregar") = dt_Materiales

        Dim dt_MaterialesNueva As DataTable = CreaTablaMaterialesAgregar()

        gv_MaterialesAgregados.DataSource = fn_MostrarSiempre(dt_MaterialesNueva)
        gv_MaterialesAgregados.DataBind()

    End Sub

    Protected Sub gv_MaterialesAgregados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_MaterialesAgregados.PageIndexChanging
        gv_MaterialesAgregados.PageIndex = e.NewPageIndex
        gv_MaterialesAgregados.DataSource = pTabla("tbl_MaterialAgregar")
        gv_MaterialesAgregados.DataBind()
    End Sub

    Protected Sub gv_MaterialesAgregados_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_MaterialesAgregados.RowDataBound
        gv_MaterialesAgregados.SelectedIndex = -1
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
        Dim dt_MaterialCliente As New DataTable
        dt_MaterialCliente.Columns.Add("Id_Inventario")
        dt_MaterialCliente.Columns.Add("Clave")
        dt_MaterialCliente.Columns.Add("Descripcion")
        dt_MaterialCliente.Columns.Add("Existe")
        dt_MaterialCliente.Columns.Add("Precio")
        dt_MaterialCliente.Columns.Add("IDCS")

        dt_MaterialCliente = cn.fn_SolicitudMateriales_GetMateriales(ddl_Clientes.SelectedValue)
        pTabla("tbl_MaterialCliente") = dt_MaterialCliente
        If Not dt_MaterialCliente Is Nothing Then
            fn_LlenarDropDown(ddl_Material, dt_MaterialCliente, False)
            pTabla("tbl_MaterialCliente") = dt_MaterialCliente
        End If
    End Sub

    Protected Sub gv_MaterialesAgregados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_MaterialesAgregados.SelectedIndexChanged
        If IsDBNull(gv_MaterialesAgregados.DataKeys(0).Value) OrElse CStr(gv_MaterialesAgregados.DataKeys(0).Value) = "" OrElse CStr(gv_MaterialesAgregados.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_MaterialAgregar As DataTable = pTabla("tbl_MaterialAgregar")
        If dt_MaterialAgregar Is Nothing Then Return
        Dim Id_Inventario As Integer = gv_MaterialesAgregados.SelectedDataKey("Id_Inventario")

        Dim filas As DataRow() = dt_MaterialAgregar.Select("Id_Inventario = " & Id_Inventario)

        For Each dr As DataRow In dt_MaterialAgregar.Rows
            If dr("Id_Inventario") = Id_Inventario Then
                dt_MaterialAgregar.Rows.Remove(dr)
                Exit For
            End If
        Next

        pTabla("tbl_MaterialAgregar") = dt_MaterialAgregar
        gv_MaterialesAgregados.DataSource = fn_MostrarSiempre(dt_MaterialAgregar)
        gv_MaterialesAgregados.DataBind()
    End Sub

    Protected Sub ddl_Material_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Material.SelectedIndexChanged
        'txt_Cantidad.Text = String.Empty
        txt_Cantidad.Focus()
    End Sub

    Protected Sub txt_Cantidad_TextChanged(sender As Object, e As EventArgs) Handles txt_Cantidad.TextChanged

    End Sub
End Class