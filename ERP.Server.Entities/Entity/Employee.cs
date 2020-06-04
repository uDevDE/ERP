using ERP.Contracts.Domain.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ERP.Server.Entities.Entity
{
    [Table("Employee")]
    public class Employee : IEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public string Description { get; set; }

        [Required]
        public string Alias { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public long Permissions { get; set; }

        [Required]
        public bool IsAdministrator { get; set; }

        [ForeignKey("Device")]
        public System.Guid? DeviceId { get; set; }

        public virtual Device Device { get; set; }

        public string Color { get; set; }

        public bool IsLoggedIn { get; set; }

        public bool KeepConnected { get; set; }

        public System.DateTime LastLogin { get; set; }


        //[Required]
        //public IDivision Division { get; set; }

        //public Employee() => Division = new Division();
    }
}
