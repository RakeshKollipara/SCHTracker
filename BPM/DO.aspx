<%@ Page Title="Delivery Order Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DO.aspx.cs" Inherits="DO" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="jumbotron">
        <script src="Scripts/ModalPopupWindow.js" type="text/javascript"></script>
        <script>

            var modalWin = new CreateModalPopUpObject();
            modalWin.SetLoadingImagePath("Images/loading.gif");
            modalWin.SetCloseButtonImagePath("Images/remove.gif");
            //Uncomment below line to make look buttons as link
            //modalWin.SetButtonStyle("background:none;border:none;textDecoration:underline;cursor:pointer");

            function ShowNewPage() {
                modalWin.Draggable = false;
                modalWin.ShowURL('AchiveFilesPOPUp.aspx?type=DO', 420, 470, 'Archvie Files', null);
            }

            function ShowMessage(IDOCNumber, PoNumber, ErrorNumber, ErrorDesc) {
                var FinalMessage = ' IDOC Number : ' + IDOCNumber + '\n' + ' DO Number : ' + PoNumber + '\n' + ' Error Details : ' + '\n' + ' Error Number : ' + ErrorNumber + '\n' + ' Error Description : ' + '\n' + ErrorDesc;
                modalWin.ShowMessage(FinalMessage, 300, 400, 'Error Descitpion');
            }

        </script>
        <p><%--<a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a>--%></p>
        <div class="form-group">
            <table>
                <tr>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label3" runat="server" Text="IDOC Number" />
                            <asp:TextBox ID="TxtIDOC" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label1" runat="server" Text="DO Number" />
                            <asp:TextBox ID="txtDO" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label4" runat="server" Text="PO Number" />
                            <asp:TextBox ID="txtPO" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label5" runat="server" Text="Plant" />
                            <asp:TextBox ID="txtPlant" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label6" runat="server" Text="Partner" />
                            <asp:DropDownList ID="drdServiceComponentPartnerSearch" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label9" runat="server" Text="Order Type" />
                            <asp:TextBox ID="txtOrderType" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label2" runat="server" Text="DO Status" />
                            <asp:DropDownList ID="drdStatus" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Success" Value="Success" />
                                <asp:ListItem Text="Failed" Value="Failed" />
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label7" runat="server" CssClass="sdm-required control-label" Text="Date From" />
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control dropdown sdm-dropdown" AutoCompleteType="None"  />
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateFrom" TargetControlID="txtDateFrom" Format="MM/dd/yyyy" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label8" runat="server" CssClass="sdm-required control-label" Text="Date To" />
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control dropdown sdm-dropdown" AutoCompleteType="None"  />
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateTo" TargetControlID="txtDateTo" Format="MM/dd/yyyy" runat="server" />
                        </div>
                    </td>
                    <td></td>
                    <td class="SearchTd2" align="Right" style="vertical-align: bottom; padding-bottom: 8px">
                        <div class="SearchTd2">
                            <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </div>

                    </td>
                    <td align="middle" style="vertical-align: bottom; padding-bottom: 8px">
                        <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:RequiredFieldValidator ID="reqDateto" ControlToValidate="txtDateTo" runat="server" CssClass="errorLabel" ErrorMessage="Please select DateTo" />
                        <br />
                        <asp:RequiredFieldValidator ID="reqDateFrom" ControlToValidate="txtDateFrom" CssClass="errorLabel" runat="server" ErrorMessage="Please select DateFrom" />
                    </td>
                </tr>
                <tr id="rowPage" runat="server" visible="false">
                    <td style="padding-left: 20px; padding-right: 20px" colspan="6">
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
                    <td colspan="6" style="padding-left: 20px; padding-right: 20px; padding-bottom: 10px">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvDO" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                        AlternatingRowStyle-CssClass="alt" OnRowCommand="gvDO_RowCommand" OnPageIndexChanging="gvDO_PageIndexChanging" OnSorting="gvDO_Sorting" OnRowDataBound="gvDO_RowDataBound">
                                        <Columns>
                                            <%--<asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDetails" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                        CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="ViewDetails" ImageUrl="~/Images/Details.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TransactionID" />
                                            <asp:BoundField DataField="DONumber" HeaderText="Delivery Order" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="DONumber" />
                                            <asp:BoundField DataField="PONumber" HeaderText="Purchase Order" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PONumber" />
                                            <asp:BoundField DataField="TxnType" HeaderText="Txn Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TxnType" />
                                            <asp:BoundField DataField="CM" HeaderText="Partner" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="CM" />
                                            <asp:BoundField DataField="Plant" HeaderText="Plant" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="Plant" />
                                            <asp:BoundField DataField="OrderType" HeaderText="Order Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="OrderType" />
                                            <asp:BoundField DataField="TxnDate" HeaderText="Txn Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="Status" />
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgStatus" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline" CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="ViewError" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="MDN Status" SortExpression="MDNStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Image ID="imgMDNRcvStatus" runat="server" />
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
                                                    <asp:Label ID="lblTransactionID" runat="server" Text='<%# Bind("TransactionID")%>' />
                                                    <asp:Label ID="lblDONumber" runat="server" Text='<%# Bind("DONumber") %>' />
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' />
                                                    <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("PONumber") %>' />
                                                    <asp:Label ID="lblMDNForFuncAck" runat="server" Text='<%# Bind("MDNStatus") %>' />
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
                    <td colspan="6">
                        <asp:Label ID="lblSearchError" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                    </td>
                </tr>

            </table>
        </div>
    </div>
</asp:Content>

