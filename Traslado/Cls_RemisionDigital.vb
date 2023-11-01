Imports System.Data
Imports SiacMovilModel
Imports PortalSIAC.Cn_Portal
Imports System.IO
Imports System.Net.Mail
Imports PortalSIAC.cn_Datos
Public Class Cls_RemisionDigital
    Public Function insertarRemisionDigital(ByVal model As RemisionRequest) As UserResponse
        Dim userResponse As UserResponse = New UserResponse()

        If model Is Nothing Then
            userResponse.TipoRespuesta = Tools.Respuesta.ErrorDeValidacion
            Return userResponse
        End If


        Dim tr As System.Data.SqlClient.SqlTransaction = CreaTransaccion(CreaConexion())

        Try

            If EstatusPuntos(New PuntoRequest With {
                .IdPunto = model.PuntoRemision.IdPunto,
                .ClaveSucursal = model.ClaveSucursal
            }).Rows(0)("Status").ToString() <> "A" Then
                userResponse.TipoRespuesta = Tools.Respuesta.PuntoNoDisponible
                Return userResponse
            End If

            If Not consultarOrigenDestino(model) Then
                Throw New Exception("No se puede determinar el origen y destino de la remisión.")
            End If

            If Convert.ToBoolean(ExisteRemision(model)) Then
                userResponse.TipoRespuesta = Tools.Respuesta.RemisionDuplicada
                Return userResponse
            End If

            Dim Coments As String = Obtener_Comentarios(model)
            model.Remision.IdRemision = insertar(model, tr)
            insertarRemisionD(model, tr)
            insertarFirmas(model, tr)
            insertarEnvases(model, tr)
            EditarPuntoRemision(model, tr)
            EditarCantidadRemision(model, tr)
            terminarRemisionWeb(model, Coments, tr)
            Dim dtMovimiento As DataTable = obtenerMovimientos(model, tr)
            If dtMovimiento.Rows.Count = 0 Then Throw New Exception("No se pudo insertar el movimiento.")
            Dim movimientoService As CalculoMovimientoService = New CalculoMovimientoService(dtMovimiento)
            Dim calculoMovimiento As CalculoMovimiento = New CalculoMovimiento With {
                .Importe = movimientoService.calcularImporte(),
                .Kilometros = movimientoService.calcularKilometros(),
                .EnvaseExceso = movimientoService.calcularEnvasesExceso(),
                .Miles = movimientoService.calcularMiles()
            }
            Dim IdMovimiento As Decimal = insertar(model, tr, dtMovimiento, calculoMovimiento)
            If IdMovimiento = 0 Then Throw New Exception("No se pudo insertar el detalle del movimiento.")
            insertarD(model, tr, IdMovimiento)

            If Convert.ToBoolean(model.CerrarPunto) Then
                enRuta(model, tr)
            End If

            Dim punto As DataTable = obtenerPuntoUtilizaCasetRemisionDigital(New PuntoRequest With {
                .ClaveSucursal = model.ClaveSucursal,
                .IdPunto = model.PuntoRemision.IdPunto
            }, tr)
            tr.Commit()

            If model.CerrarPunto = 1 Then
                Dim notificiones As DataTable = obtenerNotificacion(model)

                For Each noti As DataRow In notificiones.Rows
                    model.Remision = New Remision With {
                        .NumeroRemision = Convert.ToDecimal(noti("Numero_Remision"))
                    }
                    Dim dtRemisionImporte As DataTable = obtenerRemisionWebImporte(model)
                    Dim dtEnvases As DataTable = obtenerEnvasess(model)
                    Dim dtMonedas As DataTable = obtenerImporteMoneda(model)
                    Dim envases As String = obtenerEnvases(dtEnvases)
                    Dim cantEnvaseBillete As Integer = obtenerEnvaseMoneda(dtEnvases)
                    Dim cantEnvaseMixto As Integer = obtenerEnvaseMixto(dtEnvases)
                    Dim cantEnvaseMorr As Integer = obtenerEnvaseMorralla(dtEnvases)
                    Dim impPesos As Decimal = obtenerMonenadaNacional(dtMonedas)
                    Dim impExtranjero As Decimal = obtenerMonedaExtranjera(dtMonedas)
                    Dim impDoctos As Decimal = obtenerDocumentos(dtMonedas)

                    If dtRemisionImporte.Rows.Count = 0 Then
                        Dim dr As DataRow = dtRemisionImporte.NewRow()
                        dr("Mil") = 0
                        dr("Cien") = 0
                        dr("MVeinte") = 0
                        dr("MDos") = 0
                        dr("MPVeinte") = 0
                        dr("Quinientos") = 0
                        dr("Cincuenta") = 0
                        dr("MDiez") = 0
                        dr("MUno") = 0
                        dr("MPDiez") = 0
                        dr("Docientos") = 0
                        dr("Veinte") = 0
                        dr("MCinco") = 0
                        dr("MPCincuenta") = 0
                        dr("MPCinco") = 0
                        dr("Id_RemisionesWebImportes") = 0
                        dr("Id_Remision") = 0
                        dr("Id_RemisionReal") = 0
                        dtRemisionImporte.Rows.Add(dr)
                    End If

                    Dim stream As MemoryStream = RemisionDigital.Class1.crearPDF(noti("Numero_Remision").ToString(), noti("Fecha").ToString(), noti("Hora").ToString(), noti("Envases").ToString() & "+ " + noti("EnvasesSN").ToString() & " S/N", envases, Convert.ToString(impDoctos + impExtranjero + impPesos), fn_EnLetras((impDoctos + impExtranjero + impPesos).ToString()), noti("NombreClienteOrigen").ToString(), noti("ClaveClienteOrigen").ToString(), noti("DireccionOrigen").ToString(), noti("NombreClienteDestino").ToString(), noti("DireccionDestino").ToString(), noti("Clave_Ruta").ToString(), noti("CiaTraslada").ToString(), noti("Unidad").ToString(), noti("Cajero").ToString(), noti("UsuarioClienteFirma").ToString(), Convert.ToString(impPesos), Convert.ToString(impExtranjero), Convert.ToString(impDoctos), cantEnvaseBillete.ToString(), cantEnvaseMorr.ToString(), cantEnvaseMixto.ToString(), dtRemisionImporte.Rows(0)("Mil").ToString(), dtRemisionImporte.Rows(0)("Quinientos").ToString(), dtRemisionImporte.Rows(0)("Docientos").ToString(), dtRemisionImporte.Rows(0)("Cien").ToString(), dtRemisionImporte.Rows(0)("Cincuenta").ToString(), dtRemisionImporte.Rows(0)("Veinte").ToString(), dtRemisionImporte.Rows(0)("MVeinte").ToString(), dtRemisionImporte.Rows(0)("MDiez").ToString(), dtRemisionImporte.Rows(0)("MCinco").ToString(), dtRemisionImporte.Rows(0)("MDos").ToString(), dtRemisionImporte.Rows(0)("MUno").ToString(), dtRemisionImporte.Rows(0)("MPCincuenta").ToString(), dtRemisionImporte.Rows(0)("MPVeinte").ToString(), dtRemisionImporte.Rows(0)("MPDiez").ToString(), dtRemisionImporte.Rows(0)("MPCinco").ToString(), noti("Comentarios").ToString())
                    Dim ECorreo As Correo = New Correo()
                    Dim sucursal As SucursalService = New SucursalService(New Alertaa With {
                        .ClaveSucursal = "01",
                        .IdSucursal = "1"
                    })
                    Dim correoService As CorreoService = New CorreoService(sucursal) With {
                        .AdjuntaArchivo_ = True,
                        .EsHTML_ = True
                    }
                    correoService.adjuntarArchivoo(stream, "COMPROBANTE_REMISION.pdf")
                    Dim destinatario As DataTable = New DataTable()
                    destinatario.Columns.Add("Mail")
                    destinatario.Rows.Add(New Object() {"facturacionsissa@gmail.com"})
                    destinatario.Rows.Add(New Object() {model.ClienteSesion.Mail})
                    correoService.Destinatarios(destinatario)
                    ECorreo.Mensaje = "Para mas informacion visite el sitio web  :https://www.sissaseguridad.com/AccesoClientes "
                    ECorreo.Asunto = ("Comprobante de servicio:" & " " & model.Remision.NumeroRemision & " " + noti("NombreClienteOrigen").ToString() & " " + noti("ClaveClienteOrigen").ToString() & " " + noti("Fecha").ToString())
                    correoService.enviar(ECorreo)
                Next
            End If
        Catch ex As Exception
            tr.Rollback()
            'LogRepository.insertar("InsertarRemisionDigital", "0", ex.Message, model.ClaveSucursal, model.IdSucursal, model.IdUsuario, model.EstacionNombre)
            userResponse.TipoRespuesta = Tools.Respuesta.ErrordelServidor
            userResponse.Descripcion = ex.Message
            Return userResponse
        End Try

        userResponse.TipoRespuesta = Tools.Respuesta.OperacionCorrecta
        Return userResponse
    End Function
    Public Shared Function obtenerEnvases(ByVal dtEnvases As DataTable) As String
        Dim envases As String = String.Empty

        For Each envase As DataRow In dtEnvases.Rows
            envases += "[" & envase("Numero").ToString() & "]"
        Next

        Return envases
    End Function

    Public Shared Function obtenerEnvaseMoneda(ByVal dtEnvases As DataTable) As Integer
        Return (From envase In dtEnvases.AsEnumerable() Where CStr(envase("Tipo Envase")) = "BILLETE" Select envase).Count()
    End Function

    Public Shared Function obtenerEnvaseMixto(ByVal dtEnvases As DataTable) As Integer
        Return (From envase In dtEnvases.AsEnumerable() Where CStr(envase("Tipo Envase")) = "MIXTO" Select envase).Count()
    End Function

    Public Shared Function obtenerEnvaseMorralla(ByVal dtEnvases As DataTable) As Integer
        Return (From envase In dtEnvases.AsEnumerable() Where CStr(envase("Tipo Envase")) = "MORRALLA" Select envase).Count()
    End Function
    Public Shared Function obtenerMonenadaNacional(ByVal datos As DataTable) As Decimal
        Dim monedaNacional As Decimal = 0

        For Each moneda As DataRow In datos.Rows
            If moneda("Moneda").ToString() = "PESOS" Then monedaNacional += Convert.ToDecimal(moneda("Efectivo"))
        Next

        Return monedaNacional
    End Function

    Public Shared Function obtenerMonedaExtranjera(ByVal datos As DataTable) As Decimal
        Dim monedaExt As Decimal = 0

        For Each moneda As DataRow In datos.Rows
            If moneda("Moneda").ToString() <> "PESOS" Then monedaExt += Convert.ToDecimal(moneda("Efectivo")) * Convert.ToDecimal(moneda("Tipo Cambio"))
        Next

        Return monedaExt
    End Function

    Public Shared Function obtenerDocumentos(ByVal datos As DataTable) As Decimal
        Dim doc As Decimal = 0

        For Each moneda As DataRow In datos.Rows
            doc += Convert.ToDecimal(moneda("Documentos"))
        Next

        Return doc
    End Function
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
        If Len(deci) >= 1 Then
            If Len(deci) = 1 Then deci = deci & "0"
            If Len(deci) > 2 Then deci = Left(deci, 2)
            deci = deci & "/100."
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
                    If Mid(entero, 6, 1) <> "0" Or Mid(entero, 5, 1) <> "0" Or Mid(entero, 4, 1) <> "0" Or
                        (Mid(entero, 6, 1) = "0" And Mid(entero, 5, 1) = "0" And Mid(entero, 4, 1) = "0" And
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

                If paso = 7 And valor = "MILIARDOS" And cifra = 10 And
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
End Class

Public Class CorreoService


    Private _sucursal As SucursalService
    Private _destinatarios As DataTable
    Private esHtml As Boolean = False
    Private adjuntaArchivo As Boolean = False
    Private _message As System.Net.Mail.MailMessage
    Private _memory As MemoryStream
    Private _nombreArchivo As String

    Public Property EsHTML_ As Boolean
        Set(ByVal value As Boolean)
            esHtml = value
        End Set
        Get
            Return esHtml
        End Get
    End Property

    Public Property AdjuntaArchivo_ As Boolean
        Set(ByVal value As Boolean)
            adjuntaArchivo = value
        End Set
        Get
            Return adjuntaArchivo
        End Get
    End Property

    Public Sub adjuntarArchivo(ByVal message As System.Net.Mail.MailMessage)
        _message = message
    End Sub

    Public Sub New(ByVal sucursal As SucursalService)
        _sucursal = sucursal
    End Sub

    Public Sub Destinatarios(ByVal dt As DataTable)
        _destinatarios = dt
    End Sub

    Public Function enviar(ByVal model As Correo) As Boolean
        Dim smtp As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient()
        Dim correo As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()

        Try
            correo.From = New System.Net.Mail.MailAddress(_sucursal.Sucursal.MailRemitente, _sucursal.Sucursal.MailRemitenteNombre)
            correo.Subject = model.Asunto
            correo.Body = model.Mensaje
            correo.IsBodyHtml = esHtml

            If adjuntaArchivo Then
                Dim attachment As Attachment = New Attachment(_memory, _nombreArchivo)
                correo.Attachments.Add(attachment)
            End If

            smtp.Host = _sucursal.Sucursal.Host
            smtp.UseDefaultCredentials = False
            smtp.Credentials = New System.Net.NetworkCredential(_sucursal.Sucursal.MailUser, _sucursal.Sucursal.MailClave)
            smtp.EnableSsl = _sucursal.usaSSL()
            If _sucursal.Sucursal.Puerto > 0 Then smtp.Port = _sucursal.Sucursal.Puerto
            smtp.Timeout = _sucursal.Sucursal.TimeOut

            For Each destino As DataRow In _destinatarios.Rows
                If destino("Mail").ToString() <> "" Then correo.[To].Add(destino("Mail").ToString())
            Next

            smtp.Send(correo)
        Catch ex As Exception
            correo.Dispose()

            If adjuntaArchivo Then
                _memory.Close()
            End If

            Throw ex
        End Try

        correo.Dispose()
        smtp.Dispose()

        If adjuntaArchivo Then
            _memory.Close()
        End If

        Return True
    End Function

    Public Sub adjuntarArchivoo(ByVal stream As MemoryStream, ByVal nombreArchivo As String)
        _memory = stream
        _nombreArchivo = nombreArchivo
    End Sub
End Class

Public Class SucursalService
    Private _sucursal As DataTable
    Private smtp As Smtp
    Public cn As Cn_Portal

    Public Sub New(ByVal model As Alertaa)
        Try
            _sucursal = cn.obtenerSucursal()
            sucursall()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub sucursall()
        Try
            If Not validar() Then Throw New Exception("paramétros sucursal no validos")
            smtp = New Smtp With {
                .Host = _sucursal.Rows(0)("Mail_Server").ToString(),
                .MailUser = _sucursal.Rows(0)("Mail_User").ToString(),
                .MailClave = decodificar(_sucursal.Rows(0)("Mail_Clave").ToString()),
                .MailRemitente = _sucursal.Rows(0)("Mail_Remitente").ToString(),
                .MailRemitenteNombre = _sucursal.Rows(0)("Mail_RemitenteNombre").ToString(),
                .TimeOut = Convert.ToInt32(_sucursal.Rows(0)("Mail_TiempoEspera").ToString()) * 1000,
                .Puerto = Convert.ToInt32(_sucursal.Rows(0)("Mail_Puerto").ToString())
            }
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public ReadOnly Property Sucursal As Smtp
        Get
            Return smtp
        End Get
    End Property

    Private Function validar() As Boolean
        If _sucursal.Rows.Count = 0 Then Return False
        If _sucursal.Rows(0)("Mail_Server").ToString() = "" OrElse _sucursal.Rows(0)("Mail_User").ToString() = "Mail_Clave" OrElse _sucursal.Rows(0)("Mail_Remitente").ToString() = "" OrElse _sucursal.Rows(0)("Mail_RemitenteNombre").ToString() = "" Then Return False
        Return True
    End Function

    Public Function usaSSL() As Boolean
        Return _sucursal.Rows(0)("Mail_UsarSSL").ToString() = "S"
    End Function
    Public Shared Function decodificar(ByVal valor As String) As String
        Dim byteValor As Byte() = System.Convert.FromBase64String(valor)
        Return System.Text.Encoding.UTF8.GetString(byteValor)
    End Function

    Public Shared Function codificar(ByVal valor As String) As String
        Dim byteValor As Byte() = System.Text.Encoding.UTF8.GetBytes(valor)
        Return System.Convert.ToBase64String(byteValor)
    End Function
End Class
Public Class Smtp
    Public Property Host As String
    Public Property MailUser As String
    Public Property MailClave As String
    Public Property MailRemitente As String
    Public Property MailRemitenteNombre As String
    Public Property TimeOut As Integer
    Public Property Puerto As Integer
End Class

Public Class Alertaa
    Inherits BaseUserRequest

    Public Property ClaveAlerta As String
    Public Property IdCliente As Decimal
    Public Property ModoAlerta As Int32
    Public Property AlertaRecoleccion As String
    Public Property AlertaEntrega As String
End Class

Public Class AlertaRemision
    Inherits Alertaa

    Public Property CantidadRemision As Int32
    Public Property CantidadEnvase As Int32
    Public Property Observaciones As String
    Public Property NombrePunto As String
    Public Property TipoAlerta As String
End Class

Public Class AlertaComprobanteVisita
    Inherits Alertaa

    Public Property TipoAlerta As String
    Public Property HoraSalida As String
    Public Property NombrePunto As String
    Public Property Hora As String
    Public Property Folio As String
    Public Property HoraLlegada As String
    Public Property Comentarios As String
End Class

Public Class AlertaFallas
    Inherits Alertaa

    Public Property Unidad As String
    Public Property Ruta As String
    Public Property Operador As String
    Public Property Cajero As String
    Public Property Parte As String
    Public Property Falla As String
    Public Property Observaciones As String
    Public Property TipoProceso As Integer
    Public Property Asunto As String
    Public Property Mensaje As String
End Class

Public Class Correo
    Public Property Asunto As String
    Public Property Mensaje As String
End Class
Public Class CalculoMovimientoService
    Private dtMovimiento As DataTable = Nothing

    Public Sub New()
    End Sub

    Public Sub New(ByVal moviento As DataTable)
        dtMovimiento = moviento
    End Sub

    Private Function esValido(ByVal dt As DataTable) As Boolean
        If dtMovimiento Is Nothing Then Return False
        Return Convert.ToBoolean(dtMovimiento.Rows.Count)
    End Function

    Public Function calcularMiles() As Decimal
        Dim miles As Decimal = 0

        If esValido(dtMovimiento) Then

            If Convert.ToDecimal(dtMovimiento.Rows(0)("Id_CR")) > 0 Then

                If dtMovimiento.Rows(0)("Cobra_Documentos").ToString() = "N" Then
                    miles = Convert.ToDecimal(dtMovimiento.Rows(0)("Efectivo"))
                Else
                    miles = calcularImporte()
                End If
            End If

            If Convert.ToDecimal(dtMovimiento.Rows(0)("CantidadXunidad")) > 0 Then miles = (miles / Convert.ToDecimal(dtMovimiento.Rows(0)("CantidadXunidad")))
            If dtMovimiento.Rows(0)("Redondeado").ToString() = "S" AndAlso (miles Mod 1) > 0 Then miles = Math.Truncate(miles) + 1
            miles = (miles - Convert.ToDecimal(dtMovimiento.Rows(0)("Miles_Scosto")))
            If miles < 0 Then miles = 0
            Return miles
        End If

        Return 0
    End Function

    Public Function calcularKilometros() As Decimal
        Dim kilometraje As Decimal = 0
        If Convert.ToDecimal(dtMovimiento.Rows(0)("Id_KM")) > 0 Then kilometraje = Convert.ToDecimal(dtMovimiento.Rows(0)("Cantidad_KM"))
        Return kilometraje
    End Function

    Public Function calcularImporte() As Decimal
        Dim importe As Decimal = 0
        If esValido(dtMovimiento) Then importe = (Convert.ToDecimal(dtMovimiento.Rows(0)("Efectivo")) + Convert.ToDecimal(dtMovimiento.Rows(0)("Documentos")))
        Return importe
    End Function

    Public Function calcularEnvasesExceso() As Decimal
        Dim envaseExceso As Decimal = 0
        If Convert.ToDecimal(dtMovimiento.Rows(0)("Id_EE")) > 0 Then envaseExceso = Convert.ToDecimal(dtMovimiento.Rows(0)("Envases_Scosto")) - Convert.ToDecimal(dtMovimiento.Rows(0)("Cantidad_Envases"))
        Return envaseExceso
    End Function
End Class
