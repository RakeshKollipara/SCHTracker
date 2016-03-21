<%@ Page Title="GR Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GR.aspx.cs" Inherits="GR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script src="Scripts/ModalPopupWindow.js" type="text/javascript"></script>
    <script>

        var modalWin = new CreateModalPopUpObject();
        modalWin.SetLoadingImagePath("Images/loading.gif");
        modalWin.SetCloseButtonImagePath("Images/remove.gif");
        //Uncomment below line to make look buttons as link
        //modalWin.SetButtonStyle("background:none;border:none;textDecoration:underline;cursor:pointer");

        function ShowNewPage(Type) {
            modalWin.Draggable = false;
            modalWin.ShowURL('AchiveFilesPOPUp.aspx?type=' + Type, 370, 470, 'Details', null);
        }

        function ShowMessage(TransactionID, DONumber, ErrorNumber, ErrorDesc) {
            var FinalMessage = ' TransactionID : ' + TransactionID + '\n' + ' DO Number : ' + DONumber + '\n' + ' Error Details : ' + '\n' + ' Error Number : ' + ErrorNumber + '\n' + ' Error Description : ' + '\n' + ErrorDesc;
            modalWin.ShowMessage(FinalMessage, 300, 500, 'Error Descitpion');
        }

    </script>
    <style type="text/css">
        .fancy-green .ajax__tab_header {
            background: url(Images/green_bg_Tab.gif) repeat-x;
            cursor: pointer;
        }

        .fancy-green .ajax__tab_hover .ajax__tab_outer, .fancy-green .ajax__tab_active .ajax__tab_outer {
            background: url(Images/green_left_Tab.gif) no-repeat left top;
        }

        .fancy-green .ajax__tab_hover .ajax__tab_inner, .fancy-green .ajax__tab_active .ajax__tab_inner {
            background: url(Images/green_right_Tab.gif) no-repeat right top;
        }

        .fancy .ajax__tab_header {
            font-size: 13px;
            font-weight: bold;
            color: #000;
            font-family: sans-serif;
        }

            .fancy .ajax__tab_active .ajax__tab_outer, .fancy .ajax__tab_header .ajax__tab_outer, .fancy .ajax__tab_hover .ajax__tab_outer {
                height: 39px;
            }

            .fancy .ajax__tab_active .ajax__tab_inner, .fancy .ajax__tab_header .ajax__tab_inner, .fancy .ajax__tab_hover .ajax__tab_inner {
                height: 39px;
                margin-left: 16px; /* offset the width of the left image */
            }

            .fancy .ajax__tab_active .ajax__tab_tab, .fancy .ajax__tab_hover .ajax__tab_tab, .fancy .ajax__tab_header .ajax__tab_tab {
                margin: 16px 16px 0px 0px;
            }

        .fancy .ajax__tab_hover .ajax__tab_tab, .fancy .ajax__tab_active .ajax__tab_tab {
            color: #fff;
        }

        .fancy .ajax__tab_body {
            font-family: Arial;
            font-size: 10pt;
            border-top: 0;
            border: 1px solid #808080;
            padding: 8px;
            background-color: #ffffff;
        }
    </style>

    <cc1:TabContainer ID="TabContainer1" CssClass="fancy fancy-green" runat="server" ActiveTabIndex="1" AutoPostBack="True" OnActiveTabChanged="TabContainer1_ActiveTabChanged">
        <cc1:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
            <HeaderTemplate>Extranet Details</HeaderTemplate>
            <ContentTemplate>
                <asp:ValidationSummary ID="valExtranet" runat="server" ValidationGroup="Extranet" DisplayMode="List" />
                <div class="jumbotron">
                    <p><%--<a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a>--%></p>
                    <div class="form-group">
                        <table>
                            <tr>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label3" runat="server" Text="PO Number" />
                                        <asp:TextBox ID="txtPONumber" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label9" runat="server" Text="Load ID" />
                                        <asp:TextBox ID="txtLoadID" runat="server" CssClass="form-control dropdown sdm-dropdown" />                                        
                                    </div>
                                </td>

                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label5" runat="server" CssClass="sdm-required control-label" Text="Partner" />
                                        <asp:DropDownList ID="drdServiceComponentPartnerSearch" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label7" runat="server" CssClass="sdm-required control-label" Text="Date From" />
                                        <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                        <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateFrom" TargetControlID="txtDateFrom" Format="MM/dd/yyyy" runat="server" />
                                    </div>
                                </td>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label8" runat="server" CssClass="sdm-required control-label" Text="Date To" />
                                        <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                        <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateTo" TargetControlID="txtDateTo" Format="MM/dd/yyyy" runat="server" />
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <%--<td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label1" runat="server" CssClass="sdm-required control-label" Text="GR Status" />
                            <asp:DropDownList ID="drdGRStatus" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Success" Value="Success" />
                                <asp:ListItem Text="Failed" Value="Failed" />
                            </asp:DropDownList>
                        </div>
                    </td>--%>

                                <td>
                                    <%--<div class="SearchTd2">
                                                <asp:Label ID="Label2" runat="server" CssClass="sdm-required control-label" Text="Transaction Type" />
                                                <asp:DropDownList ID="drdTxnType" runat="server" CssClass="form-control dropdown sdm-dropdown">
                                                    <asp:ListItem Text="" Value="" />
                                                    <asp:ListItem Text="4B2" Value="4B2" />
                                                    <asp:ListItem Text="861" Value="861" />
                                                </asp:DropDownList>
                                            </div>--%>
                                    <div class="SearchTd2">
                                        <asp:Label ID="Label2" runat="server" Text="Control Number" />
                                        <asp:TextBox ID="txtControlNumber" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>
                                <td></td>
                                <td></td>
                                <td align="Middle" style="vertical-align: bottom; padding-bottom: 8px">
                                    <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />

                                </td>
                                <td align="Left" style="vertical-align: bottom; padding-bottom: 8px">
                                    <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Extranet" ControlToValidate="txtDateFrom" CssClass="errorLabel" runat="server" ErrorMessage="Please select DateFrom" />
                                    <asp:RequiredFieldValidator ID="reqDateto" ValidationGroup="Extranet" ControlToValidate="txtDateTo" runat="server" CssClass="errorLabel" ErrorMessage="Please select DateTo" />
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="drdTxnType" runat="server" CssClass="errorLabel" ErrorMessage="Please select Txn Type" />--%>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Extranet" ControlToValidate="drdServiceComponentPartnerSearch" runat="server" CssClass="errorLabel" ErrorMessage="Please select Partner" />
                                </td>
                            </tr>
                            <tr id="rowPage" runat="server" visible="false">
                                <td style="padding-left: 20px; padding-right: 20px" colspan="5">
                                    <table id="tblpagesize" runat="server" width="100%">
                                        <tr>
                                            <td class="displ">
                                                <asp:Label ID="lblRec" runat="server" EnableTheming="false" Text="" />
                                            </td>
                                            <td class="rt-align">Results per page&nbsp;
                                                        <asp:Repeater ID="RepGvPaging" runat="server" OnItemCommand="RepGvPagingItem_Bound">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbtnSize" runat="server" Text='<%# Bind("PageSize") %>'></asp:LinkButton>
                                                                <asp:Label ID="lblPgSize" runat="server" EnableTheming="false" Text='<%# Bind("PageSize") %>'
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="rowGrid" runat="server">
                                <td colspan="5" style="padding-left: 20px; padding-right: 20px; padding-bottom: 10px">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvGR" runat="server" AutoGenerateColumns="False"
                                                    AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                                    AlternatingRowStyle-CssClass="alt" OnRowDataBound="gvGR_RowDataBound" OnRowCommand="gvGR_RowCommand" OnPageIndexChanging="gvGR_PageIndexChanging" OnSorting="gvGR_Sorting">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDetails" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="ViewDetails" ImageUrl="~/Images/Details.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PONumber" />
                                                        <asp:BoundField DataField="LoadID" HeaderText="Load ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="LoadID" />
                                                        <asp:BoundField DataField="CM" HeaderText="CM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="CM" />
                                                        <asp:BoundField DataField="TxnDate" HeaderText="Posting Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TxnDate" />
                                                        <asp:BoundField DataField="ControlNumber" HeaderText="Control Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ControlNumber" />
                                                        <asp:TemplateField HeaderText="GR Status" SortExpression="Status">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgStatus" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="ViewError" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ack 997 Status" SortExpression="Functional997Ack">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="img997Status" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Ack 824 Status" SortExpression="Functional824Ack">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="img824Status" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="MDN Status" SortExpression="MDNforAck">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgMDNStatus" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Files">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgFiles" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="ViewFiles" ImageUrl="~/Images/Files.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSNo" runat="server" Text='<%# Bind("SNo")%>' />
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' />
                                                                <asp:Label ID="lbl997FuncAck" runat="server" Text='<%# Bind("Functional997Ack") %>' />
                                                                <asp:Label ID="lbl824FuncAck" runat="server" Text='<%# Bind("Functional824Ack") %>' />
                                                                <asp:Label ID="lblMDN" runat="server" Text='<%# Bind("MDNforAck") %>' />
                                                                <asp:Label ID="lblPartner" runat="server" Text='<%# Bind("CM") %>' />
                                                                <asp:Label ID="lblTransactionID" runat="server" Text='<%# Bind("ControlNumber") %>' />
                                                                <asp:Label ID="lblTxnDate" runat="server" Text='<%# Bind("TxnDate") %>' />
                                                                <%--<asp:Label ID="" runat="server" Text='<%# Bind("GRStatus") %>' />
                                                                        <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("PONumber") %>' />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblSearchError" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trPODetails" style="margin-top: 25px" runat="server" visible="false">
                                <td style="padding-left: 20px; padding-right: 20px" colspan="4">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="vertical-align: top; padding-top: 10px; padding-left: 10px">
                                                <span class="btnHeader" style="width: 100%">GR Line Details</span><br />
                                                <br />
                                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvGRLines" runat="server" AutoGenerateColumns="False"
                                                            AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true"
                                                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-VerticalAlign="Middle" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvGRLines_PageIndexChanging"
                                                            OnSorting="gvGRLines_Sorting">
                                                            <Columns>
                                                                <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PONumber" />
                                                                <asp:BoundField DataField="LineNumber" HeaderText="Line Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="LineNumber" />
                                                                <asp:BoundField DataField="SKU" HeaderText="SKU" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="SKU" />
                                                                <asp:BoundField DataField="ItemQuantity" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ItemQuantity" />
                                                                <asp:BoundField DataField="RefDocument" HeaderText="Refernce Document" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="RefDocument" />
                                                                <asp:BoundField DataField="LoadID" HeaderText="Load ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="LoadID" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <div class="pagination1" runat="server" visible="false" id="divPaginationPoLine">
                                                            <asp:Button ID="btnfirst" runat="server" Font-Bold="true" Text="<<" Height="25px"
                                                                Width="43px" OnClick="btnfirst_Click" /><asp:Button ID="btnprevious" runat="server"
                                                                    Font-Bold="true" Text="<" Height="25px" Width="43px" OnClick="btnprevious_Click" /><asp:Button
                                                                        ID="btnnext" runat="server" Font-Bold="true" Text=">" Height="25px" Width="43px"
                                                                        OnClick="btnnext_Click" /><asp:Button ID="btnlast" runat="server" Font-Bold="true"
                                                                            Text=">>" Height="25px" Width="43px" OnClick="btnlast_Click" />
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </cc1:TabPanel>
        <cc1:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
            <HeaderTemplate>Corpnet Details</HeaderTemplate>
            <ContentTemplate>
                <asp:ValidationSummary ID="valCorp" runat="server" ValidationGroup="Corpnet" DisplayMode="List" />
                 <div class="jumbotron">
                    <p></p>
                    <div class="form-group">
                        <table>
                            <tr>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label1" runat="server" Text="PO Number" />
                                        <asp:TextBox ID="txtPoCorp" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label4" runat="server" Text="Load ID" />
                                        <asp:TextBox ID="txtLoadCorp" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>

                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label6" runat="server" CssClass="sdm-required control-label" Text="Partner" />
                                        <asp:DropDownList ID="drdpartnerCorp" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label10" runat="server" CssClass="sdm-required control-label" Text="Date From" />
                                        <asp:TextBox ID="txtDateFromCorp" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                        <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalendarExtender1" TargetControlID="txtDateFromCorp" Format="MM/dd/yyyy" runat="server" BehaviorID="_content_CalendarExtender1" />
                                    </div>
                                </td>
                                <td>
                                    <div class="SearchTd1">
                                        <asp:Label ID="Label11" runat="server" CssClass="sdm-required control-label" Text="Date To" />
                                        <asp:TextBox ID="txtDateToCorp" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                        <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalendarExtender2" TargetControlID="txtDateToCorp" Format="MM/dd/yyyy" runat="server" BehaviorID="_content_CalendarExtender2" />
                                    </div>
                                </td>
                            </tr>
                            <tr>

                                <td>
                                    <div class="SearchTd2">
                                        <asp:Label ID="Label12" runat="server" Text="Control Number" />
                                        <asp:TextBox ID="txtControlNumCorp" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                    </div>
                                </td>
                                <td></td>
                                <td></td>
                                <td align="Middle" style="vertical-align: bottom; padding-bottom: 8px">
                                    <asp:Button ID="btnSearchCorp" CssClass="btn btn-primary" runat="server"  CausesValidation="False" Text="Search" OnClick="btnSearchCorp_Click" />

                                </td>
                                <td align="Left" style="vertical-align: bottom; padding-bottom: 8px">
                                    <asp:Button ID="btnClearCorp" CssClass="btn btn-primary" runat="server" Text="Cancel" CausesValidation="False" OnClick="btnCancelCorp_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:RequiredFieldValidator ValidationGroup="Corpnet" ID="RequiredFieldValidator2" ControlToValidate="txtDateFromCorp" CssClass="errorLabel" runat="server" ErrorMessage="Please select DateFrom" />
                                    <asp:RequiredFieldValidator ValidationGroup="Corpnet" ID="RequiredFieldValidator4" ControlToValidate="txtDateToCorp" runat="server" CssClass="errorLabel" ErrorMessage="Please select DateTo" />
                                    <asp:RequiredFieldValidator ValidationGroup="Corpnet" ID="RequiredFieldValidator5" ControlToValidate="drdpartnerCorp" runat="server" CssClass="errorLabel" ErrorMessage="Please select Partner" />
                                </td>
                            </tr>
                            <tr id="rowPageCorp" runat="server" visible="False">
                                <td style="padding-left: 20px; padding-right: 20px" colspan="5" runat="server">
                                    <table id="Table1" runat="server" width="100%">
                                        <tr runat="server">
                                            <td class="displ" runat="server">
                                                <asp:Label ID="lblRecCorp" runat="server" EnableTheming="False" />
                                            </td>
                                            <td class="rt-align" runat="server">Results per page&nbsp;
                                                        <asp:Repeater ID="RepGvPagingCorp" runat="server" OnItemCommand="RepGvPagingItem_Bound">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbtnSize" runat="server" Text='<%# Bind("PageSize") %>'></asp:LinkButton>
                                                                <asp:Label ID="lblPgSize" runat="server" EnableTheming="false" Text='<%# Bind("PageSize") %>'
                                                                    Visible="false" />
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="rowGridCorp" runat="server">
                                <td colspan="5" style="padding-left: 20px; padding-right: 20px; padding-bottom: 10px" runat="server">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <asp:GridView ID="gvGRCorp" runat="server" AutoGenerateColumns="False"
                                                    AllowPaging="True" CssClass="mGrid" AllowSorting="True" OnRowDataBound="gvGRCorp_RowDataBound" OnRowCommand="gvGRCorp_RowCommand" OnPageIndexChanging="gvGRCorp_PageIndexChanging" OnSorting="gvGRCorp_Sorting">
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgDetails" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="ViewDetails" ImageUrl="~/Images/Details.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="PONumber" HeaderText="PO Number" SortExpression="PONumber" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="LoadID" HeaderText="Load ID" SortExpression="LoadID" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="CM" HeaderText="CM" SortExpression="CM" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="TxnDate" HeaderText="Posting Date" SortExpression="TxnDate" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ControlNumber" HeaderText="Control Number" SortExpression="ControlNumber" >
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="GR Status" SortExpression="Status">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="imgStatus" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline" CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="ViewError" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Files">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ImgFiles" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                                    CommandArgument="<%# Container.DataItemIndex %>"
                                                                    CommandName="ViewFiles" ImageUrl="~/Images/Files.png" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField Visible="False">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSNo" runat="server" Text='<%# Bind("SNo")%>' />
                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' />
                                                                <asp:Label ID="lblPartner" runat="server" Text='<%# Bind("CM") %>' />
                                                                <asp:Label ID="lblTransactionID" runat="server" Text='<%# Bind("PONumber") %>' />
                                                                <asp:Label ID="lblRefID" runat="server" Text='<%# Bind("RefID") %>' />
                                                                <asp:Label ID="lblTxnDate" runat="server" Text='<%# Bind("TxnDate") %>' />
                                                                <%--<asp:Label ID="" runat="server" Text='<%# Bind("GRStatus") %>' />
                                                                        <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("PONumber") %>' />--%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle Font-Underline="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                    <PagerStyle CssClass="pgr" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5">
                                    <asp:Label ID="lblSearchErrorCorp" runat="server" Visible="False" CssClass="errorLabel"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trGRDetails" style="margin-top: 25px" runat="server" visible="False">
                                <td style="padding-left: 20px; padding-right: 20px" colspan="4" runat="server">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="vertical-align: top; padding-top: 10px; padding-left: 10px">
                                                <span class="btnHeader" style="width: 100%">GR Line Details</span><br />
                                                <br />
                                                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvGRLinesCorp" runat="server" AutoGenerateColumns="False"
                                                            AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true"
                                                            HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center"
                                                            HeaderStyle-VerticalAlign="Middle" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvGRLinesCorp_PageIndexChanging"
                                                            OnSorting="gvGRLinesCorp_Sorting">
                                                            <Columns>
                                                                <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PONumber" />
                                                                <asp:BoundField DataField="LineNumber" HeaderText="Line Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="LineNumber" />
                                                                <asp:BoundField DataField="SKU" HeaderText="SKU" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="SKU" />
                                                                <asp:BoundField DataField="ItemQuantity" HeaderText="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ItemQuantity" />
                                                                <asp:BoundField DataField="RefDocument" HeaderText="Refernce Document" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="RefDocument" />
                                                                <asp:BoundField DataField="LoadID" HeaderText="Load ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="LoadID" />
                                                            </Columns>
                                                        </asp:GridView>
                                                        <div class="pagination1" runat="server" visible="false" id="divPaginationPoLineCorp">
                                                            <asp:Button ID="btnfirstCorp" runat="server" Font-Bold="true" Text="<<" Height="25px"
                                                                Width="43px" OnClick="btnfirstCorp_Click" /><asp:Button ID="btnpreviousCorp" runat="server"
                                                                    Font-Bold="true" Text="<" Height="25px" Width="43px" OnClick="btnpreviousCorp_Click" /><asp:Button
                                                                        ID="btnnextCorp" runat="server" Font-Bold="true" Text=">" Height="25px" Width="43px"
                                                                        OnClick="btnnextCorp_Click" /><asp:Button ID="btnlastCorp" runat="server" Font-Bold="true"
                                                                            Text=">>" Height="25px" Width="43px" OnClick="btnlastCorp_Click" />
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </ContentTemplate>
        </cc1:TabPanel>
    </cc1:TabContainer>

</asp:Content>

