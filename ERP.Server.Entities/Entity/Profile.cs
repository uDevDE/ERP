using ERP.Contracts.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("Profile")]
    public class Profile : IProfile
    {
        [Key]
        [Required]
        public int ProfileId { get; set; }

        [Required]
        public string Contraction { get; set; }

        [Required]
        public string ProfileNumber { get; set; }

        [Required]
        public double Count { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public string Length { get; set; }

        [Required]
        public string Filename { get; set; }

        [Required]
        public int PlantOrderId { get; set; }

        public string Description { get; set; }

        public int DivisionId { get; set; }

        public string Surface { get; set; }

        [ForeignKey("ElementInfo")]
        public virtual System.Collections.Generic.List<ElementInfo> ElementInfos { get; set; }

        public Profile() => ElementInfos = new System.Collections.Generic.List<ElementInfo>();
    }
}
