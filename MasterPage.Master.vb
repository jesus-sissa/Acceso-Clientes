Partial Public Class Masterpage
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack Then Exit Sub
        Dim cn As New Cn_Portal(Session, Request)

        If Session("NumNotifica") > 0 Then
            lbl_NumeroNotif.Text = Session("NumNotifica")
            imgBtn_Notifica.Visible = True
            lbl_NumeroNotif.Visible = True
        Else
            imgBtn_Notifica.Visible = False
            lbl_NumeroNotif.Visible = False
        End If

        If Session("RequiereCambioContraseña") = "NO" Then cn.fn_LoadMenu(mnu_Navegacion)
        lbl_Titulopagina.Text = Page.Title.ToUpper
        If Session("NombreConexion") <> Nothing Then
            lbl_Conexion.Text = "CONECTADO A: " & Session("NombreConexion")
        End If
    End Sub

    Protected Sub imgBtn_Notifica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtn_Notifica.Click
        Response.Redirect("~/Soporte/Notificaciones.aspx")
    End Sub
End Class