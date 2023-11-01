<%@ Page Title="Traslado>Consulta de Traslado" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaTraslado.aspx.vb" Inherits="PortalSIAC.ConsultaTraslado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="border: 1px solid Gray">
        <table>
            <tr>
                <td>Fecha Inicial
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaInicial" runat="server" CssClass="calendarioAjax" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td></td>
                <td align="right">Fecha Final
                
                    <asp:TextBox ID="txt_FechaFinal" runat="server" CssClass="calendarioAjax" AutoPostBack="True" />
                    <asp:CalendarExtender ID="CalendarExtender1" CssClass="calendarioAjax" runat="server" Enabled="True" TargetControlID="txt_FechaFinal"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
                <td class="auto-style1">
                    <asp:CheckBox ID="cbx_tripulacion" runat="server" Text="Ver tripulación" AutoPostBack="True" />
                </td>
            </tr>
            <tr>
                <td align="right">Clientes
                </td>

                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="Nombre_Comercial" Width="400"
                        DataValueField="Id_Cliente" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td class="auto-style1">
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
                </td>
                
            </tr>
        </table>
    </fieldset>

    <fieldset id="fst_Traslados" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Traslados</legend>
                   <table>

            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/1rightarrow.png"/>
                </td>
                <td>
                    Ver Detalle
                </td>
                <td>
                    <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Height="26px" Width="90px" Enabled="false" CssClass="button" />
                </td>

            </tr>

        </table>
        <asp:GridView ID="gv_Lista" runat="server" DataKeyNames="Id_Punto,IDU" CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="100%" AllowPaging="true"
            PageSize="25">
            <Columns>

                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                    ShowSelectButton="True" />

                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                <asp:BoundField DataField="Hora" HeaderText="Hora" />
                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                <asp:BoundField DataField="Origen" HeaderText="Origen" />
                <asp:BoundField DataField="Destino" HeaderText="Destino" />
                <asp:BoundField DataField="Placas" HeaderText="Placas" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        
    </fieldset>

    <fieldset id="fst_Remisiones" runat="server" style="border: 1px solid Gray; padding-left: 5px">
            <legend>Remisiones</legend>
          <table>

            <tr>                               
                   <td>
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/Detalle.png"/>
                    
                </td>
                <td>
                    Ver Remision 
                </td>
                <td> </td>
                <td>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/mail_send.png"/>
                </td>
                <td>
                    Enviar Correo
                </td>
            </tr>

        </table>
        <asp:GridView ID="gv_Remisiones" runat="server" CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="50%" DataKeyNames="Id_Remision" EnableModelValidation="True">
            <Columns>
                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>

<ItemStyle Wrap="False" Width="20px"></ItemStyle>
                </asp:TemplateField>

                <asp:BoundField DataField="Remision" HeaderText="Remision">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Hora" HeaderText="Hora">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" Width="70px" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Envases" HeaderText="Envases">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="EnvasesSN" HeaderText="EnvasesSN">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" Width="90px"/>
                </asp:BoundField>
                <asp:ButtonField ButtonType="Image" CommandName="VerDes" ImageUrl="~/Imagenes/Detalle.png" />
                <asp:ButtonField ButtonType="Image" ControlStyle-Width="20px" CommandName="EnvCorreo" ImageUrl="~/Imagenes/mail_send.png" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />

        </asp:GridView>

    </fieldset>
    <fieldset id="fst_detalleRemisiones" runat="server" style="border: 1px solid Gray; padding-left: 5px">
           <legend>Detalle Remisiones</legend>
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


    <br />
    <fieldset style="border: 1px solid Gray" id="tripulacion" runat="server" visible="false">
        <legend>Tripulación </legend>
        <br />
        <asp:Panel ID="pnl_Tripulacion" runat="server" >
            <table>

                <tr>
                    <td style="width: 300px">
                        <b>Unidad: </b>
                    </td>
                    <td rowspan="4">
                        <asp:Image ID="img_Frente" runat="server" Width="100" Height="100" />
                    </td>

                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Unidad" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <b>Placas: </b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Placas" runat="server" />
                    </td>
                </tr>

            </table>
            <hr />
            <br />
            <table>
                <tr>
                    <td style="width: 300px">
                        <b>Operador: </b>
                    </td>
                    <td rowspan="4">
                        <asp:Image ID="img_Operador" runat="server" Width="100" Height="100" />
                    </td>
                    <td rowspan="4">
                        <asp:Image ID="img_OperadorFirma" runat="server" Width="100" Height="100" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Operador" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <b>Clave: </b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_OperadorClave" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="width: 300px">
                        <b>Cajero: </b>
                    </td>
                    <td rowspan="4">
                        <asp:Image ID="img_Cajero" runat="server" Width="100" Height="100" />
                    </td>
                    <td rowspan="4">
                        <asp:Image ID="img_CajeroFirma" runat="server" Width="100" Height="100" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Cajero" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <b>Clave: </b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_CajeroClave" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <b>Custodios</b>
            <asp:DataList ID="dl_Custodios" runat="server" Width="100%" RepeatColumns="1">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <b>Nombre: </b>
                            </td>
                            <td rowspan="4">
                                <asp:Image ID="img_Operador" runat="server" Width="100" Height="100" ImageUrl='<%#Eval("Foto")%>' />
                            </td>
                            <td rowspan="4">
                                <asp:Image ID="img_OperadorFirma" runat="server" Width="100" Height="100" ImageUrl='<%#Eval("Firma")%>' />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:Label ID="lbl_Nombre" runat="server">
                                    <%#Eval("Nombre")%>
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px">
                                <b>Clave: </b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:Label ID="lbl_Clave" runat="server">
                                    <%#Eval("Clave")%>
                                </asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>


        </asp:Panel>
    </fieldset>
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            width: 117px;
        }
    </style>
</asp:Content>

