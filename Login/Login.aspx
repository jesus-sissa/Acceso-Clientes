<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb"
    Inherits="PortalSIAC.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Assembly="BotDetect" Namespace="BotDetect.Web.UI"   TagPrefix="BotDetect" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Login PortalSIAC</title>
    <link rel="icon" href="../Imagenes/fav.png" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
  
    <script src='https://www.google.com/recaptcha/api.js'></script>
 

</head>

<body>
   

    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolScriptManager1" runat="server" />
        <div id="Login" class="card card-container">

            <div class="profile-img-card">
                <asp:Label runat="server" ID="titulosiac" Text="SIAC"></asp:Label>
            </div>
            <input type="text" class="form-control" runat="server" style="text-transform: uppercase" id="cunica" title="R.F.C. (Sin Guiiones Ni Espacios)" name="cunica" maxlength="15" placeholder="R.F.C.(Sin Guiones)" />
            <input type="text" class="form-control" runat="server" style="text-transform: uppercase" id="usuario" title="Clave Usuario" name="usuario" maxlength="10" placeholder="Clave Usuario" />
            <input type="password" class="form-control" runat="server" id="password" title="Contraseña" name="password" maxlength="10" placeholder="Contraseña" />

             <%--<div style="text-align: center; max-height: 575px; transform: scale(0.86); -webkit-transform: scale(0.86); transform-origin: 0 0; -webkit-transform-origin: 0 0" class="g-recaptcha" data-theme="light" data-sitekey="6LdtDQwUAAAAAJ67OzCXRXm8SKt3MX8tRtMsCRif"></div>--%>
            
            <asp:Button runat="server" ID="btn_Entrar" class="btn_aceptar" Text="Entrar" />

        </div>
   
    </form>

</body>

</html>
