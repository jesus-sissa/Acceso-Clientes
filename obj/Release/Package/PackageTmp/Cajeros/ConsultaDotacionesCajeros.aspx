<%@ Page Title="Cajeros>consulta de Dotaciones" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaDotacionesCajeros.aspx.vb" Inherits="PortalSIAC.ConsultaDotacionesCajeros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="div_FiltroConsulta" style="border: solid 1px; border-color: Gray">

        <table>
            <tr>
                <td style="text-align: right; width: 80px">
                    <asp:Label ID="lbl_fechainicio" runat="server" Text="Fecha Inicio"></asp:Label>
                </td>

                <td>

                    <asp:TextBox ID="tbx_Fechainicio" runat="server" AutoPostBack="True" CssClass="calendarioAjax"
                        ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"
                        MaxLength="1"></asp:TextBox>

                    <asp:CalendarExtender CssClass="calendarioAjax" ID="tbx_Fechainicio_CalendarExtender" runat="server"
                        DaysModeTitleFormat="MMMM  yyyy" Enabled="True" Format="dd/MM/yyyy"
                        TargetControlID="tbx_Fechainicio" TodaysDateFormat="d MMMM yyyy">
                    </asp:CalendarExtender>

                    <asp:FilteredTextBoxExtender ID="tbx_Fechainicio_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                        TargetControlID="tbx_Fechainicio" ValidChars="/">
                    </asp:FilteredTextBoxExtender>

                </td>
                <td style="text-align: right; width: 250px">
                    <asp:Label ID="lbl_fechafin" runat="server" Text="Fecha Fin"></asp:Label>

                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="tbx_Fechafin" runat="server" AutoPostBack="True"
                        CssClass="calendarioAjax"
                        ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"
                        MaxLength="1"></asp:TextBox>

                    <asp:CalendarExtender CssClass="calendarioAjax" ID="tbx_Fechafin_CalendarExtender" runat="server"
                        DaysModeTitleFormat="MMMM  yyyy" Enabled="True" Format="dd/MM/yyyy"
                        TargetControlID="tbx_Fechafin" TodaysDateFormat="d MMMM yyyy">
                    </asp:CalendarExtender>

                    <asp:FilteredTextBoxExtender ID="tbx_Fechafin_FilteredTextBoxExtender"
                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                        TargetControlID="tbx_Fechafin" ValidChars="/">
                    </asp:FilteredTextBoxExtender>
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

            <tr>

                <td style="text-align: right; width: 80px">
                    <asp:Label ID="lbl_Status" runat="server" Text="Status"></asp:Label>
                </td>

                <td colspan="4">
                    <asp:DropDownList ID="ddl_Status" runat="server" Height="20px" Width="200px" AutoPostBack="True">

                        <asp:ListItem Value="0" Text="Seleccione..." />
                        <asp:ListItem Value="SO" Text="SOLICITADA" />
                        <asp:ListItem Value="CA" Text="CANCELADA" />
                        <asp:ListItem Value="AD" Text="ASIGNADA A DOTADOR" />
                        <asp:ListItem Value="VD" Text="ACEPTADA POR DOTADOR" />
                        <asp:ListItem Value="VA" Text="VALIDADA O PREPARADA" />
                        <asp:ListItem Value="EB" Text="ENVIADA A BOVEDA" />
                        <asp:ListItem Value="RB" Text="RECIBIDA EN BOVEDA" />
                        <asp:ListItem Value="DB" Text="DEVUELTA POR BOVEDA" />
                        <asp:ListItem Value="NA" Text="NO APLICADA" />
                    </asp:DropDownList>

                    <asp:CheckBox ID="chk_Status" runat="server" Text="Todos"
                        AutoPostBack="True" />
                </td>

                <td>

                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />

                </td>


            </tr>
        </table>

    </div>
    <fieldset id="fst_dotaciones" runat="server" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Dotaciones</legend>


        <asp:GridView ID="gv_Dotaciones" runat="server" AutoGenerateColumns="False"
            CssClass="gv_general" DataKeyNames="Id_Dotacion,Id_Remision,RemisionRetiro,Id_PuntoC,Observaciones,TarjetasReales,TarjetasTeoricas,RolloNuevo,DescargaInfo"
            AllowPaging="True" Width="100%" PageSize="25">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="ImageButton2" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                    <ItemStyle Width="20px" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="NumRemision" HeaderText="Remision" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Cajero" HeaderText="Cajero" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" HeaderStyle-HorizontalAlign="Left">
                    <ItemStyle Width="400px" />
                </asp:BoundField>

                <asp:BoundField DataField="Fecha Captura" HeaderText="Fecha Captura" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Fecha Entrega" HeaderText="Fecha Entrega" HeaderStyle-HorizontalAlign="Left" />

                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="Moneda" HeaderText="Moneda" HeaderStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left" />
                <asp:ButtonField ButtonType="Image" CommandName="VerDes" ImageUrl="~/Imagenes/Detalle.png" />
            </Columns>
            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

    </fieldset>
    <fieldset id="fst_DetalleD" style="width: 342px; border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Desglose</legend>


        <asp:GridView ID="gv_DetalleD" runat="server" AutoGenerateColumns="False"
            CssClass="gv_general" PageSize="8">
            <Columns>

                <asp:BoundField DataField="Presentacion" HeaderText="Presentación">
                    <HeaderStyle HorizontalAlign="Center" Width="80px" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="Denominacion" HeaderText="Denominación">
                    <HeaderStyle HorizontalAlign="Right" Width="80px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                    <HeaderStyle HorizontalAlign="Right" Width="70px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:BoundField DataField="Importe" HeaderText="Importe">
                    <HeaderStyle HorizontalAlign="Right" Width="100px" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>


            </Columns>

            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

        </asp:GridView>


    </fieldset>

    <fieldset id="fst_detallePuntos" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Detalle de Dotacion</legend>

        <table style="width: 100%">

            <tr>
                <td style="text-align: right; width: 140px">
                    <asp:Label ID="lbl_cambiopapel" runat="server" Text="Cambio Papel"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rdb_cambiopapelSI" runat="server" Text="Si" GroupName="cambioPapel" Enabled="False" />
                   <asp:RadioButton ID="rdb_cambiopapelNO" runat="server" Text="No" GroupName="cambioPapel" Enabled="False" />
             
                </td>
                             <td style="width: 30px"></td>
                <td>
                    <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 140px">
                    <asp:Label ID="lbl_descargoinfo" runat="server" Text="Descargó Información"></asp:Label>
                </td>
                <td  colspan="2">
                    <asp:RadioButton ID="rdb_descargoinfoSI" runat="server" Text="Si" GroupName="descargoInfo" Enabled="False" />
                 <asp:RadioButton ID="rdb_descargoinfoNO" runat="server" Text="No" GroupName="descargoInfo" Enabled="False" />
                </td>
                <td style="width: 30px"></td>

                <td rowspan="6" style="height: 100%">
                    <asp:TextBox ID="tbx_Observaciones" runat="server" Width="99%" Height="115px" TextMode="MultiLine" Font-Names="Verdana,Arial" Font-Size="X-Small" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 140px">
                    <asp:Label ID="lbl_huboretiro" runat="server" Text="Hubo Retiro"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rdb_huboretiroSI" runat="server" Text="Si" GroupName="huboRetiro" Enabled="False" />
                <asp:RadioButton ID="rdb_huboretiroNO" runat="server" Text="No" GroupName="huboRetiro" Enabled="False" />
               
                     </td>
                <td style="width: 30px"></td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lbl_remision" runat="server" Text="Remision"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="tbx_Remision" runat="server" Width="120px" ReadOnly="True"></asp:TextBox>
                </td>
                <td style="width: 30px"></td>

            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lbl_importe" runat="server" Text="Importe"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="tbx_Importe" runat="server" Width="120px" ReadOnly="True"></asp:TextBox>
               
                     </td>
                <td style="width: 30px"></td>

            </tr>
            <tr>
                <td style="text-align: right" rowspan="2">
                    <asp:Label ID="lbl_tarjetasretenidas" runat="server" Text="Tarjetas Retenidas"></asp:Label>
                </td>
                <td style="width:60px">
                    <asp:Label ID="lbl_Teoricas" runat="server" Text="Teoricas"></asp:Label>
               
                     </td>
                <td style="text-align: right; width:60px">
                    <asp:Label ID="lbl_Reales" runat="server" Text="Reales"></asp:Label>
               
                     </td>
                <td style="width: 30px"></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="tbx_Teoricas" runat="server" Width="30px" ReadOnly="True"></asp:TextBox>
                
                </td>
                <td style="text-align: right">
                    <asp:TextBox ID="tbx_Reales" runat="server" Width="30px" ReadOnly="True"></asp:TextBox>
                </td>
                <td style="width: 30px"></td>
            </tr>
        </table>

    </fieldset>

    <fieldset id="fst_tarjetasretenidas" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Tarjetas Retenidas</legend>

        <asp:GridView ID="gv_TarjetasFallas" runat="server" AutoGenerateColumns="False"
            CssClass="gv_general" PageSize="8" Width="100%" DataKeyNames="Id_Tarjeta,Encontrada_En">
            <Columns>

                <asp:BoundField DataField="Tarjeta" HeaderText="Tarjeta">
                    <HeaderStyle HorizontalAlign="Left" Width="130px" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="Banco" HeaderText="Banco">
                    <HeaderStyle HorizontalAlign="Left" Width="200px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Titular" HeaderText="Titular">
                    <HeaderStyle HorizontalAlign="Left" Width="250px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Vence" HeaderText="Vence">
                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Clonada" HeaderText="Clonada">
                    <HeaderStyle HorizontalAlign="Left" Width="45px" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

            </Columns>

            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />

        </asp:GridView>
    </fieldset>
</asp:Content>
