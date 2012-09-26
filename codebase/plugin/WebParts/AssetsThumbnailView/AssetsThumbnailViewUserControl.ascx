<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetsThumbnailViewUserControl.ascx.cs"
    Inherits="OoyalaPlugin.AssetsThumbnailView.AssetsThumbnailViewUserControl" %>
<link type="text/css" rel="stylesheet" href="/_layouts/images/OoyalaPlugin/Webpart.css" />
<script type="text/javascript">
    function GetParentByTagName(selObj) {
        if (selObj.options[selObj.selectedIndex].value == 'metadata') {
            document.getElementById('<%= txtCustom.ClientID %>').style.display = '';
            document.getElementById('<%= txtCustom.ClientID %>').value = 'Enter Metadata Key';
            document.getElementById('<%= txtSearch.ClientID %>').value = 'Enter Search Text';
            document.getElementById('<%= lblResults.ClientID %>').value = "";
        }
        else {
            document.getElementById('<%= txtCustom.ClientID %>').style.display = 'none';
            document.getElementById('<%= txtSearch.ClientID %>').value = 'Enter Search Text';
            document.getElementById('<%= lblResults.ClientID %>').value = "";
            var msgLabel = document.getElementById('<%= checkEmpty.ClientID %>');
            msgLabel.innerHTML = "";

        }
    }

    function isEmptyCustomMetadata() {
        var msgLabel = document.getElementById('<%= checkEmpty.ClientID %>');
        var customText = document.getElementById('<%= txtCustom.ClientID %>').value;
        var searchText = document.getElementById('<%= txtSearch.ClientID %>').value;
        var IndexValue = document.getElementById('<%=ddlSearch.ClientID %>').selectedIndex;
        var SelectedVal = document.getElementById('<%=ddlSearch.ClientID %>').options[IndexValue].text;

        customText = trim(customText);

        if (customText.length == 0) {
            msgLabel.innerHTML = "<span style='color:red'>Metadata key should not be empty</span>";
            return false;
        }

        if ((SelectedVal == 'Custom Metadata' && customText == 'Enter Metadata Key' && searchText != 'Enter Search Text')) {
            msgLabel.innerHTML = "<span style='color:red'>Metadata key should not be empty</span>";
            return false;
        }
    }


    function trim(stringToTrim) {
        return stringToTrim.replace(/^\s+|\s+$/g, "");
    }
    function ltrim(stringToTrim) {
        return stringToTrim.replace(/^\s+/, "");
    }
    function rtrim(stringToTrim) {
        return stringToTrim.replace(/\s+$/, "");
    }

</script>
<asp:Panel ID="pnlMain" runat="server">
    <asp:TextBox ID="txtAPIKey" runat="server" Visible="false" />
    <asp:TextBox ID="txtSecretKey" runat="server" Visible="false" />
    <asp:Panel ID="pnlSearch" runat="server" Width="100%">
        <asp:Image ImageUrl="~/_layouts/images/OoyalaPlugin/Ooyala.png" runat="server" Height="25"
            ImageAlign="Bottom" ID="imgSearch" />
        <asp:DropDownList runat="server" ID="ddlSearch" onchange="javascript:GetParentByTagName(this);">
            <asp:ListItem Text="Description" Value="description" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Label" Value="labels"></asp:ListItem>
            <asp:ListItem Text="Title" Value="name"></asp:ListItem>
            <asp:ListItem Text="Status" Value="status"></asp:ListItem>
            <asp:ListItem Text="Custom Metadata" Value="metadata"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtCustom" runat="server" Width="250px" onblur="if(value=='') value = 'Enter Metadata Key'"
            onfocus="if(value=='Enter Metadata Key') value = ''" Text="Enter Metadata Key" />
        <asp:TextBox ID="txtSearch" runat="server" Width="250px" onblur="if(value=='') value = 'Enter Search Text'"
            onfocus="if(value=='Enter Search Text') value = ''" Text="Enter Search Text" />
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
            Style="font-weight: 700" ToolTip="Search" OnClientClick="javascript:return isEmptyCustomMetadata();" />
        <asp:Label ID="checkEmpty" runat="server" />
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlAssets" runat="server" Width="90%" Height="100%">
    <table class="GridViewAlternatingRowStyle" runat="server" id="tblMessage" Width="100%" visible="false">
    <tr ><td><asp:Label ID="lblDataListMessage" runat="server" /></td></tr>
    </table>
        
        <asp:DataList runat="server" ID="dtlAsssets" OnItemDataBound="dtlAsssets_ItemDataBound"
            BorderStyle="Inset" BorderWidth="1px" GridLines="Horizontal" RepeatColumns="5"
            BackColor="White" BorderColor="#E7E7FF" CellPadding="3" Width="100%">
            <AlternatingItemStyle CssClass="GridViewAlternatingRowStyle" />
            <ItemTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="litAsset" Text='<%# Eval("name").ToString() %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="imgAsset" runat="server" ImageUrl='<%# Eval("preview_image_url")%>'
                                ImageAlign="AbsMiddle" Width="150px" Height="100px" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <SelectedItemStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
        </asp:DataList>
        <br />
        <asp:Panel ID="pnlPage" runat="server" Width="100%">
            <table style="width: 100%">
                <tr>
                    <td width="40%">
                        <asp:ImageButton ID="btnPrev" AlternateText="Prev" align="left" ImageUrl="~/_layouts/images/OoyalaPlugin/Prev_32.png"
                            runat="server" OnClick="btnPrev_Click" ToolTip="Previous" />
                    </td>
                    <td align="center" width="20%">
                        <asp:Label ID="lblPageCount" runat="server" Text="Page No : 1" Style="font-weight: 700" />
                    </td>
                    <td width="40%">
                        <asp:ImageButton ID="btnNext" align="right" runat="server" AlternateText="Next" ImageUrl="~/_layouts/images/OoyalaPlugin/Next_32.png"
                            OnClick="btnNext_Click" ToolTip="Next" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <br />
</asp:Panel>
<asp:Panel ID="pnlErrorMessage" runat="server">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
