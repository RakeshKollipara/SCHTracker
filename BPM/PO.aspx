<%@ Page Title="PO Search" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="PO.aspx.cs" Inherits="PO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/ModalPopupWindow.js" type="text/javascript"></script>
    <script>

        var modalWin = new CreateModalPopUpObject();
        modalWin.SetLoadingImagePath("Images/loading.gif");
        modalWin.SetCloseButtonImagePath("Images/remove.gif");
        //Uncomment below line to make look buttons as link
        //modalWin.SetButtonStyle("background:none;border:none;textDecoration:underline;cursor:pointer");

        function ShowNewPage() {
            modalWin.Draggable = false;
            modalWin.ShowURL('AchiveFilesPOPUp.aspx?type=PO', 370, 470, 'Archvie Files', null);
        }

        function ShowMessage(PoNumber, TxnType, ErrorNumber, ErrorDesc) {
            var FinalMessage = ' PO Number : ' + PoNumber + '\n' + ' Transaction Type : ' + TxnType + '\n' + ' Error Details : ' + '\n' + ' Error Number : ' + ErrorNumber + '\n' + ' Error Description : ' + '\n' + ErrorDesc;
            modalWin.ShowMessage(FinalMessage, 300, 400, 'Error Descitpion');
        }

    </script>
    <div class="jumbotron">
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
                            <asp:Label ID="Label2" runat="server" Text="ICOEorExtranet" />
                            <asp:DropDownList ID="drdISOEorExtranet" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="ICOE" Value="ICOE" />
                                <asp:ListItem Text="Extranet" Value="Extranet" />
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label4" runat="server" Text="Transaction Type" />
                            <asp:DropDownList ID="drdTxnType" runat="server" CssClass="form-control dropdown sdm-dropdown">
                                <asp:ListItem Text="850,Process Purchase Order" Value="Process Purchase Order" />
                                <asp:ListItem Text="860,Change Purchase Order" Value="Change Purchase Order" />
                                <asp:ListItem Text="3A4,Process Purchase Order" Value="Process Purchase Order" />
                                <asp:ListItem Text="3A8,Change Purchase Order" Value="Change Purchase Order" />
                            </asp:DropDownList>
                            <
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label1" runat="server" Text="Status" />
                            <asp:DropDownList ID="drdPOStatus" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Success" Value="Success" />
                                <asp:ListItem Text="Failed" Value="Failed" />
                                <asp:ListItem Text="ExtranetFailed" Value="ExtranetFailed" />
                                <%--   <asp:ListItem Text="PosConfirmBOD" Value="PosConfirmBOD" />
                                <asp:ListItem Text="NegConfirmBOD" Value="NegConfirmBOD" />
                                <asp:ListItem Text="NoConfirmBOD" Value="NoConfirmBOD" />--%>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label5" runat="server" CssClass="sdm-required control-label" Text="Partner" />
                            <asp:DropDownList ID="drdServiceComponentPartnerSearch" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label7" runat="server" CssClass="sdm-required control-label" Text="Date From" />
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control dropdown sdm-dropdown" AutoCompleteType="None" />
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateFrom" TargetControlID="txtDateFrom" Format="MM/dd/yyyy" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label8" runat="server" CssClass="sdm-required control-label" Text="Date To" />
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control dropdown sdm-dropdown" AutoCompleteType="None" />
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateTo" TargetControlID="txtDateTo" Format="MM/dd/yyyy" runat="server" />
                        </div>
                    </td>
                    <td></td>
                    <td align="Middle" style="vertical-align: bottom; padding-bottom: 8px">
                        <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" CausesValidation="false" OnClick="btnSearch_Click" />

                    </td>
                    <td align="Left" style="vertical-align: bottom; padding-bottom: 8px">
                        <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:RequiredFieldValidator ID="reqDateto" ControlToValidate="txtDateTo" runat="server" CssClass="errorLabel" ErrorMessage="Please select DateTo" />
                        <br />
                        <asp:RequiredFieldValidator ID="reqDateFrom" ControlToValidate="txtDateFrom" CssClass="errorLabel" runat="server" ErrorMessage="Please select DateFrom" />
                        <br />
                        <asp:RequiredFieldValidator ID="Reqpartner" ControlToValidate="drdServiceComponentPartnerSearch" CssClass="errorLabel" runat="server" ErrorMessage="Please select Partner" />

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
                                    <asp:GridView ID="gvPurchaseOrder" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                        AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvPurchaseOrder_PageIndexChanging" OnSorting="gvPurchaseOrder_Sorting" OnRowCommand="gvPurchaseOrder_RowCommand" OnRowDataBound="gvPurchaseOrder_RowDataBound">
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
                                            <asp:BoundField DataField="TxnType" HeaderText="Transaction" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TxnType" />
                                            <asp:BoundField DataField="CM" HeaderText="CM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="CM" />
                                            <asp:BoundField DataField="PODate" HeaderText="Txn Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PODate" />
                                            <asp:BoundField DataField="isICOEPartner" HeaderText="ICOE Partner" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="isICOEPartner" />
                                            <asp:TemplateField HeaderText="Corp Status" SortExpression="CorpnetStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgCorpStatus" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                        CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="ViewError" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Extranet Status" SortExpression="ExtranetStatus">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgExtStatus" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                        CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="ViewError" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO Confirm BOD" SortExpression="POCBODRcvd">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Image ID="imgCBODStatus" runat="server" />
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
                                                    <asp:Label ID="lblCorpnetStatus" runat="server" Text='<%# Bind("CorpnetStatus")%>' />
                                                    <asp:Label ID="lblExtStatus" runat="server" Text='<%# Bind("ExtranetStatus") %>' />
                                                    <asp:Label ID="LabelTxnType" runat="server" Text='<%# Bind("TxnType") %>' />
                                                    <asp:Label ID="LabelPOCBODRcvd" runat="server" Text='<%# Bind("POCBODRcvd") %>' />
                                                    <asp:Label ID="lblPoNumber" runat="server" Text='<%# Bind("PONumber") %>' />
                                                    <asp:Label ID="lblICOE" runat="server" Text='<%# Bind("isICOEPartner") %>' />
                                                    <asp:Label ID="lblTxnDate" runat="server" Text='<%# Bind("PODate") %>' />
                                                    <asp:Label ID="lblPartner" runat="server" Text='<%# Bind("CM") %>' />
                                                    <asp:Label ID="lblTransactioID" runat="server" Text='<%# Bind("ReferenceID") %>' />
                                                    <asp:Label ID="lblMessageType" runat="server" Text='<%# Bind("MessageType") %>' />
                                                    <asp:Label ID="lblArchivePath" runat="server" Text='<%# Bind("MessageArchivePath") %>' />
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
                    <td style="padding-left: 20px; padding-right: 20px" colspan="5">
                        <table style="width: 100%">
                            <tr>
                                <td id="tdHeaderDetails" runat="server" style="vertical-align: top; padding-top: 10px; padding-left: 10px">
                                    <table>
                                        <tr>
                                            <td colspan="4">
                                                <asp:Label ID="lblHeaderDetails" runat="server" CssClass="btnHeader" Width="100%" Text="Header Details" /><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblSAPVendorNumberDisp" runat="server" Text="SAP Vendor" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblSAPVendorNumber" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCMDisp" runat="server" Text="CM" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblCM" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSAPPOTypeDisp" runat="server" Text="PO Type" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblSAPPOType" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPOTotalPriceDisp" runat="server" Text="Total Price" Font-Bold="true" />
                                                <br />
                                                <br />
                                            </td>

                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblPOTotalPrice" runat="server" Text="" />
                                                <br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="background-color: #808080; color: white">
                                                <asp:Label ID="Label13Disp" runat="server" Text="SAP Ship To Details" /><br />
                                            </td>
                                        </tr>
                                        <tr runat="server" visible="false">
                                            <td>
                                                <asp:Label ID="lblShipToNameDisp" runat="server" Text="Name" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToName" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblShipToPhoneDisp" runat="server" Text="Phone" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToPhone" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblShipToPartnernameDisp" runat="server" Text="Partner Name" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToPartnername" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblShipToAddressLine1Disp" runat="server" Text="Address" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToAddressLine1" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblShipToCityDisp" runat="server" Text="City" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToCity" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblShipToCountryDisp" runat="server" Text="Country" Font-Bold="true" /></td>
                                            <td><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToCountry" runat="server" Text="" /></td>
                                            <td>
                                                <asp:Label ID="lblShipToStateDisp" runat="server" Text="State" Font-Bold="true" /></td>
                                            <td><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToState" runat="server" Text="" /></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblShipToZIPDisp" runat="server" Text="ZIP" Font-Bold="true" /></td>
                                            <td colspan="3"><span style="font-weight: bold">:</span>
                                                <asp:Label ID="lblShipToZIP" runat="server" Text="" /></td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; padding-top: 10px; padding-left: 10px">
                                    <span class="btnHeader" style="width: 100%">PO Line Details</span><br />
                                    <br />
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvPurchaseOrderLine" runat="server" AutoGenerateColumns="False"
                                                AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true"
                                                HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-VerticalAlign="Middle" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvPurchaseOrderLine_PageIndexChanging"
                                                OnSorting="gvPurchaseOrderLine_Sorting">
                                                <Columns>
                                                    <asp:BoundField DataField="POLineNumber" HeaderText="Line Number" SortExpression="POLineNumber" />
                                                    <asp:BoundField DataField="DeliveryDate" HeaderText="Delivery Date" SortExpression="DeliveryDate" />
                                                    <asp:BoundField DataField="ShipDate" HeaderText="Ship Date" SortExpression="ShipDate" />
                                                    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price" />
                                                    <asp:BoundField DataField="Material" HeaderText="Material" SortExpression="Material" />
                                                    <asp:BoundField DataField="SerialNumber" HeaderText="Serial Number" SortExpression="Serial Number" />
                                                    <asp:BoundField DataField="OrderQuantity" HeaderText="Order Quantity" SortExpression="OrderQuantity" />
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
                                <td style="vertical-align: top; padding-top: 10px; padding-left: 10px">
                                    <span id="Label6" class="btnHeader" style="width: 100%">PO History Details</span><br />
                                    <br />
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvPOHistory" runat="server" AutoGenerateColumns="False"
                                                AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true"
                                                HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center"
                                                HeaderStyle-VerticalAlign="Middle" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="gvPOHistory_PageIndexChanging"
                                                OnSorting="gvPOHistory_Sorting">
                                                <Columns>
                                                    <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" SortExpression="TransactionDate" />
                                                    <asp:BoundField DataField="TransactionTypeID" HeaderText="Transaction Type" SortExpression="TransactionTypeID" />
                                                    <asp:BoundField DataField="TransactionTypeDesc" HeaderText="Txn Description" SortExpression="TransactionTypeDesc" />
                                                </Columns>
                                            </asp:GridView>
                                            <div class="pagination1" runat="server" visible="false" id="divPaginationPOHistory">
                                                <asp:Button ID="btnfirstHistory" runat="server" Font-Bold="true" Text="<<" Height="25px"
                                                    Width="43px" OnClick="btnfirstHistory_Click" /><asp:Button ID="btnPreviousHistory" runat="server"
                                                        Font-Bold="true" Text="<" Height="25px" Width="43px" OnClick="btnPreviousHistory_Click" /><asp:Button
                                                            ID="btnNextHistory" runat="server" Font-Bold="true" Text=">" Height="25px" Width="43px"
                                                            OnClick="btnNextHistory_Click" /><asp:Button ID="btnlastHistory" runat="server" Font-Bold="true"
                                                                Text=">>" Height="25px" Width="43px" OnClick="btnlastHistory_Click" />
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
</asp:Content>
