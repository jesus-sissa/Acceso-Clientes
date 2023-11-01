<%@ Page Title="Proceso>Fichas de Morralla" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="FichasdeMorralla.aspx.vb" Inherits="PortalSIAC.FichasdeMorralla" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function marca() {
            // va al marcador1 desglose Importe
            location.href = "#Marcador1"
            return false;
        }
    </script>

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
                <td style="text-align: right">G. Deposito
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_GruposDeposito" runat="server" AutoPostBack="true"
                        DataTextField="Descripcion" Width="400px"
                        DataValueField="Id_GrupoDepo">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="cbx_GruposDeposito" Text="Todos" runat="server"
                        AutoPostBack="True" />
                </td>

            </tr>

            <tr>
                <td style="text-align: right">Cliente
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
                <td align="right">Moneda
                </td>
                <td align="left" colspan="2">
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
            PageSize="35">

            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                    HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="70px"></asp:BoundField>

                <asp:BoundField DataField="Estacion" HeaderText="Estacion "
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>


                <asp:BoundField DataField="Remision" HeaderText="Remisión" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                    <ItemStyle Width="90px" Font-Bold="False" />
                </asp:BoundField>

                <asp:BoundField DataField="Entidad" HeaderText="Entidad " ItemStyle-Width="80px"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Folio" HeaderText="Folio " ItemStyle-Width="80px"
                    HeaderStyle-HorizontalAlign="Left">
                    <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>

                <asp:BoundField DataField="ImporteEsperado" DataFormatString="{0:c2}" HeaderText="Importe Esp."
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="ImporteReal" DataFormatString="{0:c2}" HeaderText="Importe Real"
                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

                <asp:BoundField DataField="Diferencia" HeaderText="Diferencia"
                    HeaderStyle-HorizontalAlign="Center" DataFormatString="{0:c2}" ItemStyle-HorizontalAlign="Right"></asp:BoundField>

            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <table>
            <tr>
                <td>
                    <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Width="140px" Height="26px" />
                </td>
                <td>
                    <asp:Label ID="lbl_Leyenda" runat="server"
                        Text="Si selecciona todos los Clientes y un periodo muy amplio, la consulta puede tardar algunos segundos extra debido a la cantidad de registros que existen."
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>

    </fieldset>

    <fieldset id="fst_DesgloseDep" runat="server" style="padding-left: 5px; width: 650px; float: left; border: 1px solid Gray">
        <legend>Desglose</legend>
        <a name="Marcador1"></a>
        <div name="div_gvDesglose" id="div_gvDesglose" runat="server" style="width: 350px; float: left">

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
