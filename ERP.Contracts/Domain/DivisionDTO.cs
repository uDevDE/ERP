﻿using ERP.Contracts.Domain.Core;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class DivisionDTO : IDivision
    {
        public DivisionDTO()
        {
            DivisionType = new DivisionInfoDTO();
            ProcessTemplates = new List<ProcessTemplateDTO>();
        }

        [DataMember]
        public int DivisionId { get; set; }

        [DataMember]
        public int DivisionInfoId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DivisionInfoDTO DivisionType { get; set; }

        [DataMember]
        public List<ProcessTemplateDTO> ProcessTemplates { get; set; }
    }
}
