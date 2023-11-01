Imports System.Data.SqlClient
Imports System.Data

Public Class cn_Datos
    Public Shared Function CreaConexion() As SqlConnection
        Return New SqlConnection(HttpContext.Current.Session("ConexionLocal"))
        'Return New SqlConnection(HttpContext.Current.Session("Data Source=DVL-MTY-V01; Initial Catalog=CTRSIAC; User ID=SIACDEVELOPER; Password=5iacDeveloper*1")) '----
    End Function

    Public Shared Function CreaTransaccion() As SqlTransaction
        Dim cn As SqlConnection = CreaConexion()
        cn.Open()
        Return cn.BeginTransaction
    End Function

    Public Shared Function CreaTransaccion(ByVal Conexion As SqlConnection) As SqlTransaction
        Conexion.Open()
        Dim tr As SqlTransaction = Conexion.BeginTransaction
        Return tr
    End Function

    Public Shared Function CreaComando(ByVal Procedimiento As String) As SqlCommand
        Dim cmd As New SqlCommand(Procedimiento, CreaConexion())
        cmd.CommandType = CommandType.StoredProcedure
        Return cmd
    End Function

    Public Shared Function Crea_Comando(ByVal Consulta As String, ByVal Tipo As CommandType, ByVal cone As SqlConnection) As SqlCommand
        Dim Com As SqlCommand = New SqlCommand(Consulta, cone)
        Com.CommandType = Tipo
        Return Com
    End Function

    Public Shared Function CreaComando(ByVal Conexion As SqlConnection, ByVal Procedimiento As String) As SqlCommand
        Dim cmd As New SqlCommand(Procedimiento, Conexion)
        cmd.CommandType = CommandType.StoredProcedure
        Return cmd
    End Function

    Public Shared Function CreaComando(ByVal Transaccion As SqlTransaction, ByVal Procedimiento As String) As SqlCommand
        Dim cmd As New SqlCommand(Procedimiento, Transaccion.Connection, Transaccion)
        cmd.CommandType = CommandType.StoredProcedure
        Return cmd
    End Function

    Public Shared Function CreaParametro(ByVal Nombre As String, ByVal Tipo As DbType, ByVal Valor As Object) As SqlParameter
        Dim prm As New SqlParameter(Nombre, Tipo)
        prm.Value = Valor
        Return prm
    End Function

    Public Shared Function CreaParametro(ByVal Nombre As String, ByVal Valor As Object)
        Return New SqlParameter(Nombre, Valor)
    End Function

    Public Shared Sub CreaParametro(ByRef cmd As SqlCommand, ByVal Nombre As String, ByVal Valor As Object)
        cmd.Parameters.Add(CreaParametro(Nombre, Valor))
    End Sub

    Public Shared Function CreaParametro(ByRef cm As SqlCommand, ByVal Nombre As String, ByVal Tipo As SqlDbType, ByVal Valor As Object, Optional ByVal Mayusculas As Boolean = True) As SqlParameter
        Dim Para As New SqlParameter()
        Para = cm.CreateParameter()
        Para.ParameterName = Nombre
        Para.SqlDbType = Tipo

        Select Case Tipo
            Case SqlDbType.Int
                Para.Value = Integer.Parse(Valor.ToString())
            Case SqlDbType.Decimal
                Para.Value = Decimal.Parse(Valor.ToString())
            Case SqlDbType.BigInt
                Para.Value = Long.Parse(Valor.ToString())
            Case SqlDbType.VarChar
                If Mayusculas Then
                    Para.Value = Valor.ToString().Trim.ToUpper()
                Else
                    Para.Value = Valor.ToString().Trim
                End If
            Case SqlDbType.Text
                If Mayusculas Then
                    Para.Value = Valor.ToString().Trim.ToUpper()
                Else
                    Para.Value = Valor.ToString().Trim
                End If
            Case SqlDbType.Money
                Para.Value = Double.Parse(Valor.ToString())
            Case SqlDbType.DateTime
                Para.Value = CDate(Valor.ToString())
            Case SqlDbType.Date
                Para.Value = CDate(Valor.ToString())
            Case SqlDbType.Time
                Para.Value = Valor.ToString.Trim
            Case SqlDbType.Image
                Para.Value = Valor
            Case Else
                Para.Value = Valor
        End Select
        cm.Parameters.Add(Para)
        Return Para
    End Function

    Public Shared Function EjecutaConsulta(ByRef cmd As SqlCommand) As DataTable
        Dim tbl As New DataTable

        If cmd.Connection.State = ConnectionState.Open Then
            tbl.Load(cmd.ExecuteReader())
        Else
            cmd.Connection.Open()
            tbl.Load(cmd.ExecuteReader())
            cmd.Connection.Close()
        End If
        Return tbl
    End Function

    Public Shared Function EjecutaDataSet(ByRef cmd As SqlCommand) As DataSet

        Dim ds As New DataSet

        If cmd.Connection.State = ConnectionState.Open Then
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
        Else
            cmd.Connection.Open()
            Dim da As New SqlDataAdapter(cmd)
            da.Fill(ds)
            cmd.Connection.Close()
        End If

        Return ds
    End Function

    Public Shared Function EjecutaScalar(ByVal cmd As SqlCommand) As Object
        Dim Res As Object

        If cmd.Connection.State = ConnectionState.Open Then
            Res = cmd.ExecuteScalar()
        Else
            cmd.Connection.Open()
            Res = cmd.ExecuteScalar()
            cmd.Connection.Close()
        End If

        Return Res
    End Function
    Public Shared Function EjecutarScalarT(ByVal command As SqlCommand) As Integer
        Dim Identidad As Object = ""
        Try
            Identidad = command.ExecuteScalar()
        Catch ex As Exception
            Throw ex
            'command.Connection.Close();
        Finally
        End Try
        Return Convert.ToInt32(Identidad)
    End Function

    Public Shared Function EjecutaNonQuery(ByVal cmd As SqlCommand) As Integer

        Dim res As Integer

        If cmd.Connection.State = ConnectionState.Open Then
            res = cmd.ExecuteNonQuery()
        Else
            cmd.Connection.Open()
            res = cmd.ExecuteNonQuery()
            cmd.Connection.Close()
        End If

        Return res

    End Function
    Public Shared Function EjecutarNonQueryT(ByVal command As SqlCommand) As Integer
        Dim renglonesAfectados As Integer = -1
        Try
            renglonesAfectados = command.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
            'command.Connection.Close();
        Finally

        End Try
        Return renglonesAfectados
    End Function

End Class
