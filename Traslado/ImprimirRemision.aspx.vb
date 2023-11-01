Imports SiacMovilModel
Imports PortalSIAC.Cls_Remision

Public Class ImprimirRemision
    Inherits BasePage
    Dim Indice As Integer
    Dim TOKEN As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''ClientScript.RegisterStartupScript(GetType(),"h","codigo.barcode ( "1234567890128" ,  "ean13" );  )
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->
        If IsPostBack Then Exit Sub
        'pId_Punto = cn.fn_PuntosActivosxC()
        'If pId_Punto = 0 Then
        '    lbl_puntos.Text = "</br> NO EXISTE VISITA PROGRAMADA POR PARTE DE SISSA."
        '    Exit Sub
        'End If
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: GENERAR REMISIONES")
        If Not Obtener_puntos() Then
            Exit Sub
        End If
        ''Monedas combo
        pTabla("Tipo_cambio") = cn.fn_ConsultaTipoCambio()
        Cargar_data()
        '
        '
        '          not
    End Sub

    Function Obtener_puntos() As Boolean
        Dim dt_Solicitudes As DataTable = cn.fn_PuntosActivosxC()
        If dt_Solicitudes Is Nothing Then
            'gv_puntos.DataSource = fn_CreaGridVacio("Id_Punto,Cliente_Origen,Cliente_Destino,Remisiones,Descripcion,Clave_Cliente")
            gv_puntos.DataSource = fn_CreaGridVacio("Id_Punto,Cliente_Origen,Cliente_Destino,Remisiones,Descripcion,Clave_Cliente")
            gv_puntos.DataBind()
            Return False
        Else
            gv_puntos.DataSource = fn_MostrarSiempre(dt_Solicitudes)
            gv_puntos.DataBind()
            pTabla("tbl_puntos") = dt_Solicitudes
        End If
        gv_puntos.SelectedIndex = -1
        Return True
    End Function
    Sub Cargar_data()
        Dim dt_MonedasT As DataTable = cn.fn_ConsultaMonrdas()
        If dt_MonedasT Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        Call fn_LlenarDropDown(ddl_monedas, dt_MonedasT, False)
        '
        'Envases combo
        Dim dt_EnvasesT As DataTable = cn.fn_ConsultaEnvases()

        If dt_EnvasesT Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        Call fn_LlenarDropDown(ddl_envases, dt_EnvasesT, False)
        '

        Dim dt_monedas As DataTable = CreaTablaMonedas()
        pTabla("tbl_monedas") = dt_monedas
        gv_Monedas.DataSource = fn_MostrarSiempre(dt_monedas)
        gv_Monedas.DataBind()
        '
        Dim dt_envases As DataTable = CreaTablaEnvases()
        pTabla("tbl_envases") = dt_envases
        gv_Envases.DataSource = fn_MostrarSiempre(dt_envases)
        gv_Envases.DataBind()
    End Sub
    Private Function CreaTablaMonedas() As DataTable
        Dim dt_monedas As New DataTable
        dt_monedas = New Data.DataTable
        dt_monedas.Columns.Add("Id_Moneda", GetType(Integer))
        dt_monedas.Columns.Add("Moneda", GetType(String))
        dt_monedas.Columns.Add("Efectivo", GetType(String))
        dt_monedas.Columns.Add("Documentos", GetType(String))
        'dt_monedas.Columns.Add("Cantidad", GetType(Integer))
        'dt_monedas.Columns.Add("IdCS", GetType(Integer))
        'dt_monedas.Columns.Add("Precio", GetType(Decimal))
        Return dt_monedas
    End Function
    Private Function CreaTablaEnvases() As DataTable
        Dim dt_envases As New DataTable
        dt_envases = New Data.DataTable
        dt_envases.Columns.Add("Id_TipoE", GetType(Integer))
        dt_envases.Columns.Add("TipoEnvase", GetType(String))
        dt_envases.Columns.Add("Numero", GetType(String))
        'dt_envases.Columns.Add("Material", GetType(String))
        'dt_envases.Columns.Add("Cantidad", GetType(Integer))
        'dt_envases.Columns.Add("IdCS", GetType(Integer))
        'dt_envases.Columns.Add("Precio", GetType(Decimal))
        Return dt_envases
    End Function
    Private Function CreaTablaPuntos() As DataTable
        Dim dt_envases As New DataTable
        dt_envases = New Data.DataTable
        dt_envases.Columns.Add("Id_Punto", GetType(Integer))
        dt_envases.Columns.Add("Cliente", GetType(String))
        dt_envases.Columns.Add("Destino", GetType(String))
        dt_envases.Columns.Add("Remiciones", GetType(String))
        Return dt_envases
    End Function

    Protected Sub Btn_Agregar_Click(sender As Object, e As EventArgs) Handles Btn_Agregar.Click
        'Dim dt_monedas As DataTable = pTabla("tbl_monedas")
        'If dt_monedas Is Nothing Then
        '    dt_monedas = New Data.DataTable
        '    dt_monedas = CreaTablaMonedas()
        'End If
        'If Not Validar_monedas() Then Exit Sub
        'For Each dr_Existe As DataRow In dt_monedas.Rows
        '    If dr_Existe("Id_Moneda") = ddl_monedas.SelectedValue Then
        '        fn_Alerta("No se puede agragar la moneda porque ya ha sido agregada.")
        '        Exit Sub
        '    End If
        'Next
        'Dim dr_Agregar As DataRow = dt_monedas.NewRow()
        'dr_Agregar("Id_Moneda") = ddl_monedas.SelectedValue
        'dr_Agregar("Moneda") = ddl_monedas.SelectedItem.Text
        'dr_Agregar("Efectivo") = Tbx_Efectivo.Text
        'dr_Agregar("Documentos") = Tbx_Documentos.Text
        'dt_monedas.Rows.Add(dr_Agregar)

        'pTabla("tbl_monedas") = dt_monedas
        'gv_Monedas.DataSource = dt_monedas
        'gv_Monedas.DataBind()
        'ddl_monedas.SelectedValue = 0
        'Tbx_Efectivo.Text = Nothing
        'Tbx_Documentos.Text = 0
        Dim x, y As String
        Dim dt_monedas As DataTable = pTabla("tbl_monedas")
        If dt_monedas Is Nothing Then
            dt_monedas = New Data.DataTable
            dt_monedas = CreaTablaMonedas()
        End If
        If Not Validar_monedas() Then Exit Sub
        For Each dr_Existe As DataRow In dt_monedas.Rows
            If dr_Existe("Id_Moneda") = ddl_monedas.SelectedValue Then
                dr_Existe("Efectivo") = CDbl(Tbx_Efectivo.Text) + CDbl(dr_Existe("Efectivo"))
                dr_Existe("Documentos") = CDbl(Tbx_Documentos.Text) + CDbl(dr_Existe("Documentos"))
                Llenar(dt_monedas)
                Exit Sub
            End If
        Next

        Dim dr_Agregar As DataRow = dt_monedas.NewRow()
        dr_Agregar("Id_Moneda") = ddl_monedas.SelectedValue
        dr_Agregar("Moneda") = ddl_monedas.SelectedItem.Text
        dr_Agregar("Efectivo") = Tbx_Efectivo.Text
        dr_Agregar("Documentos") = Tbx_Documentos.Text
        dt_monedas.Rows.Add(dr_Agregar)
        Llenar(dt_monedas)
    End Sub
    Function Llenar(datos As DataTable)
        pTabla("tbl_monedas") = datos
        gv_Monedas.DataSource = datos
        gv_Monedas.DataBind()
        ddl_monedas.SelectedValue = 0
        Tbx_Efectivo.Text = Nothing
        Tbx_Documentos.Text = 0
    End Function
    Function Validar_monedas() As Boolean
        If ddl_monedas.SelectedValue = "0" Then
            fn_Alerta("Debe Seleccionar una moneda")
            Return False
        End If
        If Tbx_Efectivo.Text = "0" Then
            fn_Alerta("Debe capturar una cantidad diferente a 0")
            Return False
        End If
        If Not IsNumeric(Tbx_Efectivo.Text) Then
            fn_Alerta("Debe capturar una cantidad de efectivo valida")
            Return False
        End If
        If Not IsNumeric(Tbx_Documentos.Text) Then
            fn_Alerta("Debe capturar una cantidad de documentos valida")
            Return False
        End If
        Return True
    End Function
    Function Validar_Envases() As Boolean
        If ddl_envases.SelectedValue = "0" Then
            fn_Alerta("Debe Seleccionar un envase")
            Return False
        End If
        If Tbx_Numero.Text.Trim = "" Then
            fn_Alerta("Debe capturar un numero de evase valido")
            Return False
        End If
        'If Not IsNumeric(Tbx_Documentos.Text) Then
        '    fn_Alerta("Debe capturar una cantidad de documentos valida")
        '    Return False
        'End If
        Return True
    End Function

    Protected Sub gv_Monedas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Monedas.SelectedIndexChanged
        If IsDBNull(gv_Monedas.DataKeys(0).Value) OrElse CStr(gv_Monedas.DataKeys(0).Value) = "" OrElse CStr(gv_Monedas.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_MonedasAgregar As DataTable = pTabla("tbl_monedas")
        If dt_MonedasAgregar Is Nothing Then Return
        Dim Id_Inventario As Integer = gv_Monedas.SelectedDataKey("Id_Moneda")

        'Dim filas As DataRow() = dt_MonedasAgregar.Select("Id_Inventario = " & Id_Inventario)

        For Each dr As DataRow In dt_MonedasAgregar.Rows
            If dr("Id_Moneda") = Id_Inventario Then
                dt_MonedasAgregar.Rows.Remove(dr)
                Exit For
            End If
        Next

        pTabla("tbl_monedas") = dt_MonedasAgregar
        gv_Monedas.DataSource = fn_MostrarSiempre(dt_MonedasAgregar)
        gv_Monedas.DataBind()
    End Sub

    Protected Sub gv_Monedas_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv_Monedas.RowDataBound
        gv_Monedas.SelectedIndex = -1
    End Sub

    Protected Sub Btn_AgregarE_Click(sender As Object, e As EventArgs) Handles Btn_AgregarE.Click
        Dim dt_envases As DataTable = pTabla("tbl_envases")
        If dt_envases Is Nothing Then
            dt_envases = New Data.DataTable
            dt_envases = CreaTablaEnvases()
        End If
        If Not Validar_Envases() Then Exit Sub
        For Each dr_Existe As DataRow In dt_envases.Rows
            If dr_Existe("Numero") = Tbx_Numero.Text.ToUpper Then
                fn_Alerta("No se puede agregar el envase (Numero repetido).")
                Exit Sub
            End If
        Next

        If cn.fn_ConsultaEnvasesWeb(Tbx_Numero.Text, pId_Punto) > 0 Then
            fn_Alerta("No se puede agregar el envase (Numero repetido).")
            Exit Sub
        End If


        Dim dr_Agregar As DataRow = dt_envases.NewRow()
        dr_Agregar("Id_TipoE") = ddl_envases.SelectedValue
        dr_Agregar("TipoEnvase") = ddl_envases.SelectedItem.Text
        dr_Agregar("Numero") = Tbx_Numero.Text.ToUpper
        dt_envases.Rows.Add(dr_Agregar)
        pTabla("tbl_envases") = dt_envases
        gv_Envases.DataSource = dt_envases
        gv_Envases.DataBind()
        'ddl_envases.SelectedValue = 0
        Tbx_Numero.Text = Nothing
        Tbx_Numero.Focus()
    End Sub

    Protected Sub gv_Envases_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Envases.SelectedIndexChanged
        If IsDBNull(gv_Envases.DataKeys(0).Value) OrElse CStr(gv_Envases.DataKeys(0).Value) = "" OrElse CStr(gv_Envases.DataKeys(0).Value) = "0" Then Exit Sub

        Dim dt_EnvasesAgregar As DataTable = pTabla("tbl_envases")
        If dt_EnvasesAgregar Is Nothing Then Return
        Dim Id_Inventario As String = gv_Envases.SelectedDataKey("Numero")

        'Dim filas As DataRow() = dt_MonedasAgregar.Select("Id_Inventario = " & Id_Inventario)

        For Each dr As DataRow In dt_EnvasesAgregar.Rows
            If dr("Numero") = Id_Inventario Then
                dt_EnvasesAgregar.Rows.Remove(dr)
                Exit For
            End If
        Next

        pTabla("tbl_envases") = dt_EnvasesAgregar
        gv_Envases.DataSource = fn_MostrarSiempre(dt_EnvasesAgregar)
        gv_Envases.DataBind()
    End Sub

    Protected Sub gv_Envases_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv_Envases.RowDataBound
        gv_Envases.SelectedIndex = -1
    End Sub

    Protected Sub Guardar_Click(sender As Object, e As EventArgs) Handles Guardar.Click
        Dim Importes_xd() = Importe_denominaciones()
        If pTabla("tbl_monedas") Is Nothing Then
            Guardar_lbl.Text = "No hay monedas capturadas."
            Exit Sub
        End If

        If pTabla("tbl_envases") Is Nothing And txt_envasesSN.Text = "0" Then
            Guardar_lbl.Text = "No hay envases capturados."
            Exit Sub
        End If
        If Importes_xd Is Nothing Then
            Guardar_lbl.Text = "Importes por denominacion erroneos."
            Exit Sub
        End If
        If Validar_Importes() = False Then
            Guardar_lbl.Text = "El importe por denominación no coicide con el importe total (En su defecto dejar en 0)."
            Exit Sub
        End If
        If (cn.fn_GuardarRemisionWeb(pTabla("tbl_monedas"), pTabla("tbl_envases"), Importe(0), pId_Punto, Importes_xd, txt_Comentarios.Text, txt_envasesSN.Text)) Then
            Limpiar_t()
            fn_Alerta("Remisión generada correctamente.")
        Else
            fn_Alerta("Verificar los valores capturados.")
        End If
    End Sub
    Function Validar_Importes() As Boolean
        If (tbx_1.Text <> "0" Or tbx_2.Text <> "0" Or tbx_3.Text <> "0" Or tbx_4.Text <> "0" Or tbx_5.Text <> "0" Or tbx_6.Text <> "0" Or tbx_7.Text <> "0" Or
            tbx_8.Text <> "0" Or tbx_9.Text <> "0" Or tbx_10.Text <> "0" Or tbx_11.Text <> "0" Or tbx_12.Text <> "0" Or txt_mixto.Text <> "0") Then
            ''Realizar suma de impotes
            Dim Total As Double = 0
            For Each index As Double In Importe_denominaciones()
                Total += index
            Next
            If Total <> Importe(1) Then Return False
            Return True
        Else
            Return True
        End If
    End Function
    Function Importe(tipo As Integer) As Double
        Dim Imp As Double
        For Each row As DataRow In pTabla("tbl_monedas").Rows
            For Each r As DataRow In pTabla("Tipo_cambio").Rows
                If (row("Id_Moneda") = r("Id_Moneda")) Then
                    Imp += (CDbl(row("Efectivo")) * CDbl(r("TipoCambio")))
                    If tipo <> 1 Then
                        Imp += (CDbl(row("Documentos")) * CDbl(r("TipoCambio")))
                    End If
                End If
            Next
        Next
        Return Imp
    End Function
    Function Importe_denominaciones() As Double()
        Dim Ixd(13) As Double
        If (tbx_1.Text.Trim <> Nothing And tbx_2.Text.Trim <> Nothing And tbx_3.Text.Trim <> Nothing And tbx_4.Text.Trim <> Nothing And tbx_5.Text.Trim <> Nothing _
            And tbx_6.Text.Trim <> Nothing And tbx_7.Text.Trim <> Nothing And tbx_8.Text.Trim <> Nothing And tbx_9.Text.Trim <> Nothing _
            And tbx_10.Text.Trim <> Nothing And tbx_11.Text.Trim <> Nothing And tbx_12.Text.Trim <> Nothing) Then
            Ixd(0) = CDbl(tbx_1.Text)
            Ixd(1) = CDbl(tbx_2.Text)
            Ixd(2) = CDbl(tbx_3.Text)
            Ixd(3) = CDbl(tbx_4.Text)
            Ixd(4) = CDbl(tbx_5.Text)
            Ixd(5) = CDbl(tbx_6.Text)
            Ixd(6) = CDbl(tbx_7.Text)
            Ixd(7) = CDbl(tbx_8.Text)
            Ixd(8) = CDbl(tbx_9.Text)
            Ixd(9) = CDbl(tbx_10.Text)
            Ixd(10) = CDbl(tbx_11.Text)
            Ixd(11) = CDbl(tbx_12.Text)
            Ixd(12) = CDbl(txt_mixto.Text)

            Return Ixd
        Else
            fn_Alerta("Debe capturar una cantidad valida")
            Return Nothing
        End If
        Return Nothing
    End Function

    Protected Sub gv_puntos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_puntos.SelectedIndexChanged
        If IsDBNull(gv_puntos.DataKeys(0).Value) OrElse CStr(gv_puntos.DataKeys(0).Value) = "" OrElse CStr(gv_puntos.DataKeys(0).Value) = "0" Then Exit Sub
        If Indice > 0 Then Exit Sub
        pId_Punto = gv_puntos.SelectedDataKey("Id_Punto")
        Cliente_origen = gv_puntos.SelectedDataKey("Cliente_Origen")
        Cliente_Destino = gv_puntos.SelectedDataKey("Cliente_Destino")
        pClaveSIAC = gv_puntos.SelectedDataKey("Clave_Cliente")
        Sec_puntos.Visible = False
        Sec_monedas.Visible = True
        Sec_envases.Visible = True
        Sec_denominaciones.Visible = True
        Guardar.Visible = True
        Regresar.Visible = True
        lbl_destino.InnerText = "DESTINO: " + Cliente_Destino
        lbl_destino.Visible = True
        coments.Visible = True
        Limpiar_t()
    End Sub
    Sub Regresar_()
        Sec_puntos.Visible = True
        Sec_monedas.Visible = False
        Sec_envases.Visible = False
        Det.Visible = False
        Sec_denominaciones.Visible = False
        Guardar.Visible = False
        Regresar.Visible = False
        Guardar_lbl.Text = ""
        lbl_destino.Visible = False
        coments.Visible = False
        DetEnvio.Items.Clear()
        If Not Obtener_puntos() Then
            Exit Sub
        End If

    End Sub
    Protected Sub Regresar_Click(sender As Object, e As EventArgs) Handles Regresar.Click
        Regresar_()
    End Sub
    Sub Limpiar_t()
        Dim dt_monedas As DataTable = CreaTablaMonedas()
        pTabla("tbl_monedas") = dt_monedas
        gv_Monedas.DataSource = fn_MostrarSiempre(dt_monedas)
        gv_Monedas.DataBind()
        '
        Dim dt_envases As DataTable = CreaTablaEnvases()
        pTabla("tbl_envases") = dt_envases
        gv_Envases.DataSource = fn_MostrarSiempre(dt_envases)
        gv_Envases.DataBind()

        ddl_envases.SelectedIndex = -1
        ddl_monedas.SelectedIndex = -1
        Tbx_Documentos.Text = 0
        Tbx_Efectivo.Text = ""
        Tbx_Numero.Text = ""
        txt_Comentarios.Text = ""
        txt_Comentarios.Text = ""
        txt_envasesSN.Text = "0"
        tbx_1.Text = 0
        tbx_2.Text = 0
        tbx_3.Text = 0
        tbx_4.Text = 0
        tbx_5.Text = 0
        tbx_6.Text = 0
        tbx_7.Text = 0
        tbx_8.Text = 0
        tbx_9.Text = 0
        tbx_10.Text = 0
        tbx_11.Text = 0
        tbx_12.Text = 0
        txt_mixto.Text = 0
        Guardar_lbl.Text = ""

    End Sub

    Protected Sub ddl_envases_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_envases.SelectedIndexChanged
        Tbx_Numero.Focus()
    End Sub

    Protected Sub gv_Envases_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Envases.PageIndexChanging
        Dim dt_Ventas As DataTable = pTabla("tbl_envases")

        gv_Envases.PageIndex = e.NewPageIndex
        gv_Envases.DataSource = dt_Ventas
        gv_Envases.DataBind()
        gv_Envases.SelectedIndex = -1
    End Sub

    Protected Sub gv_puntos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_puntos.RowCommand
        If IsDBNull(gv_puntos.DataKeys(0).Value) OrElse CStr(gv_puntos.DataKeys(0).Value) = "" OrElse CStr(gv_puntos.DataKeys(0).Value) = "0" Then Exit Sub
        Dim pId_Punto
        Dim Dt As DataTable
        Dim Remision As String = ""
        Dim Envases As String = "---"
        Select Case e.CommandName.ToUpper

            Case "AUTORIZAR"
                Mensaje.Text = ""
                Indice = e.CommandArgument
                'Dim txt As TextBox = gv_puntos.Rows(Indice).Cells(6).FindControl("tbx_NEW")
                'txt.Focus()
                'gv_puntos.Columns(6).Visible = True
                gv_puntos.SelectedIndex = Indice

                pId_Punto = gv_puntos.SelectedDataKey("Id_Punto")
                If gv_puntos.SelectedDataKey("Remisiones") = "0" Then Exit Sub

                Dt = cn.fn_DetalleEnvio(pId_Punto)

                For index As Integer = 0 To Dt.Rows.Count - 1
                    If Dt.Rows(index)(0).ToString = "" Or Remision = Dt.Rows(index)(0).ToString Then Continue For
                    Remision = Dt.Rows(index)(0).ToString
                    DetEnvio.Items.Add(Remision)
                    For i As Integer = 0 To Dt.Rows.Count - 1
                        If Remision = Dt.Rows(i)(0).ToString Then
                            Envases = "*" + Dt.Rows(i)(1).ToString
                            DetEnvio.Items.Add(Envases)
                        End If
                    Next
                    'DetEnvio.Items.Add(Remision)
                    'DetEnvio.Items.Add(Envases)
                Next
                Det.Visible = True

        End Select
        For index As Integer = 0 To gv_puntos.Rows.Count - 1
            If index = Indice Then
                gv_puntos.Rows(index).Enabled = False
                Continue For
            End If
            gv_puntos.Rows(index).Visible = False
        Next
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Regresar_()
    End Sub
    Private Sub popup_click(sender As Object, e As EventArgs) Handles lblHiddenReasignar.Click

    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Mensaje.Text = ""
        Dim Punto = gv_puntos.SelectedDataKey("Id_Punto")
        Dim dt = cn.fn_TvRemisionesWeb_V(Punto)
        If CInt(dt.Rows(0)("Total")) > 0 Then
            Mensaje.Text = "Existen remisiones sin validar."
            Exit Sub
        End If

        tb_Token.Text = ""
        popup_click(sender, e)
        MmodalPanel.Visible = True
        'MmodalPanel.Focus()
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MmodalPanel.Visible = False

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Punto = gv_puntos.SelectedDataKey("Id_Punto")
        If tb_Token.Text.Trim() = Punto Then
            TOKEN = True
            Enviar_Remsion_Portal_Cliente(Punto)
            MmodalPanel.Visible = False
            fn_Alerta("El envío de informacion se realizó correctamente.")
            Regresar_()
        Else
            TOKEN = False
            MmodalPanel.Visible = False
            Mensaje.Text = "TOKEN invalido,solicite un TOKEN valido."
        End If


    End Sub
    Dim _EnvasesWeb() As Envase
    Dim _RemisionWebD() As RemisionD
    Sub EnvasesWeb(Id_RemisionWeb As Decimal)
        Dim dt = cn.fn_Tv_EnvasesWeb_Mod_Portal(Id_RemisionWeb)
        _EnvasesWeb = New Envase(dt.Rows.Count - 1) {}
        For i = 0 To dt.Rows.Count - 1
            _EnvasesWeb(i) = New Envase With {
                 .Estatus = "A",
                 .IdTipoEnvase = dt(i)("Id_TipoE"),
                 .Numero = dt(i)("Numero").ToString
              }
        Next
    End Sub
    Sub RemisionWebD(Id_RemisionWeb As Decimal)
        Dim dt = cn.fn_Tv_RemisionesWebD_Mod_Portal(Id_RemisionWeb)
        _RemisionWebD = New RemisionD(dt.Rows.Count - 1) {}
        For i = 0 To dt.Rows.Count - 1
            _RemisionWebD(i) = New RemisionD With {
                .ImporteDocumentos = dt(i)("Importe_Doc").ToString,
                .ImporteEfectivo = dt(i)("Importe_Efectivo").ToString,
                .IdMoneda = dt(i)("Id_Moneda").ToString
            }
        Next
    End Sub
    Sub RemisionWebModel(Row As DataRow, Cerrar_Punto As Integer)
        Dim model = New RemisionRequest With
        {
           .Envases = _EnvasesWeb,
           .RemisionesD = _RemisionWebD,
           .Remision = New Remision With
           {
                        .IdCia = 1,
                        .IdCiaPropia = 1,
                        .Envases = Row("Envases").ToString,
                        .EnvasesSN = Row("EnvasesSN").ToString,
                        .ImporteTotal = Row("Importe").ToString,
                        .NumeroRemision = Row("NumeroR").ToString,
                        .MonedaLocal = 1,
                        .Estatus = "A",
                        .HoraRemision = DateTime.Now.ToShortTimeString(),
                        .Morralla = "N",
                        .ClienteOrigen = 0,
                        .ClienteDestino = 0,
                        .IdClientP = 0,
                        .IdRemision = 0,
                        .IdRemisionWeb = Row("Id_RemsionWeb"),
                        .IdRemisionTemporal = 0
           },
           .PuntoRemision = New PuntoRemision With
           {
           .IdPunto = Row("Id_Punto")
           },
           .ClienteSesion = New ClienteSesion With
           {
                .IdUsuarioCliente = pId_Usuario,
                .NombreUsuarioCliente = pNombre_Usuario,
                .Sesion = HttpContext.Current.User.Identity.Name,
                .Mail = pMail_Usuario
           },
            .ModoCaptura = 2,
            .CerrarPunto = Cerrar_Punto,
            .ComentarioRecolleccion = "Se captura desde PORTAL WEB",
            .IdSucursal = 1,
            .IdUsuario = 5106,
            .EstacionNombre = "PORTAL WEB",
            .IdLogin = 0
        }
        Dim Remision_ As New Cls_RemisionDigital
        Remision_.insertarRemisionDigital(model)
    End Sub
    Sub Enviar_Remsion_Portal_Cliente(Id_Punto As Long)
        Dim Data = cn.fn_Tv_RemisionesWeb_Mod_Portal(Id_Punto)
        Dim cont = 0
        For Each row In Data.Rows
            cont += 1
            RemisionWebD(row("Id_RemsionWeb"))
            EnvasesWeb(row("Id_RemsionWeb"))
            RemisionWebModel(row, IIf(cont = Data.Rows.Count, 1, 0))
        Next
    End Sub
End Class