Partial Public Class AutorizarDotacionesCajeros
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Return
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: AUTORIZAR DOTACIONES CAJEROS ATMS")
        Dim dt_SucursalesPropias As DataTable = cn.fn_SucursalesPropias_Get

        If dt_SucursalesPropias Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        Call fn_LlenarDropDown(ddl_SucursalPropia, dt_SucursalesPropias, False)
        Call MuestragridDotacion_vacio()
    End Sub

    Sub MuestragridDotacion_vacio()
        gv_DotacionesActivas.DataSource = fn_CreaGridVacio("Id_DotacionCaj,FechaCaptura,HoraCaptura,FechaEntrega,HoraEntrega,NoCajero,Cajero,Importe,Moneda,IdM,IdCajaB,IdS,Comentarios,Conexion,PrioridadBanco,Especial,Prioridad,IdRuta,NumReporte,IdC,RequiereCorte,HoraSolicitaBanco")
        gv_DotacionesActivas.DataBind()
        gv_DotacionesActivas.SelectedIndex = -1
        Call LimpiarDetalle()
    End Sub

    Sub LimpiarDetalle()
        txt_Comentarios.Text = String.Empty
        gv_Detalle.DataSource = fn_CreaGridVacio("Presentacion,Denominacion,Cantidad,Importe,IdD")
        gv_Detalle.DataBind()

        If gv_DotacionesActivas.Rows.Count < 4 Then
            fst_AutorizarDotaciones.Style.Add("height", "210px")
        End If
    End Sub

    Private Sub ddl_SucursalesPropias_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddl_SucursalPropia.SelectedIndexChanged
        Call fn_LlenarDotaciones(ddl_SucursalPropia.SelectedValue)
    End Sub

    Protected Sub fn_LlenarDotaciones(ByVal P_Clave_SucursalPropia As String)
        Dim dt_Dotaciones As DataTable = cn.fn_AutorizarDotacionesCajeros_GetActivas(P_Clave_SucursalPropia)
        fst_AutorizarDotaciones.Style.Remove("height")

        If dt_Dotaciones Is Nothing Then
            gv_DotacionesActivas.DataSource = fn_CreaGridVacio("Id_DotacionCaj,FechaCaptura,HoraCaptura,FechaEntrega,HoraEntrega,NoCajero,Cajero,Importe,Moneda,IdM,IdCajaB,IdS,Comentarios,Conexion,PrioridadBanco,Especial,Prioridad,IdRuta,NumReporte,IdC,RequiereCorte,HoraSolicitaBanco")
            gv_DotacionesActivas.DataBind()
        Else
            gv_DotacionesActivas.DataSource = fn_MostrarSiempre(dt_Dotaciones)
            gv_DotacionesActivas.DataBind()
            pTabla("tablaDotaciones") = dt_Dotaciones
        End If
        gv_DotacionesActivas.SelectedIndex = -1
        Call LimpiarDetalle()
    End Sub

    Protected Sub gv_DotacionesActivas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gv_DotacionesActivas.RowCommand
        If IsDBNull(gv_DotacionesActivas.DataKeys(0).Value) OrElse CStr(gv_DotacionesActivas.DataKeys(0).Value) = "" OrElse CStr(gv_DotacionesActivas.DataKeys(0).Value) = "0" Then Exit Sub

        Select Case e.CommandName.ToUpper
            Case "VERDETALLE"

                Dim Indice As Integer = e.CommandArgument
                gv_DotacionesActivas.SelectedIndex = Indice
                Dim Id_DotacionCaj As Integer = Integer.Parse(gv_DotacionesActivas.SelectedDataKey("Id_DotacionCaj"))

                Call fn_LlenarDetalle(Id_DotacionCaj)
                txt_Comentarios.Text = (gv_DotacionesActivas.SelectedDataKey("Comentarios"))

                gv_DotacionesActivas.SelectedIndex = Indice
            Case "AUTORIZAR"

                Dim Indice As Integer = e.CommandArgument
                gv_DotacionesActivas.SelectedIndex = Indice
                Dim Id_DotacionCaj As Integer = Integer.Parse(gv_DotacionesActivas.SelectedDataKey("Id_DotacionCaj"))
                Dim P_IdM As Integer = Integer.Parse(gv_DotacionesActivas.SelectedDataKey("IdM"))
                Dim P_IdC As Integer = Integer.Parse(gv_DotacionesActivas.SelectedDataKey("IdC"))
                Dim P_IdS As Integer = Integer.Parse(gv_DotacionesActivas.SelectedDataKey("IdS"))
                Dim Comentarios As String = gv_DotacionesActivas.SelectedDataKey("Comentarios")
                Dim p_PrioridadBanco As Char = gv_DotacionesActivas.SelectedDataKey("PrioridadBanco")
                Dim p_Especial As Char = gv_DotacionesActivas.SelectedDataKey("Especial")
                Dim p_Prioridad As Integer = CInt(gv_DotacionesActivas.SelectedDataKey("Prioridad"))
                Dim p_IdRuta As Integer = CInt(gv_DotacionesActivas.SelectedDataKey("IdRuta"))
                Dim P_NumReporte As String = gv_DotacionesActivas.SelectedDataKey("NumReporte")
                Dim Importe As Decimal = Decimal.Parse(gv_DotacionesActivas.SelectedRow.Cells(9).Text)
                Dim FechaEntrega As Date = Date.Parse(gv_DotacionesActivas.SelectedRow.Cells(5).Text)
                Dim HoraEntrega As String = gv_DotacionesActivas.SelectedRow.Cells(6).Text
                Dim HoraSolicitaBanco As String = gv_DotacionesActivas.SelectedDataKey("HoraSolicitaBanco")
                Dim P_RequiereCorte As Char = gv_DotacionesActivas.SelectedDataKey("RequiereCorte")
                Dim Cadena() As String = Split(gv_DotacionesActivas.SelectedDataKey("Conexion"), "&")
                Cadena(0) = BasePage.fn_Decode(Cadena(0))
                Cadena(1) = BasePage.fn_Decode(Cadena(1))
                Cadena(2) = BasePage.fn_Decode(Cadena(2))
                Cadena(3) = BasePage.fn_Decode(Cadena(3))
                Dim Conexion As String = "Data Source=" & Cadena(0) & "; Initial Catalog=" & Cadena(1) & ";User ID=" & Cadena(2) & ";Password=" & Cadena(3) & ";"

                Dim tblDetalle = cn.fn_AutorizarDotacionesCajeros_GetDetalle(Id_DotacionCaj)

                If tblDetalle Is Nothing Then
                    Call LimpiarDetalle()
                    Fn_Alerta("No se puede continuar debido a un error.")
                    Exit Sub
                End If

                If Not cn.fn_AutorizarDotacionesCajeros_Autorizar(Id_DotacionCaj, Conexion, tblDetalle, P_IdS, P_IdM, Importe, FechaEntrega, HoraEntrega, Comentarios, _
                                                                   P_IdC, P_RequiereCorte, HoraSolicitaBanco, P_NumReporte, p_PrioridadBanco, p_Especial, p_Prioridad, p_IdRuta) Then

                    Fn_Alerta("No se puede Autorizar la dotación debido a un error")
                Else
                    Dim DetalleHTML As String = ""

                    gv_DotacionesActivas.SelectedIndex = Indice
                    With gv_DotacionesActivas.SelectedRow
                        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
                        DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                    & "<tr><td colspan='4' align='center'> SOLICITUD WEB: DOTACION ATM </td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr>" _
                                    & "<tr><td align='right'><label><b>Usuario Autoriza:</b></label></td><td> " & pNombre_Cliente & " \ " & pNombre & " </td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Solicitante:</b></label></td><td> " & .Cells(8).Text & " </td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Fecha Requerida:</b></label></td><td>" & .Cells(5).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Hora Requerida:</b></label></td><td>" & .Cells(6).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Moneda:</b></label></td><td>" & .Cells(10).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Importe Solicitado:</b></label></td><td>" & .Cells(9).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & Comentarios & "</td><td></td><td></td></tr>" _
                                    & "<tr><td colspan='4'>" & fn_DatatableToHTML(tblDetalle, "DETALLE", 0, 1) & "</td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

                    End With
                    Dim dt_Mails As DataTable = cn.fn_AlertasGeneradas_ObtenerMails("55")

                    If Not dt_Mails Is Nothing AndAlso dt_Mails.Rows.Count > 0 Then
                        For Each Mail As DataRow In dt_Mails.Rows
                            If Mail("Mail") = "" Then Continue For
                            cn_mail.fn_Enviar_MailHTML(Mail("Mail"), "SOLICITUD WEB: DOTACION ATM.", DetalleHTML, "", P_IdS)
                        Next
                    End If

                    If pMail_Usuario <> "" Then
                        cn_mail.fn_Enviar_MailHTML(pMail_Usuario, "SOLICITUD WEB: DOTACION ATM.", DetalleHTML, "", pId_Sucursal)
                    End If
                    With gv_DotacionesActivas.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "AUTORIZO DOTACION ATM, SOLICITANTE: " & .Cells(7).Text & " / FECHA DE ENTREGA: " & .Cells(5).Text & _
                                             " / HORA ENTREGA: " & .Cells(6).Text & " / IMPORTE: " & .Cells(8).Text & " / MONEDA: " & .Cells(9).Text)
                    End With
                End If

                Call fn_LlenarDotaciones(ddl_SucursalPropia.SelectedValue)
            Case "CANCELAR"

                Dim Indice As Integer = e.CommandArgument
                gv_DotacionesActivas.SelectedIndex = Indice
                Dim Id_DotacionCaj As Integer = Integer.Parse(gv_DotacionesActivas.SelectedDataKey("Id_DotacionCaj"))

                If Not cn.fn_AutorizarDoracionesCajeros_Status(Id_DotacionCaj, "CA") Then
                    Fn_Alerta("No se puede Cancelar la dotación a Cajero debido a un error")
                Else
                    With gv_DotacionesActivas.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "CANCELO LA DOTACION ATM, SOLICITANTE: " & .Cells(8).Text & " / FECHA DE ENTREGA: " & .Cells(5).Text & _
                                             " / HORA ENTREGA: " & .Cells(6).Text & " / IMPORTE: " & .Cells(9).Text & " / MONEDA: " & .Cells(10).Text)
                    End With
                End If
                Call fn_LlenarDotaciones(ddl_SucursalPropia.SelectedValue)
        End Select
    End Sub

    Protected Sub fn_LlenarDetalle(ByVal Id_DotacionCaj As Integer)
        If Id_DotacionCaj = 0 Then Exit Sub

        Dim dt_Detalle As DataTable = cn.fn_AutorizarDotacionesCajeros_GetDetalle(Id_DotacionCaj)

        If dt_Detalle Is Nothing Then
            Call LimpiarDetalle()
            fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        gv_Detalle.DataSource = dt_Detalle
        gv_Detalle.DataBind()

    End Sub

    Protected Sub gv_DotacionesActivas_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv_DotacionesActivas.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Id As Integer '= gv_DotacionesActivas.DataKeys(e.Row.RowIndex).Value
            If CStr(gv_DotacionesActivas.DataKeys(e.Row.RowIndex).Value) = "" Then
                Id = -1
            Else
                Id = gv_DotacionesActivas.DataKeys(e.Row.RowIndex).Value
            End If

            If Id.Equals(-1) Then
                e.Row.Cells(0).Controls.Clear()
                e.Row.Cells(1).Controls.Clear()
                e.Row.Cells(2).Controls.Clear()
            End If
        End If
    End Sub

    Protected Sub gv_DotacionesActivas_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gv_DotacionesActivas.DataBound
        gv_DotacionesActivas.SelectedIndex = -1
        Call LimpiarDetalle()
    End Sub

    Protected Sub gv_DotacionesActivas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gv_DotacionesActivas.PageIndexChanging
        Dim dt_Dotaciones As DataTable = pTabla("tablaDotaciones")
        gv_DotacionesActivas.PageIndex = e.NewPageIndex
        gv_DotacionesActivas.DataSource = dt_Dotaciones
        gv_DotacionesActivas.DataBind()
        If gv_DotacionesActivas.Rows.Count < 4 Then
            fst_AutorizarDotaciones.Style.Add("height", "210px")
        End If
    End Sub
End Class