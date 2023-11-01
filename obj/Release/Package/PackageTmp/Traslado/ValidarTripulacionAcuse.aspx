<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ValidarTripulacionAcuse.aspx.vb" Inherits="PortalSIAC.ValidarTripulacionAcuse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style2 {
            width: 92px;
        }
        .auto-style4 {
            width: 141px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="tblTitulo"style="width:700px">
            <tr>
                <td colspan="4" style ="text-align:center"><h2>Acuse de validación de tripulación</h2> </td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style4">Fecha Validación:</td>
                <td>
                    <asp:Label ID="lbl_FechaValidacion" runat="server" Text="99/99/9999"></asp:Label>
                </td>
                <td style="text-align:right">Hora:</td>
                <td>
                    <asp:Label ID="lbl_HoraValidacion" runat="server" Text="00:00"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align:right" class="auto-style4">Usuario:</td>
                <td colspan="4">
                    <asp:Label ID="lbl_NombreUsuario" runat="server" Text="NombredeUsuario"></asp:Label>
                </td>
            </tr>
         </table>
        <table id="tblFormato" style="width:700px;border-style:solid;border-color:black; border-width:thin">
             <tr>
                <td colspan="2" style="text-align:center">
                    <h4>Datos del Traslado</h4>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Folio:
                </td>
                <td>
                    <asp:Label ID="lbl_Folio" runat="server" Text="000000"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Cia TV:
                </td>
                <td>
                    <asp:Label ID="lbl_CiaTV" runat="server" Text="Compañia"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Fecha de Ruta:
                </td>
                <td>
                    <asp:Label ID="lbl_Fecha" runat="server" Text="99/99/9999"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Origen:
                </td>
                <td>
                    <asp:Label ID="lbl_Origen" runat="server" Text="Origen del punto"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style2">
                    Destino:
                </td>
                <td>
                    <asp:Label ID="lbl_Destino" runat="server" Text="Destino del punto"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Unidad:
                </td>
                <td>
                    <asp:Label ID="lbl_Unidad" runat="server" Text="SVC-64455"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center">
                    <h4>Tripulación</h4>
                </td>
            </tr>
            <tr>
               <td>Cajero:</td>
                <td>
                    <asp:Label ID="lbl_Cajero" runat="server" Text="NombreCajero"></asp:Label>
                </td>
            </tr>
             <tr>
               <td>Operador:</td>
                <td>
                    <asp:Label ID="lbl_Operador" runat="server" Text="NombreOperador"></asp:Label>
                </td>
            </tr>
             <tr>
               <td id="tdCustodio1">Custodio:</td>
                <td>
                    <asp:Label ID="lbl_Custodio1" runat="server" Text="NombreCustodio 1"></asp:Label>
                </td>
            </tr>
            <tr>
               <td id ="tdCustodio2">
                   <asp:Label ID="lbl_tdCustodio2" runat="server" Text="Custodio:"></asp:Label>
               </td>
                <td>
                    <asp:Label ID="lbl_Custodio2" runat="server" Text="NombreCustodio 2"></asp:Label>
                </td>
            </tr>
            
        </table>
        <br />
        <asp:Button ID="btn_Imprimir" runat="server" Text="Imprimir Comprobante" CssClass="button" />
    </div>
    </form>
</body>
</html>
