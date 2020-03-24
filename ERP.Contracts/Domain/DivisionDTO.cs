using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class DivisionDTO : IDivision
    {
        public DivisionDTO() => DivisionType = new DivisionInfoDTO();

        [DataMember]
        public int DivisionId { get; set; }

        [DataMember]
        public int DivisionInfoId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DivisionInfoDTO DivisionType { get; set; }
    }
}
