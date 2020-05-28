using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Models
{
    [DataContract]
    public class AttributeModel
    {
        [DataMember(Name = "LN")]
        public string LogicalName { get; set; }

        [DataMember(Name = "DN")]
        public string DisplayName { get; set; }

        [DataMember(Name = "T")]
        public int Type { get; set; }
    }
}
