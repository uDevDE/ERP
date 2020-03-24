using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class FileEntryDTO : IFileEntry
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RelativePath { get; set; }

        [DataMember]
        public FileEntryInfoDTO FileInfo { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        public FileEntryDTO(string name, string relativePath, FileEntryInfoDTO fileInfo = null)
        {
            FileInfo = fileInfo;
            Name = name;
            RelativePath = relativePath;
            FilePath = System.IO.Path.Combine(RelativePath, Name).Replace('\\', '/');
        }
    }
}
