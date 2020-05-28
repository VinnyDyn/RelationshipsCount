using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinnyB.RetrieveSchema360.Extensions
{
    public static class LabelExtensions
    {
        /// <summary>
        /// Get the right label based on languageCode
        /// </summary>
        /// <param name="th"></param>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public static string GetLabel(this Microsoft.Xrm.Sdk.Label th, int? languageCode = null)
        {
            string label = string.Empty;
            if (languageCode == null)
                label = th.LocalizedLabels.Select(s => s.Label).FirstOrDefault();
            else
            {
                label = th.LocalizedLabels.Where(w => w.LanguageCode == languageCode).Select(s => s.Label).FirstOrDefault();
                if (string.IsNullOrEmpty(label))
                    label = th.LocalizedLabels.Select(s => s.Label).FirstOrDefault();
            }

            if (string.IsNullOrEmpty(label))
                label = "--";

            return label;
        }
    }
}
