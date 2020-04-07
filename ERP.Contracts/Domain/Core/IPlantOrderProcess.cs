
namespace ERP.Contracts.Domain.Core
{
    public interface IPlantOrderProcess
    {
        int ProcessId { get; set; }
        string Process { get; set; }
        int ProcessNumber { get; set; }
        System.Guid ProcessGuid { get; set; }
        int GroupId { get; set; }
    }
}
