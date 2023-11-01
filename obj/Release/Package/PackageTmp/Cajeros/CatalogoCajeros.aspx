<%@ Page Title="Cajeros>Catalogo de Cajeros"  Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="CatalogoCajeros.aspx.vb" Inherits="PortalSIAC.CatalogoCajeros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="div_cajeros" runat="server" style="border: 1px solid Gray; padding-left:5px">
    
        <table>

            <tr>
                <td  style="height:28px">
                    <asp:Label ID="lbl_Status" runat="server" Text="Status"></asp:Label>
                </td>

                <td >
                    <asp:DropDownList ID="ddl_Status" runat="server" Width="200px" AutoPostBack="True">
                        <asp:ListItem Text="ACTIVO" Value="A" />
                        <asp:ListItem Text="BAJAS" Value="B" />
                        <asp:ListItem Text="PENDIENTES" Value="P" />
                        <asp:ListItem Selected="True" Value="0">Seleccione...</asp:ListItem>
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbx_Todos" runat="server" Text="Todos" AutoPostBack="True" />
                </td>

                <td style="width:150px; text-align:right">
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
                </td>
            </tr>
        </table>

        <asp:GridView ID="gv_Lista" runat="server" DataKeyNames="Id_Cajero"
            CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="100%" AllowPaging="true"
            PageSize="25">

            <Columns>
                   <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                        ItemStyle-Width="20px">
                        <ItemTemplate>
                            <asp:ImageButton ID="SeleccionaCajero" CommandName="Select" runat="server"
                                ImageUrl="~/Imagenes/1rightarrow.png" />
                        </ItemTemplate>

                        <ItemStyle Wrap="False" Width="20px"></ItemStyle>
                    </asp:TemplateField>

                <asp:BoundField DataField="Caja" HeaderText="Caja">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="No. Cajero" HeaderText="N°Cajero">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Descripcion" HeaderText="Descripción">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Direccion" HeaderText="Dirección">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

                <asp:BoundField DataField="Latitud" HeaderText="Latitud">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Longitud" HeaderText="Longitud">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>

            </Columns>
            <RowStyle CssClass="rowHover" />
           <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
           <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

        <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Height="26px" Width="90px" />
      </div>

      <fieldset id="fst_DetalleCaj" runat="server" style="border: 1px solid Gray; padding-left:5px">
          <legend>Detalle Cajero</legend>
       <asp:GridView ID="gv_Config" runat="server" AutoGenerateColumns="False"
            CellPadding="4" Width="50%"
            CssClass="gv_general" EnableModelValidation="True">
            <Columns>

                <asp:BoundField DataField="Denominacion" HeaderText="Denominacion"
                    DataFormatString="{0:c2}">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                </asp:BoundField>

                <asp:BoundField DataField="Caset" HeaderText="Caset" DataFormatString="{0:n0}">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                </asp:BoundField>

                <asp:BoundField DataField="Capacidad" HeaderText="Capacidad">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />

                </asp:BoundField>

               <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:c2}">
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" DataFormatString="{0:c2}">
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>

            </Columns>
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

        </asp:GridView>
 
     </fieldset>

    <br />
</asp:Content>
