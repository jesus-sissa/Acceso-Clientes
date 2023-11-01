<%@ Page Title="Cajeros>Consulta de Cortes" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="Cortes.aspx.vb" Inherits="PortalSIAC.Cortes" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    d
</asp:Content>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <fieldset style="border: 1px solid Gray">
        <table>
            <tr>
                <td style="text-align: right; width:80px">
                    <label>Fecha Inicial</label>
                </td>

                <td>
                  
                    <asp:TextBox ID="txt_FInicial" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_FInicial_CalendarExtender" runat="server" Enabled="true" CssClass="calendarioAjax"
                        TargetControlID="txt_FInicial" Format="dd/MM/yyyy"
                        PopupPosition="BottomRight">
                    </asp:CalendarExtender>

                    </td>

                <td style="width:120px; text-align:right">

                     <label> Fecha Final</label>
                      </td>
                 <td>
                    <asp:TextBox ID="txt_FFinal" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
               
            
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" runat="server" Enabled="True" CssClass="calendarioAjax"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
                   <td colspan="4" align="right">
                    <asp:Button ID="Bnt_consultar" runat="server" Text="Consultar"
                        Style="height: 26px" Height="26px" Width="90px" CssClass="button" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lbl_cajero" runat="server" Text="Cajero"></asp:Label>
                </td>

                <td colspan="4">
                    <asp:DropDownList ID="ddl_Cajero" runat="server" Height="20px" Width="400px"
                        DataTextField="Descripcion" DataValueField="Id_Cajero" AutoPostBack="True">
                    </asp:DropDownList>
                </td>

                <td style="width: 80px">
                    <asp:CheckBox ID="chk_Cajeros" runat="server" Text="Todos"
                        AutoPostBack="True" />
                </td>


            </tr>

        </table>

    </fieldset>
         <fieldset id="fst_Cortes" runat="server" style="border: 1px solid Gray; padding-left:5px">
       <legend>Cortes</legend>
         <asp:GridView ID="gv_Cortes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" PageSize="25" EnableModelValidation="True" DataKeyNames="IDRR">
            <Columns>

                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>

<ItemStyle Wrap="False" Width="20px"></ItemStyle>
                </asp:TemplateField>

                <asp:BoundField DataField="Remision" HeaderText="Remision" HeaderStyle-HorizontalAlign="left">
<HeaderStyle HorizontalAlign="left"></HeaderStyle>
                </asp:BoundField>

               <asp:BoundField DataField="Cajero" HeaderText="Cajero" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
<HeaderStyle HorizontalAlign="Left" Width="400px"></HeaderStyle>

                    <ItemStyle HorizontalAlign="Left" Width="400px" />
                </asp:BoundField>

                <asp:BoundField DataField="Importe" HeaderText="Importe" />

                <asp:BoundField DataField="Status" HeaderText="Status" >
                     <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>

               <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                </asp:BoundField>
                <asp:ButtonField ButtonType="Image" CommandName="VerDes" ImageUrl="~/Imagenes/Detalle.png" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

     </fieldset>
</asp:Content>
