<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APISettingsUserControl.ascx.cs"
    Inherits="OoyalaPlugin.WP_API_Settings.APISettings.APISettingsUserControl" %>
<table style="width: 55%;">
    <tr>
        <td style="width: 20%">
            <asp:Label ID="lblPartnerCode" runat="server" AssociatedControlID="txtPartnerCode"
                Text="Partner Code" Style="font-weight: 700" />
            <img id="lblPartnerCodeastricx" src="/_layouts/images/OoyalaPlugin/required-red-asterisk.gif"
                alt="" />
        </td>
        <td style="width: 50%">
            <asp:TextBox ID="txtPartnerCode" runat="server" Width="100%" />
        </td>
        <td style="width: 30%">
            <asp:RequiredFieldValidator runat="server" ID="rfvPartnerCode" Display="Dynamic"
                ControlToValidate="txtPartnerCode" ValidationGroup="APIInfoGroup" Text="Partner Code Required" />
        </td>
    </tr>
    <tr>
        <td style="width: 20%">
            <asp:Label ID="lblAPIKey" runat="server" Text="API Key" AssociatedControlID="txtAPIKey"
                Style="font-weight: 700" />
            <img id="lblAPIKeyAstricx" src="/_layouts/images/OoyalaPlugin/required-red-asterisk.gif"
                alt="" style="margin-bottom: 0px" />
        </td>
        <td style="width: 50%">
            <asp:TextBox ID="txtAPIKey" runat="server" Width="100%" />
        </td>
        <td style="width: 30%">
            <asp:RequiredFieldValidator runat="server" ID="rfvAPIKey" Display="Dynamic" ControlToValidate="txtAPIKey"
                ValidationGroup="APIInfoGroup" Text=" API Key Required" />
        </td>
    </tr>
    <tr>
        <td style="width: 20%">
            <asp:Label ID="lblSecretKey" runat="server" Text="Secret Key" AssociatedControlID="txtSecretKey"
                Style="font-weight: 700" />
            <img id="SKastricx" src="/_layouts/images/OoyalaPlugin/required-red-asterisk.gif"
                alt="" />
        </td>
        <td style="width: 50%">
            <asp:TextBox ID="txtSecretKey" runat="server" Width="100%" />
        </td>
        <td style="width: 30%">
            <asp:RequiredFieldValidator runat="server" ID="rfvSecretKey" Display="Dynamic" ControlToValidate="txtSecretKey"
                ValidationGroup="APIInfoGroup" Text="Secret Key Required" />
        </td>
    </tr>
    <tr>
        <td>
        </td>
        <td align="right">
            <asp:ImageButton ID="btnSave" runat="server" AlternateText="Save" ImageUrl="~/_layouts/images/OoyalaPlugin/Accept_32.png"
                ValidationGroup="APIInfoGroup" OnClick="btnSave_Click" Height="32px" Width="32px"
                ToolTip="Save" />
            <asp:ImageButton ID="btnReset" runat="server" AlternateText="Reset" ImageUrl="~/_layouts/images/OoyalaPlugin/reset.png"
                OnClick="btnReset_Click" Height="32px" Width="32px" ToolTip="Reset" />
        </td>
        <td>
        </td>
    </tr>
</table>
<br />
<asp:Panel ID="pnlErrorMessage" runat="server">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
