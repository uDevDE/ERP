using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ElementDTO : IElement
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

        [DataMember]
        public List<ElementDTO> Children { get; set; }

        [DataMember]
        public ElementType ElementType { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public string Length { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ColourInside { get; set; }

        [DataMember]
        public string ColourOutside { get; set; }

        [DataMember]
        public string ProfileNumber { get; set; }

        public ElementDTO() => Children = new List<ElementDTO>();
    }
}
