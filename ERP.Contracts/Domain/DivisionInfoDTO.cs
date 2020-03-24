using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    public class DivisionInfoDTO : IDivisionInfo
    {
        [DataMember]
        public int DivisionInfoId { get; set; }

        [DataMember]
        public DivisionType DivisionType { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string MachinePath { get; set; }
    }
}
