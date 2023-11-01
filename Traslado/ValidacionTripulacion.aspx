<%@ Page Title="Traslado>Validacion de tripulacion" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="ValidacionTripulacion.aspx.vb" Inherits="PortalSIAC.ValidacionTripulacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hdf_ImagenesDisponibles" runat="server" Value="0" />

    <script language="JavaScript" type="text/javascript"> 
<!-- 

   /* var message = "Funcion deshabilitada!";

    //para IE 
    function clickIE4() {
        if (event.button == 2) {
            alert(message);
            return false;
        }
    }

    //Para Netscape u otros browsers 
    function clickNS4(e) {
        if (document.layers || document.getElementById && !document.all) {
            if (e.which == 2 || e.which == 3) {
                alert(message);
                return false;
            }
        }
    }

    if (document.layers) {
        document.captureEvents(Event.MOUSEDOWN);
        document.onmousedown = clickNS4;
    }
    else if (document.all && !document.getElementById) {
        document.onmousedown = clickIE4;
    }

    document.oncontextmenu = new Function("alert(message);return false")

        // --> 

        */
    </script>

   

    <fieldset style="border: 1px solid Gray">
        <table>
            <tr>
                <td align="right">Fecha
                </td>
                <td>
                    <asp:TextBox ID="txt_FechaInicial" runat="server" CssClass="calendarioAjax" AutoPostBack="true" />
                    <asp:CalendarExtender ID="txt_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="txt_FechaInicial" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
                <td></td>
            </tr>
            <tr>
                <td align="right">Clientes
                </td>
                <td>
                    <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="Nombre_Comercial" Width="400"
                        DataValueField="Id_Cliente" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:CheckBox ID="cbx_Todos_Clientes" runat="server" Text="Todos" AutoPostBack="True" />
                </td>
                <td>
                    <asp:Button ID="btn_Mostrar" runat="server" Text="Consultar" CssClass="button"
                        Style="height: 26px" Height="26px" Width="90px" />
                </td>

            </tr>
        </table>

    </fieldset>
    <fieldset id="fst_Unidades" runat="server" style="border: 1px solid Gray; padding-left: 5px">
        <legend></legend>

        <asp:GridView ID="gv_Lista" runat="server" DataKeyNames="Id_Punto,Placas,Id_Unidad" CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="100%" AllowPaging="True" PageSize="25">
            <Columns>

                <asp:CommandField ButtonType="Image" SelectImageUrl="~/Imagenes/1rightarrow.png"
                    ShowSelectButton="true" ItemStyle-Width="20px" />
       
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-Width="80px" />
                <asp:BoundField DataField="Unidad" HeaderText="Unidad" ItemStyle-Width="270px" />
                <asp:BoundField DataField="Origen" HeaderText="Origen" />
                <asp:BoundField DataField="Destino" HeaderText="Destino" />
                
            </Columns>
            <RowStyle CssClass="rowHover" />
            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <SelectedRowStyle CssClass="FilaSeleccionada_gv" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>
    </fieldset>

    <fieldset id="fst_Tripulacion" runat="server" style="width: 760px; border: 1px solid Gray">
        <legend>Tripulación  </legend>
        <br />
        <asp:Panel ID="pnl_Tripulacion" runat="server" Visible="False" Width="98%">
            <table id="tbl_Unidad">
                <tr>
                    <td style="width: 300px">
                        <b>Unidad: </b>
                    </td>
                    <td rowspan="6">
                        <asp:Image ID="img_UnidadTV" runat="server" Width="200" Height="200" />
                    </td>

                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Unidad" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <b>Placas: </b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Placas" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 33px"></td>
                </tr>
                <tr>
                    <td style="height: 33px"></td>
                </tr>

            </table>

            <hr />
            <br />

            <table id="tbl_Operador">
                <tr>
                    <td style="width: 300px">
                        <b>Operador: </b>
                    </td>
                    <td rowspan="6">
                        <asp:Image ID="img_Operador" runat="server" Width="200" Height="200" />
                        <br />
                    </td>
                    <td rowspan="6" style="padding-left: 10px">
                        <asp:Image ID="img_OperadorFirma" runat="server" Width="200" Height="200" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Operador" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <b>Clave: </b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_OperadorClave" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 33px"></td>
                </tr>
                <tr>
                    <td style="height: 33px"></td>
                </tr>

            </table>
            <br />
            <table id="tbl_Cajero">
                <tr>
                    <td style="width: 300px">
                        <b>Cajero: </b>
                    </td>
                    <td rowspan="6">
                        <asp:Image ID="img_Cajero" runat="server" Width="200" Height="200" />
                    </td>
                    <td rowspan="6" style="padding-left: 10px">
                        <asp:Image ID="img_CajeroFirma" runat="server" Width="200" Height="200" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_Cajero" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <b>Clave: </b>
                    </td>
                </tr>
                <tr>
                    <td style="width: 300px">
                        <asp:Label ID="lbl_CajeroClave" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 33px"></td>
                </tr>
                <tr>
                    <td style="height: 33px"></td>
                </tr>
            </table>
            <br />
            <hr />
            <b>Custodios</b>

            <asp:DataList ID="dl_Custodios" runat="server" Width="97%" RepeatColumns="1">
                <ItemTemplate>
                    <table runat="server">
                        <tr runat="server">
                            <td style="width: 300px">
                                <b>Nombre: </b>
                            </td>
                            <td rowspan="6" runat="server" style="padding-left: 10px">
                                <asp:Image ID="img_Custodio" runat="server"
                                    Width="200" Height="200" ImageUrl='<%#Eval("Foto")%>' />
                            </td>
                            <td rowspan="6" style="padding-left: 10px">
                                <asp:Image ID="img_CustodioFirma" runat="server"
                                    Width="200" Height="200" ImageUrl='<%#Eval("Firma")%>' />
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 300px">
                                <asp:Label ID="lbl_Nombre" runat="server">
                                    <%#Eval("Nombre")%>
                                </asp:Label>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 300px">
                                <b>Clave: </b>
                            </td>
                        </tr>

                        <tr>
                            <td style="width: 300px">
                                <asp:Label ID="lbl_Clave" runat="server">
                                    <%#Eval("Clave")%>
                                </asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 33px"></td>
                        </tr>
                        <tr>
                            <td style="height: 33px"></td>
                        </tr>

                    </table>
                </ItemTemplate>
            </asp:DataList>

        </asp:Panel>

    

    </fieldset>



    <fieldset id ="validar" style="border:none">
            <asp:Button ID ="btnValidar" runat ="server" Text="Validar" Height="40px" Width="150px" Font-Bold="true" CssClass="button" />
            <button  type="button" id="btn_Imprimir" class="entrada button" disabled="disabled">Imprimir</button>
            
        <!--<asp:Button ID ="btnImprimir" runat ="server" Text="Imprimir" Height="40px" Width="150px" Font-Bold="true" Enabled="False"  /> -->
    </fieldset>
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
            var imagenesDisponibles = document.getElementById("<%= hdf_ImagenesDisponibles.ClientID%>").value;
            //si hay imagenes disponibles para imprimir retorna 1 en caso contrario 0
            if (imagenesDisponibles == 1) {
                document.getElementById("btn_Imprimir").disabled = false;
            }


            var btnImprimir = document.getElementById("btn_Imprimir");

            btnImprimir.onclick = function () {
                imprimir();
            }

        }

        function imprimir() {
            var nuevaEstructura = "<html><title></title><head></head><body>";
            var cerraduraBody = "</body>"
            var cerradurahtml = "</html>"
            var contenedorImagen = document.all.item("<%= pnl_Tripulacion.ClientID%>").innerHTML;
           // var contenedorPagina = document.body.innerHTML;
            document.body.innerHTML = nuevaEstructura + contenedorImagen + cerraduraBody + cerradurahtml;
            window.print()
            //document.body.innerHTML = contenedorPagina;
            location.reload()
        }
    </script>
</asp:Content>



