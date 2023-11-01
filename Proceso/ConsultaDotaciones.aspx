<%@ Page Title="Proceso>Consulta de Dotaciones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaDotaciones.aspx.vb" Inherits="PortalSIAC.ConsultaDotaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="border: 1px solid Gray">
        <table>
            <tr>
                <td align="right">Fecha Inicial
                </td>
                <td align="left">
                    <asp:TextBox ID="txt_FInicial" runat="server" AutoPostBack="true" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FInicial_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td></td>
                <td style="text-align: right">Fecha Final
                    <asp:TextBox ID="txt_FFinal" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>

            <tr>
                <td align="right">Cliente
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server" AutoPostBack="true"
                        DataTextField="NombreComercial" Width="400px"
                        DataValueField="Id_Cliente">
                    </asp:DropDownList>
                </td>
                <td style="width: 80px">
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
            </tr>

            <tr>
                <td align="right">Moneda
                </td>
                <td align="left" colspan="3">
                    <asp:DropDownList ID="ddl_Moneda" runat="server" DataTextField="Nombre" DataValueField="Id_Moneda"
                        AutoPostBack="True" Height="20px" Width="117px">
                    </asp:DropDownList>

                    <asp:CheckBox ID="cbx_Moneda" runat="server" AutoPostBack="True" Text="Todas" />
                </td>

            </tr>
            <tr>

                <td align="right">Status
                </td>
                <td align="left" colspan="2">
                    <asp:DropDownList ID="ddl_Status" runat="server" AutoPostBack="True">
                        <asp:ListItem Text="ENTREGADAS" Value="EN" />
                        <asp:ListItem Text="EN TRANSITO" Value="DE" />
                        <asp:ListItem Text="PENDIENTES" Value="PE" />
                        <asp:ListItem Text="CANCELADAS" Value="CA" />
                        <asp:ListItem Selected="True" Value="0">Seleccione...</asp:ListItem>
                    </asp:DropDownList>

                    <asp:CheckBox ID="cbx_Status" runat="server" AutoPostBack="True" Text="Todos" />
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
                </td>

            </tr>
        </table>
    </fieldset>

    <fieldset id="fst_Dotaciones" runat="server" style="overflow: auto; border: 1px solid Gray; padding-left: 5px; padding-bottom: 5px">
        <legend>Dotaciones</legend>

        <asp:GridView ID="gv_Resultado" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" DataKeyNames="Id_Dotacion,Id_Remision" PageSize="25">
            <Columns>

                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Remision" HeaderText="Remisión" HeaderStyle-HorizontalAlign="Right">
                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-HorizontalAlign="Left">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="Moneda" HeaderText="Moneda" />
                <asp:BoundField DataField="Importe" DataFormatString="{0:n}" HeaderText="Importe">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Envases" HeaderText="Envases">
                    <ItemStyle HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:ButtonField ButtonType="Image" CommandName="VerDes" ImageUrl="~/Imagenes/Detalle.png" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Height="26px" Width="90px" CssClass="button" />
    </fieldset>

    <fieldset id="fst_DesgloseDot" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Desglose</legend>
        <asp:GridView ID="gv_Desglose" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="gv_general"
            CellSpacing="0" PageSize="15" Width="349px">
            <Columns>
                <asp:BoundField DataField="Presentacion" HeaderText="Presentacion">
                    <ItemStyle Width="85px" />
                </asp:BoundField>

                <asp:BoundField DataField="Denominacion" HeaderText="Denominación">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" Width="85px" />
                </asp:BoundField>
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>

            </Columns>
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

        </asp:GridView>
    </fieldset>

</asp:Content>
