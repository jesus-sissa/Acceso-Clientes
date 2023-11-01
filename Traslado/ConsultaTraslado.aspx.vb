Imports System.IO

Partial Public Class ConsultaTraslado
    Inherits BasePage
    Dim dt_Puntos As DataTable
    Dim Id_Remision As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: CONSULTA DE TRASLADO")

        Call Limpiar()
        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()

        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If
        txt_FechaInicial.Text = Date.Now
        txt_FechaFinal.Text = Date.Now
        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

    End Sub

    Protected Sub gv_Lista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.SelectedIndexChanged
        ViewState("RutaIndice") = gv_Lista.SelectedIndex
        ViewState("RutaPagina") = gv_Lista.PageIndex
        '
        'dt_Puntos = pTabla("Lista")
        'If (IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0") Then
        '    'Clave_Ruta = dt_Puntos.Rows(gv_Lista.SelectedIndex)("Clave_Ruta")
        'End If
        'MsgBox(gv_Lista.SelectedIndex.ToString)
        Call ActualizarDetalles(gv_Lista.SelectedIndex)
    End Sub

    Protected Sub ActualizarDetalles(index As Integer)
        If IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        ' pnl_Tripulacion.Visible = True
        dt_Puntos = pTabla("Lista")
        Clave_Ruta = dt_Puntos.Rows(index)("Clave_Ruta")
        If gv_Lista.SelectedIndex = -1 Then
            gv_Remisiones.DataSource = fn_CreaGridVacio("Remision,Hora,Importe,Envases,EnvasesSN,Id_Remision")
            gv_Remisiones.DataBind()

            pnl_Tripulacion.Visible = False
            Exit Sub
        End If

        Dim IdPunto As Integer = Integer.Parse(gv_Lista.SelectedDataKey("Id_Punto"))
        pId_Punto = IdPunto
        Dim dt_tripulacion As DataTable = cn.fn_ValidacionTripulacion_GetNombres(IdPunto)

        'Realizar consulta para traer las Unidades

        If dt_tripulacion.Rows.Count = 0 Then
            fn_Alerta("No se puede mostrar la tripulación")
            pnl_Tripulacion.Visible = False
            Exit Sub
        End If

        lbl_Operador.Text = dt_tripulacion.Rows(0)("Operador")
        lbl_OperadorClave.Text = dt_tripulacion.Rows(0)("ClaveOperador")
        lbl_Cajero.Text = dt_tripulacion.Rows(0)("Cajero")
        lbl_CajeroClave.Text = dt_tripulacion.Rows(0)("ClaveCajero")

        img_Operador.ImageUrl = "~/Traslado/Foto.aspx?Id=" & dt_tripulacion.Rows(0)("Id_Operador")
        img_OperadorFirma.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt_tripulacion.Rows(0)("Id_Operador")
        img_Cajero.ImageUrl = "~/Traslado/Foto.aspx?Id=" & dt_tripulacion.Rows(0)("Id_Cajero")
        img_CajeroFirma.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt_tripulacion.Rows(0)("Id_Cajero")

        lbl_Unidad.Text = gv_Lista.SelectedRow.Cells(3).Text
        lbl_Placas.Text = gv_Lista.SelectedRow.Cells(6).Text
        Unidad_Ruta = gv_Lista.SelectedRow.Cells(3).Text
        img_Frente.ImageUrl = "~/Traslado/FotoUnidad.aspx?Id_Unidad=" & Integer.Parse(gv_Lista.SelectedDataKey("IDU"))

        dl_Custodios.DataSource = cn.fn_ValidacionTripulacion_GetCustodios(IdPunto)
        dl_Custodios.DataBind()

        Dim dt_Remisiones As DataTable = cn.fn_ConsultaTraslado_GetRemisiones(IdPunto)

        pTabla("Remisiones") = dt_Remisiones
        gv_Remisiones.DataSource = fn_MostrarSiempre(dt_Remisiones)
        gv_Remisiones.DataBind()
        ''
        Id_Cajero = dt_tripulacion.Rows(0)("Id_Cajero")
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click
        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If (Not Date.TryParse(txt_FechaInicial.Text, FechaInicial)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If (Not Date.TryParse(txt_FechaFinal.Text, FechaFinal)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        'If FechaInicial > Today() Or FechaFinal > Today() Then
        '    fn_Alerta("Debe seleccionar una fecha menor o igual que hoy.")
        '    Exit Sub
        'End If

        If FechaInicial > FechaFinal Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = 0 Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        fst_Traslados.Style.Remove("height")

        dt_Puntos = cn.fn_ConsultaTraslado_GetPuntos(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)
        If (dt_Puntos.Rows.Count > 0) Then
            btn_Exportar.Enabled = True
        Else
            btn_Exportar.Enabled = False
        End If
        pTabla("Lista") = dt_Puntos
        gv_Lista.SelectedIndex = -1

        pnl_Tripulacion.Visible = False

        gv_Lista.DataSource = fn_MostrarSiempre(dt_Puntos)
        gv_Lista.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_Traslados.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Lista_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        Dim dt_Traslados As DataTable = pTabla("Lista")
        Dim RutaIndice As Integer = ViewState("RutaIndice")
        Dim RutaPagina As Integer = ViewState("RutaPagina")

        If RutaPagina = e.NewPageIndex Then
            gv_Lista.SelectedIndex = RutaIndice
        Else
            gv_Lista.SelectedIndex = -1
        End If

        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = fn_MostrarSiempre(dt_Traslados)
        gv_Lista.DataBind()

        Call ActualizarDetalles(0)
        fst_Traslados.Style.Remove("height")

        If gv_Lista.Rows.Count < 4 Then
            fst_Traslados.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub gv_Remisiones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Remisiones.SelectedIndexChanged
        If IsDBNull(gv_Remisiones.DataKeys(0).Value) OrElse CStr(gv_Remisiones.DataKeys(0).Value) = "" OrElse CStr(gv_Remisiones.DataKeys(0).Value) = "0" Then Exit Sub

        Id_Remision = gv_Remisiones.SelectedDataKey("Id_Remision")
        'MsgBox(Id_Remision.ToString())
        Dim dt_Monedas As DataTable = cn.fn_ConsultaTraslado_GetMonedas(Id_Remision)
        pTabla("TablaMonedas") = dt_Monedas

        gvMonedas.DataSource = fn_MostrarSiempre(dt_Monedas)
        gvMonedas.DataBind()

        Dim dt_Envases As DataTable = cn.fn_ConsultaTraslado_GetEnvases(Id_Remision)
        pTabla("TablaEnvases") = dt_Envases
        gvEnvases.DataSource = fn_MostrarSiempre(dt_Envases)
        gvEnvases.DataBind()
    End Sub


    Protected Sub gv_Remisiones_DataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Remisiones.DataBinding
        gv_Remisiones.SelectedIndex = -1

        gvEnvases.DataSource = fn_CreaGridVacio("Id_Envase,Tipo Envase,Numero,IDTE")
        gvEnvases.DataBind()

        gvMonedas.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Efectivo,Documentos,Tipo Cambio")
        gvMonedas.DataBind()
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = 0

    End Sub

    Sub Limpiar()
        fst_Traslados.Style.Add("height", "205px")

        gv_Lista.DataSource = fn_CreaGridVacio("Id_Punto,Fecha,Unidad,Origen,Destino,Hora,Placas,IDU")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1

        gv_Remisiones.DataSource = fn_CreaGridVacio("Remision,Hora,Importe,Envases,EnvasesSN,Id_Remision")
        gv_Remisiones.DataBind()
        gv_Remisiones.SelectedIndex = -1

        gvMonedas.DataSource = fn_CreaGridVacio("Id_Moneda,Moneda,Efectivo,Documentos,Tipo Cambio")
        gvMonedas.DataBind()

        gvEnvases.DataSource = fn_CreaGridVacio("Id_Envase,Tipo Envase,Numero,IDTE")
        gvEnvases.DataBind()

        pnl_Tripulacion.Visible = False
    End Sub

    Protected Sub btn_Exportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Exportar.Click
        Dim FechaInicial As Date
        Dim FechaFinal As Date

        If (Not Date.TryParse(txt_FechaInicial.Text, FechaInicial)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If (Not Date.TryParse(txt_FechaFinal.Text, FechaFinal)) Then
            fn_Alerta("El rango de fechas parece ser incorrecto.")
            Exit Sub
        End If

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = 0 Then
                fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        Dim dt_temporal As DataTable = cn.fn_ConsultaTraslado_GetPuntos(FechaInicial, FechaFinal, ddl_Clientes.SelectedValue, cbx_Todos_Clientes.Checked, pNivel)

        If dt_temporal.Rows.Count = 0 Then
            fn_Alerta("No se encontraron elementos para exportar.")
            Exit Sub
        End If

        fn_Exportar_Excel(dt_temporal, Page.Title, "Desde: " & FechaInicial, "Hasta: " & FechaFinal, 1)
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FechaInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub txt_FechaFinal_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaFinal.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub gv_Remisiones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Remisiones.RowCommand
        If IsDBNull(gv_Remisiones.DataKeys(0).Value) OrElse CStr(gv_Remisiones.DataKeys(0).Value) = "" OrElse CStr(gv_Remisiones.DataKeys(0).Value) = "0" Then Exit Sub
        'Dim Indice As Integer = e.CommandArgument
        Select Case e.CommandName.ToUpper
            Case "VERDES"
                Num_Remision = gv_Remisiones.DataKeys(e.CommandArgument).Value
                Numeros_Env(Num_Remision)
                Cant_M(cn.fn_ConsultaTraslado_GetMonedas(Num_Remision))
                Cant_Env(cn.fn_ConsultaTraslado_GetEnvases(Num_Remision))

                'Num_Remision = gv_ConsultaMat.SelectedDataKey("Folio").ToString()
                'Num_Remision = gv_ConsultaMat.DataKeys(e.CommandArgument).Value
                'Num_Remision = gv_ConsultaMat.Rows(e.CommandArgument).Cells(1).Text
                Response.Redirect("ImprimirR.aspx")

            Case "ENVCORREO"

                Dim notificiones As DataTable = Cn_Portal.obtenerNotificacion(pId_Punto)
                Dim num_remision = gv_Remisiones.Rows(e.CommandArgument).Cells(1).Text
                '  im Query = dt.AsEnumerable.Where(Function(dr) dr("column name").ToString = "something").ToList
                Dim remisiones As DataTable = notificiones.AsEnumerable.Where(Function(r) r("Numero_Remision").ToString = num_remision).CopyToDataTable()
                For Each noti As DataRow In remisiones.Rows



                    Dim dtRemisionImporte As DataTable = Cn_Portal.obtenerRemisionWebImporte(noti("Numero_Remision"))
                    Dim dtEnvases As DataTable = Cn_Portal.obtenerEnvases(noti("Numero_Remision"))
                    Dim dtMonedas As DataTable = Cn_Portal.obtenerImporteMoneda(noti("Numero_Remision"))

                    Dim envases As String = Cn_Portal.obtenerEnvases(dtEnvases)
                    Dim cantEnvaseBillete As Integer = Cn_Portal.obtenerEnvaseMoneda(dtEnvases)
                    Dim cantEnvaseMixto As Integer = Cn_Portal.obtenerEnvaseMixto(dtEnvases)
                    Dim cantEnvaseMorr As Integer = Cn_Portal.obtenerEnvaseMorralla(dtEnvases)

                    Dim impPesos As Decimal = Cn_Portal.obtenerMonenadaNacional(dtMonedas)
                    Dim impExtranjero As Decimal = Cn_Portal.obtenerMonenadaExtranjera(dtMonedas)
                    Dim impDoctos As Decimal = Cn_Portal.obtenerDocumentos(dtMonedas)

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

                    Dim pdf As MemoryStream = RemisionDigital.Class1.crearPDF(noti("Numero_Remision").ToString(), noti("Fecha").ToString(), noti("Hora").ToString(),
                                                                   noti("Envases").ToString() + "+ " + noti("EnvasesSN").ToString() + " S/N", envases, Convert.ToString(impDoctos + impExtranjero + impPesos), FuncionesGlobales.FuncionesGlobales.fn_EnLetras((impDoctos + impExtranjero + impPesos).ToString()),
                                                                   noti("NombreClienteOrigen").ToString(), noti("ClaveClienteOrigen").ToString(), noti("DireccionOrigen").ToString(),
                                                                   noti("NombreClienteDestino").ToString(), noti("DireccionDestino").ToString(), noti("Clave_Ruta").ToString(),
                                                                   noti("CiaTraslada").ToString(), noti("Unidad").ToString(), noti("Cajero").ToString(),
                                                                   noti("UsuarioClienteFirma").ToString(), Convert.ToString(impPesos), Convert.ToString(impExtranjero), Convert.ToString(impDoctos),
                                                                   cantEnvaseBillete.ToString(), cantEnvaseMorr.ToString(), cantEnvaseMixto.ToString(),
                                                                   dtRemisionImporte.Rows(0)("Mil").ToString(), dtRemisionImporte.Rows(0)("Quinientos").ToString(), dtRemisionImporte.Rows(0)("Docientos").ToString(),
                                                                   dtRemisionImporte.Rows(0)("Cien").ToString(), dtRemisionImporte.Rows(0)("Cincuenta").ToString(), dtRemisionImporte.Rows(0)("Veinte").ToString(),
                                                                   dtRemisionImporte.Rows(0)("MVeinte").ToString(), dtRemisionImporte.Rows(0)("MDiez").ToString(), dtRemisionImporte.Rows(0)("MCinco").ToString(),
                                                                   dtRemisionImporte.Rows(0)("MDos").ToString(), dtRemisionImporte.Rows(0)("MUno").ToString(), dtRemisionImporte.Rows(0)("MPCincuenta").ToString(),
                                                                   dtRemisionImporte.Rows(0)("MPVeinte").ToString(), dtRemisionImporte.Rows(0)("MPDiez").ToString(), dtRemisionImporte.Rows(0)("MPCinco").ToString(), noti("Comentarios").ToString())


                    Dim Ecorreo As New cn_mail()

                    Ecorreo.adjuntarArchivo(pdf, "Remision_" + num_remision + ".pdf")
                    Ecorreo.Mensaje = "Para mas informacion visite el sitio web  :https://www.sissaseguridad.com/AccesoClientes "
                    Ecorreo.Asunto = "Comprobante de servicio: " + num_remision.ToString
                    Dim destinatario = New DataTable()
                    destinatario.Columns.Add("Mail")

                    destinatario.Rows.Add(New Object() {pMail_Usuario})
                    Ecorreo.Destinatarios = destinatario
                    If Ecorreo.Enviar() Then
                        fn_Alerta("Remision enviada correctamente")
                    End If
                Next

        End Select
    End Sub
    Sub Numeros_Env(Id_Remision As Integer)
        Tipo_Remision = "TRAS"
        Dim dt_Envases As DataTable = cn.fn_ConsultaTraslado_GetEnvases(Id_Remision)
        Envases_Remision = Nothing
        For Each rows In dt_Envases.Rows
            Envases_Remision += "[" + rows("Numero").ToString() + "]"
        Next
    End Sub
    Sub Cant_M(Dt As DataTable)
        Mon_Na = 0
        Mon_Ex = 0
        Mon_Otros = 0
        For Each Row As DataRow In Dt.Rows
            If (Row("Moneda").ToString() = "PESOS") Then
                Mon_Na += CDbl(Row("Efectivo").ToString())
                Mon_Otros += CDbl(Row("Documentos").ToString())
            ElseIf (Row("Moneda").ToString() = "DOLARES" Or Row("Moneda").ToString() = "ORO ONZA" Or Row("Moneda").ToString() = "EUROS" Or Row("Moneda").ToString() = "PLATA" Or Row("Moneda").ToString() = "ORO") Then
                Mon_Ex += (CDbl(Row("Efectivo").ToString()) * CDbl(Row("Tipo Cambio").ToString()))
                Mon_Otros += CDbl(Row("Documentos").ToString())
            End If
        Next
    End Sub

    Sub Cant_Env(Dt As DataTable)
        Env_B = 0
        Env_M = 0
        Env_MIX = 0
        For Each Row As DataRow In Dt.Rows
            If (Row("Tipo Envase").ToString() = "BILLETE") Then
                Env_B += 1
            ElseIf (Row("Tipo Envase").ToString() = "MIXTO") Then
                Env_MIX += 1
            ElseIf (Row("Tipo Envase").ToString() = "MORRALLA") Then
                Env_M += 1
            End If
        Next
    End Sub

    Protected Sub cbx_tripulacion_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_tripulacion.CheckedChanged
        If (cbx_tripulacion.Checked = True) Then
            pnl_Tripulacion.Visible = True
            tripulacion.Visible = True
        Else
            pnl_Tripulacion.Visible = False
            tripulacion.Visible = False
        End If
    End Sub
End Class