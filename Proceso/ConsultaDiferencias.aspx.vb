Public Partial Class ConsultaDiferencias
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Call cn.fn_Crear_Log(pId_Login, "ACCEDIO A: CONSULTA DE DIFERENCIAS")
    End Sub

    Protected Sub btn_Mostrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Mostrar.Click
        If txt_FInicial.Text.Trim = "" Then
            fn_Alerta("Debe Seleccionar una Fecha Inicial.")
            Exit Sub
        End If

        Dim FInicial As Date

        If Not Date.TryParse(txt_FInicial.Text, FInicial) Then
            fn_Alerta("Debe Seleccionar una Fecha Inicial Valida.")
            Exit Sub
        End If

        If txt_FFinal.Text.Trim = "" Then
            fn_Alerta("Debe Seleccionar una Fecha Final")
            Exit Sub
        End If

        Dim FFinal As Date

        If Not Date.TryParse(txt_FFinal.Text, FFinal) Then
            fn_Alerta("Debe Seleccionar una Fecha Final Valida")
            Exit Sub
        End If

    End Sub
End Class