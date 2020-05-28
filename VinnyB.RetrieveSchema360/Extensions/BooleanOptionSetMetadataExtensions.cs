using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveSchema360.Models;

namespace VinnyB.RetrieveSchema360.Extensions
{
    public static class BooleanOptionSetMetadataExtensions
    {
        public static OptionsModel Parse(this BooleanOptionSetMetadata th, int? languageCode = null)
        {
            var options = new List<OptionModel>();

            var nullOption = new OptionModel();
            nullOption.DisplayName = "Null";
            nullOption.Hexadecimal = "#000000";
            nullOption.Value = null;
            nullOption.Count = 0;
            options.Add(nullOption);

            var trueOption = new OptionModel();
            trueOption.DisplayName = th.TrueOption.Label.GetLabel(languageCode);
            trueOption.Hexadecimal = th.TrueOption.Color;
            trueOption.Value = th.TrueOption.Value.Value;
            trueOption.Count = 0;
            options.Add(trueOption);

            var falseOption = new OptionModel();
            falseOption.DisplayName = th.FalseOption.Label.GetLabel(languageCode);
            falseOption.Hexadecimal = th.FalseOption.Color;
            falseOption.Value = th.FalseOption.Value.Value;
            falseOption.Count = 0;
            options.Add(falseOption);

            return new OptionsModel() { Options = options.ToArray() };
        }
    }
}
