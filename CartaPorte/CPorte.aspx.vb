Imports System
Imports System.Collections.Generic
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.IO.Compression
Imports System.Net
Imports PortalSIAC.FTPCliente

Public Class _CPorte
    Inherits BasePage

    Private dt As New DataTable
    Private visorCP_Path = Server.MapPath("~") + "\CartaPorte\VisorCP\"
    Private FacturasAComprimir = Server.MapPath("~") + "\CartaPorte\FacturasAComprimir\"
    Private Zips_Path = Server.MapPath("~") + "\CartaPorte\Zips\"
    Private ftp_Path = "Comandas/ConExito/"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim _cliente As String = Session("CUnica") + "/"
        Dim archiveName As String = String.Format(_cliente.Replace("/", "") + "-{0}.zip",
                DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))

        If Not IsPostBack Then

            RFC.Text = Session("CUnica")
            FOLIO.Text = ""
        End If
        ''revisar si existe el directoria Zips, si no existe crea el directorio
        If Not Directory.Exists(Zips_Path) Then
            Directory.CreateDirectory(Zips_Path)
        End If
        'si hay archivo con el nombre y fecha del dia eliminalo
        If File.Exists(Zips_Path + archiveName) Then
            File.Delete(Zips_Path + archiveName)
        End If
        ''revisar si existe el directoria FacturasAComprimir, si no existe crea el directorio
        If Not Directory.Exists(FacturasAComprimir) Then
            Directory.CreateDirectory(FacturasAComprimir)
        End If
        'revisar si se existe directorio para cliente, si no crea uno
        If Not Directory.Exists(FacturasAComprimir + _cliente) Then
            Directory.CreateDirectory(FacturasAComprimir + _cliente)
        End If
        'revisar si existe el directoria VisorCP, si no existe crea el directorio
        If Not Directory.Exists(visorCP_Path) Then
            Directory.CreateDirectory(visorCP_Path)
        End If
        'eliminar pdfs relacionados a este cliente en el servidor
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(visorCP_Path)
            If foundFile.Contains(RFC.Text) Then
                File.Delete(foundFile)
            End If
        Next
        'eliminar archivos zip relacionados a este cliente en el servidor
        For Each foundFile As String In My.Computer.FileSystem.GetFiles(Zips_Path)
            If foundFile.Contains(RFC.Text) Then
                File.Delete(foundFile)
            End If
        Next

    End Sub
    Protected Sub CheckBox2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim showCheckBox As CheckBox = CType(sender, CheckBox)
        If showCheckBox.Checked Then
            SelectAllCheckBox(True)
        Else
            SelectAllCheckBox(False)
        End If
    End Sub

    Protected Sub SelectAllCheckBox(ByVal show As Boolean)
        Dim i As Integer
        Dim row As GridViewRow
        Dim ch As CheckBox
        For i = 0 To GridView1.Rows.Count - 1
            row = GridView1.Rows(i)
            If i = 5 Then Exit For
            If row.RowType = DataControlRowType.DataRow Then
                ch = row.FindControl("CheckBox1") 'localiza el control checkbox
                ch.Checked = show
            End If
        Next
        ' btnVisualizar.Enabled = Not btnVisualizar.Enabled
    End Sub

    Private Sub Consulta(pRfc As String, pFolio As String, pFechaI As String, pFechaF As String)

        Using conn As New SqlConnection("Data Source=SISSASQL;Initial Catalog=SISSAWEB; User ID=SiacNet; Password=SisTema.SIACLogin;")

            Dim cmd As New SqlCommand("GetControl_CartaPorte_Rfcn", conn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("@Rfc", pRfc)  '"GDD13092615"
            cmd.Parameters.AddWithValue("@Folio", pFolio)
            cmd.Parameters.AddWithValue("@FechaI", pFechaI)
            cmd.Parameters.AddWithValue("@FechaF", pFechaF)

            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
        End Using
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' GridView1.Columns(4).Visible = True
        Dim FI As String
        Dim FF As String
        FI = Request.Form("datepickerI")
        FF = Request.Form("datepickerF")

        If RFC.Text.Length = 0 Then
            'Dim myScript As String = "window.alert('Favor de capturar el RFC');"
            'ClientScript.RegisterStartupScript(Me.GetType(), "myScript", myScript, True)
            fn_Alerta("Favor de capturar el RFC")
            Exit Sub
        End If
        Consulta(RFC.Text, FOLIO.Text, FI, FF)
        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            GridView1.DataSource = dt
            GridView1.DataBind()
            btnDescargar.Visible = True
            btnVisualizar.Visible = True
            btnExcel.Visible = True
            For Each pdfexist As DataRow In dt.Rows
                Dim pdf = pdfexist("Directorio")
                If File.Exists(visorCP_Path + pdf) Then
                    File.Delete(visorCP_Path + pdf)
                End If
            Next
        End If

    End Sub



    Protected Sub btnDescargar_Click(sender As Object, e As EventArgs) Handles btnDescargar.Click

        Dim v_Path As String
        Dim V_File As String
        Dim V_XML As String
        Dim files As String()
        Dim _cliente As String = Session("CUnica") + "/"

        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                If chkRow.Checked Then
                    v_Path = row.Cells(4).Text
                    V_File = Path.GetFileName(v_Path)
                    'comprueba si existe el pdf dentro del directorio del cliente, si existe eliminalo
                    If File.Exists(FacturasAComprimir + _cliente + V_File) Then
                        File.Delete(FacturasAComprimir + _cliente + V_File)
                    End If
                    Try
                        'crea conexion a ftp
                        Dim ftp As New FTPCliente
                        'descarga pdf del ftp
                        ftp.downLoad(ftp_Path + V_File, FacturasAComprimir + _cliente + V_File)
                        'cambia la extencion pdf por xml
                        V_XML = Replace(V_File.ToUpper, ".PDF", ".XML")
                        'descarga xml del ftp
                        ftp.downLoad(ftp_Path + V_XML, FacturasAComprimir + _cliente + V_XML)
                    Catch ex As Exception
                    End Try

                End If
            End If
        Next

        Dim archiveName As String = String.Format(_cliente.Replace("/", "") + "-{0}.zip",
                DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))

        archiveName = String.Format(_cliente.Replace("/", "") + "-{0}.zip",
                DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))

        'eliminar zip despues de descargarlo
        If File.Exists(Zips_Path + archiveName) Then
            File.Delete(Zips_Path + archiveName)
        End If
        'crea archivo Zip. comprime todo lo que este dentro del directorio en la ruta FacturasAComprimir + _cliente
        'y lo introduce dentro del directorio en la ruta Zips_Path + archiveName
        ZipFile.CreateFromDirectory(FacturasAComprimir + _cliente, Zips_Path + archiveName)
        'espera 1 min
        Threading.Thread.Sleep(1000)
        'elimina los archivos dentro de facturasAComprimir + _cliente
        files = Directory.GetFiles(FacturasAComprimir + _cliente)
        For Each file As String In files
            System.IO.File.Delete(file)
        Next
        'eliminamos la carpeta del cliente
        Directory.Delete(FacturasAComprimir + _cliente)

        'se descarga el archivo zip en el navegador
        Response.Clear()
        Response.ContentType = "application/zip"
        Response.AddHeader("content-disposition", "filename= " + archiveName)
        HttpContext.Current.Response.WriteFile(Zips_Path + archiveName)
        Response.End()


    End Sub

    Protected Sub btnVisualizar_Click(sender As Object, e As EventArgs) Handles btnVisualizar.Click

        Dim pathToPDF As String
        Dim v_Path As String = ""
        Dim URL As String = ""
        Dim count As Integer = 0
        'revisa cuantos registro fueron seleccionados
        For Each row As GridViewRow In GridView1.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim chkRow As CheckBox = TryCast(row.Cells(0).FindControl("CheckBox1"), CheckBox)
                If chkRow.Checked Then
                    'se extrae el nombre del pdf
                    v_Path = row.Cells(4).Text
                    'revisa si hay archivos pdf en caso de contrar alguno los elimina
                    If File.Exists(visorCP_Path + v_Path) Then
                        File.Delete(visorCP_Path + v_Path)
                    End If

                    If v_Path.Length > 0 Then
                        Try
                            'conectamos a ftp
                            Dim ftp As New FTPCliente
                            'extraemos el nombre y extension del archivo
                            Dim V_File As String = Path.GetFileName(v_Path)
                            'si se dercargo el archivo se concatena a URL, en caso contrario se concatena un mensaje de error
                            If ftp.downLoad(ftp_Path + V_File, visorCP_Path + V_File) Then
                                pathToPDF = "\VisorCP/" + V_File

                                URL = URL + "window.open('" + pathToPDF + "','_blank');"
                                count += 1
                            Else
                                URL = "alert('No es posible visualizar el archivo')"
                                ClientScript.RegisterStartupScript(Me.GetType(), "myScript", URL, True)

                            End If

                        Catch ex As Exception
                            ClientScript.RegisterStartupScript(Me.GetType(), "myScript", "alert(" + ex.ToString() + ")", True)

                        End Try


                    End If

                End If
            End If
            If count > 5 Then
                Exit For
            End If
        Next
        'abrir los pdf selecionaods
        ClientScript.RegisterStartupScript(Me.GetType(), "myScript", URL, True)

    End Sub

    Protected Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        Dim _cliente As String = Session("CUnica")
        Dim archiveName As String = String.Format(_cliente + "-{0}.xls",
               DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"))

        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        GridView1.Columns(0).Visible = False
        Me.GridView1.EnableViewState = False
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        pagina.Controls.Add(form)
        form.Controls.Add(Me.GridView1)
        pagina.RenderControl(htw)
        GridView1.Columns(0).Visible = True
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" + archiveName)

        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()


    End Sub

    Protected Sub RFC_TextChanged(sender As Object, e As EventArgs) Handles RFC.TextChanged

    End Sub
End Class