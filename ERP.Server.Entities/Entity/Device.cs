using ERP.Contracts.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("Device")]
    public class Device : IDevice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid DeviceId { get; set; }

        [Required]
        public string IpAddress { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public string Hostname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsVerified { get; set; }

        [ForeignKey("Employee")]
        public int? EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [ForeignKey("Division")]
        public int? DivisionId { get; set; }

        public Division Division { get; set; }
    }
}
