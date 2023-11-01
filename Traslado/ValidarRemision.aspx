<%@ Page Title="Traslado>Autorizar remisiones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ValidarRemision.aspx.vb" Inherits="PortalSIAC.ValidarRemision" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset id="Fieldset1" runat="server" style="border: 1px solid Gray; padding-left: 5px">
              <table>

            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Detalle.png" />
                </td>
                <td>
                    Ver Detalle
                </td>
                <td style="width:20px"></td>

                   <td>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/HoraSi.png" />
                </td>
                <td>
                    Confirmar Remisión </td>
                <td  style="width:20px"></td>

                <td>
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/Eliminar16x16.png" />
                </td>
                <td>
                    Eliminar Remisión </td>
             
            </tr>   
        </table>
                <table>
            <tr>
                <td style="text-align: right">Fecha Inicial
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaInicial" runat="server" CssClass="calendarioAjax" AutoPostBack="true" />
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
                <td></td>
            </tr>
                    <tr>
                        <td style="text-align: right">
                            Clientes
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Cli" runat="server" DataTextField="Nombre_Comercial" Width="400"
                            DataValueField="Id_Cliente" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
            <tr>
                <td style="text-align: right">Status
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server" Width="400" AutoPostBack="True">
                        <asp:ListItem Value="0">Seleccione ...</asp:ListItem>
                        <asp:ListItem Value="A">CAPTURADA</asp:ListItem>
                        <asp:ListItem Value="V">VALIDADA</asp:ListItem>
                        <asp:ListItem Value="T">TERMINADA</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 80px">
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" CssClass="button"
                       Height="26px" Width="90px" />
                    <%--<asp:CheckBox  ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />--%>
                    <%--<asp:CheckBox  ID="cbx_Todos_Clientes1" runat="server" Text ="todos"/>--%>
                </td>
                <td>
                    

                </td>

            </tr>
        </table>
     </fieldset>
        <fieldset id="fst_AutorizarMateriales" runat="server" style="border: 1px solid Gray; padding-left: 5px">

        <legend>Autorizar Remisiones</legend>                     
     <asp:Label ID="Mensaje" runat="server"
          ForeColor="Red" Text="" >
     </asp:Label>

        <asp:GridView ID="gv_Solicitudes" runat="server" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" AllowPaging="True" PageSize="25" DataKeyNames="Folio">
            <Columns>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Ver Detalle" ImageUrl="~/Imagenes/Detalle.png"
                    CommandName="VerDetalle">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Autorizar" ImageUrl="~/Imagenes/HoraSi.png"
                    CommandName="Autorizar">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25px" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Cancelar" ImageUrl="~/Imagenes/Eliminar16x16.png"
                    CommandName="Cancelar">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25px" />
                </asp:ButtonField>
                <asp:BoundField DataField="Folio" HeaderText="Folio">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha Captura">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Hora" HeaderText="Hora Captura">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                    <asp:BoundField DataField="Envases" HeaderText="Envases">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                    <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                    <asp:BoundField DataField="Solicita" HeaderText="Solicita">
              
                </asp:BoundField>
                  <asp:BoundField DataField="Destino" HeaderText="Destino">
                    
                </asp:BoundField>
                <%--<asp:BoundField DataField="Solicita" HeaderText="Solicita" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>--%>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>
        <fieldset id="Remision_det" visible="false" runat="server" style="border: 1px solid Gray; padding-left: 5px">
           <legend>Detalle Remisiones</legend>
        <table width="100%">
            <tr>
                <td>
                    <asp:Button ID="Btn_edit" runat="server" Text="Editar Envases" CssClass="button"
                       Height="26px" Width="158px" />
                </td>
            </tr>
            <tr>
                <td>Monedas
                </td>
                <td>Envases
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gvMonedas" runat="server" CellPadding="10" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="100%" DataKeyNames="Id_Moneda" EnableModelValidation="True">
                        <Columns>

                            <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Efectivo" HeaderText="Efectivo" ReadOnly="True">
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
                        CssClass="gv_general" CellSpacing="1" Width="100%" DataKeyNames="Id_EnvasesWeb" EnableModelValidation="True">
                        <Columns>

                            <asp:BoundField DataField="Tipo Envase" HeaderText="Tipo Envase">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero" HeaderText="Numero">

                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:TemplateField   HeaderText="Nuevo Numero" Visible="False">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="tbx_NEW" Width="120" Height="15px" MaxLength="13" Style="text-align:left" ></asp:TextBox>

                      <%--  <asp:FilteredTextBoxExtender runat="server" ID="ftb_Piezas" TargetControlID="tbx_Piezas" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>

                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                </asp:TemplateField>

                        </Columns>


                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>
                </td>
            </tr>
        </table>

    </fieldset>
</asp:Content>
