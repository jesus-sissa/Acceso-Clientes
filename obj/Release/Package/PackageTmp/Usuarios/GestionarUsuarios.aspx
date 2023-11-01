<%@ Page Title="Usuarios >Alta,Consulta y Reasignacion " Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="GestionarUsuarios.aspx.vb" Inherits="PortalSIAC.CrearUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <table>
            <tr>
                <td>
                    <asp:Button ID="btnAdUsuarios" runat="server" Text="Agregar Usuarios Nuevos"  CssClass="buttonU"/>
                </td>
                <td>
                    <asp:Button ID="btnConsUsuarios" runat="server" Text="Consultar y Reasignacion de Usuarios"  CssClass="buttonU"/>
                </td>
            </tr>
        </table>
    <fieldset style="border: 1px solid Gray" runat="server" id="FsGestionUsuarios">

        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="vAltaUsuarios" runat="server">
                <legend>Gestión de Usuarios</legend>
                <table>
                    <tr style="padding-bottom: 30px">
                        <td align="right">
                            <asp:Label ID="lbl_Localidad" runat="server" Text="Localidad"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddl_Localidad" runat="server" Width="300px" DataTextField="Nombre_Sucursal"
                                DataValueField="Clave_Sucursal" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>

                        <td align="right">
                            <asp:Label ID="lbl_Sucursales" runat="server" Text="Sucursal"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Sucursales" runat="server" Width="300px" DataTextField="Nombre_Sucursal"
                                DataValueField="Clave_Sucursal" AutoPostBack="True" Enabled="False">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Nivel" runat="server" Text="Nivel"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddl_Nivel" runat="server" Width="200px" AutoPostBack="True" Enabled="False">
                                <asp:ListItem Selected="True" Value="0" Text="Seleccione..." />
                                <asp:ListItem Value="2" Text="USUARIO LOCAL" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <%-- <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Cliente" runat="server" Text="Cliente"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="NombreComercial"
                                Width="300px" DataValueField="Id_Cliente">
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_Nombre" runat="server" Text="Nombre Completo"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbx_Nombre" runat="server" Width="400px" OnTextChanged="tbx_Nombre_TextChanged" CssClass="tbx_Mayusculas" Enabled="False" TabIndex="1" Style="text-transform: uppercase"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_NombreSesion" runat="server" Text="Usuario"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbx_Sesion" runat="server" Width="150px" MaxLength="10" CssClass="tbx_Mayusculas" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" class="auto-style1">
                            <asp:Label ID="lbl_ModoCosulta" runat="server" Text="Modo Consulta"></asp:Label>
                        </td>
                        <td align="left" class="auto-style1">
                            <asp:DropDownList ID="ddl_ModoConsulta" runat="server" Width="200px" AutoPostBack="True" Enabled="False">
                                <asp:ListItem Selected="True" Value="0" Text="Seleccione..." />
                                <asp:ListItem Value="1" Text="SOLO SUCURSAL" />
                                
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label2" runat="server" Text="Mail"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="tbx_mail" runat="server" Width="400px" CssClass="tbx_Mayusculas" TabIndex="2"></asp:TextBox>
                            <%--<asp:TextBox ID="tbx_mail" runat="server" Width="400px" CssClass="tbx_Mayusculas" TabIndex="2"></asp:TextBox>--%>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Height="30px" Width="90px" CssClass="button" />
                            <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="--Usuario y Contraseña se guardan en  MAYUSCULAS"></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vConsultaUsuarios" runat="server">
                <legend>Usuarios Existentes</legend>
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
                            <asp:Label ID="Label3" runat="server" Text="Sucursal" Visible="true"></asp:Label>
                        </td>

                        <td>
                            <asp:DropDownList ID="ddl_SucursalesConsulta" runat="server" AutoPostBack="True" DataTextField="Nombre_Sucursal" DataValueField="Clave_Sucursal" Enabled="False" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="BuscarUsuarios" runat="server" CssClass="button" Height="30px" Text="Buscar" Width="90px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">Reiniciar Clave </td>
                        <td style="text-align: center">Alta/Baja Usuario </td>
                        <td style="text-align: center">Bloquear/Desbloquear </td>
                        <td style="text-align: center">Reasignar Usuario </td>
                        <td style="text-align: center">Cambiar Correo</td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/ReiniciarClave.png" />
                        </td>
                         <td style="text-align: center">
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/Imagenes/delete.png" />
                        </td>
                        <td style="text-align: center">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/BloquearUsuarios.png" />
                        </td>
                        <td style="text-align: center">
                            <asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/cambio.png" />
                        </td>
                        <td style="text-align: center">
                            <asp:Image ID="Image4" runat="server" ImageUrl="~/Imagenes/mail.png" />

                        </td>
                    </tr>
                </table>
                <asp:Button ID="lblHiddenReasignar" Visible="false" runat="server" Text="popup" ></asp:Button>
                <asp:GridView ID="gv_CrearUsuarios" runat="server" CellPadding="10"
                    AutoGenerateColumns="False" DataKeyNames="Id_Usuario" 
                    CssClass="gv_general" CellSpacing="1"
                    Width="100%" AllowPaging="True" PageSize="15">
                    <Columns>
                        <asp:ButtonField CommandName="Reiniciar" AccessibleHeaderText="Reiniciar" ButtonType="Image"
                            ImageUrl="~/Imagenes/ReiniciarClave.png" Text="Reiniciar">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" Height="30px" />
                        </asp:ButtonField>
                        <asp:ButtonField CommandName="Baja" AccessibleHeaderText="Baja" ButtonType="Image" Visible="True"
                            ImageUrl="~/Imagenes/delete.png" Text="Baja">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:ButtonField>
                        <asp:ButtonField CommandName="Desbloquear" AccessibleHeaderText="Desbloquear" ButtonType="Image"
                            ImageUrl="~/Imagenes/BloquearUsuarios.png" Text="Desbloquear">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                        </asp:ButtonField>

                        <asp:ButtonField CommandName="Suspender" AccessibleHeaderText="Suspender" ButtonType="Image" Visible="false"
                            ImageUrl="~/Imagenes/suspender.png" Text="Suspender">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:ButtonField>

                        <asp:ButtonField CommandName="Reasignar" AccessibleHeaderText="Reasignar" ButtonType="Image" Visible="true"
                            ImageUrl="~/Imagenes/cambio.png" Text="Reasignar">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="40px" />
                        </asp:ButtonField>
                        <asp:ButtonField CommandName="CambiarCorreo" AccessibleHeaderText="CambiarCorreo" ButtonType="Image" Visible="true"
                            ImageUrl="~/Imagenes/mail.png" Text="CambiarCorreo">
                        </asp:ButtonField>
                        <asp:BoundField DataField="Id_Usuario" HeaderText="Id_Usuario" HeaderStyle-HorizontalAlign="Left" Visible="false">
                            <ItemStyle HorizontalAlign="Left" CssClass="gv_Item" Width="350px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Usuario" HeaderText="Nombre de Usuario" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle HorizontalAlign="Left" CssClass="gv_Item" Width="350px" />
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Sesion" HeaderText="Session">
                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Localidad" HeaderText="Localidad">
                            <HeaderStyle HorizontalAlign="Center" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="100px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Cliente" HeaderText="Sucursal" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle HorizontalAlign="Left" Width="500px" />
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
                        <asp:BoundField DataField="Mail" HeaderText="Correo">
                             <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="70px" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                    </Columns>

                    <RowStyle CssClass="rowHover" />
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                </asp:GridView>

            </asp:View>
        </asp:MultiView>
        <ajaxtoolkit:modalpopupextender ID="mPopUpReasignar" runat="server" TargetControlID="lblHiddenReasignar" PopupControlID="MmodalPanel" BackgroundCssClass="modalBackground"></ajaxtoolkit:modalpopupextender>

       
  
   
    </fieldset>  
 
      <asp:Panel ID="MmodalPanel" runat="server" Width="300px" Visible="false" CssClass="modalpanel" align="center" >
        <div id="hedmodal" class="modalheader">
            <asp:Label ID="headerModal" runat="server">Reasginar Usuario</asp:Label>
        </div>
        <asp:Panel ID="panelmain" runat="server">
            <div class="mainmodal">
                <asp:Label ID="lblResp" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblIdUsuarioReasginar" runat="server" Visible="false" Enabled="true"></asp:Label>
                <asp:Label ID="lblClave_SucursalReasginar" runat="server" Visible="false" Enabled="true"></asp:Label>
                <asp:TextBox ID="tb_usuarioReasignar" runat="server" Enabled="false" Width="250px"></asp:TextBox>
                <asp:Label ID="lblSucursal" runat="server" Text="Sucursal" Visible="false"></asp:Label>
                <asp:DropDownList ID="ddl_ReasignarSucursal" Visible="false" runat="server" AutoPostBack="True" DataTextField="Nombre_Sucursal" DataValueField="Clave_Sucursal" Width="250px">
                </asp:DropDownList>
                <asp:Label ID="lblcorreo" runat="server" Text="Correo" Visible="false"></asp:Label>
                <asp:TextBox ID="tbxCorreo" runat="server" Width="250px" Visible="false"></asp:TextBox>
            </div>
        </asp:Panel>
             
       
        <div class="modalfooter">
            <asp:Button ID="btnReasginar" runat="server" Text="Reasignar" CssClass="buttonB" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  CssClass="buttonB"/>
        </div>
        
    </asp:Panel>


</asp:Content>


