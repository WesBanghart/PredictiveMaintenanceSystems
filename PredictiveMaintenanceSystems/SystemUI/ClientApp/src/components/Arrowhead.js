// @flow

import * as React from 'react';

type IArrowheadMarkerProps = {
    edgeArrowSize?: number,
};

class Arrowhead extends React.Component<IArrowheadMarkerProps> {
    static defaultProps = {
        edgeArrowSize: 8,
    };

    render() {
        const {edgeArrowSize} = this.props;

        if (!edgeArrowSize && edgeArrowSize !== 0) {
            return null;
        }

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