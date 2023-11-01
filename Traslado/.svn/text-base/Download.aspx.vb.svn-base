Imports System.IO
Imports System.Drawing

Partial Public Class Download
    Inherits System.Web.UI.Page

    Private ReadOnly Property IdDescarga() As Integer
        Get
            Return Session("IdDescarga")
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim cn As New Cn_Portal(Session, Request)

        Dim Bytes As Byte() = cn.fn_Descargas_GetBytes(IdDescarga)

        If Bytes IsNot Nothing Then
            Response.ContentType = "application/pdf"
            Response.BinaryWrite(Bytes)
        End If
    End Sub

End Class