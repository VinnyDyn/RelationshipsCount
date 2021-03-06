export class RetrieveEntityDetailsRequest
{
	constructor()
	{
		this.getMetadata = function () {
            return {
                boundParameter: null,
                parameterTypes: {
                    "LanguageCode": {
                        "typeName": "Edm.Int32",
                        "structuralProperty": 1
                    },
                    "RecordId": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    },
                    "RecordLogicalName": {
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
                    "IntersectEntityName": {
                        "typeName": "Edm.String",
                        "structuralProperty": 1
                    }
                },
                operationType: 0,
                operationName: "vnb_Schema360RetrieveRelationshipAdditionalDetails"
            };
        }; 
	}

	public LanguageCode : number;
	public RecordId : string;
	public RecordLogicalName : string;
	public EntityLogicalName : string;
	public AttributeLogicalName : string;
	public IntersectEntityName : string | null;
	
	getMetadata(): void {
		
	}
}