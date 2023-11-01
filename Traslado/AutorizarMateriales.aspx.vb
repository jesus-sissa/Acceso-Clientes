Public Class AutorizarMateriales
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: AUTORIZAR MATERIALES")
        'Dim dt_SucursalesPropias As DataTable = cn.fn_SucursalesPropias_Get
        Dim dt_Clientes As DataTable = cn.fn_AutorizarDotaciones_GetClientes()
        If dt_Clientes Is Nothing Then
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        'If dt_SucursalesPropias Is Nothing Then
        '    Fn_Alerta("No se puede continuar debido a un error.")
        '    Exit Sub
        'End If
        Call MostrarGrid_Vacios()
        Call fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.Enabled = False
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
            fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
            Call fn_LlenarSolicitudes(ddl_Clientes.SelectedValue)
        End If
        'Call fn_LlenarDropDown(ddl_SucursalPropia, dt_SucursalesPropias, False)


    End Sub

    Sub MostrarGrid_Vacios()
        gv_Solicitudes.DataSource = fn_CreaGridVacio("IdMv,IdS,IdC,FechaCaptura,HoraCaptura,FechaEntrega,Solicita,Conexion,Comentarios")
        gv_Solicitudes.DataBind()
        gv_Solicitudes.SelectedIndex = -1
        'ddl_SucursalPropia.SelectedIndex = 0
        Call LimpiarDetalle()
    End Sub

    Sub LimpiarDetalle()
        txt_Comentarios.Text = String.Empty
        gv_Detalle.DataSource = fn_CreaGridVacio("Cantidad,Material")
        gv_Detalle.DataBind()
        'ddl_SucursalPropia.SelectedIndex = 0

        If gv_Solicitudes.Rows.Count < 4 Then
            fst_AutorizarMateriales.Style.Add("height", "210px")
        End If
    End Sub

    Private Sub ddl_SucursalesPropias_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_Clientes.SelectedIndexChanged 'ddl_SucursalPropia.SelectedIndexChanged
        Call fn_LlenarSolicitudes(ddl_Clientes.SelectedValue) '(ddl_SucursalPropia.SelectedValue)
    End Sub

    Protected Sub fn_LlenarSolicitudes(ByVal pId_Cliente As Integer) '(ByVal P_Clave_SucursalPropia As String)

        Dim dt_Solicitudes As DataTable = cn.fn_AutorizarMateriales_Get(pId_Cliente, "SO") '(P_Clave_SucursalPropia, "SO")
        fst_AutorizarMateriales.Style.Remove("height")
        'ddl_SucursalPropia.SelectedIndex = 0
        If dt_Solicitudes Is Nothing Then
            gv_Solicitudes.DataSource = fn_CreaGridVacio("IdMv,IdS,IdC,FechaCaptura,HoraCaptura,FechaEntrega,Solicita,Conexion,Comentarios")
            gv_Solicitudes.DataBind()
            ddl_Clientes.SelectedIndex = 0
            'ddl_SucursalPropia.SelectedIndex = 0
        Else
            gv_Solicitudes.DataSource = fn_MostrarSiempre(dt_Solicitudes)
            gv_Solicitudes.DataBind()
            pTabla("tablaSolicitudes") = dt_Solicitudes
        End If
        gv_Solicitudes.SelectedIndex = -1
        Call LimpiarDetalle()

    End Sub

    Private Sub gv_Solicitudes_DataBound(sender As Object, e As EventArgs) Handles gv_Solicitudes.DataBound
        gv_Solicitudes.SelectedIndex = -1
        Call LimpiarDetalle()
    End Sub

    Private Sub gv_Solicitudes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_Solicitudes.PageIndexChanging
        Dim dt_Solicitudes As DataTable = pTabla("tablaSolicitudes")
        gv_Solicitudes.PageIndex = e.NewPageIndex
        gv_Solicitudes.DataSource = dt_Solicitudes
        gv_Solicitudes.DataBind()
        gv_Solicitudes.SelectedIndex = -1

        If gv_Solicitudes.Rows.Count < 4 Then
            fst_AutorizarMateriales.Style.Add("height", "210px")
        End If
    End Sub

    Private Sub gv_Solicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Solicitudes.RowCommand
        If IsDBNull(gv_Solicitudes.DataKeys(0).Value) OrElse CStr(gv_Solicitudes.DataKeys(0).Value) = "" OrElse CStr(gv_Solicitudes.DataKeys(0).Value) = "0" Then Exit Sub

        Select Case e.CommandName.ToUpper
            Case "VERDETALLE"

                Dim Indice As Integer = e.CommandArgument
                gv_Solicitudes.SelectedIndex = Indice
                Dim Id_Solicitud As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdMV"))

                Call fn_LlenarDetalle(Id_Solicitud)
                gv_Solicitudes.SelectedIndex = Indice
                txt_Comentarios.Text = (gv_Solicitudes.SelectedDataKey("Comentarios"))

            Case "AUTORIZAR"
                'If IsPostBack Then
                '    Dim Indicea As Integer = e.CommandArgument
                'End If
                Dim Indice As Integer = e.CommandArgument
                gv_Solicitudes.SelectedIndex = Indice
                Dim Id_MatVenta As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdMV"))
                Dim IdS As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdS"))
                Dim IdC As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdC"))
                Dim FechaEntrega As DateTime = DateTime.Parse(gv_Solicitudes.SelectedRow.Cells(5).Text)
                Dim comenta As String = gv_Solicitudes.SelectedDataKey("Comentarios")

                Dim Cadena() As String = Split(gv_Solicitudes.SelectedDataKey("Conexion"), "&")
                Cadena(0) = BasePage.fn_Decode(Cadena(0))
                Cadena(1) = BasePage.fn_Decode(Cadena(1))
                Cadena(2) = BasePage.fn_Decode(Cadena(2))
                Cadena(3) = BasePage.fn_Decode(Cadena(3))
                Dim Conexion As String = "Data Source=" & Cadena(0) & "; Initial Catalog=" & Cadena(1) & ";User ID=" & Cadena(2) & ";Password=" & Cadena(3) & ";"
                Dim dt_Detalle = cn.fn_AutorizarMateriales_GetDetalle(Id_MatVenta)
                If dt_Detalle Is Nothing Then
                    LimpiarDetalle()
                    Fn_Alerta("No se puede continuar debido a un error.")
                    Exit Sub
                End If

                Dim dt As DataTable = cn.fn_ConsultarAutorizacion_Materiales(Id_MatVenta, "SO")

                If dt.Rows.Count = 0 Then

                    Call fn_LlenarSolicitudes(ddl_Clientes.SelectedValue) '(ddl_SucursalPropia.SelectedValue)
                    'fn_Alerta("No se puede continuar debido a un error.")
                    Exit Sub
                End If


                If Not cn.fn_AutorizarMateriales_Autorizar(IdS, Id_MatVenta, FechaEntrega, IdC, dt_Detalle, Conexion, comenta) Then
                    Fn_Alerta("No se puede Autorizar la dotación debido a un error")
                    Exit Sub
                Else
                    Dim DetalleHTML As String = ""

                    gv_Solicitudes.SelectedIndex = Indice
                    With gv_Solicitudes.SelectedRow
                        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
                        DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                    & "<tr><td colspan='4' align='center'> SOLICITUD WEB: MATERIAL OPERATIVO </td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr>" _
                                    & "<tr><td align='right'><label><b>Usuario Autoriza:</b></label></td><td> " & pNombre_Cliente & " \ " & pNombre & " </td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Solicitante:</b></label></td><td> " & .Cells(6).Text & " </td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Fecha Requerida:</b></label></td><td>" & .Cells(5).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & comenta & "</td><td></td><td></td></tr>" _
                                    & "<tr><td colspan='4'>" & fn_DatatableToHTML(dt_Detalle, "DETALLE", 1, 2) & "</td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

                    End With

                    Dim dt_Mails As DataTable = cn.fn_AlertasGeneradas_ObtenerMails("50")

                    If Not dt_Mails Is Nothing AndAlso dt_Mails.Rows.Count > 0 Then
                        For Each Mail As DataRow In dt_Mails.Rows
                            If Mail("Mail") = "" Then Continue For
                            cn_mail.fn_Enviar_MailHTML(Mail("Mail"), "SOLICITUD WEB: MATERIAL OPERATIVO.", DetalleHTML, "", IdS)
                        Next
                    End If

                    If pMail_Usuario <> "" Then
                        cn_mail.fn_Enviar_MailHTML(pMail_Usuario, "SOLICITUD WEB: MATERIAL OPERATIVO.", DetalleHTML, "", pId_Sucursal)
                    End If
                    With gv_Solicitudes.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "AUTORIZO SOLICITUD DE MATERIAL, SOLICITANTE: " & .Cells(6).Text & " / FECHA DE ENTREGA: " & .Cells(5).Text)
                    End With
                End If

                fn_Alerta("La solicitud se ha creado con exito, y se ha enviado la peticion al proveedor")

                Call fn_LlenarSolicitudes(ddl_Clientes.SelectedValue)'(ddl_SucursalPropia.SelectedValue)

            Case "CANCELAR"

                Dim Indice As Integer = e.CommandArgument
                gv_Solicitudes.SelectedIndex = Indice
                Dim Id_MatVenta As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdMV"))

                If Not cn.fn_AutorizarMateriales_Status(Id_MatVenta, "CA") Then
                    Fn_Alerta("No se puede Cancelar la la Solicitud de Material debido a un error")
                Else
                    With gv_Solicitudes.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "CANCELO SOLICITUD DE MATERIAL, SOLICITANTE: " & .Cells(6).Text & " / FECHA DE ENTREGA: " & .Cells(5).Text)
                    End With
                    Fn_Alerta("Se ha cancelado correctamente la solicitud de materiales.")
                End If
                Call fn_LlenarSolicitudes(ddl_Clientes.SelectedValue) '(ddl_SucursalPropia.SelectedValue)

        End Select
    End Sub

    Protected Sub fn_LlenarDetalle(ByVal Id_Solicitud As Integer)

        If Id_Solicitud = 0 Then Exit Sub

        Dim dt_Detalle As DataTable = cn.fn_AutorizarMateriales_GetDetalle(Id_Solicitud)

        If dt_Detalle Is Nothing Then
            LimpiarDetalle()
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        gv_Detalle.DataSource = dt_Detalle
        gv_Detalle.DataBind()

    End Sub

    Private Sub gv_Solicitudes_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gv_Solicitudes.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Id As Integer '= gv_Solicitudes.DataKeys(e.Row.RowIndex).Value
            If CStr(gv_Solicitudes.DataKeys(e.Row.RowIndex).Value) = "" Then
                Id = -1
            Else
                Id = gv_Solicitudes.DataKeys(e.Row.RowIndex).Value
            End If

            If Id.Equals(-1) Then
                e.Row.Cells(0).Controls.Clear()
                e.Row.Cells(1).Controls.Clear()
                e.Row.Cells(2).Controls.Clear()
            End If
        End If
    
    End Sub

    Protected Sub gv_Solicitudes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gv_Solicitudes.SelectedIndexChanged

    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged

    End Sub

    'Protected Sub ddl_SucursalPropia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_SucursalPropia.SelectedIndexChanged

    'End Sub
End Class