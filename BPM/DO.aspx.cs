using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPM.BusinessEntities;
using BPM.BusinessComponents;
using System.Configuration;
using System.Xml;
using System.IO;

public partial class DO : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string vDOList = "vDOList";
    private const string vSortDir = "sortDirection";
    private const string vSortDirLine = "sortDirectionLine";
    private const string grdPageSize = "grdPazeSize";
    LogHelper logFile = new LogHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Delivery Order Search";
        if (!IsPostBack)
        {
            lblSearchError.Text = "";
            //PopulateServiceComponentDropDown("", "", "", "PARTNERS");
            PopulateServiceComponentDropDown(drdServiceComponentPartnerSearch);
            BindRepeater();
        }
    }

    private void BindRepeater()
    {
        ViewState[vSortDir] = SortDirection.Ascending;
        int intPageSize = Utils.bindRepeater(RepGvPaging);
        ViewState[grdPageSize] = intPageSize.ToString();
        lblRec.ToolTip = intPageSize.ToString();
        Utils.SetRptDefaultPage(RepGvPaging, intPageSize);
        lblSearchError.Visible = false;
        Session[vDOList] = null;
    }

    private void PopulateServiceComponentDropDown(DropDownList drd)
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<PartnerBE> PartnerList = (GenericCollection<PartnerBE>)Session["ExtranetPartnerList"];
            drd.DataTextField = "PartnerName";
            drd.DataValueField = "PartnerName";
            drd.DataSource = PartnerList;
            drd.DataBind();
            drd.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }
    }

    //private void PopulateServiceComponentDropDown(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompType)
    //{
    //    try
    //    {
    //        DropDownBC objBC = new DropDownBC();
    //        GenericCollection<ServiceComponent> ServiceComponentList = new GenericCollection<ServiceComponent>();
    //        ServiceComponentList = objBC.GetServiceComponentList(ServiceLineID, ServiceID, ServiceOptionID, ServCompType);
    //        drdServiceComponentPartnerSearch.DataTextField = "ServiceComponentDesc";
    //        drdServiceComponentPartnerSearch.DataValueField = "ServiceComponentDesc";
    //        drdServiceComponentPartnerSearch.DataSource = ServiceComponentList;
    //        drdServiceComponentPartnerSearch.DataBind();
    //        //if (drdServiceComponentPartnerSearch != null)
    //        //{
    //        //    foreach (ListItem li in drdServiceComponentPartnerSearch.Items)
    //        //    {
    //        //        li.Attributes["title"] = li.Text;
    //        //    }
    //        //}
    //        drdServiceComponentPartnerSearch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
    //    }
    //    catch (Exception ex)
    //    {
    //        logFile.ErrorLogging(ex);
    //    }
    //}





    protected void retainDropdownToolTip(DropDownList dropdown)
    {
        foreach (ListItem _listItem in dropdown.Items)
        {
            _listItem.Attributes.Add("title", _listItem.Text);
        }
        dropdown.Attributes.Add("onmouseover", "this.title=this.options[this.selectedIndex].title");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        lblSearchError.Text = "";
        gvDO.DataSource = null;
        gvDO.DataBind();
        rowGrid.Visible = false;
        lblSearchError.Visible = false;
        BindRepeater();
        BindDeliveryOrder();
    }

    private void BindDeliveryOrder()
    {
        TransactionBC objBC = new TransactionBC();
        PurchaseOrderBC objPucBC = new PurchaseOrderBC();
        try
        {
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
            GenericCollection<DeliveryShipmentBE> listOfBE = objBC.GetDeliveryShipmentDetails("Get", TxtIDOC.Text.Trim(), txtDO.Text.Trim(),txtPO.Text.Trim(), drdStatus.SelectedValue, drdServiceComponentPartnerSearch.SelectedValue,txtPlant.Text.Trim(),txtOrderType.Text.Trim(), DateFrom, DateTo);

            foreach (DeliveryShipmentBE obj in listOfBE)
            {
                string PartnerName = objPucBC.GetPartnerName(obj.Plant);
                obj.CM = PartnerName == "" ? obj.CM : PartnerName;
            }

            Session[vDOList] = listOfBE;



            if (ViewState[grdPageSize] != null)
            {
                gvDO.PageSize = Convert.ToInt32(ViewState[grdPageSize].ToString());
            }
            else
            {
                gvDO.PageSize = 10;
            }


            lblRec.Text = Utils.GridRecDispMsg(gvDO.PageIndex, gvDO.PageSize, listOfBE.Count);
            gvDO.Visible = true;
            if (listOfBE != null && listOfBE.Count > 0)
            {

                rowPage.Visible = true;
                rowGrid.Visible = true;

                gvDO.DataSource = listOfBE;
                gvDO.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvDO.PageIndex, gvDO.PageSize, listOfBE.Count);
                gvDO.Visible = true;
                lblRec.Visible = true;
            }
            else
            {
                lblRec.Visible = false;
                rowGrid.Visible = false;
                rowPage.Visible = false;


                gvDO.Visible = false;
                gvDO.DataSource = null;
                gvDO.DataBind();
                lblSearchError.Text = "No records found.";
                lblSearchError.Visible = true;

            }


        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }

    //private GenericCollection<ShowShipHeaderBE> PrepareShowShipDetails(GenericCollection<ShowShipHeaderBE> listOfBE)
    //{
    //    TransactionBC objTranBC = new TransactionBC();
    //    PurchaseOrderBC objBC = new PurchaseOrderBC();
    //    GenericCollection<ShowShipHeaderBE> ShowShipList = new GenericCollection<ShowShipHeaderBE>();
    //    for (int i = 0; i < listOfBE.Count; i++)
    //    {
    //        ShowShipHeaderBE objBE = new ShowShipHeaderBE();
    //        objBE.SNo = i;
    //        objBE.TxnType = "3B2";
    //        objBE.TxnDate = listOfBE[i].TxnDate;
    //        //string File = listOfBE[0].ArchiveFile;
    //        XmlDocument ShowShipFile = new XmlDocument();
    //        //ShowShipFile.Load(@"D:\BPM\SampleFiles\ShowShipEGLFiles.xml");
    //        ShowShipFile.Load(listOfBE[i].ArchiveFile);
    //        objBE.ArchiveFile = listOfBE[i].ArchiveFile;
    //        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(ShowShipFile.NameTable);
    //        namespaceManager.AddNamespace("ns0", "http://MS.IT.Ops.HED.ShowShipment_V02_00_00");

    //        XmlNode xPartnerNode = ShowShipFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/ApplicationArea/Sender/ns0:LogicalId", namespaceManager);
    //        objBE.CM = xPartnerNode == null ? "" : objBC.GetPartnerName(xPartnerNode.InnerText);
    //        objBE.CM = objBE.CM == "" ? xPartnerNode.InnerText : objBE.CM;

    //        XmlNode xASTransactionID = ShowShipFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/ApplicationArea/ns0:ReferenceId", namespaceManager);
    //        objBE.ASFeedTxnID = xASTransactionID == null ? "" : xASTransactionID.InnerText;

    //        XmlNode xReceiverKey = ShowShipFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/routing/ReceiverKey", namespaceManager);
    //        objBE.ReceiverKey = xReceiverKey == null ? "" : xReceiverKey.InnerText;

    //        XmlNode xShipDate = ShowShipFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/DataArea/Shipment/Header/TransportationEvent/ns0:DateTime", namespaceManager);
    //        if (xShipDate != null)
    //        {
    //            try
    //            {
    //                objBE.ShipDate = Convert.ToDateTime(xShipDate.InnerText.Substring(0, 4) + "-" + xShipDate.InnerText.Substring(4, 2) + "-" + xShipDate.InnerText.Substring(6, 2));
    //            }
    //            catch (Exception ex)
    //            {

    //            }
    //        }

    //        XmlNodeList PartiesNodeList = ShowShipFile.SelectNodes("ns0:ShowShipment_V02_00_00/DataArea/Shipment/Header/Parties", namespaceManager);
    //        XmlNodeList TrackingReferenceNodes = ShowShipFile.SelectNodes("ns0:ShowShipment_V02_00_00/DataArea/Shipment/Header/TrackingReference", namespaceManager);
    //        XmlNodeList LineItemNodes = ShowShipFile.SelectNodes("ns0:ShowShipment_V02_00_00/DataArea/Shipment/LineItem", namespaceManager);

    //        if (LineItemNodes != null)
    //            objBE.LineCount = LineItemNodes.Count;


    //        foreach (XmlNode xnd in PartiesNodeList)
    //        {
    //            if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Originating Party")
    //            {
    //                XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
    //                objBE.OriginationParty = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
    //                objBE.OriginationParty = objBE.OriginationParty == "" ? xCMNode.InnerText : objBE.OriginationParty;
    //            }
    //            if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Selling Partner")
    //            {
    //                XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
    //                objBE.SellingPartner = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
    //                objBE.SellingPartner = objBE.SellingPartner == "" ? xCMNode.InnerText : objBE.SellingPartner;
    //            }
    //            if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Ship To")
    //            {
    //                XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
    //                objBE.SAPShipTo = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
    //                objBE.SAPShipTo = objBE.SAPShipTo == "" ? xCMNode.InnerText : objBE.SAPShipTo;
    //            }

    //            if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Freight Forwarder")
    //            {
    //                XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
    //                objBE.FMV = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
    //                objBE.FMV = objBE.FMV == "" ? xCMNode.InnerText : objBE.FMV;
    //            }
    //        }
    //        foreach (XmlNode xnd in TrackingReferenceNodes)
    //        {
    //            if (xnd.SelectSingleNode("ns0:TrackingCode", namespaceManager).InnerText == "Load Identifier")
    //            {
    //                XmlNode xCMNode = xnd.SelectSingleNode("ns0:TrackingId", namespaceManager);
    //                objBE.LoadID = xCMNode == null ? "" : xCMNode.InnerText.Substring(0, xCMNode.InnerText.Length - 2);
    //            }
    //        }
    //        GenericCollection<ShowShipLinesBE> objLinesList = new GenericCollection<ShowShipLinesBE>();
    //        int j = 0;
    //        foreach (XmlNode xnd in LineItemNodes)
    //        {
    //            ShowShipLinesBE objLineBE = new ShowShipLinesBE();
    //            XmlNodeList LineDocRefNodes = ShowShipFile.SelectNodes("/ns0:ShowShipment_V02_00_00/DataArea/Shipment/LineItem/DocumentReference", namespaceManager);
    //            foreach (XmlNode xndDoc in LineDocRefNodes)
    //            {
    //                if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Packing Slip Number")
    //                {
    //                    XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
    //                    objLineBE.PackSlipNumber = xPartnerNode == null ? "" : xCMNode.InnerText;
    //                }
    //                if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Purchase Order")
    //                {
    //                    XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
    //                    objLineBE.PONumber = xPartnerNode == null ? "" : xCMNode.InnerText;
    //                    XmlNode xLineNumber = xndDoc.SelectSingleNode("ns0:LineNumber", namespaceManager);
    //                    objLineBE.LineNumber = xLineNumber == null ? "" : xLineNumber.InnerText;
    //                }
    //            }
    //            XmlNode xSKUNode = xnd.SelectSingleNode("OrderItem/ns0:ProductIdentifier", namespaceManager);
    //            objLineBE.SKU = xSKUNode == null ? "" : xSKUNode.InnerText;
    //            XmlNode xUnitsShippedNode = xnd.SelectSingleNode("ns0:UnitsShipped", namespaceManager);
    //            objLineBE.ItemQuantity = xUnitsShippedNode == null ? "" : xUnitsShippedNode.InnerText;
    //            objLinesList.Add(j, objLineBE);
    //            j++;
    //        }
    //        objBE.ShowShipLines = objLinesList;
    //        if (objBE.LineCount == 1 && objLinesList != null)
    //            objBE.PONumber = objLinesList[0].PONumber;
    //        else
    //            objBE.PONumber = "See Details";
    //        ShowShipList.Add(i, objBE);
    //    }
    //    return ShowShipList;
    //}

    protected void gvDO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            //if (e.CommandName == "ViewDetails")
            //{
            //    ImageButton lb = (ImageButton)e.CommandSource;
            //    GridViewRow gvr = (GridViewRow)lb.NamingContainer;
            //    GridView gridview = gvr.NamingContainer as GridView;
            //    int index = Convert.ToInt32(e.CommandArgument);
            //    GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];
            //    Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
            //    GenericCollection<ShowShipHeaderBE> listOfBE = (GenericCollection<ShowShipHeaderBE>)Session[vDOList];
            //    GenericCollection<ShowShipLinesBE> lines = new GenericCollection<ShowShipLinesBE>();
            //    ShowShipHeaderBE Headerobj = new ShowShipHeaderBE();

            //    foreach (ShowShipHeaderBE obj in listOfBE)
            //    {
            //        if (obj.SNo.ToString() == lblSNo.Text)
            //        {
            //            Headerobj = obj;
            //            break;
            //        }
            //    }

            //    lines = Headerobj.ShowShipLines;
            //    trPODetails.Visible = false;
            //    Session["ShowShipLineCollection"] = lines;
            //    databind();
            //}
            if (e.CommandName == "ViewFiles")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
                Label lblTransactionID = (Label)selectedRow.FindControl("lblTransactionID");
                Label lblDONumber = (Label)selectedRow.FindControl("lblDONumber");
                GenericCollection<DeliveryShipmentBE> listOfBE = (GenericCollection<DeliveryShipmentBE>)Session[vDOList];
                DeliveryShipmentBE Headerobj = new DeliveryShipmentBE();

                foreach (DeliveryShipmentBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }

                TransactionBC objBC = new TransactionBC();
                GenericCollection<DeliveryShipmentBE> ArchiveObjects = new GenericCollection<DeliveryShipmentBE>();
                ArchiveObjects = objBC.GetDeliveryShipmentDetails("Files", lblTransactionID.Text, lblDONumber.Text, "", "", "", "", "", null, null);
                Headerobj.DOIDOCArchiveFile = ArchiveObjects[0].DOIDOCArchiveFile;
                Headerobj.ProcessShipmentArchiveFile = ArchiveObjects[0].ProcessShipmentArchiveFile;
                Session["ArchiveFiles"] = Headerobj;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage();", true);
            }
            if (e.CommandName == "ViewError")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblTransactioID = (Label)selectedRow.FindControl("lblTransactionID");
                Label lblDONumber = (Label)selectedRow.FindControl("lblDONumber");

                PurchaseOrderBC objBC = new PurchaseOrderBC();
                string Error = objBC.LoadPOErrorMessage(lblDONumber.Text, lb.ID == "imgStatus" ? "Extranet" : "Corpnet");
                string PoNumber = " Transaction ID : " + lblTransactioID.Text + " \\n Delivery Order : " + lblDONumber.Text + " \\n Error Details : \\n ";
                string errormessage = "Error Number : " + Error.Split(',')[0] + " \\n Error Description : \\n " + Error.Split(',')[1];
                //Label LabelTxnType = (Label)selectedRow.FindControl("LabelTxnType");
                string Eror = "My Error";
                //ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowMessage();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Registering", "$(document).ready(function(){ ShowMessage('" + lblTransactioID.Text + "','" + lblDONumber.Text + "','" + Error.Split(',')[0] + "','" + Error.Split(',')[1] + "'); });", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void gvDO_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblMDNForFuncAck = (Label)e.Row.FindControl("lblMDNForFuncAck");
                ImageButton imStatus = (ImageButton)e.Row.FindControl("imgStatus");
                Image imgMDNStatus = (Image)e.Row.FindControl("imgMDNRcvStatus");

                if (lblStatus.Text == "Success")
                {
                    imStatus.ImageUrl = "~/Images/Success.png";
                    imStatus.AlternateText = "Success";
                    imStatus.Enabled = false;
                }
                else if (lblStatus.Text == "Failed")
                {
                    imStatus.ImageUrl = "~/Images/Failed.png";
                    imStatus.AlternateText = "Failed";
                    imStatus.Enabled = true;
                }
                else
                {
                    imStatus.ImageUrl = "~/Images/NA.png";
                    imStatus.AlternateText = "Not Applicable";
                    imStatus.Enabled = false;
                }
                if (lblMDNForFuncAck.Text.ToLower() == "nomdn")
                {
                    imgMDNStatus.ImageUrl = "~/Images/NoMDN.png";
                    imgMDNStatus.AlternateText = "Success";
                }
                else if (lblMDNForFuncAck.Text.ToLower() == "posmdn")
                {
                    imgMDNStatus.ImageUrl = "~/Images/PosMDN.png";
                    imgMDNStatus.AlternateText = "Failed";
                }

                else if (lblMDNForFuncAck.Text.ToLower() == "negmdn")
                {
                    imgMDNStatus.ImageUrl = "~/Images/NegMDN.png";
                    imgMDNStatus.AlternateText = "Failed";
                }
                else
                {
                    imgMDNStatus.ImageUrl = "~/Images/Warning.png";
                    imgMDNStatus.AlternateText = "Not Applicable";
                }
            }
        }

        catch (Exception ex)
        {

        }
    }

    //protected void btnfirst_Click(object sender, EventArgs e)
    //{
    //    pos = 0;
    //    databind();
    //}

    //protected void btnprevious_Click(object sender, EventArgs e)
    //{
    //    pos = (int)this.ViewState["vs"];
    //    pos -= 1;
    //    this.ViewState["vs"] = pos;
    //    databind();
    //}

    //protected void btnnext_Click(object sender, EventArgs e)
    //{
    //    pos = (int)this.ViewState["vs"];
    //    pos += 1;
    //    this.ViewState["vs"] = pos;
    //    databind();
    //}

    //protected void btnlast_Click(object sender, EventArgs e)
    //{
    //    pos = (ViewState["PageCount"] != null ? Convert.ToInt32(ViewState["PageCount"]) : 1) - 1;
    //    databind();
    //}

    //private void databind()
    //{
    //    try
    //    {
    //        GenericCollection<ShowShipLinesBE> ShowShipLineCollection = Session["ShowShipLineCollection"] == null ? null : Session["ShowShipLineCollection"] as GenericCollection<ShowShipLinesBE>;
    //        if (ShowShipLineCollection != null)
    //        {
    //            adsource = new PagedDataSource();
    //            adsource.DataSource = ShowShipLineCollection;
    //            adsource.PageSize = 10;
    //            ViewState["PageCount"] = (ShowShipLineCollection.Count % 10) == 0 ? (ShowShipLineCollection.Count / 10) : (ShowShipLineCollection.Count / 10) + 1;
    //            adsource.AllowPaging = true;
    //            adsource.CurrentPageIndex = pos;
    //            btnfirst.Enabled = !adsource.IsFirstPage;
    //            btnprevious.Enabled = !adsource.IsFirstPage;
    //            btnlast.Enabled = !adsource.IsLastPage;
    //            btnnext.Enabled = !adsource.IsLastPage;

    //            gvDOLines.DataSource = adsource;
    //            gvDOLines.DataBind();
    //            divPaginationPoLine.Visible = true;
    //            gvDOLines.Visible = true;
    //            trPODetails.Visible = true;
    //        }
    //        else
    //        {
    //            trPODetails.Visible = false;
    //            gvDOLines.DataSource = null;
    //            gvDOLines.DataBind();
    //            divPaginationPoLine.Visible = false;
    //            gvDOLines.Visible = false;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblSearchError.Text = Ex.Message;
    //        lblSearchError.Visible = true;
    //        logFile.ErrorLogging(Ex);
    //    }
    //}


    protected void gvDO_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            lblSearchError.Text = string.Empty;
            GenericCollection<DeliveryShipmentBE> listofBE = (GenericCollection<DeliveryShipmentBE>)Session[vDOList];

            if (listofBE != null)
            {
                gvDO.PageIndex = e.NewPageIndex;
                gvDO.DataSource = listofBE;
                gvDO.DataBind();
            }
            else
            {
                gvDO.DataSource = null;
                gvDO.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    //protected void gvDOLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        lblSearchError.Text = string.Empty;
    //        GenericCollection<ShowShipLinesBE> listofBE = (GenericCollection<ShowShipLinesBE>)Session["ShowShipLineCollection"];

    //        if (listofBE != null)
    //        {
    //            gvDOLines.PageIndex = e.NewPageIndex;
    //            gvDOLines.DataSource = listofBE;
    //            gvDOLines.DataBind();
    //            divPaginationPoLine.Visible = true;
    //            gvDOLines.Visible = true;
    //        }
    //        else
    //        {
    //            gvDOLines.DataSource = null;
    //            gvDOLines.DataBind();
    //            divPaginationPoLine.Visible = false;
    //            gvDOLines.Visible = false;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        lblSearchError.Text = Ex.Message;
    //        lblSearchError.Visible = true;
    //        logFile.ErrorLogging(Ex);
    //    }
    //}
    //protected void gvDOLines_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    try
    //    {
    //        SortDirection dir = (ViewState[vSortDirLine] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirLine].ToString());
    //        GenericCollection<ShowShipLinesBE> list = Session[vDOList] as GenericCollection<ShowShipLinesBE>;
    //        list = Utils.GridSorting<ShowShipLinesBE>(gvDOLines, dir, e.SortExpression, list);
    //        Session["ShowShipLineCollection"] = list;
    //        ViewState[vSortDirLine] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblSearchError.Text = ex.Message;
    //        lblSearchError.Visible = true;
    //        logFile.ErrorLogging(ex);
    //    }
    //}
    protected void gvDO_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDir] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDir].ToString());
            GenericCollection<DeliveryShipmentBE> list = Session[vDOList] as GenericCollection<DeliveryShipmentBE>;
            list = Utils.GridSorting<DeliveryShipmentBE>(gvDO, dir, e.SortExpression, list);
            Session[vDOList] = list;
            ViewState[vSortDir] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void RepGvPagingItem_Bound(Object sender, RepeaterCommandEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            foreach (RepeaterItem item in RepGvPaging.Items)
            {
                LinkButton lbtSize = (LinkButton)item.FindControl("lbtnSize");
                lbtSize.Visible = true;
                Label lblSize = (Label)item.FindControl("lblPgSize");
                lblSize.Visible = false;
            }

            LinkButton lbtnSize = (LinkButton)e.Item.FindControl("lbtnSize");
            string size = lbtnSize.Text;
            Label lbl = (Label)e.Item.FindControl("lblPgSize");
            lbtnSize.Visible = false;
            lbl.Visible = true;
            ViewState[grdPageSize] = size.ToString();
            gridPaging(size);
        }
    }

    protected void gridPaging(string size)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<DeliveryShipmentBE> listofBE = (GenericCollection<DeliveryShipmentBE>)Session[vDOList];

            if (listofBE != null)
            {
                if (listofBE.Count == 0)
                {
                    lblSearchError.Text = "No records found.";
                    lblSearchError.Visible = true;
                    gvDO.DataSource = null;
                    gvDO.DataBind();
                    lblRec.Visible = false;
                }
                else
                {
                    Utils.GridPaging(gvDO, Convert.ToInt32(size), listofBE);
                    lblRec.Text = Utils.GridRecDispMsg(gvDO.PageIndex, gvDO.PageSize, listofBE.Count);
                    lblRec.Visible = true;
                }

            }
            else
            {
                gvDO.DataSource = null;
                gvDO.DataBind();
                lblRec.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            logFile.ErrorLogging(Ex);
        }
    }



    protected void drdServiceComponentPartnerSearch_PreRender(object sender, EventArgs e)
    {

    }
    protected void drdServiceComponentPartnerSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        rowGrid.Visible = false;
        lblSearchError.Text = "";
        BindRepeater();
        txtPO.Text = "";
        txtDO.Text = "";
        txtDateFrom.Text = "";
        txtDateTo.Text = "";
        TxtIDOC.Text = "";
        txtOrderType.Text = "";
        txtPlant.Text = "";
        drdStatus.SelectedIndex = 0;
        drdServiceComponentPartnerSearch.SelectedIndex = 0;
    }
}