'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class ConsultaDotacionesCajeros

    '''<summary>
    '''Control lbl_fechainicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_fechainicio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Fechainicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Fechainicio As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control tbx_Fechainicio_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Fechainicio_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control tbx_Fechainicio_FilteredTextBoxExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Fechainicio_FilteredTextBoxExtender As Global.AjaxControlToolkit.FilteredTextBoxExtender

    '''<summary>
    '''Control lbl_fechafin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_fechafin As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Fechafin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Fechafin As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control tbx_Fechafin_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Fechafin_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control tbx_Fechafin_FilteredTextBoxExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Fechafin_FilteredTextBoxExtender As Global.AjaxControlToolkit.FilteredTextBoxExtender

    '''<summary>
    '''Control lbl_cajero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_cajero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_Cajero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_Cajero As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control chk_Cajeros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chk_Cajeros As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control lbl_Status.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Status As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control ddl_Status.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_Status As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control chk_Status.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chk_Status As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control btn_Mostrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Mostrar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control fst_dotaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fst_dotaciones As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Control gv_Dotaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_Dotaciones As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control gv_DetalleD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_DetalleD As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control lbl_cambiopapel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_cambiopapel As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rdb_cambiopapelSI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_cambiopapelSI As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_cambiopapelNO.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_cambiopapelNO As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control lbl_Observaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Observaciones As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_descargoinfo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_descargoinfo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rdb_descargoinfoSI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_descargoinfoSI As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_descargoinfoNO.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_descargoinfoNO As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control tbx_Observaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Observaciones As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lbl_huboretiro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_huboretiro As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control rdb_huboretiroSI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_huboretiroSI As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control rdb_huboretiroNO.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rdb_huboretiroNO As Global.System.Web.UI.WebControls.RadioButton

    '''<summary>
    '''Control lbl_remision.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_remision As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Remision.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Remision As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lbl_importe.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_importe As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Importe.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Importe As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control lbl_tarjetasretenidas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_tarjetasretenidas As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_Teoricas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Teoricas As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_Reales.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Reales As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control tbx_Teoricas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Teoricas As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control tbx_Reales.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tbx_Reales As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control gv_TarjetasFallas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_TarjetasFallas As Global.System.Web.UI.WebControls.GridView
End Class
