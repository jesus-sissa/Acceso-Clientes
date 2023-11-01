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


Partial Public Class ValidacionTripulacion

    '''<summary>
    '''Control hdf_ImagenesDisponibles.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hdf_ImagenesDisponibles As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control txt_FechaInicial.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_FechaInicial As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control txt_Fecha_CalendarExtender.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txt_Fecha_CalendarExtender As Global.AjaxControlToolkit.CalendarExtender

    '''<summary>
    '''Control ddl_Clientes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddl_Clientes As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cbx_Todos_Clientes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cbx_Todos_Clientes As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control btn_Mostrar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btn_Mostrar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control fst_Unidades.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fst_Unidades As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Control gv_Lista.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gv_Lista As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control fst_Tripulacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fst_Tripulacion As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    '''<summary>
    '''Control pnl_Tripulacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnl_Tripulacion As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control img_UnidadTV.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents img_UnidadTV As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lbl_Unidad.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Unidad As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_Placas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Placas As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control img_Operador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents img_Operador As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control img_OperadorFirma.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents img_OperadorFirma As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lbl_Operador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Operador As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_OperadorClave.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_OperadorClave As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control img_Cajero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents img_Cajero As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control img_CajeroFirma.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents img_CajeroFirma As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lbl_Cajero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_Cajero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lbl_CajeroClave.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lbl_CajeroClave As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control dl_Custodios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents dl_Custodios As Global.System.Web.UI.WebControls.DataList

    '''<summary>
    '''Control btnValidar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnValidar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control btnImprimir.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnImprimir As Global.System.Web.UI.WebControls.Button
End Class
