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


public partial class PO : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string vPOList = "vPOList";
    private const string vSortDir = "sortDirection";
    private const string vSortDirLine = "sortDirectionLine";
    private const string vSortDirHistory = "sortDirectionHistory";
    private const string grdPageSize = "grdPazeSize";
    LogHelper logFile = new LogHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "PO Search";
        if (!IsPostBack)
        {
            lblSearchError.Text = "";
            //PopulateTransactionType("", "", "", "");
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
        lblSearchError.Visible = false;
        Session[vPOList] = null;
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

    private void PopulateTransactionType(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompID)
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<TransactionType> TransactionTypeList = new GenericCollection<TransactionType>();
            TransactionTypeList = objBC.GetTransactionTypeList(ServiceLineID, ServiceID, ServiceOptionID, ServCompID);
            drdTxnType.DataTextField = "TransactionTypeDesc";
            drdTxnType.DataValueField = "TransactionTypeID";
            drdTxnType.DataSource = TransactionTypeList;
            drdTxnType.DataBind();
            if (drdTxnType != null)
            {
                foreach (ListItem li in drdTxnType.Items)
                {
                    li.Attributes["title"] = li.Text;
                }
            }
            drdTxnType.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }
    }



    protected void drdTxnType_PreRender(object sender, EventArgs e)
    {
        retainDropdownToolTip(drdTxnType);
    }
    protected void drdTxnType_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        if (drdTxnType.Items.Count > 1)
        {
            gvPurchaseOrder.DataSource = null;
            gvPurchaseOrder.DataBind();
            rowGrid.Visible = false;
            lblSearchError.Visible = false;
            BindRepeater();
            BindPurchaseOrder();
        }
        else
        {
            gvPurchaseOrder.DataSource = null;
            gvPurchaseOrder.DataBind();
            rowGrid.Visible = false;
            lblSearchError.Visible = true;
            lblSearchError.Text = "No Records available for the following Selection, Please change your selection and try once again.";
        }
    }

    private void BindPurchaseOrder()
    {
        PurchaseOrderBC objBC = new PurchaseOrderBC();
        try
        {
            trPODetails.Visible = false; 
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateFrom.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
            trPODetails.Visible = false;
            GenericCollection<PurchaseOrderBE> listOfBE = objBC.GetIndividualPODetails(txtPONumber.Text, drdTxnType.SelectedValue, DateFrom, DateTo, drdServiceComponentPartnerSearch.SelectedValue);

            GenericCollection<PurchaseOrderBE> FinalList = new GenericCollection<PurchaseOrderBE>();


            string ExtranetPartners = ConfigurationSettings.AppSettings["ExtranetCMS"];

            foreach (PurchaseOrderBE objBE in listOfBE)
            {
                if (ExtranetPartners.Contains(objBE.CM))
                {
                    objBE.isICOEPartner = "NO";
                }
                else
                    objBE.isICOEPartner = "YES";
            }

            if (drdISOEorExtranet.SelectedIndex == 1)
            {
                int i = 0;
                foreach (PurchaseOrderBE obj1 in listOfBE)
                {
                    if (obj1.isICOEPartner == "YES")
                        FinalList.Add(i, obj1);
                }
                listOfBE = FinalList;
            }

            if (drdISOEorExtranet.SelectedIndex == 2)
            {
                int i = 0;
                foreach (PurchaseOrderBE obj1 in listOfBE)
                {
                    if (obj1.isICOEPartner == "NO")
                        FinalList.Add(i, obj1);
                }
                listOfBE = FinalList;
            }


            if (drdISOEorExtranet.SelectedIndex != 1)
                listOfBE = ModifyExtranetPODetails(listOfBE);


            if (drdPOStatus.SelectedIndex == 1)
            {
                int j = 0;
                for (int i = 0; i < listOfBE.Count; i++ )
                {
                    if (listOfBE[i].isICOEPartner == "NO" && listOfBE[i].ExtranetStatus.ToLower() == "success")
                    {
                        FinalList.Add(j, listOfBE[i]);
                        j++;
                    }
                    if (listOfBE[i].isICOEPartner == "YES" && listOfBE[i].CorpnetStatus.ToLower() == "success")
                    { 
                        FinalList.Add(j, listOfBE[i]);
                        j++;
                    }
                }
                listOfBE = FinalList;
            }

            if (drdPOStatus.SelectedIndex == 2)
            {
                int j = 0;
                for (int i = 0; i < listOfBE.Count; i++ )
                {
                    if (listOfBE[i].isICOEPartner == "NO" && listOfBE[i].ExtranetStatus.ToLower() == "failed")
                    {
                        FinalList.Add(j, listOfBE[i]);
                        j++;
                    }
                    if (listOfBE[i].isICOEPartner == "YES" && listOfBE[i].CorpnetStatus.ToLower() == "failed")
                    {
                        FinalList.Add(j, listOfBE[i]);
                        j++;
                    }
                }
                listOfBE = FinalList;
            }

            if (drdPOStatus.SelectedIndex == 3)
            {
                for (int i = 0; i < listOfBE.Count; i++ )
                {
                    if (listOfBE[i].isICOEPartner == "NO" && (listOfBE[i].ExtranetStatus.ToLower() == "failed" || listOfBE[i].ExtranetStatus.ToLower() == "na"))
                        FinalList.Add(i, listOfBE[i]);
                }
                listOfBE = FinalList;
            }


            //listOfBE = GetASNStatus(listOfBE);


            if (ViewState[grdPageSize] != null)
            {
                gvPurchaseOrder.PageSize = Convert.ToInt32(ViewState[grdPageSize].ToString());
            }
            else
            {
                gvPurchaseOrder.PageSize = 10;
            }


            lblRec.Text = Utils.GridRecDispMsg(gvPurchaseOrder.PageIndex, gvPurchaseOrder.PageSize, listOfBE.Count);
            gvPurchaseOrder.Visible = true;
            if (listOfBE != null && listOfBE.Count > 0)
            {

                rowPage.Visible = true;
                rowGrid.Visible = true;

                gvPurchaseOrder.DataSource = listOfBE;
                gvPurchaseOrder.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvPurchaseOrder.PageIndex, gvPurchaseOrder.PageSize, listOfBE.Count);
                gvPurchaseOrder.Visible = true;
                lblRec.Visible = true;
            }
            else
            {
                lblRec.Visible = false;
                rowGrid.Visible = false;
                rowPage.Visible = false;


                gvPurchaseOrder.Visible = false;
                gvPurchaseOrder.DataSource = null;
                gvPurchaseOrder.DataBind();
                lblSearchError.Text = "No records found.";
                lblSearchError.Visible = true;

            }
            Session[vPOList] = listOfBE;

        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }

    private GenericCollection<PurchaseOrderBE> GetASNStatus(GenericCollection<PurchaseOrderBE> CorpnetPOlist)
    {
        PurchaseOrderBC objBC = new PurchaseOrderBC();
        GenericCollection<PurchaseOrderBE> ExtranetList = new GenericCollection<PurchaseOrderBE>();
        string POList = @"'";
        string ExtranetPartners = ConfigurationSettings.AppSettings["ExtranetCMS"];
        foreach (PurchaseOrderBE objBE in CorpnetPOlist)
        {
            if (ExtranetPartners.Contains(objBE.CM))
                POList += objBE.PONumber + @"','";
        }

        ExtranetList = objBC.GetASNStatus(POList);

        foreach (PurchaseOrderBE ExtobjBE in ExtranetList)
        {
            foreach (PurchaseOrderBE CorpObjBE in CorpnetPOlist)
            {
                if (ExtobjBE.PONumber == CorpObjBE.PONumber)
                {
                    CorpObjBE.ExtranetStatus = ExtobjBE.ExtranetStatus;
                }
            }
        }

        return CorpnetPOlist;
    }

    private GenericCollection<PurchaseOrderBE> ModifyExtranetPODetails(GenericCollection<PurchaseOrderBE> CorpnetPOlist)
    {
        PurchaseOrderBC objBC = new PurchaseOrderBC();
        GenericCollection<PurchaseOrderBE> ExtranetList = new GenericCollection<PurchaseOrderBE>();


        ExtranetList = objBC.ModifyExtranetPODetails(txtPONumber.Text, "PoDetails");

        foreach (PurchaseOrderBE ExtobjBE in ExtranetList)
        {
            foreach (PurchaseOrderBE CorpObjBE in CorpnetPOlist)
            {
                if (ExtobjBE.ReferenceID == CorpObjBE.ReferenceID)
                {
                    CorpObjBE.ExtranetStatus = ExtobjBE.ExtranetStatus;
                    if (ExtobjBE.POCBODRcvd == "Error_NotRcvdConfirmBODFrmPartnerEx")
                        CorpObjBE.POCBODRcvd = "NoBOD";
                    if (ExtobjBE.POCBODRcvd == "PositiveConfirmBODReceived")
                        CorpObjBE.POCBODRcvd = "PosBOD";
                    if (ExtobjBE.POCBODRcvd == "NegativeConfirmBODReceived")
                        CorpObjBE.POCBODRcvd = "NegBOD";
                }
            }
        }

        return CorpnetPOlist;

    }
    protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            lblSearchError.Text = string.Empty;
            GenericCollection<PurchaseOrderBE> listofBE = (GenericCollection<PurchaseOrderBE>)Session[vPOList];

            if (listofBE != null)
            {
                gvPurchaseOrder.PageIndex = e.NewPageIndex;
                gvPurchaseOrder.DataSource = listofBE;
                gvPurchaseOrder.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvPurchaseOrder.PageIndex, gvPurchaseOrder.PageSize, listofBE.Count);
                lblRec.Visible = true;
            }
            else
            {
                gvPurchaseOrder.DataSource = null;
                gvPurchaseOrder.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }



    protected void gvPurchaseOrder_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDir] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDir].ToString());
            GenericCollection<PurchaseOrderBE> list = Session[vPOList] as GenericCollection<PurchaseOrderBE>;
            list = Utils.GridSorting<PurchaseOrderBE>(gvPurchaseOrder, dir, e.SortExpression, list);
            Session[vPOList] = list;
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
            GenericCollection<PurchaseOrderBE> listofBE = (GenericCollection<PurchaseOrderBE>)Session[vPOList];

            if (listofBE != null)
            {
                if (listofBE.Count == 0)
                {
                    lblSearchError.Text = "No records found.";
                    lblSearchError.Visible = true;
                    gvPurchaseOrder.DataSource = null;
                    gvPurchaseOrder.DataBind();
                    lblRec.Visible = false;
                }
                else
                {
                    Utils.GridPaging(gvPurchaseOrder, Convert.ToInt32(size), listofBE);
                    lblRec.Text = Utils.GridRecDispMsg(gvPurchaseOrder.PageIndex, gvPurchaseOrder.PageSize, listofBE.Count);
                    lblRec.Visible = true;
                }

            }
            else
            {
                gvPurchaseOrder.DataSource = null;
                gvPurchaseOrder.DataBind();
                lblRec.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvPurchaseOrder_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCorpnetStatus = (Label)e.Row.FindControl("lblCorpnetStatus");
                Label lblExtStatus = (Label)e.Row.FindControl("lblExtStatus");
                Label lblTransactionID = (Label)e.Row.FindControl("lblTransactionID");
                Label LabelTxnType = (Label)e.Row.FindControl("LabelTxnType");
                Label lblIsICOE = (Label)e.Row.FindControl("lblICOE");
                Label LabelPOCBODRcvd = (Label)e.Row.FindControl("LabelPOCBODRcvd");
                Label lblPoNumber = (Label)e.Row.FindControl("lblPoNumber");

                ImageButton imgCorpStatus = (ImageButton)e.Row.FindControl("imgCorpStatus");
                ImageButton imgExtStatus = (ImageButton)e.Row.FindControl("imgExtStatus");
                Image imgCBODStatus = (Image)e.Row.FindControl("imgCBODStatus");

                if (lblCorpnetStatus.Text == "Success")
                {
                    imgCorpStatus.ImageUrl = "~/Images/Success.png";
                    imgCorpStatus.AlternateText = "Success";
                    imgCorpStatus.Enabled = false;
                }
                else if (lblCorpnetStatus.Text == "Failed")
                {
                    imgCorpStatus.ImageUrl = "~/Images/Failed.png";
                    imgCorpStatus.AlternateText = "Failed";
                    imgCorpStatus.Enabled = true;
                }
                else
                {
                    imgCorpStatus.ImageUrl = "~/Images/NA.png";
                    imgCorpStatus.AlternateText = "Not Applicable";
                    imgCorpStatus.Enabled = false;
                }

                if (lblIsICOE.Text.ToLower() == "no")
                {
                    if (lblExtStatus.Text == "Success")
                    {
                        imgExtStatus.ImageUrl = "~/Images/Success.png";
                        imgExtStatus.AlternateText = "Success";
                        imgExtStatus.Enabled = false;
                    }
                    else if (lblExtStatus.Text == "Failed")
                    {
                        imgExtStatus.ImageUrl = "~/Images/Failed.png";
                        imgExtStatus.AlternateText = "Failed";
                        imgExtStatus.Enabled = true;
                    }
                    else
                    {
                        imgExtStatus.ImageUrl = "~/Images/Warning.png";
                        imgExtStatus.AlternateText = "Not Applicable";
                        imgExtStatus.Enabled = false;
                    }

                    if (LabelPOCBODRcvd.Text == "NegBOD")
                    {
                        imgCBODStatus.ImageUrl = "~/Images/NegBOD.png";
                        imgCBODStatus.AlternateText = "Negative BOD";
                    }
                    else if (LabelPOCBODRcvd.Text == "PosBOD")
                    {
                        imgCBODStatus.ImageUrl = "~/Images/PosBOD.png";
                        imgCBODStatus.AlternateText = "Positive BOD";
                    }
                    else
                    {
                        imgCBODStatus.ImageUrl = "~/Images/NoBOD.png";
                        imgCBODStatus.AlternateText = "No BOD";
                    }
                }
                else
                {
                    imgExtStatus.ImageUrl = "~/Images/NA.png";
                    imgExtStatus.AlternateText = "Not Applicable";
                    imgExtStatus.Enabled = false;
                    imgCBODStatus.ImageUrl = "~/Images/NA.png";
                    imgCBODStatus.AlternateText = "Not Applicable";
                }
            }
        }

        catch (Exception ex)
        {

        }
    }
    protected void gvPurchaseOrder_RowCommand(object sender, GridViewCommandEventArgs e)
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

                Label lblPoNumber = (Label)selectedRow.FindControl("lblPoNumber");
                Label lblArchivePath = (Label)selectedRow.FindControl("lblArchivePath");
                Label lblMessageType = (Label)selectedRow.FindControl("lblMessageType");

                LoadPODetails(lblPoNumber.Text, lblArchivePath.Text, lblMessageType.Text);
            }
            if (e.CommandName == "ViewFiles")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblCorpnetStatus = (Label)selectedRow.FindControl("lblCorpnetStatus");
                Label lblExtStatus = (Label)selectedRow.FindControl("lblExtStatus");
                Label LabelTxnType = (Label)selectedRow.FindControl("LabelTxnType");
                Label lblTransactioID = (Label)selectedRow.FindControl("lblTransactioID");
                Label lblPoNumber = (Label)selectedRow.FindControl("lblPoNumber");
                Label lblTxnDate = (Label)selectedRow.FindControl("lblTxnDate");
                Label lblPartner = (Label)selectedRow.FindControl("lblPartner");
                Label lblICOE = (Label)selectedRow.FindControl("lblICOE");
                Label lblArchivePath = (Label)selectedRow.FindControl("lblArchivePath");
                Label LabelPOCBODRcvd = (Label)selectedRow.FindControl("LabelPOCBODRcvd");

                LoadPOFiles(lblArchivePath.Text, lblTransactioID.Text, lblCorpnetStatus.Text, lblExtStatus.Text, LabelTxnType.Text, lblPoNumber.Text, lblTxnDate.Text, lblPartner.Text, lblICOE.Text, LabelPOCBODRcvd.Text);
            }
            if (e.CommandName == "ViewError")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblTransactioID = (Label)selectedRow.FindControl("lblTransactioID");
                Label lblPoNumber = (Label)selectedRow.FindControl("lblPoNumber");
                Label LabelTxnType = (Label)selectedRow.FindControl("LabelTxnType");

                PurchaseOrderBC objBC = new PurchaseOrderBC();
                string Error = objBC.LoadPOErrorMessage(lblTransactioID.Text, lb.ID == "imgExtStatus" ? "Extranet" : "Corpnet");
                string PoNumber = " PO Number : " + lblPoNumber.Text + " \\n Transaction Type : " + LabelTxnType.Text + " \\n Error Details : \\n ";
                string errormessage = "Error Number : " + Error.Split(',')[0] + " \\n Error Description : \\n " + Error.Split(',')[1];
                //Label LabelTxnType = (Label)selectedRow.FindControl("LabelTxnType");
                string Eror = "My Error";
                //ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowMessage();", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Registering", "$(document).ready(function(){ ShowMessage('" + lblPoNumber.Text + "','" + LabelTxnType.Text + "','" + Error.Split(',')[0] + "','" + Error.Split(',')[1] + "'); });", true);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void LoadPOFiles(string Archivepath, string TransactionID, string CorpStatus, string ExtStatus, string TxnType, string PONumber, string PODate, string Partner, string IsICOE, string BOD)
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

    private void LoadPODetails(string PoNumber, string POFile, string MessageType)
    {
        try
        {
            PurchaseOrderBC objBC = new PurchaseOrderBC();
            //string ArchiveFile = objBC.GetPOArchiveFile(PONumber);

            XmlDocument poFile = new XmlDocument();
            poFile.Load(POFile);
            string RootNode = "";
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(poFile.NameTable);
            if (MessageType.ToLower() == "http://ms.it.ops.cm.processpurchaseorder_v02_10_00#processpurchaseorder_v02_10_00")
            {
                namespaceManager.AddNamespace("ns0", "http://MS.IT.Ops.CM.ProcessPurchaseOrder_V02_10_00");
                RootNode = "ProcessPurchaseOrder_V02_10_00";
            }
            else
            {
                namespaceManager.AddNamespace("ns0", "http://MS.IT.Ops.CM.ChangePurchaseOrder_V01_00_00");
                RootNode = "ChangePurchaseOrder_V01_00_00";
            }

            XmlNode xPriceNode = poFile.SelectSingleNode("/ns0:" + RootNode + "/DataArea/PurchaseOrder/Header/ns0:Price/ns0:Amount", namespaceManager);
            lblPOTotalPrice.Text = xPriceNode == null ? "" : xPriceNode.InnerText;
            XmlNodeList PartiesNodeList = poFile.SelectNodes("/ns0:" + RootNode + "/DataArea/PurchaseOrder/Header/ns0:Parties", namespaceManager);


            foreach (XmlNode xnd in PartiesNodeList)
            {
                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "SAPVendor")
                {
                    XmlNode xSAPVendorNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    lblSAPVendorNumber.Text = xSAPVendorNode == null ? "" : xSAPVendorNode.InnerText;
                    lblSAPVendorNumber.Text = objBC.GetPartnerName(lblSAPVendorNumber.Text);
                }
                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Contract Manufacturer")
                {
                    XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    lblCM.Text = xCMNode == null ? "" : xCMNode.InnerText;
                    lblCM.Text = objBC.GetPartnerName(lblCM.Text);
                }
                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "ShipTo")
                {
                    XmlNode xNameNode = xnd.SelectSingleNode("ns0:ContactInformation/ns0:Name", namespaceManager);
                    lblShipToName.Text = xNameNode == null ? "" : xNameNode.InnerText;
                    XmlNode xPhoneNode = xnd.SelectSingleNode("ns0:ContactInformation/ns0:Phone", namespaceManager);
                    lblShipToPhone.Text = xPhoneNode == null ? "" : xPhoneNode.InnerText;
                    XmlNode xPartnerNameNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    lblShipToPartnername.Text = xPartnerNameNode == null ? "" : xPartnerNameNode.InnerText;
                    lblShipToPartnername.Text = objBC.GetPartnerName(lblShipToPartnername.Text);
                    XmlNode xAddressLine1Node = xnd.SelectSingleNode("ns0:PhysicalAddress/ns0:AddressLine1", namespaceManager);
                    lblShipToAddressLine1.Text = xAddressLine1Node == null ? "" : xAddressLine1Node.InnerText;
                    XmlNode xCityNode = xnd.SelectSingleNode("ns0:PhysicalAddress/ns0:City", namespaceManager);
                    lblShipToCity.Text = xCityNode == null ? "" : xCityNode.InnerText;
                    XmlNode xCountryNode = xnd.SelectSingleNode("ns0:PhysicalAddress/ns0:Country", namespaceManager);
                    lblShipToCountry.Text = xCountryNode == null ? "" : xCountryNode.InnerText;
                    XmlNode xZipNode = xnd.SelectSingleNode("ns0:PhysicalAddress/ns0:Zip", namespaceManager);
                    lblShipToZIP.Text = xZipNode == null ? "" : xZipNode.InnerText;
                    XmlNode xStateNode = xnd.SelectSingleNode("ns0:PhysicalAddress/ns0:State", namespaceManager);
                    lblShipToState.Text = xStateNode == null ? "" : xStateNode.InnerText;
                }

            }

            XmlNode xPOTypeNode = poFile.SelectSingleNode("/ns0:" + RootNode + "/DataArea/PurchaseOrder/Header/Application/SAP/ns0:PurchaseOrderType", namespaceManager);
            lblSAPPOType.Text = xPOTypeNode == null ? "" : xPOTypeNode.InnerText;

            BindPOLines(poFile, namespaceManager, RootNode);
            GenericCollection<PurchaseOrderHistoryBE> POHistryList = new GenericCollection<PurchaseOrderHistoryBE>();
            POHistryList = objBC.BindPOHistory(PoNumber);
            Session["POHistoryCollection"] = POHistryList;
            databind();
            this.ViewState["vs"] = 0;
            databindPOHistory();
            this.ViewState["vsHistory"] = 0;

            trPODetails.Visible = true;
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
            trPODetails.Visible = false;
        }
    }

    private void databindPOHistory()
    {
        try
        {
            GenericCollection<PurchaseOrderHistoryBE> POHistoryCollection = Session["POHistoryCollection"] == null ? null : Session["POHistoryCollection"] as GenericCollection<PurchaseOrderHistoryBE>;
            if (POHistoryCollection != null)
            {
                adsourceHistory = new PagedDataSource();
                adsourceHistory.DataSource = POHistoryCollection;
                adsourceHistory.PageSize = 5;
                ViewState["PageCountHistory"] = (POHistoryCollection.Count % 5) == 0 ? (POHistoryCollection.Count / 5) : (POHistoryCollection.Count / 5) + 1;
                adsourceHistory.AllowPaging = true;
                adsourceHistory.CurrentPageIndex = posHistory;
                btnfirstHistory.Enabled = !adsourceHistory.IsFirstPage;
                btnPreviousHistory.Enabled = !adsourceHistory.IsFirstPage;
                btnlastHistory.Enabled = !adsourceHistory.IsLastPage;
                btnNextHistory.Enabled = !adsourceHistory.IsLastPage;

                gvPOHistory.DataSource = adsourceHistory;
                gvPOHistory.DataBind();
                divPaginationPOHistory.Visible = true;
            }
            else
            {
                gvPOHistory.DataSource = adsourceHistory;
                gvPOHistory.DataBind();
                divPaginationPOHistory.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
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

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = (ViewState["PageCount"] != null ? Convert.ToInt32(ViewState["PageCount"]) : 1) - 1;
        databind();
    }

    private void databind()
    {
        try
        {
            GenericCollection<PurchaseOrderLineBE> POLineCollection = Session["POLineCollection"] == null ? null : Session["POLineCollection"] as GenericCollection<PurchaseOrderLineBE>;
            if (POLineCollection != null)
            {
                adsource = new PagedDataSource();
                adsource.DataSource = POLineCollection;
                adsource.PageSize = 10;
                ViewState["PageCount"] = (POLineCollection.Count % 10) == 0 ? (POLineCollection.Count / 10) : (POLineCollection.Count / 10) + 1;
                adsource.AllowPaging = true;
                adsource.CurrentPageIndex = pos;
                btnfirst.Enabled = !adsource.IsFirstPage;
                btnprevious.Enabled = !adsource.IsFirstPage;
                btnlast.Enabled = !adsource.IsLastPage;
                btnnext.Enabled = !adsource.IsLastPage;

                gvPurchaseOrderLine.DataSource = adsource;
                gvPurchaseOrderLine.DataBind();
                divPaginationPoLine.Visible = true;
                gvPurchaseOrderLine.Visible = true;
            }
            else
            {
                gvPurchaseOrderLine.DataSource = null;
                gvPurchaseOrderLine.DataBind();
                divPaginationPoLine.Visible = false;
                gvPurchaseOrderLine.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }

    private void BindPOLines(XmlDocument poFile, XmlNamespaceManager namespaceManager, string RootNode)
    {
        try
        {
            XmlNodeList POLinesList = poFile.SelectNodes("/ns0:" + RootNode + "/DataArea/PurchaseOrder/Line", namespaceManager);

            GenericCollection<PurchaseOrderLineBE> POLineCollection = new GenericCollection<PurchaseOrderLineBE>();
            int i = 0;
            foreach (XmlNode xnd in POLinesList)
            {
                PurchaseOrderLineBE objBE = new PurchaseOrderLineBE();
                XmlNode xLineNumberNode = xnd.SelectSingleNode("ns0:LineNumber", namespaceManager);
                objBE.POLineNumber = xLineNumberNode == null ? "" : xLineNumberNode.InnerText;
                XmlNodeList xDateNodeList = xnd.SelectNodes("ns0:DocumentDates", namespaceManager);
                if (xDateNodeList.Count > 0)
                {
                    foreach (XmlNode xdatenode in xDateNodeList)
                    {
                        if (xdatenode.SelectSingleNode("ns0:DateTypeCode", namespaceManager).InnerText == "DeliveryDate")
                        {
                            XmlNode xDelivryDateNode = xdatenode.SelectSingleNode("ns0:DateTime", namespaceManager);
                            objBE.DeliveryDate = xDelivryDateNode == null ? "" : xDelivryDateNode.InnerText.Substring(0, 8);
                        }
                        if (xdatenode.SelectSingleNode("ns0:DateTypeCode", namespaceManager).InnerText == "ShipDate")
                        {
                            XmlNode xShipDateNode = xdatenode.SelectSingleNode("ns0:DateTime", namespaceManager);
                            objBE.ShipDate = xShipDateNode == null ? "" : xShipDateNode.InnerText.Substring(0, 8);
                        }
                    }
                }
                XmlNode xUnitPriceNode = xnd.SelectSingleNode("ns0:Price/ns0:Amount", namespaceManager);
                objBE.Price = xUnitPriceNode == null ? "" : xUnitPriceNode.InnerText;
                XmlNodeList xOrderNodeList = xnd.SelectNodes("ns0:OrderItem", namespaceManager);
                foreach (XmlNode xordernode in xOrderNodeList)
                {
                    if (xordernode.SelectSingleNode("ns0:ProductTypeCode", namespaceManager).InnerText == "MSPartNumber")
                    {
                        XmlNode xPartNumberNode = xordernode.SelectSingleNode("ns0:ProductIdentifier", namespaceManager);
                        objBE.Material = xPartNumberNode == null ? "" : xPartNumberNode.InnerText;
                    }
                    if (xordernode.SelectSingleNode("ns0:ProductTypeCode", namespaceManager).InnerText == "UPC")
                    {
                        XmlNode xSerialNumberNode = xordernode.SelectSingleNode("ns0:ProductIdentifier", namespaceManager);
                        objBE.SerialNumber = xSerialNumberNode == null ? "" : xSerialNumberNode.InnerText;
                    }
                }
                XmlNode xOrderQuantityNode = xnd.SelectSingleNode("ns0:OrderQuantity", namespaceManager);
                objBE.OrderQuantity = xOrderQuantityNode == null ? "" : xOrderQuantityNode.InnerText;
                POLineCollection.Add(i, objBE);
                i++;
            }
            Session["POLineCollection"] = POLineCollection;
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvPurchaseOrderLine_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<PurchaseOrderLineBE> listofBE = (GenericCollection<PurchaseOrderLineBE>)Session["POLineCollection"];

            if (listofBE != null)
            {
                gvPurchaseOrderLine.PageIndex = e.NewPageIndex;
                gvPurchaseOrderLine.DataSource = listofBE;
                gvPurchaseOrderLine.DataBind();
                divPaginationPoLine.Visible = true;
                gvPurchaseOrderLine.Visible = true;
            }
            else
            {
                gvPurchaseOrderLine.DataSource = null;
                gvPurchaseOrderLine.DataBind();
                divPaginationPoLine.Visible = false;
                gvPurchaseOrderLine.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvPurchaseOrderLine_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDirLine] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirLine].ToString());
            GenericCollection<PurchaseOrderLineBE> list = Session["POLineCollection"] as GenericCollection<PurchaseOrderLineBE>;
            list = Utils.GridSorting<PurchaseOrderLineBE>(gvPurchaseOrderLine, dir, e.SortExpression, list);
            Session["POLineCollection"] = list;
            ViewState[vSortDirLine] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void gvPOHistory_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDirHistory] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirHistory].ToString());
            GenericCollection<PurchaseOrderHistoryBE> list = Session["POHistoryCollection"] as GenericCollection<PurchaseOrderHistoryBE>;
            list = Utils.GridSorting<PurchaseOrderHistoryBE>(gvPOHistory, dir, e.SortExpression, list);
            Session["POHistoryCollection"] = list;
            ViewState[vSortDirHistory] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void gvPOHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<PurchaseOrderHistoryBE> listofBE = (GenericCollection<PurchaseOrderHistoryBE>)Session["POHistoryCollection"];

            if (listofBE != null)
            {
                gvPOHistory.PageIndex = e.NewPageIndex;
                gvPOHistory.DataSource = listofBE;
                gvPOHistory.DataBind();
                divPaginationPOHistory.Visible = true;
            }
            else
            {
                gvPOHistory.DataSource = null;
                gvPOHistory.DataBind();
                divPaginationPOHistory.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }


    protected void btnfirstHistory_Click(object sender, EventArgs e)
    {
        posHistory = 0;
        databindPOHistory();
    }

    protected void btnPreviousHistory_Click(object sender, EventArgs e)
    {
        posHistory = (int)this.ViewState["vsvsHistory"];
        posHistory -= 1;
        this.ViewState["vsvsHistory"] = posHistory;
        databindPOHistory();
    }

    protected void btnNextHistory_Click(object sender, EventArgs e)
    {
        posHistory = (int)this.ViewState["vsvsHistory"];
        posHistory += 1;
        this.ViewState["vsvsHistory"] = posHistory;
        databindPOHistory();
    }

    protected void btnlastHistory_Click(object sender, EventArgs e)
    {
        posHistory = (ViewState["PageCountvsHistory"] != null ? Convert.ToInt32(ViewState["PageCountvsHistory"]) : 1) - 1;
        databindPOHistory();
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
        PopulateTransactionType("", "", "", "");
        BindRepeater();
        trPODetails.Visible = false;
    }
}

