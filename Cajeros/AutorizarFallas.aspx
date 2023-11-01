<%@ Page Title="Cajeros>Autorizar Fallas"  Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="AutorizarFallas.aspx.vb" Inherits="PortalSIAC.AutorizarFallas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="fst_AutorizarFallas" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Autorizar Fallas</legend>
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
        <asp:GridView ID="gv_FallasActivas" runat="server" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" DataKeyNames="Id_FallaCli,FechaCaptura,HoraCaptura,FechaRequerida,TiempoRespuesta,FechaAlarma,HoraAlarma,HoraSolicitaBanco,NumReporte,NoCajero,Cajero,Parte,TipoFalla,IdC,IdPF,Tipo,Comentarios,IdS,Conexion" AllowPaging="True" PageSize="25">
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
                <asp:BoundField DataField="FechaRequerida" HeaderText="Fecha Requerida">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="TiempoRespuesta" HeaderText="Tiempo Respuesta">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="NoCajero" HeaderText="Cajero">
                    <HeaderStyle HorizontalAlign="Center" Width="50px" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>
                <asp:BoundField DataField="Cajero" HeaderText="Descripcion">
                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                    <ItemStyle HorizontalAlign="Left" Width="250px" />
                </asp:BoundField>

                <asp:BoundField DataField="Parte" HeaderText="Parte Falla">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="TipoFalla" HeaderText="Tipo Falla">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>

            </Columns>
            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>

    <fieldset id="fst_detalleFalla" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Comentarios</legend>
        <asp:Label ID="lbl_comentarios" runat="server" Text="Comentarios Adicionales(Seleccione una Falla)"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Comentarios" Width="99%"
            Height="35px" runat="server" CssClass="tbx_Mayusculas" ReadOnly="true">
        </asp:TextBox>
    </fieldset>
</asp:Content>
