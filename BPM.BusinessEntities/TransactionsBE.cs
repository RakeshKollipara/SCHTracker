using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BusinessEntities
{
    public class TransactionsBE
    {
        public string TransactionID { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string IsICOE { get; set; }
        public string POCorpStatus { get; set; }
        public string GRExtStatus { get; set; }
        public string Status { get; set; }
        public string POCBODRcvd { get; set; }
        public string PONumber { get; set; }
        public string FunctionalAck { get; set; }
        public string MDNForFuncAck { get; set; }
        public string LoadID { get; set; }
        public string DUNS { get; set; }
        public string StageName { get; set; }
        public string ControlNumber { get; set; }
        public string ASNStatus { get; set; }
        public string NoFuncAck { get; set; }
    }

    public class AllTransactionBE
    {
        public GenericCollection<TransactionsBE> POTransactions { get; set; }
        public GenericCollection<TransactionsBE> ASNTransactions { get; set; }
        public GenericCollection<TransactionsBE> GR4B2Transactions { get; set; }
        public GenericCollection<TransactionsBE> GR861Transactions { get; set; }
        public GenericCollection<TransactionsBE> Transactions841 { get; set; }

    }

    public class ASNHeaderBE
    {
        public int SNo { get; set; }
        public string ArchiveFile { get; set; }
        public string ReceiverKey { get; set; }
        public DateTime? ShipDate { get; set; }
        public string OriginationParty { get; set; }
        public string SellingPartner { get; set; }
        public string SAPShipTo { get; set; }
        public string FMV { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string Status { get; set; }
        public string PONumber { get; set; }
        public string LoadID { get; set; }
        public int LineCount { get; set; }
        public string ASFeedTxnID { get; set; }
        public string FMVCopy { get; set; }
        public GenericCollection<ASNLinesBE> ASNLines { get; set; }
    }

    public class ASNLinesBE
    {
        public string PONumber { get; set; }
        public string PackSlipNumber { get; set; }
        public string SKU { get; set; }
        public string ItemQuantity { get; set; }
        public string LineNumber { get; set; }
    }

    public class GRHeaderBE
    {
        public int SNo { get; set; }
        public string ArchiveFile { get; set; }
        public string LoadID { get; set; }
        public string ReceiverKey { get; set; }
        public string ReceivedFrom { get; set; }
        public string ReceivedBy { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string Status { get; set; }
        public string PONumber { get; set; }
        public int LineCount { get; set; }
        public string FeedTxnID { get; set; }
        public string ControlNumber { get; set; }
        public GenericCollection<GRLinesBE> GRLines { get; set; }
    }

    public class GRListBE
    {
        public int SNo { get; set; }
        public string LoadID { get; set; }
        public string TransactionID { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string Status { get; set; }
        public string PONumber { get; set; }
        public string ControlNumber { get; set; }
        public string RefID { get; set; }
        public string Functional824Ack { get; set; }
        public string Functional997Ack { get; set; }
        public string MDNforAck { get; set; }
        public string MessageType { get; set; }
        public string ArchiveFile { get; set; }
        public string V02ArchiveFile { get; set; }
        public string EDIXMLArchiveFile { get; set; }
        public string AckXMLArchiveFile { get; set; }
    }

    public class GRLinesBE
    {
        public string LoadID { get; set; }
        public string PONumber { get; set; }
        public string RefDocument { get; set; }
        public string SKU { get; set; }
        public string ItemQuantity { get; set; }
        public string LineNumber { get; set; }
    }

    public class TransformationBE 
    {
        public int SNo { get; set; }
        public string ArchiveFile { get; set; }
        public string AckArchiveFile { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string DUNS { get; set; }
        public string Status { get; set; }
        public string ControlNumber { get; set; }
        public string FuncAck { get; set; }
        public string MDN { get; set; }
        public string StageName { get; set; }
        public string PONumber { get; set; }
        public string Plant { get; set; }

    }

    public class ShowShipHeaderBE
    {
        public int SNo { get; set; }
        public string DesAdvArchiveFile { get; set; }
        public string ShowShipArchiveFile { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string SAPVendorNumber { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public string DONumber { get; set; }
    }

    public class PartnerBE
    {
        public string PartnerName { get; set; }
        public string SAPVendorNumber { get; set; }
        public string DUNS { get; set; }
        public string Plant { get; set; }
        public string ID { get; set; }
        public string ReceiverKey { get; set; }

    }

    public class DeliveryShipmentBE
    {
        public int SNo { get; set; }
        public string DOIDOCArchiveFile { get; set; }
        public string ProcessShipmentArchiveFile { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string PONumber { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public string DONumber { get; set; }
        public string Plant { get; set; }
        public string OrderType { get; set; }
        public string MDNStatus { get; set; }
    }

    public class ShowShipmentBE
    {
        public int SNo { get; set; }
        public string DesAdvArchiveFile { get; set; }
        public string EDIXMLArchiveFile { get; set; }
        public string AckArchiveFile { get; set; }
        public string ShowShipArchiveFile { get; set; }
        public string TxnType { get; set; }
        public DateTime? TxnDate { get; set; }
        public string CM { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public string DONumber { get; set; }
        public string Plant { get; set; }
        public string OrderType { get; set; }
        public string LoadID { get; set; }
        public string Ack997Status { get; set; }
        public string Ack824Status { get; set; }
        public string AckAperakStatus { get; set; }
        public string StageName { get; set; }
        public string MDNStatus { get; set; }
    }

    public class QueryBE
    {
        public int SNo { get; set; }
        public string QueryName { get; set; }
        public string QueryEnv { get; set; }
        public string Query { get; set; }
        public string QueryText { get; set; }
    }

    public class ConnectionStringBE
    {
        public int SNo { get; set; }
        public string HubName { get; set; }
        public string ConnectionString { get; set; }
    }
}
