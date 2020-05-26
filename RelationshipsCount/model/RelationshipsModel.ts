import { RelationshipModel } from "./RelationshipModel";

export class RelationshipsModel
{
    public RecordId : string;
    public LastUpdate : string;
    public RelationshipsOneToMany : Array<RelationshipModel>;
    public RelationshipsManyToMany : Array<RelationshipModel>;
}