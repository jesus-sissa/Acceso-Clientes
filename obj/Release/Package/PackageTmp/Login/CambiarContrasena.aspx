<%@ Page Title="" Language="vb" AutoEventWireup="false" CodeBehind="CambiarContrasena.aspx.vb"
     Inherits="PortalSIAC.CambiarContrasena" %>
   
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Cambiar Contraseña</title>
        <meta name="viewport" content="width=device-width, initial-scale=1" />
   
</head>

<body>

    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolScriptManager1" runat="server" EnableScriptGlobalization="True" />

        <div id="Login" class="card card-container">
         
                <div style="font-size: large; text-align: center; height: 40px; background-color: transparent; border: 0px; margin-top: 6px; padding-top: 8px">
                    <asp:Label ID="Label1" runat="server" Text="CAMBIAR CONTRASEÑA"></asp:Label>
                </div>

                <input type="password" class="form-control" runat="server" id="pass_actual" title="Contraseña Actual" name="pass_actual" maxlength="10" placeholder="Contraseña Actual" />
                <input type="password" class="form-control" runat="server" id="pass_nuevo" title="Nueva Contraseña" name="pass_nuevo" maxlength="10" placeholder="Nueva Contraseña" />
                <input type="password" class="form-control" runat="server" id="pass_confirmar" title="Confirmar Contraseña" name="pass_confirmar" maxlength="10" placeholder="Confirmar Contraseña" />
                <br />
                <br />
                <asp:Button runat="server" ID="btn_Guardar" class="btn_aceptar" Text="Guardar" />
                <br />
                <br />
                <asp:Button runat="server" ID="btn_Cancelar" class="btn_aceptar" Text="Cancelar" />
          
        </div>
    </form>
</body>
</html>


