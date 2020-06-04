using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ElementFilterDTO : IElementFilter
    {
        [DataMember]
        public int FilterId { get; set; }

        [DataMember]
        public string PropertyName { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public string Filter { get; set; }

        [DataMember]
        public int UsedCounter { get; set; }

        [DataMember]
        public int PlantOrderId { get; set; }

        [DataMember]
        public int EmployeeId { get; set; }
    }
}
