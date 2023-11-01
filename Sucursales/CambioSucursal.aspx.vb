Public Partial Class CambioSucursal
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub

        'Se la agrege para que no guarde Cache
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CAMBIO DE SUCURSAL")
        'If pNivel = 2 Then Response.Redirect("~/Default.aspx")

        Dim dt_Cadenascnx As DataTable = cn.fn_ConsultaCadendas_Conexion(pClave_Corporativo)
        fn_LlenarDropDown(ddl_Sucursales, dt_Cadenascnx, False)

    End Sub

    Protected Sub btn_CmabiarConexion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_CmabiarConexion.Click

        If ddl_Sucursales.SelectedValue = "0" Then
            fn_Alerta("Seleccione la Sucursal.")
            Exit Sub
        End If
        If Not CambiarConexion() Then
            fn_Alerta("No tiene acceso a esta Sucursal")
        Else
            Response.Redirect("~/Default.aspx")
        End If


    End Sub

    Private Function CambiarConexion() As Boolean
        Dim resp As Boolean = False
        'SE CAMBIA EL IDCLIENTE AL CAMBIAR DE CONEXION YA QUE ES DIFERENTE PARA CADA SUCURSAL
        ' Cadena(4) es la Clave_SucursalPropia, pero viene concatenada con la cadena

        '        Dim CadenaDesencriptada As String = cn.fn_Decode(ddl_Sucursales.SelectedValue)
        Dim Cadena() As String = Split(ddl_Sucursales.SelectedValue, ",")

        Cadena(0) = fn_Decode(Cadena(0))
        Cadena(1) = fn_Decode(Cadena(1))
        Cadena(2) = fn_Decode(Cadena(2))
        Cadena(3) = fn_Decode(Cadena(3))

        Dim Id_ClienteAlterno As Integer = cn.fn_Consulta_Id_ClienteXSucursal(pId_Usuario, Cadena(4))
        pClave_SucursalPropia = Cadena(4)
        If Id_ClienteAlterno = 0 Or Id_ClienteAlterno = -1 Then
            Exit Function
        End If

        Call cn.fn_Crear_Log(pId_Login, "CAMBIO DE CONEXION: " & ddl_Sucursales.SelectedItem.Text)
        pId_Cliente = Id_ClienteAlterno
        Session("ConexionLocal") = "Data Source=" & Cadena(0) & "; Initial Catalog=" & Cadena(1) & ";User ID=" & Cadena(2) & ";Password=" & Cadena(3) & ";"
        Session("NombreConexion") = ""
        Session("NombreConexion") = ddl_Sucursales.SelectedItem.Text
        Session("cnx_siac") = Session("ConexionLocal")
        pId_CajaBancaria = cn.fn_Login_GetCajaBancaria(pId_Cliente)
        pId_ClienteOriginal = pId_Cliente 'Se agrego esta linea para que cambie su valo al momento de cambiar de sucursal
        If pId_CajaBancaria = -1 Then
            fn_Alerta("No se pudo obtener el identificador de la Caja Bancaria.")
        End If
        resp = True
        Return resp
    End Function

    Protected Sub ddl_Sucursales_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Sucursales.SelectedIndexChanged

    End Sub
End Class