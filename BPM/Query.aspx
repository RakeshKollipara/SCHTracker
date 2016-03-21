<%@ Page Title="Query" ValidateRequest="false" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="Query" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="jumbotron">
        <div class="form-group">
            <table style="width: 100%">
                <tr>
                    <td align="right" style="width: 20%">
                        <asp:Label ID="Label1" runat="server" Text="Query Type" />

                    </td>
                    <td align="middle" style="width: 60%">
                        <div style="margin: 10px">
                            <asp:DropDownList ID="drdQuery" runat="server" AutoPostBack="true" Width="80%" CssClass="form-control dropdown sdm-dropdown" OnSelectedIndexChanged="drdQuery_SelectedIndexChanged" />
                        </div>
                    </td>
                    <td align="left" style="width: 20%">

                        <asp:CheckBox ID="chkCeva" CssClass="chkCEVA" runat="server" AutoPostBack="true" />
                        <asp:Label ID="lblToggleButton" Text="Is CEVA" CssClass="form-control dropdown sdm-dropdown" runat="server"
                            ToolTip="is CEVA" Width="39%" />

                    </td>
                </tr>
                <tr>
                    <td style="width: 100%" colspan="3">
                        <asp:TextBox ID="txtQuery" runat="server" TextMode="MultiLine" CssClass="textBig" /></td>
                </tr>
                <tr>
                    <td align="right" colspan="3">
                        <asp:Button ID="btnExecuteQuery" runat="server" Text="Execute" OnClick="btnExecuteQuery_Click" /></td>
                </tr>
                <tr id="rowGrid" runat="server" visible="false">
                    <td colspan="3">
                        <div style="overflow-x: scroll; width: 1028px">
                            <asp:GridView ID="gvResults" runat="server"
                                AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="true" OnPageIndexChanging="gvResults_PageIndexChanging" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblError" Visible="false" Text="You are not Authorised to perform following operations over the BAM database." runat="server" CssClass="errorLabel" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>


