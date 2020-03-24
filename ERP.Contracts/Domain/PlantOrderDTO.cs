using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class PlantOrderDTO : IPlantOrder
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ProcessTemplate { get; set; }

        [DataMember]
        public bool IsFinished { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Contraction { get; set; }

        [DataMember]
        public string Section { get; set; }

        [DataMember]
        public string MaterialRequirement { get; set; }

        [DataMember]
        public int LosId { get; set; }
    }
}
