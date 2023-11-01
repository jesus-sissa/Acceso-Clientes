<%@ Page Title="Traslado>Captura de remisiones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" 
    CodeBehind="ImprimirRemision.aspx.vb" Inherits="PortalSIAC.ImprimirRemision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:UpdatePanel ID="upd_general" runat="server">
        <ContentTemplate>
     <asp:Label ID="NotaSolicitud" runat="server"
          ForeColor="Red" Text="La compañia de traslado de valores (SISSA) debe programar la visita previo a la captura de información." >
     </asp:Label>
    
     <asp:Label ID="lbl_puntos" runat="server"
          ForeColor="Red" Text="" >
     </asp:Label>
    <h1 ID="lbl_destino" runat="server" visible="false" class="h2personalizado"></h1>
    
    <fieldset style="border:1px solid Gray ; padding-left:5px; padding-right:5px; padding-bottom:5px">
        
    <fieldset style="border:1px solid Gray" id="Sec_puntos" runat="server">
        <legend>VISITAS PROGRAMADAS CON FECHA ACTUAL</legend>
                      <table>

            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/1rightarrow.png"  />
                </td>
                <td>
                    Capturar remisiones
                </td>
                

             
            </tr>   
        </table>
        <asp:GridView ID="gv_puntos" runat="server" CellPadding="10" AutoGenerateColumns="False" CssClass="gv_general"
                    AllowPaging="True" CellSpacing="1" Width="64%" PageSize="10" EnableModelValidation="True" DataKeyNames="Id_Punto,Cliente_Origen,Cliente_Destino,Remisiones,Clave_Cliente">

                    <Columns>

                        <asp:TemplateField ItemStyle-Wrap="false"
                            ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton  ID="SeleccionaMaterial" CommandName="Select" runat="server"
                                    ImageUrl="~/Imagenes/1rightarrow.png" />
                            </ItemTemplate>

<ItemStyle Wrap="False" Width="20px"></ItemStyle>
                        </asp:TemplateField>
                        <asp:ButtonField ButtonType="Image" HeaderText="" Text="Autorizar" ImageUrl="~/Imagenes/HoraSi.png"
                    CommandName="Autorizar">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="25px" />
                </asp:ButtonField>


                        <asp:BoundField DataField="Cliente_Origen" HeaderText="Cliente">
                            <ItemStyle Wrap="False" Width="20px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Cliente_Destino" HeaderText="Destino">
                            <ItemStyle Wrap="False" Width="20px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                             <asp:BoundField DataField="Descripcion" HeaderText="Descripcion">
                            <ItemStyle Wrap="False" Width="20px"></ItemStyle>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Remisiones" HeaderText="Remisiones">
                            <HeaderStyle HorizontalAlign="center" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                         <asp:BoundField DataField="Clave_Cliente" HeaderText="Clave_Cliente" Visible="false" >
                            <HeaderStyle HorizontalAlign="center"  />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

         <%--<%--<%--                 <asp:TemplateField  HeaderText="TOKEN" HeaderStyle-HorizontalAlign="Center" Visible="false">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="tbx_NEW" Width="120" Height="15px" MaxLength="13" Style="text-align:center" ></asp:TextBox>

                      <%--  <asp:FilteredTextBoxExtender runat="server" ID="ftb_Piezas" TargetControlID="tbx_Piezas" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>--%>

               <%--     </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Width="120px" />
                </asp:TemplateField>--%>

                    </Columns>

                    <RowStyle CssClass="rowHover" />
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
                </asp:GridView>

        
 </fieldset>
        <fieldset style="border:1px solid Gray" visible="false" id="Det" runat="server">
            <div id="hedmodal1" class="modalheader">
            Detalle de envio
        </div>
        <asp:Panel ID="panelmain" runat="server" align="left">
            <div class="mainmodal">    
                <asp:ListBox ID="DetEnvio" runat="server" Height="160px" Width="866px" Font-Bold="True" Font-Names="Arial" Font-Size="Larger" >

                </asp:ListBox>
            </div>
        </asp:Panel>
                <asp:Label ID="Mensaje" runat="server"
          ForeColor="Red" Text="" >
     </asp:Label>
       
        <div class="modalfooter">
            <asp:Button ID="btnGuardar" runat="server" Text="Enviar" CssClass="buttonB" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"  CssClass="buttonB"/>
        </div>
        
            </fieldset>
                 
    <fieldset style="border:1px solid Gray" id="Sec_monedas" visible="False" runat="server">
        <legend>Tipo de monedas</legend>
        <table>
            <tr>
                <td align="right">Moneda:</td>
                <td>                    
                    <asp:DropDownList ID="ddl_monedas" runat="server" DataTextField="Nombre" Width="150"
                        DataValueField="Id_moneda" AutoPostBack="True">
                    </asp:DropDownList>                
                </td>
                <td align="right">Efectivo: </td>
                <td>

                  <%--<asp:TextBox ID="TextBox1" runat="server" Width="69px" />
                     <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                    Enabled="True" TargetControlID="txt_Cantidad" WatermarkText="Cantidad"
                    WatermarkCssClass="WaterMark" >
                </asp:TextBoxWatermarkExtender>--%>
                    <asp:TextBox ID="Tbx_Efectivo" runat="server" Width="69px" />
                     <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server"
                    Enabled="True" TargetControlID="Tbx_Efectivo" WatermarkText="Cantidad"
                    WatermarkCssClass="WaterMark" >
                </asp:TextBoxWatermarkExtender>

                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                        runat="server" Enabled="True" TargetControlID="Tbx_Efectivo"
                        ValidChars="1234567890.">
                    </asp:FilteredTextBoxExtender>
                    </td>
 
            </tr>
            <tr>
                <td align="right">
                    Documentos (Cheques/vales):</td>
                <td>
                    <asp:TextBox ID="Tbx_Documentos" runat="server" Width="150px" >0</asp:TextBox>
                     <asp:TextBoxWatermarkExtender ID="Tbx_Documentos_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="Tbx_Documentos" WatermarkText="Cantidad en efectivo"
                    WatermarkCssClass="WaterMark" >
                </asp:TextBoxWatermarkExtender>

                    
                        <asp:FilteredTextBoxExtender ID="Tbx_Documentos_FilteredTextBoxExtender"
                        runat="server" Enabled="True" TargetControlID="Tbx_Documentos"
                        ValidChars="1234567890.">
                    </asp:FilteredTextBoxExtender>

                </td>
                <td>&nbsp;</td>
                <td>
               <asp:Button ID="Btn_Agregar"  runat="server" Text="Agregar" CssClass="button"
                                Height="26px" Width="90px" />
                </td>


            </tr>
           </table>
          <table>

            <tr>
                <td>
                    <%--<asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/Eliminar16x16.png" />--%>
                </td>
                <td>
                    *Detalle de monedas
                </td>
                </tr>
        </table>
        <asp:GridView ID="gv_Monedas" runat="server" CellPadding="10" AutoGenerateColumns="False" CssClass="gv_general"
                    AllowPaging="True" CellSpacing="1" Width="64%" PageSize="6" EnableModelValidation="True" DataKeyNames="Id_Moneda">

                    <Columns>

                        <asp:TemplateField ItemStyle-Wrap="false"
                            ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton ID="SeleccionaMaterial" CommandName="Select" runat="server"
                                    ImageUrl="~/Imagenes/Eliminar16x16.png" />
                            </ItemTemplate>

<ItemStyle Wrap="False" Width="20px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField DataField="Moneda" HeaderText="Moneda">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                           <asp:BoundField DataField="Efectivo" HeaderText="Efectivo">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right"/>
                        </asp:BoundField>
                        <asp:BoundField DataField="Documentos" HeaderText="Documentos(Cheques/vales)">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>

                    </Columns>

                    <RowStyle CssClass="rowHover" />
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                </asp:GridView>
 </fieldset>    
    <fieldset style="border:1px solid Gray" runat="server" id="Sec_envases" visible="False">
        <legend>Tipo de envases</legend>
        <table>
            <tr>
                <td>
                    Envase:
                </td>
                <td> <asp:DropDownList ID="ddl_envases" runat="server" DataTextField="Descripcion" Width="150"
                        DataValueField="Id_TipoE" AutoPostBack="True">
                    </asp:DropDownList></td>
                <td>Numero de bolsa:</td>
                <td> 
                    <asp:Panel ID="panelContenedor" runat="server" DefaultButton="">
                       <%-- <input type="text" class="form-control" runat="server" style="text-transform: uppercase" id="cunica" title="R.F.C. (Sin Guiiones Ni Espacios)" name="cunica" maxlength="15" placeholder="R.F.C.(Sin Guiones)" />--%>
                     <asp:TextBox ID="Tbx_Numero" runat="server" Width="150px"  onkeydown = "return (event.keyCode!=13);"   />
                    </asp:Panel>
                    
                     <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                        runat="server" Enabled="True" TargetControlID="Tbx_Numero"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>--%>
                </td>
                  <td>
               <asp:Button ID="Btn_AgregarE" CssClass="button" runat="server" Text="Agregar"
                                Height="26px" Width="90px" />
                </td>

            </tr>
                 <tr>
                <td>
                    
                </td>
                <td> </td>
                <td>Envases S/N:</td>
                <td> 
                    <asp:Panel ID="panel1" runat="server" DefaultButton="">
                     <asp:TextBox ID="txt_envasesSN" runat="server" Width="150px" Text="0" ForeColor="#990000" />                                       
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                        runat="server" Enabled="True" TargetControlID="tbx_1"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>
                    </asp:Panel>
                    
                     <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2"
                        runat="server" Enabled="True" TargetControlID="Tbx_Numero"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>--%>
                </td>
                  <td>

                </td>

            </tr>
           </table>
        <table>

            <tr>
                <td>
                    <%--<asp:Image ID="Image3" runat="server" ImageUrl="~/Imagenes/Eliminar16x16.png" />--%>
                </td>
                <td>
                    *Detalle de envases
                </td>
                </tr>
        </table>
        <asp:GridView ID="gv_Envases" runat="server" CellPadding="10" AutoGenerateColumns="False" CssClass="gv_general"
                    AllowPaging="True" CellSpacing="1" Width="64%" PageSize="10" EnableModelValidation="True" DataKeyNames="Id_TipoE,Numero">

                    <Columns>

                        <asp:TemplateField ItemStyle-Wrap="false"
                            ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:ImageButton ID="SeleccionaMaterial" CommandName="Select" runat="server"
                                    ImageUrl="~/Imagenes/Eliminar16x16.png" />
                            </ItemTemplate>

<ItemStyle Wrap="False" Width="20px"></ItemStyle>
                        </asp:TemplateField>

                        <asp:BoundField DataField="TipoEnvase" HeaderText="TipoEnvase">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Numero" HeaderText="Numero">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>

                    </Columns>

                    <RowStyle CssClass="rowHover" />
                    <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
                    <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
                    <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

                </asp:GridView>
 </fieldset>         
    <fieldset id="Sec_denominaciones" runat="server" visible="False">
        <legend>Importe por denominacion</legend>
    <table class="  table table-responsive-md table-bordered " >
        <tr align="center" class="thead-light">
            <th colspan="6" class="auto-style3" >Billetes</th>
            <th colspan="10" class="auto-style3" >Monedas</th>

        </tr>
        <tr>
            <th class="auto-style3">1000</th>
            <th class="auto-style1">500</th>
            <th class="auto-style3">200</th>
            <th class="auto-style3">100</th>
            <th class="auto-style3">50</th>
            <th class="auto-style3">20</th>
            <th></th>
            <th class="auto-style3">20</th>
            <th class="auto-style3">10</th>
            <th class="auto-style3">5</th>
            <th class="auto-style3">2</th>
            <th class="auto-style3">1</th>
            <th class="auto-style3">.50</th>
<%--            <th class="auto-style3">.20</th>
            <th class="auto-style3">.10</th>
            <th class="auto-style3">.5</th>--%>
            <th class="auto-style3">Importe mixto</th>
        </tr> 
        <tr>
            <td><asp:TextBox ID="tbx_1" runat="server" Width="43px" Text="0" />                                       
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3"
                        runat="server" Enabled="True" TargetControlID="tbx_1"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td class="auto-style1"><asp:TextBox ID="tbx_2" runat="server" Width="43px" Text="0"/>                                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4"
                        runat="server" Enabled="True" TargetControlID="tbx_2"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_3" runat="server" Width="43px" Text="0"/>                                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5"
                        runat="server" Enabled="True" TargetControlID="tbx_3"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_4" runat="server" Width="43px" Text="0" />                                      
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender6"
                        runat="server" Enabled="True" TargetControlID="tbx_4"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_5" runat="server" Width="43px" Text="0" />                                  
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender7"
                        runat="server" Enabled="True" TargetControlID="tbx_5"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_6" runat="server" Width="43px" Text="0"/>                               
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender8"
                        runat="server" Enabled="True" TargetControlID="tbx_6"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td></td>
            <td><asp:TextBox ID="tbx_7" runat="server" Width="43px" Text="0" />                                
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender9"
                        runat="server" Enabled="True" TargetControlID="tbx_7"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_8" runat="server" Width="43px" Text="0"/>                                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender10"
                        runat="server" Enabled="True" TargetControlID="tbx_8"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_9" runat="server" Width="43px" Text="0"/>                                  
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender11"
                        runat="server" Enabled="True" TargetControlID="tbx_9"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_10" runat="server" Width="43px" Text="0"/>                                
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender12"
                        runat="server" Enabled="True" TargetControlID="tbx_10"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_11" runat="server" Width="43px" Text="0"/>                           
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender13"
                        runat="server" Enabled="True" TargetControlID="tbx_11"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_12" runat="server" Width="43px" Text="0"/>                             
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender14"
                        runat="server" Enabled="True" TargetControlID="tbx_12"
                        ValidChars=".1234567890">
                    </asp:FilteredTextBoxExtender></td>
    <%--        <td><asp:TextBox ID="tbx_13" runat="server" Width="43px" Text="0"/>                                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15"
                        runat="server" Enabled="True" TargetControlID="tbx_13"
                        ValidChars=".1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_14" runat="server" Width="43px" Text="0"/>                                     
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender16"
                        runat="server" Enabled="True" TargetControlID="tbx_14"
                        ValidChars=".1234567890">
                    </asp:FilteredTextBoxExtender></td>
            <td><asp:TextBox ID="tbx_15" runat="server" Width="43px" Text="0"/>                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender17"
                        runat="server" Enabled="True" TargetControlID="tbx_15"
                        ValidChars=".1234567890">
                    </asp:FilteredTextBoxExtender></td>--%>
            <td><asp:TextBox ID="txt_mixto" runat="server" Width="80px" Text="0"/>                    
                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender18"
                        runat="server" Enabled="True" TargetControlID="txt_mixto"
                        ValidChars=".1234567890">
                    </asp:FilteredTextBoxExtender></td>
    
        </tr>               
    </table>
    </fieldset>
        <fieldset id="coments" runat="server" visible="false">
        <legend>*Comentarios/Notas(Opcional)</legend>
            <asp:TextBox ID="txt_Comentarios" TextMode="MultiLine" Width="99%"
             Height="50px" runat="server" CssClass="tbx_Mayusculas">
        </asp:TextBox>
    </fieldset>
    <br />
         <asp:Label ID="Guardar_lbl" runat="server"
          ForeColor="Red" Text="" >
     </asp:Label><br />
                   <asp:Button ID="Guardar" CssClass="button" runat="server" Text="Guardar"
                                Height="26px" Width="90px" Visible="False" />
        <asp:Button ID="Regresar" CssClass="button" runat="server" Text="Regresar"
                                Height="26px" Width="90px" Visible="False" />








    </fieldset>   
                    <asp:Panel ID="MmodalPanel" runat="server" Width="300px" Visible="false" CssClass="modalpanel"  align="center" >
        <div id="hedmodal" class="modalheader">
            SISSA
        </div>
        <asp:Panel ID="panel2" runat="server" align="left">
            <div class="mainmodal">                 
                <%--<asp:Label ID="lblToken" runat="server" Text="TOKEN" Enabled="true"  ></asp:Label>--%>
                <asp:TextBox ID="tb_Token" runat="server" Width="250px" Font-Size="Medium" />
                     <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server"
                    Enabled="True" TargetControlID="tb_Token" WatermarkText="INGRESE TOKEN"
                    WatermarkCssClass="WaterMark" >
                </asp:TextBoxWatermarkExtender>
                  <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender15"
                        runat="server" Enabled="True" TargetControlID="tb_Token"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>
                <%--<asp:TextBox ID="tb_Token" runat="server" Enabled="true" Width="250px" Font-Size="Medium"></asp:TextBox>--%>
            </div>
        </asp:Panel>
             
       
        <div class="modalfooter">
            <asp:Button ID="Button1" runat="server" Text="Confirmar" CssClass="buttonB" />
            <asp:Button ID="Button2" runat="server" Text="Cancelar"  CssClass="buttonB"/>
        </div>
        
    </asp:Panel>
    <asp:Button ID="lblHiddenReasignar" Visible="false" runat="server" Text="popup" Height="26px" ></asp:Button>
          </ContentTemplate>
        
        </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            width: 45px;
        }
    </style>    
</asp:Content>

