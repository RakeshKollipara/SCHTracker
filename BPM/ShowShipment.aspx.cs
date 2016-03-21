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

public partial class ShowShipment : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string vSSList = "vSSList";
    private const string vSortDir = "sortDirection";
    private const string vSortDirLine = "sortDirectionLine";
    private const string grdPageSize = "grdPazeSize";
    LogHelper logFile = new LogHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "945 EDI/DESADV Search";
        if (!IsPostBack)
        {
            lblSearchError.Text = "";
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
        Session[vSSList] = null;
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
        BindShowShipment();
    }

    private void BindShowShipment()
    {
        TransactionBC objBC = new TransactionBC();
        PurchaseOrderBC objPucBC = new PurchaseOrderBC();
        try
        {
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);

            TimeSpan? duration = null;

            // Assign values to a and b...

            if (DateFrom.HasValue && DateTo.HasValue)
            {
                duration = DateTo.Value - DateFrom.Value;
            }

            double days = duration.GetValueOrDefault().TotalDays;

            if (days > 2 && txtDO.Text.Trim() == "")
            {
                lblSearchError.Text = "Please select 2 Days difference between dates due to high volume of data or Provide DO Number to Search";
                lblSearchError.Visible = true;
                return;
            }

            GenericCollection<ShowShipmentBE> listOfBE = objBC.GetShowShipment945Details("Get", TxtIDOC.Text.Trim(), txtDO.Text.Trim(), txtLoadID.Text.Trim(), drdStatus.SelectedValue, drdTxnType.SelectedValue, drdServiceComponentPartnerSearch.SelectedValue, txtPlant.Text.Trim(), txtOrderType.Text.Trim(), DateFrom, DateTo);

            foreach (ShowShipmentBE objExt1 in listOfBE)
            {
                if (objExt1.StageName == "SentPositive997ToPartner")
                    objExt1.Ack997Status = "pos997";
                if (objExt1.StageName == "SentNegative997ToPartner")
                    objExt1.Ack997Status = "neg997";
                if (objExt1.StageName == "SentPositiveEDI824ToPartner")
                    objExt1.Ack824Status = "pos824";
                if (objExt1.StageName == "SentNegativeEDI824ToPartner")
                    objExt1.Ack824Status = "Neg824";
                if (objExt1.StageName == "SendPositiveEDI824for945ToBatchPrimaryTransport")
                    objExt1.Ack824Status = "Pos824Batch";
                if (objExt1.StageName == "SendNegativeEDI824for945ToBatchPrimaryTransport")
                    objExt1.Ack824Status = "Neg824Batch";
                if (objExt1.StageName == "SendNegativeEDI824for945ToBatchSecondaryTransport")
                    objExt1.Ack824Status = "Neg824Batch";
                if (objExt1.StageName == "SendPositiveEDI824for945ToBatchSecondaryTransport")
                    objExt1.Ack824Status = "Pos824Batch";
                if (objExt1.StageName == "AperakAckSentToPartner")
                    objExt1.AckAperakStatus = "posAperak";
                if (objExt1.StageName == "AperakNAckSentToPartner")
                    objExt1.AckAperakStatus = "negAperak";
                if (objExt1.MDNStatus == "RcvdPositiveMDNFromPartner")
                    objExt1.MDNStatus = "PosMDN";
                if (objExt1.MDNStatus == "Error_RcvdNegativeMDNFromPartnerEx")
                    objExt1.MDNStatus = "NegMDN";
                if (objExt1.MDNStatus == "Error_MDNNotReceivedFromPartner")
                    objExt1.MDNStatus = "NoMDN";
            }

            foreach (ShowShipmentBE obj in listOfBE)
            {
                string PartnerName = objPucBC.GetPartnerName(obj.Plant);
                obj.CM = PartnerName == "" ? obj.CM : PartnerName;
            }

            Session[vSSList] = listOfBE;



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

    protected void gvDO_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
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
                GenericCollection<ShowShipmentBE> listOfBE = (GenericCollection<ShowShipmentBE>)Session[vSSList];
                ShowShipmentBE Headerobj = new ShowShipmentBE();

                foreach (ShowShipmentBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }

                TransactionBC objBC = new TransactionBC();
                GenericCollection<ShowShipmentBE> ArchiveObjects = new GenericCollection<ShowShipmentBE>();
                ArchiveObjects = objBC.GetShowShipment945Details("Files", lblTransactionID.Text, lblDONumber.Text, "", "", "", "", "", "", null, null);
                Headerobj.AckAperakStatus = ArchiveObjects[0].AckAperakStatus;
                Headerobj.AckArchiveFile = ArchiveObjects[0].AckArchiveFile;
                Headerobj.DesAdvArchiveFile = ArchiveObjects[0].DesAdvArchiveFile;
                Headerobj.EDIXMLArchiveFile = ArchiveObjects[0].EDIXMLArchiveFile;
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
                string Error = objBC.LoadPOErrorMessage(lblTransactioID.Text, lb.ID == "imgStatus" ? "Extranet" : "Corpnet");
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
                ImageButton imStatus = (ImageButton)e.Row.FindControl("imgStatus");

                Label lbl997FuncAck = (Label)e.Row.FindControl("lbl997FuncAck");
                Label lbl824FuncAck = (Label)e.Row.FindControl("lbl824FuncAck");
                Label lblAperakFuncAck = (Label)e.Row.FindControl("lblAperakFuncAck");
                Label lblMDNStatus = (Label)e.Row.FindControl("lblMDNStatus");

                Label lblTxnType = (Label)e.Row.FindControl("lblTxnType");

                Image img997Status = (Image)e.Row.FindControl("img997Status");
                Image img824tatus = (Image)e.Row.FindControl("img824tatus");
                Image imgAperakStatus = (Image)e.Row.FindControl("imgAperakStatus");
                Image imgMDNStatus = (Image)e.Row.FindControl("imgMDNStatus");


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

                if (lbl997FuncAck.Text == "pos997")
                {
                    img997Status.ImageUrl = "~/Images/Pos997.png";
                    img997Status.AlternateText = "Positive 997";
                    img824tatus.ImageUrl = "~/Images/NA.png";
                    img824tatus.AlternateText = "Not Applicable";
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }
                else if (lbl997FuncAck.Text == "neg997")
                {
                    img997Status.ImageUrl = "~/Images/Neg997.png";
                    img997Status.AlternateText = "Negative 997";
                    img824tatus.ImageUrl = "~/Images/NA.png";
                    img824tatus.AlternateText = "Not Applicable";
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }

                if (lbl824FuncAck.Text == "pos824")
                {
                    img997Status.ImageUrl = "~/Images/Warning.png";
                    img997Status.AlternateText = "Warning";
                    img824tatus.ImageUrl = "~/Images/Pos824.png";
                    img824tatus.AlternateText = "Positive 824";
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }
                else if (lbl824FuncAck.Text == "Neg824")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/Neg824.png";
                    img824tatus.AlternateText = "negative 824";
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }
                else if (lbl824FuncAck.Text == "Pos824Batch")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/Pos824.png";
                    img824tatus.AlternateText = "Positive 824 Batch";
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }
                else if (lbl824FuncAck.Text == "Neg824Batch")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/Neg824.png";
                    img824tatus.AlternateText = "negative 824";
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }

                if (lblAperakFuncAck.Text == "posAperak")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/NA.png";
                    img824tatus.AlternateText = "Not Applicable";
                    imgAperakStatus.ImageUrl = "~/Images/posAperak.png";
                    imgAperakStatus.AlternateText = "Positive Aperak";
                }
                else if (lblAperakFuncAck.Text == "negAperak")
                {
                    img997Status.ImageUrl = "~/Images/NA.png";
                    img997Status.AlternateText = "Not Applicable";
                    img824tatus.ImageUrl = "~/Images/NA.png";
                    img824tatus.AlternateText = "Not Applicable";
                    imgAperakStatus.ImageUrl = "~/Images/negAperak.png";
                    imgAperakStatus.AlternateText = "Negative Aperak";
                }

                if (lblTxnType.Text.ToLower() != "945desadv")
                {
                    imgAperakStatus.ImageUrl = "~/Images/NA.png";
                    imgAperakStatus.AlternateText = "Not Applicable";
                }

                if (lbl824FuncAck.Text.ToLower().Contains("824"))
                {
                    if (lblMDNStatus.Text == "PosMDN")
                    {
                        imgMDNStatus.ImageUrl = "~/Images/PosMDN.png";
                        imgMDNStatus.AlternateText = "Positive MDN";
                    }
                    else if (lblMDNStatus.Text == "NegMDN")
                    {
                        imgMDNStatus.ImageUrl = "~/Images/NegMDN.png";
                        imgMDNStatus.AlternateText = "Negative MDN";
                    }
                    else if (lblMDNStatus.Text == "NoMDN")
                    {
                        imgMDNStatus.ImageUrl = "~/Images/NoMDN.png";
                        imgMDNStatus.AlternateText = "No MDN";
                    }
                    else
                    {
                        imgMDNStatus.ImageUrl = "~/Images/NA.png";
                        imgMDNStatus.AlternateText = "Not Applicable";
                    }
                }
                else
                {
                    imgMDNStatus.ImageUrl = "~/Images/NA.png";
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
            GenericCollection<ShowShipmentBE> listofBE = (GenericCollection<ShowShipmentBE>)Session[vSSList];

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
    //        GenericCollection<ShowShipLinesBE> list = Session[vSSList] as GenericCollection<ShowShipLinesBE>;
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
            GenericCollection<ShowShipmentBE> list = Session[vSSList] as GenericCollection<ShowShipmentBE>;
            list = Utils.GridSorting<ShowShipmentBE>(gvDO, dir, e.SortExpression, list);
            Session[vSSList] = list;
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
            GenericCollection<ShowShipmentBE> listofBE = (GenericCollection<ShowShipmentBE>)Session[vSSList];

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
        txtLoadID.Text = "";
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