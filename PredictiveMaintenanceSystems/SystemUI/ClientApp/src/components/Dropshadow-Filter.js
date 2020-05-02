// @flow

import * as React from 'react';

//Create the shadow for the workflow graph
//Visual effects only
class DropshadowFilter extends React.Component<any> {
    render() {
        return (
            <filter id="dropshadow" key="dropshadow" height="130%">
                <feGaussianBlur in="SourceAlpha" stdDeviation="3"/>
                <feOffset dx="2" dy="2" result="offsetblur"/>
                <feComponentTransfer>
                    <feFuncA type="linear" slope="0.1"/>
                </feComponentTransfer>
                <feMerge>
                    <feMergeNode/>
                    <feMergeNode in="SourceGraphic"/>
                </feMerge>
            </filter>
        );
    }
}

export default DropshadowFilter;