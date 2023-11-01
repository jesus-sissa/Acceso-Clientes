<%@ Page Title="Proceso>Consulta de Diferencias" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ConsultaDiferencias.aspx.vb" Inherits="PortalSIAC.ConsultaDiferencias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>Fecha Inicial
            </td>
            <td>
                <asp:TextBox ID="txt_FInicial" runat="server" CssClass="calendarioAjax" />
                <asp:CalendarExtender ID="txt_FInicial_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                    TargetControlID="txt_FInicial" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
            <td>Tipo
            </td>
            <td>
                <asp:DropDownList ID="ddl_Tipo" runat="server">
                    <asp:ListItem Text="Contra Remision" Value="C" />
                    <asp:ListItem Text="Contra Ficha" Value="F" />
                    <asp:ListItem Text="Contra Parcial" Value="P" />
                    <asp:ListItem Selected="True" Value="0">Seleccione...</asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox ID="cbx_Tipo" runat="server" AutoPostBack="True" Text="Todas" />
            </td>
        </tr>
        <tr>
            <td>Fecha Final
            </td>
            <td>
                <asp:TextBox ID="txt_FFinal" runat="server" CssClass="calendarioAjax" />
                <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                    TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>
            <td>&nbsp;
            </td>
            <td>
                <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
            </td>
        </tr>
    </table>
    <fieldset class="fieldsetFijo">
        <legend>Resultados</legend>
        <asp:GridView ID="gv_Resultados" runat="server" CssClass="gv_general" EnableModelValidation="True">
        </asp:GridView>
    </fieldset>
</asp:Content>
