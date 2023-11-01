Public Partial Class ReportarAnomalias
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '------>

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: REPORTAR ANOMALIAS")
        Dim dt_Localidad As DataTable = cn.fn_LocalidadCorporativo(pClave_SucursalPropia)
        Call fn_LlenarDropDown(ddl_Localidad, dt_Localidad, False)
        If dt_Localidad.Rows.Count > 0 Then
            ddl_Localidad.SelectedIndex = 1
        End If
        ddl_Localidad_SelectedIndexChanged(sender, e)
        Limpiar()
    End Sub

    Public Sub Limpiar()
        ddl_Rubros.SelectedIndex = 0
        descripcion_falla.Text = ""
    End Sub

    Protected Sub btn_Enviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Enviar.Click
        If ddl_Sucursales.SelectedIndex = 0 Then
            fn_Alerta("Selecciona una sucursal")
            Exit Sub

        End If

        If ddl_Rubros.SelectedIndex = 0 Then
            fn_Alerta("Selecciona un departamento.")
            Exit Sub

        End If

        If descripcion_falla.Text.Trim = "" Then
            fn_Alerta("Capture una descripción.")
            Exit Sub

        End If

        If cn.fn_Mardar_Ticket(descripcion_falla.Text, 2, ddl_Rubros.SelectedValue, pNombre_Cliente, pNombre_Usuario) Then
            fn_Alerta("El ticket se envió Correctamente")
            Limpiar()
        Else
            fn_Alerta("Error al enviar el ticket")
        End If



    End Sub

    Protected Sub ddl_Localidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Localidad.SelectedIndexChanged
        Dim dt_Sucursales As DataTable = cn.Fn_Sucursal_Cliente(pClave_Corporativo, ddl_Localidad.SelectedValue, pClave_Sucursal)
        Dim dt_Rubros As DataTable = cn.fn_Get_Rubros(pNivel)
        If ddl_Localidad.SelectedValue <> 0 Then
            ddl_Sucursales.Enabled = True
            Call fn_LlenarDropDown(ddl_Sucursales, dt_Sucursales, False)
            ddl_Sucursales.SelectedIndex = 1
            fn_LlenarDropDown(ddl_Rubros, dt_Rubros, False)
        Else
            Call fn_LlenarDropDownVacio(ddl_Sucursales)

        End If
    End Sub

    Protected Sub ddl_Rubros_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Rubros.SelectedIndexChanged
        descripcion_falla.Focus()
    End Sub
End Class
