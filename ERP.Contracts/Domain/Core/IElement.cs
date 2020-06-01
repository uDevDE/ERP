using ERP.Contracts.Domain.Core.Enums;
using System.Collections.Generic;

namespace ERP.Contracts.Domain.Core
{
    /// <summary>
    /// Sql Interface for table Werkauftragspositionen
    /// </summary>
    public interface IElement : IPlantOrderPosition
    {
        ElementType ElementType { get; set; }
        string Length { get; set; }
        string Name { get; set; }
        string ColourInside { get; set; }
        string ColourOutside { get; set; }
        string ProfileNumber { get; set; }
        string Contraction { get; set; }
        string Filename { get; set; }
    }
}
