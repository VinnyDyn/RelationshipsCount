import { RetrieveEntityDetailsResponse } from "../action/RetrieveEntityDetailsResponse";
import { RelationshipDetailsModel } from "../model/RelationshipDetailsModel";
import { RelationshipModel } from "../model/RelationshipModel";
import React = require("react");
import { RetrieveEntityDetailsRequest } from "../action/RetrieveEntityDetailsRequest";

export interface IRelationshipTSXProps {
    languageCode: number,
    recordId: string,
    recordLogicalName: string,
    relationshipModel: RelationshipModel
}

export interface IRelationshipTSXState extends React.ComponentState, IRelationshipTSXProps {
}

export class RelationshipTSX extends React.Component<IRelationshipTSXProps, IRelationshipTSXState> {

    constructor(props: IRelationshipTSXProps) {
        super(props);
        this.state = {
            languageCode: this.props.languageCode,
            recordId: this.props.recordId,
            recordLogicalName: this.props.recordLogicalName,
            relationshipModel: this.props.relationshipModel
        };
        this.onLoad();
    }

    componentWillReceiveProps(props: IRelationshipTSXProps) {
        this.state = {
            languageCode: props.languageCode,
            recordId: props.recordId,
            recordLogicalName: props.recordLogicalName,
            relationshipModel: props.relationshipModel
        };
        this.onLoad();
    }

    render(): JSX.Element {
        return <div key={this.state.relationshipModel.SN}>
            <label title={this.state.relationshipModel.SN}>
                {this.state.relationshipModel.C} - {this.state.relationshipModel.DN}
            </label>
        </div>;
    }

    //Session
    onLoad = () => {
        this.RetrieveRelationships();
    }

    private RetrieveRelationships(): void {
        let request = new RetrieveEntityDetailsRequest();
        request.LanguageCode = this.state.languageCode;
        request.RecordId = this.state.recordId;
        request.RecordLogicalName = this.state.recordLogicalName;
        request.EntityLogicalName = this.state.relationshipModel.RE;
        request.AttributeLogicalName = this.state.relationshipModel.RA;
        request.IntersectEntityName = this.state.relationshipModel.RT == 1 ? this.state.relationshipModel.SN : "";

        request.getMetadata = function () {
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

    private FetchStream(caller: RelationshipTSX, stream: ReadableStream | null): void {
        const reader = stream!.getReader();
        let text: string;
        text = "";
        reader.read().then(function processText({ done, value }): void {

            if (done) {
                let content: RetrieveEntityDetailsResponse = JSON.parse(text);
                let relationshipDetailsModel: RelationshipDetailsModel = JSON.parse(content.RelationshipAdditionalDetails);

                if (relationshipDetailsModel !== undefined) {
                    let update = caller.state.relationshipModel;
                    update.DN = relationshipDetailsModel.DN;
                    update.C = relationshipDetailsModel.C;
                    caller.setState({
                        relationshipModel: update
                    });
                }
                return;
            }

            if (value)
                text += new TextDecoder("utf-8").decode(value);
            reader.read().then(processText);
        });
    }
}