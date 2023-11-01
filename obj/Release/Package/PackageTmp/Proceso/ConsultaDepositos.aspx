<%@ Page Title="Proceso>Consulta de Depositos" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaDepositos.aspx.vb" Inherits="PortalSIAC.ConsultaDepositos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="fst_filtroDeposito" style="border: 1px solid Gray">
        <table>
            <tr>
                <td style="text-align: right">
                    <label>
                        Fecha Inicial</label>
                </td>

                <td>
                    <asp:TextBox ID="txt_FInicial" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" CssClass="calendarioAjax"
                        TargetControlID="txt_FInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td></td>
                <td style="text-align: right">
                    <label>
                        Fecha Final

                    </label>
                    <asp:TextBox ID="txt_FFinal" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" runat="server" Enabled="True" CssClass="calendarioAjax"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <label>
                        Cliente</label>
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
            </tr>
            <tr>
                <td style="text-align: right">
                    <label>
                        Status</label>
                </td>

                <td colspan="3">
                    <asp:DropDownList ID="ddl_Status" runat="server" AutoPostBack="True"
                        Width="200px">
                        <asp:ListItem Value="0" Text="Seleccione..." Selected="True" />
                        <asp:ListItem Value="RC" Text="RECIBIDO" />
                        <asp:ListItem Value="AS" Text="ASIGNADO" />
                        <asp:ListItem Value="AC" Text="ACEPTADO POR CAJERO" />
                        <asp:ListItem Value="IN" Text="INICIADO" />
                        <asp:ListItem Value="BL" Text="BLOQUEADO" />
                        <asp:ListItem Value="VE" Text="VERIFICADO" />
                        <asp:ListItem Value="CO" Text="CONTABILIZADO" />
                        <asp:ListItem Value="DB" Text="DEVUELTO A BOVEDA" />
                        <asp:ListItem Value="DV" Text="EN BOVEDA" />
                        <asp:ListItem Value="RE" Text="RETENIDO" />
                        <asp:ListItem Value="RB" Text="RETENIDO A BOVEDA" />
                        <asp:ListItem Value="RR" Text="RETENIDO EN BOVEDA" />
                        <asp:ListItem Value="DC" Text="DEVUELTO AL CLIENTE" />
                    </asp:DropDownList>

                    <asp:CheckBox ID="cbx_Todos" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar"
                        Style="height: 26px" Width="90px" CssClass="button" />
                </td>
            </tr>

        </table>

    </fieldset>

    <fieldset id="fst_Depositos" runat="server" style="overflow: auto; left: 0px; border: 1px solid Gray; padding-left: 5px">
        <legend>Depositos</legend>

        <table>
            <tr>
                <td style="width: 70px">
                    <asp:Label ID="lbl_Faltantes" runat="server" Text="Faltantes" ForeColor="Black"></asp:Label></td>
                <td style="width: 80px">
                    <asp:Label ID="lbl_Sobrantes" runat="server" Text="Sobrantes" ForeColor="Black"></asp:Label></td>
                <td style="width: 130px">
                    <asp:Label ID="lbl_MonedaExtranjera" runat="server" Text="Moneda Extranjera" ForeColor="Black"></asp:Label></td>
                <td>
                    <b>
                        <asp:Label ID="Label4" runat="server" Text="Cálculo de la diferencia: Desglose de las Fichas vs Importe en la Remisión." ForeColor="Red"></asp:Label></b>

                </td>
            </tr>
        </table>

        <center>
            <asp:GridView ID="gv_Depositos" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_Servicio"
                CssClass="gv_general" AllowPaging="True" CellSpacing="1" Width="100%"
                PageSize="25" EnableModelValidation="True">
                <Columns>

                    <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                        ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                                ImageUrl="~/Imagenes/1rightarrow.png" />
                        </ItemTemplate>

                        <ItemStyle Wrap="False" Width="20px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:BoundField DataField="Remision" HeaderText="Remisión">
                        <HeaderStyle HorizontalAlign="Right" />

                        <ItemStyle CssClass="gv_Item" Width="90px" Font-Bold="False" HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                        <ItemStyle CssClass="gv_Item" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-HorizontalAlign="Left">
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>

                        <ItemStyle CssClass="gv_Item" Width="400px" HorizontalAlign="Left" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Cia" HeaderText="ETV">
                        <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle CssClass="gv_Item" Width="100px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Fichas" HeaderText="Fichas" DataFormatString="{0:n0}">
                        <ItemStyle CssClass="gv_Item" HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Dice_Contener" HeaderText="Dice Contener" DataFormatString="{0:c2}">
                        <ItemStyle HorizontalAlign="Right" CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ImporteReal" HeaderText="Importe Real" DataFormatString="{0:c2}">
                        <ItemStyle HorizontalAlign="Right" CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Diferencia" HeaderText="Diferencia">
                        <ItemStyle HorizontalAlign="Right" CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="Right" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Envases" HeaderText="Envases" DataFormatString="{0:n0}">
                        <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="60" />
                        <HeaderStyle HorizontalAlign="Center" Width="60" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HoraFinaliza" HeaderText="Hora Verificado">
                        <ItemStyle CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Moneda Extranjera" HeaderText="Moneda Extranjera">
                        <ItemStyle CssClass="gv_Item" HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>


                </Columns>

                <RowStyle CssClass="rowHover" />
                <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
            </asp:GridView>

        </center>
        <asp:Button ID="btn_Exportar" runat="server"
            Text="Exportar" Height="26px" Width="90px" CssClass="button" />
    </fieldset>

    <fieldset id="fst_detalledeposito" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Fichas</legend>
        <asp:GridView ID="gv_Fichas" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="Id_Ficha"
            CssClass="gv_general" Width="100%" PageSize="15">
            <Columns>

                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionarFicha" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Ficha" HeaderText="Ficha" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />

                <asp:BoundField DataField="Moneda" HeaderStyle-HorizontalAlign="Left" HeaderText="Moneda" />
                <asp:BoundField DataField="Tipo" HeaderStyle-HorizontalAlign="Left" HeaderText="Tipo" />
                <asp:BoundField DataField="Efectivo" DataFormatString="{0:c2}" HeaderText="Efectivo">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Cheques" DataFormatString="{0:c2}" HeaderText="Cheques">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Otros" DataFormatString="{0:c2}" HeaderText="Otros">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Dif. Efectivo" DataFormatString="{0:c2}" HeaderText="Dif. Efectivo">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Dif. Cheques" DataFormatString="{0:c2}" HeaderText="Dif. Cheques">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Dif. Otros" DataFormatString="{0:c2}" HeaderText="Dif. Otros">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

        </asp:GridView>
    </fieldset>

    <br />

    <asp:TabContainer ID="tc_Desglose" runat="server" ActiveTabIndex="0">

        <asp:TabPanel runat="server" HeaderText="Efectivo" ID="tp_Efectivo">
            <HeaderTemplate>Efectivo</HeaderTemplate>
            <ContentTemplate>
                <fieldset id="fst_Efectivo" runat="server" style="width: 650px; float: left">

                    <div id="div_gvEfectivo" runat="server" style="width: 350px; float: left">
                        <asp:GridView ID="gv_Efectivo" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" Width="100%"
                            CssClass="gv_general" EnableModelValidation="True">
                            <Columns>
                                <asp:BoundField DataField="Presentacion" HeaderText="Presentacion">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />

                                </asp:BoundField>

                                <asp:BoundField DataField="Denominacion" HeaderText="Denominacion"
                                    DataFormatString="{0:c2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" DataFormatString="{0:n0}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                        </asp:GridView>
                    </div>
                    <div id="div_ImporteEfectivo" runat="server" style="float: right">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label1" runat="server" Text="Total Billetes"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TotalBilletes" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox><br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label2" runat="server" Text="Total Monedas"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TotalMonedas" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox><br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label3" runat="server" Text="Importe Efectivo"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_ImporteTotal" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>

            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel runat="server" HeaderText="Parciales" ID="tp_Parciales">
            <HeaderTemplate>Parciales</HeaderTemplate>
            <ContentTemplate>
                <fieldset id="fst_Parciales" runat="server" style="width: 650px; border: 1px solid Gray">
                    <legend>Parciales</legend>

                    <asp:GridView ID="gv_Parciales" runat="server" AllowPaging="True" AutoGenerateColumns="False" CellPadding="4"
                        DataKeyNames="Id_Parcial" CssClass="gv_general" Width="100%" EnableModelValidation="True">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="SeleccionarFicha" CommandName="Select" runat="server"
                                        ImageUrl="~/Imagenes/1rightarrow.png" />
                                </ItemTemplate>
                                <ItemStyle Width="20px" Wrap="False" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Parcial" HeaderText="Parcial">

                                <HeaderStyle HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Hora" HeaderText="Hora">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Referencia" HeaderText="Referencia">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DiceContener" DataFormatString="{0:c2}" HeaderText="DiceContener">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Direfencia" DataFormatString="{0:c2}" HeaderText="Direfencia">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>

                        <RowStyle CssClass="rowHover" />
                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                        <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                        <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                    </asp:GridView>

                </fieldset>
                <br />
                <fieldset id="fst_ParcialesD" runat="server" style="width: 650px; float: left">

                    <div id="div_ParcialD" runat="server" style="width: 350px; float: left">
                        <asp:GridView ID="gv_ParcialD" runat="server" AutoGenerateColumns="False"
                            CellPadding="4" Width="100%"
                            CssClass="gv_general" EnableModelValidation="True">
                            <Columns>
                                <asp:BoundField DataField="Presentacion" HeaderText="Presentacion">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" Width="80px" />

                                </asp:BoundField>

                                <asp:BoundField DataField="Denominacion" HeaderText="Denominacion"
                                    DataFormatString="{0:c2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" DataFormatString="{0:n0}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c2}">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                        </asp:GridView>
                    </div>
                    <div id="div_ImporteParcialD" runat="server" style="float: right">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label5" runat="server" Text="Total Billetes"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TotalBilletesP" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox><br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label6" runat="server" Text="Total Monedas"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_TotalMonedasP" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox><br />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label7" runat="server" Text="Importe Efectivo"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="tbx_ImporteTotalP" runat="server" AutoPostBack="True" ReadOnly="True" Width="102px" Height="20px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>

                </fieldset>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="tp_Cheques" runat="server" HeaderText="Cheques">
            <HeaderTemplate>Cheques</HeaderTemplate>
            <ContentTemplate>
                <fieldset id="fst_cheques" runat="server" style="width: 720px">

                    <asp:GridView ID="gv_Cheques" runat="server" CellPadding="4"
                        CssClass="gv_general" AutoGenerateColumns="False"
                        DataKeyNames="Id_Cheque" OnRowCommand="gv_Cheques_RowCommand"
                        Width="97%" EnableModelValidation="True">
                        <Columns>

                            <asp:ButtonField DataTextField="Numero" DataTextFormatString="{0:n}" HeaderText="Numero"
                                CommandName="Numero">

                                <ItemStyle Font-Underline="true" ForeColor="Blue" HorizontalAlign="Left" Font-Bold="False" />
                            </asp:ButtonField>

                            <asp:BoundField DataField="Banco" HeaderText="Banco">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Width="400px" HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Cuenta" HeaderText="Cuenta" DataFormatString="{0:n}">
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c2}">
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>

                </fieldset>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="tp_Otros" runat="server" HeaderText="Otros">
            <ContentTemplate>
                <fieldset id="fst_otros" runat="server" style="width: 820px">

                    <asp:GridView ID="gv_Otros" runat="server" CellPadding="4"
                        CssClass="gv_general"
                        EnableModelValidation="True" Width="98%" AutoGenerateColumns="False">

                        <Columns>
                            <asp:BoundField DataField="Documento" HeaderText="Documento" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle Width="350px" HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe" DataFormatString="{0:c2}">
                                <ItemStyle HorizontalAlign="Right" Width="110px" />
                                <HeaderStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>

                </fieldset>
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel ID="tp_BilletesFalsos" runat="server" HeaderText="Falsos">
            <ContentTemplate>
                <fieldset id="fst_BilletesFalsos" runat="server" style="width: 780px">

                    <asp:GridView ID="gv_Falsos" runat="server" CellPadding="4"
                        DataKeyNames="Id_Falso" CssClass="gv_general"
                        EnableModelValidation="True" Width="98%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Denominacion" HeaderText="Denominacion">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Importe" HeaderText="Importe">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" Width="60px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Serie" HeaderText="Serie">
                                <ItemStyle HorizontalAlign="Left" Width="70px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" HeaderStyle-HorizontalAlign="Left">
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>

                </fieldset>
            </ContentTemplate>
        </asp:TabPanel>

    </asp:TabContainer>
</asp:Content>


