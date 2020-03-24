using ERP.Contracts.Domain.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class FolderDTO : IFolder
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string RelativePath { get; set; }

        [DataMember]
        public bool IsRoot { get; set; }

        [DataMember]
        public bool IsJob { get; set; }

        [DataMember]
        public bool IsWork { get; set; }

        [DataMember]
        public Dictionary<string, FileEntryDTO> Files { get; set; }

        [DataMember]
        public Dictionary<string, FolderDTO> SubFolders { get; set; }

        public FolderDTO(string name, string relativePath, bool isJob = false, bool isWork = false, bool isRoot = false)
        {
            Name = name;
            RelativePath = relativePath;
            IsRoot = isRoot;
            IsJob = isJob;
            IsWork = isWork;
            Files = new Dictionary<string, FileEntryDTO>();
            SubFolders = new Dictionary<string, FolderDTO>();
        }


    }
}
