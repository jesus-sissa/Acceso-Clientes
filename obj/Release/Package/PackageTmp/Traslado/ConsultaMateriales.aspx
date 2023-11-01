<%@ Page Title="Traslado>Consulta de Materiales" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ConsultaMateriales.aspx.vb" Inherits="PortalSIAC.ConsultaMateriales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <asp:UpdatePanel runat="server">
        <ContentTemplate>--%>
             <fieldset style="border: 1px solid Gray">
        <table>
            <tr>
                <td style="text-align: right">Fecha Inicial
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaInicial" runat="server" CssClass="calendarioAjax" AutoPostBack="True" />
                    <asp:CalendarExtender ID="txt_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>
                </td>
                <td></td>
                <td style="text-align: right">Fecha Final
                
                    <asp:TextBox ID="txt_FechaFinal" runat="server" CssClass="calendarioAjax" AutoPostBack="True" />
                    <asp:CalendarExtender ID="CalendarExtender1" CssClass="calendarioAjax" runat="server" Enabled="True" TargetControlID="txt_FechaFinal"
                        Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
                <td></td>
            </tr>
            <tr>
                <td style="text-align: right">Clientes
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="NombreComercial" Width="400"
                        DataValueField="Id_Cliente" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td style="width: 80px">
                    <asp:CheckBox  ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />
                    <%--<asp:CheckBox  ID="cbx_Todos_Clientes1" runat="server" Text ="todos"/>--%>
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" CssClass="button"
                       Height="26px" Width="90px" />

                </td>

            </tr>
        </table>
    </fieldset>

    <fieldset id="fst_Materiales" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend>Consulta Materiales</legend>
                   <table>

            <tr>
                <td>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/1rightarrow.png"/>
                </td>
                <td>
                    Ver Detalle
                </td>
                <td style="width:20px"></td>

                   <td>
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Imagenes/Detalle.png"/>
                </td>
                <td>
                    Ver Remision 
                </td>
            </tr>

        </table>
        <asp:GridView ID="gv_ConsultaMat" runat="server" AutoGenerateColumns="False" CellPadding="4"
            Width="100%" CssClass="gv_general"
            AllowPaging="True" EnableModelValidation="True" PageSize="25" DataKeyNames="Folio,Status">
            <Columns>

                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="VerDetalle" runat="server" CommandName="Select" ImageUrl="~/Imagenes/1rightarrow.png" />
                    </ItemTemplate>
                    <ItemStyle Width="20px" Wrap="False" />
                </asp:TemplateField>

                <asp:BoundField DataField="Folio" HeaderText="Folio">
                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="50px" />
                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="FechaSolicita" HeaderText="Fecha Solicita">
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>

                <asp:BoundField DataField="HoraSolicita" HeaderText="Hora Solicita" ItemStyle-Width="100px" >
<ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FechaEntrega" HeaderText="Fecha de Entrega" ItemStyle-Width="100px" >
<ItemStyle Width="100px"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Cliente" HeaderText="Cliente" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>

<ItemStyle Wrap="True"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Remision" HeaderText="Remision" />
                <asp:ButtonField ButtonType="Image"  CommandName="VerDes" ImageUrl="~/Imagenes/Detalle.png" Text="Ver/Descargar" />
            </Columns>

            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>

    <fieldset id="fst_DetalleMateriales" runat="server" style="border: 1px solid Gray; padding-left:5px" >
        <legend>Detalle</legend>
        <asp:GridView ID="gv_Detalle" runat="server" AutoGenerateColumns="False" CellPadding="4" CssClass="gv_general"
            AllowPaging="True" Width="60%">

            <Columns>
                <asp:BoundField DataField="Material" HeaderText="Material"></asp:BoundField>
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad">
                    <HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" />
                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Width="80px" />
                </asp:BoundField>
            </Columns>


            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
        
     <%--<asp:Button ID ="btnImprimir" runat ="server" Text="Ver remisión" Height="40px" Width="150px" Font-Bold="true" Enabled="false"  />--%>
    </fieldset>
      <fieldset id ="validar" style="border:none">
            <%--<asp:Button ID ="btnValidar" runat ="server" Text="Validar" Height="40px" Width="150px" Font-Bold="true" />
            <%--<button type="button" runat ="server"  id="btn_Imprimir" class="entrada" >Ver remision</button>--%>
            
        <%--<asp:Button ID ="btnImprimir" runat ="server" Text="Imprimir" Height="40px" Width="150px" Font-Bold="true" Enabled="true"  />--%> 
    </fieldset>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
   
</asp:Content>

<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .entrada {
            height:40px;
            width:150px;
            font-weight:bold
        }
    </style>

    <script type="text/jscript">
        window.onload = function () {
            //var imagenesDisponibles = document.getElementById(%= hdf_ImagenesDisponibles.ClientID%>").value;
            //si hay imagenes disponibles para imprimir retorna 1 en caso contrario 0
            //if (imagenesDisponibles == 1) {
            //   document.getElementById("btn_Imprimir").disabled = false;
            //}

            document.getElementById("btn_Imprimir").disabled = false;
            var btnImprimir = document.getElementById("btn_Imprimir");

            btnImprimir.onclick = function () {
                // imprimir();
            }

        }

        function imprimir() {
            var nuevaEstructura = "<html><title></title><head></head><body>";
            var cerraduraBody = "</body>"
            var cerradurahtml = "</html>"
            var contenedorImagen = document.all.item("<%= fst_Materiales.ClientID%>").innerHTML;
            // var contenedorPagina = document.body.innerHTML;
            document.body.innerHTML = nuevaEstructura + contenedorImagen + cerraduraBody + cerradurahtml;
            window.print()
            //document.body.innerHTML = contenedorPagina;
            location.reload()
        }
    </script>
</asp:Content>



