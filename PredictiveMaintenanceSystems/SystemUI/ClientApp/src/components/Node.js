// @flow

import * as d3 from 'd3';
import * as React from 'react';
import Edge from './Edge';
import GraphUtils from '../utilities/Workflow-Utils';
import NodeText from './Node-Text';

export type IPoint = {
    x: number,
    y: number,
};

export type INode = {
    title: string,
    x?: number | null,
    y?: number | null,
    type?: string | null,
    subtype?: string | null,
    [key: string]: any,
};

type INodeProps = {
    data: INode,
    id: string,
    nodeTypes: any, // TODO: make a nodeTypes interface
    nodeSubtypes: any, // TODO: make a nodeSubtypes interface
    opacity?: number,
    nodeKey: string,
    nodeSize?: number,
    onNodeMouseEnter: (event: any, data: any, hovered: boolean) => void,
    onNodeMouseLeave: (event: any, data: any) => void,
    onNodeMove: (point: IPoint, id: string, shiftKey: boolean) => void,
    onNodeSelected: (
        data: any,
        id: string,
        shiftKey: boolean,
        event?: any
    ) => void,
    onNodeUpdate: (point: IPoint, id: string, shiftKey: boolean) => void,
    renderNode?: (
        nodeRef: any,
        data: any,
        id: string,
        selected: boolean,
        hovered: boolean
    ) => any,
    renderNodeText?: (data: any, id: string | number, isSelected: boolean) => any,
    isSelected: boolean,
    layoutEngine?: any,
    viewWrapperElem: HTMLDivElement,
    centerNodeOnMove: boolean,
    maxTitleChars: number,
};

type INodeState = {
    hovered: boolean,
    x: number,
    y: number,
    selected: boolean,
    mouseDown: boolean,
    drawingEdge: boolean,
    pointerOffset: ?{ x: number, y: number },
};

class Node extends React.Component<INodeProps, INodeState> {
    static defaultProps = {
        isSelected: false,
        nodeSize: 154,
        maxTitleChars: 12,
        onNodeMouseEnter: () => {

        },
        onNodeMouseLeave: () => {

        },
        onNodeMove: () => {

        },
        onNodeSelected: () => {

        },
        onNodeUpdate: () => {

        },
        centerNodeOnMove: true,
    };
    nodeRef: any;
    oldSibling: any;

    constructor(props: INodeProps) {
        super(props);

        this.state = {
            drawingEdge: false,
            hovered: false,
            mouseDown: false,
            selected: false,
            x: props.data.x || 0,
            y: props.data.y || 0,
            pointerOffset: null,
        };

        this.nodeRef = React.createRef();
    }

    static getDerivedStateFromProps(
        nextProps: INodeProps,
        prevState: INodeState
    ) {
        return {
            selected: nextProps.isSelected,
            x: nextProps.data.x,
            y: nextProps.data.y,
        };
    }

    static getNodeTypeXlinkHref(data: INode, nodeTypes: any) {
        if (data.type && nodeTypes[data.type]) {
            return nodeTypes[data.type].shapeId;
        } else if (nodeTypes.emptyNode) {
            return nodeTypes.emptyNode.shapeId;
        }

        return null;
    }

    static getNodeSubtypeXlinkHref(data: INode, nodeSubtypes?: any) {
        if (data.subtype && nodeSubtypes && nodeSubtypes[data.subtype]) {
            return nodeSubtypes[data.subtype].shapeId;
        } else if (nodeSubtypes && nodeSubtypes.emptyNode) {
            return nodeSubtypes.emptyNode.shapeId;
        }

        return null;
    }

    componentDidMount() {
        const dragFunction = d3
            .drag()
            .on('drag', () => {
                this.handleMouseMove(d3.event);
            })
            .on('start', this.handleDragStart)
            .on('end', () => {
                this.handleDragEnd(d3.event);
            });

        d3.select(this.nodeRef.current)
            .on('mouseout', this.handleMouseOut)
            .call(dragFunction);
    }

    handleMouseMove = (event: any) => {
        const mouseButtonDown = event.sourceEvent.buttons === 1;
        const shiftKey = event.sourceEvent.shiftKey;
        const {
            nodeSize,
            layoutEngine,
            nodeKey,
            viewWrapperElem,
            data,
        } = this.props;
        const {pointerOffset} = this.state;

        if (!mouseButtonDown) {
            return;
        }

        const newState = {
            x: event.x,
            y: event.y,
            pointerOffset,
        };

        if (!this.props.centerNodeOnMove) {
            newState.pointerOffset = pointerOffset || {
                x: event.x - (data.x || 0),
                y: event.y - (data.y || 0),
            };
            newState.x -= newState.pointerOffset.x;
            newState.y -= newState.pointerOffset.y;
        }

        if (shiftKey) {
            this.setState({drawingEdge: true});
            const off = Edge.calculateOffset(
                nodeSize,
                this.props.data,
                newState,
                nodeKey,
                true,
                viewWrapperElem
            );

            newState.x += off.xOff;
            newState.y += off.yOff;
        } else if (!this.state.drawingEdge && layoutEngine) {
            Object.assign(newState, layoutEngine.getPositionForNode(newState));
        }

        this.setState(newState);
        this.props.onNodeMove(newState, this.props.data[nodeKey], shiftKey);
    };

    handleDragStart = () => {
        if (!this.nodeRef.current) {
            return;
        }

        if (!this.oldSibling) {
            this.oldSibling = this.nodeRef.current.parentElement.nextSibling;
        }

        this.nodeRef.current.parentElement.parentElement.appendChild(
            this.nodeRef.current.parentElement
        );
    };

    handleDragEnd = (event: any) => {
        if (!this.nodeRef.current) {
            return;
        }

        const {x, y, drawingEdge} = this.state;
        const {data, nodeKey, onNodeSelected, onNodeUpdate} = this.props;
        const {sourceEvent} = event;

        this.setState({
            mouseDown: false,
            drawingEdge: false,
            pointerOffset: null,
        });

        if (this.oldSibling && this.oldSibling.parentElement) {
            this.oldSibling.parentElement.insertBefore(
                this.nodeRef.current.parentElement,
                this.oldSibling
            );
        }

        const shiftKey = sourceEvent.shiftKey;

        onNodeUpdate({x, y}, data[nodeKey], shiftKey || drawingEdge);

        onNodeSelected(data, data[nodeKey], shiftKey || drawingEdge, sourceEvent);
    };

    handleMouseOver = (event: any) => {
        let hovered = false;

        if (event && event.buttons !== 1) {
            hovered = true;
            this.setState({hovered});
        }

        this.props.onNodeMouseEnter(event, this.props.data, hovered);
    };

    handleMouseOut = (event: any) => {

        this.setState({hovered: false});
        this.props.onNodeMouseLeave(event, this.props.data);
    };

    renderShape() {
        const {renderNode, data, nodeTypes, nodeSubtypes, nodeKey} = this.props;
        const {hovered, selected} = this.state;
        const props = {
            height: this.props.nodeSize || 0,
            width: this.props.nodeSize || 0,
        };
        const nodeShapeContainerClassName = GraphUtils.classNames('shape');
        const nodeClassName = GraphUtils.classNames('node', {selected, hovered});
        const nodeSubtypeClassName = GraphUtils.classNames('subtype-shape', {
            selected: this.state.selected,
        });
        const nodeTypeXlinkHref = Node.getNodeTypeXlinkHref(data, nodeTypes) || '';
        const nodeSubtypeXlinkHref =
            Node.getNodeSubtypeXlinkHref(data, nodeSubtypes) || '';

        const defSvgNodeElement: any = nodeTypeXlinkHref
            ? document.querySelector(`defs>${nodeTypeXlinkHref}`)
            : null;
        const nodeWidthAttr = defSvgNodeElement
            ? defSvgNodeElement.getAttribute('width')
            : 0;
        const nodeHeightAttr = defSvgNodeElement
            ? defSvgNodeElement.getAttribute('height')
            : 0;

        props.width = nodeWidthAttr ? parseInt(nodeWidthAttr, 10) : props.width;
        props.height = nodeHeightAttr ? parseInt(nodeHeightAttr, 10) : props.height;

        if (renderNode) {
            return renderNode(this.nodeRef, data, data[nodeKey], selected, hovered);
        } else {
            return (
                <g className={nodeShapeContainerClassName} {...props}>
                    {!!data.subtype && (
                        <use
                            className={nodeSubtypeClassName}
                            x={-props.width / 2}
                            y={-props.height / 2}
                            width={props.width}
                            height={props.height}
                            xlinkHref={nodeSubtypeXlinkHref}
                        />
                    )}
                    <use
                        className={nodeClassName}
                        x={-props.width / 2}
                        y={-props.height / 2}
                        width={props.width}
                        height={props.height}
                        xlinkHref={nodeTypeXlinkHref}
                    />
                </g>
            );
        }
    }

    renderText() {
        const {
            data,
            id,
            nodeTypes,
            renderNodeText,
            isSelected,
            maxTitleChars,
        } = this.props;

        if (renderNodeText) {
            return renderNodeText(data, id, isSelected);
        }

        return (
            <NodeText
                data={data}
                nodeTypes={nodeTypes}
                isSelected={this.state.selected}
                maxTitleChars={maxTitleChars}
            />
        );
    }

    render() {
        const {x, y, hovered, selected} = this.state;
        const {opacity, id, data} = this.props;
        const className = GraphUtils.classNames('node', data.type, {
            hovered,
            selected,
        });

        return (
            <g
                className={className}
                onMouseOver={this.handleMouseOver}
                onMouseOut={this.handleMouseOut}
                id={id}
                ref={this.nodeRef}
                opacity={opacity}
                transform={`translate(${x}, ${y})`}
                style={{transform: `matrix(1, 0, 0, 1, ${x}, ${y})`}}
            >
                {this.renderShape()}
                {this.renderText()}
            </g>
        );
    }
}

export default Node;
