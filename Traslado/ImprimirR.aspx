<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ImprimirR.aspx.vb" Inherits="PortalSIAC.ImprimirR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"/>--%>
    <link href="../Content/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../Scripts/JsBarcode.all.min.js"></script>
    <%--<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/jsbarcode@3.11.0/dist/JsBarcode.all.min.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="Content/rem.css" />
    
    <style type="text/css">

body:after {
  content:""; 
  font-size: 14em;  
  color: rgba(52, 166, 214, 0.4);
  align-items: center;
  justify-content: center;
  position:fixed;
  top: auto;
  right: auto;
  bottom: auto;
  left: auto;   
}
        .newdorder
        {
            border-radius: 30px
        }
        .auto-style3
        {
            height: 27px;
        }
        .auto-style4
        {
            height: 26px;
        }
        .auto-style6
        {
            height: 84px;
            width: 1100px;
        }
        .divv
        {
           background-color: white;
        }
        </style>
    
</head>
<body>     
    <form id="Form1" runat="server" class="divv">
          &nbsp;&nbsp;&nbsp;
          <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager> 
        <fieldset id="fst_Materiales" runat="server" style=" padding-left: 5px">            
    <div class="container">
        <table class="table table-responsive-md" id="Encabezado" >
             <tr style="text-align:right">
                 <td  class="auto-style3" >
                      <asp:Image id="Image1" runat="server" Height="130px" ImageUrl="~/Imagenes/Logosissa.png"  Width="100px" AlternateText="Imagen no disponible" ImageAlign="TextTop" />  
                 </td>
                 <td style="text-align:center" class="auto-style6">
               <h3 class="d-flex justify-content-center mt-4">Servicio Integral de Seguridad. S.A. de C.V.</h3>
               <p class="text-center">ALVAREZ NTE. 209 MONTERREY, N.L. C.P. 64000<br/>CONMUTADOR: 8047-4545, 8047-4546
                    FAX 8047 4550
               <br/>www.sissaseguridad.com
                </p>
                     <div id="barcode"><img id="codigo"/ width="220px" height="80px"></div>
                 </td>
                  <td  class="auto-style3" style="text-align:left">
                      <%--<asp:Image id="Image2" runat="server" Height="130px" ImageUrl="~/Imagenes/Logosissa.png"  Width="100px" AlternateText="Imagen no disponible" ImageAlign="Left" />  --%>
                      <br /><br />  <br /><br />
                    <h4> <span class="auto-style4"><b>REMISION:</b><br />
                         <asp:Label runat="server" ID="remision_n" CssClass="badge badge-danger"></asp:Label>                         
                     <br /></span></h4>
                     <span  class="auto-style4">  <b>RUTA N°:</b><br /><asp:Label ID="ruta_remision" runat="server" CssClass="font-italic"></asp:Label></span><br />
                     <span  class="auto-style4">  <b>UNIDAD:</b><br /><asp:Label ID="unidad_remision" runat="server" CssClass="font-italic"></asp:Label></span>
                 </td>
                <%-- <td  class="auto-style5">
                     <br /><br />  <br /><br />
                    <h4> <span class="auto-style4"><b>REMISION:</b><br />
                         <asp:Label runat="server" ID="remision_n" CssClass="badge badge-danger"></asp:Label>                         
                     <br /></span></h4>
                     <span  class="auto-style4">  <b>RUTA N°:</b><br /><asp:Label ID="ruta_remision" runat="server" CssClass="font-italic"></asp:Label></span><br />
                     <span  class="auto-style4">  <b>UNIDAD:</b><br /><asp:Label ID="unidad_remision" runat="server" CssClass="font-italic"></asp:Label></span>
                 </td>--%>
             </tr>        
         </table>
        </div>    
            <div class="container rounded border border-secondary container"><br />
      <table class="  table table-responsive-md table-bordered " >
        <tr align="center" class="thead-light">
            <th colspan="6" class="auto-style3" >Billetes</th>
            <th colspan="9" class="auto-style3" >Monedas</th>
        </tr>
        <tr>
            <th class="auto-style3">1000</th>
            <th class="auto-style3">500</th>
            <th class="auto-style3">200</th>
            <th class="auto-style3">100</th>
            <th class="auto-style3">50</th>
            <th class="auto-style3">20</th>

            <th class="auto-style3">20</th>
            <th class="auto-style3">10</th>
            <th class="auto-style3">5</th>
            <th class="auto-style3">2</th>
            <th class="auto-style3">1</th>
            <th class="auto-style3">.50</th>
<%--            <th class="auto-style3">.20</th>
            <th class="auto-style3">.10</th>
            <th class="auto-style3">.5</th>--%>
        </tr> 
        <tr>
            <td>$<asp:Label ID="Mil" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Quinientos" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Docientos" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Cien" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Cincuenta" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Veinte" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="VeinteM" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Diez" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Cinco" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Dos" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="Uno" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="PCincuenta" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
<%--             <td>$<asp:Label ID="PVenite" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="PDiez" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>
             <td>$<asp:Label ID="PCinto" runat="server" CssClass="font-italic" Text="0"></asp:Label></td>--%>
        </tr>               
    </table>
                </div>
        
  <br />            
            <div class="container rounded border border-secondary container">
             <table id="Table1" runat="server" class="table table-responsive-md " >
                          <%--  <tr>
                               <td></td>
                                <td></td>
                                <td></td>
                                <td colspan="4">
                                    <b>  IMPORTE:</b>
                                </td>
                            </tr>--%>
                            <tr>
                                <td colspan="3" >
                                      <b>VALORES RECIBIDOS DE:</b> <asp:Label ID="traslada" runat="server" CssClass="font-italic"></asp:Label>
                                </td>
                                <td class="auto-style3">
                                     <b> MONEDA NACIONAL:</b>
                                </td>
                                <td class="auto-style3"><asp:Label ID="moneda_na" runat="server" CssClass="font-italic"></asp:Label></td>                               
                            </tr>
                                 <tr>
                                     <td colspan="3">
                                         <div id="Div1">
                                      <b>  NUM CLIENTE:</b><asp:Label ID="num_cliente" runat="server" CssClass="font-italic"></asp:Label>                                      
                                    </div>
                                     </td>
                                   
                                <td colspan="1">
                                    <b> MONEDA EXTRANJERA:</b>
                                </td>                               
                                <td><asp:Label ID="moneda_ex" runat="server" CssClass="font-italic"></asp:Label></td>                               
                            </tr>
                            <%--  --%>
                            <tr>
                                  <td colspan="3">
                                         <div class="mt-2">
                                       <b> FECHA:</b><asp:Label ID="fecha_va" runat="server" CssClass="font-italic"></asp:Label>
                                    </div>
                                     </td>
                             
                                <td class="auto-style3">
                                     <b> OTROS:</b>
                                </td>
                                <td class="auto-style3"><asp:Label ID="otros" runat="server" CssClass="font-italic"></asp:Label></td>                               
                            </tr>
                                 <tr>
                                        <td colspan="3" >
                                   <b> DIRECCION:</b><asp:Label ID="dir" runat="server" CssClass="font-italic"></asp:Label>
                                </td>
                                <td colspan="1">
                                    <b> TOTAL:</b>
                                </td>                               
                                <td><asp:Label ID="total" runat="server" CssClass="font-italic"></asp:Label></td>                               
                            </tr>
                            <%--  --%>
                          <tr>
                              <td>  <b>ENVASES CON BILLETES:</b><asp:Label ID="envases_bi" runat="server" CssClass="font-italic"></asp:Label></td>
                              <td>  <b>ENVASES CON MORRALLA:</b><asp:Label ID="envases_mo" runat="server" CssClass="font-italic"></asp:Label></td>
                              <td>  <b>ENVASES MIXTOS:</b><asp:Label ID="envases_do" runat="server" CssClass="font-italic"></asp:Label></td>
                              <td colspan="2">  <b>ENVASES TOTALES:</b> <asp:Label ID="envases_total" runat="server" CssClass="font-italic"></asp:Label></td>
                          </tr>
                            <%--  --%>
                        <tr>
                            <td colspan="5" class="auto-style4">  <b>IMPORTE EN LETRAS:</b><asp:Label ID="importe_le" runat="server" CssClass="font-italic"></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="5">  <b>ENTREGAR ENVASES EN:</b><asp:Label ID="nombre_co" runat="server" CssClass="font-italic"></asp:Label></td>
                        </tr>
                  <%--      <tr>
                            <td colspan="5">  <b>DIRECCIÓN:</b></td>
                        </tr>--%>
                        <tr >
                            <td colspan="5" >  <b>SELLOS: </b><asp:Label ID="sellos_to" runat="server" CssClass="font-italic"></asp:Label></td>
                        </tr>
                        <tr >
                            <td colspan="5" >  <b>NOTAS: </b><asp:Label ID="notas" runat="server" CssClass="font-italic"></asp:Label></td>
                        </tr>
             </table><br />              
                </div><br />

            <div class="container">
                  <div class="row">
                    <div class="col border-secondary rounded border">
                        <table class="" >
                                                <tr>
                                                    <td colspan="2" >
                                                      <h5>FIRMA DE REMITENTE     </h5>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="auto-style4">  <b></b></td>
                                                    <td>
                                                        <img id="qrRemite" runat="server" />                                                                                                               
                                                    </td>                                                    
                                                </tr>                                          
                                            </table>
                    </div>
                    <div class="col border-secondary rounded border">
                        <table > 
                                                <tr>
                                                    <td><p></p></td>
                                                </tr>                               
                                                <tr>
                                                    <td align="center"><asp:Image ID="firma_cajero" runat="server" Width="179px" Height="64px"/><p>____________________________</p></td>                          
                                                </tr>
                                                  <tr>
                                                    <td align="center">  <b>TRASPORTACION DE VALORES</b></td>                                
                                                </tr>
                                            </table>
                    </div>
                    <div class="w-100"></div>
                    <div class="col border-secondary rounded border">
                            <table class="" >
                                                <tr>
                                                    <td colspan="2" >
                                                      <h5> FIRMA DE CONSIGNATORIO</h5>
                                                     
                                                    </td>
                                                    
                                                </tr>
                                                <tr>
                                                    <td class="auto-style4">  <b></b></td>                                             
                                                    <td>
                                                        <img id="qrConsignatorio" runat="server" />

                                                      <br />
                                                    </td>                        
                                                </tr>                                         
                            </table>

                       </div>
                          <div class="col border-secondary rounded border">
                         <table> 
                                                <tr>
                                                    <td>  <b>FECHA DE SERVICIO:</b></td>
                                                </tr>                               
                                                <tr>
                                                    <td align="center"><p><asp:Label ID="fecha_real" runat="server"></asp:Label></p><p><b>HORA DE SERVICIO:</b></p></td>                          
                                                </tr>
                                                  <tr>
                                                    <td align="center"> <asp:Label ID="hora_real" runat="server"></asp:Label> </td>                                
                                                </tr>
                                            </table>
                    </div>
                    </div>

  </div>  <br />   
   <div  class ="border-secondary rounded border container">
       <h5 class="text-center">IMPORTANTE</h5>       
       ° CUALQUIER ALTERACIÓN HACE NULO ESTE DOCUMENTO.<br />
       ° "LA COMPAÑIA " NO SERA RESPONSABLE POR INCUMPLIMIENTO DE ESTE SERVICIO EN CASOS FORTUITOS O FUERZA MAYOR.<br />
       ° "LA COMPAÑIA " NO ATENDERA RECLAMACION ALGUNA DESPUES DE 60 DIAS DE LA FECHA DE ESTE UNICO DOCUMENTO.<br />
       ° NO ENTREGUE SUS VALORES SI EXISTE DUDA SOBRE LA IDENTIDAD DEL PERSONAL.<br />
       ° PARA EFECTOS DE FACTURACION, CADA REMISION REPRESENTA UN SERVICIO.
   </div>
</fieldset>

  <div class="col-md-12 text-center divv">
      <br />
       <button type="button" runat ="server"  id="btn_Imprimir" class="btn btn-primary align-content-md-center "  >Imprimir</button>
  <%--    <button type="button" runat ="server"  id="btn_Buscar" class="btn btn-primary align-content-md-center " visible="false"  ></button>--%>
       <button type="button"  id="btn_cerrar" runat="server" class="btn btn-primary align-content-md-center">Cerrar</button>      
      </div>             
    </form>

  </body>
</html>

<script type="text/javascript">
    
    JsBarcode("#codigo", document.getElementById("remision_n").textContent);
</script>
    <script type="text/jscript">        
        window.onload = function () {
            document.getElementById("btn_Imprimir").disabled = false;
            var btnImprimir = document.getElementById("btn_Imprimir");
            var btncerrar = document.getElementById("btn_cerrar");

            

            btnImprimir.onclick = function () {
                 imprimir();
            }
            
           btncerrar.onclick = function () {
             sessionStorage.clear()
             var valor = document.getElementById("<%= remision_n.ClientID%>").innerHTML                
             sessionStorage.setItem('key', valor);
             history.back();
           }
        }

        function imprimir() {        
            var nuevaEstructura = "<html><title></title><head></head><body>";
            var cerraduraBody = "</body>"
            var cerradurahtml = "</html>"
            var contenedorImagen = document.all.item("<%= fst_Materiales.ClientID%>").innerHTML;
            document.body.innerHTML = nuevaEstructura + contenedorImagen + cerraduraBody + cerradurahtml;            
            window.print()
            location.reload()
        }
    </script>
