using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Models
{
    [DataContract]
    public class RelationshipDetailsModel
    {
        [DataMember(Name = "DN")]
        public string DisplayName { get; set; }

        [DataMember(Name = "C")]
        public int Count { get; set; }
    }
}
