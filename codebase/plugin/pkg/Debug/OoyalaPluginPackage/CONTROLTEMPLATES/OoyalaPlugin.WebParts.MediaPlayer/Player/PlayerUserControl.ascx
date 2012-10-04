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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlayerUserControl.ascx.cs"
    Inherits="OoyalaPlugin.WebParts.MediaPlayer.Player.PlayerUserControl" %>
<asp:TextBox ID="txtAPIKey" runat="server" Visible="false" />
<asp:TextBox ID="txtSecretKey" runat="server" Visible="false" />
<asp:Panel runat="server" ID="pnlPlayer" ScrollBars="Auto">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblMediaAssets" Text="Media Assets" runat="server" Width="80px"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlMediaAssets" Width="200px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lblMediaPlayers" Text="Media Players" runat="server" Width="80px"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlMediaPlayers" Width="200px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:Button runat="server" ID="btnPlay" OnClick="btnPlay_Click" Text="Load" Width="75px" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Literal runat="server" ID="litPlayer" />
</asp:Panel>
<br />
<asp:Panel ID="pnlErrorMessage" runat="server">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
