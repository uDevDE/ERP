using ERP.Contracts.Domain.Core;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ProcessTemplateDTO : IProcessTemplate
    {
        [DataMember]
        public string ProcessTemplateName { get; set; }

        [DataMember]
        public string Process { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int ProcessNumber { get; set; }

        [DataMember]
        public string AV { get; set; }

        [DataMember]
        public int GroupId { get; set; }
    }
}
