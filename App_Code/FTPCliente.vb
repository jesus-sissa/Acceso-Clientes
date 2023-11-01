Imports System.IO
Imports System.Net

Public Class FTPCliente
    Dim ftpRequest As FtpWebRequest
    Dim ftpResponse As FtpWebResponse
    Dim ftpStream As Stream
    Dim bufferSize As Byte = 255
    Dim host As String = "ftp://192.168.1.181/"
    ''Dim host As String = "ftp://mtystsb1-p11/"

    Dim user As String = "sissa-ftp"
    Dim pass As String = "Pass.010"
    Public Function directoryList() As String()
        Try
            ftpRequest = FtpWebRequest.Create(host)
            ftpRequest.Credentials = New NetworkCredential(user, pass)
            ftpRequest.UseBinary = True
            ftpRequest.UsePassive = True
            ftpRequest.KeepAlive = True
            ftpRequest.Method = "NLST"
            ftpResponse = ftpRequest.GetResponse()
            ftpStream = ftpResponse.GetResponseStream()
            Dim reader As StreamReader = New StreamReader(ftpStream)
            Dim Str As String = Nothing
            Try
                While True
                    If reader.Peek() = -1 Then
                        Exit While
                    End If
                    If reader.ReadLine() <> "" Then
                        Str = Str + reader.ReadLine() + "|"
                    End If

                End While
            Catch ex As Exception

            End Try
            reader.Close()
            ftpStream.Close()
            ftpResponse.Close()
            ftpRequest = Nothing
            Try
                Return Str.Split("|".ToArray())
            Catch ex As Exception

            End Try

        Catch ex As Exception

        End Try
        Return New String() {""}
    End Function

    ''' <summary>
    ''' Recibe  Se esperan 2 parametros |
    ''' remote: direccion donde esta el archivo remoto
    ''' local: direccion donde se guardara el archivo |
    ''' Devuelve 
    ''' devuelve True si de descargo el archivo |
    ''' </summary>
    ''' <param name="remote"></param>
    ''' <param name="local"></param>
    ''' <returns></returns>
    Public Function downLoad(remote As String, local As String) As Boolean
        Dim downloaded As Boolean = False
        Try
            ftpRequest = FtpWebRequest.Create(host + remote)
            ftpRequest.Credentials = New NetworkCredential(user, pass)
            ftpRequest.UseBinary = True
            ftpRequest.UsePassive = True
            ftpRequest.KeepAlive = True
            ftpRequest.Method = "RETR"
            ftpResponse = ftpRequest.GetResponse()
            ftpStream = ftpResponse.GetResponseStream()
            Dim stream As FileStream = New FileStream(local, FileMode.Create)
            Dim Buffer As Byte() = New Byte(bufferSize) {}
            Dim count As Integer = ftpStream.Read(Buffer, 0, bufferSize)
            Try
                While True
                    If count <= 0 Then
                        Exit While
                    End If
                    stream.Write(Buffer, 0, count)
                    count = ftpStream.Read(Buffer, 0, bufferSize)
                End While
                downloaded = True
            Catch ex As Exception

            End Try
            stream.Close()
            ftpStream.Close()
            ftpResponse.Close()
            ftpRequest = Nothing
        Catch ex As Exception

        End Try
        Return downloaded
    End Function


End Class
