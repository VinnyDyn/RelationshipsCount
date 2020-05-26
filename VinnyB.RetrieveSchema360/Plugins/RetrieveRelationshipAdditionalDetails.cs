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
    /// Get additional details about Entity
    /// </summary>
    public class RetrieveRelationshipAdditionalDetails : PluginBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RetrieveRelationshipAdditionalDetails() : base(typeof(RetrieveRelationshipAdditionalDetails)) { }

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
            //Entity Logical Name
            string entityLogicalName = (string)localContext.PluginExecutionContext.InputParameters["EntityLogicalName"];
            //Atrribute Logical Name
            string attributeLogicalName = (string)localContext.PluginExecutionContext.InputParameters["AttributeLogicalName"];
            //Intersect Entity Name (optional)
            string intersectEntityName = (string)localContext.PluginExecutionContext.InputParameters["IntersectEntityName"];
            //DAO
            var DAO = new DynamicsDAO(localContext.OrganizationService, localContext.OrganizationServiceAdmin, new EntityReference(recordLogicalName, new Guid(recordId)), languageCode);
            //Get additional details and entity count
            localContext.PluginExecutionContext.OutputParameters["RelationshipAdditionalDetails"] = DAO.RetrieveRelationshipAdditionalDetails(entityLogicalName, attributeLogicalName, intersectEntityName).ToJSON();
        }
    }
}
