<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AchiveFilesPOPUp.aspx.cs" Inherits="AchiveFilesPOPUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="jumbotron">
                <p><%--<a href="http://www.asp.net" class="btn btn-primary btn-lg">Learn more &raquo;</a>--%></p>
                <div class="form-group" id="divPO" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label3" runat="server" CssClass="sdm-required control-label" Text="PO Number : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtPONumber" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label1" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtCM" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label2" runat="server" CssClass="sdm-required control-label" Text="TransactionID : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtTransactionID" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label4" runat="server" CssClass="sdm-required control-label" Text="PO Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtPoDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label5" runat="server" CssClass="sdm-required control-label" Text="Is ICOE Partner" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtISICOE" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label6" runat="server" CssClass="sdm-required control-label" Text="Corpnet Status" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtCorpStatus" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label7" runat="server" CssClass="sdm-required control-label" Text="Extranet Status" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtExtStatus" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label8" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trV02" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label9" runat="server" CssClass="sdm-required control-label" Text="V02 File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkVO2File" CausesValidation="false" CommandArgument="V02" OnClick="lnkAttachments_Click" runat="server" Text="V02File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trOagis" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label10" runat="server" CssClass="sdm-required control-label" Text="Oagis File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkOagisFile" CausesValidation="false" CommandArgument="Oagis" OnClick="lnkAttachments_Click" runat="server" Text="Oagis File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trBOD" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label11" runat="server" CssClass="sdm-required control-label" Text="Confirm BOD File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkConfirmBODFile" CausesValidation="false" CommandArgument="BOD" OnClick="lnkAttachments_Click" runat="server" Text="BOD File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="lblSearchError" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="divGR" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label12" runat="server" CssClass="sdm-required control-label" Text="PO Number : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtGRPONumber" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label13" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtGRCM" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label14" runat="server" CssClass="sdm-required control-label" Text="Control NUmber : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtGRControlNumber" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label15" runat="server" CssClass="sdm-required control-label" Text="GR Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtGRDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <%--                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label16" runat="server" CssClass="sdm-required control-label" Text="Is ICOE Partner" />
                                </div>
                                </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtGRISICOE" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>  --%>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label17" runat="server" CssClass="sdm-required control-label" Text="Load ID" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtGRLoadID" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label19" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trV02GR" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label20" runat="server" CssClass="sdm-required control-label" Text="Inventory File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkGRV02File" CausesValidation="false" CommandArgument="V02GR" OnClick="lnkAttachments_Click" runat="server" Text="V02 File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trEDIGR" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label54" runat="server" CssClass="sdm-required control-label" Text="EDI XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkEDIXML" CausesValidation="false" CommandArgument="EDIXML" OnClick="lnkAttachments_Click" runat="server" Text="EDI XML" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trAckGR" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label59" runat="server" CssClass="sdm-required control-label" Text="Ack XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkAckXML" CausesValidation="false" CommandArgument="AckXML" OnClick="lnkAttachments_Click" runat="server" Text="ACK XML" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label23" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="divASN" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label18" runat="server" CssClass="sdm-required control-label" Text="PO Number : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtASNPONumber" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label21" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtASNCM" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label22" runat="server" CssClass="sdm-required control-label" Text="AS Feed Name : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtASNFeedID" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label24" runat="server" CssClass="sdm-required control-label" Text="ASN Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtASNDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label26" runat="server" CssClass="sdm-required control-label" Text="Load ID" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtASNLoadID" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label27" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trV02ASN" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label28" runat="server" CssClass="sdm-required control-label" Text="Inventory File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton1" CausesValidation="false" CommandArgument="V02ASN" OnClick="lnkAttachments_Click" runat="server" Text="V02 File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label29" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="div841" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label16" runat="server" CssClass="sdm-required control-label" Text="Control Number : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txt841ControlNumber" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label171" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txt841Partner" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label30" runat="server" CssClass="sdm-required control-label" Text="841 Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txt841TxnDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label32" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="tr841" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label33" runat="server" CssClass="sdm-required control-label" Text="Inventory File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton2" CausesValidation="false" CommandArgument="EDI841" OnClick="lnkAttachments_Click" runat="server" Text="EDI 841 File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="tr997" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label25" runat="server" CssClass="sdm-required control-label" Text="Inventory File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton3" CausesValidation="false" CommandArgument="EDI997" OnClick="lnkAttachments_Click" runat="server" Text="997 File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label34" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="divShowShip" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label41" runat="server" CssClass="sdm-required control-label" Text="Delivery Order : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSSDO" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label31" runat="server" CssClass="sdm-required control-label" Text="Transaction ID : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSSTxnID" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label35" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSSCM" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label36" runat="server" CssClass="sdm-required control-label" Text="Txn Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSSTxnDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label37" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trShowShip" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label38" runat="server" CssClass="sdm-required control-label" Text="Show Shipment File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton4" CausesValidation="false" CommandArgument="ShowShipment" OnClick="lnkAttachments_Click" runat="server" Text="Show Shipment File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trDESADV" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label39" runat="server" CssClass="sdm-required control-label" Text="DESADV XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton5" CausesValidation="false" CommandArgument="desadv" OnClick="lnkAttachments_Click" runat="server" Text="DESADV XML File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label40" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="divDO" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label42" runat="server" CssClass="sdm-required control-label" Text="Delivery Order : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDONumber" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label50" runat="server" CssClass="sdm-required control-label" Text="PO Number : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOPONumber" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label43" runat="server" CssClass="sdm-required control-label" Text="Transaction ID : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOTxnID" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label44" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOCM" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label52" runat="server" CssClass="sdm-required control-label" Text="Plant" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOPlant" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label51" runat="server" CssClass="sdm-required control-label" Text="Order Type" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOOrderType" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label45" runat="server" CssClass="sdm-required control-label" Text="Txn Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOTxnDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label53" runat="server" CssClass="sdm-required control-label" Text="Status" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtDOStatus" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label46" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trDOIDOC" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label47" runat="server" CssClass="sdm-required control-label" Text="IDOC XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkDOIDOC" CausesValidation="false" CommandArgument="DODIDOC" OnClick="lnkAttachments_Click" runat="server" Text="IDOC XML" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trPShipXML" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label48" runat="server" CssClass="sdm-required control-label" Text="Process Shipment File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="lnkDOProcessShipmentXML" CausesValidation="false" CommandArgument="DOProcShip" OnClick="lnkAttachments_Click" runat="server" Text="Process Shipment XML" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label49" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="divShowShip945" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label411" runat="server" CssClass="sdm-required control-label" Text="Delivery Order : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945DO" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label311" runat="server" CssClass="sdm-required control-label" Text="Transaction ID : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945TxnID" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label351" runat="server" CssClass="sdm-required control-label" Text="Partner : " />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945CM" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label361" runat="server" CssClass="sdm-required control-label" Text="Txn Date" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945TxnDate" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label541" runat="server" CssClass="sdm-required control-label" Text="Status" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945Status" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label55" runat="server" CssClass="sdm-required control-label" Text="Order Type" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945OrderType" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label56" runat="server" CssClass="sdm-required control-label" Text="Plant" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:TextBox ID="txtSS945Plant" runat="server" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label371" runat="server" CssClass="sdm-required control-label" Text="Archive Files" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trShowShip945" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label381" runat="server" CssClass="sdm-required control-label" Text="Show Shipment File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton41" CausesValidation="false" CommandArgument="ShowShipment945" OnClick="lnkAttachments_Click" runat="server" Text="Show Shipment File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trSSDESADV" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label392" runat="server" CssClass="sdm-required control-label" Text="DESADV XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton51" CausesValidation="false" CommandArgument="desadv945" OnClick="lnkAttachments_Click" runat="server" Text="DESADV XML File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trSSEDI945" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label57" runat="server" CssClass="sdm-required control-label" Text="EDI XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton6" CausesValidation="false" CommandArgument="edi" OnClick="lnkAttachments_Click" runat="server" Text="EDI XML File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr id="trSSAperak" runat="server">
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="Label58" runat="server" CssClass="sdm-required control-label" Text="Ack XML File" />
                                </div>
                            </td>
                            <td>
                                <div class="SearchTd1">
                                    <asp:LinkButton ID="LinkButton7" CausesValidation="false" CommandArgument="Aperak" OnClick="lnkAttachments_Click" runat="server" Text="Aperak File" CssClass="form-control dropdown sdm-dropdown" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:Label ID="Label401" runat="server" Visible="false" CssClass="errorLabel"></asp:Label>
                            </td>
                        </tr>

                    </table>
                </div>
                <div class="form-group" id="divError" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="SearchTd1">
                                    <asp:Label ID="lblErrorMessage" runat="server" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>


            </div>
        </div>
    </form>
</body>
</html>
