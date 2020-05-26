using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveSchema360.Models;

namespace VinnyB.RetrieveSchema360.Extensions
{
    public static class EntitySchemaExtensions
    {
        /// <summary>
        /// Convert EntitySchema to JSON
        /// </summary>
        /// <param name="th"></param>
        /// <returns></returns>
        public static string ToJSON(this RelationshipsModel th)
        {
            if (th.RelationshipsOneToMany != null)
                th.RelationshipsOneToMany = th.RelationshipsOneToMany.ToList().OrderByDescending(o => o.Count).ToArray();

            if (th.RelationshipsManyToMany != null)
                th.RelationshipsManyToMany = th.RelationshipsManyToMany.ToList().OrderByDescending(o => o.Count).ToArray();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(th.GetType());
                serializer.WriteObject(memoryStream, th);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
