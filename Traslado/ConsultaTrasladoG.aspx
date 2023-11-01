<%@ Page Title="Traslado>Consulta de Traslados Globales" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaTrasladoG.aspx.vb" Inherits="PortalSIAC.ConsultaTrasladoG" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="border: 1px solid Gray">
        <table>

            <tr>
                <td style="text-align: right">Fecha Inicial
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaInicial" runat="server" CssClass="calendarioAjax" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td></td>
                <td style="text-align: right">Fecha Final
                   
                 <asp:TextBox ID="txt_FechaFinal" runat="server" CssClass="calendarioAjax" AutoPostBack="True" />
                    <asp:CalendarExtender ID="CalendarExtender1" CssClass="calendarioAjax" runat="server" Enabled="True" TargetControlID="txt_FechaFinal"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                    
                </td>

            </tr>

            <tr>
                <td style="text-align: right">Clientes
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="Nombre_Comercial" Width="400"
                        DataValueField="Id_Cliente" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td style="width: 80px">
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px"  CssClass="button"/>
                </td>
            </tr>
        </table>
    </fieldset>

    <fieldset id="fst_TrasladosG" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Traslados Globales</legend>

        <asp:GridView ID="gv_Lista" runat="server" DataKeyNames="Id_Remision" CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="100%" AllowPaging="true"
            PageSize="25">
            <Columns>

                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Remision" HeaderText="Remision"
                    ItemStyle-Font-Bold="False" ItemStyle-Width="90px">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="80px" />
                <asp:BoundField DataField="Hora" HeaderText="Hora" ItemStyle-Width="70px" />

                <asp:BoundField DataField="Origen" HeaderText="Origen" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Destino" HeaderText="Destino" HeaderStyle-HorizontalAlign="Left"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe"
                    ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:BoundField>

            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
        <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Height="26px" Width="90px" CssClass="button" />
    </fieldset>

    <fieldset id="fst_detalleTrasladoG" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Detalle Traslado</legend>

        <table width="100%">
            <tr>
                <td>Monedas
                </td>

                <td>Envases
                </td>
            </tr>
            <tr>
                <td valign="top">

                    <asp:GridView ID="gvMonedas" runat="server" CellPadding="10" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="100%" DataKeyNames="Id_Moneda">

                        <Columns>
                            <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Efectivo" HeaderText="Efectivo">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Documentos" HeaderText="Documentos">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>
                </td>

                <td valign="top">
                    <asp:GridView ID="gvEnvases" runat="server" CellPadding="10" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="100%" DataKeyNames="Id_Envase">
                        <Columns>

                            <asp:BoundField DataField="Tipo Envase" HeaderText="Tipo Envase">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero" HeaderText="Numero">

                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                        </Columns>


                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    </asp:GridView>
                </td>
            </tr>

        </table>
    </fieldset>
</asp:Content>
