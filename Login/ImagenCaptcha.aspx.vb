Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Drawing

Public Class ImagenCaptcha
    Inherits System.Web.UI.Page
    Private code As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Clear()
        Dim ImagenCodigo As Byte() = Nothing
        'code = Request("Codigo")
        code = Session("CodigoCaptcha")
        Call CreateImage()
    End Sub

#Region "Captcha al vuelo"


    Private Sub CreateImage()
        Dim bitmap As New Bitmap(200, 50, PixelFormat.Format32bppArgb)
        Dim g As Graphics = Graphics.FromImage(bitmap)
        Dim pen As New Pen(Color.Yellow)
        Dim rect As New Rectangle(0, 0, 200, 50)
        Dim ColorFondo As New SolidBrush(Color.Gold)
        Dim ColorLetra As New SolidBrush(Color.Black)
        Dim counter As Integer = 0
        Dim rand As New Random()
        Dim X As Integer = 10
        Dim Y As Integer = 0
        Dim Size As Integer = 10
        Dim Fuente = New Font("Courier New", Size)

        Dim ColoresFondo(4) As System.Drawing.Color
        ColoresFondo(0) = Color.Gold
        ColoresFondo(1) = Color.Pink
        ColoresFondo(2) = Color.Gray
        ColoresFondo(3) = Color.LightSalmon

        Dim ColoresLetras(8) As System.Drawing.Color
        ColoresLetras(0) = Color.Black
        ColoresLetras(1) = Color.DarkBlue
        ColoresLetras(2) = Color.Blue
        ColoresLetras(3) = Color.Green
        ColoresLetras(4) = Color.Olive
        ColoresLetras(5) = Color.RoyalBlue
        ColoresLetras(6) = Color.OrangeRed
        ColoresLetras(7) = Color.Brown

        Dim NombresFuentes(5) As String
        NombresFuentes(0) = "Courier New"
        NombresFuentes(1) = "Arial"
        NombresFuentes(2) = "Verdana"
        NombresFuentes(3) = "Times New Roman"
        NombresFuentes(4) = "Calibri"

        g.DrawRectangle(pen, rect)
        ColorFondo.Color = ColoresFondo(rand.[Next](0, 4))
        g.FillRectangle(ColorFondo, rect)

        For i As Integer = 0 To code.Length - 1
            ColorLetra = New SolidBrush(ColoresLetras(rand.[Next](0, 7)))
            X += rand.[Next](15, 25)
            Y = rand.[Next](6, 10)
            Size = 12 + rand.[Next](10, 17)
            Fuente = New Font(NombresFuentes(rand.[Next](0, 4)), Size)
            g.DrawString(code(i).ToString(), Fuente, ColorLetra, New PointF(X, Y))
        Next


        Dim ColoresLineas(5) As System.Drawing.Color
        ColoresLineas(0) = Color.Aqua
        ColoresLineas(1) = Color.Beige
        ColoresLineas(2) = Color.Blue
        ColoresLineas(3) = Color.Green
        ColoresLineas(4) = Color.Lavender
        Dim Brush As New SolidBrush(ColoresLineas(rand.[Next](0, 4)))
        Dim Pluma As Pen = New Pen(Brush, 1)
        Dim Puntos As Point() = {New Point(rand.[Next](0, 150), rand.[Next](0, 50)), New Point(rand.[Next](20, 150), rand.[Next](10, 50))}

        Pluma.Brush = Brush

        'Una linea Fija a todo lo ancho
        Pluma.Color = ColoresLineas(rand.[Next](0, 4))
        Puntos = {New Point(0, 25), New Point(200, 25)}
        g.DrawRectangle(Pluma, 1, 1, 198, 48)
        g.DrawLines(Pluma, Puntos)

        '4 Lineas aleatorias
        Pluma.Color = ColoresLineas(rand.[Next](0, 4))
        Puntos = {New Point(rand.[Next](0, 150), rand.[Next](0, 50)), New Point(rand.[Next](20, 150), rand.[Next](10, 50))}
        g.DrawLines(Pluma, Puntos)

        Pluma.Color = ColoresLineas(rand.[Next](0, 4))
        Puntos = {New Point(rand.[Next](0, 150), rand.[Next](0, 50)), New Point(rand.[Next](20, 150), rand.[Next](10, 50))}
        g.DrawLines(Pluma, Puntos)

        Pluma.Color = ColoresLineas(rand.[Next](0, 4))
        Puntos = {New Point(rand.[Next](0, 150), rand.[Next](0, 50)), New Point(rand.[Next](20, 150), rand.[Next](10, 50))}
        g.DrawLines(Pluma, Puntos)

        Pluma.Color = ColoresLineas(rand.[Next](0, 4))
        Puntos = {New Point(rand.[Next](0, 150), rand.[Next](0, 50)), New Point(rand.[Next](20, 150), rand.[Next](10, 50))}
        g.DrawLines(Pluma, Puntos)


        Dim BytesImagen As Byte() = Nothing
        Dim ms As New MemoryStream
        bitmap.Save(ms, Imaging.ImageFormat.Jpeg)
        BytesImagen = ms.GetBuffer()

        Response.BinaryWrite(BytesImagen)

        g.Dispose()
        bitmap.Dispose()
    End Sub

#End Region

End Class