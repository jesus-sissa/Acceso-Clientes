<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Cheques.aspx.vb" 
    Inherits="PortalSIAC.Cheques" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <fieldset>
        <legend>Frente</legend>
            <table>
            <tr>
                <td>
                    <asp:Image ID="img_Frente" runat="server" Visible="true" Width="600px" Height="200px" />
                </td>
            </tr>
            </table>
    </fieldset>
    <br />
    <fieldset>
        <legend>Reverso</legend>
            <table>
            <tr>
                <td>
                    <asp:Image ID="img_Reverso" runat="server" Visible="true" Width="600px" Height="200px" />
                </td>
            </tr>
            </table>
    </fieldset>
    </div>
    
    </form>
</body>
</html>
