Imports System.Web.UI

Partial Public Class Descargas
    Inherits BasePage

    Private WriteOnly Property IdDescarga() As Integer
        Set(ByVal value As Integer)
            Session("IdDescarga") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: DESCARGAS")
        Dim Lista As DataTable = cn.fn_Descargas_GetLista()

        pTabla("Lista") = Lista

        gv_Lista.DataSource = fn_MostrarSiempre(Lista)
        gv_Lista.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_documentos.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Lista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        Dim Lista As DataTable = pTabla("Lista")

        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(Lista)
        gv_Lista.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_documentos.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Lista_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_Lista.RowCommand
        Dim Id As Integer = Convert.ToInt32(gv_Lista.DataKeys(e.CommandArgument).Value)

        IdDescarga = Id
        'Response.Redirect("Download.aspx")
        'Abrirlo en otra nueva pestaña
        Dim script As String = "window.open('Download.aspx?sub=1');"
        ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "", script, True)

    End Sub

    Protected Sub gv_Lista_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Lista.SelectedIndexChanged

    End Sub
End Class