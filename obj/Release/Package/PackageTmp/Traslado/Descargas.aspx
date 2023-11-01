<%@ Page Title="Traslado>Descarga de Documentos" Language="vb" AutoEventWireup="false"
    MasterPageFile="~/MasterPage.Master" CodeBehind="Descargas.aspx.vb"
    Inherits="PortalSIAC.Descargas" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="fst_documentos" runat="server" style="border: 1px solid Gray; padding-left: 5px">
                <legend>Documentos Disponibles</legend>

        <asp:GridView ID="gv_Lista" runat="server" DataKeyNames="Id_Doc" CellPadding="10" AutoGenerateColumns="False"
            CssClass="gv_general" CellSpacing="1" Width="100%" AllowPaging="True"
            PageSize="25">
            <Columns>

                <asp:ButtonField DataTextField="Nombre_Doc" HeaderText="Nombre">
                    <HeaderStyle HorizontalAlign="Left" Width="30%" />
                    <ItemStyle HorizontalAlign="Left" Width="30%" Font-Underline="true" ForeColor="Blue" />
                </asp:ButtonField>
                <asp:BoundField DataField="Descripcion_Doc" HeaderText="Descripcion">
                    <HeaderStyle HorizontalAlign="Left" Width="60%" />
                    <ItemStyle HorizontalAlign="Left" Width="60%" />
                </asp:BoundField>
                <asp:BoundField DataField="FechaActualizacion"
                    HeaderText="Fecha de Actualizacion">

                    <HeaderStyle HorizontalAlign="Center" Width="10%" />
                    <ItemStyle HorizontalAlign="Center" Width="10%" />
                </asp:BoundField>

            </Columns>

            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
            <PagerStyle CssClass="Paginado_gv" HorizontalAlign="Center" />
        </asp:GridView>

    </fieldset>

  </asp:Content>
