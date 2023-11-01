<%@ Page Title="Cajeros>Consulta de Tarjetas Retenidas" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaTarjetasRetenidas.aspx.vb" Inherits="PortalSIAC.ConsultaTarjetasRetenidas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="border: 1px solid Gray">

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
                <td style="text-align: right">Tipo de Tarjetas
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_TipoTarjetas" runat="server" Width="300px" AutoPostBack="True">
                        <asp:ListItem Selected="True" Value="0" Text="Seleccione..." />
                        <asp:ListItem Value="N" Text="SOLO ORIGINALES" />
                        <asp:ListItem Value="S" Text="SOLO CLONADAS" />
                    </asp:DropDownList>


                </td>
                <td style="width: 80px">
                    <asp:CheckBox ID="chk_TodosTarjetas" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar"
                        Style="height: 26px" Height="26px" Width="90px" CssClass="button" />
                </td>

            </tr>
        </table>

    </fieldset>

    <fieldset id="fst_TarjetasRetenidas" runat="server" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Tarjetas Retenidas</legend>

        <asp:GridView ID="gv_tarjetasRetenidas" runat="server" AutoGenerateColumns="False"
            CssClass="gv_general" DataKeyNames="NumCajero,Proveedor,Ciudad,Estado,Observaciones"
            AllowPaging="True" Width="100%" PageSize="25" CellPadding="4">
            <Columns>

                  <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="VerDetalle" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="NumCajero" HeaderText="No.Cajero" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" HeaderStyle-HorizontalAlign="Left">
                    <ItemStyle Width="400px" />
                </asp:BoundField>
                <asp:BoundField DataField="NoTarjeta" HeaderText="NoTarjeta" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" />
                <asp:BoundField DataField="BancoTarjeta" HeaderText="BancoTarjeta" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />

                <asp:BoundField DataField="FechaRecoleccion" HeaderText="Fecha Recolecta">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                  <asp:BoundField DataField="FechaRecepcion" HeaderText="Fecha Recepcion">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

               <asp:BoundField DataField="Encontrada_En" HeaderText="Encontrada_En" HeaderStyle-HorizontalAlign="Left" />
               
                   <asp:BoundField DataField="Clonada" HeaderText="Clonada">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                 
            </Columns>
            <RowStyle CssClass="rowHover" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

            <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones(Seleccione una Registro)"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Observaciones" Width="99%"
            Height="35px" runat="server" CssClass="tbx_Mayusculas" ReadOnly="true"> </asp:TextBox>

   </fieldset>

</asp:Content>
