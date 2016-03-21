using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BPM.BusinessEntities;

public partial class AchiveFilesPOPUp : System.Web.UI.Page
{
    public static string MessageArchivePath = "";
    public static string OagisArchivePath = "";
    public static string BODArchivePath = "";
    public static string EDIXMLArchivePath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        PurchaseOrderBE objPOBE;
        ASNHeaderBE objASNBE;
        GRListBE objGRBE;
        TransformationBE obj841BE;
        ShowShipHeaderBE objShowShipBE;
        DeliveryShipmentBE objDOShipBE;
        ShowShipmentBE objSSShipBE;


        if (Request.QueryString["type"].ToString() == "ContactUS")
        {
            string ErrorMessage = Session["ContactUs"].ToString();

            lblErrorMessage.Text = ErrorMessage;
            divPO.Visible = false;
            divASN.Visible = false;
            divGR.Visible = false;
            div841.Visible = false;
            divShowShip.Visible = false;
            divDO.Visible = false;
            divShowShip945.Visible = false;
            divError.Visible = true;
        }

        if (Session["ArchiveFiles"] != null)
        {
            if (Request.QueryString["type"].ToString() == "Error")
            {
                string ErrorMessage = Session["ArchiveFiles"].ToString();

                lblErrorMessage.Text = ErrorMessage;
                divPO.Visible = false;
                divASN.Visible = false;
                divGR.Visible = false;
                div841.Visible = false;
                divShowShip.Visible = false;
                divDO.Visible = false;
                divShowShip945.Visible = false;
                divError.Visible = true;
            }
            if (Request.QueryString["type"].ToString() == "PO")
            {
                objPOBE = (PurchaseOrderBE)Session["ArchiveFiles"];

                txtPONumber.Text = objPOBE.PONumber;
                txtCM.Text = objPOBE.CM;
                txtCorpStatus.Text = objPOBE.CorpnetStatus;
                txtExtStatus.Text = objPOBE.ExtranetStatus;
                txtISICOE.Text = objPOBE.isICOEPartner;
                txtTransactionID.Text = objPOBE.ReferenceID;
                txtPoDate.Text = objPOBE.PODate;
                MessageArchivePath = objPOBE.MessageArchivePath;
                OagisArchivePath = objPOBE.OagisArchivePath;
                BODArchivePath = objPOBE.ConfirmBODArchivePath;
                divPO.Visible = true;
                divASN.Visible = false;
                divGR.Visible = false;
                div841.Visible = false;
                divShowShip.Visible = false;
                divDO.Visible = false;
                divShowShip945.Visible = false;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    trV02.Visible = true;
                else
                    trV02.Visible = false;
                if (!string.IsNullOrEmpty(OagisArchivePath))
                    trOagis.Visible = true;
                else
                    trOagis.Visible = false;
                if (!string.IsNullOrEmpty(BODArchivePath))
                    trBOD.Visible = true;
                else
                    trBOD.Visible = false;
            }
            if (Request.QueryString["type"].ToString() == "ASN")
            {
                objASNBE = (ASNHeaderBE)Session["ArchiveFiles"];
                txtASNPONumber.Text = objASNBE.PONumber;
                txtASNCM.Text = objASNBE.CM;
                txtASNDate.Text = objASNBE.TxnDate.Value.ToShortDateString();
                txtASNFeedID.Text = objASNBE.ASFeedTxnID;
                txtASNLoadID.Text = objASNBE.LoadID;
                MessageArchivePath = objASNBE.ArchiveFile;
                divPO.Visible = false;
                divASN.Visible = true;
                divGR.Visible = false;
                div841.Visible = false;
                divShowShip.Visible = false;
                divDO.Visible = false;
                divShowShip945.Visible = false;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    trV02ASN.Visible = true;
                else
                    trV02ASN.Visible = false;
            }
            if (Request.QueryString["type"].ToString() == "GR")
            {
                objGRBE = (GRListBE)Session["ArchiveFiles"];
                txtGRPONumber.Text = objGRBE.PONumber;
                txtGRCM.Text = objGRBE.CM;
                txtGRDate.Text = objGRBE.TxnDate.Value.ToShortDateString();
                txtGRLoadID.Text = objGRBE.LoadID;
                txtGRControlNumber.Text = objGRBE.ControlNumber;
                MessageArchivePath = objGRBE.V02ArchiveFile;
                EDIXMLArchivePath = objGRBE.EDIXMLArchiveFile;
                BODArchivePath = objGRBE.AckXMLArchiveFile;
                divPO.Visible = false;
                divASN.Visible = false;
                divGR.Visible = true;
                div841.Visible = false;
                divShowShip.Visible = false;
                divDO.Visible = false;
                divShowShip945.Visible = false;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    trV02GR.Visible = true;
                else
                    trV02GR.Visible = false;
                if (!string.IsNullOrEmpty(BODArchivePath))
                    trAckGR.Visible = true;
                else
                    trAckGR.Visible = false;
                if (!string.IsNullOrEmpty(EDIXMLArchivePath))
                    trEDIGR.Visible = true;
                else
                    trEDIGR.Visible = false;
            }
            if (Request.QueryString["type"].ToString() == "841")
            {
                obj841BE = (TransformationBE)Session["ArchiveFiles"];
                txt841ControlNumber.Text = obj841BE.ControlNumber;
                txt841Partner.Text = obj841BE.CM;
                txt841TxnDate.Text = obj841BE.TxnDate.Value.ToShortDateString();
                MessageArchivePath = obj841BE.ArchiveFile;
                BODArchivePath = obj841BE.AckArchiveFile;
                divPO.Visible = false;
                divASN.Visible = false;
                divGR.Visible = false;
                div841.Visible = true;
                divShowShip.Visible = false;
                divDO.Visible = false;
                divShowShip945.Visible = false;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    tr841.Visible = true;
                else
                    tr841.Visible = false;
                if (!string.IsNullOrEmpty(BODArchivePath))
                    tr997.Visible = true;
                else
                    tr997.Visible = false;
            }
            if (Request.QueryString["type"].ToString() == "DESADV")
            {
                objShowShipBE = (ShowShipHeaderBE)Session["ArchiveFiles"];

                txtSSCM.Text = objShowShipBE.CM;
                txtSSDO.Text = objShowShipBE.DONumber;
                txtSSTxnDate.Text = objShowShipBE.TxnDate.Value.ToShortDateString();
                txtSSTxnID.Text = objShowShipBE.TransactionID;
                MessageArchivePath = objShowShipBE.DesAdvArchiveFile;
                BODArchivePath = objShowShipBE.ShowShipArchiveFile;
                divPO.Visible = false;
                divASN.Visible = false;
                divGR.Visible = false;
                div841.Visible = false;
                divDO.Visible = false;
                divShowShip.Visible = true;
                divShowShip945.Visible = false;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    trShowShip.Visible = true;
                else
                    trShowShip.Visible = false;
                if (!string.IsNullOrEmpty(BODArchivePath))
                    trDESADV.Visible = true;
                else
                    trDESADV.Visible = false;
            }
            if (Request.QueryString["type"].ToString() == "DO")
            {
                objDOShipBE = (DeliveryShipmentBE)Session["ArchiveFiles"];

                txtDOPONumber.Text = objDOShipBE.PONumber;
                txtDOCM.Text = objDOShipBE.CM;
                txtDOStatus.Text = objDOShipBE.Status;
                //txtMDNStatus.Text = objDOShipBE.MDNStatus;
                txtDOTxnID.Text = objDOShipBE.TransactionID;
                txtDONumber.Text = objDOShipBE.DONumber;
                txtDOPlant.Text = objDOShipBE.Plant;
                txtDOOrderType.Text = objDOShipBE.OrderType;
                txtDOTxnDate.Text = objDOShipBE.TxnDate.ToString();
                MessageArchivePath = objDOShipBE.DOIDOCArchiveFile;
                OagisArchivePath = objDOShipBE.ProcessShipmentArchiveFile;
                divPO.Visible = false;
                divASN.Visible = false;
                divGR.Visible = false;
                div841.Visible = false;
                divShowShip.Visible = false;
                divDO.Visible = true;
                divShowShip945.Visible = false;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    trDOIDOC.Visible = true;
                else
                    trDOIDOC.Visible = false;
                if (!string.IsNullOrEmpty(OagisArchivePath))
                    trPShipXML.Visible = true;
                else
                    trPShipXML.Visible = false;
            }
            if (Request.QueryString["type"].ToString() == "SS")
            {
                objSSShipBE = (ShowShipmentBE)Session["ArchiveFiles"];

                txtDOPONumber.Text = objSSShipBE.LoadID;
                txtDOCM.Text = objSSShipBE.CM;
                txtDOStatus.Text = objSSShipBE.Status;
                //txtMDNStatus.Text = objDOShipBE.MDNStatus;
                txtDOTxnID.Text = objSSShipBE.TransactionID;
                txtDONumber.Text = objSSShipBE.DONumber;
                txtDOPlant.Text = objSSShipBE.Plant;
                txtDOOrderType.Text = objSSShipBE.OrderType;
                txtDOTxnDate.Text = objSSShipBE.TxnDate.ToString();
                MessageArchivePath = objSSShipBE.ShowShipArchiveFile;
                OagisArchivePath = objSSShipBE.AckArchiveFile;
                BODArchivePath = objSSShipBE.DesAdvArchiveFile;
                EDIXMLArchivePath = objSSShipBE.EDIXMLArchiveFile;
                divPO.Visible = false;
                divASN.Visible = false;
                divGR.Visible = false;
                div841.Visible = false;
                divShowShip.Visible = false;
                divDO.Visible = false;
                divShowShip945.Visible = true;
                divError.Visible = false;
                if (!string.IsNullOrEmpty(MessageArchivePath))
                    trShowShip945.Visible = true;
                else
                    trShowShip945.Visible = false;
                if (!string.IsNullOrEmpty(OagisArchivePath))
                    trSSAperak.Visible = true;
                else
                    trSSAperak.Visible = false;
                if (!string.IsNullOrEmpty(BODArchivePath))
                    trSSDESADV.Visible = true;
                else
                    trSSDESADV.Visible = false;
                if (!string.IsNullOrEmpty(EDIXMLArchivePath))
                    trSSEDI945.Visible = true;
                else
                    trSSEDI945.Visible = false;
            }
        }
    }

    private void downloadFile(string newFile, bool delete)
    {
        try
        {
            if (newFile != "")
            {
                FileInfo fle = new FileInfo(newFile);
                if (fle.Extension == "")
                {
                    string tempath = System.IO.Path.GetTempPath();
                    if (!File.Exists(tempath + fle.Name + ".xml"))
                        File.Copy(newFile, tempath + fle.Name + ".xml");
                    newFile = tempath + fle.Name + ".xml";
                }
            }

            if (!File.Exists(newFile))
            {
                lblSearchError.Text = "No File Exists at specified location.";
                return;

            }
            if (newFile != null && newFile != string.Empty)
            {
                System.Web.HttpResponse fileResponse = System.Web.HttpContext.Current.Response;

                fileResponse.Clear();
                fileResponse.ClearHeaders();

                System.IO.FileInfo fileToDownload = new System.IO.FileInfo(newFile);

                if (fileToDownload.Extension.Contains(".pdf"))
                {
                    fileResponse.ContentType = "application/pdf";
                }
                else if (fileToDownload.Extension.Contains(".doc"))
                {
                    fileResponse.ContentType = "application/msword";
                }
                else
                {
                    fileResponse.ContentType = "text/plain";
                }

                fileResponse.AppendHeader("Content-Disposition", "Attachment; Filename=\"" + fileToDownload.Name + "\"");
                fileResponse.Flush();
                if (File.Exists(fileToDownload.FullName))
                {
                    fileResponse.WriteFile(fileToDownload.FullName);
                    //fileResponse.End();
                    fileResponse.Flush();
                    fileResponse.Close();
                    if (delete)
                        File.Delete(fileToDownload.FullName);

                }
                else
                {
                    lblSearchError.Text = "No File Exists at specified location.";
                }
            }
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
        }
    }

    protected void lnkAttachments_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)(sender);
            string FileType = btn.CommandArgument;
            if (FileType == "V02")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "Oagis")
                downloadFile(OagisArchivePath, false);
            else if (FileType == "V02GR")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "EDIXML")
                downloadFile(EDIXMLArchivePath, false);
            else if (FileType == "AckXML")
                downloadFile(BODArchivePath, false);
            else if (FileType == "V02ASN")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "EDI841")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "ShowShipment")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "desadv")
                downloadFile(BODArchivePath, false);
            else if (FileType == "DODIDOC")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "DOProcShip")
                downloadFile(OagisArchivePath, false);
            else if (FileType == "ShowShipment945")
                downloadFile(MessageArchivePath, false);
            else if (FileType == "desadv945")
                downloadFile(OagisArchivePath, false);
            else if (FileType == "edi")
                downloadFile(BODArchivePath, false);
            else if (FileType == "Aperak")
                downloadFile(EDIXMLArchivePath, false);
            else
                downloadFile(BODArchivePath, false);
        }
        catch (Exception ex)
        {
            lblSearchError.Text = ex.Message;
        }
    }


}
