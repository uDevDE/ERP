using ERP.Contracts.Domain.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("ElementInfo")]
    public class ElementInfo : IElementInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ElementInfoId { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public double Amount { get; set; }
    }
}
