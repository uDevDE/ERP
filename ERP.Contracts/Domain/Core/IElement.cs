
using ERP.Contracts.Domain.Core.Enums;
using System.Collections.Generic;

namespace ERP.Contracts.Domain.Core
{
    /// <summary>
    /// Sql Interface for table Werkauftragspositionen
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Column ID - Werkauftrag Positions Identifier
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Column WAID - Sekundärer Schlüssel Tabelle Werkauftraege
        /// </summary>
        int PlantOrderId { get; set; }
        /// <summary>
        /// Column Position - Position
        /// </summary>
        string Position { get; set; }
        /// <summary>
        /// Column Bezeichnung - Bezeichnung / Beschreibung (Drehfenster...)
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Column Anzahl - Anzahl der vorkommenden Position innerhalb von Werkauftrag
        /// </summary>
        double Count { get; set; }
        /// <summary>
        /// Column Mengeneinheit - Mengeneinheit (Stück, Meter...)
        /// </summary>
        string Unit { get; set; }
        /// <summary>
        /// Column Oberflaeche - Oberfläche
        /// </summary>
        string Surface { get; set; }

        ElementType ElementType { get; set; }
        string Length { get; set; }
        string Name { get; set; }
        string ColourInside { get; set; }
        string ColourOutside { get; set; }
        string ProfileNumber { get; set; }
    }
}
