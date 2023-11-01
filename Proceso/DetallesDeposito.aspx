<%@ Page Title="Proceso>Detalles de Deposito" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="DetallesDeposito.aspx.vb" Inherits="PortalSIAC.DetallesDeposito" %>

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
                  <asp:TextBox ID="txt_FFinal" runat="server" AutoPostBack="true" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <label class="label">
                        Cliente</label>
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server" AutoPostBack="true"
                        DataTextField="Nombre_Comercial" Width="400px"
                        DataValueField="Id_Cliente">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="cbx_Todos_Clientes" Text="Todos" runat="server"
                        AutoPostBack="True" />
                </td>

            </tr>
            <tr>
                <td align="right">Caja Bancaria
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CajaBancaria" runat="server" AutoPostBack="true"
                        DataTextField="Nombre_Comercial" Width="400px"
                        DataValueField="Id_CajaBancaria">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">Moneda
                </td>
                <td align="left" colspan="3">
                    <asp:DropDownList ID="ddl_Moneda" runat="server" DataTextField="Nombre" DataValueField="Id_Moneda"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Button ID="Btn_Consultar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
                </td>
            </tr>

        </table>
    </fieldset>

    <fieldset id="fst_Depositos" runat="server" style="overflow: auto; border: 1px solid Gray; padding-left: 5px">
        <legend>Concentraciones o Depositos</legend>

        <asp:GridView ID="gv_Lista" runat="server"
            DataKeyNames="Id_Servicio" CellPadding="10" AutoGenerateColumns="False"
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

                <asp:BoundField DataField="Remision" HeaderText="Remisión" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                    <ItemStyle Width="90px" Font-Bold="False" />
                </asp:BoundField>

                <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="Cliente" HeaderText="Cliente " ItemStyle-Width="400px"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Dice_Contener" DataFormatString="{0:c2}" HeaderText="Dice Contener"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="ImporteReal" DataFormatString="{0:c2}" HeaderText="Importe Real"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="Diferencia" HeaderText="Diferencia"
                    HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:c2}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="Envases" HeaderText="Envases"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Envases SN" HeaderText="EnvasesSN"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">

                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <table>
            <tr>
                <td>
                    <asp:Button ID="Btn_Exp_Ficha" runat="server" Text="Exportar X Ficha" Width="140px" Height="26px" CssClass="button"  />
                </td>
                <td>
                    <asp:Button ID="Btn_Exp_Remision" runat="server" Text="Exportar X Remisión" Width="140px" Height="26px" CssClass="button" />
                </td>
                <td>
                    <asp:Label ID="lbl_Leyenda" runat="server"
                        Text="Si selecciona todos los Clientes y un periodo muy amplio, la consulta puede tardar algunos segundos extra debido a la cantidad de depósitos que existen."
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>

    </fieldset>

    <fieldset id="fst_DesgloseDep" runat="server" style="padding-left: 5px; width: 650px; float: left; border: 1px solid Gray">
        <legend>Desglose</legend>
        <div id="div_gvDesglose" runat="server" style="width: 350px; float: left">
            <asp:GridView ID="gv_Desglose" runat="server" CellPadding="10" AutoGenerateColumns="False"
                CssClass="gv_general" CellSpacing="1" AllowPaging="False"
                PageSize="15">

                <Columns>
                    <asp:BoundField DataField="Presentacion" HeaderText="Presentacion" />
                    <asp:BoundField DataField="Denominacion" HeaderText="Denominacion"
                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad "
                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="70px" />
                    <asp:BoundField DataField="Importe" HeaderText="Importe"
                        HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px" />
                </Columns>

                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

            </asp:GridView>
        </div>
        <div id="div_importes" runat="server" style="float: right">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label1" runat="server" Text="Total Billetes"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Billetes" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label2" runat="server" Text="Total Monedas"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Monedas" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label3" runat="server" Text="Importe Efectivo"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_ImporteTotal" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>

    </fieldset>

    <br />

</asp:Content>

