<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaPlayerSettingsUserControl.ascx.cs"
    Inherits="OoyalaPlugin.MediaPlayerSettings.MediaPlayerSettingsUserControl" %>
<link type="text/css" rel="stylesheet" href="/_layouts/images/OoyalaPlugin/Webpart.css" />
<asp:TextBox ID="txtAPIKey" runat="server" Visible="false" />
<asp:TextBox ID="txtSecretKey" runat="server" Visible="false" />
<div style="height: 90%;  width: 100%">
    <div style="width: 50%; text-align: right">
        <table>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="lblAdd" Text="Add Media Player" Font-Size="Small"></asp:Label>
                </td>
                <td>
                    <asp:ImageButton runat="server" AlternateText="Add New" ID="btnAdd" ImageUrl="~/_layouts/images/OoyalaPlugin/Add_32.png"
                        OnClick="AddButton_Click" ToolTip="Add" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div style="width: 50%">
        <asp:GridView runat="server" ID="playerGrid" AutoGenerateColumns="False" GridLines="Horizontal"
            OnRowDeleting="playerGrid_RowDeleting" OnRowEditing="playerGrid_RowEditing" OnRowCancelingEdit="playerGrid_RowCancelingEdit"
            OnRowUpdating="playerGrid_RowUpdating" CellPadding="3" EnableModelValidation="True"
            CssClass="GridViewStyle" OnRowDataBound="playerGrid_RowDataBound">
            <AlternatingRowStyle CssClass="GridViewAlternatingRowStyle" />
            <Columns>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" >
                    <ItemTemplate>
                        <asp:CheckBox ID="chkDefault" runat="server" Visible="false" Checked='<%#Eval("is_default") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblPlayerID" runat="server" Width="50%" Visible="false" Text='<%#Eval("id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Label ID="lblPlayerName" runat="server" Text='<%#Eval("name") %>' />
                    </ItemTemplate>
                    <HeaderTemplate>
                        PLAYER NAME</HeaderTemplate>
                    <EditItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPlayer" runat="server" Text="Player Name" />
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtplayerName" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkShowShareButton" Text="Share" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowShareDigg" Text="Digg" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowShareFacebook" Text="Facebook" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowShareEmailToFriend" Text="Email to Friend" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowShareToTwitter" Text="Twitter" runat="server" />
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkShowInfoButton" Text="Info" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowInfoExposeTitle" Text="Title" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowInfoExposeDescription" Text="Description" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowInfoExposeProvider" Text="Presented By" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowShareLinkURL" runat="server" Text="Show Share URL" />
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkReplay" runat="server" Text="Replay" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowShareGrabEmbedCode" Text="Embed" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowChannelButton" Text="Channels" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkAdCountdown" Text="Ad Countdown Message" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkShowVolumeButton" Text="Volume" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkShowBitrateButton" Text="Bitrates" runat="server" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkBufferOnPause" Text="Buffer on Pause" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkAlwaysShowScrubber" Text="Always show scrubber bar" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                    <HeaderStyle ForeColor="black" />
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="EDIT" ShowHeader="false" ItemStyle-HorizontalAlign="Left"
                    ItemStyle-Width="60px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEdit" runat="server" CommandName="Edit" AlternateText="Edit"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/Edit_32.png" ToolTip="Edit" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:ImageButton ID="imgSave" runat="server" CommandName="Update" AlternateText="Save"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/Accept_32.png" ToolTip="Save" />
                        <asp:ImageButton ID="imgCancel" runat="server" CommandName="Cancel" AlternateText="Cancel"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/Cancel_32.png" ToolTip="Cancel" />
                    </EditItemTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <Columns>
                <asp:TemplateField HeaderText="DELETE" ShowHeader="false" ItemStyle-HorizontalAlign="Left"
                    ItemStyle-Width="60px">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgdelete" runat="server" CommandName="Delete" AlternateText="Delete"
                            ImageUrl="~/_layouts/images/OoyalaPlugin/Delete_32.png" ToolTip="Delete" OnClientClick="return confirm('Are you sure about delete the Media Player?');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <HeaderStyle ForeColor="black" />
                    <ItemStyle HorizontalAlign="Right"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="GridViewHeaderStyle" />
            <RowStyle CssClass="GridViewSelectedRowStyle" />
        </asp:GridView>
    </div>
</div>
<br />
<asp:Panel ID="pnlErrorMessage" runat="server">
    <asp:Label ID="lblResults" runat="server" />
</asp:Panel>
