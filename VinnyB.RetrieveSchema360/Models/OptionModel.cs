﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Models
{
    [DataContract]
    public class OptionModel
    {
        [DataMember(Name = "DN")]
        public string DisplayName { get; set; }

        [DataMember(Name = "HEX")]
        public string Hexadecimal { get; set; }

        [DataMember(Name = "V")]
        public int? Value { get; set; }

        [DataMember(Name = "C")]
        public int Count { get; set; }
    }
}
