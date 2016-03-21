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


public partial class Main : System.Web.UI.Page
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
        Page.Title = "All Txns Search";
        if (!IsPostBack)
        {
            lblSearchError.Text = "";
            PopulateServiceLineDropDown();
            PopulateServiceDropDown("");
            PopulateServiceOptionDropDown("", "");
            PopulateServiceComponentDropDown("", "", "", "SCH");
            PopulateServiceComponentDropDown("", "", "", "PARTNERS");
            PopulateTransactionType("", "", "", "");
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

    private void PopulateServiceLineDropDown()
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<ServiceLine> ServiceLineList = new GenericCollection<ServiceLine>();
            ServiceLineList = objBC.GetServiceLineList();
            drdServiceLineSearch.DataTextField = "ServiceLineDesc";
            drdServiceLineSearch.DataValueField = "ServiceLineID";
            drdServiceLineSearch.DataSource = ServiceLineList;
            drdServiceLineSearch.DataBind();
            //if (drdServiceLineSearch != null)
            //{
            //    foreach (ListItem li in drdServiceLineSearch.Items)
            //    {
            //        li.Attributes["title"] = li.Text;
            //    }
            //}
            drdServiceLineSearch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }
    }



    public void PopulateServiceDropDown(string ServiceLineID)
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<Service> ServiceList = new GenericCollection<Service>();
            ServiceList = objBC.GetServiceList(ServiceLineID);
            drdServiceSearch.DataTextField = "ServiceDesc";
            drdServiceSearch.DataValueField = "ServiceID";
            drdServiceSearch.DataSource = ServiceList;
            drdServiceSearch.DataBind();
            //if (drdServiceSearch != null)
            //{
            //    foreach (ListItem li in drdServiceSearch.Items)
            //    {
            //        li.Attributes["title"] = li.Text;
            //    }
            //}
            drdServiceSearch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }

    }

    private void PopulateServiceOptionDropDown(string ServiceLineID, string ServiceID)
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<ServiceOption> ServiceOptionList = new GenericCollection<ServiceOption>();
            ServiceOptionList = objBC.GetServiceOptionList(ServiceLineID, ServiceID);
            drdServiceOptionSearch.DataTextField = "ServiceOptionName";
            drdServiceOptionSearch.DataValueField = "ServiceOptionID";
            drdServiceOptionSearch.DataSource = ServiceOptionList;
            drdServiceOptionSearch.DataBind();
            //if (drdServiceOptionSearch != null)
            //{
            //    foreach (ListItem li in drdServiceOptionSearch.Items)
            //    {
            //        foreach (ServiceOption obj in ServiceOptionList)
            //        {
            //            if (obj.ServiceOptionID.ToString() == li.Value)
            //                li.Attributes["title"] = obj.ServiceOptionBriefDesc;
            //        }
            //    }
            //}
            drdServiceOptionSearch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }
    }

    private void PopulateServiceComponentDropDown(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompType)
    {
        try
        {
            DropDownBC objBC = new DropDownBC();
            GenericCollection<ServiceComponent> ServiceComponentList = new GenericCollection<ServiceComponent>();
            ServiceComponentList = objBC.GetServiceComponentList(ServiceLineID, ServiceID, ServiceOptionID, ServCompType);
            if (ServCompType == "SCH")
            {
                drdServiceComponentAppSearch.DataTextField = "ServiceComponentDesc";
                drdServiceComponentAppSearch.DataValueField = "ServiceComponentID";
                drdServiceComponentAppSearch.DataSource = ServiceComponentList;
                drdServiceComponentAppSearch.DataBind();
                //if (drdServiceComponentAppSearch != null)
                //{
                //    foreach (ListItem li in drdServiceComponentAppSearch.Items)
                //    {
                //        li.Attributes["title"] = li.Text;
                //    }
                //}
                drdServiceComponentAppSearch.Items.Insert(0, new ListItem(String.Empty, String.Empty));
            }
            else
            {
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
            //if (drdTxnType != null)
            //{
            //    foreach (ListItem li in drdTxnType.Items)
            //    {
            //        li.Attributes["title"] = li.Text;
            //    }
            //}
            drdTxnType.Items.Insert(0, new ListItem(String.Empty, String.Empty));
        }
        catch (Exception ex)
        {
            logFile.ErrorLogging(ex);
        }
    }


    protected void drdServiceLineSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateServiceDropDown(drdServiceLineSearch.SelectedValue);
        PopulateServiceOptionDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue);
        PopulateServiceComponentDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, "SCH");
        PopulateServiceComponentDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, "PARTNERS");
    }
    protected void drdServiceSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateServiceOptionDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue);
        PopulateServiceComponentDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, "SCH");
        PopulateServiceComponentDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, "PARTNERS");
    }
    protected void drdServiceOptionSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateServiceComponentDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, "SCH");
        PopulateServiceComponentDropDown(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, "PARTNERS");
    }
    protected void drdServiceComponentSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        PopulateTransactionType(drdServiceLineSearch.SelectedValue, drdServiceSearch.SelectedValue, drdServiceOptionSearch.SelectedValue, drdServiceComponentAppSearch.SelectedValue);
    }

    protected void drdServiceLineSearch_PreRender(object sender, EventArgs e)
    {
        retainDropdownToolTip(drdServiceLineSearch);
    }

    protected void drdServiceSearch_PreRender(object sender, EventArgs e)
    {
        retainDropdownToolTip(drdServiceSearch);
    }

    protected void drdServiceOptionSearch_PreRender(object sender, EventArgs e)
    {
        retainDropdownToolTip(drdServiceOptionSearch);
    }

    protected void drdServiceComponentSearch_PreRender(object sender, EventArgs e)
    {
        retainDropdownToolTip(drdServiceComponentAppSearch);
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
        //foreach (ListItem _listItem in dropdown.Items)
        //{
        //    _listItem.Attributes.Add("title", _listItem.Text);
        //}
        //dropdown.Attributes.Add("onmouseover", "this.title=this.options[this.selectedIndex].title");
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
        TransactionBC objBC = new TransactionBC();
        try
        {
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateFrom.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
            trPODetails.Visible = false;
            AllTransactionBE listOfBE = objBC.GetTransactionList(drdTxnType.SelectedValue, DateFrom, DateTo, drdServiceComponentPartnerSearch.SelectedValue);

            listOfBE = GetTxnDetailsfromExtranet(listOfBE);

            GenericCollection<TransactionsBE> TransactionsList = new GenericCollection<TransactionsBE>();

            GenericCollection<TransactionsBE> POTransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> ASNTransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> Transactions841List = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> GR4B2TransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> GR861TransactionsList = new GenericCollection<TransactionsBE>();

            if (listOfBE != null)
            {
                if (listOfBE.POTransactions != null)
                    POTransactionsList = listOfBE.POTransactions;
                if (listOfBE.ASNTransactions != null)
                    ASNTransactionsList = listOfBE.ASNTransactions;
                if (listOfBE.GR4B2Transactions != null)
                    GR4B2TransactionsList = listOfBE.GR4B2Transactions;
                if (listOfBE.GR861Transactions != null)
                    GR861TransactionsList = listOfBE.GR861Transactions;
                if (listOfBE.Transactions841 != null)
                    Transactions841List = listOfBE.Transactions841;
            }

            if (POTransactionsList.Count > 0)
            {
                int i = 0;
                foreach (TransactionsBE obj in POTransactionsList)
                {
                    TransactionsList.Add(i, obj);
                    i++;
                }
            }

            if (ASNTransactionsList.Count > 0)
            {
                int i = 0;
                foreach (TransactionsBE obj in ASNTransactionsList)
                {
                    TransactionsList.Add(i, obj);
                    i++;
                }
            }

            if (GR4B2TransactionsList.Count > 0)
            {
                int i = 0;
                foreach (TransactionsBE obj in GR4B2TransactionsList)
                {
                    TransactionsList.Add(i, obj);
                    i++;
                }
            }

            if (GR861TransactionsList.Count > 0)
            {
                int i = 0;
                foreach (TransactionsBE obj in GR861TransactionsList)
                {
                    TransactionsList.Add(i, obj);
                    i++;
                }
            }

            if (Transactions841List.Count > 0)
            {
                int i = 0;
                foreach (TransactionsBE obj in Transactions841List)
                {
                    TransactionsList.Add(i, obj);
                    i++;
                }
            }


            GenericCollection<TransactionsBE>.GenericComparer comp = new GenericCollection<TransactionsBE>.GenericComparer("TxnDate", SortDirection.Descending);
            TransactionsList.Sort(comp);

            //listOfBE = GetASNStatus(listOfBE);


            if (ViewState[grdPageSize] != null)
            {
                gvPurchaseOrder.PageSize = Convert.ToInt32(ViewState[grdPageSize].ToString());
            }
            else
            {
                gvPurchaseOrder.PageSize = 10;
            }


            lblRec.Text = Utils.GridRecDispMsg(gvPurchaseOrder.PageIndex, gvPurchaseOrder.PageSize, TransactionsList.Count);
            gvPurchaseOrder.Visible = true;
            if (TransactionsList != null && TransactionsList.Count > 0)
            {

                rowPage.Visible = true;
                rowGrid.Visible = true;

                gvPurchaseOrder.DataSource = TransactionsList;
                gvPurchaseOrder.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvPurchaseOrder.PageIndex, gvPurchaseOrder.PageSize, TransactionsList.Count);
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
            Session[vPOList] = TransactionsList;

        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }

    private AllTransactionBE GetTxnDetailsfromExtranet(AllTransactionBE listOfBE)
    {
        TransactionBC objBC = new TransactionBC();
        AllTransactionBE ExtTransactionsList = new AllTransactionBE();

        GenericCollection<TransactionsBE> POTransactionsList = new GenericCollection<TransactionsBE>();
        GenericCollection<TransactionsBE> GRTransactionsList = new GenericCollection<TransactionsBE>();

        GenericCollection<TransactionsBE> POExtTransactionsList = new GenericCollection<TransactionsBE>();
        GenericCollection<TransactionsBE> GRExtTransactionsList = new GenericCollection<TransactionsBE>();
        GenericCollection<TransactionsBE> Transactions841List = new GenericCollection<TransactionsBE>();

        if (listOfBE != null)
        {
            if (listOfBE.POTransactions != null)
                POTransactionsList = listOfBE.POTransactions;
            if (listOfBE.GR861Transactions != null)
                GRTransactionsList = listOfBE.GR861Transactions;
        }

        if (POTransactionsList != null || drdTxnType.SelectedValue == "841" || GRTransactionsList != null)
        {
            string POList = "";
            string ControlNumberList = "";
            string ExtranetPartners = ConfigurationSettings.AppSettings["ExtranetCMS"];

            foreach (TransactionsBE objBE in POTransactionsList)
            {
                if (ExtranetPartners.Contains(objBE.CM) && objBE.TxnType != "4B2")
                {
                    POList = POList == "" ? objBE.PONumber : POList + "," + objBE.PONumber;
                    objBE.IsICOE = "NO";
                }
                else
                    objBE.IsICOE = "YES";
            }

            //foreach (TransactionsBE objBE in GRTransactionsList)
            //{
            //    if (objBE.TxnType == "861")
            //        ControlNumberList += objBE.LoadID + ":" + objBE.DUNS + @"','";
            //}


            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);

            ExtTransactionsList = objBC.GetTxnDetailsfromExtranet(POList, ControlNumberList, drdTxnType.SelectedValue, drdServiceComponentPartnerSearch.SelectedValue, DateFrom, DateTo);

            if (ExtTransactionsList != null)
            {
                if (ExtTransactionsList.POTransactions != null)
                {
                    POExtTransactionsList = ExtTransactionsList.POTransactions;
                    foreach (TransactionsBE obj in POTransactionsList)
                    {
                        foreach (TransactionsBE objExt in POExtTransactionsList)
                        {
                            if (objExt.PONumber == obj.TransactionID)
                            {
                                obj.TransactionID = objExt.TransactionID;
                                obj.Status = objExt.Status;
                                if (objExt.StageName == "Error_NotRcvdConfirmBODFrmPartnerEx")
                                    obj.POCBODRcvd = "NoBOD";
                                if (objExt.StageName == "PositiveConfirmBODReceived")
                                    obj.POCBODRcvd = "PosBOD";
                                if (objExt.StageName == "NegativeConfirmBODReceived")
                                    obj.POCBODRcvd = "NegBOD";
                            }
                        }
                    }
                }
                if (ExtTransactionsList.GR861Transactions != null)
                {
                    GRExtTransactionsList = ExtTransactionsList.GR861Transactions;
                    foreach (TransactionsBE objExt in GRExtTransactionsList)
                    {
                        string TxnID = objExt.TransactionID;
                        string DUNS = objExt.DUNS;
                        string LoadID = objExt.LoadID;
                        foreach (TransactionsBE objExt1 in GRExtTransactionsList)
                        {
                            if (objExt1.LoadID == LoadID && objExt1.DUNS == DUNS && objExt1.TransactionID == TxnID)
                            {
                                if (objExt1.StageName == "SentReceiveInventoryToCorpNet")
                                    objExt.GRExtStatus = "Success";
                                if (objExt1.StageName.ToLower().Contains("error") && objExt1.StageName != "Error_RcvdNegativeMDNFromPartnerEx")
                                    objExt.GRExtStatus = "Failed";
                                if (objExt1.StageName == "SentPositive997ToPartner")
                                    objExt.FunctionalAck = "pos997";
                                if (objExt1.StageName == "SentNegative997ToPartner")
                                    objExt.FunctionalAck = "neg997";
                                if (objExt1.StageName == "SentPositive824ToPartner")
                                    objExt.FunctionalAck = "pos824";
                                if (objExt1.StageName == "SentNegative824ToPartner")
                                    objExt.FunctionalAck = "Neg824";
                                if (objExt1.StageName == "MDNNotReceivedFromPartner")
                                    objExt.MDNForFuncAck = "NoMDN";
                                if (objExt1.StageName == "RcvdPositiveMDNFromPartner")
                                    objExt.MDNForFuncAck = "posMDN";
                                if (objExt1.StageName == "Error_RcvdNegativeMDNFromPartnerEx")
                                    objExt.MDNForFuncAck = "NegMDN";
                                if (objExt1.StageName == "SendPositive824for861ToBatchPrimaryTransPortEx")
                                    objExt.FunctionalAck = "Pos824Batch";
                                if (objExt1.StageName == "SendNegative824for861ToBatchPrimaryTransPortEx")
                                    objExt.FunctionalAck = "Neg824Batch";
                            }
                        }
                    }
                    foreach (TransactionsBE obj in GRTransactionsList)
                    {
                        if (obj.IsICOE == "NO")
                        {
                            foreach (TransactionsBE objExt in GRExtTransactionsList)
                            {
                                if (objExt.TransactionID == obj.ControlNumber && obj.DUNS == objExt.DUNS && obj.LoadID == objExt.LoadID)
                                {
                                    obj.PONumber = obj.TransactionID;
                                    obj.TransactionID = objExt.TransactionID;
                                    obj.CM = objExt.CM;
                                    obj.Status = objExt.GRExtStatus;
                                    obj.FunctionalAck = objExt.FunctionalAck;
                                    obj.MDNForFuncAck = objExt.MDNForFuncAck;
                                    if (obj.Status == "Success" && String.IsNullOrEmpty(obj.FunctionalAck) && string.IsNullOrEmpty(obj.MDNForFuncAck))
                                        obj.NoFuncAck = "TRUE";
                                    else
                                        obj.NoFuncAck = "FALSE";

                                }
                            }
                        }
                    }
                }
                if (ExtTransactionsList.Transactions841 != null)
                {
                    foreach (TransactionsBE objExt1 in ExtTransactionsList.Transactions841)
                    {
                        if ((objExt1.StageName.ToLower().Contains("error") || objExt1.StageName.ToLower().Contains("failed")) && (objExt1.StageName != "LogError_NegativeMDNReceived" || objExt1.StageName != "LogError_MDN_NotReceived"))
                            objExt1.Status = "Failed";
                        else
                            objExt1.Status = "Success";
                        if (objExt1.StageName == "SentPositive997ToPartner")
                            objExt1.FunctionalAck = "pos997";
                        if (objExt1.StageName == "SentNegative997ToPartner")
                            objExt1.FunctionalAck = "neg997";
                        if (objExt1.StageName == "SentPositiveEDI824ToPartner")
                            objExt1.FunctionalAck = "pos824";
                        if (objExt1.StageName == "SentNegativeEDI824ToPartner")
                            objExt1.FunctionalAck = "Neg824";
                        if (objExt1.StageName == "LogError_MDN_NotReceived")
                            objExt1.MDNForFuncAck = "NoMDN";
                        if (objExt1.StageName == "PositiveMDNReceived")
                            objExt1.MDNForFuncAck = "posMDN";
                        if (objExt1.StageName == "LogError_NegativeMDNReceived")
                            objExt1.MDNForFuncAck = "NegMDN";
                        if (objExt1.StageName == "SendPositiveEDI824for841ToBatchPrimaryTransport")
                            objExt1.FunctionalAck = "Pos824Batch";
                        if (objExt1.StageName == "SendNegativeEDI824for841ToBatchPrimaryTransport")
                            objExt1.FunctionalAck = "Neg824Batch";
                    }
                    for (int j = 0; j < ExtTransactionsList.Transactions841.Count; j++)
                    {
                        if (Transactions841List == null)
                        {
                            Transactions841List.Add(j, ExtTransactionsList.Transactions841[j]);
                        }
                        else
                        {
                            bool duplicate = false;
                            for (int k = 0; k < Transactions841List.Count; k++)
                            {
                                if (ExtTransactionsList.Transactions841[j].ControlNumber == Transactions841List[k].ControlNumber && ExtTransactionsList.Transactions841[j].CM == Transactions841List[k].CM && ExtTransactionsList.Transactions841[j].TxnDate == Transactions841List[k].TxnDate)
                                {
                                    Transactions841List[k].Status = Transactions841List[k].Status == "" ? ExtTransactionsList.Transactions841[j].Status : Transactions841List[k].Status;
                                    Transactions841List[k].FunctionalAck = Transactions841List[k].FunctionalAck == "" ? ExtTransactionsList.Transactions841[j].FunctionalAck : Transactions841List[k].FunctionalAck;
                                    Transactions841List[k].MDNForFuncAck = Transactions841List[k].MDNForFuncAck == "" ? ExtTransactionsList.Transactions841[j].MDNForFuncAck : Transactions841List[k].MDNForFuncAck;
                                    duplicate = true;
                                }
                            }
                            if (!duplicate)
                                Transactions841List.Add(Transactions841List.Count, ExtTransactionsList.Transactions841[j]);
                        }
                    }
                    listOfBE.Transactions841 = ExtTransactionsList.Transactions841;
                }
            }
        }
        return listOfBE;
    }
    protected void gvPurchaseOrder_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            lblSearchError.Text = string.Empty;
            GenericCollection<TransactionsBE> listofBE = (GenericCollection<TransactionsBE>)Session[vPOList];

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
            GenericCollection<TransactionsBE> list = Session[vPOList] as GenericCollection<TransactionsBE>;
            list = Utils.GridSorting<TransactionsBE>(gvPurchaseOrder, dir, e.SortExpression, list);
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
            GenericCollection<TransactionsBE> listofBE = (GenericCollection<TransactionsBE>)Session[vPOList];

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
                Label lblASNStatus = (Label)e.Row.FindControl("lblASNStatus");
                Label lblTransactionID = (Label)e.Row.FindControl("lblTransactionID");
                Label LabelTxnType = (Label)e.Row.FindControl("LabelTxnType");
                Label lblIsICOE = (Label)e.Row.FindControl("lblICOE");
                Label LabelPOCBODRcvd = (Label)e.Row.FindControl("LabelPOCBODRcvd");
                Label LabelFunctionalAck = (Label)e.Row.FindControl("LabelFunctionalAck");
                Label lblMDNForFuncAck = (Label)e.Row.FindControl("lblMDNForFuncAck");
                Label lblPoNumber = (Label)e.Row.FindControl("lblPoNumber");
                Label LabelLoadID = (Label)e.Row.FindControl("LabelLoadID");
                Label lblNoFuncAck = (Label)e.Row.FindControl("lblNoFuncAck");

                Image imgCorpStatus = (Image)e.Row.FindControl("imgCorpStatus");
                Image imgExtStatus = (Image)e.Row.FindControl("imgExtStatus");
                Image imgASNStatus = (Image)e.Row.FindControl("imgASNStatus");
                Image imgAckStatus = (Image)e.Row.FindControl("imgAckStatus");
                Image imgMDNRcvStatus = (Image)e.Row.FindControl("imgMDNRcvStatus");
                Image imgCBODStatus = (Image)e.Row.FindControl("imgCBODStatus");


                if (LabelTxnType.Text.ToLower() == "850" || LabelTxnType.Text == "3A4")
                {
                    if (lblCorpnetStatus.Text == "Success")
                    {
                        imgCorpStatus.ImageUrl = "~/Images/Success.png";
                        imgCorpStatus.AlternateText = "Success";
                    }
                    else if (lblCorpnetStatus.Text == "Failed")
                    {
                        imgCorpStatus.ImageUrl = "~/Images/Failed.png";
                        imgCorpStatus.AlternateText = "Failed";
                    }
                    else
                    {
                        imgCorpStatus.ImageUrl = "~/Images/NA.png";
                        imgCorpStatus.AlternateText = "Not Applicable";
                    }

                    if (lblIsICOE.Text.ToLower() == "no")
                    {
                        if (lblExtStatus.Text == "Success")
                        {
                            imgExtStatus.ImageUrl = "~/Images/Success.png";
                            imgExtStatus.AlternateText = "Success";
                        }
                        else
                        {
                            imgExtStatus.ImageUrl = "~/Images/Failed.png";
                            imgExtStatus.AlternateText = "Failed";
                        }

                        if (LabelPOCBODRcvd.Text == "NoBOD")
                        {
                            imgCBODStatus.ImageUrl = "~/Images/NoBOD.png";
                            imgCBODStatus.AlternateText = "Not Received";
                        }
                        else if (LabelPOCBODRcvd.Text == "PosBOD")
                        {
                            imgCBODStatus.ImageUrl = "~/Images/PosBOD.png";
                            imgCBODStatus.AlternateText = "Positive BOD";
                        }
                        else
                        {
                            imgCBODStatus.ImageUrl = "~/Images/NegBOD.png";
                            imgCBODStatus.AlternateText = "Positive BOD";
                        }
                    }
                    else
                    {
                        imgExtStatus.ImageUrl = "~/Images/NA.png";
                        imgExtStatus.AlternateText = "Not Applicable";
                        imgCBODStatus.ImageUrl = "~/Images/NA.png";
                        imgCBODStatus.AlternateText = "Not Applicable";
                    }

                    imgASNStatus.ImageUrl = "~/Images/NA.png";
                    imgASNStatus.AlternateText = "Not Applicable";
                    imgAckStatus.ImageUrl = "~/Images/NA.png";
                    imgAckStatus.AlternateText = "Not Applicable";
                    imgMDNRcvStatus.ImageUrl = "~/Images/NA.png";
                    imgMDNRcvStatus.AlternateText = "Not Applicable";
                }

                if (LabelTxnType.Text == "3B2")
                {
                    if (lblASNStatus.Text == "Success")
                    {
                        imgASNStatus.ImageUrl = "~/Images/Success.png";
                        imgASNStatus.AlternateText = "Success";
                    }
                    else if (lblASNStatus.Text == "Failed")
                    {
                        imgASNStatus.ImageUrl = "~/Images/Failed.png";
                        imgASNStatus.AlternateText = "Failed";
                    }
                    else
                    {
                        imgASNStatus.ImageUrl = "~/Images/NA.png";
                        imgASNStatus.AlternateText = "Not Applicable";
                    }


                    imgExtStatus.ImageUrl = "~/Images/NA.png";
                    imgExtStatus.AlternateText = "Not Applicable";
                    imgAckStatus.ImageUrl = "~/Images/NA.png";
                    imgAckStatus.AlternateText = "Not Applicable";
                    imgMDNRcvStatus.ImageUrl = "~/Images/NA.png";
                    imgMDNRcvStatus.AlternateText = "Not Applicable";
                    imgCBODStatus.ImageUrl = "~/Images/NA.png";
                    imgCBODStatus.AlternateText = "Not Applicable";


                }

                if (LabelTxnType.Text  == "4B2")
                {
                    if (lblCorpnetStatus.Text == "Success")
                    {
                        imgCorpStatus.ImageUrl = "~/Images/Success.png";
                        imgCorpStatus.AlternateText = "Success";
                    }
                    else if (lblCorpnetStatus.Text == "Failed")
                    {
                        imgCorpStatus.ImageUrl = "~/Images/Failed.png";
                        imgCorpStatus.AlternateText = "Failed";
                    }
                    else
                    {
                        imgCorpStatus.ImageUrl = "~/Images/NA.png";
                        imgCorpStatus.AlternateText = "Not Applicable";
                    }

                    imgExtStatus.ImageUrl = "~/Images/NA.png";
                    imgExtStatus.AlternateText = "Not Applicable";
                    imgAckStatus.ImageUrl = "~/Images/NA.png";
                    imgAckStatus.AlternateText = "Not Applicable";
                    imgMDNRcvStatus.ImageUrl = "~/Images/NA.png";
                    imgMDNRcvStatus.AlternateText = "Not Applicable";
                    imgCBODStatus.ImageUrl = "~/Images/NA.png";
                    imgCBODStatus.AlternateText = "Not Applicable";
                }

                if (LabelTxnType.Text.ToLower() == "861")
                {
                    if (lblCorpnetStatus.Text == "Success")
                    {
                        imgCorpStatus.ImageUrl = "~/Images/Success.png";
                        imgCorpStatus.AlternateText = "Success";
                    }
                    else if (lblCorpnetStatus.Text == "Failed")
                    {
                        imgCorpStatus.ImageUrl = "~/Images/Failed.png";
                        imgCorpStatus.AlternateText = "Failed";
                    }
                    else
                    {
                        imgCorpStatus.ImageUrl = "~/Images/NA.png";
                        imgCorpStatus.AlternateText = "Not Applicable";
                    }

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
                        imgExtStatus.ImageUrl = "~/Images/NA.png";
                        imgExtStatus.AlternateText = "Not Applicable";
                    }
                    if (lblNoFuncAck.Text == "NO")
                    {
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
                    else
                    {
                        imgAckStatus.ImageUrl = "~/Images/NA.png";
                        imgAckStatus.AlternateText = "Not Applicable";
                        imgMDNRcvStatus.ImageUrl = "~/Images/NA.png";
                        imgMDNRcvStatus.AlternateText = "Not Applicable";
                    }

                }

                if (LabelTxnType.Text.ToLower() == "841")
                {
                    imgCorpStatus.ImageUrl = "~/Images/NA.png";
                    imgCorpStatus.AlternateText = "Not Applicable";

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
                        imgExtStatus.ImageUrl = "~/Images/NA.png";
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
                //ImageButton lb = (ImageButton)e.CommandSource;
                //GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                //GridView gridview = gvr.NamingContainer as GridView;
                //int index = Convert.ToInt32(e.CommandArgument);
                //GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                //Label lblPoNumber = (Label)selectedRow.FindControl("lblPoNumber");

                //LoadPODetails(lblPoNumber.Text);
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void LoadPODetails(string PONumber)
    {
        try
        {
            PurchaseOrderBC objBC = new PurchaseOrderBC();
            string ArchiveFile = objBC.GetPOArchiveFile(PONumber);

            XmlDocument poFile = new XmlDocument();
            poFile.Load(@"D:\BPM\SampleFiles\Liteon.xml");
            string RootNode = "";
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(poFile.NameTable);
            if (ArchiveFile.Split(',')[1].ToLower() == "http://ms.it.ops.cm.processpurchaseorder_v02_10_00#processpurchaseorder_v02_10_00")
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
            POHistryList = objBC.BindPOHistory(PONumber);
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
                adsource.PageSize = 5;
                ViewState["PageCount"] = (POLineCollection.Count % 5) == 0 ? (POLineCollection.Count / 5) : (POLineCollection.Count / 5) + 1;
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
        PopulateServiceLineDropDown();
        PopulateServiceDropDown("");
        PopulateServiceOptionDropDown("", "");
        PopulateServiceComponentDropDown("", "", "", "SCH");
        PopulateServiceComponentDropDown("", "", "", "PARTNERS");
        PopulateTransactionType("", "", "", "");
        BindRepeater();
        trPODetails.Visible = false;

    }
}