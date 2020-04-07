using ERP.Contracts.Domain.Core;
using System;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class PlantOrderProcessDTO : IPlantOrderProcess
    {
        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public string Process { get; set; }

        [DataMember]
        public int ProcessNumber { get; set; }

        [DataMember]
        public Guid ProcessGuid { get; set; }

        [DataMember]
        public int GroupId { get; set; }
    }
}
