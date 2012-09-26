<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
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
