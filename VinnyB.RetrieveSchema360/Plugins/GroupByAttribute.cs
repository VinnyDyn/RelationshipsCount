using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveSchema360.Business;
using VinnyB.RetrieveSchema360.Extensions;

namespace VinnyB.RetrieveSchema360.Plugins
{
    public class GroupByAttribute : PluginBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GroupByAttribute() : base(typeof(GroupByAttribute)) { }

        /// <summary>
        /// Principal method
        /// </summary>
        /// <param name="localContext"> Contexto de execução. </param>

        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)
        {
            if (localContext == null) { throw new InvalidPluginExecutionException("Context not found!"); };

            //Language Code
            int languageCode = (int)localContext.PluginExecutionContext.InputParameters["LanguageCode"];
            //Record Logical Name
            string recordLogicalName = (string)localContext.PluginExecutionContext.InputParameters["RecordLogicalName"];
            //Record Id
            string recordId = (string)localContext.PluginExecutionContext.InputParameters["RecordId"];
            //Entity Logical Name
            string entityLogicalName = (string)localContext.PluginExecutionContext.InputParameters["EntityLogicalName"];
            //Atrribute Logical Name
            string attributeLogicalName = (string)localContext.PluginExecutionContext.InputParameters["AttributeLogicalName"];
            //Group By Atrribute Logical Name
            string groupByAttributeLogicalName = (string)localContext.PluginExecutionContext.InputParameters["GroupByAttributeLogicalName"];
            //Attribute Type
            string groupByAttributeType = (string)localContext.PluginExecutionContext.InputParameters["GroupByAttributeType"];
            //Intersect Entity Name
            string intersectEntityName = (string)localContext.PluginExecutionContext.InputParameters["IntersectEntityName"];
            //DAO
            var DAO = new DynamicsDAO(localContext.OrganizationService, localContext.OrganizationServiceAdmin, new EntityReference(recordLogicalName, new Guid(recordId)), languageCode);
            //Get additional details and entity count
            localContext.PluginExecutionContext.OutputParameters["GroupByResult"] = DAO.ExecuteGroupByQuery(entityLogicalName, attributeLogicalName, groupByAttributeType, groupByAttributeLogicalName, intersectEntityName).ToJSON();
        }
    }
}