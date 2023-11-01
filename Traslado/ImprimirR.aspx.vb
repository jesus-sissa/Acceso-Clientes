Imports PortalSIAC.cn_Datos
Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing
Imports Gma.QrCodeNet.Encoding
Imports Gma.QrCodeNet.Encoding.Windows.Render
Imports System.Drawing.Imaging


Public Class ImprimirR
    'Inherits System.Web.UI.Page
    Inherits BasePage
    Dim dt As DataTable
    Dim qrmsg As String
    Dim Id_Venta As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadC()
    End Sub
    Sub LoadC()
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->'''
        If IsPostBack Then Exit Sub
        If (Tipo_Remision = "MAT") Then
            Llenar_Remision1()
        ElseIf Tipo_Remision = "TRAS" Then
            Llenar_Remision2(0)
        ElseIf (Tipo_Remision = "DOT") Then
            Llenar_Remision3()
        ElseIf (Tipo_Remision = "ATMD") Then
            Llenar_Remision4()
        ElseIf (Tipo_Remision = "ATMF") Then
            Llenar_Remision5()
        ElseIf (Tipo_Remision = "ATMC") Then
            Llenar_Remision6()
        End If
    End Sub
    Public Sub btn_next(Total As Integer)
        Dim contador As Integer = 0
        contador += 1
        If contador <= Total Then
        End If
        Llenar_Remision2(contador)
    End Sub
    Public Sub Llenar_Remision1()
        If Data_Remision IsNot Nothing AndAlso Data_Remision.Rows.Count > 0 Then
            dt = Data_Remision
        Else
            dt = cn.fn_ConsultaR1(CInt(Num_Remision))
        End If
        Try
            remision_n.Text = dt.Rows(0)("Numero_Remision").ToString()
            num_cliente.Text = dt.Rows(0)("Clave_Cliente").ToString
            fecha_va.Text = dt.Rows(0)("Fecha_Valida").ToString
            envases_total.Text = dt.Rows(0)("Cantidad_Envases").ToString
            dir.Text = "ALVAREZ 209 COL. CENTRO, MONTERREY, NUEVO LEON"
            otros.Text = "0.00"
            total.Text = Format(dt.Rows(0)("Importe"), "0.00").ToString
            traslada.Text = dt.Rows(0)("nombre_cia").ToString
            nombre_co.Text = dt.Rows(0)("Nombre_Comercial").ToString + " (" + dt.Rows(0)("Numero_Comercial").ToString + "," + dt.Rows(0)("Colonia_Comercial").ToString + "," + dt.Rows(0)("CP_Comercial").ToString + " )"
            'direccion_co.Text = dt.Rows(0)("Calle_Comercial").ToString + " " + dt.Rows(0)("Numero_Comercial").ToString + "," + dt.Rows(0)("Colonia_Comercial").ToString + "," + dt.Rows(0)("CP_Comercial").ToString
            importe_le.Text = fn_EnLetras(dt(0)("Importe").ToString)
            moneda_ex.Text = "0.00"
            moneda_na.Text = Format(dt.Rows(0)("Importe"), "0.00").ToString
            envases_mo.Text = "0" + " +" + dt.Rows(0)("EnvasesSN").ToString + " SN"
            envases_bi.Text = 0
            envases_do.Text = 0
            Dim envase As String = ""
            For Each row In dt.Rows
                If envase <> row("EnvaseN").ToString() Then
                    sellos_to.Text += "[" + row("EnvaseN").ToString() + "]"
                    envase = row("EnvaseN").ToString()
                End If
            Next
            envase = ""
            Dim acces As Boolean = True

            For Each row In dt.Rows
                If (envase = row("EnvaseN").ToString()) Then
                    sellos_to.Text += " / " + row("Cantidad").ToString() + " " + row("Descripcion").ToString() + " "
                ElseIf acces Then
                    sellos_to.Text += " / " + row("Cantidad").ToString() + " " + row("Descripcion").ToString() + " "
                    envase = row("EnvaseN").ToString()
                    acces = False
                End If
            Next
            'firma.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(0)("id_empleado")
            firma_cajero.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(0)("Firma_cajero")
            ruta_remision.Text = dt.Rows(0)("Ruta").ToString
            unidad_remision.Text = dt.Rows(0)("Unidad").ToString()
            fecha_real.Text = dt.Rows(0)("Fechar_entrega").ToString()
            hora_real.Text = dt.Rows(0)("Horar_entrega").ToString()
            StringQR(dt, 1)
            StringQR(dt, 2)
            'QR_Remitente(dt)
            'QR_Consignatorio(dt)            
        Catch ex As Exception
            fn_Alerta("La remision no se encuentra disponible en este momento.")
        End Try
    End Sub
    Public Sub Llenar_Remision2(Fila As Integer)
        dt = New DataTable()
        Try
            If Data_Remision IsNot Nothing AndAlso Data_Remision.Rows.Count > 0 Then
                dt = Data_Remision
            Else
                dt = cn.fn_ConsultaR2(CDbl(Num_Remision))
            End If
            If (dt IsNot Nothing AndAlso dt.Rows.Count > 0) Then
                Importes_Web()
                remision_n.Text = dt.Rows(Fila)("Numero_Remision").ToString()
                num_cliente.Text = dt.Rows(Fila)("Clave_Cliente").ToString()
                fecha_va.Text = dt.Rows(Fila)("Fecha").ToString()
                fecha_real.Text = dt.Rows(Fila)("Fecha").ToString()
                hora_real.Text = dt.Rows(Fila)("Hora").ToString()
                envases_bi.Text = Env_B
                envases_mo.Text = Env_M
                envases_do.Text = Env_MIX
                envases_total.Text = dt.Rows(Fila)("Cantidad_En").ToString() + "+ " + dt.Rows(Fila)("Envases_Sin").ToString() + " S/N"
                total.Text = Format((Mon_Na + Mon_Otros + Mon_Ex), "##,##0.00").ToString()
                moneda_na.Text = Format(Mon_Na, "##,##0.00").ToString()
                moneda_ex.Text = Format(Mon_Ex, "##,##0.00").ToString()
                otros.Text = Format(Mon_Otros, "##,##0.00").ToString()
                dir.Text = dt.Rows(Fila)("Calle_Comercial").ToString + " " + dt.Rows(Fila)("Numero_Comercial").ToString + "," + dt.Rows(Fila)("Colonia_Comercial").ToString + "," + dt.Rows(Fila)("Ciudad") + "," + dt.Rows(Fila)("CP_Comercial").ToString
                ruta_remision.Text = dt.Rows(Fila)("Ruta").ToString()
                unidad_remision.Text = dt.Rows(Fila)("Unidad").ToString()
                traslada.Text = dt.Rows(Fila)("Nombre_Comercial")
                'importe_le.Text = fn_EnLetras(dt(Fila)("Importe").ToString)
                importe_le.Text = fn_EnLetras(Format((Mon_Na + Mon_Otros + Mon_Ex), "##,##0.00").ToString())
                nombre_co.Text = dt.Rows(Fila)("Cd_nombre").ToString + " (" + dt.Rows(Fila)("Cd_Calle").ToString + " " + dt.Rows(Fila)("Cd_Numero").ToString + "," + dt.Rows(Fila)("Cd_Colonia").ToString + "," + dt.Rows(Fila)("CiudadD") + "," + dt.Rows(Fila)("Cd_Cp").ToString + ")"
                'direccion_co.Text = dt.Rows(Fila)("Cd_Calle").ToString + " " + dt.Rows(Fila)("Cd_Numero").ToString + "," + dt.Rows(Fila)("Cd_Colonia").ToString + "," + dt.Rows(Fila)("CiudadD") + "," + dt.Rows(Fila)("Cd_Cp").ToString
                sellos_to.Text = Envases_Remision
                notas.Text = dt(Fila)("Comentarios").ToString 'Agregado 31-11-21
                firma_cajero.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(Fila)("Firma_cajero").ToString
                StringQR(dt, 1)
                StringQR(dt, 2)
            Else
                fn_Alerta("La remision no se encuentra disponible en este momento.")
            End If
        Catch ex As Exception
            fn_Alerta("La remision no se encuentra disponible en este momento." + ex.Message.ToString)
        End Try
    End Sub
    Public Sub Llenar_Remision3()
        dt = New DataTable()
        Try
            Importes_Web()
            If Data_Remision IsNot Nothing AndAlso Data_Remision.Rows.Count > 0 Then
                dt = Data_Remision
            Else
                dt = cn.fn_ConsultaR3(CDbl(Num_Remision))
            End If
            If (Not dt Is Nothing AndAlso dt.Rows.Count > 0) Then
                remision_n.Text = dt.Rows(0)("Numero_Remision").ToString()
                num_cliente.Text = dt.Rows(0)("Clave_Cliente").ToString()
                fecha_va.Text = dt.Rows(0)("Fecha").ToString()
                fecha_real.Text = dt.Rows(0)("Fecha").ToString()
                hora_real.Text = dt.Rows(0)("Hora").ToString()
                envases_bi.Text = Env_B
                envases_mo.Text = Env_M
                envases_do.Text = Env_MIX
                envases_total.Text = dt.Rows(0)("Cantidad_En").ToString() + "+ " + dt.Rows(0)("Envases_Sin").ToString() + " S/N"
                total.Text = Format(dt.Rows(0)("Importe"), "##,##0.00").ToString()
                moneda_na.Text = Format(Mon_Na, "##,##0.00").ToString()
                moneda_ex.Text = Format(Mon_Ex, "##,##0.00").ToString()
                otros.Text = Format(Mon_Otros, "##,##0.00").ToString()
                dir.Text = dt.Rows(0)("Calle_Comercial").ToString + " " + dt.Rows(0)("Numero_Comercial").ToString + "," + dt.Rows(0)("Colonia_Comercial").ToString + "," + dt.Rows(0)("Ciudad").ToString + "," + dt.Rows(0)("CP_Comercial").ToString
                ruta_remision.Text = dt.Rows(0)("Ruta").ToString()
                unidad_remision.Text = dt.Rows(0)("Unidad").ToString
                traslada.Text = dt.Rows(0)("Nombre_Comercial")
                importe_le.Text = fn_EnLetras(dt(0)("Importe").ToString)
                nombre_co.Text = dt.Rows(0)("Cd_nombre").ToString + " (" + dt.Rows(0)("Cd_Calle").ToString + " " + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD").ToString + "," + dt.Rows(0)("Cd_Cp").ToString + " )"
                'direccion_co.Text = dt.Rows(0)("Cd_Calle").ToString + " " + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD").ToString + "," + dt.Rows(0)("Cd_Cp").ToString
                sellos_to.Text = Envases_Remision
                firma_cajero.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(0)("Cajero").ToString
                StringQR(dt, 1)
                StringQR(dt, 2)
            Else
                fn_Alerta("La remision no se encuentra disponible en este momento...")
            End If
        Catch ex As Exception
            fn_Alerta("La remision no se encuentra disponible en este momento.")
        End Try
    End Sub
    Public Sub Llenar_Remision4()
        dt = New DataTable()
        Try
            Importes_ATMD()
            If Data_Remision IsNot Nothing AndAlso Data_Remision.Rows.Count > 0 Then
                dt = Data_Remision
            Else
                dt = cn.fn_ConsultaR4(CDbl(Num_Remision))
            End If
            If (Not dt Is Nothing AndAlso dt.Rows.Count > 0) Then
                remision_n.Text = dt.Rows(0)("NumeroRemision").ToString()
                num_cliente.Text = dt.Rows(0)("NumCliente").ToString()
                fecha_va.Text = dt.Rows(0)("FechaRegistro").ToString()
                fecha_real.Text = dt.Rows(0)("FechaREntrega").ToString()
                hora_real.Text = dt.Rows(0)("HoraREntrega").ToString()
                envases_bi.Text = Env_B
                envases_mo.Text = Env_M
                envases_do.Text = Env_MIX
                envases_total.Text = dt.Rows(0)("CantEnvases").ToString()
                total.Text = Format(dt.Rows(0)("Importe"), "##,##0.00").ToString()
                moneda_na.Text = Format(Mon_Na, "##,##0.00").ToString()
                moneda_ex.Text = Format(Mon_Ex, "##,##0.00").ToString()
                otros.Text = Format(Mon_Otros, "##,##0.00").ToString()
                dir.Text = dt.Rows(0)("Calle").ToString + " " + dt.Rows(0)("Numero").ToString + "," + dt.Rows(0)("Colonia").ToString + "," + dt.Rows(0)("Ciudad").ToString + "," + dt.Rows(0)("Estado").ToString
                ruta_remision.Text = dt.Rows(0)("Ruta").ToString()
                unidad_remision.Text = dt.Rows(0)("Unidad").ToString
                traslada.Text = dt.Rows(0)("VRecibidos")
                importe_le.Text = fn_EnLetras(dt(0)("Importe").ToString)
                nombre_co.Text = dt.Rows(0)("EntregarEn").ToString + " (" + dt.Rows(0)("Direccion_Fisica").ToString + " )"
                'direccion_co.Text = dt.Rows(0)("Direccion_Fisica").ToString '+ " " + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD").ToString + "," + dt.Rows(0)("Cd_Cp").ToString
                sellos_to.Text = Envases_Remision
                firma_cajero.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(0)("Transporta").ToString
                StringQR(dt, 1)
                StringQR(dt, 2)
            Else
                fn_Alerta("La remision no se encuentra disponible en este momento...")
            End If
        Catch ex As Exception
            fn_Alerta("La remision no se encuentra disponible en este momento.")
        End Try
    End Sub
    Public Sub Llenar_Remision5()
        dt = New DataTable()
        Try
            'Importes_ATMD()
            If Data_Remision IsNot Nothing AndAlso Data_Remision.Rows.Count > 0 Then
                dt = Data_Remision
            Else
                dt = cn.fn_ConsultaR5(CDbl(Num_Remision))
            End If
            If (Not dt Is Nothing AndAlso dt.Rows.Count > 0) Then
                remision_n.Text = dt.Rows(0)("NumeroRemision").ToString()
                num_cliente.Text = dt.Rows(0)("NumCliente").ToString()
                fecha_va.Text = dt.Rows(0)("FechaRegistro").ToString()
                fecha_real.Text = dt.Rows(0)("FechaREntrega").ToString()
                hora_real.Text = dt.Rows(0)("HoraREntrega").ToString()
                envases_bi.Text = Env_B
                envases_mo.Text = Env_M
                envases_do.Text = Env_MIX
                envases_total.Text = dt.Rows(0)("CantEnvases").ToString()
                total.Text = Format(dt.Rows(0)("Importe"), "##,##0.00").ToString()
                moneda_na.Text = Format(Mon_Na, "##,##0.00").ToString()
                moneda_ex.Text = Format(Mon_Ex, "##,##0.00").ToString()
                otros.Text = Format(Mon_Otros, "##,##0.00").ToString()
                dir.Text = dt.Rows(0)("Direccion_Fisica").ToString
                ruta_remision.Text = dt.Rows(0)("Ruta").ToString()
                unidad_remision.Text = dt.Rows(0)("Unidad").ToString
                traslada.Text = dt.Rows(0)("VRecibidos")
                importe_le.Text = fn_EnLetras(dt(0)("Importe").ToString)
                nombre_co.Text = dt.Rows(0)("EntregarEn").ToString + " (" + dt.Rows(0)("Calle").ToString + " " + dt.Rows(0)("Numero").ToString + "," + dt.Rows(0)("Colonia").ToString + "," + dt.Rows(0)("Ciudad").ToString + "," + dt.Rows(0)("Estado").ToString + ") "
                'direccion_co.Text = dt.Rows(0)("Calle").ToString + " " + dt.Rows(0)("Numero").ToString + "," + dt.Rows(0)("Colonia").ToString + "," + dt.Rows(0)("Ciudad").ToString + "," + dt.Rows(0)("Estado").ToString
                sellos_to.Text = Envases_Remision
                firma_cajero.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(0)("Transporta").ToString
                StringQR(dt, 1)
                StringQR(dt, 2)
            Else
                fn_Alerta("La remision no se encuentra disponible en este momento...")
            End If
        Catch ex As Exception
            fn_Alerta("La remision no se encuentra disponible en este momento.")
        End Try
    End Sub
    Public Sub Llenar_Remision6()
        dt = New DataTable()
        Try
            'Importes_ATMD()
            If Data_Remision IsNot Nothing AndAlso Data_Remision.Rows.Count > 0 Then
                dt = Data_Remision
            Else
                dt = cn.fn_ConsultaR6(CDbl(Num_Remision))
            End If
            If (Not dt Is Nothing AndAlso dt.Rows.Count > 0) Then
                remision_n.Text = dt.Rows(0)("NumeroRemision").ToString()
                num_cliente.Text = dt.Rows(0)("NumCliente").ToString()
                fecha_va.Text = dt.Rows(0)("FechaRegistro").ToString()
                fecha_real.Text = dt.Rows(0)("FechaREntrega").ToString()
                hora_real.Text = dt.Rows(0)("HoraREntrega").ToString()
                envases_bi.Text = Env_B
                envases_mo.Text = Env_M
                envases_do.Text = Env_MIX
                envases_total.Text = dt.Rows(0)("CantEnvases").ToString()
                total.Text = Format(dt.Rows(0)("Importe"), "##,##0.00").ToString()
                moneda_na.Text = Format(Mon_Na, "##,##0.00").ToString()
                moneda_ex.Text = Format(Mon_Ex, "##,##0.00").ToString()
                otros.Text = Format(Mon_Otros, "##,##0.00").ToString()
                dir.Text = dt.Rows(0)("Direccion_Fisica").ToString
                ruta_remision.Text = dt.Rows(0)("Ruta").ToString()
                unidad_remision.Text = dt.Rows(0)("Unidad").ToString
                traslada.Text = dt.Rows(0)("VRecibidos")
                importe_le.Text = fn_EnLetras(dt(0)("Importe").ToString)
                nombre_co.Text = dt.Rows(0)("EntregarEn").ToString + " (" + dt.Rows(0)("Calle").ToString + " " + dt.Rows(0)("Numero").ToString + "," + dt.Rows(0)("Colonia").ToString + "," + dt.Rows(0)("Ciudad").ToString + "," + dt.Rows(0)("Estado").ToString + " )"
                'direccion_co.Text = dt.Rows(0)("Calle").ToString + " " + dt.Rows(0)("Numero").ToString + "," + dt.Rows(0)("Colonia").ToString + "," + dt.Rows(0)("Ciudad").ToString + "," + dt.Rows(0)("Estado").ToString
                sellos_to.Text = Envases_Remision
                firma_cajero.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt.Rows(0)("Transporta").ToString
                StringQR(dt, 1)
                StringQR(dt, 2)
            Else
                fn_Alerta("La remision no se encuentra disponible en este momento...")
            End If
        Catch ex As Exception
            fn_Alerta("La remision no se encuentra disponible en este momento.")
        End Try
    End Sub
    Sub Importes_ATMD()
        'fn_ImportesATMD
        Dim tbl As New DataTable
        tbl = cn.fn_ImportesATMD(CDbl(Num_Remision))
        For Each Rows As DataRow In tbl.Rows
            If (Rows(1).ToString() = "20.0000") Then
                Veinte.Text = Format(CDbl(Rows(1)) * CDbl(Rows(2)), "##,##0.00").ToString()
            ElseIf (Rows(1).ToString() = "50.0000") Then
                Cincuenta.Text = Format(CDbl(Rows(1)) * CDbl(Rows(2)), "##,##0.00").ToString()
            ElseIf (Rows(1).ToString() = "100.0000") Then
                Cien.Text = Format(CDbl(Rows(1)) * CDbl(Rows(2)), "##,##0.00").ToString()
            ElseIf (Rows(1).ToString() = "200.0000") Then
                Docientos.Text = Format(CDbl(Rows(1)) * CDbl(Rows(2)), "##,##0.00").ToString()
            ElseIf (Rows(1).ToString() = "500.0000") Then
                Quinientos.Text = Format(CDbl(Rows(1)) * CDbl(Rows(2)), "##,##0.00").ToString()
            ElseIf (Rows(1).ToString() = "1000.0000") Then
                Mil.Text = Format(CDbl(Rows(1)) * CDbl(Rows(2)), "##,##0.00").ToString()
            End If
        Next
    End Sub
    Sub Importes_Web()
        Dim tbl As New DataTable
        tbl = cn.fn_Tv_Remisionesweb_GetImportes(CDbl(Num_Remision))
        If tbl IsNot Nothing Then
            'MsgBox(Format(tbl.Rows(0)(2).ToString, "##,##0.00").ToString + "" + tbl.Rows(0)(2).ToString)
            Mil.Text = Format(CDbl(tbl.Rows(0)(2)), "##,##0.00").ToString
            Quinientos.Text = Format(CDbl(tbl.Rows(0)(3)), "##,##0.00").ToString
            Docientos.Text = Format(CDbl(tbl.Rows(0)(4)), "##,##0.00").ToString
            Cien.Text = Format(CDbl(tbl.Rows(0)(5)), "##,##0.00").ToString
            Cincuenta.Text = Format(CDbl(tbl.Rows(0)(6)), "##,##0.00").ToString
            Veinte.Text = Format(CDbl(tbl.Rows(0)(7)), "##,##0.00").ToString
            VeinteM.Text = Format(CDbl(tbl.Rows(0)(8)), "##,##0.00").ToString
            Diez.Text = Format(CDbl(tbl.Rows(0)(9)), "##,##0.00").ToString
            Cinco.Text = Format(CDbl(tbl.Rows(0)(10)), "##,##0.00").ToString
            Dos.Text = Format(CDbl(tbl.Rows(0)(11)), "##,##0.00").ToString
            Uno.Text = Format(CDbl(tbl.Rows(0)(12)), "##,##0.00").ToString
            PCincuenta.Text = Format(CDbl(tbl.Rows(0)(13)), "##,##0.00").ToString
            'PVenite.Text = Format(CDbl(tbl.Rows(0)(14)), "##,##0.00").ToString
            'PDiez.Text = Format(CDbl(tbl.Rows(0)(15)), "##,##0.00").ToString
            'PCinto.Text = Format(CDbl(tbl.Rows(0)(16)), "##,##0.00").ToString
        End If
    End Sub
    Public Function fn_EnLetras(ByVal numero As String, Optional ByVal IDMoneda As Integer = 0) As String

        Dim BandBilion As Boolean
        Dim b, paso, cifra As Integer
        Dim expresion As String = ""
        Dim entero As String = ""
        Dim deci As String = ""
        Dim flag As String = ""
        Dim valor As String = ""
        Dim gOpcionMil As Boolean = False
        Dim Moneda As String

        flag = "N"
        numero = Replace(numero, ",", "")
        '** AQUI REVISAMOS SI EL MONTO TIENE PARTE DECIMAL.
        For paso = 1 To Len(numero) 'DETERMINA CUANTOS CARACTERES TIENE LA CADENA
            If Mid(numero, paso, 1) = "." Or Mid(numero, paso, 1) = "," Then 'DEPENDIENDO DE LA REGIÓN
                flag = "S"
            Else
                If flag = "N" Then
                    entero = entero + Mid(numero, paso, 1) 'EXTAE LA PARTE ENTERA DEL NUMERO
                Else
                    deci = deci + Mid(numero, paso, 1) 'EXTRAE LA PARTE DECIMAL DEL NUMERO
                End If
            End If
        Next paso

        'DEFINIMOS VALOR EN LAS VARIABLES
        'CIFRA Y VALOR PARA USARLAS COMO
        'BANDERAS CONDICIONALES.

        cifra = Len(entero)

        Select Case cifra
            Case Is = 1
                valor = "UNIDAD" 'SIN USAR
            Case Is = 2
                valor = "DECENAS" 'SIN USAR
            Case Is = 3
                valor = "CENTENAS" 'SIN USAR
            Case Is = 4, 5, 6
                valor = "MILES" 'USADO
            Case Is = 7, 8, 9
                valor = "MILION" 'USADO
            Case Is = 10, 11, 12
                valor = "MILIARDOS" 'USADO
            Case Is = 13, 14, 15
                valor = "BILIONES" 'USADO
        End Select

        '*** SI LA CIFRA TIENE VALOR DECIMAL LO ASIGNAMOS AQUI.
        If Len(deci) = 1 Then
            deci = deci & "0/100."  'ANTES TENIA & "0" "/100."
        ElseIf Len(deci) = 2 Then
            deci = deci & "/100."  'ANTES TENIA & "0" "/100."
        ElseIf Len(deci) > 2 Then
            deci = Microsoft.VisualBasic.Left(deci, 2) & "/100."  'ANTES TENIA & "0" "/100."
        Else
            deci = "00/100."
        End If


        flag = "N"
        If Val(numero) >= -999999999999999.0# And Val(numero) <= 999999999999999.0# Then  'SI EL NUMERO ESTA DENTRO DE 0 A 999.999.999
            For paso = Len(entero) To 1 Step -1
                b = Len(entero) - (paso - 1)
                Select Case paso
                    Case 3, 6, 9, 12, 15
                        Select Case Mid(entero, b, 1)
                            Case "1"
                                If Mid(entero, b + 1, 1) = "0" And Mid(entero, b + 2, 1) = "0" Then
                                    expresion = expresion & "CIEN "
                                Else
                                    expresion = expresion & "CIENTO "
                                End If
                            Case "2"
                                expresion = expresion & "DOSCIENTOS "
                            Case "3"
                                expresion = expresion & "TRESCIENTOS "
                            Case "4"
                                expresion = expresion & "CUATROCIENTOS "
                            Case "5"
                                expresion = expresion & "QUINIENTOS "
                            Case "6"
                                expresion = expresion & "SEISCIENTOS "
                            Case "7"
                                expresion = expresion & "SETECIENTOS "
                            Case "8"
                                expresion = expresion & "OCHOCIENTOS "
                            Case "9"
                                expresion = expresion & "NOVECIENTOS "

                        End Select

                    Case 2, 5, 8, 11, 14
                        Select Case Mid(entero, b, 1)
                            Case "1"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    flag = "S"
                                    expresion = expresion & "DIEZ "
                                End If
                                If Mid(entero, b + 1, 1) = "1" Then
                                    flag = "S"
                                    expresion = expresion & "ONCE "
                                End If
                                If Mid(entero, b + 1, 1) = "2" Then
                                    flag = "S"
                                    expresion = expresion & "DOCE "
                                End If
                                If Mid(entero, b + 1, 1) = "3" Then
                                    flag = "S"
                                    expresion = expresion & "TRECE "
                                End If
                                If Mid(entero, b + 1, 1) = "4" Then
                                    flag = "S"
                                    expresion = expresion & "CATORCE "
                                End If
                                If Mid(entero, b + 1, 1) = "5" Then
                                    flag = "S"
                                    expresion = expresion & "QUINCE "
                                End If
                                If Mid(entero, b + 1, 1) > "5" Then
                                    flag = "N"
                                    expresion = expresion & "DIECI"
                                End If

                            Case "2"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "VEINTE "
                                    flag = "S"
                                Else
                                    expresion = expresion & "VEINTI"
                                    flag = "N"
                                End If

                            Case "3"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "TREINTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "TREINTA Y "
                                    flag = "N"
                                End If

                            Case "4"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "CUARENTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "CUARENTA Y "
                                    flag = "N"
                                End If

                            Case "5"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "CINCUENTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "CINCUENTA Y "
                                    flag = "N"
                                End If

                            Case "6"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "SESENTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "SESENTA Y "
                                    flag = "N"
                                End If

                            Case "7"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "SETENTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "SETENTA Y "
                                    flag = "N"
                                End If

                            Case "8"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "OCHENTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "OCHENTA Y "
                                    flag = "N"
                                End If

                            Case "9"
                                If Mid(entero, b + 1, 1) = "0" Then
                                    expresion = expresion & "NOVENTA "
                                    flag = "S"
                                Else
                                    expresion = expresion & "NOVENTA Y "
                                    flag = "N"
                                End If

                            Case "0"
                                'EXPRESION = EXPRESION & ""
                                flag = "N"
                        End Select


                    Case 1, 4, 7, 10, 13
                        Select Case Mid(entero, b, 1)
                            Case "1"

                                If flag = "N" Then
                                    If paso = 1 Then
                                        expresion = expresion & "UNO "
                                    Else
                                        expresion = expresion & "UN "
                                    End If
                                End If

                            Case "2"
                                If flag = "N" Then
                                    expresion = expresion & "DOS "
                                End If

                            Case "3"
                                If flag = "N" Then
                                    expresion = expresion & "TRES "
                                End If
                            Case "4"
                                If flag = "N" Then
                                    expresion = expresion & "CUATRO "
                                End If
                            Case "5"
                                If flag = "N" Then
                                    expresion = expresion & "CINCO "
                                End If
                            Case "6"
                                If flag = "N" Then
                                    expresion = expresion & "SEIS "
                                End If
                            Case "7"
                                If flag = "N" Then
                                    expresion = expresion & "SIETE "
                                End If
                            Case "8"
                                If flag = "N" Then
                                    expresion = expresion & "OCHO "
                                End If
                            Case "9"
                                If flag = "N" Then
                                    expresion = expresion & "NUEVE "
                                End If
                        End Select
                End Select

                '*************************************************************************

                '********* MILES PARA MILES
                If paso = 4 And valor = "MILES" Then
                    If Mid(entero, 6, 1) <> "0" Or Mid(entero, 5, 1) <> "0" Or Mid(entero, 4, 1) <> "0" Or _
                        (Mid(entero, 6, 1) = "0" And Mid(entero, 5, 1) = "0" And Mid(entero, 4, 1) = "0" And _
                        Len(entero) <= 6) Then
                        expresion = expresion & "MIL "
                    End If
                End If

                '********** MILES PARA MILLONES
                If paso = 4 And valor = "MILION" Then

                    If cifra = 7 And Val(Mid(entero, 2, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If


                    If cifra = 8 And Val(Mid(entero, 3, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If

                    If cifra = 9 And Val(Mid(entero, 4, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If
                End If


                '********** MILES PARA MILLARDOS
                If paso = 4 And valor = "MILIARDOS" Then

                    If cifra = 10 And Val(Mid(entero, 5, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If


                    If cifra = 11 And Val(Mid(entero, 6, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If

                    If cifra = 12 And Val(Mid(entero, 7, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If
                End If

                '********** MILES PARA BILLONES
                If paso = 4 And valor = "BILIONES" Then

                    If cifra = 13 And Val(Mid(entero, 8, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If

                    If cifra = 14 And Val(Mid(entero, 9, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If

                    If cifra = 15 And Val(Mid(entero, 10, 3)) >= 1 Then
                        expresion = expresion & "MIL "
                    End If
                End If

                '**********"INICIAMOS CONDICIONES PARA USAR PALABRA MILES DE MILLONES"*****************
                Select Case gOpcionMil
                    Case True 'DESEA USAR LA PALABRA MILES DE MILLONES
                        'Z********[SOLO PARA MILLARDOS] CUANDO MILLONES ES IGUAL A CERO
                        If paso = 7 And valor = "MILIARDOS" And cifra = 10 _
                        And Val(Mid(entero, 2, 3)) = 0 Then
                            expresion = expresion & "MILLONES "
                        End If


                        If paso = 7 And valor = "MILIARDOS" And cifra = 11 _
                        And Val(Mid(entero, 3, 3)) = 0 Then
                            expresion = expresion & "MILLONES "
                        End If


                        If paso = 7 And valor = "MILIARDOS" And cifra = 12 _
                        And Val(Mid(entero, 4, 3)) = 0 Then
                            expresion = expresion & "MILLONES "
                        End If
                        'Z*****PONER MILLARDOS DE BILLONES ******
                        If paso = 10 And valor = "BILIONES" And cifra = 13 _
                        And Val(Mid(entero, 2, 3)) > 0 Then
                            expresion = expresion & "MIL "
                            BandBilion = True
                        End If

                        If paso = 10 And valor = "BILIONES" And cifra = 14 _
                        And Val(Mid(entero, 3, 3)) > 0 Then
                            expresion = expresion & "MIL "
                            BandBilion = True
                        End If

                        If paso = 10 And valor = "BILIONES" And cifra = 15 _
                        And Val(Mid(entero, 4, 3)) > 0 Then
                            expresion = expresion & "MIL "
                            BandBilion = True
                        End If

                        'Z******** SOLO PARA BILLONES CUANDO MILLARDOS ES MAS DE CERO
                        If paso = 7 And valor = "BILIONES" And cifra = 13 _
                        And Val(Mid(entero, 5, 3)) = 0 And BandBilion Then
                            expresion = expresion & "MILLONES "
                            BandBilion = False
                        End If

                        If paso = 7 And valor = "BILIONES" And cifra = 14 _
                        And Val(Mid(entero, 6, 3)) = 0 And BandBilion Then
                            expresion = expresion & "MILLONES "
                            BandBilion = False
                        End If

                        If paso = 7 And valor = "BILIONES" And cifra = 15 _
                        And Val(Mid(entero, 7, 3)) = 0 And BandBilion Then
                            expresion = expresion & "MILLONES "
                            BandBilion = False
                        End If

                        'Z********** SOLO PARA MILLARDOS PRONUNCIADOS EN MILES DE MILLONES.
                        If paso = 10 And valor = "MILIARDOS" Then
                            expresion = expresion & "MIL "
                        End If
                        '**********"TERMINAMOS CONDICIONES PARA USAR PALABRA MILES DE MILLONES"**********


                        '**********"INICIAMOS CONDICIONES PARA USAR PALABRA MILLARDO(S)"**********
                    Case Else ' DESEA USAR  LA PALABRA MILLARDOS

                        If paso = 10 And valor = "BILIONES" And cifra = 13 _
                        And Val(Mid(entero, 2, 3)) > 0 Then
                            If Val(Mid(entero, 2, 3)) = 1 Then
                                expresion = expresion & "MILLARDO "
                            Else
                                expresion = expresion & "MILLARDOS "
                            End If
                        End If
                        If paso = 10 And valor = "BILIONES" And cifra = 14 _
                        And Val(Mid(entero, 3, 3)) > 0 Then
                            If Val(Mid(entero, 3, 3)) = 1 Then
                                expresion = expresion & "MILLARDO "
                            Else
                                expresion = expresion & "MILLARDOS "
                            End If
                        End If
                        If paso = 10 And valor = "BILIONES" And cifra = 15 _
                        And Val(Mid(entero, 4, 3)) > 0 Then
                            If Val(Mid(entero, 4, 3)) = 1 Then
                                expresion = expresion & "MILLARDO "
                            Else
                                expresion = expresion & "MILLARDOS "
                            End If
                        End If

                        '********** MILLARDOS

                        If paso = 10 And valor = "MILIARDOS" Then
                            If Len(entero) = 10 And Mid(entero, 1, 1) = "1" Then
                                expresion = expresion & "MILLARDO "
                            Else
                                expresion = expresion & "MILLARDOS "
                            End If
                        End If
                        '**********"TERMINAMOS CONDICIONES PARA USAR PALABRA MILLARDO(S)"**********
                        '**************************************************************************
                End Select

                '*******[SOLO PARA MILLARDOS] CUANDO MILLONES ES MAS DE CERO

                If paso = 7 And valor = "MILIARDOS" And cifra = 10 And _
                Val(Mid(entero, 2, 3)) > 0 Then
                    If Val(Mid(entero, 2, 3)) = 1 Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If

                If paso = 7 And valor = "MILIARDOS" And cifra = 11 _
                And Val(Mid(entero, 3, 3)) > 0 Then
                    If Val(Mid(entero, 3, 3)) = 1 Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If

                If paso = 7 And valor = "MILIARDOS" And cifra = 12 _
                And Val(Mid(entero, 4, 3)) > 0 Then
                    If Val(Mid(entero, 4, 3)) = 1 Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If

                '*************************************************


                '******** SOLO BILLONES

                If paso = 7 And valor = "BILIONES" And cifra = 13 _
                And Val(Mid(entero, 5, 3)) > 0 Then
                    If Val(Mid(entero, 5, 3)) = 1 Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If

                If paso = 7 And valor = "BILIONES" And cifra = 14 _
                And Val(Mid(entero, 6, 3)) > 0 Then
                    If Val(Mid(entero, 6, 3)) = 1 Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If

                If paso = 7 And valor = "BILIONES" And cifra = 15 _
                And Val(Mid(entero, 7, 3)) > 0 Then
                    If Val(Mid(entero, 7, 3)) = 1 Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If
                '****************************************************


                '********** SOLO PARA MILLONES
                If paso = 7 And valor = "MILION" Then
                    If Len(entero) = 7 And Mid(entero, 1, 1) = "1" Then
                        expresion = expresion & "MILLÓN "
                    Else
                        expresion = expresion & "MILLONES "
                    End If
                End If

                '******** SOLO PARA BILLONES
                If paso = 13 Then
                    If Len(entero) = 13 And Mid(entero, 1, 1) = "1" Then
                        expresion = expresion & "BILLÓN "
                    Else
                        expresion = expresion & "BILLONES "
                    End If
                End If


            Next paso

            'Agregar Moneda

            If IDMoneda <> 0 Then

                Moneda = fn_ObtenTipoMoneda(IDMoneda)
                expresion += " " + Moneda + " "
            End If


            '*** EVALUAR QUE ESCRIBIR
            If deci <> "" Then 'SI EL VALOR RESULTANTE ES NEGATIVO CON DECIMAL
                If Mid(entero, 1, 1) = "-" Or Mid(entero, 1, 1) = "(" Then
                    fn_EnLetras = "MENOS " & expresion & "CON " & deci 'ANTES & "/100"
                Else
                    fn_EnLetras = expresion & "CON " & deci 'ANTES & "/100"
                End If
            Else 'SI EL VALOR RESULTANTE ES NEGATIVO SIN DECIMAL
                If Mid(entero, 1, 1) = "-" Or Mid(entero, 1, 1) = "(" Then
                    fn_EnLetras = "MENOS " & expresion
                Else
                    fn_EnLetras = expresion 'SI NO TIENE DECIMAL
                End If
            End If

            If Val(numero) = 0 Then fn_EnLetras = "MONTO ES IGUAL A CERO." 'NO DEBERÍA LLEAGR AQUI
        Else 'SI EL NUMERO A CONVERTIR ESTÁ FUERA DEL RANGO SUPERIOR O INFERIOR
            fn_EnLetras = "ERROR EN EL DATO INTRODUCIDO"
        End If
    End Function
    Public Function fn_ObtenTipoMoneda(ByVal IdMoneda As Integer) As String
        Try
            'Dim cnn As New SqlConnection(Session("cnx_siac"))
            Dim cmd As SqlClient.SqlCommand = cn_Datos.CreaComando("Cat_Monedas_ReadTipoCambio")
            Dim dt_lc As New DataTable

            CreaParametro(cmd, "@Id_Moneda", SqlDbType.BigInt, IdMoneda)

            dt_lc = cn_Datos.EjecutaConsulta(cmd)

            If dt_lc.Rows.Count > 0 Then
                Return (dt_lc.Rows(0)(1).ToString)
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Sub StringQR(dt As DataTable, Tipo As Integer)    
        If (Tipo = 1) Then
            If (Tipo_Remision = "MAT") Then
                qrmsg = dt.Rows(0)("Nombre_em").ToString() + "," + dt.Rows(0)("fecha_firma").ToString() + "," + dt.Rows(0)("Hora").ToString() + ","
            ElseIf Tipo_Remision = "TRAS" Then
                qrmsg = dt.Rows(0)("Nombre_Comercial").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
            ElseIf Tipo_Remision = "DOT" Then
                qrmsg = "Servicio Integral de Seguridad. S.A. de C.V.,ALVAREZ NTE. 209 MONTERREY, N.L. C.P. 64000 ,CONMUTADOR: 8047-4545, 8047-4546 FAX 8047 4550"
            ElseIf Tipo_Remision = "ATMD" Then
                qrmsg = dt.Rows(0)("Remitente") + "," + dt.Rows(0)("HoraRemitente").ToString + ". " ' + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD") + "," + dt.Rows(0)("Cd_Cp").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
            ElseIf Tipo_Remision = "ATMF" Then
                qrmsg = dt.Rows(0)("Remitente") + "," + dt.Rows(0)("HoraRemitente").ToString + ". "
            ElseIf Tipo_Remision = "ATMC" Then
                qrmsg = dt.Rows(0)("Remitente") + "," + dt.Rows(0)("HoraRemitente").ToString + ". "
            End If
            CrearQR(qrRemite)
        Else
            If Tipo_Remision = "MAT" Then
                qrmsg = dt.Rows(0)("Nombre_Comercial").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
            ElseIf Tipo_Remision = "TRAS" Then
                qrmsg = "Servicio Integral de Seguridad. S.A. de C.V.,ALVAREZ NTE. 209 MONTERREY, N.L. C.P. 64000 ,CONMUTADOR: 8047-4545, 8047-4546 FAX 8047 4550"
            ElseIf Tipo_Remision = "DOT" Then
                qrmsg = dt.Rows(0)("Cd_nombre") + "," + dt.Rows(0)("Cd_Calle").ToString + " " + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD") + "," + dt.Rows(0)("Cd_Cp").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
            ElseIf Tipo_Remision = "ATMD" Then
                qrmsg = "Sin firma de consignatorio."
            ElseIf Tipo_Remision = "ATMF" Then
                qrmsg = "Sin firma de consignatorio."
            ElseIf Tipo_Remision = "ATMC" Then
                qrmsg = "Sin firma de consignatorio."
            End If
            CrearQR(qrConsignatorio)
        End If
    End Sub
    Sub CrearQR(Img As System.Web.UI.HtmlControls.HtmlImage)         
        Dim qrEncoder As QrEncoder = New QrEncoder(ErrorCorrectionLevel.H)
        Dim qrCode As QrCode = New QrCode()
        qrEncoder.TryEncode(qrmsg, qrCode)
        Dim renderer As GraphicsRenderer = New GraphicsRenderer(New FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White)
        Dim memori As MemoryStream = New MemoryStream()
        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, memori)
        Dim imgbites As Byte() = memori.ToArray
        Img.Src = "data:image/gif;base64," + Convert.ToBase64String(imgbites)
        Img.Height = 100
        Img.Width = 100
    End Sub
    'Sub QR_Consignatorio(ByVal dt As DataTable)
    '    Dim qrEncoder As QrEncoder = New QrEncoder(ErrorCorrectionLevel.H)
    '    Dim qrCode As QrCode = New QrCode()
    '    Dim qrmsg As String
    '    If Tipo_Remision = "MAT" Then
    '        qrmsg = dt.Rows(0)("Nombre_Comercial").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
    '    ElseIf Tipo_Remision = "TRAS" Then
    '        qrmsg = "Servicio Integral de Seguridad. S.A. de C.V.,ALVAREZ NTE. 209 MONTERREY, N.L. C.P. 64000 ,CONMUTADOR: 8047-4545, 8047-4546 FAX 8047 4550"
    '    ElseIf Tipo_Remision = "DOT" Then
    '        qrmsg = dt.Rows(0)("Cd_nombre") + "," + dt.Rows(0)("Cd_Calle").ToString + " " + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD") + "," + dt.Rows(0)("Cd_Cp").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
    '    ElseIf Tipo_Remision = "ATMD" Then
    '        qrmsg = "Sin firma de consignatorio."
    '    ElseIf Tipo_Remision = "ATMF" Then
    '        qrmsg = "Sin firma de consignatorio."
    '    ElseIf Tipo_Remision = "ATMC" Then
    '        qrmsg = "Sin firma de consignatorio."
    '    End If

    '    qrEncoder.TryEncode(qrmsg, qrCode)
    '    Dim renderer As GraphicsRenderer = New GraphicsRenderer(New FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White)        
    '    Dim memori As MemoryStream = New MemoryStream()
    '    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, memori)
    '    Dim imgbites As Byte() = memori.ToArray
    '    qrRemite.Src = "data:image/gif;base64," + Convert.ToBase64String(imgbites)        
    '    qrRemite.Height = 100
    '    qrRemite.Width = 100
    'End Sub
    'Sub QR_Remitente(ByVal dt As DataTable)
    '    Dim qrEncoder As QrEncoder = New QrEncoder(ErrorCorrectionLevel.H)
    '    Dim qrCode As QrCode = New QrCode()        
    '    Dim qrmsg As String
    '    If (Tipo_Remision = "MAT") Then
    '        qrmsg = dt.Rows(0)("Nombre_em").ToString() + "," + dt.Rows(0)("fecha_firma").ToString() + "," + dt.Rows(0)("Hora").ToString() + ","
    '    ElseIf Tipo_Remision = "TRAS" Then
    '        qrmsg = dt.Rows(0)("Nombre_Comercial").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
    '    ElseIf Tipo_Remision = "DOT" Then
    '        qrmsg = "Servicio Integral de Seguridad. S.A. de C.V.,ALVAREZ NTE. 209 MONTERREY, N.L. C.P. 64000 ,CONMUTADOR: 8047-4545, 8047-4546 FAX 8047 4550"
    '    ElseIf Tipo_Remision = "ATMD" Then        
    '        qrmsg = dt.Rows(0)("Remitente") + "," + dt.Rows(0)("HoraRemitente").ToString + ". " ' + dt.Rows(0)("Cd_Numero").ToString + "," + dt.Rows(0)("Cd_Colonia").ToString + "," + dt.Rows(0)("CiudadD") + "," + dt.Rows(0)("Cd_Cp").ToString + "," + dt.Rows(0)("Nombre_firma").ToString() + "," + dt.Rows(0)("fecha_firma").ToString()
    '    ElseIf Tipo_Remision = "ATMF" Then
    '        qrmsg = dt.Rows(0)("Remitente") + "," + dt.Rows(0)("HoraRemitente").ToString + ". "
    '    ElseIf Tipo_Remision = "ATMC" Then
    '        qrmsg = dt.Rows(0)("Remitente") + "," + dt.Rows(0)("HoraRemitente").ToString + ". "
    '    End If

    '    qrEncoder.TryEncode(qrmsg, qrCode)
    '    Dim renderer As GraphicsRenderer = New GraphicsRenderer(New FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White)        
    '    Dim memori As MemoryStream = New MemoryStream()
    '    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, memori)
    '    Dim imgbites As Byte() = memori.ToArray

    '    qrConsignatorio.Src = "data:image/gif;base64," + Convert.ToBase64String(imgbites)
    '    qrConsignatorio.Height = 100
    '    qrConsignatorio.Width = 100
    'End Sub
    'Protected Sub contador_Click(sender As Object, e As EventArgs) Handles contador.Click
    '    'Llenar_Remision2(1)
    'End Sub

    'Private Sub btn_Buscar_ServerClick(sender As Object, e As EventArgs) Handles btn_Buscar.ServerClick
    '    If (btn_Buscar.InnerText.Trim = "") Then Exit Sub
    '    Dim tbl As DataTable

    '    tbl = cn.fn_ConsultaIDRemision(btn_Buscar.InnerText.Trim)
    '    If tbl.Rows.Count > 0 Then
    '        Num_Remision = tbl.Rows(0)(0).ToString()
    '        Id_Venta = tbl.Rows(0)(1).ToString
    '        Atm_Remision()
    '    End If
    'End Sub
    'Sub Atm_Remision()
    '    Dim Tbl_Remision As DataTable = New DataTable
    '    If Id_Venta <> "" Then
    '        Tbl_Remision = cn.fn_ConsultaR1(CInt(Id_Venta))
    '        Tipo_Remision = ""
    '    End If
    '    If Tbl_Remision.Rows.Count = 0 Then
    '        Tbl_Remision = cn.fn_ConsultaR2(CInt(Num_Remision))
    '         Tipo_Remision = ""
    '    End If
    '    If Tbl_Remision.Rows.Count = 0 Then
    '        Tbl_Remision = cn.fn_ConsultaR3(CInt(Num_Remision))
    '         Tipo_Remision = ""
    '    End If
    '    If Tbl_Remision.Rows.Count = 0 Then
    '        Tbl_Remision = cn.fn_ConsultaR4(CInt(Num_Remision))
    '         Tipo_Remision = ""
    '    End If
    '    If Tbl_Remision.Rows.Count = 0 Then
    '        Tbl_Remision = cn.fn_ConsultaR5(CInt(Num_Remision))
    '         Tipo_Remision = ""
    '    End If
    '    If Tbl_Remision.Rows.Count = 0 Then
    '        Tbl_Remision = cn.fn_ConsultaR6(CInt(Num_Remision))
    '         Tipo_Remision = ""
    '    End If
    '    If Tbl_Remision.Rows.Count = 0 Then
    '        'Alerta.Text = "*Remision no encontrada,verifique que sea correcta."
    '        Exit Sub
    '    End If
    '    Data_Remision = Tbl_Remision
    '    Complemetos()
    '    LoadC()
    '    'Response.Redirect("~/Traslado/ImprimirR.aspx")
    'End Sub
    'Sub Complemetos()
    '    Numeros_Env(CDbl(Num_Remision))
    '    Cant_M(cn.fn_ConsultaTraslado_GetMonedas(CDbl(Num_Remision)))
    '    Cant_Env(cn.fn_ConsultaTraslado_GetEnvases(CDbl(Num_Remision)))
    'End Sub
    'Sub Numeros_Env(Id_Remision As Double)
    '    Dim dt_Envases As DataTable = cn.fn_ConsultaTraslado_GetEnvases(Id_Remision)
    '    Envases_Remision = Nothing
    '    For Each rows In dt_Envases.Rows
    '        Envases_Remision += "[" + rows("Numero").ToString() + "]"
    '    Next
    'End Sub
    'Sub Cant_M(Dt As DataTable)
    '    Mon_Na = 0
    '    Mon_Ex = 0
    '    Mon_Otros = 0
    '    For Each Row As DataRow In Dt.Rows
    '        If (Row("Moneda").ToString() = "PESOS") Then
    '            Mon_Na += CDbl(Row("Efectivo").ToString())
    '            Mon_Otros += CDbl(Row("Documentos").ToString())
    '        ElseIf (Row("Moneda").ToString() = "DOLARES" Or Row("Moneda").ToString() = "ORO ONZA" Or Row("Moneda").ToString() = "EUROS" Or Row("Moneda").ToString() = "PLATA" Or Row("Moneda").ToString() = "ORO") Then
    '            Mon_Ex += (CDbl(Row("Efectivo").ToString()) * CDbl(Row("Tipo Cambio").ToString()))
    '            Mon_Otros += CDbl(Row("Documentos").ToString())
    '        End If
    '    Next
    'End Sub
    'Sub Cant_Env(Dt As DataTable)
    '    Env_B = 0
    '    Env_M = 0
    '    Env_MIX = 0
    '    For Each Row As DataRow In Dt.Rows
    '        If (Row("Tipo Envase").ToString() = "BILLETE") Then
    '            Env_B += 1
    '        ElseIf (Row("Tipo Envase").ToString() = "MIXTO") Then
    '            Env_MIX += 1
    '        ElseIf (Row("Tipo Envase").ToString() = "MORRALLA") Then
    '            Env_M += 1
    '        End If
    '    Next
    'End Sub
End Class