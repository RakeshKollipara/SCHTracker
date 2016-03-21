using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BAMQueries
/// </summary>
public class BAMQueries
{
    public static string PurchaseOrderQuery = @"IF EXISTS(SELECT 1 FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..[#StageTable]'))
BEGIN
	DROP TABLE #StageTable
END
IF EXISTS(SELECT 1 FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..[#StatusTable]'))
BEGIN
	DROP TABLE #StatusTable
END
IF EXISTS(SELECT 1 FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..[#SearchTable]'))
BEGIN
	DROP TABLE #SearchTable
END
IF EXISTS(SELECT 1 FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..[#PODate]'))
BEGIN
	DROP TABLE #PODate
END

select Recordid,TransactionID,StageName,
case  when StageName like 'error%' then 'Failed'
else 'Success'  end  as Status into #StageTable from bam_track_message_completed (nolock)
where applicationName ='ERPOrder' and Field3 != 'EGL' 
and StageName not in ('PrivatePOConvertedToProcessPurchaseOrder','VendorLookUpCompleted','IdocConvertedToPrivatePurchaseOrder','ReceivedPOIdocfromSAP')
order by 1 desc,TransactionID

select s.TransactionID,Status into #StatusTable from #StageTable s
inner join (select Max(RecordID) as RecorID,TransactionID from #StageTable group by TransactionID) temp on s.RecordID = temp.RecorID
order by RecordID desc

select distinct TransactionID,Field4 as PODate into #PODate from  bam_track_message_completed  (nolock)
where ApplicationName='ERPOrder' and Field3 != 'EGL' and TaskID in ('3A4','850') and MessageType='http://erporder.zorders01.v3#zorders01'";
 
        public static string PurchaseOrderQuery1 = @"select distinct TransactionID,TaskID into #TotalTxns from bam_track_message_completed btmc (nolock)
 where btmc.ApplicationName in ('FGPIntegration','ERPOrder','celestica')
 and TaskID = '";

    public static string PurchaseOrderQuery2 = @"select distinct TransactionID,TaskID into #TotalTxns from bam_track_message_completed btmc (nolock)
 where btmc.ApplicationName in ('FGPIntegration','ERPOrder','celestica')";

    public static string PurchaseOrderQuery3 = @"
select distinct btmc.TransactionID,TaskID,Field3 as CM,s.Status,p.PODate from bam_track_message_completed (nolock) btmc
inner join #StatusTable s on s.TransactionID = btmc.TransactionID
inner join #PODate p on p.TransactionID = btmc.TransactionID
where ApplicationName='ERPOrder' and Field3 != 'EGL' and TaskID in ('3A4','850') 
and MessageType in ('http://ms.it.ops.cm.processpurchaseorder_v02_10_00#processpurchaseorder_v02_10_00','http://ms.it.ops.cm.changepurchaseorder_v01_00_00#changepurchaseorder_v01_00_00')
where ";

public static string PurchaseOrderQuery4 =" order by PODate desc,btmc.TransactionID desc";


    public static string UpdateExtranetStatusforPO1 = @"IF EXISTS(SELECT 1 FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..[#StageTable]'))
BEGIN
	DROP TABLE #StageTable
END
IF EXISTS(SELECT 1 FROM tempdb.dbo.sysobjects WHERE ID = OBJECT_ID(N'tempdb..[#StatusTable]'))
BEGIN
	DROP TABLE #TempTable
END

select distinct case when Field2 is null then btmc.TransactionID else field2 end as Field2,field3,creationDateTime,
case  when Field5 like 'error%' then 'Failed'
else 'Success'  end  as Status into #StageTable from bam_track_message_completed btmc (nolock)
where btmc.ApplicationName in ('MTVIntegration','PartnerRouting','RL') and (btmc.MessageType = 'http://ms.it.ops.cm.processpurchaseorder_v02_10_00#processpurchaseorder_v02_10_00' or messageType='http://ms.it.ops.cm.changepurchaseorder_v01_00_00#changepurchaseorder_v01_00_00')
and ((btmc.field2 in (";

    public static string UpdateExtranetStatusforPO2 = "') or (btmc.field2 is null and btmc.TransactionID in (";

    public static string UpdateExtranetStatusforPO3 = @"'))))
and (btmc.field5 not in ('ProcessPOReceived','POConvertedToVersion9.2','HEDVendorLookupRulesCalled'))
and Field5 is not null and creationdatetime is not null
order by field2 desc,field3 desc

select max(creationdatetime) as CreationDateTime,Field2,Field3 into #TempTable from #StageTable 
group by Field2,Field3

select distinct d.Field2,d.Field3,t.Status from #TempTable d
inner join #StageTable t on t.CreationDateTime = d.CreationDateTime
order by d.field2,d.field3";

    public static string GetIndividualPODetails = @"select distinct btmc.TransactionID,btmc.MessageArchiveLocation,MessageType from bam_track_message_completed btmc
inner join (select  max(LastModified) as LastModified,TransactionID from bam_track_message_completed 
where (MessageType = 'http://ms.it.ops.cm.processpurchaseorder_v02_10_00#processpurchaseorder_v02_10_00' or messageType='http://ms.it.ops.cm.changepurchaseorder_v01_00_00#changepurchaseorder_v01_00_00')
and MessageArchiveLocation is not null
and applicationname ='ERPOrder'
group by TransactionID) as TemTable on TemTable.LastModified = btmc.LastModified
where btmc.applicationname ='ERPOrder'and 
btmc.MessageArchiveLocation is not null
and (btmc.MessageType = 'http://ms.it.ops.cm.processpurchaseorder_v02_10_00#processpurchaseorder_v02_10_00' or btmc.messageType='http://ms.it.ops.cm.changepurchaseorder_v01_00_00#changepurchaseorder_v01_00_00')
and btmc.TransactionID = ('";

    public static string GetPartnerName = @"select Name from HED_VendorLookUp where DUNS = '";

    public static string GetPOHistory = @"select TransactionID,TaskID,Case when TaskID = '4B2' then 'Goods Receipts Received' 
when TaskID = '3B2' then 'ASN Received'
when TaskID in ('850','3A4') then 'Process PO Received'
when TaskID in ('860','3A8') then 'Change PO Received'
when TaskID = 'BOMUpdate' then 'PO BackFlush Received'
when TaskID = '3B3' then '3B3 Received'
when TaskID = 'Ack' and ProcessName='Celestica_Acknowledgement' then 'Ack Flat File Received'
when TaskID = 'Ack' and ProcessName='Celestica_ShowShipmentResubmission' then 'Resubmission'
when TaskID = 'Email' then 'Email Sent for Neg Ack'
else 'Misc Transaction' end as TransDesc ,LastModified from bam_Track_Message_Completed
where StageName in ('1a - Received_NACKType','1b - Received_NonNACKType','1 - AckReceived','1 - AcknowledgementReceived','1 - 3B3Received','ReceivedPOIdocfromSAP','1 - ReceivedInventoryAdjustmentIn','1 - ASNFromMsgBoxReceived','1 - ConstructPrivateMessage')
and (Field3 is null or Field3 not in ('EGL','TMCSCH','MSFTMOB')) and ApplicationName in ('FGPIntegration','ERPOrder')
and TransactionID = '"; 
}