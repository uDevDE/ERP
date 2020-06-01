using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ProfileDTO : IProfile
    {
        [DataMember]
        public int ProfileId { get; set; }

        [DataMember]
        public string Contraction { get; set; }

        [DataMember]
        public string ProfileNumber { get; set; }

        [DataMember]
        public double Count { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public string Length { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Surface { get; set;  }

        [DataMember]
        public int PlantOrderId { get; set; }

        [DataMember]
        public string Filename { get; set; }
    }
}
