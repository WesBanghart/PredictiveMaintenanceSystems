// @flow

import * as React from 'react';
import Arrowhead from './Arrowhead';
import BackgroundPattern from './Background-Pattern';
import DropshadowFilter from './Dropshadow-Filter';

type IDefsProps = {
    gridSpacing?: number,
    gridDotSize?: number,
    edgeArrowSize?: number,
    nodeTypes: any, // TODO: define nodeTypes, nodeSubtypes, and edgeTypes. Must have shape and shapeId!
    nodeSubtypes: any,
    edgeTypes: any,
    renderDefs?: () => any | null,
};

type IDefsState = {
    graphConfigDefs: any,
};

//Holds definitions for the workflow
class Definitions extends React.Component<IDefsProps, IDefsState> {
    static defaultProps = {
        gridDotSize: 2,
        renderDefs: () => null,
    };

    constructor(props: IDefsProps) {
        super(props);
        this.state = {
            graphConfigDefs: [],
        };
    }

    //Create static properties for the workflow
    static getDerivedStateFromProps(nextProps: any, prevState: any) {
        const graphConfigDefs = [];

        Definitions.processGraphConfigDefs(nextProps.nodeTypes, graphConfigDefs);
        Definitions.processGraphConfigDefs(nextProps.nodeSubtypes, graphConfigDefs);
        Definitions.processGraphConfigDefs(nextProps.edgeTypes, graphConfigDefs);

        return {
            graphConfigDefs,
        };
    }

    static processGraphConfigDefs(typesObj: any, graphConfigDefs: any) {
        Object.keys(typesObj).forEach(type => {
            const safeId = typesObj[type].shapeId
                ? typesObj[type].shapeId.replace('#', '')
                : 'graphdef';

            graphConfigDefs.push(
                React.cloneElement(typesObj[type].shape, {
                    key: `${safeId}-${graphConfigDefs.length + 1}`,
                })
            );
        });
    }

    //Render based on defined sizes
    render() {
        const {edgeArrowSize, gridSpacing, gridDotSize} = this.props;

        return (
            <defs>
                {this.state.graphConfigDefs}

                <Arrowhead edgeArrowSize={edgeArrowSize}/>

                <BackgroundPattern
                    gridSpacing={gridSpacing}
                    gridDotSize={gridDotSize}
                />

                <DropshadowFilter/>

                {this.props.renderDefs && this.props.renderDefs()}
            </defs>
        );
    }
}

export default Definitions;