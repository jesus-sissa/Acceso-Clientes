<%@ Page Title="SISSA>Consultar Remision" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="Consultar_Remision.aspx.vb" Inherits="PortalSIAC.Consultar_Remision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        window.onload = function () {
            document.getElementById("<%= txt_Remision.ClientID%>").focus()           
            if (sessionStorage.getItem('key') == document.getElementById("<%= txt_Remision.ClientID%>").value) {
                    document.getElementById("<%= Alerta.ClientID%>").innerHTML = "";
                    document.getElementById("<%= txt_Remision.ClientID%>").focus()
             }
        }
        function Setval() {
            sessionStorage.setItem('key', "");
        }
    </script>
<%--    <asp:updatepanel runat="server">
        <ContentTemplate>--%>
              <fieldset style="border: 1px solid Gray">
        <asp:Label ID="Notificacion" Font-Size="Medium" runat="server"
          ForeColor="Red" Text="*Para obtener una busqueda mas eficiente se recomienda seleccionar el tipo de remisión corrrecto."  >       
     </asp:Label>
             <br />
        <table>
             <tr>
                <td>Tipo Remision 
                </td>
                <td>
                    <asp:DropDownList ID="ddl_tipo" runat="server" AutoPostBack="true"  DataTextField="Nombre" Width="270px"
                        DataValueField="Clave SP" Enabled="False">
                        <asp:ListItem Value="MAT">MATERIAL</asp:ListItem>
                        <asp:ListItem Value="TRAS">RECOLECCION</asp:ListItem>
                        <asp:ListItem Value="DOT">DOTACION</asp:ListItem>
                        <asp:ListItem Value="ATMD">ATMD</asp:ListItem>
                        <asp:ListItem Value="ATMF">ATMF</asp:ListItem>
                        <asp:ListItem Value="ATMC">ATMC</asp:ListItem>
                    </asp:DropDownList>
                
                </td>
                <td><asp:CheckBox  ID="cbx_todos" runat="server" Text="Todos" AutoPostBack="true" Checked="True" /></td>         
                <td class="auto-style1">
                     
                </td>
            </tr> 
            <tr>
                <td>Numero de remision 
                </td>
                <td>
                    <asp:TextBox ID="txt_Remision" runat="server" Height="28px" Width="270px" Font-Size="20pt" />          
                    <asp:FilteredTextBoxExtender ID="txt_Remision_v"
                        runat="server" Enabled="True" TargetControlID="txt_Remision"
                        ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>                                   
                </td>
                <td><asp:Button ID="Consultar"  runat="server" Text="Consultar" OnClientClick="Setval();"  Height="26px" Width="90px " CssClass="button" /></td>         
                <td class="auto-style1">
                   
                </td>
            </tr>
      <%--      <tr>
                <td><asp:FileUpload ID="Subir" runat="server" />
                </td>
                <td><asp:Button ID="Subirb"  runat="server" Text="Subir"   Height="26px" Width="90px " /></td>
                <td><asp:Button ID="Descargar1"  runat="server" Text="Descargar"   Height="26px" Width="90px " /></td>
            </tr>   --%>         
        </table>
        <br />
         <asp:Label ID="Alerta" Font-Size="Large" runat="server" ForeColor="Red"  Text="" >
             </asp:Label>        
    </fieldset>
<%--        </ContentTemplate>
    </asp:updatepanel>--%>
  
</asp:Content>
