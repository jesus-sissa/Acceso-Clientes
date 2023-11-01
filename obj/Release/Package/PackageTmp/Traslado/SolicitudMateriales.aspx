<%@ Page Title="Traslado>Solicitud de Materiales" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="SolicitudMateriales.aspx.vb" Inherits="PortalSIAC.SolicitudMateriales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label ID="NotaSolicitud" runat="server"
                ForeColor="Red" Text="Los servicios que se solicitan deben ser validados en la opción 'Autorizar Materiales',<br/>de lo contrario no le llegará la solicitud a la compañía.">
            </asp:Label>

            <br />

            <fieldset style="border: 1px solid Gray">

                <table>
                    <tr>
                        <td align="right">Fecha Requerida
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Fecha" runat="server" CssClass="calendarioAjax" />
                            <asp:CalendarExtender ID="txt_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                                TargetControlID="txt_Fecha" Format="dd/MM/yyyy">
                            </asp:CalendarExtender>
                        </td>
                        <td align="right">Cliente
                        </td>
                        <td>

                            <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="Nombre_Comercial" Width="400"
                                DataValueField="Id_Cliente" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">Cantidad
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Cantidad" runat="server" Width="69px" />
                            <asp:TextBoxWatermarkExtender ID="txt_Cantidad_TextBoxWatermarkExtender" runat="server"
                                Enabled="True" TargetControlID="txt_Cantidad" WatermarkText="Cantidad"
                                WatermarkCssClass="WaterMark">
                            </asp:TextBoxWatermarkExtender>


                            <asp:FilteredTextBoxExtender ID="txt_Cantidad_FilteredTextBoxExtender"
                                runat="server" Enabled="True" TargetControlID="txt_Cantidad"
                                ValidChars="1234567890">
                            </asp:FilteredTextBoxExtender>

                        </td>
                        <td>Material</td>
                        <td>
                            <asp:DropDownList ID="ddl_Material" runat="server" DataTextField="Descripcion"
                                DataValueField="Id_Inventario" Height="20px" Width="400px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>


                    </tr>
                    <tr>
                        <td colspan="3"></td>
                        <td align="right">
                            <asp:Button ID="btn_Agregar" CssClass="button" runat="server" Text="Agregar"
                                Height="26px" Width="90px" />
                        </td>
                    </tr>
                </table>
            </fieldset>

            <fieldset id="fst_detalleMaterial" runat="server" style="border: 1px solid Gray; padding-left: 5px">
                <legend>Detalle Material</legend>

                <table>

                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Eliminar16x16.png" />
                        </td>
                        <td>Eliminar material
                        </td>
                </table>
                <asp:GridView ID="gv_MaterialesAgregados" runat="server" CellPadding="10" AutoGenerateColumns="False" CssClass="gv_general"
                    AllowPaging="True" CellSpacing="1" Width="64%" DataKeyNames="Id_Inventario,IdCs,Precio" PageSize="25" EnableModelValidation="True">

                    <Columns>

                        <asp:TemplateField ItemStyle-Wrap="false"
                            ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton ID="SeleccionaMaterial" CommandName="Select" runat="server"
                                    ImageUrl="~/Imagenes/Eliminar16x16.png" />
                            </ItemTemplate>

                            <ItemStyle Wrap="False" Width="20px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Material" HeaderText="Material">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                        </asp:BoundField>

                    </Columns>

                    <RowStyle CssClass="rowHover" />
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                </asp:GridView>
                <asp:Label ID="lbl_comentariosAd" runat="server"
                    Text="Comentarios Adicionales">
                </asp:Label>

                <br />
                <asp:TextBox ID="txt_Comentarios" TextMode="MultiLine" Width="99%"
                    Height="50px" runat="server" CssClass="tbx_Mayusculas">
                </asp:TextBox>
                <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Height="26px" Width="90px" CssClass="button" />

            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
