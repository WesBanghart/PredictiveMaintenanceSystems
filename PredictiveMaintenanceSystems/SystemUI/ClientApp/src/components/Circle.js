// @flow

import * as React from 'react';

type ICircleProps = {
    gridSpacing?: number,
    gridDotSize?: number,
};

//Creates the circle for the nodes of the graph
class Circle extends React.Component<ICircleProps> {
    static defaultProps = {
        gridDotSize: 2,
        gridSpacing: 36,
    };

    //Render the circle based on the graph design behind it
    render() {
        const {gridSpacing, gridDotSize} = this.props;

        return (
            <circle
                className="circle"
                cx={(gridSpacing || 0) / 2}
                cy={(gridSpacing || 0) / 2}
                r={gridDotSize}
            />
        );
    }
}

export default Circle;