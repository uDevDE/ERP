using ERP.Contracts.Domain.Core;
using System;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ElementInfoDTO : IElementInfo
    {
        [DataMember]
        public int ElementInfoId { get; set; }

        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public double Amount { get; set; }
    }
}
