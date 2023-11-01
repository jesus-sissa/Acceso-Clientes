<%@ Page Title="Cajeros>Autorizar Dotaciones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="AutorizarDotacionesCajeros.aspx.vb" Inherits="PortalSIAC.AutorizarDotacionesCajeros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="fst_AutorizarDotaciones" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Autorizar Dotaciones</legend>
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

        <asp:GridView ID="gv_DotacionesActivas" runat="server" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" DataKeyNames="Id_DotacionCaj,IdM,IdCajaB,IdS,Comentarios,Conexion,PrioridadBanco,Especial,Prioridad,IdRuta,NumReporte,IdC,RequiereCorte,HoraSolicitaBanco" AllowPaging="True" PageSize="25">
            <Columns>
                <asp:ButtonField ButtonType="Image" HeaderText="" Text="Ver Detalle" ImageUrl="~/Imagenes/billeteselect.png"
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
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="HoraCaptura" HeaderText="Hora Captura">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="HoraEntrega" HeaderText="Hora Entrega">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="NoCajero" HeaderText="Cajero">
                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="Cajero" HeaderText="Descripcion">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="90px" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>
            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

    </fieldset>

    <fieldset id="fst_detalleDotacion" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Detalle Dotación</legend>
        <asp:Label ID="lbl_comentarios" runat="server" Text="Comentarios Adicionales(Seleccione una Dotación)"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Comentarios" Width="99%"
            Height="35px" runat="server" CssClass="tbx_Mayusculas" ReadOnly="true">
        </asp:TextBox>
        <asp:GridView ID="gv_Detalle" runat="server"
            AutoGenerateColumns="False" CellPadding="4"
            ForeColor="#333333" CssClass="gv_general"
            Width="40%" DataKeyNames="IdD">
            <Columns>
                <asp:BoundField DataField="Presentacion" HeaderText="Presentación">
                    <HeaderStyle HorizontalAlign="Center" Width="100px" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Denominacion" HeaderText="Denominación">
                    <HeaderStyle HorizontalAlign="Right" Width="100px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="90px" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>
            </Columns>


            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
        </asp:GridView>
    </fieldset>

</asp:Content>
