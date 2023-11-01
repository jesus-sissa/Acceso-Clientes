Public Class Consultar_Remision
    Inherits BasePage
    Dim Id_Venta As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->
        If IsPostBack Then Exit Sub
    End Sub
    Protected Sub Consultar_Click(sender As Object, e As EventArgs) Handles Consultar.Click
        If (txt_Remision.Text.Trim = "") Then Exit Sub
        Dim tbl As DataTable

        tbl = cn.fn_ConsultaIDRemision(txt_Remision.Text.Trim)
        If tbl.Rows.Count > 0 Then
            Num_Remision = tbl.Rows(0)(0).ToString()
            Id_Venta = tbl.Rows(0)(1).ToString
            If cbx_todos.Checked Then
                Atm_Remision()
            Else

                If (ddl_tipo.SelectedIndex = 0) Then
                    Num_Remision = tbl(0)(1).ToString
                    If Num_Remision = "" Or Num_Remision Is Nothing Then 'Cuando la remision no pertence a 'Materiales' pero pertenece a otra seleccion, en este caso de utiliza el Id de la venta.
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                    tbl = cn.fn_ConsultaR1(CInt(Num_Remision)) 'Comprobamos que la remision perteneca a Materiales
                    If tbl.Rows.Count = 0 Then
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                    Data_Remision = tbl
                    Complemetos()
                    Response.Redirect("~/Traslado/ImprimirR.aspx")
                ElseIf (ddl_tipo.SelectedIndex = 1) Then 'Comprobamo que la remision pertenesca ala seccion de recoleccion
                    tbl = cn.fn_ConsultaR2(CInt(Num_Remision))
                    If tbl.Rows.Count = 0 Then
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                ElseIf (ddl_tipo.SelectedIndex = 2) Then 'Comprobamo que la remision pertenesca ala seccion 
                    tbl = cn.fn_ConsultaR3(CInt(Num_Remision))
                    If tbl.Rows.Count = 0 Then
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                ElseIf (ddl_tipo.SelectedIndex = 3) Then 'Comprobamo que la remision pertenesca ala seccion 
                    tbl = cn.fn_ConsultaR4(CInt(Num_Remision))
                    If tbl.Rows.Count = 0 Then
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                ElseIf (ddl_tipo.SelectedIndex = 4) Then 'Comprobamo que la remision pertenesca ala seccion 
                    tbl = cn.fn_ConsultaR5(CInt(Num_Remision))
                    If tbl.Rows.Count = 0 Then
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                ElseIf (ddl_tipo.SelectedIndex = 5) Then 'Comprobamo que la remision pertenesca ala seccion 
                    tbl = cn.fn_ConsultaR6(CInt(Num_Remision))
                    If tbl.Rows.Count = 0 Then
                        Alerta.Text = "*Remision no encontrada en la selección actual."
                        Exit Sub
                    End If
                End If
                Data_Remision = tbl
                Complemetos()
                Response.Redirect("~/Traslado/ImprimirR.aspx")
            End If
        Else            
            Alerta.Text = "*Remision no encontrada,verifique que el número sea correcto."
        End If
    End Sub
    Sub Atm_Remision()             
        Dim Tbl_Remision As DataTable = New DataTable
        If Id_Venta <> "" Then
            Tbl_Remision = cn.fn_ConsultaR1(CInt(Id_Venta))
            Alerta.Text = String.Empty
            ddl_tipo.SelectedIndex = 0
        End If
        If Tbl_Remision.Rows.Count = 0 Then
            Tbl_Remision = cn.fn_ConsultaR2(CInt(Num_Remision))
            Alerta.Text = String.Empty
            ddl_tipo.SelectedIndex = 1        
        End If
        If Tbl_Remision.Rows.Count = 0 Then
            Tbl_Remision = cn.fn_ConsultaR3(CInt(Num_Remision))
            Alerta.Text = String.Empty
            ddl_tipo.SelectedIndex = 2            
        End If
        If Tbl_Remision.Rows.Count = 0 Then
            Tbl_Remision = cn.fn_ConsultaR4(CInt(Num_Remision))
            Alerta.Text = String.Empty
            ddl_tipo.SelectedIndex = 3            
        End If
        If Tbl_Remision.Rows.Count = 0 Then
            Tbl_Remision = cn.fn_ConsultaR5(CInt(Num_Remision))
            Alerta.Text = String.Empty
            ddl_tipo.SelectedIndex = 4            
        End If
        If Tbl_Remision.Rows.Count = 0 Then
            Tbl_Remision = cn.fn_ConsultaR6(CInt(Num_Remision))
            Alerta.Text = String.Empty
            ddl_tipo.SelectedIndex = 5            
        End If
        If Tbl_Remision.Rows.Count = 0 Then            
            Alerta.Text = "*Remision no encontrada,verifique que sea correcta."
            Exit Sub
        End If                
        Data_Remision = Tbl_Remision        
        Complemetos()
        Response.Redirect("~/Traslado/ImprimirR.aspx")
    End Sub
    Sub Complemetos()
        Numeros_Env(CDbl(Num_Remision))
        Cant_M(cn.fn_ConsultaTraslado_GetMonedas(CDbl(Num_Remision)))
        Cant_Env(cn.fn_ConsultaTraslado_GetEnvases(CDbl(Num_Remision)))
        Tipo_Remision = ddl_tipo.SelectedValue
    End Sub
    Sub Numeros_Env(Id_Remision As Double)
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

    Protected Sub ddl_tipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_tipo.SelectedIndexChanged
        Alerta.Text = ""
        Data_Remision = Nothing
        txt_Remision.Focus()
    End Sub

    Protected Sub cbx_todos_CheckedChanged(sender As Object, e As EventArgs) Handles cbx_todos.CheckedChanged
        ddl_tipo.Enabled = Not cbx_todos.Checked
        Data_Remision = Nothing
        Alerta.Text = String.Empty
        txt_Remision.Focus()
    End Sub

    'Protected Sub Subirb_Click(sender As Object, e As EventArgs) Handles Subirb.Click
    '    'If Subir.HasFile Then
    '    '    Subir.SaveAs(Server.MapPath("~/Remision/" + Subir.FileName))
    '    '    fn_Alerta("Archivo guardado correctamente")
    '    'End If
    'End Sub

    'Protected Sub Descargar_Click(sender As Object, e As EventArgs) Handles Descargar1.Click
    '    'Response.Clear()
    '    'Response.ContentType = "application/octect-stream"
    '    'Response.AppendHeader("content-disposition", "filename=" + "Constancia1.pdf")
    '    'Response.TransmitFile(Server.MapPath("~/Remision/Constancia1.pdf"))
    '    'Response.End()
    'End Sub

End Class