import { RelationshipsModel } from "../model/RelationshipsModel";
import React = require("react");
import { RelationshipsCountData } from "../data/RelationshipsCountData";
import { RelationshipTSX } from "./RelationshipTSX";

export interface IRelationshipsTSXProps {
    languageCode: number,
    recordId: string,
    recordLogicalName: string,
    relationshipsModel: RelationshipsModel
}

export interface IRelationshipsTSXState extends React.ComponentState, IRelationshipsTSXProps {
}

export class RelationshipsTSX extends React.Component<IRelationshipsTSXProps, IRelationshipsTSXState> {

    // Lists will receive the relationships
    private oneToManyEntities: any[];
    private manyToManyEntities: any[];

    // Init
    constructor(props: IRelationshipsTSXProps) {
        super(props);

        this.oneToManyEntities = new Array();
        this.manyToManyEntities = new Array();

        this.state = {
            languageCode: this.props.languageCode,
            recordId: this.props.recordId,
            recordLogicalName: this.props.recordLogicalName,
            relationshipsModel: this.props.relationshipsModel,

            OneToManyEntities: this.oneToManyEntities,
            ManyToManyEntities: this.manyToManyEntities,

            Descending: false
        };
        this.onLoad();
    }

    // Update
    componentWillReceiveProps(props: IRelationshipsTSXProps) {
        this.oneToManyEntities = new Array();
        this.manyToManyEntities = new Array();
        this.setState({
            languageCode: props.languageCode,
            recordId: props.recordId,
            recordLogicalName: props.recordLogicalName,
            relationshipsModel: props.relationshipsModel,

            OneToManyEntities: this.oneToManyEntities,
            ManyToManyEntities: this.manyToManyEntities,

            Descending: false
        });
        this.onLoad();
    }

    // Render
    render(): JSX.Element {
        return <div key={this.state.relationshipsModel.RecordId} id="selector360_container">
            <div>
                <button className="refresh_button" onClick={this.refresh}>
                    {RelationshipsCountData.Resx().ButtonRefresh} [{this.state.relationshipsModel.LastUpdate}]
                </button>
                <button className="sortby_button" onClick={this.sortByDisplayName}>
                    A ↑↓
                </button>
                <button className="sortby_button" onClick={this.sortByCount}>
                    9 ↑↓
                </button>
            </div>
            <div>
                <div>
                    <label>1:N</label>
                    <div>
                        {this.oneToManyEntities}
                    </div>
                </div>
                <div>
                    <label>N:N</label>
                    <div>
                        {this.manyToManyEntities}
                    </div>
                </div>
            </div>
        </div>;
    }

    // Called by button, refresh all controls
    refresh = () => {
        RelationshipsCountData.Refresh();
    }

    // Called on Init or Update
    onLoad = () => {

        //1:N
        this.state.relationshipsModel.RelationshipsOneToMany.forEach(relationship_ => {
            this.oneToManyEntities.push(React.createElement(RelationshipTSX,
                {
                    languageCode: this.state.languageCode,
                    recordId: this.state.recordId,
                    recordLogicalName: this.state.recordLogicalName,
                    relationshipModel: relationship_
                }));
        });

        //N:N
        this.state.relationshipsModel.RelationshipsManyToMany.forEach(relationship_ => {
            this.manyToManyEntities.push(React.createElement(RelationshipTSX,
                {
                    languageCode: this.state.languageCode,
                    recordId: this.state.recordId,
                    recordLogicalName: this.state.recordLogicalName,
                    relationshipModel: relationship_
                }));
        });        
    }

    //OrderBy Display Name
    sortByDisplayName = () => {
        if (!this.state.Descending) {
            this.setState({
                Descending : true,
                OneToManyEntities: this.oneToManyEntities.sort(this.orderByDisplayName),
                ManyToManyEntities: this.manyToManyEntities.sort(this.orderByDisplayName),
            });
        }
        else
        {
            this.setState({
                Descending : false,
                OneToManyEntities: this.oneToManyEntities.sort(this.orderByDescDisplayName),
                ManyToManyEntities: this.manyToManyEntities.sort(this.orderByDescDisplayName),
            });
        }
    }

    //OrderBy Count Records
    sortByCount = () => {
        if (!this.state.Descending) {
            this.setState({
                Descending : true,
                OneToManyEntities: this.oneToManyEntities.sort(this.orderByCount),
                ManyToManyEntities: this.manyToManyEntities.sort(this.orderByCount),
            });
        }
        else
        {
            this.setState({
                Descending : false,
                OneToManyEntities: this.oneToManyEntities.sort(this.orderByDescCount),
                ManyToManyEntities: this.manyToManyEntities.sort(this.orderByDescCount),
            });
        }
    }

    //A > Z
    orderByDisplayName(a: RelationshipTSX, b: RelationshipTSX): number {
        if (a.props.relationshipModel.DN > b.props.relationshipModel.DN) {
            return 1;
        }
        if (a.props.relationshipModel.DN < b.props.relationshipModel.DN) {
            return -1;
        }
        return 0;
    }

    //Z > A
    orderByDescDisplayName(a: RelationshipTSX, b: RelationshipTSX): number {
        if (a.props.relationshipModel.DN < b.props.relationshipModel.DN) {
            return 1;
        }
        if (a.props.relationshipModel.DN > b.props.relationshipModel.DN) {
            return -1;
        }
        return 0;
    }

    //0 > 9
    orderByCount(a: RelationshipTSX, b: RelationshipTSX): number {
        if (a.props.relationshipModel.C > b.props.relationshipModel.C) {
            return 1;
        }
        if (a.props.relationshipModel.C < b.props.relationshipModel.C) {
            return -1;
        }
        return 0;
    }

    //9 > 0
    orderByDescCount(a: RelationshipTSX, b: RelationshipTSX): number {
        if (a.props.relationshipModel.C < b.props.relationshipModel.C) {
            return 1;
        }
        if (a.props.relationshipModel.C > b.props.relationshipModel.C) {
            return -1;
        }
        return 0;
    }
}