// @flow

import * as React from 'react';

type IBackgroundProps = {
    gridSize?: number,
    backgroundFillId?: string,
    renderBackground?: (gridSize?: number) => any,
};

class Background extends React.Component<IBackgroundProps> {
    static defaultProps = {
        backgroundFillId: '#grid',
        gridSize: 40960,
    };

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