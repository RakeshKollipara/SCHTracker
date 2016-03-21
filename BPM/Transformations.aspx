<%@ Page Title="Transformation 841 Search" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Transformations.aspx.cs" Inherits="Transformations" %>

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
                modalWin.ShowURL('AchiveFilesPOPUp.aspx?type=841', 370, 470, 'Archvie Files', null);
            }

            function ShowMessage(PoNumber, TxnType, ErrorNumber, ErrorDesc) {
                var FinalMessage = ' PO Number : ' + PoNumber + '\n' + ' Transaction Type : ' + TxnType + '\n' + ' Error Details : ' + '\n' + ' Error Number : ' + ErrorNumber + '\n' + ' Error Description : ' + '\n' + ErrorDesc;
                modalWin.ShowMessage(FinalMessage, 300, 400, 'Error Descitpion');
            }

        </script>
        <p><%--<a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a>--%></p>
        <div class="form-group">
            <table>
                <tr>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label1" runat="server" Text="PO Number" />
                            <asp:TextBox ID="txtPONumber" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label4" runat="server" Text="Plant" />
                            <asp:TextBox ID="txtPlant" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label3" runat="server" Text="Control Number" />
                            <asp:TextBox ID="txtControlNumber" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label5" runat="server" CssClass="sdm-required control-label" Text="Partner" />
                            <asp:DropDownList ID="drdServiceComponentPartnerSearch" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd1">
                            <asp:Label ID="Label2" runat="server" Text="Status" />
                            <asp:DropDownList ID="drdStatus" runat="server" AutoPostBack="true" CssClass="form-control dropdown sdm-dropdown">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="Success" Value="Success" />
                                <asp:ListItem Text="Failed" Value="Failed" />
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                   
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label7" runat="server" CssClass="sdm-required control-label" Text="Date From" />
                            <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateFrom" TargetControlID="txtDateFrom" Format="MM/dd/yyyy" runat="server" />
                        </div>
                    </td>
                    <td>
                        <div class="SearchTd2">
                            <asp:Label ID="Label8" runat="server" CssClass="sdm-required control-label" Text="Date To" />
                            <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                            <ajaxToolkit:CalendarExtender CssClass="cal_Theme1" ID="CalExtDateTo" TargetControlID="txtDateTo" Format="MM/dd/yyyy" runat="server" />
                        </div>
                    </td>
                    <td></td>
                    <td class="SearchTd2" align="Right" style="vertical-align: bottom;">
                        <div class="SearchTd2">
                            <asp:Button ID="btnSearch" CssClass="btn btn-primary" runat="server" Text="Search" OnClick="btnSearch_Click" />
                        </div>
                    </td>
                    <td class="SearchTd2" align="middle" style="vertical-align: bottom;">
                        <div class="SearchTd2">
                            <asp:Button ID="btnCancel" CssClass="btn btn-primary" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:RequiredFieldValidator ID="reqDateto" ControlToValidate="txtDateTo" runat="server" CssClass="errorLabel" ErrorMessage="Please select DateTo" />
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtDateFrom" CssClass="errorLabel" runat="server" ErrorMessage="Please select DateFrom" />
                         <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="drdServiceComponentPartnerSearch" CssClass="errorLabel" runat="server" ErrorMessage="Please select Partner" />
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
                                    <asp:GridView ID="gvTransformations" runat="server" AutoGenerateColumns="False"
                                        AllowPaging="true" CssClass="mGrid" PagerStyle-CssClass="pgr" AllowSorting="true" HeaderStyle-ForeColor="White" HeaderStyle-Font-Underline="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle"
                                        AlternatingRowStyle-CssClass="alt" OnRowCommand="gvTransformations_RowCommand" OnPageIndexChanging="gvTransformations_PageIndexChanging" OnSorting="gvTransformations_Sorting" OnRowDataBound="gvTransformations_RowDataBound">
                                        <Columns>
                                            <%--<asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgDetails" runat="server" Style="cursor: hand; font-size: 12px; text-decoration: underline"
                                                        CommandArgument="<%# Container.DataItemIndex %>"
                                                        CommandName="ViewDetails" ImageUrl="~/Images/Details.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:BoundField DataField="TxnType" HeaderText="Txn Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TxnType" />
                                            <asp:BoundField DataField="CM" HeaderText="CM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="CM" />
                                            <asp:BoundField DataField="PONumber" HeaderText="PO Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="PONumber" />
                                            <%--<asp:BoundField DataField="Plant" HeaderText="CM" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="Plant" />--%>
                                            <asp:BoundField DataField="TxnDate" HeaderText="Txn Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="TxnDate" />
                                            <asp:BoundField DataField="ControlNumber" HeaderText="Control Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ControlNumber" />
                                            <asp:TemplateField HeaderText="841 Status" SortExpression="Status">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Image ID="imgStatus" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="997/824 Sent" SortExpression="FuncAck">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:Image ID="imgAckStatus" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="GRDate" HeaderText="GR Received" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />--%>
                                            <asp:TemplateField HeaderText="MDN of 997/824" SortExpression="MDN">
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
                                                    <asp:Label ID="LabelFunctionalAck" runat="server" Text='<%# Bind("FuncAck") %>' />
                                                    <asp:Label ID="lblMDNForFuncAck" runat="server" Text='<%# Bind("MDN") %>' />
                                                    <asp:Label ID="lblExtranetStatus" runat="server" Text='<%# Bind("Status") %>' />
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
                        <asp:Label ID="lblSearchError" runat="server" CssClass="errorLabel"></asp:Label>
                    </td>
                </tr>

            </table>
        </div>
    </div>
</asp:Content>

