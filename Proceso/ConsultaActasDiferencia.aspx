<%@ Page Title="Proceso>Consulta de Actas de Diferencia" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaActasDiferencia.aspx.vb" Inherits="PortalSIAC.ConsultaActasDiferencia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset style="border: 1px solid Gray">

        <table>
            <tr>

                <td style="text-align: right">
                    <label>
                        Fecha Inicial</label>
                </td>

                <td>
                    <asp:TextBox ID="txt_FInicial" runat="server" AutoPostBack="true" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FInicial_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="true"
                        TargetControlID="txt_FInicial" Format="dd/MM/yyyy"
                        PopupPosition="BottomRight">
                    </asp:CalendarExtender>
                </td>
                <td></td>

                <td style="text-align: right">
                    <label>Fecha Final</label>
                    <asp:TextBox ID="txt_FFinal" runat="server" AutoPostBack="true" CssClass="calendarioAjax" MaxLength="10" />
                    <asp:CalendarExtender ID="txt_FFinal_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FFinal" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
                <td></td>

            </tr>
            <tr>
                <td align="right">Cliente
                </td>
                <td colspan="3">

                    <asp:DropDownList ID="ddl_Clientes" runat="server" AutoPostBack="true"
                        DataTextField="NombreComercial" Width="400px"
                        DataValueField="Id_Cliente">
                    </asp:DropDownList>

                </td>
                <td style="width: 90px">
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />

                </td>

            </tr>
            <tr>
                <td align="right">Caja Bancaria
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CajaBancaria" runat="server" AutoPostBack="true"
                        DataTextField="Nombre_Comercial" Width="400px"
                        DataValueField="Id_CajaBancaria">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" Height="26px" Width="90px" CssClass="button" />

                </td>

            </tr>
        </table>

    </fieldset>

    <fieldset id="fst_ActasDiferencia" runat="server" style="overflow: auto; border: 1px solid Gray; padding-left: 5px">
        <legend>Actas</legend>
        <asp:GridView ID="gv_CV" runat="server" CellPadding="10" AutoGenerateColumns="False" DataKeyNames="Id_Acta"
            CssClass="gv_general" AllowPaging="True" CellSpacing="1" Width="100%"
            PageSize="15">
            <Columns>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha">
                    <HeaderStyle HorizontalAlign="Center" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Acta" HeaderText="Acta">
                    <HeaderStyle HorizontalAlign="center" />
                    <ItemStyle CssClass="gv_Item" />
                </asp:BoundField>
                <asp:BoundField DataField="Remision" HeaderText="Remision">
                    <ItemStyle CssClass="gv_Item" HorizontalAlign="Center" />
                    <HeaderStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="Caja" HeaderText="Caja">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="400" />
                    <HeaderStyle HorizontalAlign="center" />
                </asp:BoundField>
                <asp:BoundField DataField="Cliente" HeaderText="Cliente">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="300" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Tipo Diferencia" HeaderText="Tipo Diferencia">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="300" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="300" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>

                <asp:BoundField DataField="Status" HeaderText="Status">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Comentarios" HeaderText="Comentarios">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="200" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Comentarios_Valida" HeaderText="Validacion">
                    <ItemStyle HorizontalAlign="Center" CssClass="gv_Item" Width="200" />
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:BoundField>

            </Columns>


            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>
</asp:Content>
