
namespace ERP.Contracts.Domain.Core
{
    public interface IEmployee
    {
        int EmployeeId { get; set; }
        int Number { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string Description { get; set; }
        string Alias { get; set; }
        string Password { get; set; }
        long Permissions { get; set; }
        bool IsAdministrator { get; set; }
        System.Guid? DeviceId { get; set; }
        string Color { get; set; }
        bool IsLoggedIn { get; set; }
        bool KeepConnected { get; set; }
        System.DateTime LastLogin { get; set; }
        //IDivision Division { get; set; }
    }
}
