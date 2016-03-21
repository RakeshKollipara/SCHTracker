using BPM.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BPM.DataAccessLayer;

namespace BPM.BusinessComponents
{
    public class DropDownBC
    {

        public GenericCollection<Service> GetServiceList(string ServiceLineID)
        {
            GenericCollection<Service> ServiceList = new GenericCollection<Service>();
            DropDownDO objDO = new DropDownDO();
            try
            {
                ServiceList = objDO.GetServiceList(ServiceLineID);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return ServiceList;
        }

        public GenericCollection<ServiceComponent> GetServiceComponentList(string ServiceLineID, string ServiceID, string ServiceOptionID, string ServCompType)
        {
            GenericCollection<ServiceComponent> ServiceComponentList = new GenericCollection<ServiceComponent>();
            DropDownDO objDO = new DropDownDO();
            try
            {
                ServiceComponentList = objDO.GetServiceComponentList(ServiceLineID, ServiceID,ServiceOptionID, ServCompType);
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
            DropDownDO objDO = new DropDownDO();
            try
            {
                ServiceOptionList = objDO.GetServiceOptionList(ServiceLineID, ServiceID);
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
            DropDownDO objDO = new DropDownDO();
            try
            {
                ServiceLineList = objDO.GetServiceLineList();
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
            DropDownDO objDO = new DropDownDO();
            try
            {
                TransactionTypeList = objDO.GetTransactionTypeList(ServiceLineID, ServiceID, ServiceOptionID, ServCompID);
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
            DropDownDO objDO = new DropDownDO();
            try
            {
                QueryList = objDO.GetQueryList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return QueryList;
        }
    }
}
