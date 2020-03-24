
namespace ERP.Contracts.Domain.Core
{
    public interface IFolder
    {
        string Name { get; set; }
        string RelativePath { get; set; }
        bool IsRoot { get; set; }
        bool IsJob { get; set; }
        bool IsWork { get; set; }
    }
}
