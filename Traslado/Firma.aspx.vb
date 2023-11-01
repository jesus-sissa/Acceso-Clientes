Partial Public Class Firma
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim cn As New Cn_Portal(Session, Request)
        Dim IdEmpleado As Integer = Request("Id")
        Dim dt_FirmaEmpleado As DataTable = cn.fn_FotoFirma_Get(IdEmpleado)

        If dt_FirmaEmpleado Is Nothing Then Exit Sub

        If dt_FirmaEmpleado.Rows.Count > 0 Then
            Dim Foto As Byte() = Nothing
            If dt_FirmaEmpleado.Rows(0)("TieneCatalogoF") = "SI" Then
                Foto = dt_FirmaEmpleado.Rows(0)("CatalogoF")
            Else
                Foto = dt_FirmaEmpleado.Rows(0)("Firma")
            End If
            Response.BinaryWrite(Foto)
        End If
    End Sub

End Class