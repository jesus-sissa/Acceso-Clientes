Public Partial Class Foto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()

        Dim cn As New Cn_Portal(Session, Request)
        Dim IdEmpleado As Integer = Request("Id")

        If IdEmpleado = 640 Then
            MsgBox("")
        End If
        Dim dt_FotoEmpleado As DataTable = cn.fn_FotoFirma_Get(IdEmpleado)

        If dt_FotoEmpleado Is Nothing Then Exit Sub

        If dt_FotoEmpleado.Rows.Count > 0 Then
            Dim Foto As Byte() = Nothing

            If dt_FotoEmpleado.Rows(0)("TieneCatalogo") = "SI" Then
                Foto = dt_FotoEmpleado.Rows(0)("Catalogo")
            Else
                Foto = dt_FotoEmpleado.Rows(0)("Frente")
            End If
            Response.BinaryWrite(Foto)

        End If
    End Sub

End Class