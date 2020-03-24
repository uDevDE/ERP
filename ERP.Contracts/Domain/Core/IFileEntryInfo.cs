using ERP.Contracts.Domain.Core.Enums;

namespace ERP.Contracts.Domain.Core
{
    public interface IFileEntryInfo
    {
        int ProjectNumber { get; set; }
        string Direction { get; set; }
        string ProjectIdentifier { get; set; }
        string Section { get; set; }
        string Contraction { get; set; }
        string Description { get; set; }
        FileEntryExtensionType Extension { get; set; }
    }
}
