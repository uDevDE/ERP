using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("DivisionInfo")]
    public class DivisionInfo : IDivisionInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DivisionInfoId { get; set; }

        [Required]
        public DivisionType DivisionType { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string MachinePath { get; set; }


    }
}
