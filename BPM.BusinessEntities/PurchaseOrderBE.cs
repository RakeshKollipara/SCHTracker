using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BusinessEntities
{
    public class PurchaseOrderBE
    {
        public string PONumber { get; set; }
        public string TxnType { get; set; }
        public string CM { get; set; }
        public string CorpnetStatus { get; set; }
        public string ExtranetStatus { get; set; }
        public string PODate { get; set; }
        public string isICOEPartner { get; set; }
        public string ASNDate { get; set; }
        public string ASNStatus { get; set; }
        public string GRDate { get; set; }
        public string GRStatus { get; set; }
        public string POCBODRcvd { get; set; }
        public string ReferenceID { get; set; }
        public string MessageType { get; set; }
        public string MessageArchivePath { get; set; }
        public string OagisArchivePath { get; set; }
        public string ConfirmBODArchivePath { get; set; }
        public string POBOMReceived { get; set; }
        public string ShowShip3B3Received { get; set; }
    }

    public class PurchaseOrderLineBE
    {
        public string POLineNumber { get; set; }
        public string DeliveryDate { get; set; }
        public string ShipDate { get; set; }
        public string Price { get; set; }
        public string Material { get; set; }
        public string SerialNumber { get; set; }
        public string OrderQuantity { get; set; }
    }

    public class PurchaseOrderHeaderBE
    {
        public string PODate { get; set; }
        public string Price { get; set; }
        public string SAPVendor { get; set; }
        public string CM { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string PartnerName { get; set; }
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string State { get; set; }
        public string SAPPurchaseOrderType { get; set; }
    }

    public class PurchaseOrderHistoryBE
    {
        public string TransactionTypeID { get; set; }
        public string TransactionTypeDesc { get; set; }
        public string TransactionDate { get; set; }
    }
}
