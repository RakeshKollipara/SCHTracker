﻿using System;
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

public partial class ASN : System.Web.UI.Page
{
    int pos;
    PagedDataSource adsource;
    int posHistory;
    PagedDataSource adsourceHistory;
    private const string vASNList = "vASNList";
    private const string vSortDir = "sortDirection";
    private const string vSortDirLine = "sortDirectionLine";
    private const string grdPageSize = "grdPazeSize";
    LogHelper logFile = new LogHelper();
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "ASN Search";
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
        lblSearchError.Visible = false;
        Session[vASNList] = null;
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
        trPODetails.Visible = false;
        gvASN.DataSource = null;
        gvASN.DataBind();
        rowGrid.Visible = false;
        lblSearchError.Visible = false;
        BindRepeater();
        BindPurchaseOrder();
    }

    private void BindPurchaseOrder()
    {
        TransactionBC objBC = new TransactionBC();
        try
        {
            trPODetails.Visible = false;
            DateTime? DateFrom = txtDateFrom.Text == "" ? Convert.ToDateTime("1/1/1990") : Convert.ToDateTime(txtDateFrom.Text);
            DateTime? DateTo = txtDateTo.Text == "" ? Convert.ToDateTime("12/31/2050") : Convert.ToDateTime(txtDateTo.Text);
            GenericCollection<ASNHeaderBE> listOfBE = objBC.GetASNArchiveFiles(DateFrom, DateTo);
            GenericCollection<ASNHeaderBE> DisplayList = new GenericCollection<ASNHeaderBE>();



            //for (int i = 0; i < listOfBE.Count; i++)
            //{
            //    for (int j = 0; j < listOfBE.Count; j++)
            //    {
            //        TimeSpan? duration = listOfBE[i].TxnDate.Value - listOfBE[j].TxnDate.Value;

            //        if (duration.Value.Duration().TotalSeconds < 20)
            //            listOfBE[j].TxnDate = listOfBE[i].TxnDate;
            //    }
            //}

            //for (int j = 0; j < listOfBE.Count; j++)
            //{
            //    if (DisplayList.Count == 0)
            //    {
            //        DisplayList.Add(j, listOfBE[j]);
            //    }
            //    else
            //    {
            //        bool duplicate = false;
            //        for (int k = 0; k < DisplayList.Count; k++)
            //        {
            //            if (listOfBE[j].TxnDate == DisplayList[k].TxnDate)
            //                duplicate = true;
            //        }
            //        if (!duplicate)
            //            DisplayList.Add(DisplayList.Count, listOfBE[j]);
            //    }
            //}

            //if (DisplayList.Count > 0)
            //{
            //    listOfBE = DisplayList;
            //    DisplayList = null;
            //}

            listOfBE = PrepareASNList(listOfBE);

            for (int i = 0; i < listOfBE.Count; i++)
            {
                for (int j = 0; j < listOfBE.Count; j++)
                {
                    if (listOfBE[i].LoadID == listOfBE[j].LoadID && listOfBE[i].PONumber == listOfBE[j].PONumber && listOfBE[j].ReceiverKey != "HEDBTS")
                        listOfBE[i].FMVCopy = listOfBE[j].ArchiveFile;
                }
            }


            for (int j = 0; j < listOfBE.Count; j++)
            {
                if (DisplayList == null)
                {
                    if (listOfBE[j].ReceiverKey == "HEDBTS")
                        DisplayList.Add(j, listOfBE[j]);
                }
                else
                {
                    bool duplicate = false;
                    for (int k = 0; k < DisplayList.Count; k++)
                    {
                        if ((listOfBE[j].ReceiverKey == "HEDBTS" && listOfBE[j].LoadID == DisplayList[k].LoadID && listOfBE[j].PONumber == DisplayList[k].PONumber) || listOfBE[j].ReceiverKey != "HEDBTS")
                            duplicate = true;
                    }
                    if (!duplicate)
                        DisplayList.Add(DisplayList.Count, listOfBE[j]);
                }
            }
            if (DisplayList.Count > 0)
            {
                listOfBE = DisplayList;
                DisplayList = null;
            }

            if (drdServiceComponentPartnerSearch.SelectedIndex != 0)
            {
                for (int j = 0; j < listOfBE.Count; j++)
                {
                    if (listOfBE[j].CM != drdServiceComponentPartnerSearch.SelectedValue)
                    {
                        DisplayList.Add(j, listOfBE[j]);
                    }
                }
                if (DisplayList.Count > 0)
                {
                    listOfBE = DisplayList;
                    DisplayList = null;
                }
            }


            if (txtLoadID.Text != "")
            {
                for (int j = 0; j < listOfBE.Count; j++)
                {
                    if (listOfBE[j].LoadID.ToLower() != txtLoadID.Text.Trim().ToLower())
                    {
                        DisplayList.Add(j, listOfBE[j]);
                    }
                }
                if (DisplayList.Count > 0)
                {
                    listOfBE = DisplayList;
                    DisplayList = null;
                }
            }


            if (txtPONumber.Text != "")
            {
                for (int j = 0; j < listOfBE.Count; j++)
                {
                    if (listOfBE[j].PONumber.ToLower() != txtPONumber.Text.Trim().ToLower())
                    {
                        DisplayList.Add(j, listOfBE[j]);
                    }
                }
                if (DisplayList.Count > 0)
                {
                    listOfBE = DisplayList;
                    DisplayList = null;
                }
            }
            Session[vASNList] = listOfBE;



            if (ViewState[grdPageSize] != null)
            {
                gvASN.PageSize = Convert.ToInt32(ViewState[grdPageSize].ToString());
            }
            else
            {
                gvASN.PageSize = 10;
            }


            lblRec.Text = Utils.GridRecDispMsg(gvASN.PageIndex, gvASN.PageSize, listOfBE.Count);
            gvASN.Visible = true;
            if (listOfBE != null && listOfBE.Count > 0)
            {

                rowPage.Visible = true;
                rowGrid.Visible = true;

                gvASN.DataSource = listOfBE;
                gvASN.DataBind();
                lblRec.Text = Utils.GridRecDispMsg(gvASN.PageIndex, gvASN.PageSize, listOfBE.Count);
                gvASN.Visible = true;
                lblRec.Visible = true;
            }
            else
            {
                lblRec.Visible = false;
                rowGrid.Visible = false;
                rowPage.Visible = false;


                gvASN.Visible = false;
                gvASN.DataSource = null;
                gvASN.DataBind();
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

    private GenericCollection<ASNHeaderBE> PrepareASNList(GenericCollection<ASNHeaderBE> listOfBE)
    {
        TransactionBC objTranBC = new TransactionBC();
        PurchaseOrderBC objBC = new PurchaseOrderBC();
        GenericCollection<ASNHeaderBE> ASNList = new GenericCollection<ASNHeaderBE>();
        for (int i = 0; i < listOfBE.Count; i++)
        {
            ASNHeaderBE objBE = new ASNHeaderBE();
            objBE.SNo = i;
            objBE.TxnType = "3B2";
            objBE.TxnDate = listOfBE[i].TxnDate;
            //string File = listOfBE[0].ArchiveFile;
            XmlDocument ASNFile = new XmlDocument();
            //ASNFile.Load(@"D:\BPM\SampleFiles\ASNEGLFiles.xml");
            ASNFile.Load(listOfBE[i].ArchiveFile);
            objBE.ArchiveFile = listOfBE[i].ArchiveFile;
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(ASNFile.NameTable);
            namespaceManager.AddNamespace("ns0", "http://MS.IT.Ops.HED.ShowShipment_V02_00_00");

            XmlNode xPartnerNode = ASNFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/ApplicationArea/Sender/ns0:LogicalId", namespaceManager);
            objBE.CM = xPartnerNode == null ? "" : objBC.GetPartnerName(xPartnerNode.InnerText);
            objBE.CM = objBE.CM == "" ? xPartnerNode.InnerText : objBE.CM;

            XmlNode xASTransactionID = ASNFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/ApplicationArea/ns0:ReferenceId", namespaceManager);
            objBE.ASFeedTxnID = xASTransactionID == null ? "" : xASTransactionID.InnerText;

            XmlNode xReceiverKey = ASNFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/routing/ReceiverKey", namespaceManager);
            objBE.ReceiverKey = xReceiverKey == null ? "" : xReceiverKey.InnerText;

            XmlNode xShipDate = ASNFile.SelectSingleNode("ns0:ShowShipment_V02_00_00/DataArea/Shipment/Header/TransportationEvent/ns0:DateTime", namespaceManager);
            if (xShipDate != null)
            {
                try
                {
                    objBE.ShipDate = Convert.ToDateTime(xShipDate.InnerText.Substring(0, 4) + "-" + xShipDate.InnerText.Substring(4, 2) + "-" + xShipDate.InnerText.Substring(6, 2));
                }
                catch (Exception ex)
                {

                }
            }

            XmlNodeList PartiesNodeList = ASNFile.SelectNodes("ns0:ShowShipment_V02_00_00/DataArea/Shipment/Header/Parties", namespaceManager);
            XmlNodeList TrackingReferenceNodes = ASNFile.SelectNodes("ns0:ShowShipment_V02_00_00/DataArea/Shipment/Header/TrackingReference", namespaceManager);
            XmlNodeList LineItemNodes = ASNFile.SelectNodes("ns0:ShowShipment_V02_00_00/DataArea/Shipment/LineItem", namespaceManager);

            if (LineItemNodes != null)
                objBE.LineCount = LineItemNodes.Count;


            foreach (XmlNode xnd in PartiesNodeList)
            {
                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Originating Party")
                {
                    XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    objBE.OriginationParty = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
                    objBE.OriginationParty = objBE.OriginationParty == "" ? xCMNode.InnerText : objBE.OriginationParty;
                }
                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Selling Partner")
                {
                    XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    objBE.SellingPartner = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
                    objBE.SellingPartner = objBE.SellingPartner == "" ? xCMNode.InnerText : objBE.SellingPartner;
                }
                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Ship To")
                {
                    XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    objBE.SAPShipTo = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
                    objBE.SAPShipTo = objBE.SAPShipTo == "" ? xCMNode.InnerText : objBE.SAPShipTo;
                }

                if (xnd.SelectSingleNode("ns0:PartyTypeCode", namespaceManager).InnerText == "Freight Forwarder")
                {
                    XmlNode xCMNode = xnd.SelectSingleNode("ns0:PartyIdentifier", namespaceManager);
                    objBE.FMV = xCMNode == null ? "" : objBC.GetPartnerName(xCMNode.InnerText);
                    objBE.FMV = objBE.FMV == "" ? xCMNode.InnerText : objBE.FMV;
                }
            }
            foreach (XmlNode xnd in TrackingReferenceNodes)
            {
                if (xnd.SelectSingleNode("ns0:TrackingCode", namespaceManager).InnerText == "Load Identifier")
                {
                    XmlNode xCMNode = xnd.SelectSingleNode("ns0:TrackingId", namespaceManager);
                    objBE.LoadID = xCMNode == null ? "" : xCMNode.InnerText.Substring(0, xCMNode.InnerText.Length - 2);
                }
            }
            GenericCollection<ASNLinesBE> objLinesList = new GenericCollection<ASNLinesBE>();
            int j = 0;
            foreach (XmlNode xnd in LineItemNodes)
            {
                ASNLinesBE objLineBE = new ASNLinesBE();
                XmlNodeList LineDocRefNodes = ASNFile.SelectNodes("/ns0:ShowShipment_V02_00_00/DataArea/Shipment/LineItem/DocumentReference", namespaceManager);
                foreach (XmlNode xndDoc in LineDocRefNodes)
                {
                    if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Packing Slip Number")
                    {
                        XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
                        objLineBE.PackSlipNumber = xPartnerNode == null ? "" : xCMNode.InnerText;
                    }
                    if (xndDoc.SelectSingleNode("ns0:DocumentTypeCode", namespaceManager).InnerText == "Purchase Order")
                    {
                        XmlNode xCMNode = xndDoc.SelectSingleNode("ns0:Id", namespaceManager);
                        objLineBE.PONumber = xPartnerNode == null ? "" : xCMNode.InnerText;
                        XmlNode xLineNumber = xndDoc.SelectSingleNode("ns0:LineNumber", namespaceManager);
                        objLineBE.LineNumber = xLineNumber == null ? "" : xLineNumber.InnerText;
                    }
                }
                XmlNode xSKUNode = xnd.SelectSingleNode("OrderItem/ns0:ProductIdentifier", namespaceManager);
                objLineBE.SKU = xSKUNode == null ? "" : xSKUNode.InnerText;
                XmlNode xUnitsShippedNode = xnd.SelectSingleNode("ns0:UnitsShipped", namespaceManager);
                objLineBE.ItemQuantity = xUnitsShippedNode == null ? "" : xUnitsShippedNode.InnerText;
                objLinesList.Add(j, objLineBE);
                j++;
            }
            objBE.ASNLines = objLinesList;
            if (objBE.LineCount == 1 && objLinesList != null)
                objBE.PONumber = objLinesList[0].PONumber;
            else
                objBE.PONumber = "See Details";
            ASNList.Add(i, objBE);
        }
        return ASNList;
    }

    protected void gvASN_RowCommand(object sender, GridViewCommandEventArgs e)
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
                GenericCollection<ASNHeaderBE> listOfBE = (GenericCollection<ASNHeaderBE>)Session[vASNList];
                GenericCollection<ASNLinesBE> lines = new GenericCollection<ASNLinesBE>();
                ASNHeaderBE Headerobj = new ASNHeaderBE();

                foreach (ASNHeaderBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }

                lines = Headerobj.ASNLines;
                trPODetails.Visible = false;
                Session["ASNLineCollection"] = lines;
                databind();
            }
            if (e.CommandName == "ViewFiles")
            {
                ImageButton lb = (ImageButton)e.CommandSource;
                GridViewRow gvr = (GridViewRow)lb.NamingContainer;
                GridView gridview = gvr.NamingContainer as GridView;
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow selectedRow = gridview.Rows[index - (gridview.PageIndex * gridview.PageSize)];

                Label lblSNo = (Label)selectedRow.FindControl("lblSNo");
                GenericCollection<ASNHeaderBE> listOfBE = (GenericCollection<ASNHeaderBE>)Session[vASNList];
                ASNHeaderBE Headerobj = new ASNHeaderBE();

                foreach (ASNHeaderBE obj in listOfBE)
                {
                    if (obj.SNo.ToString() == lblSNo.Text)
                    {
                        Headerobj = obj;
                        break;
                    }
                }

                Session["ArchiveFiles"] = Headerobj;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "ShowNewPage();", true);
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

    protected void btnlast_Click(object sender, EventArgs e)
    {
        pos = (ViewState["PageCount"] != null ? Convert.ToInt32(ViewState["PageCount"]) : 1) - 1;
        databind();
    }

    private void databind()
    {
        try
        {
            GenericCollection<ASNLinesBE> ASNLineCollection = Session["ASNLineCollection"] == null ? null : Session["ASNLineCollection"] as GenericCollection<ASNLinesBE>;
            if (ASNLineCollection != null)
            {
                adsource = new PagedDataSource();
                adsource.DataSource = ASNLineCollection;
                adsource.PageSize = 10;
                ViewState["PageCount"] = (ASNLineCollection.Count % 10) == 0 ? (ASNLineCollection.Count / 10) : (ASNLineCollection.Count / 10) + 1;
                adsource.AllowPaging = true;
                adsource.CurrentPageIndex = pos;
                btnfirst.Enabled = !adsource.IsFirstPage;
                btnprevious.Enabled = !adsource.IsFirstPage;
                btnlast.Enabled = !adsource.IsLastPage;
                btnnext.Enabled = !adsource.IsLastPage;

                gvASNLines.DataSource = adsource;
                gvASNLines.DataBind();
                divPaginationPoLine.Visible = true;
                gvASNLines.Visible = true;
                trPODetails.Visible = true;
            }
            else
            {
                trPODetails.Visible = false;
                gvASNLines.DataSource = null;
                gvASNLines.DataBind();
                divPaginationPoLine.Visible = false;
                gvASNLines.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
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

    private GenericCollection<PurchaseOrderBE> ModifyExtranetPOStatus(GenericCollection<PurchaseOrderBE> CorpnetPOlist)
    {
        PurchaseOrderBC objBC = new PurchaseOrderBC();
        GenericCollection<PurchaseOrderBE> ExtranetList = new GenericCollection<PurchaseOrderBE>();
        string POList = @"'";
        string ExtranetPartners = ConfigurationSettings.AppSettings["ExtranetCMS"];
        foreach (PurchaseOrderBE objBE in CorpnetPOlist)
        {
            if (ExtranetPartners.Contains(objBE.CM))
            {
                POList += objBE.PONumber + @"','";
                objBE.isICOEPartner = "NO";
            }
            else
                objBE.isICOEPartner = "YES";
        }

        ExtranetList = objBC.ModifyExtranetPOStatus(POList);

        foreach (PurchaseOrderBE ExtobjBE in ExtranetList)
        {
            foreach (PurchaseOrderBE CorpObjBE in CorpnetPOlist)
            {
                if (ExtobjBE.PONumber == CorpObjBE.PONumber && ExtobjBE.CM == CorpObjBE.CM)
                {
                    CorpObjBE.ExtranetStatus = ExtobjBE.ExtranetStatus;
                }
            }
        }

        return CorpnetPOlist;

    }
    protected void gvASN_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {

            lblSearchError.Text = string.Empty;
            GenericCollection<ASNLinesBE> listofBE = (GenericCollection<ASNLinesBE>)Session[vASNList];

            if (listofBE != null)
            {
                gvASN.PageIndex = e.NewPageIndex;
                gvASN.DataSource = listofBE;
                gvASN.DataBind();
            }
            else
            {
                gvASN.DataSource = null;
                gvASN.DataBind();
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvASNLines_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            lblSearchError.Text = string.Empty;
            GenericCollection<ASNLinesBE> listofBE = (GenericCollection<ASNLinesBE>)Session["ASNLineCollection"];

            if (listofBE != null)
            {
                gvASNLines.PageIndex = e.NewPageIndex;
                gvASNLines.DataSource = listofBE;
                gvASNLines.DataBind();
                divPaginationPoLine.Visible = true;
                gvASNLines.Visible = true;
            }
            else
            {
                gvASNLines.DataSource = null;
                gvASNLines.DataBind();
                divPaginationPoLine.Visible = false;
                gvASNLines.Visible = false;
            }
        }
        catch (Exception Ex)
        {
            lblSearchError.Text = Ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(Ex);
        }
    }
    protected void gvASNLines_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDirLine] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDirLine].ToString());
            GenericCollection<ASNLinesBE> list = Session[vASNList] as GenericCollection<ASNLinesBE>;
            list = Utils.GridSorting<ASNLinesBE>(gvASNLines, dir, e.SortExpression, list);
            Session["ASNLineCollection"] = list;
            ViewState[vSortDirLine] = (dir == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
            lblSearchError.Visible = true;
            logFile.ErrorLogging(ex);
        }
    }
    protected void gvASN_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SortDirection dir = (ViewState[vSortDir] == null) ? e.SortDirection : (SortDirection)Enum.Parse(typeof(SortDirection), ViewState[vSortDir].ToString());
            GenericCollection<PurchaseOrderBE> list = Session[vASNList] as GenericCollection<PurchaseOrderBE>;
            list = Utils.GridSorting<PurchaseOrderBE>(gvASN, dir, e.SortExpression, list);
            Session[vASNList] = list;
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
            GenericCollection<PurchaseOrderBE> listofBE = (GenericCollection<PurchaseOrderBE>)Session[vASNList];

            if (listofBE != null)
            {
                if (listofBE.Count == 0)
                {
                    lblSearchError.Text = "No records found.";
                    lblSearchError.Visible = true;
                    gvASN.DataSource = null;
                    gvASN.DataBind();
                    lblRec.Visible = false;
                }
                else
                {
                    Utils.GridPaging(gvASN, Convert.ToInt32(size), listofBE);
                    lblRec.Text = Utils.GridRecDispMsg(gvASN.PageIndex, gvASN.PageSize, listofBE.Count);
                    lblRec.Visible = true;
                }

            }
            else
            {
                gvASN.DataSource = null;
                gvASN.DataBind();
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
}