<%@ Page  Title="Soporte>Notificaciones" Language="vb" AutoEventWireup="false" CodeBehind="Notificaciones.aspx.vb"
     Inherits="PortalSIAC.Notificaciones" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content_Retiros" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="divNotificaciones" style="width:95%; height:700px; overflow:auto">
       <asp:PlaceHolder ID="PlaceHolderNotifica" runat="server" >
       
        </asp:PlaceHolder>
    
    </div>

</asp:Content>