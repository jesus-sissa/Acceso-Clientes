Imports System.Drawing
Imports System.IO

Partial Public Class Frente
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim cn As New Cn_Portal(Session, Request)
        Dim Id_Cheque As Integer = Request("Id")
        Dim dt_FrenteCheque As DataTable = cn.fn_Cheque_Get(Id_Cheque)
        If dt_FrenteCheque Is Nothing Then Exit Sub

        If dt_FrenteCheque.Rows.Count > 0 Then
            Dim str_tiff As New MemoryStream(dt_FrenteCheque.Rows(0)("Front1"), False)
            Dim bmp As New Bitmap(str_tiff)
            Dim str_jpg As New MemoryStream()
            bmp.Save(str_jpg, System.Drawing.Imaging.ImageFormat.Jpeg)

            Dim Foto As Byte() = str_jpg.GetBuffer()
            Response.ContentType = "image/jpeg"
            Response.BinaryWrite(Foto)
        End If
    End Sub

End Class