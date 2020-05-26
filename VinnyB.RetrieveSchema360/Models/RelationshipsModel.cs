using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Models
{
    [DataContract]
    public class RelationshipsModel
    {
        [DataMember] 
        public string RecordId { get; set; }

        [DataMember]
        public string LastUpdate { get; set; }

        [DataMember]
        public RelationshipModel[] RelationshipsOneToMany { get; set; }

        [DataMember]
        public RelationshipModel[] RelationshipsManyToMany { get; set; }
    }

    public class Attribute
    {
        public string Label { get; set; }
        public string LogicalName { get; set; }
        public string SchemaName { get; set; }
    }
}