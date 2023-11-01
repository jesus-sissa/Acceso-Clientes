<%@ Page Title="Traslado>Rastreo de Remisiones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="RastreoRemisiones.aspx.vb" Inherits="PortalSIAC.RastreoRemisiones1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <fieldset style="border:1px solid Gray; padding-left:5px">
        <legend>Datos Remision</legend>
       <table>
        <tr>
            <td align="right">
             <b>Remisión</b>
             </td>
            <td align="right">
                <asp:TextBox ID="txt_Remision" runat="server" MaxLength="14" />
                <asp:TextBoxWatermarkExtender ID="txt_Remision_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txt_Remision" WatermarkText="Numero de remisión"
                    WatermarkCssClass="WaterMark">
                </asp:TextBoxWatermarkExtender>

                  <asp:FilteredTextBoxExtender ID="txt_Remision_FilteredTextBoxExtender"
                        runat="server" Enabled="True" TargetControlID="txt_Remision"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>

                <asp:Button ID="btn_Buscar" runat="server" Text="Buscar" Height="26px" Width="90px" CssClass="button" />
            </td>
        </tr>
    </table>
    <br />
    <table>
        <tr>
            <td align="right">
                Numero de Remision
            </td>
            <td align="right">
                <asp:TextBox ID="txt_NumeroRemision" runat="server" 
                    ReadOnly="True" Width="95px"   ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Fecha de Captura
            </td>

            <td align="right">
                <asp:TextBox ID="txt_FechaCaptura" runat="server" ReadOnly="True" Width="95px"></asp:TextBox>
            </td>

            <td align="right">
                Hora Captura
            </td>
            <td align="right">
                <asp:TextBox ID="txt_HoraCaptura" runat="server" ReadOnly="True" Width="40px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                Importe
            </td>
            <td align="right">
                <asp:TextBox ID="txt_Importe" runat="server" ReadOnly="True" Width="95px"></asp:TextBox>
            </td>
            <td>
                Envases Sin Numero
            </td>
            <td align="right">
                <asp:TextBox ID="txt_EnvasesSN" runat="server" ReadOnly="True" Width="40px"></asp:TextBox>
            </td>
        </tr>
    </table>

     </fieldset>

    <fieldset style="border:1px solid Gray; padding-left:5px">
        <legend>Importes</legend>
        <asp:GridView ID="gv_Importes" runat="server" AutoGenerateColumns="False"
            CellPadding="4"  Width="400px"
            CssClass="gv_general">
            <EmptyDataTemplate>
                No se Encontraron Importes...
            </EmptyDataTemplate>
                       <Columns>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Importe" HeaderText="Efectivo" DataFormatString="{0:n2}">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Documentos" HeaderText="Documentos" DataFormatString="{0:n2}">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>

        
  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
 
        </asp:GridView>
    </fieldset>
    <br />
    <fieldset  style="border:1px solid Gray; padding-left:5px">
        <legend>Envases</legend>
        <asp:GridView ID="gv_Envases" runat="server" AutoGenerateColumns="False"
            CellPadding="4"  Width="400px"
            CssClass="gv_general">
            <EmptyDataTemplate>
                No se Encontraron Envases...
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                <asp:BoundField DataField="Numero" HeaderText="Numero" />
            </Columns>
              <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
        </asp:GridView>
    </fieldset>

    <fieldset style="border:1px solid Gray;padding-left:5px">
        <legend>Log</legend>
        <asp:GridView ID="gv_Log" runat="server" AutoGenerateColumns="false"
            CssClass="gv_general" CellPadding="4" Width="400px">
            <EmptyDataTemplate>
                No se Encontraron Envases...
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="Concepto" HeaderText="Concepto" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="Hora" HeaderText="Hora" />
            </Columns>
             <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
        </asp:GridView>
    </fieldset>
</asp:Content>
