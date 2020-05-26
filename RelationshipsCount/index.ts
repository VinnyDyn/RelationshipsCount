import {IInputs, IOutputs} from "./generated/ManifestTypes";
import { Resx } from "./data/Resx";
import { RelationshipsCountData } from "./data/RelationshipsCountData";
import ReactDOM = require("react-dom");
import React = require("react");
import { RelationshipsModel } from "./model/RelationshipsModel";
import { RelationshipsTSX } from "./tsx/RelationshipsTSX";
import { RetrieveRelationshipsRequest } from "./action/RetrieveRelationshipsRequest";
import { RetrieveRelationshipsResponse } from "./action/RetrieveRelationshipsResponse";

export class RelationshipsCount implements ComponentFramework.StandardControl<IInputs, IOutputs> {

	private _entityLogicalName: string;
	private _entityId: string;
	private _languageCode: number;
	private _container: HTMLDivElement;
	private _oneToManyRelationships: string;
	private _manyToManyRelationships: string;

	/**
	 * Empty constructor.
	 */
	constructor()
	{

	}

	/**
	 * Used to initialize the control instance. Controls can kick off remote server calls and other initialization actions here.
	 * Data-set values are not initialized here, use updateView.
	 * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to property names defined in the manifest, as well as utility functions.
	 * @param notifyOutputChanged A callback method to alert the framework that the control has new outputs ready to be retrieved asynchronously.
	 * @param state A piece of data that persists in one session for a single user. Can be set at any point in a controls life cycle by calling 'setControlState' in the Mode interface.
	 * @param container If a control is marked control-type='standard', it will receive an empty div element within which it can render its content.
	 */
	public init(context: ComponentFramework.Context<IInputs>, notifyOutputChanged: () => void, state: ComponentFramework.Dictionary, container:HTMLDivElement)
	{
		//Attributes
		this._languageCode = context.userSettings.languageId;
		this._entityId = Xrm.Page.data.entity.getId();
		this._entityLogicalName = Xrm.Page.data.entity.getEntityName();
		this._oneToManyRelationships = context.parameters.OneToMany.raw!;
		this._manyToManyRelationships = context.parameters.ManyToMany.raw!;

		//Resx
		let resx: Resx = new Resx(context);

		//Context
		RelationshipsCountData.AddContext(this, resx);

		//HTML Main
		this._container = document.createElement('div');
		container.append(this._container);

		//Render Rules
		this.RetrieveRelationships();
	}


	/**
	 * Called when any value in the property bag has changed. This includes field values, data-sets, global values such as container height and width, offline status, control metadata values such as label, visible, etc.
	 * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to names defined in the manifest, as well as utility functions
	 */
	public updateView(context: ComponentFramework.Context<IInputs>): void
	{
		// Add code to update control view
	}

	/** 
	 * It is called by the framework prior to a control receiving new data. 
	 * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as “bound” or “output”
	 */
	public getOutputs(): IOutputs
	{
		return {};
	}

	/** 
	 * Called when the control is to be removed from the DOM tree. Controls should use this call for cleanup.
	 * i.e. cancelling any pending remote calls, removing listeners, etc.
	 */
	public destroy(): void
	{
		// Add code to cleanup control if necessary
	}

	public Refresh() {
		this.RetrieveRelationships();
	}

		/**
	 * Retrieve entity relationships 1:N, N:N and quantity records
	 */
	private RetrieveRelationships(): void {
		let request = new RetrieveRelationshipsRequest();
		request.LanguageCode = this._languageCode;
		request.RecordId = this._entityId;
		request.RecordLogicalName = this._entityLogicalName;
		request.OneToManyRelationships = this._oneToManyRelationships;
		request.ManyToManyRelationships = this._manyToManyRelationships;

		request.getMetadata = function() {
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

		const self = this;
		Xrm.WebApi.online.execute(request).then(
			function (result) {
				if (result.ok) {
					self.FetchStream(self, result.body);
				}
			},
			function (error) {
				Xrm.Utility.alertDialog(error.message, function () { });
			}
		);
	}
	private FetchStream(caller: RelationshipsCount, stream: ReadableStream | null): void {
		const reader = stream!.getReader();
		let text: string;
		text = "";
		reader.read().then(function processText({ done, value }): void {

			if (done) {
				let content: RetrieveRelationshipsResponse = JSON.parse(text);
				let relationshipsModel: RelationshipsModel = JSON.parse(content.Relationships);

				if (relationshipsModel !== undefined) {
					caller.RenderEntitySchema(relationshipsModel);
				}
				return;
			}

			if (value)
				text += new TextDecoder("utf-8").decode(value);
			reader.read().then(processText);
		});
	}

	/**
	 * Render records
	 * @param _relationshipsModel
	 */
	private RenderEntitySchema(_relationshipsModel: RelationshipsModel) {
		ReactDOM.render(
			React.createElement(RelationshipsTSX,
				{
					languageCode: this._languageCode,
					recordId: this._entityId,
					recordLogicalName: this._entityLogicalName,
					relationshipsModel: _relationshipsModel
				}),
			this._container);
	}
}