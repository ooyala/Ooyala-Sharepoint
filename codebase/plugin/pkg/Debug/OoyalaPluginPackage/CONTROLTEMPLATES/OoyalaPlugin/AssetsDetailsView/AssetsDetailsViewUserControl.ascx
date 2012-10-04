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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssetsDetailsViewUserControl.ascx.cs"
    Inherits="OoyalaPlugin.AssetsDetailsView.AssetsDetailsViewUserControl" %>
<%@ Register Src="~/_controltemplates/OoyalaPlugin.AssetsThumbnailView/Labels/LabelsUserControl.ascx"
    TagPrefix="OoyalaControl" TagName="Labels" %>
<link type="text/css" rel="stylesheet" href="/_layouts/images/OoyalaPlugin/Webpart.css" />
<script type="text/javascript">

    function GetParentByTagName(parentTagName, childElementObj) {
        var parent = childElementObj.parentNode;
        while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
            parent = parent.parentNode;
        }
        return parent;
    }

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

        if (SelectedVal == 'Custom Metadata' && customText == 'Enter Metadata Key' && searchText != 'Enter Search Text') {
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
<asp:TextBox ID="txtAPIKey" runat="server" Visible="false" />
<asp:TextBox ID="txtSecretKey" runat="server" Visible="false" />
<asp:Panel ID="pnlMain" runat="server">
    <asp:Panel runat="server" ID="pnlSearch" Width="100%">
        <asp:Image ImageUrl="~/_layouts/images/OoyalaPlugin/Ooyala.png" runat="server" ImageAlign="Bottom"
            ID="imgSearch" />
        <asp:DropDownList runat="server" ID="ddlSearch" onchange="javascript:GetParentByTagName(this);">
            <asp:ListItem Text="Description" Value="description" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Label" Value="labels"></asp:ListItem>
            <asp:ListItem Text="Title" Value="name"></asp:ListItem>
            <asp:ListItem Text="Status" Value="status"></asp:ListItem>
            <asp:ListItem Text="Custom Metadata" Value="metadata"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtCustom" runat="server" Width="250px" onblur="if(value=='') value = 'Enter Metadata Key'"
            onfocus="if(value=='Enter Metadata Key') value = ''" Text="Enter Metadata Key"
            Height="20px" />
        <asp:TextBox ID="txtSearch" runat="server" Width="250px" onblur="if(value=='') value = 'Enter Search Text'"
            onfocus="if(value=='Enter Search Text') value = ''" Text="Enter Search Text"
            Height="20px" />
        <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
            Style="font-weight: 700" ToolTip="Search" Height="22px" OnClientClick="javascript:return isEmptyCustomMetadata();" />
        <asp:Label ID="checkEmpty" runat="server" />
    </asp:Panel>
    <br />
    <asp:Panel runat="server" ID="pnlGrid" ScrollBars="Vertical" BorderColor="White"
        Height="480px" Width="100%">
        <asp:GridView runat="server" ID="dtgAssets" CellPadding="4" AllowSorting="True" GridLines="Horizontal"
            AutoGenerateColumns="False" OnRowDataBound="dtgAssets_RowDataBound" EnableModelValidation="True"
            OnRowDeleting="dtgAssets_RowDeleting" OnRowEditing="dtgAssets_RowEditing" OnRowCancelingEdit="dtgAssets_RowCancelingEdit"
            OnRowUpdating="dtgAssets_RowUpdating" CssClass="GridViewStyle">
            <EmptyDataTemplate>
                No Data Found!
            </EmptyDataTemplate>
            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
            <Columns>
                <asp:TemplateField HeaderText="DELETE" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgdelete" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure about delete this Asset?');"
                            AlternateText="Delete" ImageUrl="~/_layouts/images/OoyalaPlugin/Delete_32.png"
                            ToolTip="Delete" />
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="EDIT" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" AlternateText="Edit"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/edit_32.png" ToolTip="Edit" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:ImageButton ID="imgSave" runat="server" CommandName="Update" AlternateText="Save"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/Accept_32.png" ToolTip="Save" />
                        <asp:ImageButton ID="imgCancel" runat="server" CommandName="Cancel" AlternateText="Cancel"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/Cancel_32.png" ToolTip="Cancel" />
                    </EditItemTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("name")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        TITLE
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAssetName" runat="server" Width="100%" Text='<%# Eval("name")%>' />
                        <asp:TreeView runat="server" ID="trvAsset" ShowCheckBoxes="All" BorderStyle="Inset"
                            BorderWidth="1px" ImageSet="Simple" ShowLines="True">
                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                            <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px"
                            NodeSpacing="0px" VerticalPadding="0px" />
                            <ParentNodeStyle Font-Bold="False" />
                            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px"
                                VerticalPadding="0px" />
                        </asp:TreeView>
                    </EditItemTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        DESCRIPTION
                    </HeaderTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtAssetDesc" runat="server" TextMode="MultiLine" Width="100%" Text='<%# Eval("Description")%>' /><br />
                        <asp:GridView runat="server" ID="dtgMetadata" CellPadding="4" Width="100%" AutoGenerateColumns="False"
                            GridLines="None" EnableModelValidation="True" ForeColor="#333333">
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
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="black" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        STATUS
                    </HeaderTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblOrgFileName" runat="server" Text='<%# Eval("original_file_name")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        FILE NAME
                    </HeaderTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Image ID="imgAssetType" runat="server" ImageUrl="" />
                        <asp:Label ID="lblAssetType" runat="server" Text='<%# Eval("asset_type")%>' Visible="false" />
                        <asp:Label ID="lblEmbedCode" runat="server" Text='<%# Eval("embed_code")%>' Visible="false" />
                    </ItemTemplate>
                    <HeaderTemplate>
                        TYPE
                    </HeaderTemplate>
                    <HeaderStyle ForeColor="black" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblDuration" runat="server" Text='<%# Eval("duration")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        DURATION
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="black" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblUploaded" runat="server" Text='<%# Eval("created_at")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        CREATED ON
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="black" />
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblUpdated" runat="server" Text='<%# Eval("updated_at")%>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        UPDATED ON
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle ForeColor="black" />
                </asp:TemplateField>
            </Columns>
            <EditRowStyle VerticalAlign="Top" />
            <HeaderStyle CssClass="GridViewHeaderStyle" />
            <RowStyle CssClass="GridViewSelectedRowStyle" />
        </asp:GridView>
    </asp:Panel>
</asp:Panel>
<br />
<asp:Panel ID="pnlPage" runat="server" Width="100%" Height="35px">
    <asp:ImageButton ID="btnPrev" AlternateText="Prev" align="left" ImageUrl="~/_layouts/images/OoyalaPlugin/Prev_32.png"
        runat="server" OnClick="btnPrev_Click" ToolTip="Previous" />
    <asp:ImageButton ID="btnNext" align="right" runat="server" AlternateText="Next" ImageUrl="~/_layouts/images/OoyalaPlugin/Next_32.png"
        OnClick="btnNext_Click" ToolTip="Next" />
    <center>
        <asp:Label ID="lblPageCount" runat="server" Text="Page No : 1" Style="font-weight: 700" />
    </center>
</asp:Panel>
<br />
<asp:Panel ID="pnlErrorMessage" runat="server">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
