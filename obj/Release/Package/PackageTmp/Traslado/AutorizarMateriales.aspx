<%@ Page Title="Traslado>Autorizar Materiales" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="AutorizarMateriales.aspx.vb" Inherits="PortalSIAC.AutorizarMateriales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <fieldset id="fst_AutorizarMateriales" runat="server" style="border: 1px solid Gray; padding-left: 5px">

                <legend>Autorizar Materiales</legend>

                <table width="100%">
                    <tr>
                        <td style="text-align: right" class="auto-style1">
                            Cliente</td>

                        <td style="text-align: left" class="auto-style2">
                            <asp:DropDownList ID="ddl_Clientes" runat="server" AutoPostBack="True" DataTextField="Nombre_Comercial" DataValueField="Id_Cliente" Width="400">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>

                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/Detalle.png" />
                        </td>
                        <td>Ver Detalle
                        </td>
                        <td style="width: 20px"></td>

                        <td>
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/HoraSi.png" />
                        </td>
                        <td>Confirmar Solicitud </td>
                        <td style="width: 20px"></td>

                        <td>
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/Eliminar16x16.png" />
                        </td>
                        <td>Eliminar Solicitud </td>

                    </tr>
                </table>
                <asp:GridView ID="gv_Solicitudes" runat="server" AutoGenerateColumns="False"
                    CellPadding="4" Width="100%"
                    CssClass="gv_general" DataKeyNames="IdMV,IdC, IdS,Conexion,Comentarios" AllowPaging="True" PageSize="25">
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
                        <asp:BoundField DataField="FechaCaptura" HeaderText="Fecha Captura">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HoraCaptura" HeaderText="Hora Captura">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha Entrega">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Solicita" HeaderText="Solicita" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    </Columns>

                    <RowStyle CssClass="rowHover" />
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                </asp:GridView>

            </fieldset>

            <fieldset id="fst_detalleMaterial" runat="server" style="border: 1px solid Gray; padding-left: 5px">
                <legend>Detalle Solicitud</legend>
                <asp:Label ID="lbl_comentarios" runat="server" Text="Comentarios Adicionales(Seleccione una Solicitud)"></asp:Label>
                <br />
                <asp:TextBox ID="txt_Comentarios" Width="99%"
                    Height="35px" runat="server" CssClass="tbx_Mayusculas" ReadOnly="true">
                </asp:TextBox>

                <asp:GridView ID="gv_Detalle" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="gv_general">
                    <Columns>
                        <asp:BoundField DataField="Material" HeaderText="Material">
                            <HeaderStyle HorizontalAlign="Left" Width="400px" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                            <HeaderStyle HorizontalAlign="Right" Width="70px" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                        </asp:BoundField>
                    </Columns>
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                </asp:GridView>

            </fieldset>
        </ContentTemplate>


    </asp:UpdatePanel>
</asp:Content>






<%--<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1 {
            width: 57px;
            height: 22px;
        }

        .auto-style2 {
            height: 22px;
        }
    </style>
</asp:Content>--%>





