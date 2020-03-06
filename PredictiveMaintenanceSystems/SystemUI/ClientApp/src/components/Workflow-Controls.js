// @flow

import React from 'react';
import Parse from 'html-react-parser';
import faExpand from '@fortawesome/fontawesome-free/svgs/solid/expand.svg';

const steps = 100;
const parsedIcon = Parse(faExpand);
const ExpandIcon = () => parsedIcon;

type IGraphControlProps = {
    maxZoom?: number,
    minZoom?: number,
    zoomLevel: number,
    zoomToFit: (event: SyntheticMouseEvent<HTMLButtonElement>) => void,
    modifyZoom: (delta: number) => boolean,
};

class WorkflowControls extends React.Component<IGraphControlProps> {
    static defaultProps = {
        maxZoom: 1.5,
        minZoom: 0.15,
    };

    sliderToZoom(val: number) {
        const {minZoom, maxZoom} = this.props;

        return (val * ((maxZoom || 0) - (minZoom || 0))) / steps + (minZoom || 0);
    }

    zoomToSlider(val: number) {
        const {minZoom, maxZoom} = this.props;

        return ((val - (minZoom || 0)) * steps) / ((maxZoom || 0) - (minZoom || 0));
    }

    zoom = (e: any) => {
        const {minZoom, maxZoom} = this.props;
        const sliderVal = e.target.value;
        const zoomLevelNext = this.sliderToZoom(sliderVal);
        const delta = zoomLevelNext - this.props.zoomLevel;

        if (zoomLevelNext <= (maxZoom || 0) && zoomLevelNext >= (minZoom || 0)) {
            this.props.modifyZoom(delta);
        }
    };

    render() {
        return (
            <div className="graph-controls">
                <div className="slider-wrapper">
                    <span>-</span>
                    <input
                        type="range"
                        className="slider"
                        min={this.zoomToSlider(this.props.minZoom || 0)}
                        max={this.zoomToSlider(this.props.maxZoom || 0)}
                        value={this.zoomToSlider(this.props.zoomLevel)}
                        onChange={this.zoom}
                        step="1"
                    />
                    <span>+</span>
                </div>
                <button
                    type="button"
                    className="slider-button"
                    onMouseDown={this.props.zoomToFit}
                >
                    <ExpandIcon/>
                </button>
            </div>
        );
    }
}

export default WorkflowControls;
