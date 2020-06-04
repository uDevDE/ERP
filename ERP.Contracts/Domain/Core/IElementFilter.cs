
namespace ERP.Contracts.Domain.Core
{
    public interface IElementFilter
    {
        int FilterId { get; set; }
        string PropertyName { get; set; }
        string Action { get; set; }
        string Filter { get; set; }
        int UsedCounter { get; set; }
        int PlantOrderId { get; set; }
    }
}
