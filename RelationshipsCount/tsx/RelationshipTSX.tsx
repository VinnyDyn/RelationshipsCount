import { RetrieveEntityDetailsResponse } from "../action/RetrieveEntityDetailsResponse";
import { RelationshipDetailsModel } from "../model/RelationshipDetailsModel";
import { RelationshipModel } from "../model/RelationshipModel";
import React = require("react");
import { RetrieveEntityDetailsRequest } from "../action/RetrieveEntityDetailsRequest";
import { AttributeModel } from "../model/AttributeModel";
import { GroupByAttributeRequestRequest } from "../action/GroupByAttributeRequest";
import { GroupByAttributeResponse } from "../action/GroupByAttributeResponse";
import { OptionsModel } from "../model/OptionsModel";
import Chart from "react-google-charts";

export interface IRelationshipTSXProps {
    languageCode: number,
    recordId: string,
    recordLogicalName: string,
    relationshipModel: RelationshipModel
}

export interface IRelationshipTSXState extends React.ComponentState, IRelationshipTSXProps {
}

export class RelationshipTSX extends React.Component<IRelationshipTSXProps, IRelationshipTSXState> {

    //Attributes will be rendered in select (dropdown)
    private attributes: any[];
    //Char area
    private chart: any;

    // Init
    constructor(props: IRelationshipTSXProps) {
        super(props);

        this.attributes = new Array();
        this.chart = null;

        this.state = {
            languageCode: this.props.languageCode,
            recordId: this.props.recordId,
            recordLogicalName: this.props.recordLogicalName,
            relationshipModel: this.props.relationshipModel,

            hidden: true,
            Attributes: this.attributes,
            Chart: this.chart
        };
        this.onLoad();
    }

    // Update
    componentWillReceiveProps(props: IRelationshipTSXProps) {

        this.attributes = new Array();
        this.chart = null;

        this.state = {
            languageCode: props.languageCode,
            recordId: props.recordId,
            recordLogicalName: props.recordLogicalName,
            relationshipModel: props.relationshipModel,

            selectDisabled: true,
            Attributes: this.attributes,
            Chart: this.chart
        };
        this.onLoad();
    }

    // Render
    render(): JSX.Element {
        return <div key={this.state.relationshipModel.SN}>
            <div title={this.state.relationshipModel.SN}>
                {this.state.relationshipModel.C} - {this.state.relationshipModel.DN}
            </div>
            <div>
                <select className={"drilldown_select"} hidden={this.state.hidden} onChange={this.onChange}>
                    {this.attributes}
                </select>
            </div>
            <div>
                {this.chart}
            </div>
        </div>;
    }

    // Called by Init and Update
    onLoad = () => {
        this.RetrieveRelationships();
    }

    // Trigged when the select attribute is change
    onChange = (e: any) => {
        var index = e.target.selectedIndex;
        var optionElement = e.target.childNodes[index];
        var type = optionElement.id;
        let value = e.target.value;
        this.ExecuteGroupBy(value, type);
    }

    // 
    // 
    /**
     * Get the relationships details (Display Name and Record's Count)
     */
    private RetrieveRelationships(): void {
        let request = new RetrieveEntityDetailsRequest();
        request.LanguageCode = this.state.languageCode;
        request.RecordId = this.state.recordId;
        request.RecordLogicalName = this.state.recordLogicalName;
        request.EntityLogicalName = this.state.relationshipModel.RE;
        request.AttributeLogicalName = this.state.relationshipModel.RA;
        request.IntersectEntityName = this.state.relationshipModel.RT == 1 ? this.state.relationshipModel.SN : "";

        const self = this;
        Xrm.WebApi.online.execute(request).then(
            function (result) {
                if (result.ok) {
                    self.FetchStreamRetrieveRelationships(self, result.body);
                }
            },
            function (error) {
                Xrm.Utility.alertDialog(error.message, function () { });
            }
        );
    }
    private FetchStreamRetrieveRelationships(caller: RelationshipTSX, stream: ReadableStream | null): void {
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
                    caller.RenderAttributes(relationshipDetailsModel.Attributes);
                    caller.setState({
                        relationshipModel: update,
                        Attributes: caller.attributes
                    });
                }
                return;
            }

            if (value)
                text += new TextDecoder("utf-8").decode(value);
            reader.read().then(processText);
        });
    }

    /**
     * Execute a group by query based on selected attribute
     * @param groupByAttributeLogicalName Selected attribute option
     * @param groupByAttributeType Selected attribute type (boolean, picklist, statusreason, state)
     */
    private ExecuteGroupBy(groupByAttributeLogicalName: string, groupByAttributeType: string): void {
        let request = new GroupByAttributeRequestRequest();
        request.LanguageCode = this.state.languageCode;
        request.RecordLogicalName = this.state.recordLogicalName;
        request.RecordId = this.state.recordId;
        request.EntityLogicalName = this.state.relationshipModel.RE;
        request.AttributeLogicalName = this.state.relationshipModel.RA;
        request.GroupByAttributeLogicalName = groupByAttributeLogicalName;
        request.GroupByAttributeType = groupByAttributeType;
        request.IntersectEntityName = this.state.relationshipModel.RT == 1 ? this.state.relationshipModel.SN : "";

        const self = this;
        Xrm.WebApi.online.execute(request).then(
            function (result) {
                if (result.ok) {
                    self.FetchStreamExecuteGroupBy(self, result.body);
                }
            },
            function (error) {
                Xrm.Utility.alertDialog(error.message, function () { });
            }
        );
    }
    private FetchStreamExecuteGroupBy(caller: RelationshipTSX, stream: ReadableStream | null): void {
        const reader = stream!.getReader();
        let text: string;
        text = "";
        reader.read().then(function processText({ done, value }): void {

            if (done) {
                let content: GroupByAttributeResponse = JSON.parse(text);
                let options: OptionsModel = JSON.parse(content.GroupByResult);
                if (options !== undefined) {
                    caller.RenderChart(options);
                }
                return;
            }

            if (value)
                text += new TextDecoder("utf-8").decode(value);
            reader.read().then(processText);
        });
    }

    private RenderAttributes(attributes: Array<AttributeModel>): void {
        attributes.forEach(attribute_ => {
            this.attributes.push(<option key={attribute_.LN} id={attribute_.T} value={attribute_.LN}>{attribute_.DN}</option>);
        });

        this.setState({
            hidden: attributes.length > 0 ? false : true
        })
    }

    private RenderChart(options: OptionsModel): void {
        //Chart Configs
        var chartConfig = {
            legend: 'none'
        };

        //Chart Data
        let data: any[];
        data = new Array<any>();
        data.push(["GroupBy", "", { role: "style" }]);
        options.Options.forEach(option_ => {
            let hex = option_.HEX != null ? option_.HEX : "#808080";
            data.push([option_.DN, option_.C, hex]);
        });

        //Request Render
        this.chart = <Chart key={this.state.relationshipModel.RE + this.state.relationshipModel.RA} chartType="BarChart" width="100%" height="300px" data={data} options={chartConfig} />
        this.setState(
            {
                Chart: this.chart
            }
        )
    }
}