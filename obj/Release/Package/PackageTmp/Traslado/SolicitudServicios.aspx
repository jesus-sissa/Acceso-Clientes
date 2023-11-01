<%@ Page Title="Traslado>Solicitud de Servicios" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="SolicitudServicios.aspx.vb" Inherits="PortalSIAC.SolicitudServicios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
       <asp:Label ID="NotaSolicitud" runat="server"
          ForeColor="Red" Text="Los servicios que se solicitan deben ser validados en la opción 'Autorizar Servicios',<br/>de lo contrario no le llegará la solicitud a la compañía." >
     </asp:Label>

     <br />
     <fieldset style="border:1px solid Gray" >
        <legend>Datos del Servicio</legend>
    <table cellspacing="3">
        <tr>
                <td align="right">
                Fecha Requerida
            </td>
            <td style="width:390px">
                <asp:TextBox ID="txt_Fecha" runat="server" CssClass="calendarioAjax" />
                <asp:CalendarExtender ID="txt_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                    TargetControlID="txt_Fecha" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
            </td>

        </tr>
        <tr>
            <td align="right">
                Solicita
            </td>
            <td style="width:390px">
                <asp:DropDownList ID="ddl_Clientes" runat="server" DataTextField="NombreComercial"
                    Width="400px" DataValueField="Id_Cliente" AutoPostBack="true">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                Servicio
            </td>
            <td style="width:390px">
                <asp:DropDownList ID="ddl_Servicio" runat="server" DataTextField="Descripcion" DataValueField="Id_CS"
                    Width="400px" AutoPostBack="true">
                </asp:DropDownList>
            </td>
        
        </tr>
    </table>
         </fieldset>
    <fieldset style="border:1px solid Gray">
        <legend>Cliente de Origen</legend>
        <table cellspacing="3" width="300px">
            <tr>
                <td align="right" class="auto-style3">
                    Origen
                </td>
                <td colspan="3" class="auto-style3">
                    <asp:DropDownList ID="ddl_Origen" runat="server" Width="400px" DataTextField="Nombre_Comercial"
                        DataValueField="Id_Cliente">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    Horario de Recolección
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 40px">
                    Horario
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddl_De" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                        DataValueField="Hora" AutoPostBack="True">
                    </asp:DropDownList>
                    <asp:Label ID="Label1" runat="server" Text="/" Font-Bold="True" 
                        Font-Size="Medium" ></asp:Label>
                    <asp:DropDownList ID="ddl_A" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                        DataValueField="Hora" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </fieldset>
    <fieldset style="border:1px solid Gray">
        <legend>Cliente de Destino</legend>
        <table cellspacing="3" width="300px">
            <tr>
                <td align="right">
                    Destino</td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Destino" runat="server" Width="400px" DataTextField="Nombre_Comercial"
                        DataValueField="Id_Cliente">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center" class="auto-style2">
                    Horario de Entrega
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 40px">
                    Horario
                </td>
                <td align="left" >
                    <asp:DropDownList ID="ddl_DeD" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                        DataValueField="Hora" AutoPostBack="True">
                    </asp:DropDownList>
              <asp:Label ID="Label2" runat="server" Text="/" Font-Bold="True" 
                        Font-Size="Medium" ></asp:Label>
                    <asp:DropDownList ID="ddl_AD" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                        DataValueField="Hora" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
          <asp:Label ID="lbl_comentariosAd" runat="server" Text="Comentarios Adicionales"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Comentarios" TextMode="MultiLine" Width="100%"
             Height="50px" runat="server" CssClass="tbx_Mayusculas">
        </asp:TextBox>

    <asp:Button ID="btn_Solicitar" runat="server" Text="Solicitar" Height="26px" Width="90px" CssClass="button" />
</asp:Content>
