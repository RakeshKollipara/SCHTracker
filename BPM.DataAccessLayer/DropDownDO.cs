using BPM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BPM.DataAccessLayer
{
    public class DropDownDO
    {

        public SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["DBConnection"].ToString());
        public GenericCollection<Service> GetServiceList(string ServiceLineID)
        {
            GenericCollection<Service> ServiceList = new GenericCollection<Service>();
            try
            {
                DataSet ds = null;
                SqlParameter[] sqlparams = new SqlParameter[1];
                sqlparams[0] = new SqlParameter("@ServiceLineID", ServiceLineID);
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "Get_ServiceList", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Service objBE = new Service();
                            objBE.ServiceID = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceID"].ToString());
                            objBE.ServiceDesc = ds.Tables[0].Rows[i]["ServiceDesc"].ToString();
                            ServiceList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceList;
        }

        public GenericCollection<ServiceComponent> GetServiceComponentList(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompType)
        {
            GenericCollection<ServiceComponent> ServiceComponentList = new GenericCollection<ServiceComponent>();
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[4];
                sqlparams[0] = new SqlParameter("@ServiceLineID", ServiceLineID);
                sqlparams[1] = new SqlParameter("@ServiceID", ServiceID);
                sqlparams[2] = new SqlParameter("@ServiceOptionID", ServiceOptionID);
                sqlparams[3] = new SqlParameter("@ServiceComponentType", ServCompType);
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "Get_ServiceComponentList", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ServiceComponent objBE = new ServiceComponent();
                            if (ServCompType == "SCH")
                                objBE.ServiceComponentID = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceComponentID"].ToString());
                            objBE.ServiceComponentDesc = ds.Tables[0].Rows[i]["ServiceComponentDesc"].ToString();
                            ServiceComponentList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceComponentList;
        }

        public GenericCollection<ServiceOption> GetServiceOptionList(string ServiceLineID, string ServiceID)
        {
            GenericCollection<ServiceOption> ServiceOptionList = new GenericCollection<ServiceOption>();
            try
            {
                SqlParameter[] sqlparams = new SqlParameter[2];
                sqlparams[0] = new SqlParameter("@ServiceLineID", ServiceLineID);
                sqlparams[1] = new SqlParameter("@ServiceID", ServiceID);
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "Get_ServiceOptionList", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ServiceOption objBE = new ServiceOption();
                            objBE.ServiceOptionID = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceOptionID"].ToString());
                            objBE.ServiceOptionName = ds.Tables[0].Rows[i]["ServiceOptionName"].ToString();
                            objBE.ServiceOptionBriefDesc = ds.Tables[0].Rows[i]["ServiceOptionBriefDesc"].ToString();
                            ServiceOptionList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceOptionList;
        }

        public GenericCollection<ServiceLine> GetServiceLineList()
        {
            GenericCollection<ServiceLine> ServiceLineList = new GenericCollection<ServiceLine>();
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "Get_ServiceLineList");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ServiceLine objBE = new ServiceLine();
                            objBE.ServiceLineID = Convert.ToInt32(ds.Tables[0].Rows[i]["ServiceLineID"].ToString());
                            objBE.ServiceLineDesc = ds.Tables[0].Rows[i]["ServiceLineDesc"].ToString();
                            ServiceLineList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ServiceLineList;
        }

        public GenericCollection<TransactionType> GetTransactionTypeList(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompID)
        {
            GenericCollection<TransactionType> TransactionTypeList = new GenericCollection<TransactionType>();
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@ServiceLineID", ServiceLineID);
            sqlparams[1] = new SqlParameter("@ServiceID", ServiceID);
            sqlparams[2] = new SqlParameter("@ServiceOptionID", ServiceOptionID);
            sqlparams[3] = new SqlParameter("@ServiceComponentID", ServCompID);
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, "Get_TransactionTypesList", sqlparams);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            TransactionType objBE = new TransactionType();
                            objBE.TransactionTypeID = ds.Tables[0].Rows[i]["TransactionID"].ToString();
                            objBE.TransactionTypeDesc = ds.Tables[0].Rows[i]["TransactionID"].ToString() + "," + ds.Tables[0].Rows[i]["TransactionDesc"].ToString();
                            TransactionTypeList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return TransactionTypeList;
        }



        public GenericCollection<QueryBE> GetQueryList()
        {
            GenericCollection<QueryBE> QueryList = new GenericCollection<QueryBE>();
            try
            {
                DataSet ds = null;
                ds = SqlHelper.ExecuteDataset(connection, CommandType.Text, "select * from Queries");
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            QueryBE objBE = new QueryBE();
                            objBE.SNo = Convert.ToInt16(ds.Tables[0].Rows[i]["ID"].ToString());
                            objBE.QueryName = ds.Tables[0].Rows[i]["QueryName"].ToString();
                            objBE.QueryEnv = ds.Tables[0].Rows[i]["QueryEnv"].ToString();
                            objBE.Query = ds.Tables[0].Rows[i]["Query"].ToString();
                            objBE.QueryText = objBE.QueryName + " ! " + objBE.QueryEnv;
                            QueryList.Add(i, objBE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return QueryList;
        }
    }
}

