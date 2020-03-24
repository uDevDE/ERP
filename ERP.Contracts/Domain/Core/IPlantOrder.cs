
namespace ERP.Contracts.Domain.Core
{
    /// <summary>
    /// Sql Interface for table Werkauftraege
    /// </summary>
    public interface IPlantOrder
    {
        /// <summary>
        /// Column WAID - Werkauftrag Identifier
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Column WANr - Werkauftragnummer
        /// </summary>
        string Number { get; set; }
        /// <summary>
        /// Column ParentWAID - übergeordneter Werkauftrag
        /// </summary>
        int ParentId { get; set; }
        /// <summary>
        /// Column Name - Werkauftrag Name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Column Prozessvorlage - Prozessvorlage
        /// </summary>
        string ProcessTemplate { get; set; }
        /// <summary>
        /// Column Abgeschlossen - Werkauftrag abgeschlossen
        /// </summary>
        bool IsFinished { get; set; }
        /// <summary>
        /// Column User1 - Position Beschreibung
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Column User4 - Kürzel (FEN, PR...)
        /// </summary>
        string Contraction { get; set; }
        /// <summary>
        /// Column User6 - MP
        /// </summary>
        string Section { get; set; }
        /// <summary>
        /// Column User7 - Alle Materialanforderungen
        /// </summary>
        string MaterialRequirement { get; set; }
        /// <summary>
        /// Column LosID - Sekundärer Schlüssel zur Tabelle Lose
        /// </summary>
        int LosId { get; set; }
    }
}
