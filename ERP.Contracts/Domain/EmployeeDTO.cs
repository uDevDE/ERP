using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class EmployeeDTO : IEmployee
    {
        [DataMember]
        public int EmployeeId { get; set; }

        [DataMember]
        public int Number { get; set; }

        [DataMember]
        public string Alias { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Firstname { get; set; }

        [DataMember]
        public string Lastname { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public long Permissions { get; set; }

        [DataMember]
        public bool IsAdministrator { get; set; }

        [DataMember]
        public System.Guid? DeviceId { get; set; }

        [DataMember]
        public DeviceDTO Device { get; set; }

        [DataMember]
        public string Color { get; set; }


        //[DataMember]
        //public IDivision Division { get; set; }

        //public EmployeeDTO() => Division = new DivisionDTO();
    }
}
