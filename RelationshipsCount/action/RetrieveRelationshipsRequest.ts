export class RetrieveRelationshipsRequest
{
	public LanguageCode : number;
	public RecordId : string;
	public RecordLogicalName : string;
	public OneToManyRelationships : string;
	public ManyToManyRelationships : string;

	/**
	 * name
	 */
	public getMetadata() {
		
	} 
}