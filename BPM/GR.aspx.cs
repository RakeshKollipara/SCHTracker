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


public partial class GR : System.Web.UI.Page
{

    int pos;
    int posCorp;
    PagedDataSource adsource;
    PagedDataSource adsourceCorp;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string vGRList = "vGRList";
    private const string vSortDir = "sortDirection";
    private const string vSortDirLine = "sortDirectionLine";
    private const string grdPageSize = "grdPazeSize";
    private const string vGRListCorp = "vGRList";
    private const string vSortDirCorp = "sortDirection";
    private const string vSortDirLineCorp = "sortDirectionLine";
    private const string grdPageSizeCorp = "grdPazeSize";
    LogHelper logFile = new LogHelper();
    protected void Page_Load(object sender, EventArgs e)
    {

        Page.Title = "GR Search";
        if (!IsPostBack)
        {
            TabContainer1_ActiveTabChanged(TabContainer1, null);
            TabContainer1.ActiveTabIndex = 0;
            lblSearchError.Text = "";
            SetPartnerNames();
            PopulateServiceComponentDropDown(drdServiceComponentPartnerSearch);
            BindRepeater(vSortDir, RepGvPaging,grdPageSize,vGRList,lblSearchError);
        }
    }

    private void SetPartnerNames()
    {
        if (Cache["PartnerList"] == null)
        {
            TransactionBC objBC = new TransactionBC();
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            PartnerList = objBC.GetPartnerList();
            Cache.Insert("PartnerList", PartnerList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["PartnerList"] = PartnerList;
        }
        else
        {
            Session["PartnerList"] = (GenericCollection<PartnerBE>)Cache["PartnerList"];
        }
        if (Cache["ExtranetPartnerList"] == null)
        {
            TransactionBC objBC = new TransactionBC();
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            PartnerList = objBC.GetExtraPartnerList();
            Cache.Insert("ExtranetPartnerList", PartnerList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["ExtranetPartnerList"] = PartnerList;
        }
        else
        {
            Session["ExtranetPartnerList"] = (GenericCollection<PartnerBE>)Cache["ExtranetPartnerList"];
        }

        if (Cache["CorpnetPartnerList"] == null)
        {
            TransactionBC objBC = new TransactionBC();
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            PartnerList = objBC.GetCorpPartnerList();
            Cache.Insert("CorpnetPartnerList", PartnerList, null, DateTime.Now.AddDays(30), TimeSpan.Zero);
            Session["CorpnetPartnerList"] = PartnerList;
        }
        else
        {
            Session["CorpnetPartnerList"] = (GenericCollection<PartnerBE>)Cache["CorpnetPartnerList"];
        }
    }

    protected void TabContainer1_ActiveTabChanged(object sender, EventArgs e)
    {

        try
        {
            if (TabContainer1.ActiveTabIndex == 0)
            {

            }

            if (TabContainer1.ActiveTabIndex == 1)
            {
                PopulateServiceComponentDropDown(drdpartnerCorp);
                BindRepeater(vSortDirCorp, RepGvPagingCorp, grdPageSizeCorp, vGRListCorp, lblSearchErrorCorp);
            }

        }
        catch
        {
            throw;
        }
        finally
        {

        }

    }

    private void BindRepeater(string SrtDirection,Repeater rptr,string GridPageSize,string ListName,Label lbl)
    {
        ViewState[SrtDirection] = SortDirection.Ascending;
        int intPageSize = Utils.bindRepeater(rptr);
        ViewState[GridPageSize] = intPageSize.ToString();
        lblRec.ToolTip = intPageSize.ToString();
        Utils.SetRptDefaultPage(rptr, intPageSize);
        lbl.Visible = false;
        Session[ListName] = null;
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
            drdServiceComponentPartnerSearch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }
    }

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
        trPODetails.Visible = false;
        gvGR.DataSource = null;
        gvGR.DataBind();
        rowGrid.Visible = false;
        lblSearchError.Visible = false;
        BindRepeater(vSortDir,RepGvPaging,grdPageSize,vGRList,lblSearchError);
        BindPurchaseOrder();
    }

    protected void btnSearchCorp_Click(object sender, EventArgs e)
    {
        lblSearchErrorCorp.Text = "";
        trGRDetails.Visible = false;
        gvGRCorp.DataSource = null;
        gvGRCorp.DataBind();
        rowGridCorp.Visible = false;
        lblSearchErrorCorp.Visible = false;
        BindRepeater(vSortDirCorp, RepGvPagingCorp, grdPageSizeCorp, vGRListCorp, lblSearchErrorCorp);
        BindPurchaseOrderCorp();
    }

    private void BindPurchaseOrderCorp()
    {
        TransactionBC objBC = new TransactionBC();
        try
        {
            trGRDetails.Visible = false;
            DateTime? DateFrom = txtDateFromCorp.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFromCorp.Text);
            DateTime? DateTo = txtDateToCorp.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateToCorp.Text);
            TimeSpan? duration = null;

            // Assign values to a and b...

            if (DateFrom.HasValue && DateTo.HasValue)
            {
                duration = DateTo.Value - DateFrom.Value;
            }

            double days = duration.GetValueOrDefault().TotalDays;

            //if (days > 15)
            //{
            //    lblSearchError.Text = "Please select 15 Days difference between dates as GR has high volume of data.";
            //    lblSearchError.Visible = true;
            //    return;
            //}
            PurchaseOrderBC objPOBC = new PurchaseOrderBC();
            GenericCollection<GRListBE> listOfBE = objBC.GetGRCorpnetList("List", DateFrom, DateTo, txtPONumber.Text.Trim(), txtLoadID.Text.Trim(), drdServiceComponentPartnerSearch.SelectedValue, txtControlNumber.Text.Trim(), "",null);
            foreach (GRListBE obj in listOfBE)
            {
                obj.CM = objPOBC.GetPartnerName(obj.CM);
            }

            Session[vGRListCorp] = listOfBE;



            if (ViewState[grdPageSizeCorp] != null)
            {
                gvGRCorp.PageSize = Convert.ToInt32(ViewState[grdPageSizeCorp].ToString());
            }
            else
            {
                gvGRCorp.PageSize = 10;
            }


            lblRecCorp.Text = Utils.GridRecDispMsg(gvGRCorp.PageIndex, gvGRCorp.PageSize, listOfBE.Count);
            gvGRCorp.Visible = true;
            if (listOfBE != null && listOfBE.Count > 0)
            {

                rowPageCorp.Visible = true;
                rowGridCorp.Visible = true;

                gvGRCorp.DataSource = listOfBE;
                gvGRCorp.DataBind();
                lblRecCorp.Text = Utils.GridRecDispMsg(gvGRCorp.PageIndex, gvGRCorp.PageSize, listOfBE.Count);
                gvGRCorp.Visible = true;
                lblRecCorp.Visible = true;
            }
            else
            {
                lblRecCorp.Visible = false;
                rowGridCorp.Visible = false;
                rowPageCorp.Visible = false;


                gvGRCorp.Visible = false;
                gvGRCorp.DataSource = null;
                gvGRCorp.DataBind();
                lblSearchErrorCorp.Text = "No records found.";
                lblSearchErrorCorp.Visible = true;

            }


        }
        catch (Exception ex)
        {
            lblSearchErrorCorp.Text = ex.Message;
            lblSearchErrorCorp.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }

    private void BindPurchaseOrder()
    {
        TransactionBC objBC = new TransactionBC();
        try
        {
            trPODetails.Visible = false;
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
            TimeSpan? duration = null;

            // Assign values to a and b...

            if (DateFrom.HasValue && DateTo.HasValue)
            {
                duration = DateTo.Value - DateFrom.Value;
            }

            double days = duration.GetValueOrDefault().TotalDays;

            //if (days > 15)
            //{
            //    lblSearchError.Text = "Please select 15 Days difference between dates as GR has high volume of data.";
            //    lblSearchError.Visible = true;
            //    return;
            //}
            PurchaseOrderBC objPOBC = new PurchaseOrderBC();
            GenericCollection<GRListBE> listOfBE = objBC.GetGRExtranetList("List", DateFrom, DateTo, txtPONumber.Text.Trim(), txtLoadID.Text.Trim(), drdServiceComponentPartnerSearch.SelectedValue, txtControlNumber.Text.Trim(), null);
            foreach(GRListBE obj in listOfBE)
            {
                obj.CM = objPOBC.GetPartnerName(obj.CM);
            }

            Session[vGRList] = listOfBE;



            if (ViewState[grdPageSize] != null)
            {
                gvGR.PageSize = Convert.ToInt32(ViewState[grdPageSize].ToString());
            }
            else
            {
                gvGR.PageSize = 10;
            }


            lblRec.Text = Utils.GridRecDispMsg(gvGR.PageIndex, gvGR.PageSize, listOfBE.Count);
            gvGR.Visible = true;
            if (listOfBE != null && listOfBE.Count > 0)
            {

                rowPage.Visible = true;
                rowGrid.Visible = true;

                gvGR.DataSource = listOfBE;
                gvGR.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvGR.PageIndex, gvGR.PageSize, listOfBE.Count);
                gvGR.Visible = true;
                lblRec.Visible = true;
            }
            else
            {
                lblRec.Visible = false;
                rowGrid.Visible = false;
                rowPage.Visible = false;


                gvGR.Visible = false;
                gvGR.DataSource = null;
                gvGR.DataBind();
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

    private GRHeaderBE PrepareGRList(string FileName)
    {

        TransactionBC objTranBC = new TransactionBC();
        PurchaseOrderBC objBC = new PurchaseOrderBC();

        GRHeaderBE objBE = new GRHeaderBE();

        //string File = listOfBE[0].ArchiveFile;
        XmlDocument GRFile = new XmlDocument();
        //GRFile.Load(@"D:\BPM\SampleFiles\GREGLFiles.xml");
        if (File.Exists(FileName))
            GRFile.Load(FileName);

        objBE.ArchiveFile = FileName;
        XmlNamespaceManager namespaceManager = new XmlNamespaceManager(GRFile.NameTable);
        namespaceManager.AddNamespace("ns0", "http://MS.IT.Ops.CM.RcvInventoryMovement_V01_00_00");

        XmlNode xPartnerNode = GRFile.SelectSingleNode("ns0:RcvInventoryMovement_V01_00_00/ApplicationArea/Sender/ns0:LogicalId", namespaceManager);
        objBE.CM = xPartnerNode == null ? "" : objBC.GetPartnerName(xPartnerNode.InnerText);
        objBE.CM = objBE.CM == "" ? xPartnerNode.InnerText : objBE.CM;

        XmlNode xASTransactionID = GRFile.SelectSingleNode("ns0:RcvInventoryMovement_V01_00_00/ApplicationArea/ns0:ReferenceId", namespaceManager);
        objBE.FeedTxnID = xASTransactionID == null ? "" : xASTransactionID.InnerText;

        XmlNode xControlNumber = GRFile.SelectSingleNode("ns0:RcvInventoryMovement_V01_00_00/routing/ControlNumber", namespaceManager);
        objBE.ControlNumber = xControlNumber == null ? "" : xControlNumber.InnerText;

        XmlNode xTxnType = GRFile.SelectSingleNode("ns0:RcvInventoryMovement_V01_00_00/routing/TransactionType", namespaceManager);
        objBE.TxnType = xTxnType == null ? "" : xTxnType.InnerText;

        XmlNode xShipDate = GRFile.SelectSingleNode("ns0:RcvInventoryMovement_V01_00_00/DataArea/InventoryMovement/Header/ns0:TransactionDateTime", namespaceManager);
        //if (xShipDate != null)
        //{
        //    try
        //    {
        //        objBE.TxnDate = Convert.ToDateTime(xShipDate.InnerText.Substring(0, 4) + "-" + xShipDate.InnerText.Substring(4, 2) + "-" + xShipDate.InnerText.Substring(6, 2));
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        XmlNodeList PartiesNodeList = GRFile.SelectNodes("ns0:RcvInventoryMovement_V01_00_00/DataArea/InventoryMovement/Header/Parties", namespaceManager);
        XmlNodeList LineItemNodes = GRFile.SelectNodes("ns0:RcvInventoryMovement_V01_00_00/DataArea/InventoryMovement/LineItem", namespaceManager);

        if (LineItemNodes != null)
            objBE.LineCount = LineItemNodes.Count;


        foreach (XmlNode xnd in PartiesNodeList)
        {
            if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Received From")
            {
                XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                objBE.ReceivedFrom = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
                objBE.ReceivedFrom = objBE.ReceivedFrom == "" ? xCMNode.InnerText : objBE.ReceivedFrom;
            }
            if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Received By")
            {
                XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                objBE.ReceivedBy = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
                objBE.ReceivedBy = objBE.ReceivedBy == "" ? xCMNode.InnerText : objBE.ReceivedBy;
            }
        }
        GenericCollection<GRLinesBE> objLinesList = new GenericCollection<GRLinesBE>();
        int j = 0;
        foreach (XmlNode xnd in LineItemNodes)
        {
            GRLinesBE objLineBE = new GRLinesBE();
            XmlNodeList LineDocRefNodes = GRFile.SelectNodes("/ns0:RcvInventoryMovement_V01_00_00/DataArea/InventoryMovement/LineItem/DocumentReference", namespaceManager);
            foreach (XmlNode xndDoc in LineDocRefNodes)
            {
                if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Reference Document")
                {
                    XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
                    objLineBE.RefDocument = xPartnerNode == null ? "" : xCMNode.InnerText;
                }
                if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Purchase Order")
                {
                    XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
                    objLineBE.PONumber = xPartnerNode == null ? "" : xCMNode.InnerText;
                    XmlNode xLineNumber = xndDoc.SelectSingleNode("ns0:LineNumber", namespaceManager);
                    objLineBE.LineNumber = xLineNumber == null ? "" : xLineNumber.InnerText;
                }

                if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Load Id")
                {
                    XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
                    objLineBE.LoadID = xPartnerNode == null ? "" : xCMNode.InnerText;
                }
            }
            XmlNode xSKUNode = xnd.SelectSingleNode("ProductId/ns0:ProductIdentifier", namespaceManager);
            objLineBE.SKU = xSKUNode == null ? "" : xSKUNode.InnerText;
            XmlNode xUnitsShippedNode = xnd.SelectSingleNode("ns0:Quantity", namespaceManager);
            objLineBE.ItemQuantity = xUnitsShippedNode == null ? "" : xUnitsShippedNode.InnerText;
            objLinesList.Add(j, objLineBE);
            j++;
        }
        objBE.GRLines = objLinesList;
        if (objBE.LineCount == 1 && objLinesList != null)
        {
            objBE.PONumber = objLinesList[0].PONumber;
            objBE.LoadID = objLinesList[0].LoadID;
        }
        else
        {
            objBE.PONumber = "See Details";
            objBE.LoadID = "See Details";
        }

        return objBE;
    }

    protected void gvGR_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ViewDetails")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];
                Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
                GenericCollection<GRListBE> listOfBE = (GenericCollection<GRListBE>)Session[vGRList];
                GRHeaderBE objHeader = new GRHeaderBE();
                GenericCollection<GRLinesBE> lines = new GenericCollection<GRLinesBE>();
                GRListBE Headerobj = new GRListBE();

                foreach (GRListBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }
                TransactionBC objBC = new TransactionBC();

                DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
                DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
                GenericCollection<GRListBE> FilesLlist = objBC.GetGRExtranetList("Files", DateFrom, DateTo, Headerobj.PONumber, Headerobj.LoadID, Headerobj.CM, Headerobj.TransactionID, Headerobj.TxnDate);
                foreach (GRListBE obj1 in FilesLlist)
                {
                    if (obj1.MessageType.ToLower() == "http://ms.it.ops.cm.rcvinventorymovement_v01_00_00#rcvinventorymovement_v01_00_00")
                    {
                        objHeader = PrepareGRList(obj1.ArchiveFile);
                        break;
                    }
                }
                trPODetails.Visible = false;
                Session["GRLineCollection"] = objHeader.GRLines;
                databind();
            }
            if (e.CommandName == "ViewFiles")
            {
                DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
                DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
                GenericCollection<GRListBE> listOfBE = (GenericCollection<GRListBE>)Session[vGRList];
                GRListBE Headerobj = new GRListBE();

                foreach (GRListBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }
                TransactionBC objBC = new TransactionBC();
                GenericCollection<GRListBE> FilesLlist = objBC.GetGRExtranetList("Files", DateFrom, DateTo, Headerobj.PONumber, Headerobj.LoadID, Headerobj.CM, Headerobj.TransactionID, Headerobj.TxnDate);
                foreach (GRListBE obj1 in FilesLlist)
                {
                    if (obj1.MessageType.ToLower() == "http://ms.it.ops.cm.rcvinventorymovement_v01_00_00#rcvinventorymovement_v01_00_00")
                        Headerobj.V02ArchiveFile = obj1.ArchiveFile;
                    if (obj1.MessageType.ToLower() == "http://schemas.microsoft.com/biztalk/edi/x12/2006#x12_00401_861")
                        Headerobj.EDIXMLArchiveFile = obj1.ArchiveFile;
                    if (obj1.MessageType.ToLower() == "http://schemas.microsoft.com/biztalk/edi/x12/2006#x12_00401_997" && obj1.ArchiveFile.ToLower().Contains(".txt"))
                        Headerobj.AckXMLArchiveFile = obj1.ArchiveFile;
                }
                Session["ArchiveFiles"] = Headerobj;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage('GR');", true);
            }
            if (e.CommandName == "ViewError")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblTransactionID = (Label)selectedRow.FindControl("lblTransactionID");
                Label lblTxnDate = (Label)selectedRow.FindControl("lblTxnDate");

                PurchaseOrderBC objBC = new PurchaseOrderBC();
                DateTime? TxnDateTime = lblTxnDate.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(lblTxnDate.Text);
                string Error = objBC.LoadGRErrorMessage(lblTransactionID.Text, "Extranet", TxnDateTime);
                string PoNumber = " Transaction ID : " + lblTransactionID.Text + " ** Posting Date : " + lblTxnDate.Text + " ** ";
                string errormessage = "Error Details : ** Error Number : " + Error.Split(',')[0] + " ** Error Description : ** " + Error.Split(',')[1];
                Session["ArchiveFiles"] = (PoNumber + errormessage).Replace("**",Environment.NewLine);
                //Label LabelTxnType = (Label)selectedRow.FindControl("LabelTxnType");
                string Eror = "My Error";
                //ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowMessage();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Registering", "$(document).ready(function(){ ShowMessage('" + lblTransactionID.Text + "','" + lblTxnDate.Text + "','" + Error.Split(',')[0] + "','" + Error.Split(',')[1] + "'); });", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage('Error');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void gvGR_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                ImageButton imStatus = (ImageButton)e.Row.FindControl("imgStatus");

                Label lbl997FuncAck = (Label)e.Row.FindControl("lbl997FuncAck");
                Label lbl824FuncAck = (Label)e.Row.FindControl("lbl824FuncAck");
                Label lblAperakFuncAck = (Label)e.Row.FindControl("lblMDN");
                Label lblPartner = (Label)e.Row.FindControl("lblPartner");



                Image img997Status = (Image)e.Row.FindControl("img997Status");
                Image img824tatus = (Image)e.Row.FindControl("img824Status");
                Image imgAperakStatus = (Image)e.Row.FindControl("imgMDNStatus");


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

                if (lbl997FuncAck.Text == "Pos997")
                {
                    img997Status.ImageUrl = "~/Images/Pos997.png";
                    img997Status.AlternateText = "Positive 997";
                    img824tatus.ImageUrl = "~/Images/NA.png";
                    img824tatus.AlternateText = "Not Applicable";
                }
                else if (lbl997FuncAck.Text == "Neg997")
                {
                    img997Status.ImageUrl = "~/Images/Neg997.png";
                    img997Status.AlternateText = "Negative 997";
                    img824tatus.ImageUrl = "~/Images/NA.png";
                    img824tatus.AlternateText = "Not Applicable";
                }
                else
                {
                    if (lblPartner.Text.ToLower().Contains("ceva"))
                    {
                        img997Status.ImageUrl = "~/Images/NA.png";
                        img997Status.AlternateText = "NA";
                    }
                    else
                    {
                        img997Status.ImageUrl = "~/Images/Warning.png";
                        img997Status.AlternateText = "Warning";
                    }
                }

                if (lbl824FuncAck.Text == "pos824")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Warning";
                    img824tatus.ImageUrl = "~/Images/Pos824.png";
                    img824tatus.AlternateText = "Positive 824";
                }
                else if (lbl824FuncAck.Text == "Neg824")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/Neg824.png";
                    img824tatus.AlternateText = "negative 824";
                }
                else if (lbl824FuncAck.Text == "Pos824Batch")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/Pos824.png";
                    img824tatus.AlternateText = "Positive 824 Batch";
                }
                else if (lbl824FuncAck.Text == "Neg824Batch")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/Neg824.png";
                    img824tatus.AlternateText = "negative 824";
                }
                else
                {
                    if(lblPartner.Text.ToLower().Contains("ceva"))
                    {
                        img824tatus.ImageUrl = "~/Images/Warning.png";
                        img824tatus.AlternateText = "Warning";
                    }
                    else
                    {
                        img824tatus.ImageUrl = "~/Images/NA.png";
                        img824tatus.AlternateText = "NA";
                    }
                }

                if (lblAperakFuncAck.Text == "PosMDN")
                {
                    imgAperakStatus.ImageUrl = "~/Images/PosMDN.png";
                    imgAperakStatus.AlternateText = "Positive MDN";
                }
                else if (lblAperakFuncAck.Text == "NegMDN")
                {
                    imgAperakStatus.ImageUrl = "~/Images/NegMDN.png";
                    imgAperakStatus.AlternateText = "Negative MDN";
                }

                else if (lblAperakFuncAck.Text == "NoMDN")
                {
                    imgAperakStatus.ImageUrl = "~/Images/NoMDN.png";
                    imgAperakStatus.AlternateText = "No MDN";
                }
                else
                {
                    imgAperakStatus.ImageUrl = "~/Images/Warning.png";
                    imgAperakStatus.AlternateText = "Warning";
                }
            }
        }

        catch (Exception ex)
        {

        }
    }
   

    protected void gvGRCorp_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ViewDetails")
            {
                DateTime? DateFrom = txtDateFromCorp.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFromCorp.Text);
                DateTime? DateTo = txtDateToCorp.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateToCorp.Text);
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];
                Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
                GenericCollection<GRListBE> listOfBE = (GenericCollection<GRListBE>)Session[vGRListCorp];
                GRHeaderBE objHeader = new GRHeaderBE();
                GenericCollection<GRLinesBE> lines = new GenericCollection<GRLinesBE>();
                GRListBE Headerobj = new GRListBE();

                foreach (GRListBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }
                TransactionBC objBC = new TransactionBC();
                GenericCollection<GRListBE> FilesLlist = objBC.GetGRCorpnetList("Files", DateFrom, DateTo, Headerobj.PONumber, Headerobj.LoadID, Headerobj.CM, Headerobj.TransactionID, Headerobj.RefID, Headerobj.TxnDate);
                foreach (GRListBE obj1 in FilesLlist)
                {
                    if (obj1.MessageType.ToLower() == "http://ms.it.ops.cm.rcvinventorymovement_v01_00_00#rcvinventorymovement_v01_00_00")
                    {
                        objHeader = PrepareGRList(obj1.ArchiveFile);
                        break;
                    }
                }
                trPODetails.Visible = false;
                Session["GRLineCollectionCorp"] = objHeader.GRLines;
                databind();
            }
            if (e.CommandName == "ViewFiles")
            {
                DateTime? DateFrom = txtDateFromCorp.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFromCorp.Text);
                DateTime? DateTo = txtDateToCorp.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateToCorp.Text);
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
                GenericCollection<GRListBE> listOfBE = (GenericCollection<GRListBE>)Session[vGRListCorp];
                GRListBE Headerobj = new GRListBE();

                foreach (GRListBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }
                TransactionBC objBC = new TransactionBC();
                GenericCollection<GRListBE> FilesLlist = objBC.GetGRCorpnetList("Files", DateFrom, DateTo, Headerobj.PONumber, Headerobj.LoadID, Headerobj.CM, Headerobj.TransactionID, Headerobj.RefID, Headerobj.TxnDate);
                foreach (GRListBE obj1 in FilesLlist)
                {
                    if (obj1.MessageType.ToLower() == "http://ms.it.ops.cm.rcvinventorymovement_v01_00_00#rcvinventorymovement_v01_00_00")
                        Headerobj.V02ArchiveFile = obj1.ArchiveFile;
                }
                Session["ArchiveFiles"] = Headerobj;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage('GR');", true);
            }
            if (e.CommandName == "ViewError")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblTransactionID = (Label)selectedRow.FindControl("lblTransactionID");
                Label lblTxnDate = (Label)selectedRow.FindControl("lblTxnDate");

                PurchaseOrderBC objBC = new PurchaseOrderBC();
                DateTime? TxnDateTime = lblTxnDate.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(lblTxnDate.Text);
                string Error = objBC.LoadGRErrorMessage(lblTransactionID.Text, "Corpnet", TxnDateTime);
                string PoNumber = " Transaction ID : " + lblTransactionID.Text + " ** Posting Date : " + lblTxnDate.Text + " ** ";
                string errormessage = "Error Details : ** Error Number : " + Error.Split(',')[0] + " ** Error Description : ** " + Error.Split(',')[1];
                Session["ArchiveFiles"] = (PoNumber + errormessage).Replace("**", Environment.NewLine);
                //Label LabelTxnType = (Label)selectedRow.FindControl("LabelTxnType");
                string Eror = "My Error";
                //ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowMessage();", true);
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Registering", "$(document).ready(function(){ ShowMessage('" + lblTransactionID.Text + "','" + lblTxnDate.Text + "','" + Error.Split(',')[0] + "','" + Error.Split(',')[1] + "'); });", true);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage('Error');", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void gvGRCorp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                ImageButton imStatus = (ImageButton)e.Row.FindControl("imgStatus");

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
            }
        }

        catch (Exception ex)
        {

        }
    }

    protected void btnfirst_Click(object sender, EventArgs e)
    {
        pos = 0;
        databind();
    }

    protected void btnprevious_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos -= 1;
        this.ViewState["vs"] = pos;
        databind();
    }

    protected void btnnext_Click(object sender, EventArgs e)
    {
        pos = (int)this.ViewState["vs"];
        pos += 1;
        this.ViewState["vs"] = pos;
        databind();
    }

    protected void btnlastCorp_Click(object sender, EventArgs e)
    {
        posCorp = (ViewState["PageCountCorp"] != null ? Convert.ToInt32(ViewState["PageCountCorp"]) : 1) - 1;
        databindCorp();
    }

    protected void btnfirstCorp_Click(object sender, EventArgs e)
    {
        posCorp = 0;
        databindCorp();
    }

    protected void btnpreviousCorp_Click(object sender, EventArgs e)
    {
        posCorp = (int)this.ViewState["vsCorp"];
        posCorp -= 1;
        this.ViewState["vsCorp"] = posCorp;
        databindCorp();
    }

    protected void btnnextCorp_Click(object sender, EventArgs e)
    {
        posCorp = (int)this.ViewState["vsCorp"];
        posCorp += 1;
        this.ViewState["vsCorp"] = pos;
        databindCorp();
    }

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = (ViewState["PageCount"] != null ? Convert.ToInt32(ViewState["PageCount"]) : 1) - 1;
        databind();
    }

    private void databind()
    {
        try
        {
            GenericCollection<GRLinesBE> GRLineCollection = Session["GRLineCollection"] == null ? null : Session["GRLineCollection"] as GenericCollection<GRLinesBE>;
            if (GRLineCollection != null)
            {
                adsource = new PagedDataSource();
                adsource.DataSource = GRLineCollection;
                adsource.PageSize = 10;
                ViewState["PageCount"] = (GRLineCollection.Count % 10) == 0 ? (GRLineCollection.Count / 10) : (GRLineCollection.Count / 10) + 1;
                adsource.AllowPaging = true;
                adsource.CurrentPageIndex = pos;
                btnfirst.Enabled = !adsource.IsFirstPage;
                btnprevious.Enabled = !adsource.IsFirstPage;
                btnlast.Enabled = !adsource.IsLastPage;
                btnnext.Enabled = !adsource.IsLastPage;

                gvGRLines.DataSource = adsource;
                gvGRLines.DataBind();
                divPaginationPoLine.Visible = true;
                gvGRLines.Visible = true;
                trPODetails.Visible = true;
            }
            else
            {
                trPODetails.Visible = false;
                gvGRLines.DataSource = null;
                gvGRLines.DataBind();
                divPaginationPoLine.Visible = false;
                gvGRLines.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }

    private void databindCorp()
    {
        try
        {
            GenericCollection<GRLinesBE> GRLineCollection = Session["GRLineCollectionCorp"] == null ? null : Session["GRLineCollectionCorp"] as GenericCollection<GRLinesBE>;
            if (GRLineCollection != null)
            {
                adsourceCorp = new PagedDataSource();
                adsourceCorp.DataSource = GRLineCollection;
                adsourceCorp.PageSize = 10;
                ViewState["PageCountCorp"] = (GRLineCollection.Count % 10) == 0 ? (GRLineCollection.Count / 10) : (GRLineCollection.Count / 10) + 1;
                adsourceCorp.AllowPaging = true;
                adsourceCorp.CurrentPageIndex = pos;
                btnfirstCorp.Enabled = !adsourceCorp.IsFirstPage;
                btnpreviousCorp.Enabled = !adsourceCorp.IsFirstPage;
                btnlastCorp.Enabled = !adsourceCorp.IsLastPage;
                btnnextCorp.Enabled = !adsourceCorp.IsLastPage;

                gvGRLinesCorp.DataSource = adsourceCorp;
                gvGRLinesCorp.DataBind();
                divPaginationPoLineCorp.Visible = true;
                gvGRLinesCorp.Visible = true;
                trGRDetails.Visible = true;
            }
            else
            {
                trGRDetails.Visible = false;
                gvGRLinesCorp.DataSource = null;
                gvGRLinesCorp.DataBind();
                divPaginationPoLineCorp.Visible = false;
                gvGRLinesCorp.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchErrorCorp.Text = Ex.Message;
            lblSearchErrorCorp.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    private void LoadGRFiles(string Archivepath, string TransactionID, string CorpStatus, string ExtStatus, string TxnType, string PONumber, string PODate, string Partner, string IsICOE, string BOD)
    {
        PurchaseOrderBC objBC = new PurchaseOrderBC();
        PurchaseOrderBE objArchiveBE = objBC.LoadPOFiles(TransactionID);
        objArchiveBE.PODate = PODate;
        objArchiveBE.PONumber = PONumber;
        objArchiveBE.ExtranetStatus = ExtStatus;
        objArchiveBE.CorpnetStatus = CorpStatus;
        objArchiveBE.CM = Partner;
        objArchiveBE.ReferenceID = TransactionID;
        objArchiveBE.isICOEPartner = IsICOE;
        objArchiveBE.MessageArchivePath = Archivepath;
        objArchiveBE.POCBODRcvd = BOD;
        Session["ArchiveFiles"] = objArchiveBE;
        Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage();", true);

    }

    //private GenericCollection<PurchaseOrderBE> GetGRStatus(GenericCollection<PurchaseOrderBE> CorpnetPOlist)
    //{
    //    PurchaseOrderBC objBC = new PurchaseOrderBC();
    //    GenericCollection<PurchaseOrderBE> ExtranetList = new GenericCollection<PurchaseOrderBE>();
    //    string POList = @"'";
    //    string ExtranetPartners = ConfigurationSettings.AppSettings["ExtranetCMS"];
    //    foreach (PurchaseOrderBE objBE in CorpnetPOlist)
    //    {
    //        if (ExtranetPartners.Contains(objBE.CM))
    //            POList += objBE.PONumber + @"','";
    //    }

    //    ExtranetList = objBC.GetGRStatus(POList);

    //    foreach (PurchaseOrderBE ExtobjBE in ExtranetList)
    //    {
    //        foreach (PurchaseOrderBE CorpObjBE in CorpnetPOlist)
    //        {
    //            if (ExtobjBE.PONumber == CorpObjBE.PONumber)
    //            {
    //                CorpObjBE.ExtranetStatus = ExtobjBE.ExtranetStatus;
    //            }
    //        }
    //    }

    //    return CorpnetPOlist;
    //}

    //private GenericCollection<PurchaseOrderBE> ModifyExtranetPOStatus(GenericCollection<PurchaseOrderBE> CorpnetPOlist)
    //{
    //    PurchaseOrderBC objBC = new PurchaseOrderBC();
    //    GenericCollection<PurchaseOrderBE> ExtranetList = new GenericCollection<PurchaseOrderBE>();
    //    string POList = @"'";
    //    string ExtranetPartners = ConfigurationSettings.AppSettings["ExtranetCMS"];
    //    foreach (PurchaseOrderBE objBE in CorpnetPOlist)
    //    {
    //        if (ExtranetPartners.Contains(objBE.CM))
    //        {
    //            POList += objBE.PONumber + @"','";
    //            objBE.isICOEPartner = "NO";
    //        }
    //        else
    //            objBE.isICOEPartner = "YES";
    //    }

    //    ExtranetList = objBC.ModifyExtranetPOStatus(POList);

    //    foreach (PurchaseOrderBE ExtobjBE in ExtranetList)
    //    {
    //        foreach (PurchaseOrderBE CorpObjBE in CorpnetPOlist)
    //        {
    //            if (ExtobjBE.PONumber == CorpObjBE.PONumber && ExtobjBE.CM == CorpObjBE.CM)
    //            {
    //                CorpObjBE.ExtranetStatus = ExtobjBE.ExtranetStatus;
    //            }
    //        }
    //    }

    //    return CorpnetPOlist;

    //}
    protected void gvGR_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<GRLinesBE> listofBE = (GenericCollection<GRLinesBE>)Session[vGRList];

            if (listofBE != null)
            {
                gvGR.PageIndex = e.NewPageIndex;
                gvGR.DataSource = listofBE;
                gvGR.DataBind();
            }
            else
            {
                gvGR.DataSource = null;
                gvGR.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvGRLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<GRLinesBE> listofBE = (GenericCollection<GRLinesBE>)Session["GRLineCollection"];

            if (listofBE != null)
            {
                gvGRLines.PageIndex = e.NewPageIndex;
                gvGRLines.DataSource = listofBE;
                gvGRLines.DataBind();
                divPaginationPoLine.Visible = true;
                gvGRLines.Visible = true;
            }
            else
            {
                gvGRLines.DataSource = null;
                gvGRLines.DataBind();
                divPaginationPoLine.Visible = false;
                gvGRLines.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvGRLines_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDirLine] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirLine].ToString());
            GenericCollection<GRLinesBE> list = Session[vGRList] as GenericCollection<GRLinesBE>;
            list = Utils.GridSorting<GRLinesBE>(gvGRLines, dir, e.SortExpression, list);
            Session["GRLineCollection"] = list;
            ViewState[vSortDirLine] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void gvGR_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDir] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDir].ToString());
            GenericCollection<PurchaseOrderBE> list = Session[vGRList] as GenericCollection<PurchaseOrderBE>;
            list = Utils.GridSorting<PurchaseOrderBE>(gvGR, dir, e.SortExpression, list);
            Session[vGRList] = list;
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
            GenericCollection<PurchaseOrderBE> listofBE = (GenericCollection<PurchaseOrderBE>)Session[vGRList];

            if (listofBE != null)
            {
                if (listofBE.Count == 0)
                {
                    lblSearchError.Text = "No records found.";
                    lblSearchError.Visible = true;
                    gvGR.DataSource = null;
                    gvGR.DataBind();
                    lblRec.Visible = false;
                }
                else
                {
                    Utils.GridPaging(gvGR, Convert.ToInt32(size), listofBE);
                    lblRec.Text = Utils.GridRecDispMsg(gvGR.PageIndex, gvGR.PageSize, listofBE.Count);
                    lblRec.Visible = true;
                }

            }
            else
            {
                gvGR.DataSource = null;
                gvGR.DataBind();
                lblRec.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            logFile.ErrorLogging(Ex);
        }
    }

    protected void gvGRCorp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchErrorCorp.Text = string.Empty;
            GenericCollection<GRLinesBE> listofBE = (GenericCollection<GRLinesBE>)Session[vGRListCorp];

            if (listofBE != null)
            {
                gvGRCorp.PageIndex = e.NewPageIndex;
                gvGRCorp.DataSource = listofBE;
                gvGRCorp.DataBind();
            }
            else
            {
                gvGRCorp.DataSource = null;
                gvGRCorp.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblSearchErrorCorp.Text = Ex.Message;
            lblSearchErrorCorp.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvGRLinesCorp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchErrorCorp.Text = string.Empty;
            GenericCollection<GRLinesBE> listofBE = (GenericCollection<GRLinesBE>)Session["GRLineCollectionCorp"];

            if (listofBE != null)
            {
                gvGRLinesCorp.PageIndex = e.NewPageIndex;
                gvGRLinesCorp.DataSource = listofBE;
                gvGRLinesCorp.DataBind();
                divPaginationPoLineCorp.Visible = true;
                gvGRLinesCorp.Visible = true;
            }
            else
            {
                gvGRLinesCorp.DataSource = null;
                gvGRLinesCorp.DataBind();
                divPaginationPoLineCorp.Visible = false;
                gvGRLinesCorp.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchErrorCorp.Text = Ex.Message;
            lblSearchErrorCorp.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvGRLinesCorp_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDirLineCorp] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirLineCorp].ToString());
            GenericCollection<GRLinesBE> list = Session[vGRListCorp] as GenericCollection<GRLinesBE>;
            list = Utils.GridSorting<GRLinesBE>(gvGRLinesCorp, dir, e.SortExpression, list);
            Session["GRLineCollectionCorp"] = list;
            ViewState[vSortDirLineCorp] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void gvGRCorp_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDirCorp] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirCorp].ToString());
            GenericCollection<PurchaseOrderBE> list = Session[vGRListCorp] as GenericCollection<PurchaseOrderBE>;
            list = Utils.GridSorting<PurchaseOrderBE>(gvGRCorp, dir, e.SortExpression, list);
            Session[vGRListCorp] = list;
            ViewState[vSortDirCorp] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void RepGvPagingItemCorp_Bound(Object sender, RepeaterCommandEventArgs e)
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
            ViewState[grdPageSizeCorp] = size.ToString();
            gridPagingCorp(size);
        }
    }

    protected void gridPagingCorp(string size)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<PurchaseOrderBE> listofBE = (GenericCollection<PurchaseOrderBE>)Session[vGRListCorp];

            if (listofBE != null)
            {
                if (listofBE.Count == 0)
                {
                    lblSearchErrorCorp.Text = "No records found.";
                    lblSearchErrorCorp.Visible = true;
                    gvGRCorp.DataSource = null;
                    gvGRCorp.DataBind();
                    lblRecCorp.Visible = false;
                }
                else
                {
                    Utils.GridPaging(gvGRCorp, Convert.ToInt32(size), listofBE);
                    lblRecCorp.Text = Utils.GridRecDispMsg(gvGRCorp.PageIndex, gvGRCorp.PageSize, listofBE.Count);
                    lblRecCorp.Visible = true;
                }

            }
            else
            {
                gvGRCorp.DataSource = null;
                gvGRCorp.DataBind();
                lblRecCorp.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchErrorCorp.Text = Ex.Message;
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
        BindRepeater(vSortDir, RepGvPaging, grdPageSize, vGRList, lblSearchError);
    }
    protected void btnCancelCorp_Click(object sender, EventArgs e)
    {
        rowGridCorp.Visible = false;
        lblSearchErrorCorp.Text = "";
        BindRepeater(vSortDirCorp, RepGvPagingCorp, grdPageSizeCorp, vGRListCorp, lblSearchErrorCorp);
    }
}
