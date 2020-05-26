using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveSchema360.Business;
using VinnyB.RetrieveSchema360.Extensions;
using VinnyB.RetrieveSchema360.Models;

namespace VinnyB.RetrieveSchema360.Plugins
{
    /// <summary>
    /// Get informations about Relationships
    /// </summary>
    public class RetrieveRelationships : PluginBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RetrieveRelationships() : base(typeof(RetrieveRelationships)) { }

        /// <summary>
        /// Principal method
        /// </summary>
        /// <param name="localContext"> Contexto de execução. </param>

        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
        {
            if (localContext == null) { throw new InvalidPluginExecutionException("Context not found!"); };

            //Language Code
            int languageCode = (int)localContext.PluginExecutionContext.InputParameters["LanguageCode"];
            //Record Id
            string recordId = (string)localContext.PluginExecutionContext.InputParameters["RecordId"];
            //Record Logical Name
            string recordLogicalName = (string)localContext.PluginExecutionContext.InputParameters["RecordLogicalName"];
            //One To Many Relationships
            string oneToManyRelationships = (string)localContext.PluginExecutionContext.InputParameters["OneToManyRelationships"];
            //Many to Many Relationships
            string manyToManyRelationships = (string)localContext.PluginExecutionContext.InputParameters["ManyToManyRelationships"];

            var DAO = new DynamicsDAO(localContext.OrganizationService, localContext.OrganizationServiceAdmin, new EntityReference(recordLogicalName, new Guid(recordId)), languageCode);
            localContext.PluginExecutionContext.OutputParameters["Relationships"] = DAO.RetrieveRelationships(oneToManyRelationships, manyToManyRelationships).ToJSON();
        }
    }
}
