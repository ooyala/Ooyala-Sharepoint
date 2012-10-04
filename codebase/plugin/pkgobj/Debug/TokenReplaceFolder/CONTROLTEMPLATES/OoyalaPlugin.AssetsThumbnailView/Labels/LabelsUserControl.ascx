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
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LabelsUserControl.ascx.cs"
    Inherits="OoyalaPlugin.AssetsThumbnailView.Labels.LabelsUserControl" %>
<script type="text/javascript">
    function IsTreeChecked() {
        var TreeView = document.getElementById('<%= trvView.ClientID %>');
        var checkboxs = TreeView.getElementsByTagName("input");
        var msgLabel = document.getElementById('<%= lblResults.ClientID %>');

        for (i = 0; i < checkboxs.length; i++) {
            if (checkboxs[i].type == "checkbox" && checkboxs[i].checked) {
                return true;
            }
        }

        msgLabel.innerHTML = "<span style='color:red'>No checked items were found for Delete!.</span>";
        return false;
    }

    function fnValidateDelete() {
        if (IsTreeChecked()) {
            var answer = confirm('Are you sure about delete checked Label?');
            if (!answer) {
                return false;
            }
        }
    }

    function IsTreeCheckedEdit() {
        var TreeView = document.getElementById('<%= trvView.ClientID %>');
        var checkboxs = TreeView.getElementsByTagName("input");
        var msgLabel = document.getElementById('<%= lblResults.ClientID %>');
        var count = 0;
        for (i = 0; i < checkboxs.length; i++) {
            if (checkboxs[i].type == "checkbox" && checkboxs[i].checked) {
                //return true;
                count = count + 1;
            }
        }

        if ((count == 0) || (count > 1)) {
            msgLabel.innerHTML = "<span style='color:red'>User should select only one Label for Edit!.</span>";
            return false;
        }
        else {
            msgLabel.innerHTML = '';
            return true;
        }
    }

    function IsTreeCheckedAdd() {
        var TreeView = document.getElementById('<%= trvView.ClientID %>');
        var checkboxs = TreeView.getElementsByTagName("input");
        var msgLabel = document.getElementById('<%= lblResults.ClientID %>');
        var count = 0;
        for (i = 0; i < checkboxs.length; i++) {
            if (checkboxs[i].type == "checkbox" && checkboxs[i].checked) {
                //return true;
                count = count + 1;
            }
        }

        if ((count > 1)) {
            msgLabel.innerHTML = "<span style='color:red'>User should select only one Label for Add!.</span>";
            return false;
        }
        else {
            msgLabel.innerHTML = '';
            return true;
        }
    }

</script>
<asp:TextBox ID="txtAPIKey" runat="server" Visible="false" />
<asp:TextBox ID="txtSecretKey" runat="server" Visible="false" />
<asp:Panel runat="server" ID="pnlTree" ScrollBars="Auto">
    <table style="width: 40%; border: solid 1px black;">
        <tr>
            <td style="width: 80%">
                <asp:TreeView ID="trvView" runat="server" ImageSet="Simple" ShowCheckBoxes="All"
                    Width="309px">
                    <HoverNodeStyle BorderStyle="None" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px"
                        NodeSpacing="0px" VerticalPadding="0px" />
                    <SelectedNodeStyle BorderStyle="None" />
                    <ParentNodeStyle Font-Bold="False" />
                </asp:TreeView>
            </td>
            <td style="width: 20%; vertical-align: top;" align="right">
                <asp:ImageButton ID="imgAdd" runat="server" AlternateText="Add New" ImageUrl="~/_layouts/images/OoyalaPlugin/Add_32.png"
                    OnClick="AddButton_Click" ToolTip="Add" OnClientClick="javascript:return IsTreeCheckedAdd();" />
                <asp:ImageButton ID="imgEdit" runat="server" AlternateText="Edit" ImageUrl="~/_layouts/images/OoyalaPlugin/Edit_32.png"
                    OnClick="EditButton_Click" ToolTip="Edit" OnClientClick="javascript:return IsTreeCheckedEdit();" />
                <asp:ImageButton ID="imgDelete" runat="server" AlternateText="Delete" ImageUrl="~/_layouts/images/OoyalaPlugin/Delete_32.png"
                    OnClick="DeleteButton_Click" OnClientClick="javascript:return fnValidateDelete();"
                    ToolTip="Delete" />
            </td>
        </tr>
        <tr>
            <td style="width: 80%">
                <asp:Panel ID="pnlSave" runat="server" Visible="false" Width="100%">
                    <asp:TextBox ID="txtlblID" runat="server" Visible="false" />
                    <asp:TextBox ID="txtlblName" runat="server" Text="New Label" Width="230px" />
                </asp:Panel>
            </td>
            <td align="right" style="width: 20%">
                <asp:ImageButton ID="imgSave" runat="server" AlternateText="Save" ImageUrl="~/_layouts/images/OoyalaPlugin/Accept_32.png"
                    OnClick="SaveButton_Click" ToolTip="Save" />
                <asp:ImageButton ID="imgCancel" runat="server" AlternateText="Cancel" ImageUrl="~/_layouts/images/OoyalaPlugin/Cancel_32.png"
                    OnClick="CancelButton_Click" ToolTip="Reset" />
            </td>
        </tr>
        </tr>
    </table>
</asp:Panel>
<br />
<asp:Panel ID="pnlErrorMessage" runat="server" Width="50%">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
