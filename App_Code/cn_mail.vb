Imports System.Data.SqlClient
Imports System.Data
Imports PortalSIAC.cn_Datos
Imports System.IO

Public Class cn_mail
#Region "Variables"
    Dim _asunto As String
    Dim _mensaje As String
    Dim _memory As MemoryStream
    Dim _nombreArchivo As String
    Dim _sucursal As DataTable
    Dim _Host As String
    Dim _MailUser As String
    Dim _MailClave As String
    Dim _MailRemitente As String
    Dim _MailRemitenteNombre As String
    Dim _TimeOut As String
    Dim _Puerto As String
    Dim _destinatarios As DataTable



    Public Property Asunto As String
        Get
            Return _asunto
        End Get
        Set(value As String)
            _asunto = value
        End Set
    End Property

    Public Property Mensaje As String
        Get
            Return _mensaje
        End Get
        Set(value As String)
            _mensaje = value
        End Set
    End Property

    Public Property Memory As MemoryStream
        Get
            Return _memory
        End Get
        Set(value As MemoryStream)
            _memory = value
        End Set
    End Property

    Public Property NombreArchivo As String
        Get
            Return _nombreArchivo
        End Get
        Set(value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property Host As String
        Get
            Return _Host
        End Get
        Set(value As String)
            _Host = value
        End Set
    End Property

    Public Property MailUser As String
        Get
            Return _MailUser
        End Get
        Set(value As String)
            _MailUser = value
        End Set
    End Property

    Public Property MailClave As String
        Get
            Return _MailClave
        End Get
        Set(value As String)
            _MailClave = value
        End Set
    End Property

    Public Property MailRemitente As String
        Get
            Return _MailRemitente
        End Get
        Set(value As String)
            _MailRemitente = value
        End Set
    End Property

    Public Property MailRemitenteNombre As String
        Get
            Return _MailRemitenteNombre
        End Get
        Set(value As String)
            _MailRemitenteNombre = value
        End Set
    End Property

    Public Property TimeOut As String
        Get
            Return _TimeOut
        End Get
        Set(value As String)
            _TimeOut = value
        End Set
    End Property

    Public Property Puerto As String
        Get
            Return _Puerto
        End Get
        Set(value As String)
            _Puerto = value
        End Set
    End Property

    Public Property Destinatarios As DataTable
        Get
            Return _destinatarios
        End Get
        Set(value As DataTable)
            _destinatarios = value
        End Set
    End Property

#End Region

    Public Shared Function fn_Sucursales_Read(ByVal SucursalId As Integer) As DataTable
        Dim Cmd As SqlCommand = cn_Datos.CreaComando("Cat_ParametrosL_Read")
        CreaParametro(Cmd, "@Id_Sucursal", SqlDbType.Int, SucursalId)
        Dim Tbl As DataTable = cn_Datos.EjecutaConsulta(Cmd)
        Cmd.Dispose()
        Return Tbl
    End Function

    Public Shared Function fn_Enviar_Mail(ByVal Destino As String, ByVal Asunto As String, ByVal Texto As String, ByVal Id_Sucursal As Integer, Optional ByVal Adjunto As String = "") As Boolean

        Dim MailServer As String
        Dim MailRemitente As String
        Dim MailRemitenteNombre As String
        Dim MailUser As String
        Dim MailClaveEnc As String
        Dim MailClave As String
        Dim MailPuerto As Integer
        Dim MailTiempoEspera As Integer
        Dim MailUsarSSL As Boolean = False
        Dim Hubo_Destinos As Boolean = False
        Dim SMTP As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim CORREO As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment

        Dim Tabla As DataTable
        Tabla = fn_Sucursales_Read(Id_Sucursal)
        If Tabla Is Nothing Then
            CORREO.Dispose()
            fn_Enviar_Mail = False
            Exit Function
        End If
        If Tabla.Rows.Count = 0 Then
            CORREO.Dispose()
            fn_Enviar_Mail = False
            Exit Function
        End If
        If Tabla.Rows(0)("Mail_Server") = "" Or Tabla.Rows(0)("Mail_User") = "" Or Tabla.Rows(0)("Mail_Clave") = "" Or Tabla.Rows(0)("Mail_Remitente") = "" Or Tabla.Rows(0)("Mail_RemitenteNombre") = "" Then
            CORREO.Dispose()
            fn_Enviar_Mail = False
            Exit Function
        End If
        MailServer = Tabla.Rows(0)("Mail_Server")
        MailUser = Tabla.Rows(0)("Mail_User")
        MailClaveEnc = Tabla.Rows(0)("Mail_Clave")
        MailRemitente = Tabla.Rows(0)("Mail_Remitente")
        MailRemitenteNombre = Tabla.Rows(0)("Mail_RemitenteNombre")
        MailClave = cn_encripta.fn_Decode(MailClaveEnc)
        MailPuerto = Tabla.Rows(0)("Mail_Puerto")
        MailTiempoEspera = Tabla.Rows(0)("Mail_TiempoEspera")

        If Tabla.Rows(0)("Mail_UsarSSL") = "S" Then
            MailUsarSSL = True
        Else
            MailUsarSSL = False
        End If

        'Configuracion del Mensaje
        CORREO.From = New System.Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre, System.Text.Encoding.UTF8)
        Dim Destinos = Split(Destino, ",")
        For Ilocal As Integer = 0 To Destinos.Length - 1
            If BasePage.fn_ValidarMail(Destinos(Ilocal).Trim) Then
                CORREO.[To].Add(Destinos(Ilocal).Trim)
                Hubo_Destinos = True
            End If
        Next Ilocal
        CORREO.Subject = Asunto
        CORREO.IsBodyHtml = False
        CORREO.Body = Texto

        If Adjunto <> "" Then
            CORREO.Attachments.Add(New System.Net.Mail.Attachment(Adjunto))
        End If

        SMTP.Host = MailServer
        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)
        SMTP.EnableSsl = MailUsarSSL

        If MailPuerto > 0 Then
            SMTP.Port = MailPuerto
        End If

        SMTP.Timeout = MailTiempoEspera * 1000

        If Not Hubo_Destinos Then
            fn_Enviar_Mail = False
            CORREO.Dispose()

            Exit Function
        End If

        Try
            SMTP.Send(CORREO)
            fn_Enviar_Mail = True
        Catch ex As System.Net.Mail.SmtpException
            fn_Enviar_Mail = False

        Finally
            CORREO.Dispose()
        End Try
    End Function

    Public Shared Function fn_Enviar_MailFallas(ByVal Asunto As String, ByVal Texto As String, ByVal Adjunto As String, ByVal Id_Sucursal As Integer) As Boolean
        Dim MailServer As String
        Dim MailRemitente As String
        Dim MailRemitenteNombre As String
        Dim MailUser As String
        Dim MailClaveEnc As String
        Dim MailClave As String
        Dim MailPuerto As Integer
        Dim MailTiempoEspera As Integer
        Dim MailUsarSSL As Boolean = False
        Dim Hubo_Destinos As Boolean = False
        Dim Destino As String = ""
        Dim SMTP As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim CORREO As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment

        Dim Tabla As DataTable
        Tabla = fn_Sucursales_Read(Id_Sucursal)
        If Tabla Is Nothing Then
            CORREO.Dispose()
            fn_Enviar_MailFallas = False
            Exit Function
        End If
        If Tabla.Rows.Count = 0 Then
            CORREO.Dispose()
            fn_Enviar_MailFallas = False
            Exit Function
        End If
        If Tabla.Rows(0)("Mail_Server") = "" Or Tabla.Rows(0)("Mail_User") = "" Or Tabla.Rows(0)("Mail_Clave") = "" Or Tabla.Rows(0)("Mail_Remitente") = "" Or Tabla.Rows(0)("Mail_RemitenteNombre") = "" Or Tabla.Rows(0)("Mail_ReporteFallas") = "" Then
            CORREO.Dispose()
            fn_Enviar_MailFallas = False
            Exit Function
        End If
        MailServer = Tabla.Rows(0)("Mail_Server")
        MailUser = Tabla.Rows(0)("Mail_User")
        MailClaveEnc = Tabla.Rows(0)("Mail_Clave")
        MailRemitente = Tabla.Rows(0)("Mail_Remitente")
        MailRemitenteNombre = Tabla.Rows(0)("Mail_RemitenteNombre")
        MailClave = cn_encripta.fn_Decode(MailClaveEnc)
        Destino = Tabla.Rows(0)("Mail_ReporteFallas")
        MailPuerto = Tabla.Rows(0)("Mail_Puerto")
        MailTiempoEspera = Tabla.Rows(0)("Mail_TiempoEspera")
        If Tabla.Rows(0)("Mail_UsarSSL") = "S" Then
            MailUsarSSL = True
        Else
            MailUsarSSL = False
        End If

        'Configuracion del Mensaje
        CORREO.From = New System.Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre, System.Text.Encoding.UTF8)
        'Agregar los Destinatarios uno a uno
        Dim Destinos = Split(Destino, ",")
        For Ilocal As Integer = 0 To Destinos.Length - 1
            If BasePage.fn_ValidarMail(Destinos(Ilocal).Trim) Then
                CORREO.[To].Add(Destinos(Ilocal).Trim)
                Hubo_Destinos = True
            End If
        Next Ilocal
        CORREO.Subject = Asunto
        CORREO.IsBodyHtml = False
        CORREO.Body = Texto
        If Adjunto <> "" Then
            CORREO.Attachments.Add(New System.Net.Mail.Attachment(Adjunto))
        End If

        SMTP.Host = MailServer
        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)
        SMTP.EnableSsl = MailUsarSSL
        If MailPuerto > 0 Then
            SMTP.Port = MailPuerto
        End If
        SMTP.Timeout = MailTiempoEspera * 1000

        'Si no tiene destino se sale sin intentar el envío
        If Not Hubo_Destinos Then
            fn_Enviar_MailFallas = False
            CORREO.Dispose()

            Exit Function
        End If

        Try
            SMTP.Send(CORREO)
            fn_Enviar_MailFallas = True
        Catch ex As System.Net.Mail.SmtpException
            fn_Enviar_MailFallas = False

        Finally
            CORREO.Dispose()
        End Try
    End Function

    Public Shared Function fn_Enviar_MailHTML(ByVal Destino As String, ByVal Asunto As String, ByVal Texto As String, ByVal Adjunto As String, ByVal Id_Sucursal As Integer) As Boolean
        Dim MailServer As String
        Dim MailRemitente As String
        Dim MailRemitenteNombre As String
        Dim MailUser As String
        Dim MailClaveEnc As String
        Dim MailClave As String
        Dim MailPuerto As Integer
        Dim MailTiempoEspera As Integer
        Dim MailUsarSSL As Boolean = False
        Dim Hubo_Destinos As Boolean = False
        Dim SMTP As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim CORREO As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment

        Dim Tabla As DataTable
        Tabla = fn_Sucursales_Read(Id_Sucursal)
        If Tabla Is Nothing Then
            CORREO.Dispose()
            fn_Enviar_MailHTML = False
            Exit Function
        End If
        If Tabla.Rows.Count = 0 Then
            CORREO.Dispose()
            fn_Enviar_MailHTML = False
            Exit Function
        End If
        If Tabla.Rows(0)("Mail_Server") = "" Or Tabla.Rows(0)("Mail_User") = "" Or Tabla.Rows(0)("Mail_Clave") = "" Or Tabla.Rows(0)("Mail_Remitente") = "" Or Tabla.Rows(0)("Mail_RemitenteNombre") = "" Then
            CORREO.Dispose()
            fn_Enviar_MailHTML = False
            Exit Function
        End If
        MailServer = Tabla.Rows(0)("Mail_Server")
        MailUser = Tabla.Rows(0)("Mail_User")
        MailClaveEnc = Tabla.Rows(0)("Mail_Clave")
        MailRemitente = Tabla.Rows(0)("Mail_Remitente")
        MailRemitenteNombre = Tabla.Rows(0)("Mail_RemitenteNombre")
        MailClave = cn_encripta.fn_Decode(MailClaveEnc)
        MailPuerto = Tabla.Rows(0)("Mail_Puerto")
        MailTiempoEspera = Tabla.Rows(0)("Mail_TiempoEspera")
        If Tabla.Rows(0)("Mail_UsarSSL") = "S" Then
            MailUsarSSL = True
        Else
            MailUsarSSL = False
        End If

        'Configuracion del Mensaje
        CORREO.From = New System.Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre, System.Text.Encoding.UTF8)
        Dim Destinos = Split(Destino, ",")
        For Ilocal As Integer = 0 To Destinos.Length - 1
            If BasePage.fn_ValidarMail(Destinos(Ilocal).Trim) Then
                CORREO.[To].Add(Destinos(Ilocal).Trim)
                Hubo_Destinos = True
            End If
        Next Ilocal

        CORREO.Subject = Asunto
        CORREO.IsBodyHtml = True
        CORREO.Body = Texto

        Dim Att As System.Net.Mail.Attachment

        If Adjunto <> "" Then
            Att = New System.Net.Mail.Attachment(Adjunto)
            Att.ContentId = 1
            CORREO.Attachments.Add(Att)
        End If

        SMTP.Host = MailServer
        SMTP.UseDefaultCredentials = False
        SMTP.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)
        SMTP.EnableSsl = MailUsarSSL
        If MailPuerto > 0 Then
            SMTP.Port = MailPuerto
        End If
        SMTP.Timeout = MailTiempoEspera * 1000

        If Not Hubo_Destinos Then
            fn_Enviar_MailHTML = False
            CORREO.Dispose()

            Exit Function
        End If

        Try
            SMTP.Send(CORREO)
            fn_Enviar_MailHTML = True
        Catch ex As System.Net.Mail.SmtpException
            fn_Enviar_MailHTML = False

        Finally
            CORREO.Dispose()
        End Try

    End Function

    Public Function obtenerSucursal(ByVal Id_Sucursal As String) As DataTable
        Try
            Dim cmd As SqlCommand = CreaComando("Cat_ParametrosL_Read")
            CreaParametro(cmd, "@Id_Sucursal", SqlDbType.Decimal, Id_Sucursal)
            Dim dt As DataTable = EjecutaConsulta(cmd)
            If dt.Rows.Count = 0 Then
                dt = Nothing
            End If
            Return dt
        Catch ex As Exception

        End Try
    End Function

    Public Function adjuntarArchivo(ByVal stream As MemoryStream, nombreArchivo As String)
        Me.Memory = stream
        Me.NombreArchivo = nombreArchivo
    End Function

    Public Function Agregardestinatarios(destinatarios As DataTable)
        Me.Destinatarios = destinatarios
    End Function

    Public Function Enviar() As Boolean
        Dim smtp As New System.Net.Mail.SmtpClient 'Variable con la que se envia el correo
        Dim correo As New System.Net.Mail.MailMessage 'Variable que amlmacena los Attachment
        Try
            _sucursal = obtenerSucursal("1")
            Host = _sucursal.Rows(0)("Mail_Server").ToString()
            MailUser = _sucursal.Rows(0)("Mail_User").ToString()
            MailClave = decodifica(_sucursal.Rows(0)("Mail_Clave").ToString())
            MailRemitente = _sucursal.Rows(0)("Mail_Remitente").ToString()
            MailRemitenteNombre = _sucursal.Rows(0)("Mail_RemitenteNombre").ToString()
            TimeOut = Convert.ToInt32(_sucursal.Rows(0)("Mail_TiempoEspera").ToString()) * 1000
            Puerto = Convert.ToInt32(_sucursal.Rows(0)("Mail_Puerto").ToString())

            correo.From = New Net.Mail.MailAddress(MailRemitente, MailRemitenteNombre)
            correo.Subject = Asunto
            correo.Body = Mensaje
            correo.IsBodyHtml = True

            Dim attachment As New Net.Mail.Attachment(Memory, NombreArchivo)
            correo.Attachments.Add(attachment)

            smtp.Host = Host
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New System.Net.NetworkCredential(MailUser, MailClave)
            smtp.EnableSsl = _sucursal.Rows(0)("Mail_UsarSSL") = "S"

            If Puerto > 0 Then
                smtp.Port = Puerto
            End If

            smtp.Timeout = TimeOut

            For Each destino As DataRow In Destinatarios.Rows
                If destino("Mail").ToString <> "" Then
                    correo.To.Add(destino("Mail").ToString)
                End If
            Next

            smtp.Send(correo)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Function decodifica(valor As String) As String
        Dim byteValor As Byte() = System.Convert.FromBase64String(valor)
        Return System.Text.Encoding.UTF8.GetString(byteValor)
    End Function

    Private Function codificar(valor As String) As String
        Dim byteValor As Byte() = System.Text.Encoding.UTF8.GetBytes(valor)
        Return System.Convert.ToBase64String(byteValor)
    End Function

    '    Public Static String decodificar(String valor) {
    '        Byte[] byteValor = System.Convert.FromBase64String(valor);
    '        Return System.Text.Encoding.UTF8.GetString(byteValor);
    '    }

    '    Public Static String codificar(String valor)
    '    {
    '        Byte[] byteValor = System.Text.Encoding.UTF8.GetBytes(valor);
    '        Return System.Convert.ToBase64String(byteValor);
    '    }
    'Public bool enviar(Correo model)
    '    {
    '        System.Net.Mail.SmtpClient  smtp = New System.Net.Mail.SmtpClient();
    '        System.Net.Mail.MailMessage correo = New System.Net.Mail.MailMessage();

    '        Try
    '        {

    '            //Configuracion del Mensaje
    '            correo.From = New System.Net.Mail.MailAddress(_sucursal.Sucursal.MailRemitente, _sucursal.Sucursal.MailRemitenteNombre);
    '            correo.Subject = model.Asunto;
    '            correo.Body = model.Mensaje;
    '            correo.IsBodyHtml = EsHTML;

    '            If (adjuntaArchivo) {
    '                Attachment attachment = New Attachment(_memory, _nombreArchivo);
    '                correo.Attachments.Add(attachment);
    '            }

    '            smtp.Host = _sucursal.Sucursal.Host;
    '            smtp.UseDefaultCredentials = false;
    '            smtp.Credentials = New System.Net.NetworkCredential(_sucursal.Sucursal.MailUser, _sucursal.Sucursal.MailClave);
    '            smtp.EnableSsl = _sucursal.usaSSL();

    '            If (_sucursal.Sucursal.Puerto > 0)
    '                smtp.Port = _sucursal.Sucursal.Puerto;

    '            smtp.Timeout = _sucursal.Sucursal.TimeOut;


    '            foreach (DataRow destino in _destinatarios.Rows)
    '            {
    '                If (destino["Mail"].ToString() != "")
    '                correo.To.Add(destino["Mail"].ToString());
    '            }

    '            smtp.Send(correo);
    '        }
    '        Catch (Exception ex)
    '        {
    '            correo.Dispose();
    '            If (adjuntaArchivo)
    '            {
    '                _memory.Close();
    '            }
    '            Throw ex;
    '        }
    '        correo.Dispose();
    '        smtp.Dispose();
    '        If (adjuntaArchivo)
    '        {
    '            _memory.Close();
    '        }
    '        Return True;
    '    }


End Class
