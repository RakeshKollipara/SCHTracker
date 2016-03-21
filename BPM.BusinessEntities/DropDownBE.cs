using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPM.BusinessEntities
{
    public class ServiceLine
    {
        public int ServiceLineID { get; set; }
        public string ServiceLineDesc { get; set; }
    }

    public class Service
    {
        public int ServiceID { get; set; }
        public int ServiceLineID { get; set; }
        public string ServiceDesc { get; set; }
    }

    public class ServiceOption
    {
        public int ServiceOptionID { get; set; }
        public int ServiceID { get; set; }
        public string ServiceOptionName { get; set; }
        public string ServiceOptionBriefDesc { get; set; }
    }

    public class ServiceComponent
    {
        public int ServiceComponentID { get; set; }
        public int DependencyType { get; set; }
        public string ServiceComponentDesc { get; set; }
    }

    public class DependencyType
    {
        public int DependencyTypeID { get; set; }
        public string DependencyTypeDesc { get; set; }
    }

    public class TransactionType
    {
        public string TransactionTypeID { get; set; }
        public string TransactionTypeDesc { get; set; }
    }
}
