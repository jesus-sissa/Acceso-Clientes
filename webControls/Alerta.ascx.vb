Public Partial Class Alerta

    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btn_Cerrar.Focus()
    End Sub

    Public Sub Alerta(ByVal Mensaje As String, Tema As String)
        Select Case Tema
            Case "DORADO"
                ' div_mensaje.Style.Add("background-color", "#f5f0e7")
                div_tituloAlerta.Style.Add("background-color", "#867044")
            Case "AZUL"
                ' div_mensaje.Style.Add("background-color", "#eaeff4")
                div_tituloAlerta.Style.Add("background-color", "#104e8b")
            Case "GUINDA"
                'div_mensaje.Style.Add("background-color", "#f2ecec")
                div_tituloAlerta.Style.Add("background-color", "#6f3232")
            Case "VERDE"
                ' div_mensaje.Style.Add("background-color", "#e7f4ec")
                div_tituloAlerta.Style.Add("background-color", "#268968")
            Case Else
                ' div_mensaje.Style.Add("background-color", "#f5f0e7")
                div_tituloAlerta.Style.Add("background-color", "#000000")
        End Select

        lbl_Mensaje.Text = Mensaje
        mpeModalPopupExtender.Show()
        btn_Cerrar.Focus()
    End Sub

    Protected Sub btn_Cerrar_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs) Handles btn_Cerrar.Click
        mpeModalPopupExtender.Hide()
    End Sub
End Class