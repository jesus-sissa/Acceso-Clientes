<%@ Page Title="Soporte>Consulta de tickets" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaAnomalias.aspx.vb" Inherits="PortalSIAC.ConsultaAnomalias" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="fst_Anomalias" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Tickets</legend>
          <table>
                    <tr style="padding-bottom: 30px">
                        <td align="right">
                            <asp:Label ID="lbl_Localidad" runat="server" Text="Localidad"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddl_Localidad" runat="server" Width="300px" DataTextField="Nombre_Sucursal"
                                DataValueField="Clave_Sucursal" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_Sucursales" runat="server" Text="Sucursal"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Sucursales" runat="server" Width="300px" DataTextField="Nombre_Sucursal"
                                DataValueField="Clave_Sucursal" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_Guardar" runat="server" Text="Consultar" Height="30px" Width="90px" CssClass="button" />
                        </td>
                    </tr>
                   
              </table>
        <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False" CellPadding="4"
            Width="100%" CssClass="gv_general"
            AllowPaging="True" PageSize="25" DataKeyNames="Ticket" DataMember="Ticket">
            <Columns>

                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaTicket" CommandName="Select" CommandArgument='<%#Eval("Ticket")%>' runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Ticket" HeaderText="#Ticket">
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="50px" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="Status" HeaderText="Status">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Nombre_Sucursal" HeaderText="Sucursal">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="75px" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>

    <fieldset id="fst_DetalleMateriales" runat="server" style="border: 1px solid Gray; padding-left:5px" visible="False" >
        <legend>Detalle</legend>
        <asp:GridView ID="gv_Detalle" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="gv_general"
            AllowPaging="True" Width="60%">

            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha"></asp:BoundField>
                <asp:BoundField DataField="Hora" HeaderText="Hora">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
            </Columns>


            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
        
     <%--<asp:Button ID ="btnImprimir" runat ="server" Text="Ver remisión" Height="40px" Width="150px" Font-Bold="true" Enabled="false"  />--%>
    </fieldset>

</asp:Content>
