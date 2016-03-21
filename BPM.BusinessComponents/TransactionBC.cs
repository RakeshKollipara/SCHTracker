using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPM.BusinessEntities;
using BPM.DataAccessLayer;
using System.Data;

namespace BPM.BusinessComponents
{
    public class TransactionBC
    {

        public AllTransactionBE GetTransactionList(string TxnType, DateTime? DateFrom, DateTime? DateTo, string PartnerName)
        {
            AllTransactionBE TransactionList = new AllTransactionBE();
            TransactionDO objDO = new TransactionDO();
            try
            {
                TransactionList = objDO.GetTransactionList(TxnType, PartnerName, DateFrom, DateTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionList;
        }

        public AllTransactionBE GetTxnDetailsfromExtranet(string POList, string ControlNumberList, string TxnType, string Partnername, DateTime? DateFrom, DateTime? DateTo)
        {
            AllTransactionBE TransactionList = new AllTransactionBE();
            TransactionDO objDO = new TransactionDO();
            try
            {
                TransactionList = objDO.GetTxnDetailsfromExtranet(POList, ControlNumberList, TxnType, Partnername, DateFrom, DateTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionList;
        }

        public GenericCollection<ASNHeaderBE> GetASNArchiveFiles(DateTime? DateFrom, DateTime? DateTo)
        {
            GenericCollection<ASNHeaderBE> TransactionList = new GenericCollection<ASNHeaderBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                TransactionList = objDO.GetASNArchiveFiles(DateFrom, DateTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionList;
        }

        public GenericCollection<GRHeaderBE> GetGRArchiveFiles(DateTime? DateFrom, DateTime? DateTo,string TxnType,string Partner)
        {
            GenericCollection<GRHeaderBE> TransactionList = new GenericCollection<GRHeaderBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                TransactionList = objDO.GetGRArchiveFiles(DateFrom, DateTo,TxnType,Partner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionList;
        }

        public GenericCollection<TransformationBE> GetTransformationDetails(DateTime? DateFrom, DateTime? DateTo, string ControlNumber, string PartnerName,string PONumber,string Plant)
        {
            GenericCollection<TransformationBE> TransactionList = new GenericCollection<TransformationBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                TransactionList = objDO.GetTransformationDetails(DateFrom, DateTo,ControlNumber, PartnerName , PONumber,Plant);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionList;
        }

        public TransformationBE Load841Files(string ControlNumber, string PartnerName, DateTime? TxnDate)
        {
            TransformationBE TransactionList = new TransformationBE();
            TransactionDO objDO = new TransactionDO();
            try
            {
                TransactionList = objDO.Load841Files( ControlNumber, PartnerName,TxnDate );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionList;
        }

        public GenericCollection<ShowShipHeaderBE> GetShowShipDetails(string Type, string TxnID, string DONumber, string Status, string Partner, DateTime? DateFrom, DateTime? DateTo)
        {
            GenericCollection<ShowShipHeaderBE> ShowShipHeaderList = new GenericCollection<ShowShipHeaderBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                ShowShipHeaderList = objDO.GetTransformationDetails(Type,TxnID, DONumber, Status, Partner, DateFrom, DateTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ShowShipHeaderList;
        }

        public GenericCollection<PartnerBE> GetPartnerList()
        {
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                PartnerList = objDO.GetPartnerList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerList;
        }

        public GenericCollection<PartnerBE> GetCorpPartnerList()
        {
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                PartnerList = objDO.GetCorpPartnerList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerList;
        }

        public GenericCollection<PartnerBE> GetExtraPartnerList()
        {
            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                PartnerList = objDO.GetExtraPartnerList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerList;
        }

        public GenericCollection<DeliveryShipmentBE> GetDeliveryShipmentDetails(string Type, string TransctionID, string DO,string PO, string Status, string Partner, string Plant, string OrderType, DateTime? DateFrom, DateTime? DateTo)
        {
            GenericCollection<DeliveryShipmentBE> DeliveryShipmentDetails = new GenericCollection<DeliveryShipmentBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                DeliveryShipmentDetails = objDO.GetDeliveryShipmentDetails( Type,  TransctionID,  DO, PO,  Status,  Partner,  Plant,  OrderType,  DateFrom,  DateTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return DeliveryShipmentDetails;
           
        }

        public GenericCollection<ShowShipmentBE> GetShowShipment945Details(string Type, string TransctionID, string DO, string LoadID, string Status,string TxnType, string Partner, string Plant, string OrderType, DateTime? DateFrom, DateTime? DateTo)
        {
            GenericCollection<ShowShipmentBE> ShowShipmentDetails = new GenericCollection<ShowShipmentBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                ShowShipmentDetails = objDO.GetShowShipment945Details(Type, TransctionID, DO, LoadID, Status, TxnType, Partner, Plant, OrderType, DateFrom, DateTo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ShowShipmentDetails;
        }

        public GenericCollection<GRListBE> GetGRExtranetList(string Action, DateTime? DateFrom, DateTime? DateTo, string PONumber, string LoadID, string Partner, string ControlNumber, DateTime? TxnDate)
        {
            GenericCollection<GRListBE> ShowShipHeaderList = new GenericCollection<GRListBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                ShowShipHeaderList = objDO.GetGRExtranetList(Action,DateFrom, DateTo, PONumber,LoadID,Partner,ControlNumber,TxnDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ShowShipHeaderList;
        }

        public GenericCollection<GRListBE> GetGRCorpnetList(string Action, DateTime? DateFrom, DateTime? DateTo, string PONumber, string LoadID, string Partner, string ControlNumber, string RefID, DateTime? TxnDate)
        {
            GenericCollection<GRListBE> ShowShipHeaderList = new GenericCollection<GRListBE>();
            TransactionDO objDO = new TransactionDO();
            try
            {
                ShowShipHeaderList = objDO.GetGRCorpnetList(Action, DateFrom, DateTo, PONumber, LoadID, Partner, ControlNumber, RefID,  TxnDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ShowShipHeaderList;
        }

        public System.Data.DataSet ExecuteQueryWindow(string txtQuery,string Environment,bool IsCeva)
        {
            DataSet ds = new DataSet();
            TransactionDO objDO = new TransactionDO();
            try
            {
                ds = objDO.ExecuteQueryWindow(txtQuery, Environment,IsCeva);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

       
    }
}
