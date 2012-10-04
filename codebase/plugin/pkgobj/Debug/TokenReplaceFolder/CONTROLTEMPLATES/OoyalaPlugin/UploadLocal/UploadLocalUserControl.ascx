<!--
Copyright (c) 2012, Ooyala, Inc.
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following
conditions are met:
     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer 
        in the documentation and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, 
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT 
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-->
<%@ Assembly Name="OoyalaPlugin, Version=1.0.0.0, Culture=neutral, PublicKeyToken=84ea112d1e541612" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UploadLocalUserControl.ascx.cs"
    Inherits="OoyalaPlugin.UploadLocal.UploadLocalUserControl" %>
<style type="text/css">
    .style1
    {
        width: 272px;
    }
</style>
<script type="text/javascript">
    function checkFileExtension(elem) {
        var filePath = elem.value;

        if (filePath.indexOf('.') == -1)
            return false;

        var validExtensions = new Array();
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

        validExtensions[0] = 'WMV';
        validExtensions[1] = 'AVI';
        validExtensions[2] = 'MOV';
        validExtensions[3] = 'MOOV';
        validExtensions[4] = 'MPG';
        validExtensions[5] = 'MPEG';
        validExtensions[6] = 'M2T';
        validExtensions[7] = 'M2V';
        validExtensions[8] = 'VOB';
        validExtensions[9] = 'FLV';
        validExtensions[10] = 'MPG4';
        validExtensions[11] = 'MKV';
        validExtensions[12] = 'ASF';
        validExtensions[13] = 'M4V';
        validExtensions[14] = 'M2P';
        validExtensions[15] = '3GP';
        validExtensions[16] = '3G2';
        validExtensions[17] = 'F4V';

        for (var i = 0; i < validExtensions.length; i++) {
            if (ext == validExtensions[i].toLowerCase())
                return true;
        }

        alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
        return false;
    }

    function defaultPlayer() {
        alert('User should not delete default player');
        return false;
    }
</script>
<asp:TextBox ID="txtAPIKey" runat="server" Visible="false" />
<asp:TextBox ID="txtSecretKey" runat="server" Visible="false" />
<asp:Panel ID="pnlMain" runat="server">
    <table>
        <tr>
            <td style="font-weight: 700">
                <asp:Label ID="lblAsset" runat="server" Text="New Media Asset" />
            </td>
            <td>
                <asp:FileUpload runat="server" EnableViewState="true" Width="100%" ID="fuAsset" ToolTip="Select a file" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: 700">
                <asp:Label runat="server" ID="lblTitle" Text="Title" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtTitle" Width="99.5%" BorderColor="Black" BorderStyle="Inset"
                    BorderWidth="1px" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: 700">
                <asp:Label runat="server" ID="lblDescription" Text="Description" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtDesc" TextMode="MultiLine" Height="66px" Width="99.5%"
                    BorderColor="Black" BorderStyle="Inset" BorderWidth="1px" />
            </td>
        </tr>
        <tr>
            <td style="font-weight: 700">
                <asp:Label runat="server" ID="lblLabels" Text="Labels" />
            </td>
            <td>
                <asp:Panel runat="server" ID="lblPanel" ScrollBars="Auto" Height="250px" Width="99.5%"
                    BorderColor="Black" BorderStyle="Inset" BorderWidth="1px" Visible="false">
                    <asp:TreeView runat="server" ID="trvAsset" ShowCheckBoxes="All" Width="100%" Height="300px">
                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px"
                            NodeSpacing="0px" VerticalPadding="0px" />
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                    </asp:TreeView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td style="font-weight: 700">
                <asp:Label runat="server" ID="lblCustomMetadata" Text="Custom Metadata" />
            </td>
            <td>
                <asp:GridView runat="server" ID="dtgMetadata" CellPadding="4" Width="100%" AutoGenerateColumns="False"
                    GridLines="None" EnableModelValidation="True" ForeColor="#333333" BorderWidth="1px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtCustomMetadataKey" runat="server" Text='<%# Eval("Key") %>' />
                            </ItemTemplate>
                            <HeaderTemplate>
                                Custom Metadata</HeaderTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:TextBox ID="txtCustomMetadataValue" runat="server" Text='<%# Eval("Value") %>' />
                            </ItemTemplate>
                            <HeaderTemplate>
                                Value</HeaderTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="font-weight: 700">
                <asp:Label ID="lblPostUploadStatus" Text="Post Upload Status" runat="server" />
            </td>
            <td class="style1">
                <asp:DropDownList ID="ddlPostUploadStatus" runat="server">
                    <asp:ListItem Selected="True" Value="live">Live</asp:ListItem>
                    <asp:ListItem Value="paused">Pending Approval</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" style="font-weight: 700">
                <%--                <asp:ImageButton ID="imgBtnUpload" AlternateText="Upload" ImageUrl="~/_layouts/images/OoyalaPlugin/UploadIcon.gif"
                    runat="server" OnClick="UploadButton_Click" ToolTip="Upload"/> --%>
                <%--               <asp:ImageButton ID="imgBtnReset" AlternateText="Reset" ImageUrl="~/_layouts/images/OoyalaPlugin/Cancel_32.png"
                    runat="server" OnClick="ResetButton_Click"  ToolTip="Reset" />--%>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnUpload" runat="server" OnClick="UploadButton_Click" ToolTip="Upload"
                                Text="Upload" />
                        </td>
                        <td>
                            <asp:Button ID="btnReset" runat="server" OnClick="ResetButton_Click" ToolTip="Reset"
                                Text="Reset" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<asp:Panel ID="pnlErrorMessage" runat="server">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
