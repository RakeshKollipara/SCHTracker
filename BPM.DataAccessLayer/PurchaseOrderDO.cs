using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPM.BusinessEntities;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;

namespace BPM.DataAccessLayer
{
    public class PurchaseOrderDO
    {
        public GenericCollection<PurchaseOrderBE> GetPurchaseOrdersList(string TaskID, string Partner, DateTime? DateFrom, DateTime? DateTO, string PONumner)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[5];
                sqlparams[0] = new SqlParameter("@TaskID", TaskID);
                sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
                sqlparams[2] = new SqlParameter("@EndDate", DateTO != null ? DateTO : Convert.ToDateTime("12/31/2050"));
                sqlparams[3] = new SqlParameter("@Partner", Partner);
                sqlparams[4] = new SqlParameter("@PONumber", PONumner);
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.StoredProcedure, "Get_PODetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PurchaseOrderBE objBE = new PurchaseOrderBE();
                            objBE.PONumber = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                            objBE.TxnType = ds.Tables[0].Rows[i]["TaskID"].ToString();
                            objBE.CM = ds.Tables[0].Rows[i]["CM"].ToString();
                            objBE.CorpnetStatus = ds.Tables[0].Rows[i]["POStatus"].ToString();
                            objBE.ExtranetStatus = "NA";
                            objBE.PODate = ds.Tables[0].Rows[i]["PODate"].ToString();
                            objBE.ASNDate = ds.Tables[0].Rows[i]["ASNDate"].ToString();
                            objBE.ASNDate = objBE.ASNDate != "" ? objBE.ASNDate.Substring(0, 16) : "";
                            objBE.ASNStatus = ds.Tables[0].Rows[i]["ASNStatus"].ToString();
                            objBE.GRDate = ds.Tables[0].Rows[i]["GRDate"].ToString();
                            objBE.GRDate = objBE.GRDate != "" ? objBE.GRDate.Substring(0, 16) : "";
                            objBE.GRStatus = ds.Tables[0].Rows[i]["GRStatus"].ToString();
                            PurchaseOrderList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public GenericCollection<PurchaseOrderBE> ModifyExtranetPOStatus(string ExtranetPOList)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.Text, BAMQueries.UpdateExtranetStatusforPO1 + ExtranetPOList + BAMQueries.UpdateExtranetStatusforPO2 + ExtranetPOList + BAMQueries.UpdateExtranetStatusforPO3);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PurchaseOrderBE objBE = new PurchaseOrderBE();
                            objBE.PONumber = ds.Tables[0].Rows[i]["Field2"].ToString();
                            objBE.ExtranetStatus = ds.Tables[0].Rows[i]["Status"].ToString();
                            objBE.CM = ds.Tables[0].Rows[i]["Field3"].ToString();
                            PurchaseOrderList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public GenericCollection<PurchaseOrderBE> GetASNStatus(string POList)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, POList);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PurchaseOrderBE objBE = new PurchaseOrderBE();
                            objBE.PONumber = ds.Tables[0].Rows[i]["Field2"].ToString();
                            objBE.ExtranetStatus = ds.Tables[0].Rows[i]["Status"].ToString();
                            PurchaseOrderList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public string GetPOArchiveFile(string PONumber)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);
            string POArchiveFile = "";
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, BAMQueries.GetIndividualPODetails + PONumber + @"')");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            POArchiveFile = ds.Tables[0].Rows[i]["MessageArchiveLocation"].ToString() + "," + ds.Tables[0].Rows[i]["MessageType"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return POArchiveFile;
        }

        public string GetPartnerName(string SAPVendor)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";

            string DataStoreconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATDataStoreconnection"].ToString() : ConfigurationSettings.AppSettings["ProdDataStoreconnection"].ToString();



            SqlConnection DataStoreconnection = new SqlConnection(DataStoreconnection1);

            string PartnerName = "";
            try
            {
                //DataSet ds = null;
                //ds = SqlHelper.ExecuteDataset(DataStoreconnection, CommandType.Text, BAMQueries.GetPartnerName + SAPVendor + @"' or SAPVendorNumber = '" + SAPVendor + @"' or Plant = '" + SAPVendor + @"'");
                //if (ds != null)
                //{
                //    if (ds.Tables.Count > 0)
                //    {
                //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //        {
                //            PartnerName = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerName;
        }

        public GenericCollection<PurchaseOrderHistoryBE> BindPOHistory(string PONumber)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);
            GenericCollection<PurchaseOrderHistoryBE> POHistoryList = new GenericCollection<PurchaseOrderHistoryBE>();
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, BAMQueries.GetPOHistory + PONumber + @"'");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PurchaseOrderHistoryBE objBE = new PurchaseOrderHistoryBE();
                            objBE.TransactionTypeID = ds.Tables[0].Rows[i]["TaskID"].ToString();
                            objBE.TransactionTypeDesc = ds.Tables[0].Rows[i]["TransDesc"].ToString();
                            objBE.TransactionDate = ds.Tables[0].Rows[i]["LastModified"].ToString();
                            POHistoryList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return POHistoryList;
        }

        public GenericCollection<PurchaseOrderBE> GetIndividualPODetails(string PoNumber, string TaskID, DateTime? DateFrom, DateTime? DateTo, string PartnerName)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);

            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[5];
                sqlparams[0] = new SqlParameter("@TaskID", TaskID);
                sqlparams[1] = new SqlParameter("@StartDate", DateFrom != null ? DateFrom : Convert.ToDateTime("1/1/1990"));
                sqlparams[2] = new SqlParameter("@EndDate", DateTo != null ? DateTo : Convert.ToDateTime("12/31/2050"));
                sqlparams[3] = new SqlParameter("@Partner", PartnerName);
                sqlparams[4] = new SqlParameter("@PONumber", PoNumber);
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.StoredProcedure, "usp_get_IndividualPODetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PurchaseOrderBE objBE = new PurchaseOrderBE();
                            objBE.PONumber = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                            objBE.TxnType = ds.Tables[0].Rows[i]["TaskID"].ToString();
                            objBE.CM = ds.Tables[0].Rows[i]["CM"].ToString();
                            objBE.CorpnetStatus = ds.Tables[0].Rows[i]["POStatus"].ToString();
                            objBE.ExtranetStatus = "NA";
                            objBE.PODate = ds.Tables[0].Rows[i]["PODate"].ToString();
                            objBE.ReferenceID = ds.Tables[0].Rows[i]["field1"].ToString();
                            objBE.MessageType = ds.Tables[0].Rows[i]["MessageType"].ToString();
                            objBE.MessageArchivePath = ds.Tables[0].Rows[i]["MessageArchiveLocation"].ToString();
                            PurchaseOrderList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public GenericCollection<PurchaseOrderBE> ModifyExtranetPODetails(string POList, string Type)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";

            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();


            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);
            GenericCollection<PurchaseOrderBE> PurchaseOrderList = new GenericCollection<PurchaseOrderBE>();
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@TxnID", POList);
                sqlparams[1] = new SqlParameter("@type", Type);
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_IndividualPODetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PurchaseOrderBE objBE = new PurchaseOrderBE();
                            objBE.ReferenceID = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                            objBE.ExtranetStatus = ds.Tables[0].Rows[i]["Status"].ToString();
                            objBE.POCBODRcvd = ds.Tables[0].Rows[i]["Field5"].ToString();
                            PurchaseOrderList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PurchaseOrderList;
        }

        public PurchaseOrderBE LoadPOFiles(string TransactionID)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();


            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);
            PurchaseOrderBE objBE = new PurchaseOrderBE();
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@TxnID", TransactionID);
                sqlparams[1] = new SqlParameter("@type", "Files");
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.StoredProcedure, "usp_get_IndividualPODetails", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            objBE.OagisArchivePath = ds.Tables[0].Rows[i]["OagisFile"].ToString();
                            objBE.MessageArchivePath = ds.Tables[0].Rows[i]["VO2File"].ToString();
                            objBE.ConfirmBODArchivePath = ds.Tables[0].Rows[i]["ConfirmBOD"].ToString();
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

        public string LoadPOErrorMessage(string ReferenceID, string Environment)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();
            string DataStoreconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATDataStoreconnection"].ToString() : ConfigurationSettings.AppSettings["ProdDataStoreconnection"].ToString();
            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();

            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);
            SqlConnection DataStoreconnection = new SqlConnection(DataStoreconnection1);
            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);
            string PartnerName = "";
            try
            {
                DataSet ds = null;
                if (Environment.ToLower() == "extranet")
                    ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.Text, "select top 1 ErrorNumber + ',' + ErrorFieldDescription as Error from bam_Track_Error_Completed where TransactionID = '" + ReferenceID + "' order by 1 desc");
                else
                    ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, "select top 1 ErrorNumber + ',' + ErrorFieldDescription as Error from bam_Track_Error_Completed where field1 = '" + ReferenceID + "' order by 1 desc");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PartnerName = ds.Tables[0].Rows[i]["Error"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerName;
        }

        public string LoadGRErrorMessage(string ControlNumber, string Environment, DateTime? TxnDateTime)
        {
            string BAMEnvironment = HttpContext.Current.Session["Environment"] != null ? HttpContext.Current.Session["Environment"].ToString() : "UAT";

            string extranetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATExtranetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdExtranetBAMConn1"].ToString();
            string corpnetconnection1 = BAMEnvironment == "UAT" ? ConfigurationSettings.AppSettings["UATCorpnetBAMConn"].ToString() : ConfigurationSettings.AppSettings["ProdCorpnetBAMConn"].ToString();
            SqlConnection extranetconnection = new SqlConnection(extranetconnection1);
            SqlConnection corpnetconnection = new SqlConnection(corpnetconnection1);
            string PartnerName = "";
            try
            {
                DataSet ds = null;
                if (Environment.ToLower() == "extranet")
                    ds = SqlHelper.ExecuteDataset(extranetconnection, CommandType.Text, "select top 1 ErrorNumber + ',' + ErrorFieldDescription as Error from bam_Track_Error_Completed where TransactionID = '" + ControlNumber + "' and CONVERT(datetime, CONVERT(nchar(16), LastModified, 120), 120) = '" + TxnDateTime.ToString() + "' order by 1 desc");
                else
                    ds = SqlHelper.ExecuteDataset(corpnetconnection, CommandType.Text, "select top 1 ErrorNumber + ',' + ErrorFieldDescription as Error from bam_Track_Error_Completed where TransactionID = '" + ControlNumber + "' and CONVERT(datetime, CONVERT(nchar(16), LastModified, 120), 120) = '" + TxnDateTime.ToString() + "' order by 1 desc");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            PartnerName = ds.Tables[0].Rows[i]["Error"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartnerName;
        }
    }
}
