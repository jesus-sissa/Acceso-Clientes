
Partial Public Class SolicitudServicios
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache) '---------->

        If IsPostBack Then Exit Sub
        Call cn.fn_Crear_Log(pId_Login, "PAGINA: SOLICITUD DE SERVICIOS")

        Dim dt_Clientes As DataTable = cn.fn_ConsultaClientes()

        If dt_Clientes Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If

        If pNivel = 2 Then
            'Nivel=1 Puede ver todo
            'Nivel=2 Es local solo puede ver lo de el(Sucursal)
            ddl_Clientes.SelectedValue = pId_ClienteOriginal
            ddl_Clientes.Enabled = False
        End If

        fn_LlenarDropDown(ddl_Clientes, dt_Clientes, False)
        Dim dt_Servicios As DataTable = cn.fn_SolicitudServicios_GetServicios(ddl_Clientes.SelectedValue)

        If dt_Servicios Is Nothing Then
            Fn_Alerta("No se puede continuar debido a un error.")
            Exit Sub
        End If
        fn_LlenarDropDown(ddl_Servicio, dt_Servicios.Copy(), False)

        Dim dt_Horas As DataTable = cn.GetHoras(15)

        fn_LlenarDropDown(ddl_A, dt_Horas.Copy(), False)

        fn_LlenarDropDown(ddl_De, dt_Horas.Copy(), False)

        fn_LlenarDropDown(ddl_AD, dt_Horas.Copy(), False)

        fn_LlenarDropDown(ddl_DeD, dt_Horas.Copy(), False)

        Call CreaOrigenDestino()

    End Sub

    Protected Function fn_Validar() As Boolean

        If ddl_Servicio.SelectedValue = 0 Then
            Fn_Alerta("Debe seleccionar un servicio.")
            Return False
        End If

        If txt_Fecha.Text = "" Then
            Fn_Alerta("Debe seleccionar una fecha.")
            Return False
        End If

        Dim Fecha As Date

        If Not Date.TryParse(txt_Fecha.Text, Fecha) Then
            Fn_Alerta("Debe Seleccionar Una Fecha Valida.")
            Exit Function
        End If

        If Fecha.Date < Today.Date Then
            Fn_Alerta("Debe Seleccionar Una Fecha Valida.")
            Exit Function
        End If

        If ddl_Origen.SelectedValue = 0 Then
            Fn_Alerta("Debe seleccionar un origen.")
            Return False
        End If

        If ddl_De.SelectedItem.Text = "" Then
            Fn_Alerta("Debe seleccionar una hora de recoleccion mínima.")
            Return False
        End If

        If ddl_A.SelectedItem.Text = "" Then
            Fn_Alerta("Debe seleccionar una hora de recoleccion máxima.")
            Return False
        End If

        If CDate(ddl_De.Text) >= CDate(ddl_A.Text) Then
            Fn_Alerta("Debe Capturar una hora de recoleccion valida.")
            Exit Function
        End If

        If ddl_Destino.SelectedValue = 0 Then
            Fn_Alerta("Debe seleccionar un destino.")
            Return False
        End If

        If ddl_DeD.SelectedItem.Text = "" Then
            Fn_Alerta("Debe seleccionar una hora de entrega mínima.")
            Return False
        End If

        If ddl_AD.SelectedItem.Text = "" Then
            Fn_Alerta("Debe seleccionar una hora de entrega máxima.")
            Return False
        End If

        If ddl_Origen.SelectedValue = ddl_Destino.SelectedValue Then
            Fn_Alerta("El origen y el destino deben ser diferentes.")
            Return False
        End If

        If CDate(ddl_DeD.Text) >= CDate(ddl_AD.Text) Then
            Fn_Alerta("Debe Capturar una hora de entrega valida.")
            Exit Function
        End If

        Return True

    End Function

    Protected Sub btn_Solicitar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btn_Solicitar.Click

        If Not fn_Validar() Then Exit Sub

        Dim Fecha As Date = CDate(txt_Fecha.Text)
        Dim HoraRec As String = CDate(ddl_De.Text).ToString("HH:mm") & "/" & CDate(ddl_A.Text).ToString("HH:mm")
        Dim HoraEnt As String = CDate(ddl_DeD.Text).ToString("HH:mm") & "/" & CDate(ddl_AD.Text).ToString("HH:mm")

        If Not cn.fn_SolicitudServicios_Save(Fecha, ddl_Clientes.SelectedValue, ddl_Clientes.SelectedItem.Text, ddl_Servicio.SelectedValue, ddl_Servicio.SelectedItem.Text, _
                                             ddl_Origen.SelectedValue, ddl_Origen.SelectedItem.Text, HoraRec, ddl_Destino.SelectedValue, ddl_Destino.SelectedItem.Text, HoraEnt, txt_Comentarios.Text) Then
            Fn_Alerta("Ha ocurrido un error al intentar guardar su solicitud.")
            Exit Sub
        Else
            Call cn.fn_Crear_Log(pId_Login, "SE GUARDO SOLICITUD DE SERVICIO PARA LA FECHA: " & Fecha & " / SERVICIO: " & ddl_Servicio.SelectedItem.Text & " / CLIENTE SOLICITA: " & ddl_Clientes.SelectedItem.Text & " / CLIENTE ORIGEN: " & ddl_Origen.SelectedItem.Text & _
                                 " CON HORARIO: " & HoraRec & " / DESTINO: " & ddl_Destino.SelectedItem.Text & " CON HORARIO: " & HoraEnt)
        End If

        Fn_Alerta("La solicitud se ha realizado correctamente.")
        Call limpiarControles()
    End Sub

    Private Sub limpiarControles()
        txt_Fecha.Text = ""
        ddl_Clientes.SelectedValue = 0
        ddl_Servicio.SelectedValue = 0
        ddl_Origen.SelectedValue = 0
        ddl_Destino.SelectedValue = 0
        ddl_De.SelectedIndex = 0
        ddl_A.SelectedIndex = 0
        ddl_DeD.SelectedIndex = 0
        ddl_AD.SelectedIndex = 0
        txt_Comentarios.Text = ""
    End Sub

    Protected Sub ddl_Clientes_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddl_Clientes.SelectedIndexChanged

        Dim dt_Servicios As DataTable = cn.fn_SolicitudServicios_GetServicios(ddl_Clientes.SelectedValue)
        fn_LlenarDropDown(ddl_Servicio, dt_Servicios, False)
        If ddl_Servicio.Items.Count = 2 Then ddl_Servicio.SelectedIndex = 1
        If ddl_Clientes.SelectedValue = 0 Then
            fn_LlenarDropDownVacio(ddl_Origen)
            fn_LlenarDropDownVacio(ddl_Destino)
        Else
            Call CreaOrigenDestino()
        End If

    End Sub

    Private Sub CreaOrigenDestino()
        'Dim ds_OrigenesDestino As DataSet = cn.fn_SolicitudServicios_GetOrigenDestino(ddl_Clientes.SelectedValue)

        Dim dt_Origen As DataTable = cn.fn_SolicitudServicios_GetOrigen(ddl_Clientes.SelectedValue)
        If dt_Origen IsNot Nothing Then
            fn_LlenarDropDown(ddl_Origen, dt_Origen, False)
            If ddl_Origen.Items.Count = 2 Then ddl_Origen.SelectedIndex = 1
        End If

        Dim dt_Destino As DataTable = cn.fn_SolicitudServicios_GetDestino(ddl_Clientes.SelectedValue)
        If dt_Destino IsNot Nothing Then
            fn_LlenarDropDown(ddl_Destino, dt_Destino, False)
            If ddl_Destino.Items.Count = 2 Then ddl_Destino.SelectedIndex = 1
        End If
    End Sub

    Protected Sub ddl_De_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_De.SelectedIndexChanged
        If ddl_De.SelectedIndex = 0 Then Exit Sub

        If ddl_De.SelectedIndex <= ddl_De.Items.Count - 3 Then
            ddl_A.SelectedIndex = ddl_De.SelectedIndex + 2

            ddl_DeD.SelectedIndex = ddl_De.SelectedIndex + 6
            ddl_AD.SelectedIndex = ddl_DeD.SelectedIndex + 2
        Else

            ddl_A.SelectedIndex = (ddl_De.Items.Count - (ddl_De.SelectedIndex - 1))

            ddl_DeD.SelectedIndex = ddl_A.SelectedIndex + 4
            ddl_AD.SelectedIndex = ddl_DeD.SelectedIndex + 2
        End If

    End Sub

    Protected Sub ddl_DeD_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddl_DeD.SelectedIndexChanged
        If ddl_DeD.SelectedIndex = 0 Then Exit Sub

        If ddl_DeD.SelectedIndex <= ddl_DeD.Items.Count - 3 Then
            ddl_AD.SelectedIndex = ddl_DeD.SelectedIndex + 2
        Else
            ddl_AD.SelectedIndex = (ddl_DeD.SelectedIndex - (ddl_DeD.SelectedIndex - 1))
        End If
    End Sub
End Class