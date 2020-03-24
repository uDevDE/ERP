using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ERP.Contracts.Domain
{
    [DataContract]
    public class PdfFile
    {
        [DataMember]
        public byte[] Buffer { get; set; }

        [DataMember]
        public string Filename { get; set; }

        [DataMember]
        public long FileSize { get; set; }

        public static bool IsPdfFileValid(PdfFile pdfFile) => pdfFile.FileSize == pdfFile.Buffer?.LongLength;
    }
}
