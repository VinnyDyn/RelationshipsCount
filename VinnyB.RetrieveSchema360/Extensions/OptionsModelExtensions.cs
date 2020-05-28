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
    public static class OptionsModelExtensions
    {
        /// <summary>
        /// Convert RelationshipDetails to JSON
        /// </summary>
        /// <param name="th"></param>
        /// <returns></returns>
        public static string ToJSON(this OptionsModel th)
        {
            th.Options = th.Options.OrderBy(o => o.DisplayName).ToArray();
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(th.GetType());
                serializer.WriteObject(memoryStream, th);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }
}
