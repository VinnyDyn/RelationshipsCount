using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveSchema360.Models;

namespace VinnyB.RetrieveSchema360.Extensions
{

    public static class OptionMetadataCollectionExtensions
    {
        public static OptionsModel Parse(this OptionMetadataCollection th, int? languageCode = null)
        {
            var options = new List<OptionModel>();

            var nullOption = new OptionModel();
            nullOption.DisplayName = "Null";
            nullOption.Hexadecimal = "#000000";
            nullOption.Value = null;
            nullOption.Count = 0;
            options.Add(nullOption);

            foreach (var option_ in th)
            {
                var option = new OptionModel();
                option.DisplayName = option_.Label.GetLabel(languageCode);
                option.Hexadecimal = option_.Color;
                option.Value = option_.Value.Value;
                option.Count = 0;
                options.Add(option);

            }
            return new OptionsModel() { Options = options.ToArray() };
        }
    }
}
