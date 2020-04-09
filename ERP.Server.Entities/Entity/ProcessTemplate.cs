using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ERP.Contracts.Domain.Core;

namespace ERP.Server.Entities.Entity
{
    [Table("ProcessTemplate")]
    public class ProcessTemplate : IProcessTemplate
    {
        [Required]
        public string ProcessTemplateName { get; set; }

        [Required]
        public string Process { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ProcessNumber { get; set; }

        [Required]
        public string AV { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}
