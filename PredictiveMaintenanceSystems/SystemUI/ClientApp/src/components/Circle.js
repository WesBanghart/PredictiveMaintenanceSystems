// @flow

import * as React from 'react';

type ICircleProps = {
    gridSpacing?: number,
    gridDotSize?: number,
};

class Circle extends React.Component<ICircleProps> {
    static defaultProps = {
        gridDotSize: 2,
        gridSpacing: 36,
    };

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