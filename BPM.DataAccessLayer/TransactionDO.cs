using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPM.BusinessEntities;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;

namespace BPM.DataAccessLayer
{
    public class TransactionDO
    {

        public AllTransactionBE GetTransactionList(string TxnType, string Partnername, DateTime? DateFrom, DateTime? DateTo)
        {
            AllTransactionBE TransactionsList = new AllTransactionBE();
            GenericCollection<TransactionsBE> POTransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> ASNTransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> GR4B2TransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> GRTempList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> GR861TransactionsList = new GenericCollection<TransactionsBE>();
            try
            {
                string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
                string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

                SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);

                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@TaskID", TxnType);
                sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
                sqlparams[2] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
                sqlparams[3] = new SqlParameter("@Partner", Partnername);
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.StoredProcedure, "usp_Get_AllTransactions", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                if (ds.Tables[j].Rows[i]["TaskID"].ToString() == "3B2")
                                {
                                    TransactionsBE objBE = new TransactionsBE();
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.TxnType = ds.Tables[j].Rows[i]["TaskID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["CM"].ToString();
                                    objBE.ASNStatus = ds.Tables[j].Rows[i]["TxnStatus"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    ASNTransactionsList.Add(i, objBE);
                                }
                                else if (ds.Tables[j].Rows[i]["TaskID"].ToString() == "4B2" || ds.Tables[j].Rows[i]["TaskID"].ToString() == "861")
                                {
                                    TransactionsBE objBE = new TransactionsBE();
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.TxnType = ds.Tables[j].Rows[i]["TaskID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["CM"].ToString();
                                    objBE.POCorpStatus = ds.Tables[j].Rows[i]["TxnStatus"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.LoadID = ds.Tables[j].Rows[i]["LoadID"].ToString();
                                    objBE.DUNS = ds.Tables[j].Rows[i]["DUNS"].ToString();
                                    objBE.IsICOE = ds.Tables[j].Rows[i]["ISICOE"].ToString();
                                    objBE.ControlNumber = ds.Tables[j].Rows[i]["ControlNumber"].ToString();
                                    GRTempList.Add(i, objBE);
                                }
                                else
                                {
                                    TransactionsBE objBE = new TransactionsBE();
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.TxnType = ds.Tables[j].Rows[i]["TaskID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["CM"].ToString();
                                    objBE.POCorpStatus = ds.Tables[j].Rows[i]["TxnStatus"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    POTransactionsList.Add(i, objBE);
                                }
                            }
                        }
                    }

                }

                if (GRTempList.Count > 0)
                {
                    int i = 0;
                    for (int j = 0; j < GRTempList.Count; j++)
                    {
                        if (GRTempList[j].TxnType == "861")
                        {
                            GR861TransactionsList.Add(i, GRTempList[j]);
                            i++;

                        }
                    }
                    i = 0;
                    for (int j = 0; j < GRTempList.Count; j++)
                    {
                        if (GRTempList[j].TxnType == "4B2")
                        {
                            GR4B2TransactionsList.Add(i, GRTempList[j]);
                            i++;
                        }
                    }
                }

                TransactionsList.POTransactions = POTransactionsList;
                TransactionsList.ASNTransactions = ASNTransactionsList;
                if (TxnType == "4B2" || TxnType == "")
                    TransactionsList.GR4B2Transactions = GR4B2TransactionsList;
                if (TxnType == "861" || TxnType == "")
                    TransactionsList.GR861Transactions = GR861TransactionsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public AllTransactionBE GetTxnDetailsfromExtranet(string POList, string ControlNumberList, string TxnType, string Partnername, DateTime? DateFrom, DateTime? DateTo)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            AllTransactionBE TransactionsList = new AllTransactionBE();
            GenericCollection<TransactionsBE> POTransactionsList = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> Transactions841List = new GenericCollection<TransactionsBE>();
            GenericCollection<TransactionsBE> GRTransactionsList = new GenericCollection<TransactionsBE>();

            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@TaskID", TxnType);
            sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[2] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[3] = new SqlParameter("@Partner", Partnername);
            sqlparams[4] = new SqlParameter("@PONumber", POList);
            sqlparams[5] = new SqlParameter("@ControlNumber", ControlNumberList);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_Get_AllTransactions", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                if (ds.Tables[j].Rows[i]["TaskID"].ToString() == "841")
                                {
                                    TransactionsBE objBE = new TransactionsBE();
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.TxnType = ds.Tables[j].Rows[i]["TaskID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["CM"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.StageName = ds.Tables[j].Rows[i]["stageName"].ToString();
                                    Transactions841List.Add(i, objBE);
                                }
                                else if (ds.Tables[j].Rows[i]["TaskID"].ToString() == "861")
                                {
                                    TransactionsBE objBE = new TransactionsBE();
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["Field2"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["CM"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.LoadID = ds.Tables[j].Rows[i]["Field9"].ToString();
                                    objBE.DUNS = ds.Tables[j].Rows[i]["Field3"].ToString();
                                    objBE.ControlNumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.StageName = ds.Tables[j].Rows[i]["stageName"].ToString();
                                    GRTransactionsList.Add(i, objBE);
                                }
                                else
                                {
                                    TransactionsBE objBE = new TransactionsBE();
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["PONumber"].ToString();
                                    objBE.Status = ds.Tables[j].Rows[i]["TxnStatus"].ToString();
                                    objBE.StageName = ds.Tables[j].Rows[i]["StageName"].ToString();
                                    POTransactionsList.Add(i, objBE);
                                }
                            }
                        }
                    }
                }

                TransactionsList.POTransactions = POTransactionsList;
                TransactionsList.Transactions841 = Transactions841List;
                TransactionsList.GR861Transactions = GRTransactionsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public GenericCollection<ASNHeaderBE> GetASNArchiveFiles(DateTime? DateFrom, DateTime? DateTo)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);


            GenericCollection<ASNHeaderBE> ASNHeaderBEList = new GenericCollection<ASNHeaderBE>();

            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[1] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[2] = new SqlParameter("@TaskID", "3B2");
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.StoredProcedure, "usp_get_IndividualPODetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                ASNHeaderBE objBE = new ASNHeaderBE();
                                objBE.ArchiveFile = ds.Tables[j].Rows[i]["MessageArchiveLocation"].ToString();
                                string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                objBE.TxnDate = TxnDate;
                                ASNHeaderBEList.Add(i, objBE);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ASNHeaderBEList;
        }

        public GenericCollection<GRHeaderBE> GetGRArchiveFiles(DateTime? DateFrom, DateTime? DateTo, string TxnType, string Partner)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);

            GenericCollection<GRHeaderBE> GRHeaderBEList = new GenericCollection<GRHeaderBE>();

            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[1] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[2] = new SqlParameter("@TaskID", TxnType);
            sqlparams[3] = new SqlParameter("@Partner", Partner);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.StoredProcedure, "usp_get_IndividualPODetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                GRHeaderBE objBE = new GRHeaderBE();
                                objBE.ArchiveFile = ds.Tables[j].Rows[i]["MessageArchiveLocation"].ToString();
                                string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                objBE.TxnDate = TxnDate;
                                GRHeaderBEList.Add(i, objBE);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return GRHeaderBEList;
        }

        public GenericCollection<TransformationBE> GetTransformationDetails(DateTime? DateFrom, DateTime? DateTo, string ControlNumber, string Partnername, string PONumber, string Plant)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            GenericCollection<TransformationBE> TransactionsList = new GenericCollection<TransformationBE>();

            SqlParameter[] sqlparams = new SqlParameter[6];
            sqlparams[0] = new SqlParameter("@TaskID", "841");
            sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[2] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[3] = new SqlParameter("@Partner", Partnername);
            sqlparams[4] = new SqlParameter("@PONumber", PONumber);
            sqlparams[5] = new SqlParameter("@ControlNumber", ControlNumber);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_Get_AllTransactions", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                if (ds.Tables[j].Rows[i]["TaskID"].ToString() == "841")
                                {
                                    TransformationBE objBE = new TransformationBE();
                                    objBE.ControlNumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.TxnType = ds.Tables[j].Rows[i]["TaskID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["CM"].ToString();
                                    objBE.DUNS = ds.Tables[j].Rows[i]["Field3"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["Field4"].ToString();
                                    objBE.Plant = ds.Tables[j].Rows[i]["Field5"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.StageName = ds.Tables[j].Rows[i]["stageName"].ToString();
                                    TransactionsList.Add(i, objBE);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public TransformationBE Load841Files(string ControlNumber, string PartnerName, DateTime? TxnDate)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            TransformationBE objBE = new TransformationBE();

            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@TxnDate", TxnDate);
            sqlparams[1] = new SqlParameter("@DUNS", PartnerName);
            sqlparams[2] = new SqlParameter("@ControlNumber", ControlNumber);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_GetTransformationArchives", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                string ArchiveFile = ds.Tables[j].Rows[i]["MessageArchiveLocation"].ToString();
                                string MessageType = ds.Tables[j].Rows[i]["MessageType"].ToString();
                                if (MessageType.ToLower() == "http://schemas.microsoft.com/biztalk/edi/x12/2006#x12_00401_841")
                                    objBE.ArchiveFile = ArchiveFile;
                                else
                                {
                                    if (ArchiveFile.Contains(".txt"))
                                        objBE.AckArchiveFile = ArchiveFile;

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objBE;
        }

        public GenericCollection<ShowShipHeaderBE> GetTransformationDetails(string Type, string TxnID, string DONumber, string Status, string Partner, DateTime? DateFrom, DateTime? DateTo)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            GenericCollection<ShowShipHeaderBE> TransactionsList = new GenericCollection<ShowShipHeaderBE>();

            SqlParameter[] sqlparams = new SqlParameter[8];
            sqlparams[0] = new SqlParameter("@Type", Type);
            sqlparams[1] = new SqlParameter("@TransactionID", TxnID);
            sqlparams[2] = new SqlParameter("@DONumber", DONumber);
            sqlparams[3] = new SqlParameter("@Partner", Partner);
            sqlparams[4] = new SqlParameter("@Status", Status);
            sqlparams[5] = new SqlParameter("@LoadID", "");
            sqlparams[6] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[7] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_ShowShipmentDetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ShowShipHeaderBE ArchiveBE = new ShowShipHeaderBE();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (Type == "GetDetails")
                            {
                                ShowShipHeaderBE objBE = new ShowShipHeaderBE();
                                objBE.SNo = i;
                                objBE.TransactionID = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                                objBE.TxnType = ds.Tables[0].Rows[i]["TaskID"].ToString();
                                objBE.DONumber = ds.Tables[0].Rows[i]["DO"].ToString();
                                objBE.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                                string DateTime = ds.Tables[0].Rows[i]["TxnDate"].ToString();
                                DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                objBE.TxnDate = TxnDate;
                                objBE.SAPVendorNumber = ds.Tables[0].Rows[i]["SPVendorNumber"].ToString();
                                TransactionsList.Add(i, objBE);
                            }
                            else
                            {
                                string ArchiveFile = ds.Tables[0].Rows[i]["MessageArchiveLocation"].ToString();
                                string MessageType = ds.Tables[0].Rows[i]["MessageType"].ToString();
                                if (MessageType.ToLower() == "http://www.openapplications.org/oagis/9#showshipment")
                                    ArchiveBE.ShowShipArchiveFile = ArchiveFile;
                                else
                                    ArchiveBE.DesAdvArchiveFile = ArchiveFile;
                            }
                        }
                        if (Type == "Files")
                            TransactionsList.Add(0, ArchiveBE);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public GenericCollection<PartnerBE> GetPartnerList()
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);

            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();

            DataSet ds = null;
            try
            {
                //                ds = SqlHelper.ExecuteDataset(DataStoreconnection, CommandType.Text, @"select distinct hvl.ID,hvl.Name,DUNS,Plant,SAPVendorNumber,pv.value as ReceiverKey ,pv.name from HED_VendorLookUp hvl
                //left join PartnerValue pv on hvl.ID = pv.PID and pv.name in ('EDI_IDENTIFIER','SAPPARTNERPROFILE','EDI940AgreementName','CELESTICA_4B2SHORTNAME')");
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, "select distinct ID,Name,DUNS,Plant,SAPVendorNumber from Celestica_DataStore.dbo.HED_VendorLookUp");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            PartnerBE objBE = new PartnerBE();
                            objBE.PartnerName = ds.Tables[0].Rows[i]["Name"].ToString();
                            objBE.ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            objBE.DUNS = ds.Tables[0].Rows[i]["DUNS"].ToString();
                            objBE.Plant = ds.Tables[0].Rows[i]["Plant"].ToString();
                            objBE.SAPVendorNumber = ds.Tables[0].Rows[i]["SAPVendorNumber"].ToString();
                            //objBE.ReceiverKey = ds.Tables[0].Rows[i]["ReceiverKey"].ToString();
                            PartnerList.Add(i, objBE);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerList;
        }

        public GenericCollection<PartnerBE> GetCorpPartnerList()
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);

            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();

            DataSet ds = null;
            try
            {
                //                ds = SqlHelper.ExecuteDataset(DataStoreconnection, CommandType.Text, @"select distinct hvl.ID,hvl.Name,DUNS,Plant,SAPVendorNumber,pv.value as ReceiverKey ,pv.name from HED_VendorLookUp hvl
                //left join PartnerValue pv on hvl.ID = pv.PID and pv.name in ('EDI_IDENTIFIER','SAPPARTNERPROFILE','EDI940AgreementName','CELESTICA_4B2SHORTNAME')");
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, @"select distinct Name into #temptable from Celestica_DataStore.dbo.HED_VendorLookUp where DUNS !='0000000000' and DUNS!='000000000' INSERT INTO #temptable select distinct Name from DataStore.dbo.HED_VendorLookUp where (DUNS !='0000000000') and DUNS!='000000000' select distinct Name from #temptable");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            PartnerBE objBE = new PartnerBE();
                            objBE.PartnerName = ds.Tables[0].Rows[i]["Name"].ToString();
                            //objBE.ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            //objBE.DUNS = ds.Tables[0].Rows[i]["DUNS"].ToString();
                            //objBE.Plant = ds.Tables[0].Rows[i]["Name"].ToString();
                            //objBE.SAPVendorNumber = ds.Tables[0].Rows[i]["SAPVendorNumber"].ToString();
                            //objBE.ReceiverKey = ds.Tables[0].Rows[i]["ReceiverKey"].ToString();
                            PartnerList.Add(i, objBE);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerList;
        }

        public GenericCollection<PartnerBE> GetExtraPartnerList()
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            GenericCollection<PartnerBE> PartnerList = new GenericCollection<PartnerBE>();

            DataSet ds = null;
            try
            {
                //                ds = SqlHelper.ExecuteDataset(DataStoreconnection, CommandType.Text, @"select distinct hvl.ID,hvl.Name,DUNS,Plant,SAPVendorNumber,pv.value as ReceiverKey ,pv.name from HED_VendorLookUp hvl
                //left join PartnerValue pv on hvl.ID = pv.PID and pv.name in ('EDI_IDENTIFIER','SAPPARTNERPROFILE','EDI940AgreementName','CELESTICA_4B2SHORTNAME')");
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.Text, @"select distinct Name  INTO #temptable from DataStore.dbo.HED_VendorLookUp where (DUNS !='0000000000') and DUNS!='000000000' select distinct Name from #temptable");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {

                            PartnerBE objBE = new PartnerBE();
                            objBE.PartnerName = ds.Tables[0].Rows[i]["Name"].ToString();
                            //objBE.ID = ds.Tables[0].Rows[i]["ID"].ToString();
                            //objBE.DUNS = ds.Tables[0].Rows[i]["DUNS"].ToString();
                            //objBE.Plant = ds.Tables[0].Rows[i]["Name"].ToString();
                            //objBE.SAPVendorNumber = ds.Tables[0].Rows[i]["SAPVendorNumber"].ToString();
                            //objBE.ReceiverKey = ds.Tables[0].Rows[i]["ReceiverKey"].ToString();
                            PartnerList.Add(i, objBE);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerList;
        }

        public GenericCollection<DeliveryShipmentBE> GetDeliveryShipmentDetails(string Type, string TransctionID, string DO, string PO, string Status, string Partner, string Plant, string OrderType, DateTime? DateFrom, DateTime? DateTo)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            GenericCollection<DeliveryShipmentBE> TransactionsList = new GenericCollection<DeliveryShipmentBE>();

            SqlParameter[] sqlparams = new SqlParameter[10];
            sqlparams[0] = new SqlParameter("@Type", Type);
            sqlparams[1] = new SqlParameter("@TransactionID", TransctionID);
            sqlparams[2] = new SqlParameter("@DONumber", DO);
            sqlparams[3] = new SqlParameter("@Partner", Partner);
            sqlparams[4] = new SqlParameter("@Status", Status);
            sqlparams[5] = new SqlParameter("@PONumber", PO);
            sqlparams[6] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[7] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[8] = new SqlParameter("@POType", OrderType);
            sqlparams[9] = new SqlParameter("@Plant", Plant);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_DeliveryOrders", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        DeliveryShipmentBE ArchiveBE = new DeliveryShipmentBE();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (Type == "Get")
                            {
                                DeliveryShipmentBE objBE = new DeliveryShipmentBE();
                                objBE.SNo = i;
                                objBE.TransactionID = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                                objBE.TxnType = "940";
                                objBE.DONumber = ds.Tables[0].Rows[i]["DO"].ToString();
                                objBE.PONumber = ds.Tables[0].Rows[i]["PO"].ToString();
                                objBE.CM = ds.Tables[0].Rows[i]["CM"].ToString();
                                objBE.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                                string DateTime = ds.Tables[0].Rows[i]["TxnDate"].ToString();
                                DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                objBE.TxnDate = TxnDate;
                                objBE.Plant = ds.Tables[0].Rows[i]["Plant"].ToString();
                                objBE.OrderType = ds.Tables[0].Rows[i]["PoType"].ToString();
                                objBE.MDNStatus = ds.Tables[0].Rows[i]["mdnstatus"].ToString();
                                TransactionsList.Add(i, objBE);
                            }
                            else
                            {
                                string ArchiveFile = ds.Tables[0].Rows[i]["MessageArchiveLocation"].ToString();
                                string MessageType = ds.Tables[0].Rows[i]["MessageType"].ToString();
                                if (MessageType.ToLower() == "http://www.openapplications.org/oagis/9#processshipment")
                                    ArchiveBE.ProcessShipmentArchiveFile = ArchiveFile;
                                else
                                    ArchiveBE.DOIDOCArchiveFile = ArchiveFile;
                            }
                        }
                        if (Type == "Files")
                            TransactionsList.Add(0, ArchiveBE);

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public GenericCollection<ShowShipmentBE> GetShowShipment945Details(string Type, string TransctionID, string DO, string LoadID, string Status, string TxnType, string Partner, string Plant, string OrderType, DateTime? DateFrom, DateTime? DateTo)
        {
            GenericCollection<ShowShipmentBE> FinalTransactionsList = new GenericCollection<ShowShipmentBE>();
            List<String> ConnList = new List<String>();

            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";


            if (BAMEnvironment.ToLower() == "uat")
            {
                ConnList = ConfigurationSettings.AppSettings.AllKeys
                                .Where(key => key.Contains("UATExtranetBAMConn"))
                                .Select(key => ConfigurationSettings.AppSettings[key])
                                .ToList();
            }
            else
            {
                ConnList = ConfigurationSettings.AppSettings.AllKeys
                                 .Where(key => key.Contains("ProdExtranetBAMConn"))
                                 .Select(key => ConfigurationSettings.AppSettings[key])
                                 .ToList();
            }



            //string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : (Partner.ToLower() == "ceva" ? ConfigurationSettings.AppSettings["ProdExtranetCEVABAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString());
            int a = 1;
            Parallel.ForEach(ConnList, (ConnectionString) =>
            {
                //string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConnectionString;
                SqlConnection extranetconnection = new SqlConnection(ConnectionString);

                GenericCollection<ShowShipmentBE> TransactionsList = new GenericCollection<ShowShipmentBE>();

                SqlParameter[] sqlparams = new SqlParameter[11];
                sqlparams[0] = new SqlParameter("@Type", Type);
                sqlparams[1] = new SqlParameter("@TransactionID", TransctionID);
                sqlparams[2] = new SqlParameter("@DONumber", DO);
                sqlparams[3] = new SqlParameter("@Partner", Partner);
                sqlparams[4] = new SqlParameter("@Status", Status);
                sqlparams[5] = new SqlParameter("@LoadID", LoadID);
                sqlparams[6] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
                sqlparams[7] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
                sqlparams[8] = new SqlParameter("@POType", OrderType);
                sqlparams[9] = new SqlParameter("@Plant", Plant);
                sqlparams[10] = new SqlParameter("@TxnType", TxnType);
                DataSet ds = null;
                try
                {
                    ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_showshipment945details", sqlparams);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            ShowShipmentBE ArchiveBE = new ShowShipmentBE();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (Type == "Get")
                                {
                                    ShowShipmentBE objBE = new ShowShipmentBE();
                                    objBE.SNo = i;
                                    objBE.TransactionID = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                                    objBE.TxnType = ds.Tables[0].Rows[i]["TaskID"].ToString(); ;
                                    objBE.DONumber = ds.Tables[0].Rows[i]["DO"].ToString();
                                    objBE.LoadID = ds.Tables[0].Rows[i]["LoadID"].ToString();
                                    objBE.CM = ds.Tables[0].Rows[i]["CM"].ToString();
                                    objBE.Status = ds.Tables[0].Rows[i]["Status"].ToString();
                                    string DateTime = ds.Tables[0].Rows[i]["TxnDate"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[0].Rows[i]["TxnDate"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.Plant = ds.Tables[0].Rows[i]["Plant"].ToString();
                                    objBE.OrderType = ds.Tables[0].Rows[i]["OrderType"].ToString();
                                    objBE.StageName = ds.Tables[0].Rows[i]["StageName"].ToString();
                                    objBE.MDNStatus = ds.Tables[0].Rows[i]["MDNStatus"].ToString();
                                    FinalTransactionsList.Add(FinalTransactionsList.Count, objBE);
                                }
                                else
                                {
                                    string ArchiveFile = ds.Tables[0].Rows[i]["MessageArchiveLocation"].ToString();
                                    string MessageType = ds.Tables[0].Rows[i]["MessageType"].ToString();
                                    if (MessageType.ToLower() == "http://microsoft/msit/edifact/2009#efact_d96a_aperak")
                                        ArchiveBE.AckArchiveFile = ArchiveFile;
                                    else if (MessageType.ToLower() == "http://www.openapplications.org/oagis/9#showshipment")
                                        ArchiveBE.ShowShipArchiveFile = ArchiveFile;
                                    else if (MessageType.ToLower() == "http://schemas.microsoft.com/biztalk/edi/x12/2006#x12_00401_945")
                                        ArchiveBE.EDIXMLArchiveFile = ArchiveFile;
                                    else if (MessageType.ToLower() == "http://schemas.microsoft.com/biztalk/edi/x12/2006#x12_00401_997")
                                        ArchiveBE.AckArchiveFile = ArchiveFile;
                                    else
                                        ArchiveBE.DesAdvArchiveFile = ArchiveFile;
                                }
                            }
                            if (Type == "Files")
                                TransactionsList.Add(0, ArchiveBE);
                            //string HubName = "Hub" + a.ToString();
                            //System.Web.HttpContext.Current.Session[HubName] = TransactionsList;
                            //a++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            });

            //for (int i = 0; i < ConnList.Count; i++)
            //{
            //    GenericCollection<ShowShipmentBE> tempList = new GenericCollection<ShowShipmentBE>();
            //    tempList = (GenericCollection<ShowShipmentBE>)System.Web.HttpContext.Current.Session["Hub" + (i + 1).ToString()];
            //    if (tempList != null)
            //    {
            //        foreach (ShowShipmentBE objBE in tempList)
            //        {
            //            if (FinalTransactionsList.Count == 0)
            //                FinalTransactionsList.Add(0, objBE);
            //            else
            //                FinalTransactionsList.Add(FinalTransactionsList.Count - 1, objBE);
            //        }
            //    }
            //}
            return FinalTransactionsList;
        }

        public GenericCollection<GRListBE> GetGRExtranetList(string Action, DateTime? DateFrom, DateTime? DateTo, string PONumber, string LoadID, string Partner, string ControlNumber, DateTime? TxnDateP)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);

            GenericCollection<GRListBE> TransactionsList = new GenericCollection<GRListBE>();

            SqlParameter[] sqlparams = new SqlParameter[8];
            sqlparams[0] = new SqlParameter("@LoadID", LoadID);
            sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[2] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[3] = new SqlParameter("@Partner", Partner);
            sqlparams[4] = new SqlParameter("@PONumber", PONumber);
            sqlparams[5] = new SqlParameter("@ControlNumber", ControlNumber);
            sqlparams[6] = new SqlParameter("@Action", Action);
            sqlparams[7] = new SqlParameter("@TxnDate", TxnDateP);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_GRextranetList", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                if (Action == "List")
                                {
                                    GRListBE objBE = new GRListBE();
                                    objBE.SNo = i;
                                    objBE.TransactionID = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["PO"].ToString();
                                    objBE.Status = ds.Tables[j].Rows[i]["GRExtStatus"].ToString();
                                    objBE.Functional997Ack = ds.Tables[j].Rows[i]["AckStatus"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["DT"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["DT"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.LoadID = ds.Tables[j].Rows[i]["LoadID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["DUNS"].ToString();
                                    objBE.ControlNumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.MDNforAck = ds.Tables[j].Rows[i]["MDNStatus"].ToString();
                                    TransactionsList.Add(i, objBE);
                                }
                                else
                                {
                                    GRListBE objBE = new GRListBE();
                                    objBE.MessageType = ds.Tables[j].Rows[i]["MessageType"].ToString();
                                    objBE.ArchiveFile = ds.Tables[j].Rows[i]["MessageArchiveLocation"].ToString();
                                    TransactionsList.Add(i, objBE);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public GenericCollection<GRListBE> GetGRCorpnetList(string Action, DateTime? DateFrom, DateTime? DateTo, string PONumber, string LoadID, string Partner, string ControlNumber, string RefID, DateTime? TxnDateP)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);

            GenericCollection<GRListBE> TransactionsList = new GenericCollection<GRListBE>();

            SqlParameter[] sqlparams = new SqlParameter[9];
            sqlparams[0] = new SqlParameter("@LoadID", LoadID);
            sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
            sqlparams[2] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
            sqlparams[3] = new SqlParameter("@Partner", Partner);
            sqlparams[4] = new SqlParameter("@PONumber", PONumber);
            sqlparams[5] = new SqlParameter("@ControlNumber", ControlNumber);
            sqlparams[6] = new SqlParameter("@Action", Action);
            sqlparams[7] = new SqlParameter("@RefID", RefID);
            sqlparams[8] = new SqlParameter("@TxnDate", TxnDateP);
            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.StoredProcedure, "usp_get_GRCorpnetList", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables.Count; j++)
                        {
                            for (int i = 0; i < ds.Tables[j].Rows.Count; i++)
                            {
                                if (Action == "List")
                                {
                                    GRListBE objBE = new GRListBE();
                                    objBE.SNo = i;
                                    objBE.RefID = ds.Tables[j].Rows[i]["refID"].ToString();
                                    objBE.PONumber = ds.Tables[j].Rows[i]["TransactionID"].ToString();
                                    objBE.Status = ds.Tables[j].Rows[i]["GRCorpStatus"].ToString();
                                    string DateTime = ds.Tables[j].Rows[i]["DT"].ToString();
                                    DateTime? TxnDate = DateTime != "" ? Convert.ToDateTime(ds.Tables[j].Rows[i]["DT"].ToString()) : Convert.ToDateTime("1/1/1990");
                                    objBE.TxnDate = TxnDate;
                                    objBE.LoadID = ds.Tables[j].Rows[i]["LoadID"].ToString();
                                    objBE.CM = ds.Tables[j].Rows[i]["DUNS"].ToString();
                                    objBE.ControlNumber = ds.Tables[j].Rows[i]["controlNumber"].ToString();
                                    TransactionsList.Add(i, objBE);
                                }
                                else
                                {
                                    GRListBE objBE = new GRListBE();
                                    objBE.MessageType = ds.Tables[j].Rows[i]["MessageType"].ToString();
                                    objBE.ArchiveFile = ds.Tables[j].Rows[i]["MessageArchiveLocation"].ToString();
                                    TransactionsList.Add(i, objBE);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionsList;
        }

        public DataSet ExecuteQueryWindow(string txtQuery, string Environment, bool IsCeva)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = "";
            string extranetconnection1 = "";
            SqlConnection connection = null;

            if (BAMEnvironment == "UAT")
            {
                if (Environment.Trim() == "Extranet")
                {
                    extranetconnection1 = ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString();
                    connection = new SqlConnection(extranetconnection1);
                }
                else
                {
                    extranetconnection1 = ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString();
                    connection = new SqlConnection(corpnetconnection1);
                }
            }
            else
            {
                if (Environment == "Extranet")
                {
                    if (IsCeva)
                    {
                        extranetconnection1 = ConfigurationSettings.AppSettings["ProdExtranetCEVABAMConn"].ToString();
                        connection = new SqlConnection(extranetconnection1);
                    }
                    else
                    {
                        extranetconnection1 = ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();
                        connection = new SqlConnection(extranetconnection1);
                    }
                }
                else
                {
                    corpnetconnection1 = ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();
                    connection = new SqlConnection(corpnetconnection1);
                }
            }



            DataSet ds = null;
            try
            {
                ds = SqlHelper.ExecuteDataset(connection, CommandType.Text, txtQuery);


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
    }
}


