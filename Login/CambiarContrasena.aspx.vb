Imports FormsAuthentication = System.Web.Security.FormsAuthentication

Partial Public Class CambiarContrasena
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

#End Region

    Private Sub CambiarContrasena_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If pId_Login = 0 Then Exit Sub

        If ContraseñaExpirada Then
            pass_actual.Disabled = True
            btn_Cancelar.Visible = False
        End If
        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: CAMBIO DE CONTRASEÑA.")

        If Session("RequiereCambioContraseña") = "SI" Then
            pass_actual.Visible = False
            btn_Cancelar.Visible = False
        End If
    End Sub

    Protected Sub btn_Guardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Guardar.Click

        Dim PasswordRealHASH As String = cn.fn_CambiarContraseña_GetPassword()

        If PasswordRealHASH = "" Then
            fn_Alerta("No se puede cambiar la contraseña debido a un error.")
            Exit Sub
        End If

        Dim contra_actual As String = ""
        Dim contra_nueva As String = Request.Form("pass_nuevo").Trim
        Dim contra_confirmar As String = Request.Form("pass_confirmar").Trim

        Dim Contraseña As String

        If ContraseñaExpirada Then
            Contraseña = ContraseñaVieja
        Else
            contra_actual = Request.Form("pass_actual").Trim
            If contra_actual.Trim = "" Then
                fn_Alerta("Capture su contraseña actual.")
                Exit Sub
            End If
            Contraseña = contra_actual
        End If

        If contra_nueva.Trim = "" Then
            fn_Alerta("Capture la nueva contraseña.")
            Exit Sub
        End If
        If contra_confirmar.Trim = "" Then
            fn_Alerta("Capture la contraseña de confirmación.")
            Exit Sub
        End If

        Dim PasswordHASH As String = FormsAuthentication.HashPasswordForStoringInConfigFile(Contraseña, "SHA1")

        If PasswordRealHASH <> PasswordHASH Then
            fn_Alerta("La contraseña es incorrecta.")
            Exit Sub
        End If

        If contra_nueva <> contra_confirmar Then
            fn_Alerta("La confirmación no coincide con la contraseña nueva.")
            Exit Sub
        End If

        Dim PasswordNuevoHASH As String = FormsAuthentication.HashPasswordForStoringInConfigFile(contra_nueva, "SHA1")
        Dim Resultado As String = cn.fn_CambiarContraseña_Guardar(PasswordNuevoHASH)

        If Resultado Then
            Call cn.fn_Crear_Log(pId_Login, "CAMBIO DE CONTRASEÑA CORRECTO.")
            Response.Redirect("~\Default.aspx")
        Else
            fn_Alerta("No se puede cambiar la contraseña debido a un error.")
        End If

    End Sub

    Protected Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
        Response.Redirect("~/Default.aspx")
    End Sub
End Class