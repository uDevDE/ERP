

namespace ERP.Contracts.Domain.Core
{
    /// <summary>
    /// Table: Processvorlagen
    /// </summary>
    public interface IProcessTemplate
    {
        /// <summary>
        /// Column: VorlagenName
        /// </summary>
        string ProcessTemplateName { get; set; }

        /// <summary>
        /// Column: Process
        /// </summary>
        string Process { get; set; }

        /// <summary>
        /// Column: Beschreibung
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Column: lfdNr
        /// </summary>
        int ProcessNumber { get; set; }

        /// <summary>
        /// Column: AV
        /// </summary>
        string AV { get; set; }

        /// <summary>
        /// Column: GruppenID
        /// </summary>
        int GroupId { get; set; }
    }
}
