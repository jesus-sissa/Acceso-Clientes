<%@ Page Title="Proceso>Verificacion de Bolsas" Language="vb" AutoEventWireup="false" CodeBehind="VerificaciondeBolsas.aspx.vb"
    Inherits="PortalSIAC.VerificaciondeBolsas" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset style="border: 1px solid Gray">
        <table>

            <tr>
                <td align="right">Fecha Inicio Op.
                </td>

                <td align="left">
                    <asp:TextBox ID="txt_FechaInicioOperacion" runat="server" AutoPostBack="true" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FechaInicioOperacion_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaInicioOperacion" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>

                <td></td>

                <td style="text-align: right">Fecha Fin Op.
            
                    <asp:TextBox ID="txt_FechaFinOperaciones" runat="server" AutoPostBack="true" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FechaFinOperaciones_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaFinOperaciones" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>

            </tr>

            <tr>
                <td align="right">Fecha Cierre  </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_FechaCierre" runat="server" AutoPostBack="true"
                        DataTextField="FechaOperacion" Width="400px"
                        DataValueField="Id_Cierre">
                    </asp:DropDownList>
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

                <td style="text-align: right">Clientes
                </td>
                <td colspan="3">

                    <asp:DropDownList ID="ddl_Clientes" runat="server" AutoPostBack="true"
                        DataTextField="Nombre_Comercial" Width="400px"
                        DataValueField="Id_Cliente">
                    </asp:DropDownList>

                </td>
                <td>
                    <asp:CheckBox ID="cbx_Clientes" Text="Todos" runat="server"
                        AutoPostBack="True" />
                </td>

            </tr>

            <tr>
                <td align="right">Grupo Clientes</td>

                <td colspan="3">

                    <asp:DropDownList ID="ddl_GruposClientes" runat="server" AutoPostBack="true"
                        DataTextField="Descripcion" Width="400px"
                        DataValueField="Id_ClienteGrupo">
                    </asp:DropDownList>

                </td>
                <td style="width: 80px">
                    <asp:CheckBox ID="cbx_GruposClientes" Text="Todos" runat="server"
                        AutoPostBack="True" />

                </td>
                <td style="text-align: right">
                    <asp:Button ID="Btn_Consultar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
                </td>
            </tr>

        </table>
    </fieldset>

    <fieldset id="fst_Depositos" runat="server" style="overflow: auto; border: 1px solid Gray; padding-left: 5px">
        <legend>Fichas de Morralla</legend>

        <asp:GridView ID="gv_Lista" runat="server"
            DataKeyNames="Id_Ficha" CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="100%" AllowPaging="True"
            PageSize="35" EnableModelValidation="True">

            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>


                <asp:BoundField DataField="FechaOperacion" HeaderText="Fecha Operacion"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="65px"></asp:BoundField>

                <asp:BoundField DataField="FechaRemision" HeaderText="Fecha Remision"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="65px"></asp:BoundField>

                <asp:BoundField DataField="Remision" HeaderText="Remisión" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                    <ItemStyle Font-Bold="False" />
                </asp:BoundField>

                <asp:BoundField DataField="Cliente" HeaderText="Cliente "
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>

                <asp:BoundField DataField="Entidad" HeaderText="Entidad "
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Folio" HeaderText="Folio "
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>

                <asp:BoundField DataField="DiceContener" DataFormatString="{0:c2}" HeaderText="Dice Contener"
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="ImporteRealT" DataFormatString="{0:c2}" HeaderText="Importe Real Total"
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="75px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="ImporteReal" DataFormatString="{0:c2}" HeaderText="Importe Real"
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="Diferencia" HeaderText="Diferencia"
                    HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" DataFormatString="{0:c2}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="Grupo" HeaderText="Grupo" Visible="false" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <table>
            <tr>
                <td>
                    <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Width="140px" Height="26px" CssClass="button" />
                </td>
                <td>
                    <asp:Label ID="lbl_Leyenda" runat="server"
                        Text="Si selecciona todos los Clientes y un periodo muy amplio, la consulta puede tardar algunos segundos extra debido a la cantidad de registros que existen."
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>

    </fieldset>

</asp:Content>
