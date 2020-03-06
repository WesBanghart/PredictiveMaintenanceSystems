// @flow

import * as React from 'react';
import Circle from './Circle';

type IBackgroundPatternProps = {
    gridSpacing?: number,
    gridDotSize?: number,
};

class BackgroundPattern extends React.Component<IBackgroundPatternProps> {
    render() {
        const {gridSpacing, gridDotSize} = this.props;

        return (
            <pattern
                id="grid"
                key="grid"
                width={gridSpacing}
                height={gridSpacing}
                patternUnits="userSpaceOnUse"
            >
                <Circle gridSpacing={gridSpacing} gridDotSize={gridDotSize}/>
            </pattern>
        );
    }
}

export default BackgroundPattern;