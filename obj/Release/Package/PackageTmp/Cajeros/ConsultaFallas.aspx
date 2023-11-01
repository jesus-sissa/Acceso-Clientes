<%@ Page Title="Cajeros>Consulta de Fallas" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaFallas.aspx.vb" Inherits="PortalSIAC.ConsultaFallas" %>

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
                    <asp:TextBox ID="tbx_Fechafin" runat="server" AutoPostBack="True" CssClass="calendarioAjax" MaxLength="1" ToolTip="Haga click sobre el cuadro de texto para mostrar el calendario"></asp:TextBox>
                    <asp:CalendarExtender ID="tbx_Fechafin_CalendarExtender" runat="server" CssClass="calendarioAjax" DaysModeTitleFormat="MMMM  yyyy" Enabled="True" Format="dd/MM/yyyy" TargetControlID="tbx_Fechafin" TodaysDateFormat="d MMMM yyyy">
                    </asp:CalendarExtender>
                    <asp:FilteredTextBoxExtender ID="tbx_Fechafin_FilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" TargetControlID="tbx_Fechafin" ValidChars="/">
                    </asp:FilteredTextBoxExtender>

                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 80px">
                    <asp:Label ID="lbl_cajero" runat="server" Text="Cajero"></asp:Label>
                </td>

                <td colspan="4">
                    <asp:DropDownList ID="ddl_Cajero" runat="server" Height="20px" Width="400px"
                        DataTextField="Descripcion" DataValueField="Id_Cajero" AutoPostBack="True">
                    </asp:DropDownList>
                </td>

                <td style="width: 100px">
                    <asp:CheckBox ID="chk_Cajeros" runat="server" Text="Todos"
                        AutoPostBack="True" />
                </td>

            </tr>

            <tr>

                <td style="text-align: right; width: 80px">
                    <asp:Label ID="Label1" runat="server" Text="Status"></asp:Label>
                </td>

                <td colspan="4">
                    <asp:DropDownList ID="ddl_Status" runat="server" Height="20px" Width="250px" AutoPostBack="True">

                        <asp:ListItem Value="0" Text="Seleccione..." />
                        <asp:ListItem Value="A" Text="EN EPERA" />
                        <asp:ListItem Value="ID" Text="INSTALACION DESTINO" />
                        <asp:ListItem Value="VA" Text="ATENDIDA" />
                        <asp:ListItem Value="CA" Text="CANCELADA" />
                        <asp:ListItem Value="TR" Text="TRANSBORDADA" />

                    </asp:DropDownList>

                    <asp:CheckBox ID="chk_Todos" runat="server" Text="Todos"
                        AutoPostBack="True" />
                </td>

                <td style="width: 100px">

                    <asp:Button ID="btn_Mostrar" runat="server" Height="26px" Text="Consultar" Width="90px" CssClass="button" />

                </td>

            </tr>

        </table>
    </div>

    <fieldset id="fst_fallas" runat="server" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Fallas</legend>
        <asp:GridView ID="gv_Fallas" runat="server" AutoGenerateColumns="False"
            AllowPaging="True" CellPadding="10" CellSpacing="1" Width="100%"
            CssClass="gv_general" PageSize="25" DataKeyNames="Id_Falla,QuedoFuncionando,Observaciones,TarjetasReales,TarjetasTeoricas,HuboRetiro,RolloNuevo,Modo_Afecta,DescargaInfo,RemisionRetiro">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="imgBtn_selecciona" CommandName="Select" runat="server"
                            ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                    <ItemStyle Width="20px" Wrap="False" />
                </asp:TemplateField>

                <asp:BoundField DataField="NumReporte" HeaderText="No. Reporte" HeaderStyle-HorizontalAlign="Left">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>

                <asp:BoundField DataField="No. Cajero" HeaderText="No. Cajero" HeaderStyle-HorizontalAlign="Center">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="Cajero" HeaderText="Cajero" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="400px">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha Captura" HeaderText="Fecha Captura" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Fecha Requerida" HeaderText="Fecha Requerida" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Hora Requerida" HeaderText="Hora Requerida" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Tipo" HeaderText="Tipo" HeaderStyle-HorizontalAlign="Left">

                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Left">
                    <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:ButtonField ButtonType="Image" CommandName="VerDes" ImageUrl="~/Imagenes/Detalle.png" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />

        </asp:GridView>
    </fieldset>

    <fieldset id="fst_DetalleD" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Detalle de Falla</legend>

        <table style="width: 100%">
            <tr>
                <td style="text-align: right; width: 140px">
                    <asp:Label ID="lbl_quedofuncionando" runat="server" Text="Quedo Funcionando"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rdb_funcionaSI" runat="server" GroupName="quedoFuncionando" Text="Si" Enabled="False" />
                    <asp:RadioButton ID="rdb_funcionaNO" runat="server" GroupName="quedoFuncionando" Text="No" Enabled="False" />

                </td>
                <td style="width: 30px"></td>
                <td>
                    <asp:Label ID="lbl_Observaciones" runat="server" Text="Observaciones"></asp:Label>
                </td>
            </tr>

            <tr>
                <td style="text-align: right; width: 140px">
                    <asp:Label ID="lbl_cambiopapel" runat="server" Text="Cambio Papel"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rdb_cambiopapelSI" runat="server" Text="Si" GroupName="cambioPapel" Enabled="False" />
                    <asp:RadioButton ID="rdb_cambiopapelNO" runat="server" Text="No" GroupName="cambioPapel" Enabled="False" />
                </td>
                <td style="width: 30px"></td>

                <td rowspan="7" style="height: 100%">
                    <asp:TextBox ID="tbx_Observaciones" runat="server" Width="99%" Height="132px" TextMode="MultiLine" Font-Names="Verdana,Arial" Font-Size="X-Small" ReadOnly="True"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 140px">
                    <asp:Label ID="lbl_descargoinfo" runat="server" Text="Descargó Información"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButton ID="rdb_descargoinfoSI" runat="server" Text="Si" GroupName="descargoInfo" Enabled="False" />
                    <asp:RadioButton ID="rdb_descargoinfoNO" runat="server" Text="No" GroupName="descargoInfo" Enabled="False" />
                </td>
                <td style="width: 30px"></td>
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
                <td style="width: 60px">
                    <asp:Label ID="lbl_Teoricas" runat="server" Text="Teoricas"></asp:Label>
                </td>
                <td style="text-align: right; width: 60px">
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

    <br />

    <fieldset id="fst_tarjetasRetenidas" style="border: solid 1px; border-color: Gray; padding-left: 5px">
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



