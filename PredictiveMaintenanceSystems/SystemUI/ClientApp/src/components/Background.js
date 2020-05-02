// @flow

import * as React from 'react';

type IBackgroundProps = {
    gridSize?: number,
    backgroundFillId?: string,
    renderBackground?: (gridSize?: number) => any,
};

//Render the grid background for the graph
class Background extends React.Component<IBackgroundProps> {
    static defaultProps = {
        backgroundFillId: '#grid',
        gridSize: 40960,
    };

    //Returns the rendering of the graph based on the size of the window
    render() {
        const {gridSize, backgroundFillId, renderBackground} = this.props;

        if (renderBackground != null) {
            return renderBackground(gridSize);
        }

        return (
            <rect
                className="background"
                x={-(gridSize || 0) / 4}
                y={-(gridSize || 0) / 4}
                width={gridSize}
                height={gridSize}
                fill={`url(${backgroundFillId || ''})`}
            />
        );
    }
}

export default Background;