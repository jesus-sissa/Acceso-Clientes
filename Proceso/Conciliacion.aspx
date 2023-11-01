<%@ Page Title="Proceso>Conciliacion" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="Conciliacion.aspx.vb" Inherits="PortalSIAC.Conciliacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="fst_filtroDeposito" style="border: 1px solid Gray">
        <table>
            <tr>
                <td style="text-align: right">
                    <label>Fecha Inicial</label>
                </td>

                <td>
                    <asp:TextBox ID="txt_FInicial" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" CssClass="calendarioAjax"
                        TargetControlID="txt_FInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td></td>
                <td style="text-align: right">
                    <label>Fecha Final</label>
                    <asp:TextBox ID="txt_FFinal" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" runat="server" Enabled="True" CssClass="calendarioAjax"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <label>Cliente</label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server"
                        DataTextField="NombreComercial" Width="400px"
                        DataValueField="Id_Cliente" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td style="width: 80px">
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <td align="right">Caja Bancaria
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CajaBancaria" runat="server"
                        DataTextField="Nombre_Comercial" Width="400px"
                        DataValueField="Id_CajaBancaria" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar"
                        Style="height: 26px" Width="90px" CssClass="button" />
                </td>
            </tr>
        </table>

    </fieldset>

    <fieldset id="fst_conciliacion" runat="server" style="overflow: auto; left: 0px; border: 1px solid Gray; padding-left: 5px; padding-bottom: 2px">
        <legend>Conciliacion</legend>
        <div id="div_conciliacion" runat="server" style="width: 100%; float: left">

            <div id="div_TV" runat="server" style="width: 50%; float: left">
                <fieldset id="fst_TV" runat="server" style="overflow: auto; left: 0px; border: 1px solid Gray; padding-left: 5px">
                    <legend>Trasladado</legend>

                    <asp:GridView ID="gv_Conciliacion" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_Remision"
                        CssClass="gv_general" AllowPaging="True" CellSpacing="1" Width="100%"
                        PageSize="25" EnableModelValidation="True">
                        <Columns>

                            <asp:BoundField DataField="Remision" HeaderText="Remisión">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle CssClass="gv_Item" Width="90px" Font-Bold="False" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="FechaBoveda" HeaderText="Fecha Entrada">
                                <ItemStyle CssClass="gv_Item" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle CssClass="gv_Item" Width="400px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c2}">
                                <ItemStyle HorizontalAlign="Right" CssClass="gv_Item" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Status" HeaderText="Status">
                                <ItemStyle CssClass="gv_Item" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>

                    <table id="tbl_Importe">
                        <tr>
                            <td align="right">
                                <asp:Label ID="lbl_Importe" runat="server" Text="Importe Total"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_ImporteTotal" runat="server" AutoPostBack="True" ReadOnly="True" Width="170px" Height="20px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                </fieldset>
            </div>

            <div id="div_proceso" runat="server" style="width: 49%; float: right">
                <fieldset id="fst_proceso" runat="server" style="overflow: auto; left: 0px; border: 1px solid Gray; padding-left: 5px">
                    <legend>Procesado</legend>

                    <asp:GridView ID="gv_Proceso" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_Remision"
                        CssClass="gv_general" AllowPaging="True" CellSpacing="1" Width="100%"
                        PageSize="25" EnableModelValidation="True">
                        <Columns>

                            <asp:BoundField DataField="Remision" HeaderText="Remisión">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle CssClass="gv_Item" Width="90px" Font-Bold="False" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Fecha" HeaderText="Fecha Procesa">
                                <ItemStyle CssClass="gv_Item" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-HorizontalAlign="Left">
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                <ItemStyle CssClass="gv_Item" Width="400px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Status" HeaderText="Status">
                                <ItemStyle CssClass="gv_Item" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>

                    <asp:Button ID="btn_Exportar" runat="server"
                        Text="Exportar" Height="26px" Width="90px" CssClass="button" />
                </fieldset>
            </div>
            <br />
            <div id="div_Archivos" runat="server" style="width: 100%; float: none">
                <fieldset id="fst_Archivos" runat="server" style="width: 49.3%; overflow: auto; left: 0px; border: 1px solid Gray; padding-left: 5px">
                    <legend>Archivos .txt</legend>

                    <asp:GridView ID="gv_Archivos" runat="server" CellPadding="10" AutoGenerateColumns="False"
                        CssClass="gv_general" AllowPaging="True" CellSpacing="1" Width="100%"
                        PageSize="25" EnableModelValidation="True" DataKeyNames="Id_Remision">
                        <Columns>

                            <asp:BoundField DataField="Remision" HeaderText="Remisión">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle CssClass="gv_Item" Width="90px" Font-Bold="False" HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Genera" HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha Genera">
                                <ItemStyle CssClass="gv_Item" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Aplicacion" HeaderStyle-HorizontalAlign="Left" HeaderText="Fecha Aplicacion">
                                <ItemStyle CssClass="gv_Item" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Archivo" HeaderText="No. Archivo">
                                <ItemStyle CssClass="gv_Item" HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                    </asp:GridView>

                </fieldset>
            </div>
        </div>
    </fieldset>


</asp:Content>
