<%@ Page Title="Carta Porte" Language="VB" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="CPorte.aspx.vb" Inherits="PortalSIAC._CPorte" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.12.0/moment.js"></script>
    <style>
        .gridcss {
            text-align: center;
            background-color: #fff;
            font-family: Arial;
            font-size: 14px;
            color: #000;
            margin-top :15px
        }
    </style>
    <script type="text/javascript">
        $(function () {
            var now = moment();

            $.datepicker.setDefaults($.datepicker.regional["es"]);

            $("#datepickerI").datepicker({
                dateFormat: 'dd/mm/yy'
            });
            $("#datepickerF").datepicker({
                dateFormat: 'dd/mm/yy'
            });
            $("#datepickerI").datepicker('setDate', now.format('DD/MM/YYYY'));
            $("#datepickerF").datepicker('setDate', now.format('DD/MM/YYYY'));
        });

    </script>

    <fieldset>
         <table  border="0" cellpadding="0" cellspacing="10" >
        <tr>
            <td width="50%" style="height: 36px">
                <div>RFC:</div>
                <asp:TextBox size="25" name="RFC" ID="RFC" value="" runat="server" ReadOnly="True" />
            </td>

            <td width="20%" style="height: 36px">
                <div>FOLIO:</div>
                <asp:TextBox size="25" name="FOLIO" ID="FOLIO" value="" runat="server" />
            </td>

            <td style="width: 30%">
                <div>Fecha Inicial:</div>
                <input type="text" id="datepickerI" name="datepickerI" value=""></td>
            <td style="width: 30%">
                <div>Fecha Final:</div>
                <input type="text" id="datepickerF" name="datepickerF" value=""></td>
            <td width="50%" style="height: 36px">&nbsp;&nbsp;  </td>
            <td style="width: 30%; height: 36px;">
                <asp:Button ID="Button1" runat="server" Text="Buscar" CssClass="buttonB" />
        </tr>

    </table>
    </fieldset>
    <div class="form-horizontal2" style="margin-top:15px">

        <table style="width: 100%;">
            <tr>
                <asp:Button ID="btnVisualizar" runat="server" Text="Visualizar" Width="119px" Height="35px" CssClass="button" Visible="False" />
                <asp:Button ID="btnDescargar" runat="server" Text="Descargar" Width="119px" Height="35px" CssClass="button" Visible="False" />
                <asp:Button ID="btnExcel" runat="server" Text="Exportar Excel" Width="119px" Height="35px" CssClass="button" Visible="False" />
               
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridcss">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="CheckBox2" runat="server" AutoPostBack="True" Width="35px"
                            OnCheckedChanged="CheckBox2_CheckedChanged " />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Sucursal" HeaderText="Sucursal" SortExpression="Sucursal" ItemStyle-Width="500px" ItemStyle-Height="35px" HeaderStyle-CssClass="header"></asp:BoundField>
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" ItemStyle-Width="200px" ItemStyle-Height="35px" HeaderStyle-CssClass="header"></asp:BoundField>
                <asp:BoundField DataField="Factura" HeaderText="Folio Factura" SortExpression="Folio" ItemStyle-Width="200px" ItemStyle-Height="35px" HeaderStyle-CssClass="header"></asp:BoundField>
                <asp:BoundField DataField="Directorio" HeaderText="Nombre Factura" SortExpression="Directorio" ItemStyle-Width="300px" ItemStyle-Height="35px" HeaderStyle-CssClass="header"></asp:BoundField>
                <asp:BoundField DataField="CTOTAL" HeaderText="Total" SortExpression="CTOTAL" ItemStyle-Width="200px" ItemStyle-Height="35px" HeaderStyle-CssClass="header"></asp:BoundField>

            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
