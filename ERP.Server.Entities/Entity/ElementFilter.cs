using ERP.Contracts.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("Filter")]
    public class ElementFilter : IElementFilter
    {
        [Key]
        [Required]
        public int FilterId { get; set; }

        [Required]
        public string PropertyName { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string Filter { get; set; }

        public int UsedCounter { get; set; }
        public int PlantOrderId { get; set; }
        public int EmployeeId { get; set; }
    }
}
