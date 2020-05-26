export class RetrieveEntityDetailsRequest
{
	public LanguageCode : number;
	public RecordId : string;
	public RecordLogicalName : string;
	public EntityLogicalName : string;
	public AttributeLogicalName : string;
	public IntersectEntityName : string | null;

	/**
	 * name
	 */
	public getMetadata() {
		
	} 
}