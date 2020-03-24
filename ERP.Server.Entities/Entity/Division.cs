using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("Division")]
    public class Division : IDivision
    {
        public Division() => DivisionType = new DivisionInfo();

        [Key]
        [Required]
        public int DivisionId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
       
        public virtual DivisionInfo DivisionType { get; set; }

        [ForeignKey("DivisionType")]
        public int DivisionInfoId { get; set; }
    }
}
