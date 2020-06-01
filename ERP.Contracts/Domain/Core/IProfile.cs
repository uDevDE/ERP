
namespace ERP.Contracts.Domain.Core
{
    public interface IProfile
    {
        int ProfileId { get; set; }
        string Contraction { get; set; }
        string ProfileNumber { get; set; }
        double Count { get; set; }
        double Amount { get; set; }
        string Length { get; set; }
        string Description { get; set; }
        string Surface { get; set; }
        int PlantOrderId { get; set; }
        string Filename { get; set; }
    }
}
