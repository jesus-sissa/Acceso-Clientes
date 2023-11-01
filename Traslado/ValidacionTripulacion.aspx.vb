Partial Public Class ValidacionTripulacion
    Inherits BasePage

    Dim dt_Tripulacion2 As DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Session("PuntoId") = 0
        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: VALIDACION DE TRIPULACION")
        Call Limpiar()

        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            cbx_Todos_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
        End If
        fst_Unidades.Style.Add("height", "205px")

    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click

        Dim Fecha As Date

        If pNivel = 1 Then
            If cbx_Todos_Clientes.Checked = False AndAlso ddl_Clientes.SelectedValue = 0 Then
                Fn_Alerta("Seleccione un Cliente.")
                Exit Sub
            End If
        End If

        If (Not Date.TryParse(txt_FechaInicial.Text, Fecha)) Then
            Fn_Alerta("Debe seleccionar una fecha valida.")
            Exit Sub
        End If

        If Fecha > Today() Then
            Fn_Alerta("Debe seleccionar una fecha menor o igual que hoy.")
            Exit Sub
        End If
        fst_Unidades.Style.Remove("height")

        Dim dt_Tripulacion As DataTable = cn.fn_ValidacionTripulacion_GetLista(Fecha, ddl_Clientes.SelectedValue)
        Me.pTabla("Lista") = dt_Tripulacion
        gv_Lista.SelectedIndex = -1

        pnl_Tripulacion.Visible = False

        gv_Lista.DataSource = fn_MostrarSiempre(dt_Tripulacion)
        gv_Lista.DataBind()

        If gv_Lista.Rows.Count < 4 Then
            fst_Unidades.Style.Add("height", "205px")
        End If
    End Sub

    Sub Limpiar()
        fst_Unidades.Style.Add("height", "205px")
        gv_Lista.DataSource = fn_CreaGridVacio("Id_Punto,Fecha,Unidad,Origen,Destino,Placas,Id_Unidad")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1
        pnl_Tripulacion.Visible = False
    End Sub

    Protected Sub cbx_Todos_Clientes_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cbx_Todos_Clientes.CheckedChanged
        ddl_Clientes.Enabled = Not cbx_Todos_Clientes.Checked
        Call Limpiar()
        ddl_Clientes.SelectedValue = 0


    End Sub


    Protected Sub gv_Lista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles gv_Lista.SelectedIndexChanged
        If IsDBNull(gv_Lista.DataKeys(0).Value) OrElse CStr(gv_Lista.DataKeys(0).Value) = "" OrElse CStr(gv_Lista.DataKeys(0).Value) = "0" Then Exit Sub

        pnl_Tripulacion.Visible = True
        Dim IdPunto As Integer = Integer.Parse(gv_Lista.SelectedDataKey("Id_Punto"))



        If IdPunto = 0 Then
            pnl_Tripulacion.Visible = False
            btnImprimir.Enabled = False
            Exit Sub
        End If

        Session("Id_Punto") = Integer.Parse(gv_Lista.SelectedDataKey("Id_Punto"))
        Session("Id_Unidad") = Integer.Parse(gv_Lista.SelectedDataKey("Id_Unidad"))

        btnImprimir.Enabled = True

        dt_Tripulacion2 = cn.fn_ValidacionTripulacion_GetNombres(IdPunto)

        If dt_Tripulacion2.Rows.Count = 0 Then
            fn_Alerta("No se puede mostrar la tripulación")
            pnl_Tripulacion.Visible = False
            Exit Sub
        End If
        Session("Id_Operador") = dt_Tripulacion2.Rows(0)("Id_Operador")
        Session("Id_Cajero") = dt_Tripulacion2.Rows(0)("Id_Cajero")
        lbl_Operador.Text = dt_Tripulacion2.Rows(0)("Operador")
        lbl_OperadorClave.Text = dt_Tripulacion2.Rows(0)("ClaveOperador")
        lbl_Cajero.Text = dt_Tripulacion2.Rows(0)("Cajero")
        lbl_CajeroClave.Text = dt_Tripulacion2.Rows(0)("ClaveCajero")
        lbl_Unidad.Text = gv_Lista.SelectedRow.Cells(2).Text
        lbl_Placas.Text = gv_Lista.SelectedDataKey("Placas")

        Session("Unidad") = gv_Lista.SelectedRow.Cells(2).Text
        Session("Placa") = gv_Lista.SelectedDataKey("Placas")

        img_Operador.ImageUrl = "~/Traslado/Foto.aspx?Id=" & dt_Tripulacion2.Rows(0)("Id_Operador")
        'img_Operador.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=600, width=800,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")
        'img_Operador.CssClass = "CursorImagen" ' muestra la imagen en popUp con cursor

        img_OperadorFirma.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt_Tripulacion2.Rows(0)("Id_Operador")
        'img_OperadorFirma.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=600, width=800,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")
        'img_OperadorFirma.CssClass = "CursorImagen"

        img_Cajero.ImageUrl = "~/Traslado/Foto.aspx?Id=" & dt_Tripulacion2.Rows(0)("Id_Cajero")
        'img_Cajero.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=600, width=800,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")
        'img_Cajero.CssClass = "CursorImagen"


        img_CajeroFirma.ImageUrl = "~/Traslado/Firma.aspx?Id=" & dt_Tripulacion2.Rows(0)("Id_Cajero")
        'img_CajeroFirma.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=600, width=800,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")
        'img_CajeroFirma.CssClass = "CursorImagen"

        img_UnidadTV.ImageUrl = "~/Traslado/FotoUnidad.aspx?Id_Unidad=" & Integer.Parse(gv_Lista.SelectedDataKey("Id_Unidad"))
        'img_UnidadTV.Attributes.Add("OnClick", "window.open(this.href, 'ViewImage', 'height=600, width=800,top=90,left=100,toolbar=no,menubar=no,location=no,resizable=no,maximized=no,scrollbars=no,status=no'); return false;")
        'img_UnidadTV.CssClass = "CursorImagen"



        Dim dt_Custodios As DataTable = cn.fn_ValidacionTripulacion_GetCustodios(IdPunto)

        If dt_Custodios Is Nothing Then
            fn_Alerta("No se puede mostrar la tripulación")
            pnl_Tripulacion.Visible = False

        End If

        If dt_Custodios.Rows.Count = 0 Then
            Session("Id_Custodio1") = 0
            Session("Id_Custodio2") = 0
            EnlazarDatos(dt_Custodios)
        ElseIf dt_Custodios.Rows.Count = 1 Then
            Session("Id_Custodio1") = dt_Custodios.Rows(0)("Id_Empleado")
            Session("Id_Custodio2") = 0
            EnlazarDatos(dt_Custodios)
        ElseIf dt_Custodios.Rows.Count > 1 Then
            Session("Id_Custodio1") = dt_Custodios.Rows(0)("Id_Empleado")
            Session("Id_Custodio2") = dt_Custodios.Rows(1)("Id_Empleado")
            EnlazarDatos(dt_Custodios)
        End If
    End Sub

    Private Sub EnlazarDatos(dt_Datos As DataTable)

        dl_Custodios.DataSource = dt_Datos
        dl_Custodios.DataBind()


        hdf_ImagenesDisponibles.Value = 1
    End Sub

    Protected Sub gv_Lista_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Lista.PageIndexChanging
        pnl_Tripulacion.Visible = False

        gv_Lista.PageIndex = e.NewPageIndex
        gv_Lista.DataSource = pTabla("Lista")
        gv_Lista.DataBind()
        gv_Lista.SelectedIndex = -1
        fst_Unidades.Style.Remove("height")

        If gv_Lista.Rows.Count < 4 Then
            fst_Unidades.Style.Add("height", "205px")
        End If
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged
        Call Limpiar()
        hdf_ImagenesDisponibles.Value = 0
    End Sub

    Protected Sub txt_FechaInicial_TextChanged(sender As Object, e As EventArgs) Handles txt_FechaInicial.TextChanged
        Call Limpiar()
    End Sub

    Protected Sub btnValidar_Click(sender As Object, e As EventArgs) Handles btnValidar.Click

        If gv_Lista.SelectedIndex > -1 Then
            Dim dr As GridViewRow = gv_Lista.SelectedRow

            Dim Id_Punto As Integer = Integer.Parse(gv_Lista.SelectedDataKey("Id_Punto"))
            Dim Operador As Integer = Session("Id_Operador")
            Dim Cajero As Integer = Session("Id_Cajero")
            Dim Custodio1 As Integer = Session("Id_Custodio1")
            Dim Custodio2 As Integer = Session("Id_Custodio2")
            Dim Id_Unidad As Integer = gv_Lista.SelectedDataKey("Id_Unidad")
            Dim Identificador As String = cn.fn_ValidacionTripulacionAcuse_Guardar(Id_Punto, dr.Cells(1).Text, Operador, Cajero, Custodio1, Custodio2, Id_Unidad)
            If Identificador = "" Then
                fn_Alerta("No se puede mostrar la tripulación")
                pnl_Tripulacion.Visible = False
                Exit Sub
            End If
            Session("Identificador") = Identificador

            Session("Folio") = Integer.Parse(gv_Lista.SelectedDataKey("Id_Punto"))
            Session("Fecha") = dr.Cells(1).Text

            'Response.Redirect("ValidarTripulacionAcuse.aspx")

            fn_Alerta("Verifique que el su Navegador no tenga activado el Bloqueo de Ventanas Emergentes...")

            Dim sUrl As String = "ValidarTripulacionAcuse.aspx"
            Dim sScript As String = "<script language =javascript> "
            sScript += "window.open('" & sUrl & "',null,'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=0,resizable=1,width=800,height=500,left=100,top=100');"
            sScript += "</script> "
            Response.Write(sScript)

        End If

    End Sub

    Protected Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click

        'Dim PuntoId As Integer = Session("Id_Punto")
        'If PuntoId = 0 Then
        '    Exit Sub
        'End If
        'Dim ds As New ds_TripulacionValidacion
        'Dim dt_Tripulacion As DataTable = cn.fn_ValidacionTripulacion_GetNombresFotos(PuntoId)

        'If dt_Tripulacion Is Nothing Then
        '    fn_Alerta("Ocurrió un error al consultar los datos de la tripulación")
        '    ds.Dispose()
        '    Exit Sub
        'End If

        'If dt_Tripulacion.Rows.Count = 0 Then
        '    fn_Alerta("No se puede mostrar la tripulación")
        '    btnImprimir.Enabled = False
        '    ds.Dispose()
        '    Exit Sub
        'End If


        'Dim dt_SucursalesDatos As DataTable = cn.fn_Sucursales_GetLogo(pId_Sucursal)

        'If dt_SucursalesDatos Is Nothing Then
        '    fn_Alerta("Ocurrió un error al consultar el Logo de la Sucursal.")
        '    ds.Dispose()
        '    Exit Sub
        'End If

        'If dt_SucursalesDatos.Rows.Count = 0 Then
        '    fn_Alerta("No se puede mostrar la tripulación")
        '    btnImprimir.Enabled = False
        '    ds.Dispose()
        '    Exit Sub
        'End If

        'Dim row As DataRow = ds.tbl_SucursalLogo.NewRow
        'row("Domicilio") = dt_SucursalesDatos.Rows(0)("Domicilio")
        'row("Empresa") = dt_SucursalesDatos.Rows(0)("Empresa")
        'row("Logo") = dt_SucursalesDatos.Rows(0)("Logo")
        'ds.tbl_SucursalLogo.Rows.Add(row)


        'For Each dr As DataRow In dt_Tripulacion.Rows

        '    Dim Elemento As DataRow = ds.tbl_TripulacionNombre.NewRow
        '    Elemento("Nombre") = dr("Nombre")
        '    Elemento("Clave_Empleado") = dr("Clave_Empleado")

        '    If dr("TieneCatalogo").ToString = "SI" Then
        '        Elemento("Foto") = dr("Catalogo")
        '    Else
        '        Elemento("Foto") = dr("Frente")
        '    End If
        '    Elemento("Firma") = dr("Firma")
        '    ds.tbl_TripulacionNombre.Rows.Add(Elemento)
        'Next

        'Dim UnidadId As Integer = Session("Id_Unidad")
        'Dim Unidad As String = Session("Unidad")
        'Dim Placa As String = Session("Placa")

        'Dim dt_FotoUnidad As DataTable = cn.fn_FotoUnidad_Get(UnidadId)

        'If dt_FotoUnidad Is Nothing Then Exit Sub

        'If dt_FotoUnidad.Rows.Count > 0 Then
        '    Dim Elemento As DataRow = ds.tbl_TripulacionNombre.NewRow

        '    Elemento("Nombre") = Unidad
        '    Elemento("Clave_Empleado") = Placa
        '    Elemento("Foto") = dt_FotoUnidad.Rows(0)("FotoF")

        '    ds.tbl_TripulacionNombre.Rows.InsertAt(Elemento, 0)
        'End If

        'Dim rpt As New crp_Tripulacion
        'rpt.SetDataSource(ds)
        'rpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, True, "Validacion_Tripulacion")
        'Response.End()
    End Sub
End Class