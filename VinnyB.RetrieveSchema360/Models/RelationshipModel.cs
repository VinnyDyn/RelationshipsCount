using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Models
{
    [DataContract]
    public class RelationshipModel
    {
        [DataMember(Name = "DN")]
        public string DisplayName { get; set; }
        [DataMember(Name = "SN")]
        public string SchemaName { get; set; }
        [DataMember(Name = "RE")]
        public string ReferencingEntity { get; set; }
        [DataMember(Name = "RA")]
        public string ReferencingAttribute { get; set; }
        [DataMember(Name = "RT")]
        public int RelationshipType { get; set; }
        [DataMember(Name = "RN")]
        public string RelationshipName { get; set; }
        [DataMember(Name = "C")]
        public int Count { get; set; }
    }
}
