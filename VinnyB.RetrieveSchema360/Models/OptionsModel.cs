using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Models
{
    [DataContract]
    public class OptionsModel
    {
        [DataMember]
        public OptionModel[] Options { get; set; }

        public OptionModel GetOption(int? value)
        {
            return this.Options.Where(w => w.Value == value).FirstOrDefault();
        }
    }
}
