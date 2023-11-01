Public Partial Class FotoUnidad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim cn As New Cn_Portal(Session, Request)
        Dim Id_Unidad As Integer = Request("Id_Unidad")
        Dim dt_FotoUnidad As DataTable = cn.fn_FotoUnidad_Get(Id_Unidad)

        If dt_FotoUnidad Is Nothing Then Exit Sub

        If dt_FotoUnidad.Rows.Count > 0 Then
            Dim Foto As Byte() = dt_FotoUnidad.Rows(0)("FotoF")
            Response.BinaryWrite(Foto)
        End If

    End Sub

End Class