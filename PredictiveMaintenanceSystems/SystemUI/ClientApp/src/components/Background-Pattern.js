// @flow

import * as React from 'react';
import Circle from './Circle';

type IBackgroundPatternProps = {
    gridSpacing?: number,
    gridDotSize?: number,
};

//Creates the pattern design
class BackgroundPattern extends React.Component<IBackgroundPatternProps> {
    render() {
        const {gridSpacing, gridDotSize} = this.props;
        //Use the spacing defined from the props
        //Creates the dots
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