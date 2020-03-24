using System;

namespace ERP.Contracts.Domain.Core
{
    public interface IDevice
    {
        Guid DeviceId { get; set; }
        string IpAddress { get; set; }
        bool Status { get; set; }
        string Hostname { get; set; }
        string Username { get; set; }  
        int? EmployeeId { get; set; }
        bool IsBlocked { get; set; }
        bool IsVerified { get; set; }
        int? DivisionId { get; set; }
    }
}
