<%@ Page Title="Usuarios>Privilegios de Usuarios" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="AsignarPrivilegios.aspx.vb" Inherits="PortalSIAC.PrivilegiosUsuarios" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset runat="server">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="Label4" runat="server" Text="Localidad"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_LocalidadConsulta" runat="server" AutoPostBack="True" DataTextField="Nombre_Sucursal" DataValueField="Clave_Sucursal" Width="300px" Enabled="False">
                    </asp:DropDownList>
                </td>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Text="Sucursal" Visible="False"></asp:Label>
                </td>

                <td>
                    <asp:DropDownList ID="ddl_SucursalesConsulta" runat="server" AutoPostBack="True" DataTextField="Nombre_Sucursal" DataValueField="Clave_Sucursal" Enabled="False" Width="300px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="BuscarUsuarios" runat="server" CssClass="button" Height="30px" Text="Buscar" Width="90px" />
                </td>
            </tr>
        </table>
    </fieldset>
    <asp:Panel runat="server" ID="pUsuarios" Visible="false">
        <fieldset id="fst_Usuarios" runat="server" style="border:1px solid Gray; padding-left:5px" >
         <legend> Usuarios</legend>
         <asp:GridView ID="gv_Usuarios" runat="server" CellPadding="10"
             AutoGenerateColumns="False" DataKeyNames="Id_Usuario" CssClass="gv_general" CellSpacing="1"
             Width="100%" AllowPaging="True" PageSize="10">
             <Columns>

                 <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                     ItemStyle-Width="40px" ItemStyle-Height="30px">
                     <ItemTemplate >
                         <asp:ImageButton ID="SeleccionaUsuario" CommandName="Select" runat="server"
                             ImageUrl="~/Imagenes/1rightarrow.png" />
                     </ItemTemplate>
                 </asp:TemplateField>

                 <asp:BoundField DataField="Usuario" HeaderText="Usuario">
                     <HeaderStyle HorizontalAlign="Left" Width="300px" />
                     <ItemStyle HorizontalAlign="Left" CssClass="gv_Item" />
                 </asp:BoundField>

                 <asp:BoundField DataField="Sesion" HeaderText="Sesion">
                     <HeaderStyle HorizontalAlign="Center" Width="70px" />
                     <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="70px" />
                 </asp:BoundField>

                 <asp:BoundField DataField="Cliente" HeaderText="Sucursal" HeaderStyle-HorizontalAlign="Left">
                     <HeaderStyle HorizontalAlign="Left" Width="250px" />
                     <ItemStyle CssClass="gv_Item" />
                 </asp:BoundField>
                 <asp:BoundField DataField="ExpiraContra" HeaderText="Expira Contraseña">
                     <ItemStyle CssClass="gv_Item" HorizontalAlign="Center" Width="75px" />
                     <HeaderStyle HorizontalAlign="center" />
                 </asp:BoundField>
                 <asp:BoundField DataField="Nivel" HeaderText="Nivel">
                     <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="70px" />
                     <HeaderStyle HorizontalAlign="Center" />
                 </asp:BoundField>
                 <asp:BoundField DataField="Status" HeaderText="Status">
                     <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="70px" />
                     <HeaderStyle HorizontalAlign="Center" />
                 </asp:BoundField>
             </Columns>

             <RowStyle CssClass="rowHover" />
             <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
             <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
             <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

         </asp:GridView>
    </fieldset>

     
    </asp:Panel>
     <asp:Panel runat="server" ID="pPrivilegios" Visible="false">
         <fieldset style="border: 1px solid Gray; padding-left: 5px">
             <legend>Privilegios</legend>
             <table width="100%">
                 <tr>
                     <td colspan="2"></td>
                 </tr>
                 <tr>
                     <td valign="top" align="left" style="width: 354px">
                         <asp:GridView ID="gv_Opciones" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_Opcion" CssClass="gv_general"
                             Width="300px" PageSize="50">
                             <Columns>
                                 <asp:TemplateField>
                                     <ItemTemplate>
                                         <asp:CheckBox ID="chk_Opciones" runat="server"
                                             AutoPostBack="true" OnCheckedChanged="checkPrivilegio_CheckedChanged" CssClass="checkPriv" />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="Opcion" HeaderText="Opciones">
                                     <ItemStyle HorizontalAlign="left" CssClass="gv_Item" Width="300px" />
                                     <HeaderStyle HorizontalAlign="Center" />
                                 </asp:BoundField>
                             </Columns>
                             <RowStyle CssClass="rowHover" />
                             <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                             <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                         </asp:GridView>
                     </td>
                     <td>
                         <asp:Button ID="btn_Agregar" runat="server" Text="Agregar >>" Height="26px" Width="90px" Enabled="False" CssClass="button" />
                         <br />
                         <br />
                         <asp:Button ID="btn_Eliminar" runat="server" Text="<< Eliminar" Height="26px" Width="90px" Enabled="False" CssClass="button" />
                     </td>
                     <td valign="top" align="left" style="margin-bottom:20px">
                         <asp:GridView ID="gv_PrivilegiosOtorgados" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_Opcion" CssClass="gv_general"
                             Width="300px" PageSize="50">
                             <Columns>
                                 <asp:TemplateField>
                                     <ItemTemplate>
                                         <asp:CheckBox ID="chk_PrivilegioAsignado" runat="server"
                                             AutoPostBack="true" OnCheckedChanged="checkPrivilegioAsignado_CheckedChanged" CssClass="checkPriv" />
                                     </ItemTemplate>
                                 </asp:TemplateField>
                                 <asp:BoundField DataField="Descripcion" HeaderText="Privilegios Otorgados">
                                     <ItemStyle HorizontalAlign="left" CssClass="gv_Item" Width="300px" />
                                     <HeaderStyle HorizontalAlign="center" />
                                 </asp:BoundField>
                             </Columns>
                             <RowStyle CssClass="rowHover" />
                             <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                             <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                         </asp:GridView>
                     </td>
                 </tr>
             </table>
          </fieldset>
     </asp:Panel>
</asp:Content>
