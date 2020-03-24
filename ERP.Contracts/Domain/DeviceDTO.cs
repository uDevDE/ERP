using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class DeviceDTO : Core.IDevice
    {
        [DataMember]
        public Guid DeviceId { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        [DataMember]
        public bool Status { get; set; }

        [DataMember]
        public string Hostname { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public EmployeeDTO Employee { get; set; }

        [DataMember]
        public int? EmployeeId { get; set; }

        [DataMember]
        public bool IsBlocked { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public int? DivisionId { get; set; }

        [DataMember]
        public DivisionDTO Division { get; set; }
    }
}
