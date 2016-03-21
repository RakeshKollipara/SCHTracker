using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPM.BusinessEntities;
using BPM.DataAccessLayer;
using System.Web;

namespace BPM.BusinessComponents
{
    public class PurchaseOrderBC
    {
        public GenericCollection<PurchaseOrderBE> GetPurchaseOrdersList(string TaskID, string Partner, DateTime? DateFrom, DateTime? DateTO,string PONumner)
        {
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                PurchaseOrderList = objDO.GetPurchaseOrdersList(TaskID, Partner, DateFrom, DateTO, PONumner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public GenericCollection<PurchaseOrderBE> ModifyExtranetPOStatus(string ExtranetPOList)
        {
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                PurchaseOrderList = objDO.ModifyExtranetPOStatus(ExtranetPOList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public GenericCollection<PurchaseOrderBE> GetASNStatus(string POList)
        {
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                PurchaseOrderList = objDO.GetASNStatus(POList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public string GetPOArchiveFile(string PONumber)
        {
            string POArchiveFile = "";
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                POArchiveFile = objDO.GetPOArchiveFile(PONumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return POArchiveFile;
        }

        public string GetPartnerName(string SAPVendor)
        {
            string PartnerName = "";
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                HttpContext httpContext = HttpContext.Current;
                GenericCollection<PartnerBE> PartnerList = (GenericCollection<PartnerBE>)HttpContext.Current.Session["PartnerList"];

                for(int i=0;i < PartnerList.Count ; i++)
                {
                    if (PartnerList[i].DUNS == SAPVendor || PartnerList[i].SAPVendorNumber == SAPVendor || PartnerList[i].Plant == SAPVendor || PartnerList[i].ReceiverKey == SAPVendor || PartnerList[i].PartnerName == SAPVendor)
                    {
                        PartnerName = PartnerList[i].PartnerName;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerName;
        }

        public GenericCollection<PurchaseOrderHistoryBE> BindPOHistory(string PONumber)
        {
            GenericCollection<PurchaseOrderHistoryBE> PurchaseOrderHisList = new GenericCollection<PurchaseOrderHistoryBE>();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                PurchaseOrderHisList = objDO.BindPOHistory(PONumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderHisList;
        }

        public GenericCollection<PurchaseOrderBE> GetIndividualPODetails(string PoNumber,string TaskID, DateTime? DateFrom, DateTime? DateTo, string PartnerName)
        {
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                PurchaseOrderList = objDO.GetIndividualPODetails(PoNumber, TaskID, DateFrom, DateTo,PartnerName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public GenericCollection<PurchaseOrderBE> ModifyExtranetPODetails(string POList,string Type)
        {
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                PurchaseOrderList = objDO.ModifyExtranetPODetails(POList,Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public PurchaseOrderBE LoadPOFiles(string TransactionID)
        {
            PurchaseOrderBE ArchiveBE = new PurchaseOrderBE();
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                ArchiveBE = objDO.LoadPOFiles(TransactionID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ArchiveBE;
        }

        public string LoadPOErrorMessage(string ReferenceID,string Environment)
        {
            string ErrorDesc = "";
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                ErrorDesc = objDO.LoadPOErrorMessage(ReferenceID, Environment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrorDesc;
        }

        public string LoadGRErrorMessage(string ControlNumber, string Environment, DateTime? TxnDateTime)
        {
            string ErrorDesc = "";
            PurchaseOrderDO objDO = new PurchaseOrderDO();
            try
            {
                ErrorDesc = objDO.LoadGRErrorMessage(ControlNumber, Environment, TxnDateTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ErrorDesc;
        }
    }
}
