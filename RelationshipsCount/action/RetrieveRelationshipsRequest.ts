export class RetrieveRelationshipsRequest
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
					"RecordId": {
						"typeName": "Edm.String",
						"structuralProperty": 1
					},
					"RecordLogicalName": {
						"typeName": "Edm.String",
						"structuralProperty": 1
					},
					"OneToManyRelationships": {
						"typeName": "Edm.String",
						"structuralProperty": 1
					},
					"ManyToManyRelationships": {
						"typeName": "Edm.String",
						"structuralProperty": 1
					}
				},
				operationType: 0,
				operationName: "vnb_Schema360RetrieveRelationships"
			};
		};
	}

	public LanguageCode : number;
	public RecordId : string;
	public RecordLogicalName : string;
	public OneToManyRelationships : string;
	public ManyToManyRelationships : string;

	public getMetadata() {
		
	} 
}