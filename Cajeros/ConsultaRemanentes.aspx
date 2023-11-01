<%@ Page Title="Cajeros>Consulta de Remanentes" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ConsultaRemanentes.aspx.vb" Inherits="PortalSIAC.ConsultaRemanentes" %>

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
            </tr>

            <tr>
                <td style="text-align: right; width:80px">
                    <label class="label">
                        Status</label>
                </td>

                <td colspan="3">
                    <asp:DropDownList ID="ddl_Status" runat="server" AutoPostBack="True"
                        Width="267px">
                        <asp:ListItem Value="0" Text="Seleccione..." Selected="True" />
                        <asp:ListItem Value="RC" Text="RECIBIDO" />
                        <asp:ListItem Value="AS" Text="ASIGNADO" />
                        <asp:ListItem Value="AC" Text="ACEPTADO POR CAJERO" />
                        <asp:ListItem Value="VE" Text="VERIFICADO" />
                        <asp:ListItem Value="CO" Text="CONTABILIZADO" />
                    </asp:DropDownList>
                    </td>
                <td style="width:80px">
                    <asp:CheckBox ID="chk_Todos" runat="server" Text="Todos" AutoPostBack="True" />
            </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar"
                        Style="height: 26px" Height="26px" Width="90px" CssClass="button" />
                </td>
            </tr>

        </table>

    </fieldset>

     <fieldset id="fst_Remanentes" runat="server" style="border: 1px solid Gray; padding-left:5px">
       <legend>Remanentes</legend>
         <asp:GridView ID="gv_Remanentes" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" Width="100%"
            CssClass="gv_general" DataKeyNames="Id_Servicio,IDR" PageSize="25">
            <Columns>

                <asp:TemplateField HeaderText="" ItemStyle-Wrap="false"
                    ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:ImageButton ID="SeleccionaRemision" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="Remision" HeaderText="Remisión" HeaderStyle-HorizontalAlign="Right">
                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>

               <asp:BoundField DataField="Cajero" HeaderText="Cajero" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                    <ItemStyle HorizontalAlign="Left" Width="400px" />
                </asp:BoundField>

               <asp:BoundField DataField="FechaEntrada" HeaderText="Fecha Entrada">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                  <asp:BoundField DataField="FechaVerifica" HeaderText="Fecha Verifica">
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>

                   <asp:BoundField DataField="ImporteEfectivo" DataFormatString="{0:n}" HeaderText="Importe Efectivo">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>

                <asp:BoundField DataField="DiferenciaEfectivo" DataFormatString="{0:n}" HeaderText="Diferencia Efectivo">
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                </asp:BoundField>
             
                <asp:BoundField DataField="Status" HeaderText="Status" >
                     <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                    </asp:BoundField>
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

     </fieldset>
       <fieldset id="fst_detalleRemanente" runat="server" style="border: 1px solid Gray; padding-left:5px">
        <legend>Detalle Remanentes</legend>
           <table width="100%">
            <tr>
                <td>
                     <asp:Label ID="lbl_ImporteRem" runat="server" Text="Importe Remision"></asp:Label>
                </td>
               <td></td>
                <td>
                     <asp:Label ID="lbl_ImporteVerif" runat="server" Text="Importe Verificado"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <asp:GridView ID="gv_ImporteRemision" runat="server" CellPadding="10" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="100%" DataKeyNames="Id_Moneda">
                        <Columns>

                            <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Efectivo" HeaderText="Efectivo">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Documentos" HeaderText="Documentos">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                              <asp:BoundField DataField="TipoCambio" HeaderText="Tipo Cambio">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>


                        <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>
                </td>
                <td></td>
                <td valign="top">
                    <asp:GridView ID="gv_ImporteVerificado" runat="server" CellPadding="10" AutoGenerateColumns="False"
                        CssClass="gv_general" CellSpacing="1" Width="100%" DataKeyNames="Id_Denominacion">
                        <Columns>

                        <asp:BoundField DataField="Presentacion" HeaderText="Presentacion">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Denominacion" HeaderText="Denominacion">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                              <asp:BoundField DataField="Importe" HeaderText="Importe">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>

                        </Columns>
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

                    </asp:GridView>
                </td>
            </tr>
        </table>
</fieldset>

</asp:Content>
