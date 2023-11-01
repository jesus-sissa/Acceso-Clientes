Imports System.Web.Security
Imports System.Web.Configuration
Imports System.Configuration

Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Drawing

Partial Public Class Login
    Inherits BasePage

#Region "Propiedades"

    Private Property ContraseñaExpirada() As Boolean
        Get
            Return Convert.ToBoolean(Session("ContraseñaExpirada"))
        End Get
        Set(ByVal value As Boolean)
            Session("ContraseñaExpirada") = value
        End Set
    End Property

    Private Property ContraseñaVieja() As String
        Get
            Return Session("ContraseñaVieja")
        End Get
        Set(ByVal value As String)
            Session("ContraseñaVieja") = value
        End Set
    End Property

    Private Property FechaExpiracion() As String
        Get
            Return Session("FechaExpiracion")
        End Get
        Set(ByVal value As String)
            Session("FechaExpiracion") = value
        End Set
    End Property

    Private Property PasswordUSR() As String
        Get
            Return Session("PasswordUSR")
        End Get
        Set(ByVal value As String)
            Session("PasswordUSR") = value
        End Set
    End Property

    Private Property CodigoCaptcha() As String
        Get
            Return Session("CodigoCaptcha")
        End Get
        Set(ByVal value As String)
            Session("CodigoCaptcha") = value
        End Set
    End Property

    Public Property sCUnica() As String
        Get
            Return Session("CUnica")
        End Get
        Set(ByVal value As String)
            Session("CUnica") = value
        End Set
    End Property

#End Region

    Protected Sub btn_Entrar_Click(sender As Object, e As EventArgs) Handles btn_Entrar.Click


        'Dim nbd As String = fn_Encode("SIAC")
        'Dim nus As String = fn_Encode("SIACDeveloper")
        'Dim npass As String = fn_Encode("5iacDeveloper*1")


        'Dim Servidor As String = fn_Decode("MTkyLjE2OC4wLjcw")
        'Dim Bd As String = fn_Decode("U0lBQw==")
        'Dim Us As String = fn_Decode("U0lBQ05ldA==")
        'Dim Pass As String = fn_Decode("U2lzVGVtYS5TSUFDTG9naW4=")


        Dim ClaveUnica As String = Request.Form("cunica").ToUpper.Trim
        Dim Usuario As String = Request.Form("usuario").ToUpper.Trim
        Dim Contraseña As String = Request.Form("password").Trim

        sCUnica = ClaveUnica



        If ClaveUnica.Trim = "" Then
            fn_Alerta("Capture el RFC de la Empresa.")
            'Call CrearImagen()
            Exit Sub
        End If

        If Usuario.Trim = "" Then
            fn_Alerta("Ingrese su Usuario.")
            'Call CrearImagen()
            Exit Sub
        End If

        If Contraseña.Trim = "" Then
            fn_Alerta("Ingrese su Contraseña.")
            'Call CrearImagen()
            Exit Sub
        End If

        ''''''Comentado 20/10/20
        'If Validar_Captcha() = False Then
        '    fn_Alerta("NO ENVIADO, Valida que eres humano.")
        '    'Call CrearImagen()
        '    Exit Sub
        'End If

        Dim Dt_Usuario As New DataTable
        Dt_Usuario = cn.fn_Login_Validar_Central(Usuario, ClaveUnica)


        If Dt_Usuario Is Nothing Then
            fn_Alerta("Ocurrió un error al consultar la información")
            'Call CrearImagen()
            Exit Sub
        End If

        If Dt_Usuario.Rows.Count = 0 Then
            Dt_Usuario.Dispose()
            fn_Alerta("El Usuario ó el R.F.C. es incorrecto.")
            'Call CrearImagen()
            Exit Sub
        End If

        If ClaveUnica <> Dt_Usuario.Rows(0)("Clave_Unica") Then
            Dt_Usuario.Dispose()
            fn_Alerta("El R.F.C es Incorrecto.")
            'Call CrearImagen()
            Exit Sub
        End If

        Dim hsh_Constraseña = FormsAuthentication.HashPasswordForStoringInConfigFile(Contraseña, "SHA1")

        If hsh_Constraseña <> Dt_Usuario.Rows(0)("Password") Then
            Dt_Usuario.Dispose()
            fn_Alerta("La contraseña  es incorrecta.")
            'Call CrearImagen()
            Exit Sub
        End If

        If Dt_Usuario.Rows(0)("Status") <> "A" Then
            Dt_Usuario.Dispose()
            fn_Alerta("Su usuario ha sido bloqueado.")
            'Call CrearImagen()
            Exit Sub
        End If

        pId_Usuario = Dt_Usuario.Rows(0)("Id_Usuario")
        pTipo_Contacto = Dt_Usuario.Rows(0)("Tipo")
        pNombre = Dt_Usuario.Rows(0)("Nombre_Usuario")
        pNombre_Usuario = Usuario 'Nombre de Sesion 10 caracteres
        pId_Cliente = Dt_Usuario.Rows(0)("Id_Cliente")
        pId_ClienteOriginal = Dt_Usuario.Rows(0)("Id_Cliente")
        pId_ClienteP = Dt_Usuario.Rows(0)("Id_ClienteP")
        pId_Sucursal = Dt_Usuario.Rows(0)("Id_Sucursal")
        pNivel = Dt_Usuario.Rows(0)("Nivel")
        pClave_Corporativo = Dt_Usuario.Rows(0)("Clave_Corporativo")
        pClave_SucursalPropia = Dt_Usuario.Rows(0)("Clave_SucursalPropia")
        pMail_Usuario = Dt_Usuario.Rows(0)("Mail")
        pNombre_Cliente = Dt_Usuario.Rows(0)("Nombre_Cliente")
        pClave_Sucursal = Dt_Usuario.Rows(0)("Clave_Sucursal")
        pCiaTV = "SISSA"
        Session("NumNotifica") = cn.fn_CuentaNotificaciones()

        pExiste_Grupo = "N"
        pId_ClienteGrupo = 0
        Dim ModoConsulta As Byte = Dt_Usuario.Rows(0)("ModoConsulta")

        Dim Cadena() As String = Split(Dt_Usuario.Rows(0)("Cadena"), ",")
        Cadena(0) = BasePage.fn_Decode(Cadena(0))
        Cadena(1) = BasePage.fn_Decode(Cadena(1))
        Cadena(2) = BasePage.fn_Decode(Cadena(2))
        Cadena(3) = BasePage.fn_Decode(Cadena(3))



        'Esta conexionLocal es la que se maneja en el cn_datos y cambiara cuando cambie de sucursal
        Session("ConexionLocal") = "Data Source=" & Cadena(0) & "; Initial Catalog=" & Cadena(1) & ";User ID=" & Cadena(2) & ";Password=" & Cadena(3) & ";"
        Session("NombreConexion") = Dt_Usuario.Rows(0)("Nombre_SucursalPropia")
        Page.Title = Dt_Usuario.Rows(0)("Nombre_SucursalPropia")

        FechaExpiracion = Dt_Usuario.Rows(0)("Fecha_Expira")
        Dt_Usuario.Dispose()
        Dim conexio As String = Session("ConexionLocal")
        Dim name As String = Session("NombreConexion")
        '-----------------------------------------------------
        pId_Login = cn.fn_Crear_Login(pId_Usuario)
        '-----------------------------------------------------

        Call cn.fn_Crear_Log(pId_Login, "INICIO DE SESION")
        Session("RequiereCambioContraseña") = "NO"

        If FechaExpiracion <= Today.Date Then
            'con esta linea no redirige cuando pones iconos en frm_login o en la master
            'pero si entra si cancelas cambio de contraseña
            FormsAuthentication.SetAuthCookie(Usuario, False)
            ContraseñaExpirada = True
            ContraseñaVieja = Contraseña
            Session("RequiereCambioContraseña") = "SI"
            Response.Redirect("CambiarContrasena.aspx")
        Else
            FormsAuthentication.RedirectFromLoginPage(Usuario, False)
        End If

        '--------Obtener el Id_CajaBancaria -------
        pId_CajaBancaria = cn.fn_Login_GetCajaBancaria(pId_Cliente)

        If pId_CajaBancaria = -1 Then
            fn_Alerta("No se pudo obtener el identificador de la Caja Bancaria.")
        End If

        '-------Obtener el Id_Padre -----------
        Dim IdPadre As Integer = cn.fn_Login_GetIdPadre(pId_Cliente, pId_Sucursal)
        If IdPadre = -1 Then
            'verificar si es padre entonces traera un 0, pero
            ' traera un 0 si no hay registro
            fn_Alerta("No se pudo obtener el identificador.")
        End If

        '------Solo cuando el Cliente es PADRE
        If IdPadre = 0 Then IdPadre = pId_Cliente '2791 SISSASENDA

        If pNivel = 1 Then
            '-------Obtener Id_ClienteGrupo ---------
            If ModoConsulta = 2 Then
                '-Verifica si EXISTE Cliente en algun Grupo
                Dim dt_HayGrupo As New DataTable
                dt_HayGrupo = cn.fn_Login_GetHayGrupo(pId_Cliente)

                If dt_HayGrupo.Rows.Count > 0 Then
                    '? Puede estar el cliente en mas de 1 Grupo??
                    Session("ExisteGrupo") = "S"
                    Session("Id_ClienteGrupo") = dt_HayGrupo.Rows(0)("Id_ClienteGrupo")

                    '------Asignarle pId_Cliente el Id_Padre para que pueda filtrarse consulta
                    pId_Cliente = IdPadre
                End If
            End If
        End If
    End Sub

#Region "Funciones Privadas CAPTCHA"

    Private Function Validar_Captcha() As Boolean

        Dim Response As String = Request("g-recaptcha-response") 'Getting Response String Appned to Post Method

        Dim Valid As Boolean = False
        'Request to Google Server

        Dim req As Net.HttpWebRequest = CType(Net.WebRequest.Create(" https://www.google.com/recaptcha/api/siteverify?secret=6LdtDQwUAAAAAFriH9JnNvzyW5OScGaN5MKDIuAz&response=" & Response), Net.HttpWebRequest)
        Try

            'Google recaptcha Responce
            Using wResponse As Net.WebResponse = req.GetResponse()

                Using readStream As New IO.StreamReader(wResponse.GetResponseStream())
                    Dim jsonResponse As String = readStream.ReadToEnd()

                    Dim js As New Script.Serialization.JavaScriptSerializer()
                    Dim data As MyObject = js.Deserialize(Of MyObject)(jsonResponse) ' Deserialize Json

                    Valid = Convert.ToBoolean(data.success)
                End Using
            End Using
            Return Valid
        Catch ex As Exception
            ' msgerror = ex.ToString
            Return False

        End Try
    End Function

    Public Class MyObject
        Public Property success() As String
            Get
                Return m_success
            End Get
            Set(value As String)
                m_success = value
            End Set
        End Property
        Private m_success As String
    End Class

#End Region

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Call CrearImagen()
        End If
    End Sub

#Region "Captcha al vuelo"

    'Protected Sub ibtn_RefrescarCaptcha_Click(sender As Object, e As ImageClickEventArgs) Handles ibtn_RefrescarCaptcha.Click
    '    Call CrearImagen()
    'End Sub


    'Private Sub CrearImagen()

    '    tbxCodigoCaptcha.Text = ""
    '    CodigoCaptcha = ""
    '    CodigoCaptcha = GetRandomText()
    '    'ImgCaptcha.ImageUrl = "~/Login/ImagenCaptcha.aspx?Codigo=" & CodigoCaptcha
    '    ImgCaptcha.ImageUrl = "~/Login/ImagenCaptcha.aspx"

    'End Sub

    Private Function GetRandomText() As String
        Dim Codigo As String = ""
        Dim randomText As New StringBuilder()

        If [String].IsNullOrEmpty(Codigo) Then
            Dim alphabets As String = "abcdefghijklmnopqrstuvwxyz1234567890"
            Dim r As New Random()
            For j As Integer = 0 To 6
                randomText.Append(alphabets(r.[Next](0, alphabets.Length)))
            Next
            Codigo = randomText.ToString()
        End If
        Return Codigo
    End Function

#End Region



End Class