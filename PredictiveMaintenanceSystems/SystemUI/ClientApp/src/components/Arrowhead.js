// @flow

import * as React from 'react';

type IArrowheadMarkerProps = {
    edgeArrowSize?: number,
};
//Handles rendering the arrow head component for the graph
class Arrowhead extends React.Component<IArrowheadMarkerProps> {
    static defaultProps = {
        edgeArrowSize: 8,
    };

    render() {
        const {edgeArrowSize} = this.props;
        //Checks the side of the arrow
        if (!edgeArrowSize && edgeArrowSize !== 0) {
            return null;
        }

        //Render the arrow with the direction and size
        return (
            <marker
                id="end-arrow"
                key="end-arrow"
                viewBox={`0 -${edgeArrowSize / 2} ${edgeArrowSize} ${edgeArrowSize}`}
                refX={`${edgeArrowSize / 2}`}
                markerWidth={`${edgeArrowSize}`}
                markerHeight={`${edgeArrowSize}`}
                orient="auto"
            >
                <path
                    className="arrow"
                    d={`M0,-${edgeArrowSize / 2}L${edgeArrowSize},0L0,${edgeArrowSize /
                    2}`}
                />
            </marker>
        );
    }
}

export default Arrowhead;