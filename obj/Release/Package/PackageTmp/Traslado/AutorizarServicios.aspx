<%@ Page Title="Traslado>Autorizar Servicios" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="AutorizarServicios.aspx.vb" Inherits="PortalSIAC.AutorizarServicios" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset id="fst_AutorizarServicios" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Autorizar Servicios</legend>
        <table width="100%">
            <tr>
                <td style="width: 57px; text-align: right">
                    <asp:Label ID="lbl_server" runat="server" Text="Sucursal"></asp:Label>

                </td>

                <td style="text-align: left">
                    <asp:DropDownList ID="ddl_SucursalPropia" runat="server" AutoPostBack="true" DataTextField="Nombre" Width="300"
                        DataValueField="Clave SP">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <asp:GridView ID="gv_Solicitudes" runat="server" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" DataKeyNames="IdST,IdS,IdClienteSolicita,IdCS,IdCO,IdCD,Conexion,Comentarios,Origen,HRecoleccion,Destino,HEntrega" AllowPaging="True" PageSize="25">
            <Columns>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Ver Detalle" ImageUrl="~/Imagenes/Detalle.png"
                    CommandName="VerDetalle">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Autorizar" ImageUrl="~/Imagenes/HoraSi.png"
                    CommandName="Autorizar">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                </asp:ButtonField>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Cancelar" ImageUrl="~/Imagenes/Eliminar16x16.png"
                    CommandName="Cancelar">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                </asp:ButtonField>
                <asp:BoundField DataField="FechaCaptura" HeaderText="Fecha Captura">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="85px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="FechaServicio" HeaderText="Fecha Servicio">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="85px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Servicio" HeaderText="Servicio">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="300px" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Solicita" HeaderText="Solicita">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>

    <fieldset id="fst_detalleServicio" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Detalle Solicitud</legend>
        <asp:Label ID="lbl_comentarios" runat="server" Text="Comentarios Adicionales(Seleccione una Servicio)"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Comentarios" Width="99%"
            Height="35px" runat="server" CssClass="tbx_Mayusculas" ReadOnly="true"> </asp:TextBox>

        <asp:GridView ID="gv_Detalle" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="gv_general">
            <Columns>
                <asp:BoundField DataField="Origen" HeaderText="Origen">
                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="HRecoleccion" HeaderText="Recolección">
                    <HeaderStyle HorizontalAlign="Center" Width="120px" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Destino" HeaderText="Destino">
                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="HEntrega" HeaderText="Entrega">
                    <HeaderStyle HorizontalAlign="Center" Width="120px" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

        </asp:GridView>

    </fieldset>
</asp:Content>


