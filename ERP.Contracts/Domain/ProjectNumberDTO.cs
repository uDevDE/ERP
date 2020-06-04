using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ProjectNumberDTO : IProjectNumber
    {
        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
