using ERP.Contracts.Domain.Core.Enums;

namespace ERP.Contracts.Domain.Core
{
    public interface IDivision
    {
        int DivisionId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int DivisionInfoId { get; set; }
    }
}
