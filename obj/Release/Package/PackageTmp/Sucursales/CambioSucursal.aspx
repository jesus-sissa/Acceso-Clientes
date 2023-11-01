<%@ Page Title="Sucursales>Cambio de Sucursal" Language="vb" AutoEventWireup="false"  MasterPageFile="~/MasterPage.Master"
    CodeBehind="CambioSucursal.aspx.vb" Inherits="PortalSIAC.CambioSucursal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" 
    TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <table  style="width: 263px" >
        <tr>
            <td colspan="2"  align="center">
            Cambio de Sucursal
            </td> 
        </tr>
        <tr>
            <td align ="right" >
                Sucursal
            </td> 
            <td >
                <asp:DropDownList ID="ddl_Sucursales" runat="server" Width="200px" 
                    DataTextField="Nombre_SucursalPropia" DataValueField="Cadena">
                
                </asp:DropDownList>
             </td> 
        </tr>
        <tr>
            <td>
            
            </td>
             <td>
                 <asp:Button ID="btn_CmabiarConexion" runat="server" Text="Cambiar" CssClass="button"
                     Height="26px" Width="90px" />
            </td>
        </tr> 
    </table>
    
</asp:Content>
