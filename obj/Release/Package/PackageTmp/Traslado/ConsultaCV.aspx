<%@ Page Title="Traslado>Comprobantes de Visita" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaCV.aspx.vb" Inherits="PortalSIAC.ConsultaCV" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset style="border:1px solid Gray">
      
        <table>
            <tr>
                <td style="text-align: right" >
                    <label class="label">Fecha Inicial</label>
                </td>
                <td >
                    <asp:TextBox ID="txt_FInicial" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_FInicial_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="true" 
                        TargetControlID="txt_FInicial" Format="dd/MM/yyyy" 
                        PopupPosition="BottomRight">
                    </asp:CalendarExtender>
               </td>
               <td></td>
           
                <td style="text-align:right">
                    <label class="label">Fecha Final</label>
                   <asp:TextBox ID="txt_FFinal" runat="server" CssClass="calendarioAjax" MaxLength="10" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
          
                </td>
                <td style="width:78px">&nbsp;</td>
            </tr>
            <tr>
                <td align="right">
                    Cliente
                </td>
                <td  colspan="3">
               
                    <asp:DropDownList ID="ddl_Clientes" runat="server" 
                                    DataTextField="NombreComercial" Width="400px"
                                    DataValueField="Id_Cliente" AutoPostBack="True">
                    </asp:DropDownList>
                 </td>
                 <td>         
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />                
                </td>
                <td> 
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />
                </td>      
            </tr>
        </table> 
  </fieldset> 
  
   <fieldset id="fst_CV" runat="server" style="border:1px solid Gray; padding-left:5px">
        <legend>Comprobantes</legend>
            <asp:GridView ID="gv_CV" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_ComprobanteV"
                CssClass="gv_general" AllowPaging="True" CellSpacing="1" Width="100%" 
                PageSize="25">

                <Columns>
              
                     <asp:BoundField DataField="Numero" HeaderText="Folio">
                        <ItemStyle HorizontalAlign="Center" CssClass="gv_Item"  />
                        <HeaderStyle HorizontalAlign="center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                     <HeaderStyle HorizontalAlign="Center" Width="100px" />
                        <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Hora" HeaderText="Hora">
                     <HeaderStyle HorizontalAlign="center" />
                        <ItemStyle CssClass="gv_Item" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HoraLlegada" HeaderText="Hora Llegada" >
                        <ItemStyle CssClass="gv_Item" HorizontalAlign="Center" />
                        <HeaderStyle HorizontalAlign="center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="HoraSalida" HeaderText="Hora Salida" >
                        <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Nombre" HeaderText="Cliente" >
                        <ItemStyle HorizontalAlign="Left" CssClass="gv_Item" Width="300" />
                        <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status">
                        <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" />
                        <HeaderStyle HorizontalAlign="Center"  />
                    </asp:BoundField>
                      <asp:BoundField DataField="Comentarios" HeaderText="Comentarios">
                        <ItemStyle HorizontalAlign="Left" CssClass="gv_Item" Width="200" />
                           <HeaderStyle HorizontalAlign="Left"  />
                    </asp:BoundField>
                  
                </Columns>

           
  <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
  <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
            </asp:GridView>
       <asp:Button ID="btn_Exportar" runat="server" Text="Exportar" Height="26px" Width="90px" />
    </fieldset>
     
</asp:Content> 