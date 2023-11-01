<%@ Page Title="Cajeros>Capturar Fallas" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="CapturarFallas.aspx.vb" Inherits="PortalSIAC.Fallas" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="div_FiltroConsulta" style="border: solid 1px; border-color: Gray">
        <table style="width: 520px">
            <tr>
                <td style="text-align: right; width: 90px">
                    <asp:Label ID="lbl_Cajero" runat="server" Text="Cajero"></asp:Label>
                </td>

                <td colspan="3">
                    <asp:DropDownList ID="ddl_Cajero" runat="server" Height="20px" Width="400px"
                        DataTextField="Descripcion" DataValueField="Id_Cajero" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 90px">
                    <asp:Label ID="lbl_tipoCajero" runat="server" Text="Tipo Cajero"></asp:Label>
                </td>

                <td>
                    <asp:TextBox ID="tbx_tipoCajero" runat="server" ReadOnly="True" Width="80px"></asp:TextBox>
                </td>

                <td style="text-align: right; width: 220px">
                    <asp:Label ID="lbl_tiempoRespuesta" runat="server" Text="Tiempo Respuesta"></asp:Label>
                </td>

                <td>
                    <asp:TextBox ID="tbx_TiempoRespuesta" runat="server" ReadOnly="True" Width="60px"></asp:TextBox>
                </td>

            </tr>
        </table>


    </div>

    <br />

    <div id="div_pestana" style="border: solid 1px; border-color: Gray">
        <asp:TabContainer runat="server" ID="tbc_CapturaFallasCustodias" ActiveTabIndex="0"
            Width="100%" Height="680px" Style="margin-top: 7px; top: 30px">

            <asp:TabPanel runat="server" HeaderText="Falla o Custodia" ID="tab_Fallas">
                <ContentTemplate>
                    <table style="width: 610px">
                        <tr>
                            <td style="width: 175px">&nbsp;</td>
                            <td style="width: 175px">
                                <asp:RadioButton ID="rdb_Falla" runat="server" GroupName="FallaCustodia"
                                    Text="Falla" Checked="True" />
                                &nbsp;&nbsp;
                <asp:RadioButton ID="rdb_Custodia" runat="server"
                    GroupName="FallaCustodia" Text="Custodia" />
                            </td>
                            <td style="width: 110px">&nbsp;</td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label1" runat="server" Text="No. Reporte"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NumReporte" runat="server" Width="120px"></asp:TextBox>
                                &nbsp</td>
                            <td></td>
                            <td></td>

                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style5">
                                <asp:Label ID="Label2" runat="server" Text="Fecha Requerida"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_Fecha" runat="server" CssClass="calendarioAjax"
                                    Width="88px"></asp:TextBox>
                                <asp:CalendarExtender ID="tbx_Fecha_CalendarExtender" CssClass="calendarioAjax" runat="server" Enabled="True"
                                    TargetControlID="tbx_Fecha" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>

                            </td>
                            <td></td>
                            <td></td>

                        </tr>

                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label8" runat="server" Text="Fecha Alarma"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_FechaAlarma" runat="server" CssClass="calendarioAjax"
                                    Width="88px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender1" CssClass="calendarioAjax" runat="server" Enabled="True"
                                    TargetControlID="tbx_FechaAlarma" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                            <td style="width: 110px; text-align: right">
                                <asp:Label ID="Label9" runat="server" Text="Hora Alarma"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_HoraAlarma" runat="server" Width="40px">
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
                <asp:DropDownList ID="ddl_MinutosAlarma" runat="server" Width="40px">
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

                        </tr>
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label6" runat="server" Text="Hora que solicita el Banco"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_MinutosSolBanco" runat="server" Width="40px">
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
                <asp:DropDownList ID="ddl_MinutosSolg" runat="server" Width="40px">
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
                            <td style="width: 110px; text-align: right">&nbsp;</td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label10" runat="server" Text="Parte"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_ParteFalla" runat="server" AutoPostBack="True" DataTextField="Descripcion" DataValueField="Id_Parte" Width="250px">
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label11" runat="server" Text="Tipo Falla"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_TipoFalla" runat="server" AutoPostBack="True" DataTextField="Descripcion" DataValueField="Id_ParteF" Width="400px">
                                </asp:DropDownList>
                            </td>

                        </tr>
                        <tr>
                            <td></td>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                    </table>
                    <br />

                    <asp:Label ID="lbl_comentariosAd" runat="server" Text="Comentarios Adicionales"></asp:Label>
                    <br />
                    <asp:TextBox ID="txt_Comentarios" TextMode="MultiLine" Width="99%"
                        Height="50px" runat="server" CssClass="tbx_Mayusculas"></asp:TextBox>
                    <br />
                    <asp:Button ID="btn_GuardarFalla" runat="server" Height="26px" Text="Guardar" Width="90px" CssClass="button" />

                </ContentTemplate>

            </asp:TabPanel>

            <asp:TabPanel runat="server" HeaderText="Corte" ID="TabPanel1">
                <ContentTemplate>
                    <table style="width: 400px">
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label3" runat="server" Text="No. Reporte"></asp:Label>

                            </td>
                            <td>
                                <asp:TextBox ID="tbx_NumReporteCorte" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label12" runat="server" Text="Fecha Requerida"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="tbx_FechaCorte" runat="server" CssClass="calendarioAjax"
                                    Width="88px"></asp:TextBox>
                                <asp:CalendarExtender ID="CalendarExtender2" CssClass="calendarioAjax" runat="server" Enabled="True"
                                    TargetControlID="tbx_FechaCorte" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>

                            </td>
                        </tr>
                        <tr>
                            <td style="width: 175px; text-align: right">
                                <asp:Label ID="Label13" runat="server" Text="Horario"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_De" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                                    DataValueField="Hora" AutoPostBack="True">
                                </asp:DropDownList>
                                <asp:Label ID="Label4" runat="server" Text="/" Font-Bold="True"
                                    Font-Size="Medium"></asp:Label>
                                <asp:DropDownList ID="ddl_A" runat="server" DataTextField="Hora" DataTextFormatString="{0:HH}:{0:mm}"
                                    DataValueField="Hora" AutoPostBack="True">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btn_GuardarCorte" runat="server" Text="Guardar" Width="90px" Height="26px" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>

            </asp:TabPanel>

        </asp:TabContainer>

    </div>
</asp:Content>



