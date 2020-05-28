export class GroupByAttributeRequestRequest
{
    constructor()
    {
        this.getMetadata = function() {
            return {
                boundParameter: null,
                parameterTypes: {
                    "LanguageCode": {
                        "typeName": "Edm.Int32",
                        "structuralProperty": 1
                    },
                    "RecordLogicalName": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "RecordId": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "EntityLogicalName": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "AttributeLogicalName": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "GroupByAttributeLogicalName": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "GroupByAttributeType": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "IntersectEntityName": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    }
                },
                operationType: 0,
                operationName: "vnb_Schema360GroupByAttribute"
            };
        };
    }

    public LanguageCode : number;
    public RecordLogicalName : string;
	public RecordId : string;
	public EntityLogicalName : string;
    public AttributeLogicalName : string;
    public GroupByAttributeLogicalName : string;
    public IntersectEntityName : string;
    public GroupByAttributeType : string;
    
    getMetadata(): void {
    }
}