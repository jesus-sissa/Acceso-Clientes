<%@ Page Title="Traslado>Generar Remision" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" 
    CodeBehind="LLenar_Remision.aspx.vb" Inherits="PortalSIAC.LLenar_Remision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

     <asp:Label ID="NotaSolicitud" runat="server"
          ForeColor="Red" Text="Los servicios que se solicitan deben ser validados en la opción 'Autorizar Materiales',<br/>de lo contrario no le llegará la solicitud a la compañía." >
     </asp:Label>

    <br />

    <fieldset style="border:1px solid Gray">

        <table>
            <tr>
                <td align="right" class="auto-style1">Moneda:</td>
                <td class="auto-style1">
                    <asp:DropDownList ID="DropDownList1" runat="server" DataTextField="Descripcion"
                        DataValueField="Id_Inventario" Height="20px" Width="300px" AutoPostBack="True">
                    </asp:DropDownList>                  
                </td>
                <td align="right" class="auto-style1">Efectivo:
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="Efectivo" runat="server" Width="69px" />
               

                </td>
                <td align="right" class="auto-style1">Documentos:
                </td>
                  <td class="auto-style1">
                    <asp:TextBox ID="TextBox1" runat="server" Width="69px" />
                </td>
                   <td align="right">
                    <asp:Button ID="btn_Agregar" CssClass="button" runat="server" Text="Agregar"
                                Height="26px" Width="90px" />
                </td>

            </tr>
  
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
 </fieldset>     
</asp:Content>
<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            height: 22px;
        }
    </style>
</asp:Content>

