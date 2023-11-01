Public Partial Class Cheques
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim id As Integer = Request("Id")

        Dim Frente As String = "~\Proceso\Frente.aspx?Id=" & id
        Dim Reverso As String = "~\Proceso\Reverso.aspx?Id=" & id

        img_Frente.ImageUrl = Frente
        img_Reverso.ImageUrl = Reverso
    End Sub

End Class