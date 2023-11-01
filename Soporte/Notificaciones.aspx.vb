Public Partial Class Notificaciones
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache) '-------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: NOTIFICACIONES")
        Call Llenarlista_Notificaciones()

    End Sub

    Sub Llenarlista_Notificaciones()
        Dim dt_Notificaciones As DataTable = cn.fn_Consulta_Notificaciones()

        If dt_Notificaciones IsNot Nothing AndAlso dt_Notificaciones.Rows.Count > 0 Then
            For Each Notif As DataRow In dt_Notificaciones.Rows

                Dim lbl_Encabezado As New System.Web.UI.WebControls.Label()
                lbl_Encabezado.CssClass = "NotificacionesPortal"
                lbl_Encabezado.Text = Notif.Item("Fecha").ToString & " - " & Notif.Item("Hora").ToString & "- - -" & Notif.Item("Descripcion_Corta").ToString
                PlaceHolderNotifica.Controls.Add(lbl_Encabezado)
                PlaceHolderNotifica.Controls.Add(New LiteralControl("<br />"))
                PlaceHolderNotifica.Controls.Add(New LiteralControl("<br />"))
                '-------------------------------------------------------------------

                Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
                lbl_Seguimiento.CssClass = "NotificacionesPortal_Seguimiento"
                lbl_Seguimiento.Text = Notif.Item("Descripcion_Larga")
                PlaceHolderNotifica.Controls.Add(lbl_Seguimiento)
                PlaceHolderNotifica.Controls.Add(New LiteralControl("<hr />"))
                PlaceHolderNotifica.Controls.Add(New LiteralControl("<br />"))
            Next
        Else
            Dim lbl_Encabezado As New System.Web.UI.WebControls.Label()
            lbl_Encabezado.CssClass = "NotificacionesPortal"
            lbl_Encabezado.Text = Date.Now.ToString & "- - -" & "NOTIFICACIONES"
            PlaceHolderNotifica.Controls.Add(lbl_Encabezado)
            PlaceHolderNotifica.Controls.Add(New LiteralControl("<br />"))
            PlaceHolderNotifica.Controls.Add(New LiteralControl("<br />"))
            '-------------------------------------------------------------------

            Dim lbl_Seguimiento As New System.Web.UI.WebControls.Label()
            lbl_Seguimiento.CssClass = "NotificacionesPortal_Seguimiento"
            lbl_Seguimiento.Text = "DE MOMENTO NO HAY NOTIFICACIONES HASTA NUEVO AVISO."
            PlaceHolderNotifica.Controls.Add(lbl_Seguimiento)
            PlaceHolderNotifica.Controls.Add(New LiteralControl("<hr />"))
            PlaceHolderNotifica.Controls.Add(New LiteralControl("<br />"))
        End If
    End Sub
End Class