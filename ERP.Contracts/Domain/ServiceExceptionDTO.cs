using System;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class ServiceExceptionDTO
    {
        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public Exception Exception { get; set; }
    }
}
