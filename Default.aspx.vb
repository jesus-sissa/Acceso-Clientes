
Partial Public Class _Default
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("RequiereCambioContraseña") = "NO"
    End Sub
End Class