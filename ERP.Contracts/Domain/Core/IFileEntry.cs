
namespace ERP.Contracts.Domain.Core
{
    public interface IFileEntry
    {
        string Name { get; set; }
        string RelativePath { get; set; }
        string FilePath { get; set; }
    }
}
