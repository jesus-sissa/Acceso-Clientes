Public Class AutorizarServicios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Return
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: AUTORIZAR MATERIALES")
        Dim dt_SucursalesPropias As DataTable = cn.fn_SucursalesPropias_Get

        If dt_SucursalesPropias Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        Dim dt_detalle As New DataTable
        dt_detalle.Columns.Add("Origen", GetType(String))
        dt_detalle.Columns.Add("HRecoleccion", GetType(String))
        dt_detalle.Columns.Add("Destino", GetType(String))
        dt_detalle.Columns.Add("HEntrega", GetType(String))

        Dim dr_Fila As DataRow = dt_detalle.NewRow()
        dr_Fila("Origen") = ""
        dr_Fila("HRecoleccion") = ""
        dr_Fila("Destino") = ""
        dr_Fila("HEntrega") = ""
        dt_detalle.Rows.InsertAt(dr_Fila, 0)
        pTabla("TablaDetalle") = dt_detalle

        Call fn_LlenarDropDown(ddl_SucursalPropia, dt_SucursalesPropias, False)
        Call MostrarGrid_Vacios()

    End Sub

    Sub MostrarGrid_Vacios()
        gv_Solicitudes.DataSource = fn_CreaGridVacio("IdST,IdS,IdClienteSolicita,IdCS,IdCO,IdCD,Origen,HRecoleccion,Destino,HEntrega,FechaCaptura,FechaServicio,Servicio,Solicita,Conexion,Comentarios")
        gv_Solicitudes.DataBind()
        gv_Solicitudes.SelectedIndex = -1
        Call LimpiarDetalle()
    End Sub

    Sub LimpiarDetalle()
        txt_Comentarios.Text = String.Empty
        gv_Detalle.DataSource = fn_CreaGridVacio("Origen,HRecoleccion,Destino,HEntrega")
        gv_Detalle.DataBind()

        If gv_Solicitudes.Rows.Count < 4 Then
            fst_AutorizarServicios.Style.Add("height", "210px")
        End If
    End Sub

    Private Sub ddl_SucursalesPropias_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_SucursalPropia.SelectedIndexChanged
        Call fn_LlenarSolicitudes(ddl_SucursalPropia.SelectedValue)
    End Sub

    Protected Sub fn_LlenarSolicitudes(ByVal P_Clave_SucursalPropia As String)

        Dim dt_Solicitudes As DataTable = cn.fn_AutorizarServicios_Get(P_Clave_SucursalPropia, "SO")

        fst_AutorizarServicios.Style.Remove("height")

        If dt_Solicitudes Is Nothing Then
            gv_Solicitudes.DataSource = fn_CreaGridVacio("IdST,IdS,IdClienteSolicita,IdCS,IdCO,IdCD,Conexion,Comentarios,Origen,HRecoleccion,Destino,HEntrega,FechaCaptura,FechaServicio,Servicio,Solicita")
            gv_Solicitudes.DataBind()
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
            fst_AutorizarServicios.Style.Add("height", "210px")
        End If
    End Sub

    Private Sub gv_Solicitudes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_Solicitudes.RowCommand
        If IsDBNull(gv_Solicitudes.DataKeys(0).Value) OrElse CStr(gv_Solicitudes.DataKeys(0).Value) = "" OrElse CStr(gv_Solicitudes.DataKeys(0).Value) = "0" Then Exit Sub

        Select Case e.CommandName.ToUpper
            Case "VERDETALLE"
                Dim Indice As Integer = e.CommandArgument
                gv_Solicitudes.SelectedIndex = Indice
                Call fn_LlenarDetalle()

                txt_Comentarios.Text = (gv_Solicitudes.SelectedDataKey("Comentarios"))

            Case "AUTORIZAR"
                Dim Indice As Integer = e.CommandArgument
                gv_Solicitudes.SelectedIndex = Indice

                Dim IdST As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdST"))
                Dim IdS As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdS"))
                Dim FechaServicio As DateTime = DateTime.Parse(gv_Solicitudes.SelectedRow.Cells(4).Text)
                Dim Id_CliSolicita As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdClienteSolicita"))
                Dim IdCS As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdCS"))
                Dim Id_CliOrigen As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdCO"))
                Dim Id_CliDestino As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdCD"))
                Dim Hora_Rec As String = gv_Solicitudes.SelectedDataKey("HRecoleccion")
                Dim Hora_Ent As String = gv_Solicitudes.SelectedDataKey("HEntrega")
                Dim Origen As String = gv_Solicitudes.SelectedDataKey("Origen")
                Dim Destino As String = gv_Solicitudes.SelectedDataKey("Destino")
                Dim comenta As String = gv_Solicitudes.SelectedDataKey("Comentarios")

                Dim Cadena() As String = Split(gv_Solicitudes.SelectedDataKey("Conexion"), "&")
                Cadena(0) = BasePage.fn_Decode(Cadena(0))
                Cadena(1) = BasePage.fn_Decode(Cadena(1))
                Cadena(2) = BasePage.fn_Decode(Cadena(2))
                Cadena(3) = BasePage.fn_Decode(Cadena(3))
                Dim Conexion As String = "Data Source=" & Cadena(0) & "; Initial Catalog=" & Cadena(1) & ";User ID=" & Cadena(2) & ";Password=" & Cadena(3) & ";"

                If Not cn.fn_AutorizarServicios_Autorizar(IdST, FechaServicio, Id_CliSolicita, IdCS, Id_CliOrigen, Hora_Rec, Id_CliDestino, Hora_Ent, Conexion, comenta) Then
                    Fn_Alerta("No se puede Autorizar la dotación debido a un error")
                    Exit Sub
                Else
                    Dim DetalleHTML As String = ""

                    gv_Solicitudes.SelectedIndex = Indice
                    With gv_Solicitudes.SelectedRow
                        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
                        DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                               & "<tr><td colspan='4' align='center'> SOLICITUD WEB: SERVICIO DE TRASLADO </td></tr>" _
                                               & "<tr><td colspan='4'><hr /></td></tr>" _
                                               & "<tr><td align='right'><label><b>Usuario Solicita:</b></label></td><td> " & pNombre_Usuario & " </td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Cliente Solicita:</b></label></td><td>" & .Cells(6).Text & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Servicio:</b></label></td><td>" & .Cells(5).Text & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Fecha:</b></label></td><td>" & FormatDateTime(FechaServicio, DateFormat.ShortDate) & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Cliente Origen:</b></label></td><td>" & Origen & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Hora Recolección:</b></label></td><td>" & Hora_Rec & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Cliente Destino:</b></label></td><td>" & Destino & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Hora Entrega:</b></label></td><td>" & Hora_Ent & "</td><td></td><td></td></tr>" _
                                               & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & comenta & "</td><td></td><td></td></tr>" _
                                               & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"
                    End With

                    Dim dt_Mails As DataTable = cn.fn_AlertasGeneradas_ObtenerMails("49")

                    If Not dt_Mails Is Nothing AndAlso dt_Mails.Rows.Count > 0 Then
                        For Each dr_Mail As DataRow In dt_Mails.Rows
                            If dr_Mail("Mail") = "" Then Continue For
                            cn_mail.fn_Enviar_MailHTML(dr_Mail("Mail"), "SOLICITUD WEB: SERVICIOS DE TV", DetalleHTML, "", IdS)
                        Next
                    End If

                    If pMail_Usuario <> "" Then
                        cn_mail.fn_Enviar_MailHTML(pMail_Usuario, "SOLICITUD WEB: SERVICIOS DE TV", DetalleHTML, "", pId_Sucursal)
                    End If
                    With gv_Solicitudes.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "AUTORIZO SOLICITUD SERVICIOS DE TV, SOLICITANTE: " & .Cells(6).Text & " / FECHA DE ENTREGA: " & .Cells(5).Text)
                    End With
                End If
                Call fn_LlenarSolicitudes(ddl_SucursalPropia.SelectedValue)
                Fn_Alerta("La solicitud se ha creado con exito, y se ha enviado la peticion a la compañia de TV")
            Case "CANCELAR"

                Dim Indice As Integer = e.CommandArgument
                gv_Solicitudes.SelectedIndex = Indice
                Dim IdST As Integer = Integer.Parse(gv_Solicitudes.SelectedDataKey("IdST"))

                If Not cn.fn_AutorizarServicios_Status(IdST, "CA") Then
                    Fn_Alerta("No se puede Cancelar la la Solicitud de Servicio debido a un error")
                Else
                    With gv_Solicitudes.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "CANCELO SOLICITUD DE SERVICIO TV, SOLICITANTE: " & .Cells(6).Text & " / FECHA DE ENTREGA: " & .Cells(5).Text)
                    End With
                    Fn_Alerta("Se ha cancelado correctamente el servicio de TV.")
                End If

                Call fn_LlenarSolicitudes(ddl_SucursalPropia.SelectedValue)
        End Select
    End Sub

    Protected Sub fn_LlenarDetalle()

        Dim dt_Detalle As DataTable = pTabla("TablaDetalle")
        If dt_Detalle Is Nothing Then
            Call LimpiarDetalle()
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        dt_Detalle.Rows(0)("Origen") = gv_Solicitudes.SelectedDataKey("Origen")
        dt_Detalle.Rows(0)("HRecoleccion") = gv_Solicitudes.SelectedDataKey("HRecoleccion")
        dt_Detalle.Rows(0)("Destino") = gv_Solicitudes.SelectedDataKey("Destino")
        dt_Detalle.Rows(0)("HEntrega") = gv_Solicitudes.SelectedDataKey("HEntrega")

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

End Class