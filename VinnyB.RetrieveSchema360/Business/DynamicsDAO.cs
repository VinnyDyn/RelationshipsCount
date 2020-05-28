using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using VinnyB.RetrieveSchema360.Extensions;
using VinnyB.RetrieveSchema360.Models;

namespace VinnyB.RetrieveSchema360.Business
{
    public class DynamicsDAO
    {
        private IOrganizationService Service;
        private IOrganizationService ServiceAdmin;
        private int? LanguageCode;
        private EntityReference Record;

        public DynamicsDAO(IOrganizationService service, IOrganizationService serviceAdmin, EntityReference record, int? languageCode = null)
        {
            this.Service = service;
            this.ServiceAdmin = serviceAdmin;
            this.Record = record;
            this.LanguageCode = languageCode;
        }

        #region Main Methods
        public RelationshipsModel RetrieveRelationships(string oneToMany, string manyToMany)
        {
            //Define a contract to return
            RelationshipsModel entitySchemaModel = new RelationshipsModel();
            entitySchemaModel.RecordId = $"S360-{this.Record.Id.ToString()}";
            entitySchemaModel.LastUpdate = DateTime.UtcNow.ToString("G");

            //Request all entity relationships
            RetrieveEntityRequest request = new RetrieveEntityRequest()
            {
                LogicalName = this.Record.LogicalName,
                EntityFilters = EntityFilters.Relationships
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse)ServiceAdmin.Execute(request);
            if (response != null)
            {
                entitySchemaModel.RelationshipsOneToMany = GetOneToManyRelationships(response, oneToMany);
                entitySchemaModel.RelationshipsManyToMany = GetManyToManyRelationships(response, manyToMany);
            }

            return entitySchemaModel;
        }
        public RelationshipDetailsModel RetrieveRelationshipAdditionalDetails(string entityLogicalName, string attributeLogicalName, string intersectEntityName)
        {
            //Retorn
            var relationshipDetailsModel = new RelationshipDetailsModel();
            //Request Additional Details
            RetrieveEntityRequest request = new RetrieveEntityRequest()
            {
                LogicalName = entityLogicalName.ToLower(),
                EntityFilters = EntityFilters.Default | EntityFilters.Attributes
            };
            RetrieveEntityResponse response = (RetrieveEntityResponse)ServiceAdmin.Execute(request);

            //Display Name
            relationshipDetailsModel.DisplayName = response.EntityMetadata.DisplayName.GetLabel(this.LanguageCode);

            //One To Many
            if (string.IsNullOrEmpty(intersectEntityName))
                relationshipDetailsModel.Count = GetRecordsOneToMany(entityLogicalName, attributeLogicalName);
            else
                relationshipDetailsModel.Count = GetRecordsManyToMany(intersectEntityName, attributeLogicalName);

            //Attributes
            if (relationshipDetailsModel.Count > 0)
                relationshipDetailsModel.Attributes = GetAttributes(response);
            else
                relationshipDetailsModel.Attributes = new AttributeModel[0];

            return relationshipDetailsModel;
        }
        public OptionsModel ExecuteGroupByQuery(string entityLogicalName, string attributeLogicalName, string groupByattributeType, string groupByAttributeLogicalName, string intersectEntityName)
        {
            //Return
            var optionsModel = new OptionsModel();

            //Different rules by type
            var type = (AttributeTypeCode)Enum.Parse(typeof(AttributeTypeCode), groupByattributeType);
            switch (type)
            {
                case AttributeTypeCode.Picklist:
                    optionsModel = this.GetOptionsSetTextOnValue(entityLogicalName, groupByAttributeLogicalName).Parse(this.LanguageCode);
                    break;

                case AttributeTypeCode.Status:
                    optionsModel = this.GetStatusCodeOptionsSetTextOnValue(entityLogicalName, groupByAttributeLogicalName).Parse(this.LanguageCode);
                    break;

                case AttributeTypeCode.State:
                    optionsModel = this.GetStateCodeOptionsSetTextOnValue(entityLogicalName, groupByAttributeLogicalName).Parse(this.LanguageCode);
                    break;

                case AttributeTypeCode.Boolean:
                    optionsModel = this.GetBooleanTextOnValue(entityLogicalName, groupByAttributeLogicalName).Parse(this.LanguageCode);
                    break;

                default:
                    throw new InvalidPluginExecutionException($"Invalid type {groupByattributeType}");
                    break;
            }

            //1:N
            if (string.IsNullOrEmpty(intersectEntityName))
                return this.GetRecordsOneToManyGroupBy(entityLogicalName, attributeLogicalName, groupByAttributeLogicalName, optionsModel);
            //N:N
            else
                return this.GetRecordsManyToManyGroupBy(entityLogicalName, attributeLogicalName, groupByAttributeLogicalName, intersectEntityName, optionsModel);
        }
        #endregion

        #region Metadata
        private RelationshipModel[] GetOneToManyRelationships(RetrieveEntityResponse response, string oneToMany)
        {
            //Return
            var relationshipOneToManyModels = new List<RelationshipModel>();

            // Searchables and Visible
            var oneToManyRelationships = response.EntityMetadata.OneToManyRelationships
                .Where(w => w.IsValidForAdvancedFind == true)
                .ToList();

            // With Filter
            if (!string.IsNullOrEmpty(oneToMany))
            {
                //Trim
                oneToMany = oneToMany.Trim();

                var requestedRelationships = oneToMany.Split(',');
                if (requestedRelationships.Length > 0)
                {
                    for (int i = 0; i < requestedRelationships.Length; i++)
                    {
                        //Entity Name
                        var entityName = requestedRelationships[i];
                        if (!string.IsNullOrEmpty(entityName))
                        {
                            //Get the relationship by entity name
                            var relationship = oneToManyRelationships.Where(w => w.ReferencingEntity == entityName.ToLower()).FirstOrDefault();
                            if (relationship != null)
                            {
                                var model = new RelationshipModel();
                                model.SchemaName = relationship.SchemaName;
                                model.RelationshipType = RelationshipType.OneToMany.GetHashCode();
                                model.DisplayName = relationship.AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                                model.ReferencingEntity = relationship.ReferencingEntity;
                                model.ReferencingAttribute = relationship.ReferencingAttribute;
                                model.RelationshipName = relationship.AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                                model.Count = 0;
                                relationshipOneToManyModels.Add(model);
                            }
                        }
                    }
                }
            }
            // Without Filter
            else
            {
                foreach (var relationship in oneToManyRelationships)
                {
                    var model = new RelationshipModel();
                    model.SchemaName = relationship.SchemaName;
                    model.RelationshipType = RelationshipType.OneToMany.GetHashCode();
                    model.DisplayName = relationship.AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                    model.ReferencingEntity = relationship.ReferencingEntity;
                    model.ReferencingAttribute = relationship.ReferencingAttribute;
                    model.RelationshipName = relationship.AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                    model.Count = 0;
                    relationshipOneToManyModels.Add(model);
                }
            }
            return relationshipOneToManyModels.OrderBy(o => o.DisplayName).ToArray();
        }
        private RelationshipModel[] GetManyToManyRelationships(RetrieveEntityResponse response, string manyToMany)
        {
            //Return
            var relationshipManyToManyModel = new List<RelationshipModel>();

            // Searchables and Visible
            var manyToManyRelationships = response.EntityMetadata.ManyToManyRelationships
                .Where(w => w.IsValidForAdvancedFind == true)
                .ToList();

            // With Filter
            if (!string.IsNullOrEmpty(manyToMany))
            {
                //Trim
                manyToMany = manyToMany.Trim();

                var requestedRelationships = manyToMany.Split(',');
                if (requestedRelationships.Length > 0)
                {
                    for (int i = 0; i < requestedRelationships.Length; i++)
                    {
                        //Intersect Entity Name
                        var intersectEntityName = requestedRelationships[i];
                        if (!string.IsNullOrEmpty(intersectEntityName))
                        {
                            //Get the relationship by intersect entity name
                            var relationship = manyToManyRelationships.Where(w => w.IntersectEntityName == intersectEntityName).FirstOrDefault();
                            if (relationship != null)
                            {
                                var model = new RelationshipModel();
                                model.SchemaName = relationship.IntersectEntityName;
                                model.RelationshipType = RelationshipType.ManyToMany.GetHashCode();
                                model.DisplayName = relationship.Entity1AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                                model.ReferencingEntity = relationship.Entity1LogicalName;
                                model.ReferencingAttribute = relationship.Entity2IntersectAttribute;
                                model.RelationshipName = relationship.Entity2AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                                model.Count = 0;
                                relationshipManyToManyModel.Add(model);
                            }
                        }
                    }
                }
            }
            // Without Filter
            else
            {
                foreach (var relationship in manyToManyRelationships)
                {
                    var model = new RelationshipModel();
                    model.SchemaName = relationship.IntersectEntityName;
                    model.RelationshipType = RelationshipType.ManyToMany.GetHashCode();
                    model.DisplayName = relationship.Entity1AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                    model.ReferencingEntity = relationship.Entity1LogicalName;
                    model.ReferencingAttribute = relationship.Entity2IntersectAttribute;
                    model.RelationshipName = relationship.Entity2AssociatedMenuConfiguration.Label.GetLabel(this.LanguageCode);
                    model.Count = 0;
                    relationshipManyToManyModel.Add(model);
                }
            }

            return relationshipManyToManyModel.ToArray();
        }
        private AttributeModel[] GetAttributes(RetrieveEntityResponse response)
        {
            //Attributes
            List<AttributeModel> attributes = new List<AttributeModel>();
            foreach (var attribute_ in response.EntityMetadata.Attributes
                .Where(w => w.AttributeType != null
                && (w.AttributeType == AttributeTypeCode.Boolean
                || w.AttributeType == AttributeTypeCode.Picklist
                || w.AttributeType == AttributeTypeCode.Status
                || w.AttributeType == AttributeTypeCode.State)))
            {
                var attributeModel = new AttributeModel();
                attributeModel.DisplayName = attribute_.DisplayName.GetLabel(this.LanguageCode);
                attributeModel.LogicalName = attribute_.LogicalName;
                attributeModel.Type = attribute_.AttributeType.GetHashCode();
                attributes.Add(attributeModel);
            }
            return attributes.ToArray();
        }
        #endregion

        #region Labels
        private OptionMetadataCollection GetOptionsSetTextOnValue(string entityName, string attributeName)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)Service.Execute(retrieveAttributeRequest);
            PicklistAttributeMetadata attributeMetadata = (PicklistAttributeMetadata)retrieveAttributeResponse?.AttributeMetadata;
            if (attributeMetadata == null) return null;
            return attributeMetadata?.OptionSet?.Options;
        }
        private OptionMetadataCollection GetStatusCodeOptionsSetTextOnValue(string entityName, string attributeName)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)Service.Execute(retrieveAttributeRequest);
            StatusAttributeMetadata attributeMetadata = (StatusAttributeMetadata)retrieveAttributeResponse?.AttributeMetadata;
            if (attributeMetadata == null) return null;
            return attributeMetadata?.OptionSet?.Options;
        }
        private OptionMetadataCollection GetStateCodeOptionsSetTextOnValue(string entityName, string attributeName)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)Service.Execute(retrieveAttributeRequest);
            StateAttributeMetadata attributeMetadata = (StateAttributeMetadata)retrieveAttributeResponse?.AttributeMetadata;
            if (attributeMetadata == null) return null;
            return attributeMetadata?.OptionSet?.Options;
        }
        public BooleanOptionSetMetadata GetBooleanTextOnValue(string entityName, string attributeName)
        {
            RetrieveAttributeRequest retrieveAttributeRequest = new RetrieveAttributeRequest
            {
                EntityLogicalName = entityName,
                LogicalName = attributeName,
                RetrieveAsIfPublished = true
            };
            RetrieveAttributeResponse retrieveAttributeResponse = (RetrieveAttributeResponse)Service.Execute(retrieveAttributeRequest);
            BooleanAttributeMetadata attributeMetadata = (BooleanAttributeMetadata)retrieveAttributeResponse?.AttributeMetadata;
            if (attributeMetadata == null) return null;
            return attributeMetadata?.OptionSet;
        }
        #endregion

        #region Queries
        private int GetRecordsOneToMany(string entityLogicalName, string attributeLogicalName)
        {
            int count = 0;
            try
            {
                var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                          <entity name='{entityLogicalName}'>
                            <attribute name='{attributeLogicalName.ToLower()}' aggregate='count' alias='count'/>   
                            <filter type='and'>
                              <condition attribute='{attributeLogicalName.ToLower()}' operator='eq' value='{Record.Id}'/>
                            </filter>
                          </entity>
                        </fetch>";

                EntityCollection result = Service.RetrieveMultiple(new FetchExpression(query));

                foreach (var count_ in result.Entities) { count = (Int32)((AliasedValue)count_["count"]).Value; }
            }
            catch (FaultException fe)
            {
                if (!fe.Message.Contains("is missing prvRead") && !fe.Message.Contains("SharePoint S2S and MSTeams integration is not enabled for this org"))
                    throw new InvalidPluginExecutionException(fe.Message);
            }
            catch (KeyNotFoundException knfe)
            {
                // Some 1: N relationships do not correctly return the 'customerid' attribute
                if ("contactid".Equals(attributeLogicalName.ToLower()) || "accountid".Equals(attributeLogicalName.ToLower()))
                    count = GetRecordsOneToMany(entityLogicalName, "customerid");
                else
                    throw new InvalidPluginExecutionException(knfe.Message);
            }
            return count;
        }
        private int GetRecordsManyToMany(string relationshipSchemaName, string attributeLogicalName)
        {
            int count = 0;
            try
            {
                var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
                          <entity name='{relationshipSchemaName}'>
                            <attribute name='{attributeLogicalName.ToLower()}' aggregate='count' alias='count'/>   
                            <filter type='and'>
                              <condition attribute='{attributeLogicalName.ToLower()}' operator='eq' value='{Record.Id}'/>
                            </filter>
                          </entity>
                        </fetch>";

                EntityCollection result = Service.RetrieveMultiple(new FetchExpression(query));

                foreach (var count_ in result.Entities) { count = (Int32)((AliasedValue)count_["count"]).Value; }
            }
            catch (FaultException fe)
            {
                if (!fe.Message.Contains("is missing prvRead"))
                    throw new InvalidPluginExecutionException(fe.Message);
            }
            return count;
        }
        private OptionsModel GetRecordsOneToManyGroupBy(string entityLogicalName, string attributeLogicalName, string groupByAttributeLogicalName, OptionsModel optionsModel)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' aggregate='true'>
                          <entity name='{entityLogicalName}'>
                            <attribute name='{groupByAttributeLogicalName}' groupby='true' alias='group'/>
                            <attribute name='{attributeLogicalName}' aggregate='count' alias='count'/>   
                            <filter type='and'>
                              <condition attribute='{attributeLogicalName}' operator='eq' value='{Record.Id}'/>
                            </filter>
                          </entity>
                        </fetch>";

            EntityCollection result = Service.RetrieveMultiple(new FetchExpression(query));

            foreach (var item in result.Entities)
            {

                int? value = null;
                if (item.Contains("group"))
                {
                    var groupby = ((AliasedValue)item["group"]).Value;
                    // Define the type
                    if (groupby.GetType() == typeof(OptionSetValue))
                        value = ((OptionSetValue)groupby).Value;

                    else if (groupby.GetType() == typeof(Boolean))
                        value = Convert.ToInt32((Boolean)groupby);
                }
                else
                    value = null;

                // Get the right option
                var option = optionsModel.GetOption(value);
                if (option != null)
                    option.Count = (Int32)((AliasedValue)item["count"]).Value;
            }

            return optionsModel;
        }
        private OptionsModel GetRecordsManyToManyGroupBy(string entityLogicalName, string attributeLogicalName, string groupByAttributeLogicalName, string intersectEntityName, OptionsModel optionsModel)
        {
            var query = $@"<fetch version='1.0' output-format='xml-platform' mapping='logical' aggregate='true'>
                          <entity name='{entityLogicalName}'>
                            <attribute name='{groupByAttributeLogicalName}' groupby='true' alias='group'/>
                            <attribute name='{groupByAttributeLogicalName}' aggregate='count' alias='count'/>   
                                <link-entity name='{intersectEntityName}' from='{entityLogicalName}id' to='{entityLogicalName}id' visible='false' intersect='true'>
                                  <link-entity name='{Record.LogicalName}' from='{Record.LogicalName}id' to='{attributeLogicalName}' alias='aa'>
                                    <filter type='and'>
                                      <condition attribute='{Record.LogicalName}id' operator='eq' value='{Record.Id}'/>
                                    </filter>
                                  </link-entity>
                                </link-entity>
                              </entity>
                            </fetch>";

            EntityCollection result = Service.RetrieveMultiple(new FetchExpression(query));

            foreach (var item in result.Entities)
            {

                int? value = null;
                if (item.Contains("group"))
                {
                    var groupby = ((AliasedValue)item["group"]).Value;
                    // Define the type
                    if (groupby.GetType() == typeof(OptionSetValue))
                        value = ((OptionSetValue)groupby).Value;

                    else if (groupby.GetType() == typeof(Boolean))
                        value = Convert.ToInt32((Boolean)groupby);
                }
                else
                    value = null;

                // Get the right option
                var option = optionsModel.GetOption(value);
                if (option != null)
                    option.Count = (Int32)((AliasedValue)item["count"]).Value;
            }

            return optionsModel;
        }
        #endregion

        public enum RelationshipType
        {
            OneToMany = 0,
            ManyToMany = 1
        }
    }
}