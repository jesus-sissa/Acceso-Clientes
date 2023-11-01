Public Class AutorizarFallas
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        If IsPostBack Then Exit Sub

        Call cn.fn_Crear_Log(pId_Login, "PAGINA: AUTORIZAR FALLAS CAJEROS")
        Dim dt_SucursalesPropias As DataTable = cn.fn_SucursalesPropias_Get

        If dt_SucursalesPropias Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        Call fn_LlenarDropDown(ddl_SucursalPropia, dt_SucursalesPropias, False)
        Call MuestragridDotacion_vacio()
    End Sub

    Sub MuestragridDotacion_vacio()
        gv_FallasActivas.DataSource = fn_CreaGridVacio("Id_FallaCli,FechaCaptura,HoraCaptura,FechaRequerida,TiempoRespuesta,FechaAlarma,HoraAlarma,HoraSolicitaBanco,NumReporte,NoCajero,Cajero,Parte,TipoFalla,IdC,IdPF,Tipo,Comentarios,IdS,Conexion")
        gv_FallasActivas.DataBind()
        gv_FallasActivas.SelectedIndex = -1

        If gv_FallasActivas.Rows.Count < 4 Then
            fst_AutorizarFallas.Style.Add("height", "210px")
        End If
    End Sub

    Protected Sub ddl_SucursalPropia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_SucursalPropia.SelectedIndexChanged
        Call fn_LlenarFallasCajeros(ddl_SucursalPropia.SelectedValue)
        txt_Comentarios.Text = String.Empty
    End Sub

    Protected Sub fn_LlenarFallasCajeros(ByVal P_Clave_SucursalPropia As String)
        Dim dt_Fallas As DataTable = cn.fn_AutorizarFallas_GetActivas(P_Clave_SucursalPropia)
        txt_Comentarios.Text = String.Empty

        fst_AutorizarFallas.Style.Remove("height")

        If dt_Fallas Is Nothing Then
            Call MuestragridDotacion_vacio()
        Else
            gv_FallasActivas.DataSource = fn_MostrarSiempre(dt_Fallas)
            gv_FallasActivas.DataBind()
            pTabla("tablaFallas") = dt_Fallas
        End If

        If gv_FallasActivas.Rows.Count < 4 Then
            fst_AutorizarFallas.Style.Add("height", "210px")
        End If
    End Sub

    Protected Sub gv_FallasActivas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gv_FallasActivas.RowCommand
        If IsDBNull(gv_FallasActivas.DataKeys(0).Value) OrElse CStr(gv_FallasActivas.DataKeys(0).Value) = "" OrElse CStr(gv_FallasActivas.DataKeys(0).Value) = "0" Then Exit Sub

        Select Case e.CommandName.ToUpper
            Case "VERDETALLE"

                Dim Indice As Integer = e.CommandArgument
                gv_FallasActivas.SelectedIndex = Indice
                txt_Comentarios.Text = (gv_FallasActivas.SelectedDataKey("Comentarios"))

            Case "AUTORIZAR"

                Dim Indice As Integer = e.CommandArgument
                gv_FallasActivas.SelectedIndex = Indice

                Dim Id_FallaCli As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("Id_FallaCli"))
                Dim P_IdC As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("IdC"))
                Dim P_IdS As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("IdS"))
                Dim P_IdPF As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("IdPF"))
                Dim Comentarios As String = gv_FallasActivas.SelectedDataKey("Comentarios")
                Dim P_NumReporte As String = gv_FallasActivas.SelectedDataKey("NumReporte")
                Dim P_Tipo As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("Tipo"))
                Dim HoraSolicitaBanco As String = gv_FallasActivas.SelectedDataKey("HoraSolicitaBanco").ToString
                Dim FechaAlarma As Date = gv_FallasActivas.SelectedDataKey("FechaAlarma")
                Dim HoraAlarma As String = gv_FallasActivas.SelectedDataKey("HoraAlarma")
                Dim FechaRequerida As Date = gv_FallasActivas.SelectedDataKey("FechaRequerida")
                Dim TiempoRespuesta As String = gv_FallasActivas.SelectedDataKey("TiempoRespuesta")
                Dim Cadena() As String = Split(gv_FallasActivas.SelectedDataKey("Conexion"), "&")
                Cadena(0) = BasePage.fn_Decode(Cadena(0))
                Cadena(1) = BasePage.fn_Decode(Cadena(1))
                Cadena(2) = BasePage.fn_Decode(Cadena(2))
                Cadena(3) = BasePage.fn_Decode(Cadena(3))
                Dim Conexion As String = "Data Source=" & Cadena(0) & "; Initial Catalog=" & Cadena(1) & ";User ID=" & Cadena(2) & ";Password=" & Cadena(3) & ";"

                Dim Tipo As String = "FALLA ATM"
                If P_Tipo = 2 Then Tipo = "CUSTODIA ATM"

                If Not cn.fn_AutorizarFallas_Autorizar(Conexion, Id_FallaCli, P_IdC, P_IdPF, P_NumReporte, FechaRequerida, _
                                                       TiempoRespuesta, P_Tipo, FechaAlarma, HoraAlarma, HoraSolicitaBanco, Comentarios) Then

                    Fn_Alerta("No se puede Autorizar la " & Tipo & " debido a un error")
                Else
                    Dim DetalleHTML As String = ""

                    gv_FallasActivas.SelectedIndex = Indice
                    With gv_FallasActivas.SelectedRow
                        Dim Pie As String = "Agente de Servicios SIAC " & Now.Year.ToString
                        DetalleHTML = "<html><body><table style='border: solid 1px' width='100%'><tr><td colspan='4' align='center'><b>Boletín Informativo</b></td></tr>" _
                                    & "<tr><td colspan='4' align='center'> SOLICITUD WEB:" & Tipo & " </td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr>" _
                                    & "<tr><td align='right'><label><b>Usuario Autoriza:</b></label></td><td> " & pNombre_Cliente & " \ " & pNombre & " </td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Solicitante:</b></label></td><td> " & .Cells(8).Text & " </td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Fecha Requerida:</b></label></td><td>" & .Cells(5).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Hora Requerida:</b></label></td><td>" & .Cells(6).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Tipo Falla:</b></label></td><td>" & .Cells(10).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Parte Falla:</b></label></td><td>" & .Cells(9).Text & "</td><td></td><td></td></tr>" _
                                    & "<tr><td align='right'><label><b>Observaciones:</b></label></td><td>" & Comentarios & "</td><td></td><td></td></tr>" _
                                    & "<tr><td colspan='4'><hr /></td></tr><tr><td colspan='3' align='center'>" & Pie & "</td></tr></table><br/><br/></body></html>"

                    End With
                    Dim dt_Mails As DataTable = cn.fn_AlertasGeneradas_ObtenerMails("53")

                    If Not dt_Mails Is Nothing AndAlso dt_Mails.Rows.Count > 0 Then
                        For Each Mail As DataRow In dt_Mails.Rows
                            If Mail("Mail") = "" Then Continue For
                            cn_mail.fn_Enviar_MailHTML(Mail("Mail"), "SOLICITUD WEB: " & Tipo & ".", DetalleHTML, "", P_IdS)
                        Next
                    End If

                    If pMail_Usuario <> "" Then
                        cn_mail.fn_Enviar_MailHTML(pMail_Usuario, "SOLICITUD WEB: " & Tipo & ".", DetalleHTML, "", pId_Sucursal)
                    End If
                    With gv_FallasActivas.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "AUTORIZO " & Tipo & " , SOLICITANTE: " & .Cells(8).Text & " / FECHA REQUERIDA: " & .Cells(5).Text & _
                                             " / HORA REQUERIDA: " & .Cells(6).Text & " / TIPO FALLA: " & .Cells(10).Text & " / PARTE FALLA: " & .Cells(9).Text)
                    End With
                End If

                Call fn_LlenarFallasCajeros(ddl_SucursalPropia.SelectedValue)
            Case "CANCELAR"

                Dim Indice As Integer = e.CommandArgument
                gv_FallasActivas.SelectedIndex = Indice
                Dim P_Tipo As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("Tipo"))
                Dim Id_FallaCli As Integer = Integer.Parse(gv_FallasActivas.SelectedDataKey("Id_FallaCli"))

                Dim Tipo As String = "FALLA"
                If P_Tipo = 2 Then Tipo = "CUSTODIA"

                If Not cn.fn_AutorizarFallasCustodias_Status(Id_FallaCli, "CA") Then
                    Fn_Alerta("No se puede Cancelar la  " & Tipo & " debido a un error")
                Else
                    With gv_FallasActivas.SelectedRow
                        Call cn.fn_Crear_Log(pId_Login, "CANCELO LA " & Tipo & " , SOLICITANTE: " & .Cells(8).Text & " / FECHA REQUERIDA: " & .Cells(5).Text & _
                                             " / HORA REQUERIDA: " & .Cells(6).Text & " / TIPO FALLA: " & .Cells(10).Text & " / PARTE FALLA: " & .Cells(9).Text)
                    End With
                End If
                Call fn_LlenarFallasCajeros(ddl_SucursalPropia.SelectedValue)
        End Select
    End Sub

    Protected Sub gv_FallasActivas_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gv_FallasActivas.RowCreated

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim Id As Integer
            If CStr(gv_FallasActivas.DataKeys(e.Row.RowIndex).Value) = "" Then
                Id = -1
            Else
                Id = gv_FallasActivas.DataKeys(e.Row.RowIndex).Value
            End If

            If Id.Equals(-1) Then
                e.Row.Cells(0).Controls.Clear()
                e.Row.Cells(1).Controls.Clear()
                e.Row.Cells(2).Controls.Clear()
            End If
        End If
    End Sub

    Protected Sub gv_FallasActivas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gv_FallasActivas.PageIndexChanging
        Dim dt_Fallas As DataTable = pTabla("tablaFallas")
        gv_FallasActivas.PageIndex = e.NewPageIndex
        gv_FallasActivas.DataSource = dt_Fallas
        gv_FallasActivas.SelectedIndex = -1
        gv_FallasActivas.DataBind()

        If gv_FallasActivas.Rows.Count < 4 Then
            fst_AutorizarFallas.Style.Add("height", "210px")
        End If
    End Sub

    Protected Sub gv_FallasActivas_DataBound(sender As Object, e As EventArgs) Handles gv_FallasActivas.DataBound
        gv_FallasActivas.SelectedIndex = -1
    End Sub
End Class