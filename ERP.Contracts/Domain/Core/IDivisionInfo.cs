using ERP.Contracts.Domain.Core.Enums;

namespace ERP.Contracts.Domain.Core
{
    public interface IDivisionInfo
    {
        int DivisionInfoId { get; set; }
        DivisionType DivisionType { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string MachinePath { get; set; }
    }
}
