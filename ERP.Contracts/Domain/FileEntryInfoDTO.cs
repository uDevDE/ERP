using ERP.Contracts.Domain.Core;
using ERP.Contracts.Domain.Core.Enums;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class FileEntryInfoDTO : IFileEntryInfo
    {
        [DataMember]
        public int ProjectNumber { get; set; }

        [DataMember]
        public string Direction { get; set; }

        [DataMember]
        public string ProjectIdentifier { get; set; }

        [DataMember]
        public string Section { get; set; }

        [DataMember]
        public string Contraction { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public FileEntryExtensionType Extension { get; set; }

        public FileEntryInfoDTO() => Extension = FileEntryExtensionType.Pdf;
    }
}
