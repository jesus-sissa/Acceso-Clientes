<%@ Page Title="Proceso>Solicitud de Dotaciones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="SolicitudDotaciones.aspx.vb" Inherits="PortalSIAC.SolicitudDotaciones" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="NotaSolicitud" runat="server"
        ForeColor="Red" Text="Los servicios que se solicitan deben ser validados en la opción 'Autorizar Dotaciones',<br/>de lo contrario no le llegará la solicitud a la compañía.">
    </asp:Label>

    <br />
    <fieldset style="border: 1px solid Gray">
        <table cellspacing="3">
            <tr>
                <td style="width: 120px; text-align: right">Fecha de Entrega
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaEntrega" runat="server" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FechaEntrega_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaEntrega" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td align="right">Horario Entrega
                </td>
                <td>
                    <asp:DropDownList ID="ddl_De" runat="server" AutoPostBack="true" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                        DataValueField="Hora">
                    </asp:DropDownList>
                    <asp:Label ID="Label1" runat="server" Text="/" Font-Bold="True"
                        Font-Size="Medium"></asp:Label>
                    <asp:DropDownList ID="ddl_A" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                        DataValueField="Hora" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">Moneda
                </td>
                <td>
                    <asp:DropDownList ID="ddl_Moneda" runat="server" DataTextField="Nombre"
                        DataValueField="Id_Moneda" AutoPostBack="True" Width="100px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">Caja Bancaria
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CajaBancaria" runat="server"
                        DataTextField="Nombre_Comercial" DataValueField="Id_CajaBancaria"
                        AutoPostBack="True" Width="400px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">Cliente
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Cliente" runat="server"
                        DataTextField="Nombre_Comercial" DataValueField="Id_Cliente" Width="400px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset style="border: 1px solid Gray; padding-left: 5px; padding-bottom: 4px">
        <legend>Denominaciones</legend>
        <table width="100%">
            <tr>
                <td colspan="2">Importe Teorico<asp:TextBox ID="txt_Total" runat="server"></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txt_Total_FilteredTextBoxExtender"
                        runat="server" Enabled="True" TargetControlID="txt_Total"
                        ValidChars="1234567890.">
                    </asp:FilteredTextBoxExtender>

                    <asp:Image ID="ImgValidado" runat="server"
                        ImageUrl="~/Imagenes/Eliminar16x16.png" />

                </td>
            </tr>
            <tr>
                <td valign="top" style="width: 249px">
                    <br />
                    <b>Billetes</b>
                    <asp:GridView ID="gv_Billetes" runat="server" CellPadding="10"
                        GridLines="Horizontal" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="200px"
                        DataKeyNames="Id_Denominacion">
                        <Columns>
                            <asp:BoundField DataField="Denominacion" HeaderText="Denominacion"
                                ReadOnly="True">
                                <ItemStyle Width="30" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Cantidad">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Cantidad" Width="50" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txt_Cantidad_FilteredTextBoxExtender"
                                        runat="server" Enabled="True" TargetControlID="txt_Cantidad"
                                        ValidChars="1234567890">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True"
                                DataFormatString="{0:n2}">
                                <ItemStyle HorizontalAlign="Right" Width="40" />
                            </asp:BoundField>

                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />


                    </asp:GridView>
                </td>
                <td valign="top">
                    <br />
                    <b>Monedas</b>
                    <asp:GridView ID="gv_Monedas" runat="server" CellPadding="10"
                        GridLines="Horizontal" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="200px"
                        DataKeyNames="Id_Denominacion">
                        <Columns>
                            <asp:BoundField DataField="Denominacion" HeaderText="Denominacion" ReadOnly="True">
                                <ItemStyle Width="30" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Cantidad">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Cantidad" Width="50" runat="server"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txt_Cantidad_FilteredTextBoxExtender"
                                        runat="server" Enabled="True" TargetControlID="txt_Cantidad"
                                        ValidChars="1234567890">
                                    </asp:FilteredTextBoxExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Total" HeaderText="Total" ReadOnly="True"
                                DataFormatString="{0:n2}">
                                <ItemStyle HorizontalAlign="Right" Width="40" />
                            </asp:BoundField>
                        </Columns>

                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lbl_Total" runat="server" Text="Importe Real: $0.00 "></asp:Label>
        <br />
        <asp:Button ID="btn_Actualizar" runat="server" Text="Comprobar" Height="26px" Width="90px" CssClass="button" />
        <br />
        <br />
        <asp:Label ID="lbl_comentariosAd" runat="server" Text="Comentarios Adicionales"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Comentarios" TextMode="MultiLine" Width="99%"
            Height="50px" runat="server" CssClass="tbx_Mayusculas"> </asp:TextBox>
        <br />
        <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Enabled="False" Width="90px" Height="26px" CssClass="button" />
    </fieldset>
</asp:Content>
