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

public partial class Transformations : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string v841List = "v841List";
    private const string vSortDir = "sortDirection";
    private const string vSortDirLine = "sortDirectionLine";
    private const string grdPageSize = "grdPazeSize";
    LogHelper logFile = new LogHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Transformations 841 Search";
        if (!IsPostBack)
        {
            lblSearchError.Text = "";
            PopulateServiceComponentDropDown("", "", "", "PARTNERS");
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
        lblSearchError.Text = "";
        Session[v841List] = null;
    }

    private void PopulateServiceComponentDropDown(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompType)
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<ServiceComponent> ServiceComponentList = new GenericCollection<ServiceComponent>();
            ServiceComponentList = objBC.GetServiceComponentList(ServiceLineID, ServiceID, ServiceOptionID, ServCompType);
            drdServiceComponentPartnerSearch.DataTextField = "ServiceComponentDesc";
            drdServiceComponentPartnerSearch.DataValueField = "ServiceComponentDesc";
            drdServiceComponentPartnerSearch.DataSource = ServiceComponentList;
            drdServiceComponentPartnerSearch.DataBind();
            //if (drdServiceComponentPartnerSearch != null)
            //{
            //    foreach (ListItem li in drdServiceComponentPartnerSearch.Items)
            //    {
            //        li.Attributes["title"] = li.Text;
            //    }
            //}
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
        DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
        DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
        TimeSpan? duration = null;
        if (Convert.ToDateTime(txtDateFrom.Text) > Convert.ToDateTime(txtDateTo.Text))
        {
            lblSearchError.Text = "Date From should be less than Date To.";
            return;
        }
        if (DateFrom.HasValue && DateTo.HasValue)
        {
            duration = DateTo.Value - DateFrom.Value;
        }

        double days = duration.GetValueOrDefault().TotalDays;

        if (days > 15)
        {
            lblSearchError.Text = "Please select 15 Days difference between dates due to high volume of data.";
            lblSearchError.Visible = true;
            return;
        }
        gvTransformations.DataSource = null;
        gvTransformations.DataBind();
        rowGrid.Visible = false;
        BindRepeater();
        BindPurchaseOrder();
    }

    private void BindPurchaseOrder()
    {
        TransactionBC objBC = new TransactionBC();
        try
        {
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
            GenericCollection<TransformationBE> listOfBE = objBC.GetTransformationDetails(DateFrom, DateTo, txtControlNumber.Text.Trim(), drdServiceComponentPartnerSearch.SelectedValue,txtPONumber.Text.Trim(),txtPlant.Text.Trim());
            GenericCollection<TransformationBE> DisplayList = new GenericCollection<TransformationBE>();

            foreach (TransformationBE objExt1 in listOfBE)
            {
                if ((objExt1.StageName.ToLower().Contains("error") || objExt1.StageName.ToLower().Contains("failed")) && (objExt1.StageName != "LogError_NegativeMDNReceived" || objExt1.StageName != "LogError_MDN_NotReceived"))
                    objExt1.Status = "Failed";
                else
                    objExt1.Status = "Success";
                if (objExt1.StageName == "SentPositive997ToPartner")
                    objExt1.FuncAck = "pos997";
                if (objExt1.StageName == "SentNegative997ToPartner")
                    objExt1.FuncAck = "neg997";
                if (objExt1.StageName == "SentPositiveEDI824ToPartner")
                    objExt1.FuncAck = "pos824";
                if (objExt1.StageName == "SentNegativeEDI824ToPartner")
                    objExt1.FuncAck = "Neg824";
                if (objExt1.StageName == "LogError_MDN_NotReceived")
                    objExt1.MDN = "NoMDN";
                if (objExt1.StageName == "PositiveMDNReceived")
                    objExt1.MDN = "posMDN";
                if (objExt1.StageName == "LogError_NegativeMDNReceived")
                    objExt1.MDN = "NegMDN";
                if (objExt1.StageName == "SendPositiveEDI824for841ToBatchPrimaryTransport")
                    objExt1.FuncAck = "Pos824Batch";
                if (objExt1.StageName == "SendNegativeEDI824for841ToBatchPrimaryTransport")
                    objExt1.FuncAck = "Neg824Batch";
            }

            for (int j = 0; j < listOfBE.Count; j++)
            {
                if (DisplayList.Count == 0)
                {
                    DisplayList.Add(j, listOfBE[j]);
                }
                else
                {
                    bool duplicate = false;
                    for (int k = 0; k < DisplayList.Count; k++)
                    {
                        if (listOfBE[j].ControlNumber == DisplayList[k].ControlNumber && listOfBE[j].CM == DisplayList[k].CM && listOfBE[j].TxnDate == DisplayList[k].TxnDate)
                        {
                            DisplayList[k].Status = String.IsNullOrEmpty(DisplayList[k].Status) ? listOfBE[j].Status : DisplayList[k].Status;
                            DisplayList[k].FuncAck = String.IsNullOrEmpty(DisplayList[k].FuncAck) ? listOfBE[j].FuncAck : DisplayList[k].FuncAck;
                            DisplayList[k].MDN = String.IsNullOrEmpty(DisplayList[k].MDN) ? listOfBE[j].MDN : DisplayList[k].MDN;
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                        DisplayList.Add(DisplayList.Count, listOfBE[j]);
                }
            }

            if (DisplayList.Count > 0)
            {
                listOfBE = DisplayList;
            }

            Session[v841List] = listOfBE;

            if (ViewState[grdPageSize] != null)
            {
                gvTransformations.PageSize = Convert.ToInt32(ViewState[grdPageSize].ToString());
            }
            else
            {
                gvTransformations.PageSize = 10;
            }


            lblRec.Text = Utils.GridRecDispMsg(gvTransformations.PageIndex, gvTransformations.PageSize, listOfBE.Count);
            gvTransformations.Visible = true;
            if (listOfBE != null && listOfBE.Count > 0)
            {

                rowPage.Visible = true;
                rowGrid.Visible = true;

                gvTransformations.DataSource = listOfBE;
                gvTransformations.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvTransformations.PageIndex, gvTransformations.PageSize, listOfBE.Count);
                gvTransformations.Visible = true;
                lblRec.Visible = true;
            }
            else
            {
                lblRec.Visible = false;
                rowGrid.Visible = false;
                rowPage.Visible = false;


                gvTransformations.Visible = false;
                gvTransformations.DataSource = null;
                gvTransformations.DataBind();
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

    protected void gvTransformations_RowCommand(object sender, GridViewCommandEventArgs e)
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
                GenericCollection<TransformationBE> listOfBE = (GenericCollection<TransformationBE>)Session[v841List];
                TransformationBE Headerobj = new TransformationBE();

                foreach (TransformationBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }

                TransactionBC objBC = new TransactionBC();
                TransformationBE objArchiveBE = objBC.Load841Files(Headerobj.ControlNumber, Headerobj.DUNS, Headerobj.TxnDate);
                Headerobj.ArchiveFile = objArchiveBE.ArchiveFile;
                Headerobj.AckArchiveFile = objArchiveBE.AckArchiveFile;

                Session["ArchiveFiles"] = Headerobj;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage();", true);
            }
        }
        catch (Exception ex)
        {

        }
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
    protected void gvTransformations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<TransformationBE> listofBE = (GenericCollection<TransformationBE>)Session[v841List];

            if (listofBE != null)
            {
                gvTransformations.PageIndex = e.NewPageIndex;
                gvTransformations.DataSource = listofBE;
                gvTransformations.DataBind();
            }
            else
            {
                gvTransformations.DataSource = null;
                gvTransformations.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvTransformations_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDir] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDir].ToString());
            GenericCollection<TransformationBE> list = Session[v841List] as GenericCollection<TransformationBE>;
            list = Utils.GridSorting<TransformationBE>(gvTransformations, dir, e.SortExpression, list);
            Session[v841List] = list;
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
            GenericCollection<TransformationBE> listofBE = (GenericCollection<TransformationBE>)Session[v841List];

            if (listofBE != null)
            {
                if (listofBE.Count == 0)
                {
                    lblSearchError.Text = "No records found.";
                    lblSearchError.Visible = true;
                    gvTransformations.DataSource = null;
                    gvTransformations.DataBind();
                    lblRec.Visible = false;
                }
                else
                {
                    Utils.GridPaging(gvTransformations, Convert.ToInt32(size), listofBE);
                    lblRec.Text = Utils.GridRecDispMsg(gvTransformations.PageIndex, gvTransformations.PageSize, listofBE.Count);
                    lblRec.Visible = true;
                }

            }
            else
            {
                gvTransformations.DataSource = null;
                gvTransformations.DataBind();
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
    }
    protected void gvTransformations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblExtStatus = (Label)e.Row.FindControl("lblExtranetStatus");
            Label LabelFunctionalAck = (Label)e.Row.FindControl("LabelFunctionalAck");
            Label lblMDNForFuncAck = (Label)e.Row.FindControl("lblMDNForFuncAck");

            Image imgExtStatus = (Image)e.Row.FindControl("imgStatus");
            Image imgAckStatus = (Image)e.Row.FindControl("imgAckStatus");
            Image imgMDNRcvStatus = (Image)e.Row.FindControl("imgMDNRcvStatus");


            if (lblExtStatus.Text == "Success")
            {
                imgExtStatus.ImageUrl = "~/Images/Success.png";
                imgExtStatus.AlternateText = "Success";
            }
            else if (lblExtStatus.Text == "Failed")
            {
                imgExtStatus.ImageUrl = "~/Images/Failed.png";
                imgExtStatus.AlternateText = "Failed";
            }
            else
            {
                imgExtStatus.ImageUrl = "~/Images/Warning.png";
                imgExtStatus.AlternateText = "Not Applicable";
            }
            if (LabelFunctionalAck.Text == "pos997")
            {
                imgAckStatus.ImageUrl = "~/Images/Pos997.png";
                imgAckStatus.AlternateText = "Positive 997";
            }
            else if (LabelFunctionalAck.Text == "neg997")
            {
                imgAckStatus.ImageUrl = "~/Images/Neg997.png";
                imgAckStatus.AlternateText = "Negative 997";
            }

            else if (LabelFunctionalAck.Text == "pos824")
            {
                imgAckStatus.ImageUrl = "~/Images/Pos824.png";
                imgAckStatus.AlternateText = "Positive 824";
            }
            else if (LabelFunctionalAck.Text == "Neg824")
            {
                imgAckStatus.ImageUrl = "~/Images/Neg824.png";
                imgAckStatus.AlternateText = "negative 824";
            }
            else if (LabelFunctionalAck.Text == "Pos824Batch")
            {
                imgAckStatus.ImageUrl = "~/Images/Pos824.png";
                imgAckStatus.AlternateText = "Positive 824 batch";
            }
            else if (LabelFunctionalAck.Text == "Neg824Batch")
            {
                imgAckStatus.ImageUrl = "~/Images/Neg824.png";
                imgAckStatus.AlternateText = "Negative 824 Bath";
            }
            else
            {
                imgAckStatus.ImageUrl = "~/Images/NA.png";
                imgAckStatus.AlternateText = "Not Applicable";
            }

            if (LabelFunctionalAck.Text == "Neg824Batch" || LabelFunctionalAck.Text == "Pos824Batch")
            {
                imgMDNRcvStatus.ImageUrl = "~/Images/NA.png";
                imgMDNRcvStatus.AlternateText = "Not Applicable";
            }
            else
            {
                if (lblMDNForFuncAck.Text == "NoMDN")
                {
                    imgMDNRcvStatus.ImageUrl = "~/Images/NoMDN.png";
                    imgMDNRcvStatus.AlternateText = "Success";
                }
                else if (lblMDNForFuncAck.Text == "posMDN")
                {
                    imgMDNRcvStatus.ImageUrl = "~/Images/PosMDN.png";
                    imgMDNRcvStatus.AlternateText = "Failed";
                }

                else if (lblMDNForFuncAck.Text == "NegMDN")
                {
                    imgMDNRcvStatus.ImageUrl = "~/Images/NegMDN.png";
                    imgMDNRcvStatus.AlternateText = "Failed";
                }
                else
                {
                    imgMDNRcvStatus.ImageUrl = "~/Images/NA.png";
                    imgMDNRcvStatus.AlternateText = "Not Applicable";
                }


            }
        }
    }
}