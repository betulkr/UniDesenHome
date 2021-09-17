using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Unibim.Melanom.WebUI
{
    [DataContract]
    [Serializable]
    public class FetchParameters
    {
        [DataMember]
        [Required]
        public int PageIndex { get; set; }

        [DataMember]
        [Required]
        public int PageCount { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        [Required]
        public string SortExpression { get; set; }

        [DataMember]
        public string SearchKey { get; set; }
    }
}
