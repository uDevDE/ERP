using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class MaterialRequirementDTO : IMaterialRequirement
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int MaterialNumber { get; set; }

        [DataMember]
        public string ArticleNumber { get; set; }

        [DataMember]
        public string ArticleDescription { get; set; }

        [DataMember]
        public decimal Count { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public float Length { get; set; }

        [DataMember]
        public string SurfaceInside { get; set; }

        [DataMember]
        public string SurfaceOutside { get; set; }

        [DataMember]
        public string DescriptionStock { get; set; }

        [DataMember]
        public string Position { get; set; }
    }
}
