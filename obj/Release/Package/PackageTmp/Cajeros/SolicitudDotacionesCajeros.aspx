<%@ Page Title="Cajeros>Solicitud de Dotaciones" Language="vb" AutoEventWireup="false" CodeBehind="SolicitudDotacionesCajeros.aspx.vb"
    Inherits="PortalSIAC.Dotaciones" MasterPageFile="~/MasterPage.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <fieldset id="TextboxCaptura" runat="server" style="border: solid 1px; border-color: Gray">
        <legend>Datos Solicitud</legend>

        <table style="width: 893px">
            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="No. Reporte"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="tbx_NoReporte" runat="server" Width="120px" Wrap="False"
                        MaxLength="30"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label3" runat="server" Text="Cajero"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddl_Cajero" runat="server" AutoPostBack="True"
                        DataTextField="Descripcion" DataValueField="Id_Cajero" Width="400px">
                    </asp:DropDownList>
                </td>
            </tr>

            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label5" runat="server" Text="Moneda"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_Moneda" runat="server" Width="150px"
                        AutoPostBack="True" DataTextField="Nombre" DataValueField="Id_Moneda">
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label6" runat="server" Text="Hora que solicitó el Banco"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_HorasSol" runat="server" Width="40px">
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14 </asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                    </asp:DropDownList>
                    :
                <asp:DropDownList ID="ddl_MinutosSol" runat="server" Width="40px">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>31</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>33</asp:ListItem>
                    <asp:ListItem>34</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>36</asp:ListItem>
                    <asp:ListItem>37</asp:ListItem>
                    <asp:ListItem>38</asp:ListItem>
                    <asp:ListItem>39</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>41</asp:ListItem>
                    <asp:ListItem>42</asp:ListItem>
                    <asp:ListItem>43</asp:ListItem>
                    <asp:ListItem>44</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>46</asp:ListItem>
                    <asp:ListItem>47</asp:ListItem>
                    <asp:ListItem>48</asp:ListItem>
                    <asp:ListItem>49</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>51</asp:ListItem>
                    <asp:ListItem>52</asp:ListItem>
                    <asp:ListItem>53</asp:ListItem>
                    <asp:ListItem>54</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>56</asp:ListItem>
                    <asp:ListItem>57</asp:ListItem>
                    <asp:ListItem>58</asp:ListItem>
                    <asp:ListItem>59</asp:ListItem>
                </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label7" runat="server"
                        Text="Tiene prioridad por parte del Banco?"></asp:Label>
                </td>
                <td>
                    <asp:RadioButton ID="rdb_Siprioridad" runat="server" GroupName="Prioridad"
                        Text="Si" AutoPostBack="True" />
                    &nbsp;
                <asp:RadioButton ID="rdb_Noprioridad" runat="server" Checked="True" GroupName="Prioridad"
                    Text="No" AutoPostBack="True" />
                </td>
                <td style="text-align: right">&nbsp;</td>

            </tr>
            <tr>
                <td style="width: 246px; text-align: right">Fecha Entrega</td>
                <td>
                    <asp:TextBox ID="tbx_Fecha" runat="server" CssClass="calendarioAjax"
                        Width="88px"></asp:TextBox>
                    <asp:CalendarExtender ID="tbx_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                        TargetControlID="tbx_Fecha" Format="dd/MM/yyyy">
                    </asp:CalendarExtender>

                </td>
                <td style="text-align: right">&nbsp;</td>

            </tr>
            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label9" runat="server" Text="Entregar Entre"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddl_HorasInicio" runat="server" Width="40px">
                        <asp:ListItem>00</asp:ListItem>
                        <asp:ListItem>01</asp:ListItem>
                        <asp:ListItem>02</asp:ListItem>
                        <asp:ListItem>03</asp:ListItem>
                        <asp:ListItem>04</asp:ListItem>
                        <asp:ListItem>05</asp:ListItem>
                        <asp:ListItem>06</asp:ListItem>
                        <asp:ListItem>07</asp:ListItem>
                        <asp:ListItem>08</asp:ListItem>
                        <asp:ListItem>09</asp:ListItem>
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>11</asp:ListItem>
                        <asp:ListItem>12</asp:ListItem>
                        <asp:ListItem>13</asp:ListItem>
                        <asp:ListItem>14 </asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>16</asp:ListItem>
                        <asp:ListItem>17</asp:ListItem>
                        <asp:ListItem>18</asp:ListItem>
                        <asp:ListItem>19</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>21</asp:ListItem>
                        <asp:ListItem>22</asp:ListItem>
                        <asp:ListItem>23</asp:ListItem>
                    </asp:DropDownList>
                    :
                <asp:DropDownList ID="ddl_MinutosInicio" runat="server" Width="40px">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>31</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>33</asp:ListItem>
                    <asp:ListItem>34</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>36</asp:ListItem>
                    <asp:ListItem>37</asp:ListItem>
                    <asp:ListItem>38</asp:ListItem>
                    <asp:ListItem>39</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>41</asp:ListItem>
                    <asp:ListItem>42</asp:ListItem>
                    <asp:ListItem>43</asp:ListItem>
                    <asp:ListItem>44</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>46</asp:ListItem>
                    <asp:ListItem>47</asp:ListItem>
                    <asp:ListItem>48</asp:ListItem>
                    <asp:ListItem>49</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>51</asp:ListItem>
                    <asp:ListItem>52</asp:ListItem>
                    <asp:ListItem>53</asp:ListItem>
                    <asp:ListItem>54</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>56</asp:ListItem>
                    <asp:ListItem>57</asp:ListItem>
                    <asp:ListItem>58</asp:ListItem>
                    <asp:ListItem>59</asp:ListItem>
                </asp:DropDownList>
                    &nbsp;
                <asp:Label ID="Label14" runat="server" Text="y"></asp:Label>&nbsp;
                <asp:DropDownList ID="ddl_HorasFin" runat="server" Width="40px">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14 </asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                </asp:DropDownList>
                    :
                <asp:DropDownList ID="ddl_MinutosFin" runat="server" Width="40px">
                    <asp:ListItem>00</asp:ListItem>
                    <asp:ListItem>01</asp:ListItem>
                    <asp:ListItem>02</asp:ListItem>
                    <asp:ListItem>03</asp:ListItem>
                    <asp:ListItem>04</asp:ListItem>
                    <asp:ListItem>05</asp:ListItem>
                    <asp:ListItem>06</asp:ListItem>
                    <asp:ListItem>07</asp:ListItem>
                    <asp:ListItem>08</asp:ListItem>
                    <asp:ListItem>09</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                    <asp:ListItem>13</asp:ListItem>
                    <asp:ListItem>14</asp:ListItem>
                    <asp:ListItem>15</asp:ListItem>
                    <asp:ListItem>16</asp:ListItem>
                    <asp:ListItem>17</asp:ListItem>
                    <asp:ListItem>18</asp:ListItem>
                    <asp:ListItem>19</asp:ListItem>
                    <asp:ListItem>20</asp:ListItem>
                    <asp:ListItem>21</asp:ListItem>
                    <asp:ListItem>22</asp:ListItem>
                    <asp:ListItem>23</asp:ListItem>
                    <asp:ListItem>24</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>26</asp:ListItem>
                    <asp:ListItem>27</asp:ListItem>
                    <asp:ListItem>28</asp:ListItem>
                    <asp:ListItem>29</asp:ListItem>
                    <asp:ListItem>30</asp:ListItem>
                    <asp:ListItem>31</asp:ListItem>
                    <asp:ListItem>32</asp:ListItem>
                    <asp:ListItem>33</asp:ListItem>
                    <asp:ListItem>34</asp:ListItem>
                    <asp:ListItem>35</asp:ListItem>
                    <asp:ListItem>36</asp:ListItem>
                    <asp:ListItem>37</asp:ListItem>
                    <asp:ListItem>38</asp:ListItem>
                    <asp:ListItem>39</asp:ListItem>
                    <asp:ListItem>40</asp:ListItem>
                    <asp:ListItem>41</asp:ListItem>
                    <asp:ListItem>42</asp:ListItem>
                    <asp:ListItem>43</asp:ListItem>
                    <asp:ListItem>44</asp:ListItem>
                    <asp:ListItem>45</asp:ListItem>
                    <asp:ListItem>46</asp:ListItem>
                    <asp:ListItem>47</asp:ListItem>
                    <asp:ListItem>48</asp:ListItem>
                    <asp:ListItem>49</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>51</asp:ListItem>
                    <asp:ListItem>52</asp:ListItem>
                    <asp:ListItem>53</asp:ListItem>
                    <asp:ListItem>54</asp:ListItem>
                    <asp:ListItem>55</asp:ListItem>
                    <asp:ListItem>56</asp:ListItem>
                    <asp:ListItem>57</asp:ListItem>
                    <asp:ListItem>58</asp:ListItem>
                    <asp:ListItem>59</asp:ListItem>
                </asp:DropDownList>
                </td>
                <td style="text-align: right">&nbsp;</td>

            </tr>
            <tr>
                <td style="width: 246px; text-align: right">
                    <asp:Label ID="Label11" runat="server" Text="Requiere Corte"></asp:Label>
                </td>
                <td>
                    <asp:RadioButton ID="rdb_Sicorte" runat="server" GroupName="Requierecorte"
                        Text="Si" />
                    &nbsp;&nbsp;
                <asp:RadioButton ID="rdb_NoCorte" runat="server" Checked="True"
                    GroupName="Requierecorte" Text="No" />
                </td>
                <td style="text-align: right">&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="2" style="height: 45px">
                    <asp:Label ID="lbl_msjPrioridad" runat="server" ForeColor="Red"
                        Text="Cuando la Dotación tiene Prioridad por parte del Banco, el Horario de Entrega se calcula automáticamente basado en la hora en que se solicita y los Tiempos de Respuesta."
                        Visible="False"></asp:Label>
                </td>

            </tr>
        </table>

    </fieldset>
    <br />

    <fieldset id="fst_Dotaciones" style="border: solid 1px; border-color: Gray; padding-left: 5px">
        <legend>Detalle Solicitud</legend>
        <asp:GridView ID="gv_Dotaciones" runat="server"
            OnRowEditing="gv_Dotaciones_RowEditing"
            OnRowUpdating="gv_Dotaciones_RowUpdating"
            AutoGenerateColumns="False" CssClass="gv_general"
            DataKeyNames="Id_Denominacion">

            <Columns>
                <asp:BoundField DataField="Denominacion" HeaderText="Denominacion">
                    <ItemStyle HorizontalAlign="Right" Width="70px" Wrap="false" />
                </asp:BoundField>

                <asp:TemplateField HeaderText="Piezas">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="tbx_Piezas" Width="80px" Height="15px" MaxLength="3" Style="text-align: center"></asp:TextBox>

                        <asp:FilteredTextBoxExtender runat="server" ID="ftb_Piezas" TargetControlID="tbx_Piezas" FilterType="Numbers">
                        </asp:FilteredTextBoxExtender>

                    </ItemTemplate>
                    <ItemStyle Width="70px" HorizontalAlign="Center" />
                </asp:TemplateField>

                <asp:BoundField DataField="Total" HeaderText="Total" HeaderStyle-HorizontalAlign="Right">
                    <ItemStyle HorizontalAlign="Right" Width="120px" Wrap="false" />
                </asp:BoundField>

            </Columns>

            <HeaderStyle CssClass="Encabezado_gv" HorizontalAlign="Left" />
        </asp:GridView>
        <br />
        <asp:Label ID="lbl_Total" runat="server" Text="$ 0.00"></asp:Label>
        <br />
        <asp:Button ID="btn_Comprobar" runat="server" Text="Comprobar" Height="26px" Width="90px" CssClass="button" />
        <br />
        <br />
        <asp:Label ID="lbl_comentariosAd" runat="server" Text="Comentarios Adicionales"></asp:Label>
        <br />
        <asp:TextBox ID="txt_Comentarios" TextMode="MultiLine" Width="99%"
            Height="50px" runat="server" CssClass="tbx_Mayusculas"> </asp:TextBox>
        <br />
        <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Height="26px" Width="90px" CssClass="button" />

    </fieldset>

</asp:Content>
