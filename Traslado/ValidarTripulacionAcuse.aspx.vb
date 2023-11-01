

Public Class ValidarTripulacionAcuse
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If IsPostBack Then Exit Sub

        Dim Identificador As String = Session("Identificador")

        Dim dtCustodios As DataTable = cn.fn_ValidacionTripulacionAcuse_GetTripulacion(Identificador)
        If dtCustodios Is Nothing Then
            fn_Alerta("No se puede mostrar la tripulación")
            Exit Sub
        End If


        lbl_FechaValidacion.Text = dtCustodios.Rows(0)("Fecha")
        lbl_HoraValidacion.Text = dtCustodios.Rows(0)("Hora")
        lbl_NombreUsuario.Text = dtCustodios.Rows(0)("Nombre_Sesion") & " - " & dtCustodios.Rows(0)("UsuarioNombre")
        lbl_Folio.Text = dtCustodios.Rows(0)("IDP") & "-" & dtCustodios.Rows(0)("Identificador")
        lbl_CiaTV.Text = dtCustodios.Rows(0)("CiaTV")
        lbl_Fecha.Text = dtCustodios.Rows(0)("FechaPunto")
        lbl_Origen.Text = dtCustodios.Rows(0)("Origen")
        lbl_Destino.Text = dtCustodios.Rows(0)("Destino")
        lbl_Unidad.Text = dtCustodios.Rows(0)("Unidad")

        lbl_Cajero.Text = dtCustodios.Rows(0)("ClaveCajero") & " " & dtCustodios.Rows(0)("Cajero")
        lbl_Operador.Text = dtCustodios.Rows(0)("ClaveOperador") & " " & dtCustodios.Rows(0)("Operador")
        lbl_Custodio1.Text = dtCustodios.Rows(0)("ClaveCustodio1") & " " & dtCustodios.Rows(0)("Custodio1")



        If dtCustodios.Rows(0)("Custodio2").ToString.Trim = "" Then
            lbl_Custodio2.Text = ""
            lbl_tdCustodio2.Text = ""
        Else
            lbl_Custodio2.Text = dtCustodios.Rows(0)("ClaveCustodio2") & " " & dtCustodios.Rows(0)("Custodio2")
            lbl_tdCustodio2.Text = "Custodio:"
        End If

    End Sub

    Protected Sub btn_Imprimir_Click(sender As Object, e As EventArgs) Handles btn_Imprimir.Click
        Dim open As String
        open = "<script>window.print();</script>"
        ClientScript.RegisterStartupScript(Me.GetType(), "IMPRIMIR", open)        
    End Sub
End Class