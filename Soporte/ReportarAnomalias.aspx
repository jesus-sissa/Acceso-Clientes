<%@ Page Title="Soporte>Nuevo ticket" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ReportarAnomalias.aspx.vb" Inherits="PortalSIAC.ReportarAnomalias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="border: 1px solid Gray">

        <table style="width: 100%">
            <tr>
                <td style="text-align: right; width: 120px">Localidad</td>
                <td>
                    <asp:DropDownList ID="ddl_Localidad" runat="server" Width="300px" DataTextField="Nombre_Sucursal"
                        DataValueField="Clave_Sucursal" AutoPostBack="True" Enabled="False">
                    </asp:DropDownList>

                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 120px">Sucursal</td>
                <td>
                    <asp:DropDownList ID="ddl_Sucursales" runat="server" Width="300px" DataTextField="Nombre_Sucursal"
                        DataValueField="Clave_Sucursal" AutoPostBack="True" Enabled="False">
                    </asp:DropDownList>

                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 120px">Departamento</td>
                <td>
                    <asp:DropDownList ID="ddl_Rubros" runat="server" Width="300px" DataTextField="Nombre"
                        DataValueField="Id_Rubro" AutoPostBack="True" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 120px">Descripcion
                </td>
                <td>
                    <asp:TextBox ID="descripcion_falla" runat="server" Width="99%" Height="120px"
                        TextMode="MultiLine" CssClass="tbx_Mayusculas" />
                </td>
            </tr>
          
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btn_Enviar" runat="server" Text="Enviar"
                        Height="26px" Width="90px" CssClass="button" />
                </td>
            </tr>
        </table>

    </fieldset>
</asp:Content>
