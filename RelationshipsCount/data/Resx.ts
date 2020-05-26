import { IInputs } from "../generated/ManifestTypes";

export class Resx
{
    public ButtonRefresh : string;
    
    constructor(context : ComponentFramework.Context<IInputs>)
    {
        this.ButtonRefresh = context.resources.getString("RelationshipsTSX_ButtonRefresh");
    }
}