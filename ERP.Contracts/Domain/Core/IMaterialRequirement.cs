
namespace ERP.Contracts.Domain.Core
{
    public interface IMaterialRequirement
    {
        /// <summary>
        /// Column ID - Identifier
        /// </summary>
        int Id { get; set; }
        /// <summary>
        /// Column Mat-Nr
        /// </summary>
        int MaterialNumber { get; set; }
        /// <summary>
        /// Column Artikel-Nr
        /// </summary>
        string ArticleNumber { get; set; }
        /// <summary>
        /// Column Artikelbezeichnung
        /// </summary>
        string ArticleDescription { get; set; }
        /// <summary>
        /// Column Menge
        /// </summary>
        decimal Count { get; set; }
        /// <summary>
        /// Column Mengeneinheit
        /// </summary>
        string Unit { get; set; }
        /// <summary>
        /// Column Länge
        /// </summary>
        float Length { get; set; }
        /// <summary>
        /// Column Oberfl_Inn
        /// </summary>
        string SurfaceInside { get; set; }
        /// <summary>
        /// Column Oberfl_Aus
        /// </summary>
        string SurfaceOutside { get; set; }
        /// <summary>
        /// Column Bemerkung_Lager
        /// </summary>
        string DescriptionStock { get; set; }
        /// <summary>
        /// Column Pos
        /// </summary>
        string Position { get; set; }
    }
}
