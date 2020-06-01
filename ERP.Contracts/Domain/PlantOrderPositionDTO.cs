using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    /// <summary>
    /// Sql Object for table Werkauftragspositionen
    /// </summary>
    [DataContract]
    public class PlantOrderPositionDTO : IPlantOrderPosition
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int PlantOrderId { get; set; }

        [DataMember]
        public string Position { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public double Count { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string Surface { get; set; }
    }
}
