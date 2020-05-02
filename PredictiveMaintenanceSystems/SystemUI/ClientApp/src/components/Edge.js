// @flow

import * as d3 from 'd3';
import * as React from 'react';
import {intersect, shape} from 'svg-intersections';
import {Matrix2D, Point2D} from 'kld-affine';
import {Intersection} from 'kld-intersections';
import GraphUtils from '../utilities/Workflow-Utils';
import {type INode} from './Node';

export type IEdge = {
    source: string,
    target: string,
    type?: string,
    handleText?: string,
    handleTooltipText?: string,
    label_from?: string,
    label_to?: string,
    [key: string]: any,
};

export type ITargetPosition = {
    x: number,
    y: number,
};

type IEdgeProps = {
    data: IEdge,
    edgeTypes: any,
    edgeHandleSize?: number,
    nodeSize?: number,
    sourceNode: INode | null,
    targetNode: INode | ITargetPosition,
    isSelected: boolean,
    nodeKey: string,
    viewWrapperElem: HTMLDivElement,
    rotateEdgeHandle: true,
};

//Handles the edges between the graphs
class Edge extends React.Component<IEdgeProps> {
    static defaultProps = {
        edgeHandleSize: 50,
        isSelected: false,
        rotateEdgeHandle: true,
    };
    edgeOverlayRef: React.ElementRef<typeof Edge>;

    constructor(props: IEdgeProps) {
        super(props);
        this.edgeOverlayRef = React.createRef();
    }

    //Calculates theta for the edge angle
    static getTheta(pt1: any, pt2: any) {
        const xComp = (pt2.x || 0) - (pt1.x || 0);
        const yComp = (pt2.y || 0) - (pt1.y || 0);
        const theta = Math.atan2(yComp, xComp);

        return theta;
    }

    //Calculates the distance for the line
    static lineFunction(srcTrgDataArray: any) {
        return d3
            .line()
            .x((d: any) => {
                return d.x;
            })
            .y((d: any) => {
                return d.y;
            })(srcTrgDataArray);
    }

    //Calculates the arrow size on the edge based on the window size
    static getArrowSize(
        viewWrapperElem: HTMLDivElement | HTMLDocument = document
    ) {
        const defEndArrowElement: any = viewWrapperElem.querySelector(
            `defs>marker>.arrow`
        );

        return defEndArrowElement.getBoundingClientRect();
    }

    //Draws the edge path
    static getEdgePathElement(
        edge: IEdge,
        viewWrapperElem: HTMLDivElement | HTMLDocument = document
    ) {
        return viewWrapperElem.querySelector(
            `#edge-${edge.source}-${edge.target}-container>.edge-container>.edge>.edge-path`
        );
    }

    //Translates the path to a visual component with the location
    static parsePathToXY(edgePathElement: Element | null) {
        const response = {
            source: {x: 0, y: 0},
            target: {x: 0, y: 0},
        };

        if (edgePathElement) {
            let d = edgePathElement.getAttribute('d');

            d = d && d.replace(/^M/, '');
            d = d && d.replace(/L/, ',');
            let dArr = (d && d.split(',')) || [];

            dArr = dArr.map(dimension => {
                return parseFloat(dimension);
            });

            if (dArr.length === 4) {
                response.source.x = dArr[0];
                response.source.y = dArr[1];
                response.target.x = dArr[2];
                response.target.y = dArr[3];
            }
        }

        return response;
    }

    //Find any intersections for the edges
    static getDefaultIntersectResponse() {
        return {
            xOff: 0,
            yOff: 0,
            intersect: {
                type: 'none',
                point: {
                    x: 0,
                    y: 0,
                },
            },
        };
    }

    //Handles rotations for the edges
    static getRotatedRectIntersect(
        defSvgRotatedRectElement: any,
        src: any,
        trg: any,
        includesArrow: boolean,
        viewWrapperElem: HTMLDivElement | HTMLDocument = document
    ) {
        const response = Edge.getDefaultIntersectResponse();
        const arrowSize = Edge.getArrowSize(viewWrapperElem);
        const clientRect = defSvgRotatedRectElement.getBoundingClientRect();

        const widthAttr = defSvgRotatedRectElement.getAttribute('width');
        const heightAttr = defSvgRotatedRectElement.getAttribute('height');
        const w = widthAttr ? parseFloat(widthAttr) : clientRect.width;
        const h = heightAttr ? parseFloat(heightAttr) : clientRect.height;
        const trgX = trg.x || 0;
        const trgY = trg.y || 0;
        const srcX = src.x || 0;
        const srcY = src.y || 0;
        const top = trgY - h / 2;
        const bottom = trgY + h / 2;
        const left = trgX - w / 2;
        const right = trgX + w / 2;
        const line = shape('line', {x1: srcX, y1: srcY, x2: trgX, y2: trgY});
        const rect = {
            topLeft: new Point2D(left, top),
            bottomRight: new Point2D(right, bottom),
        };
        const poly = [
            rect.topLeft,
            new Point2D(rect.bottomRight.x, rect.topLeft.y),
            rect.bottomRight,
            new Point2D(rect.topLeft.x, rect.bottomRight.y),
        ];
        const center = rect.topLeft.lerp(rect.bottomRight, 0.5);
        const transform = defSvgRotatedRectElement.getAttribute('transform');
        let rotate = transform
            ? transform.replace(/(rotate.[0-9]*.)|[^]/g, '$1')
            : null;
        let angle = 0;

        if (rotate) {
            rotate = rotate.replace(/^rotate\(|\)$/g, '');
            angle = (parseFloat(rotate) * Math.PI) / 180.0;
        }

        const rotation = Matrix2D.rotationAt(angle, center);
        const rotatedPoly = poly.map(p => p.transform(rotation));
        const pathIntersect = Intersection.intersectLinePolygon(
            line.params[0],
            line.params[1],
            rotatedPoly
        );

        if (pathIntersect.points.length > 0) {
            let arrowWidth = 0;
            let arrowHeight = 0;
            const xIntersect = pathIntersect.points[0].x;
            const yIntersect = pathIntersect.points[0].y;

            if (xIntersect > left && xIntersect < right && yIntersect > trgY) {
                arrowHeight = arrowSize.height;
            } else if (xIntersect > left && xIntersect < right && yIntersect < trgY) {
                arrowHeight = -arrowSize.height;
            } else if (yIntersect > top && yIntersect < bottom && xIntersect < trgX) {
                arrowWidth = -arrowSize.width;
            } else {
                arrowWidth = arrowSize.width;
            }

            response.xOff =
                trgX - xIntersect - (includesArrow ? arrowWidth / 1.25 : 0);
            response.yOff =
                trgY - yIntersect - (includesArrow ? arrowHeight / 1.25 : 0);
            response.intersect = pathIntersect.points[0];
        }

        return response;
    }

    //Checks for any edge intersects on the graph
    static getPathIntersect(
        defSvgPathElement: any,
        src: any,
        trg: any,
        includesArrow?: boolean,
        viewWrapperElem: HTMLDivElement | HTMLDocument = document
    ) {
        const response = Edge.getDefaultIntersectResponse();
        const arrowSize = Edge.getArrowSize(viewWrapperElem);
        const clientRect = defSvgPathElement.getBoundingClientRect();
        const w = clientRect.width;
        const h = clientRect.height;
        const trgX = trg.x || 0;
        const trgY = trg.y || 0;
        const srcX = src.x || 0;
        const srcY = src.y || 0;
        const top = trgY - h / 2;
        const bottom = trgY + h / 2;
        const left = trgX - w / 2;
        const right = trgX + w / 2;

        let d = defSvgPathElement.getAttribute('d');

        if (!/^M/.test(d)) {
            return;
        }

        d = d.replace(/^M /, '');
        let dArr = d.split(/[ ,]+/);

        dArr = dArr.map((val, index) => {
            let isEnd = false;

            if (/Z$/.test(val)) {
                val = val.replace(/Z$/, '');
                isEnd = true;
            }

            if (index % 2 === 0) {
                return parseFloat(val) + left + (isEnd ? 'Z' : '');
            }

            return parseFloat(val) + top + (isEnd ? 'Z' : '');
        });

        const pathIntersect = intersect(
            shape('path', {d: 'M ' + dArr.join(' ')}),
            shape('line', {x1: srcX, y1: srcY, x2: trgX, y2: trgY})
        );

        if (pathIntersect.points.length > 0) {
            let arrowWidth = 0;
            let arrowHeight = 0;
            const xIntersect = pathIntersect.points[0].x;
            const yIntersect = pathIntersect.points[0].y;
            let multiplier = 1;

            if (xIntersect > left && xIntersect < right) {
                const yIntersectDiff = yIntersect - trgY;
                multiplier = yIntersect < trgY ? -1 : 1;
                arrowHeight = arrowSize.height * multiplier;
                arrowHeight = arrowHeight * Math.min(Math.abs(yIntersectDiff), 1);
            }

            if (yIntersect > top && yIntersect < bottom) {
                const xIntersectDiff = xIntersect - trgX;
                multiplier = xIntersect < trgX ? -1 : 1;
                arrowWidth = arrowSize.width * multiplier;
                arrowWidth = arrowWidth * Math.min(Math.abs(xIntersectDiff), 1);
            }

            response.xOff =
                trgX - xIntersect - (includesArrow ? arrowWidth / 1.25 : 0);
            response.yOff =
                trgY - yIntersect - (includesArrow ? arrowHeight / 1.25 : 0);

            response.intersect = pathIntersect.points[0];
        }

        return response;
    }

    //Checks for any nodes that may intersect
    static getCircleIntersect(
        defSvgCircleElement: any,
        src: any,
        trg: any,
        includesArrow?: boolean,
        viewWrapperElem: HTMLDivElement | HTMLDocument = document
    ) {
        const response = Edge.getDefaultIntersectResponse();
        const arrowSize = Edge.getArrowSize(viewWrapperElem);
        const arrowWidth = arrowSize.width;
        const arrowHeight = arrowSize.height;
        const clientRect = defSvgCircleElement.getBoundingClientRect();
        const parentElement = defSvgCircleElement.parentElement;
        let parentWidth = parentElement.getAttribute('width');
        let parentHeight = parentElement.getAttribute('width');

        if (parentWidth) {
            parentWidth = parseFloat(parentWidth);
        }

        if (parentHeight) {
            parentHeight = parseFloat(parentHeight);
        }

        const w = parentWidth ? parentWidth : clientRect.width;
        const h = parentHeight ? parentHeight : clientRect.height;
        const trgX = trg.x || 0;
        const trgY = trg.y || 0;
        const srcX = src.x || 0;
        const srcY = src.y || 0;
        const arrowOffsetDiviser = 1.25;
        const offX = w / 2 + (includesArrow ? arrowWidth / arrowOffsetDiviser : 0);
        const offY = h / 2 + (includesArrow ? arrowHeight / arrowOffsetDiviser : 0);
        const pathIntersect = intersect(
            shape('ellipse', {
                rx: offX,
                ry: offY,
                cx: trgX,
                cy: trgY,
            }),
            shape('line', {x1: srcX, y1: srcY, x2: trgX, y2: trgY})
        );

        if (pathIntersect.points.length > 0) {
            const xIntersect = pathIntersect.points[0].x;
            const yIntersect = pathIntersect.points[0].y;

            response.xOff = trgX - xIntersect;
            response.yOff = trgY - yIntersect;
            response.intersect = pathIntersect.points[0];
        }

        return response;
    }

    //Calculates moving the edges
    static calculateOffset(
        nodeSize: any,
        src: any,
        trg: any,
        nodeKey: string,
        includesArrow?: boolean,
        viewWrapperElem?: HTMLDivElement
    ) {
        let response = Edge.getDefaultIntersectResponse();

        if (!trg[nodeKey]) {
            return response;
        }

        const nodeElem = document.getElementById(`node-${trg[nodeKey]}`);

        if (!nodeElem) {
            return response;
        }

        const trgNode = nodeElem.querySelector(`use.node`);

        if (!trgNode || (trgNode && !trgNode.getAttributeNS)) {
            return response;
        }

        const xlinkHref = trgNode.getAttributeNS(
            'http://www.w3.org/1999/xlink',
            'href'
        );

        if (!xlinkHref) {
            return response;
        }

        const defSvgRectElement: SVGRectElement | null = viewWrapperElem.querySelector(
            `defs>${xlinkHref} rect:not([data-intersect-ignore=true])`
        );

        const defSvgPathElement: SVGPathElement | null = !defSvgRectElement
            ? viewWrapperElem.querySelector(
                `defs>${xlinkHref} path:not([data-intersect-ignore=true])`
            )
            : null;
        const defSvgCircleElement:
            | SVGCircleElement
            | SVGEllipseElement
            | SVGPolygonElement
            | null =
            !defSvgPathElement && !defSvgPathElement
                ? viewWrapperElem.querySelector(
                `defs>${xlinkHref} circle:not([data-intersect-ignore=true]), defs>${xlinkHref} ellipse:not([data-intersect-ignore=true]), defs>${xlinkHref} polygon:not([data-intersect-ignore=true])`
                )
                : null;

        if (defSvgRectElement) {
            response = {
                ...response,
                ...Edge.getRotatedRectIntersect(
                    defSvgRectElement,
                    src,
                    trg,
                    includesArrow,
                    viewWrapperElem
                ),
            };
        } else if (defSvgPathElement) {
            response = {
                ...response,
                ...Edge.getPathIntersect(
                    defSvgPathElement,
                    src,
                    trg,
                    includesArrow,
                    viewWrapperElem
                ),
            };
        } else {
            response = {
                ...response,
                ...Edge.getCircleIntersect(
                    defSvgCircleElement,
                    src,
                    trg,
                    includesArrow,
                    viewWrapperElem
                ),
            };
        }

        return response;
    }

    //Gets the connection between the edge and a node
    static getXlinkHref(edgeTypes: any, data: any) {
        if (data.type && edgeTypes[data.type]) {
            return edgeTypes[data.type].shapeId;
        } else if (edgeTypes.emptyEdge) {
            return edgeTypes.emptyEdge.shapeId;
        }

        return null;
    }

    //Handles the edge connection between the graphs
    getEdgeHandleTranslation = () => {
        const {data} = this.props;

        let pathDescription = this.getPathDescription(data);

        pathDescription = pathDescription.replace(/^M/, '');
        pathDescription = pathDescription.replace(/L/, ',');
        const pathDescriptionArr = pathDescription.split(',');
        const diffX =
            parseFloat(pathDescriptionArr[2]) - parseFloat(pathDescriptionArr[0]);
        const diffY =
            parseFloat(pathDescriptionArr[3]) - parseFloat(pathDescriptionArr[1]);
        const x = parseFloat(pathDescriptionArr[0]) + diffX / 2;
        const y = parseFloat(pathDescriptionArr[1]) + diffY / 2;

        return `translate(${x}, ${y})`;
    };

    //Checks the offset of the visual component
    getEdgeHandleOffsetTranslation = () => {
        const offset = -(this.props.edgeHandleSize || 0) / 2;

        return `translate(${offset}, ${offset})`;
    };

    //Check the rotation of the graph from the input
    getEdgeHandleRotation = (negate: any = false) => {
        let rotated = false;
        const src = this.props.sourceNode;
        const trg = this.props.targetNode;
        let theta = (Edge.getTheta(src, trg) * 180) / Math.PI;

        if (negate) {
            theta = -theta;
        }

        if (theta > 90 || theta < -90) {
            theta = theta + 180;
            rotated = true;
        }

        return [`rotate(${theta})`, rotated];
    };

    //Sets the translation of the edge
    getEdgeHandleTransformation = () => {
        const translation = this.getEdgeHandleTranslation();
        const rotation = this.props.rotateEdgeHandle
            ? this.getEdgeHandleRotation()[0]
            : '';
        const offset = this.getEdgeHandleOffsetTranslation();

        return `${translation} ${rotation} ${offset}`;
    };

    //The path from the nodes
    getPathDescription(edge: any) {
        const {
            sourceNode,
            targetNode,
            nodeKey,
            nodeSize,
            viewWrapperElem,
        } = this.props;
        const trgX = targetNode && targetNode.x ? targetNode.x : 0;
        const trgY = targetNode && targetNode.y ? targetNode.y : 0;
        const srcX = targetNode && sourceNode.x ? sourceNode.x : 0;
        const srcY = targetNode && sourceNode.y ? sourceNode.y : 0;
        const srcOff = Edge.calculateOffset(
            nodeSize || 0,
            targetNode,
            sourceNode,
            nodeKey,
            false,
            viewWrapperElem
        );
        const trgOff = Edge.calculateOffset(
            nodeSize || 0,
            sourceNode,
            targetNode,
            nodeKey,
            true,
            viewWrapperElem
        );

        const linePoints = [
            {
                x: srcX - srcOff.xOff,
                y: srcY - srcOff.yOff,
            },
            {
                x: trgX - trgOff.xOff,
                y: trgY - trgOff.yOff,
            },
        ];

        return Edge.lineFunction(linePoints);
    }

    //Render the edge text
    renderHandleText(data: any) {
        return (
            <text
                className="edge-text"
                textAnchor="middle"
                alignmentBaseline="central"
                transform={`${this.getEdgeHandleTranslation()}`}
            >
                {data.handleText}
            </text>
        );
    }

    //Render the label for the edge
    renderLabelText(data: any) {
        const [rotation, isRotated] = this.getEdgeHandleRotation();
        const title = isRotated
            ? `${data.label_to} ↔ ${data.label_from}`
            : `${data.label_from} ↔ ${data.label_to}`;

        return (
            <text
                className="edge-text"
                textAnchor="middle"
                alignmentBaseline="central"
                style={{fontSize: '11px', stroke: 'none', fill: 'black'}}
                transform={`${this.getEdgeHandleTranslation()} ${rotation} translate(0,-5)`}
            >
                {title}
            </text>
        );
    }

    //Render the edge when it is created on the graph
    render() {
        const {data, edgeTypes, edgeHandleSize, viewWrapperElem} = this.props;

        if (!viewWrapperElem) {
            return null;
        }

        const id = `${data.source || ''}_${data.target}`;
        const className = GraphUtils.classNames('edge', {
            selected: this.props.isSelected,
        });
        const edgeHandleTransformation = this.getEdgeHandleTransformation();

        return (
            <g
                className="edge-container"
                data-source={data.source}
                data-target={data.target}
            >
                <g className={className}>
                    <path
                        className="edge-path"
                        d={this.getPathDescription(data) || undefined}
                    />
                    <use
                        xlinkHref={Edge.getXlinkHref(edgeTypes, data)}
                        width={edgeHandleSize}
                        height={edgeHandleSize}
                        transform={edgeHandleTransformation}
                        style={{transform: edgeHandleTransformation}}
                    />
                    {data.handleText && this.renderHandleText(data)}
                    {data.label_from && data.label_to && this.renderLabelText(data)}
                </g>
                <g className="edge-mouse-handler">
                    <title>{data.handleTooltipText}</title>
                    <path
                        className="edge-overlay-path"
                        ref={this.edgeOverlayRef}
                        id={id}
                        data-source={data.source}
                        data-target={data.target}
                        d={this.getPathDescription(data) || undefined}
                    />
                </g>
            </g>
        );
    }
}

export default Edge;