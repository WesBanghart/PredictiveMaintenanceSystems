// @flow

import * as d3 from 'd3';
import * as React from 'react';
import ReactDOM from 'react-dom';
import '../styles/Workflow-Main.scss';
import {LayoutEngines} from '../utilities/layout-engine/Layout-EngineConfig';
import {type IGraphViewProps} from './Workflow-ViewPros';
import Background from './Background';
import Definitions from './Definitions';
import Edge, {type IEdge} from './Edge';
import WorkflowControls from './Workflow-Controls';
import GraphUtils, {type INodeMapNode} from '../utilities/Workflow-Utils';
import Node, {type INode, type IPoint} from './Node';

type IViewTransform = {
    k: number,
    x: number,
    y: number,
};

type IBBox = {
    x: number,
    y: number,
    width: number,
    height: number,
};

type IGraphViewState = {
    viewTransform?: IViewTransform,
    hoveredNode: boolean,
    nodesMap: any,
    edgesMap: any,
    nodes: any[],
    edges: any[],
    selectingNode: boolean,
    hoveredNodeData: INode | null,
    edgeEndNode: INode | null,
    draggingEdge: boolean,
    draggedEdge: any,
    componentUpToDate: boolean,
    selectedEdgeObj: any,
    selectedNodeObj: any,
    documentClicked: boolean,
    svgClicked: boolean,
    focused: boolean,
};

class WorkflowView extends React.Component<IGraphViewProps, IGraphViewState> {
    static defaultProps = {
        canCreateEdge: (startNode?: INode, endNode?: INode) => true,
        canDeleteEdge: () => true,
        canDeleteNode: () => true,
        edgeArrowSize: 8,
        gridSpacing: 36,
        layoutEngineType: 'None',
        maxZoom: 1.5,
        minZoom: 0.15,
        nodeSize: 154,
        readOnly: false,
        showGraphControls: false,
        zoomDelay: 1000,
        zoomDur: 750,
        rotateEdgeHandle: true,
        centerNodeOnMove: true,
    };
    nodeTimeouts: any;
    edgeTimeouts: any;
    renderNodesTimeout: any;
    renderEdgesTimeout: any;
    zoom: any;
    viewWrapper: React.RefObject<HTMLDivElement>;
    graphSvg: React.RefObject<SVGElement>;
    entities: any;
    selectedView: any;
    view: any;
    graphControls: any;
    layoutEngine: any;

    constructor(props: IGraphViewProps) {
        super(props);

        this.nodeTimeouts = {};
        this.edgeTimeouts = {};
        this.renderNodesTimeout = null;
        this.renderEdgesTimeout = null;
        this.viewWrapper = React.createRef();
        this.graphControls = React.createRef();
        this.graphSvg = React.createRef();

        if (props.layoutEngineType) {
            this.layoutEngine = new LayoutEngines[props.layoutEngineType](props);
        }

        this.state = {
            componentUpToDate: false,
            draggedEdge: null,
            draggingEdge: false,
            edgeEndNode: null,
            edges: [],
            edgesMap: {},
            hoveredNode: false,
            hoveredNodeData: null,
            nodes: [],
            nodesMap: {},
            selectedEdgeObj: null,
            selectedNodeObj: null,
            selectingNode: false,
            documentClicked: false,
            svgClicked: false,
            focused: true,
        };
    }

    static getDerivedStateFromProps(
        nextProps: IGraphViewProps,
        prevState: IGraphViewState
    ) {
        const {edges, nodeKey} = nextProps;
        let nodes = nextProps.nodes;
        const nodesMap = GraphUtils.getNodesMap(nodes, nodeKey);
        const edgesMap = GraphUtils.getEdgesMap(edges);

        GraphUtils.linkNodesAndEdges(nodesMap, edges);

        const selectedNodeMap =
            nextProps.selected && nodesMap[`key-${nextProps.selected[nodeKey]}`]
                ? nodesMap[`key-${nextProps.selected[nodeKey]}`]
                : null;
        const selectedEdgeMap =
            nextProps.selected &&
            edgesMap[`${nextProps.selected.source}_${nextProps.selected.target}`]
                ? edgesMap[`${nextProps.selected.source}_${nextProps.selected.target}`]
                : null;

        if (
            prevState.nodes.length === 0 &&
            nextProps.layoutEngineType &&
            LayoutEngines[nextProps.layoutEngineType]
        ) {
            const layoutEngine = new LayoutEngines[nextProps.layoutEngineType](
                nextProps
            );
            const newNodes = layoutEngine.adjustNodes(nodes, nodesMap);

            nodes = newNodes;
        }

        const nextState = {
            componentUpToDate: true,
            edges,
            edgesMap,
            nodes,
            nodesMap,
            readOnly: nextProps.readOnly,
            selectedEdgeObj: {
                edge: selectedEdgeMap ? selectedEdgeMap.edge : null,
            },
            selectedNodeObj: {
                nodeId: selectedNodeMap ? nextProps.selected[nodeKey] : null,
                node: selectedNodeMap ? selectedNodeMap.node : null,
            },
            selectionChanged: false,
        };

        return nextState;
    }

    componentDidMount() {
        const {initialBBox, zoomDelay, minZoom, maxZoom} = this.props;

        // TODO: can we target the element rather than the document?
        document.addEventListener('keydown', this.handleWrapperKeydown);
        document.addEventListener('click', this.handleDocumentClick);

        this.zoom = d3
            .zoom()
            .filter(this.zoomFilter)
            .scaleExtent([minZoom || 0, maxZoom || 0])
            .on('start', () => {
                this.handleZoomStart(d3.event);
            })
            .on('zoom', () => {
                this.handleZoom(d3.event);
            })
            .on('end', this.handleZoomEnd);

        d3.select(this.viewWrapper.current)
            .on('touchstart', this.containZoom)
            .on('touchmove', this.containZoom)
            .on('click', this.handleSvgClicked)
            .select('svg')
            .call(this.zoom);

        this.selectedView = d3.select(this.view);

        if (initialBBox) {
            this.handleZoomToFitImpl(initialBBox, 0);
            this.renderView();

            return;
        }

        this.renderView();
        setTimeout(() => {
            if (this.viewWrapper.current != null) {
                this.handleZoomToFit();
            }
        }, zoomDelay);
    }

    componentWillUnmount() {
        document.removeEventListener('keydown', this.handleWrapperKeydown);
        document.removeEventListener('click', this.handleDocumentClick);
    }

    shouldComponentUpdate(
        nextProps: IGraphViewProps,
        nextState: IGraphViewState
    ) {
        if (
            nextProps.nodes !== this.props.nodes ||
            nextProps.edges !== this.props.edges ||
            !nextState.componentUpToDate ||
            nextProps.selected !== this.props.selected ||
            nextProps.readOnly !== this.props.readOnly ||
            nextProps.layoutEngineType !== this.props.layoutEngineType
        ) {
            return true;
        }

        return false;
    }

    componentDidUpdate(prevProps: IGraphViewProps, prevState: IGraphViewState) {
        const {
            nodesMap,
            edgesMap,
            nodes,
            selectedNodeObj,
            selectedEdgeObj,
        } = this.state;
        const {layoutEngineType} = this.props;

        if (layoutEngineType && LayoutEngines[layoutEngineType]) {
            this.layoutEngine = new LayoutEngines[layoutEngineType](this.props);
            const newNodes = this.layoutEngine.adjustNodes(nodes, nodesMap);

            this.setState({
                nodes: newNodes,
            });
        }

        const forceReRender = prevProps.layoutEngineType !== layoutEngineType;

        this.removeOldEdges(prevState.edges, edgesMap);
        this.removeOldNodes(prevState.nodes, prevState.nodesMap, nodesMap);
        this.addNewNodes(
            this.state.nodes,
            prevState.nodesMap,
            selectedNodeObj,
            prevState.selectedNodeObj,
            forceReRender
        );

        this.addNewEdges(
            this.state.edges,
            prevState.edgesMap,
            selectedEdgeObj,
            prevState.selectedEdgeObj,
            forceReRender
        );

        this.setState({
            componentUpToDate: true,
        });
    }

    getNodeById(id: string | null, nodesMap: any | null): INodeMapNode | null {
        const nodesMapVar = nodesMap || this.state.nodesMap;

        return nodesMapVar ? nodesMapVar[`key-${id || ''}`] : null;
    }

    getEdgeBySourceTarget(source: string, target: string): IEdge | null {
        return this.state.edgesMap
            ? this.state.edgesMap[`${source}_${target}`]
            : null;
    }

    deleteEdgeBySourceTarget(source: string, target: string) {
        if (this.state.edgesMap && this.state.edgesMap[`${source}_${target}`]) {
            delete this.state.edgesMap[`${source}_${target}`];
        }
    }

    addNewNodes(
        nodes: INode[],
        oldNodesMap: any,
        selectedNode: any,
        prevSelectedNode: any,
        forceRender: boolean = false
    ) {
        if (this.state.draggingEdge) {
            return;
        }

        const nodeKey = this.props.nodeKey;
        let node = null;
        let prevNode = null;

        GraphUtils.yieldingLoop(nodes.length, 50, i => {
            node = nodes[i];
            prevNode = this.getNodeById(node[nodeKey], oldNodesMap);
            if (
                prevNode != null &&
                (!GraphUtils.isEqual(prevNode.node, node) ||
                    (selectedNode.node !== prevSelectedNode.node &&
                        ((selectedNode.node &&
                            node[nodeKey] === selectedNode.node[nodeKey]) ||
                            (prevSelectedNode.node &&
                                node[nodeKey] === prevSelectedNode.node[nodeKey]))))
            ) {
                this.asyncRenderNode(node);
            } else if (forceRender || !prevNode) {
                this.asyncRenderNode(node);
            }
        });
    }

    removeOldNodes(prevNodes: any, prevNodesMap: any, nodesMap: any) {
        const nodeKey = this.props.nodeKey;
        for (let i = 0; i < prevNodes.length; i++) {
            const prevNode = prevNodes[i];
            const nodeId = prevNode[nodeKey];
            if (this.getNodeById(nodeId, nodesMap)) {
                continue;
            }

            const prevNodeMapNode = this.getNodeById(nodeId, prevNodesMap);

            prevNodeMapNode.outgoingEdges.forEach(edge => {
                this.removeEdgeElement(edge.source, edge.target);
            });

            prevNodeMapNode.incomingEdges.forEach(edge => {
                this.removeEdgeElement(edge.source, edge.target);
            });

            setTimeout(() => {
                GraphUtils.removeElementFromDom(`node-${nodeId}-container`);
            });
        }
    }

    addNewEdges(
        edges: IEdge[],
        oldEdgesMap: any,
        selectedEdge: any,
        prevSelectedEdge: any,
        forceRender: boolean = false
    ) {
        if (!this.state.draggingEdge) {
            let edge = null;

            GraphUtils.yieldingLoop(edges.length, 50, i => {
                edge = edges[i];

                if (!edge.source || !edge.target) {
                    return;
                }

                const prevEdge = oldEdgesMap[`${edge.source}_${edge.target}`];

                if (
                    forceRender ||
                    !prevEdge ||
                    !GraphUtils.isEqual(prevEdge.edge, edge) ||
                    (selectedEdge.edge && edge === selectedEdge.edge) ||
                    (prevSelectedEdge.edge && prevSelectedEdge.edge)
                ) {
                    this.asyncRenderEdge(edge);
                }
            });
        }
    }

    removeOldEdges = (prevEdges: IEdge[], edgesMap: any) => {
        let edge = null;
        for (let i = 0; i < prevEdges.length; i++) {
            edge = prevEdges[i];
            if (
                !edge.source ||
                !edge.target ||
                !edgesMap[`${edge.source}_${edge.target}`]
            ) {
                this.removeEdgeElement(edge.source, edge.target);

            }
        }
    };

    removeEdgeElement(source: string, target: string) {
        const id = `${source}-${target}`;

        GraphUtils.removeElementFromDom(`edge-${id}-container`);
    }

    canSwap(sourceNode: INode, hoveredNode: INode | null, swapEdge: any) {
        return (
            hoveredNode &&
            sourceNode !== hoveredNode &&
            (swapEdge.source !== sourceNode[this.props.nodeKey] ||
                swapEdge.target !== hoveredNode[this.props.nodeKey])
        );
    }

    deleteNode(selectedNode: INode) {
        const {nodeKey} = this.props;
        const {nodes} = this.state;
        const nodeId = selectedNode[nodeKey];
        const newNodesArr = nodes.filter(node => node[nodeKey] !== nodeId);

        this.setState({
            componentUpToDate: false,
            hoveredNode: false,
        });

        GraphUtils.removeElementFromDom(`node-${nodeId}-container`);

        this.props.onSelectNode(null);
        this.props.onDeleteNode(selectedNode, nodeId, newNodesArr);
    }

    deleteEdge(selectedEdge: IEdge) {
        const {edges} = this.state;

        if (!selectedEdge.source || !selectedEdge.target) {
            return;
        }

        const newEdgesArr = edges.filter(edge => {
            return !(
                edge.source === selectedEdge.source &&
                edge.target === selectedEdge.target
            );
        });

        if (selectedEdge.source && selectedEdge.target) {
            this.deleteEdgeBySourceTarget(selectedEdge.source, selectedEdge.target);
        }

        this.setState({
            componentUpToDate: false,
            edges: newEdgesArr,
        });

        if (selectedEdge.source && selectedEdge.target) {
            GraphUtils.removeElementFromDom(
                `edge-${selectedEdge.source}-${selectedEdge.target}-custom-container`
            );
            GraphUtils.removeElementFromDom(
                `edge-${selectedEdge.source}-${selectedEdge.target}-container`
            );
        }

        this.props.onDeleteEdge(selectedEdge, newEdgesArr);
    }

    handleDelete = (selected: IEdge | INode) => {
        const {canDeleteNode, canDeleteEdge, readOnly} = this.props;

        if (readOnly || !selected) {
            return;
        }

        if (!selected.source && canDeleteNode && canDeleteNode(selected)) {
            this.deleteNode(selected);
        } else if (selected.source && canDeleteEdge && canDeleteEdge(selected)) {
            this.deleteEdge(selected);
        }
    };

    handleWrapperKeydown: KeyboardEventListener = d => {
        const {selected, onUndo, onCopySelected, onPasteSelected} = this.props;
        const {focused, selectedNodeObj} = this.state;
        if (!focused) {
            return;
        }

        switch (d.key) {
            case 'Delete':
            case 'Backspace':
                if (selectedNodeObj) {
                    this.handleDelete(selectedNodeObj.node || selected);
                }

                break;
            case 'z':
                if ((d.metaKey || d.ctrlKey) && onUndo) {
                    onUndo();
                }

                break;
            case 'c':
                if (
                    (d.metaKey || d.ctrlKey) &&
                    selectedNodeObj.node &&
                    onCopySelected
                ) {
                    onCopySelected();
                }

                break;
            case 'v':
                if (
                    (d.metaKey || d.ctrlKey) &&
                    selectedNodeObj.node &&
                    onPasteSelected
                ) {
                    onPasteSelected();
                }

                break;
            default:
                break;
        }
    };

    handleEdgeSelected = e => {
        const {source, target} = e.target.dataset;
        let newState = {
            svgClicked: true,
            focused: true,
        };

        if (source && target) {
            const edge: IEdge | null = this.getEdgeBySourceTarget(source, target);

            if (!edge) {
                return;
            }

            const originalArrIndex = (this.getEdgeBySourceTarget(source, target): any)
                .originalArrIndex;

            newState = {
                ...newState,
                selectedEdgeObj: {
                    componentUpToDate: false,
                    edge: this.state.edges[originalArrIndex],
                },
            };
            this.setState(newState);
            this.props.onSelectEdge(this.state.edges[originalArrIndex]);
        } else {
            this.setState(newState);
        }
    };

    handleSvgClicked = (d: any, i: any) => {
        const {
            onBackgroundClick,
            onSelectNode,
            readOnly,
            onCreateNode,
        } = this.props;

        if (this.isPartOfEdge(d3.event.target)) {
            this.handleEdgeSelected(d3.event);

            return;
        }

        if (this.state.selectingNode) {
            this.setState({
                focused: true,
                selectingNode: false,
                svgClicked: true,
            });
        } else {
            if (
                !d3.event.shiftKey &&
                onBackgroundClick &&
                d3.event.target.classList.contains('background')
            ) {
                const xycoords = d3.mouse(d3.event.target);

                onBackgroundClick(xycoords[0], xycoords[1], d3.event);
            }

            const previousSelection =
                (this.state.selectedNodeObj && this.state.selectedNodeObj.node) || null;

            this.setState({
                selectedNodeObj: null,
                focused: true,
                svgClicked: true,
            });
            onSelectNode(null);

            if (previousSelection) {
                this.syncRenderNode(previousSelection);
            }

            if (!readOnly && d3.event.shiftKey) {
                const xycoords = d3.mouse(d3.event.target);

                onCreateNode(xycoords[0], xycoords[1], d3.event);
            }
        }
    };

    handleDocumentClick = (event: any) => {
        if (
            event &&
            event.target &&
            event.target.ownerSVGElement != null &&
            event.target.ownerSVGElement === this.graphSvg.current
        ) {
            return;
        }

        this.setState({
            documentClicked: true,
            focused: false,
            svgClicked: false,
        });
    };

    isPartOfEdge(element: any) {
        return !!GraphUtils.findParent(element, '.edge-container');
    }

    handleNodeMove = (position: IPoint, nodeId: string, shiftKey: boolean) => {
        const {canCreateEdge, readOnly} = this.props;
        const nodeMapNode: INodeMapNode | null = this.getNodeById(nodeId);

        if (!nodeMapNode) {
            return;
        }

        const node = nodeMapNode.node;

        if (readOnly) {
            return;
        }

        if (!shiftKey && !this.state.draggingEdge) {
            node.x = position.x;
            node.y = position.y;
            this.renderConnectedEdgesFromNode(nodeMapNode, true);
            this.asyncRenderNode(node);
        } else if (
            (canCreateEdge && canCreateEdge(nodeId)) ||
            this.state.draggingEdge
        ) {
            this.syncRenderEdge({source: nodeId, targetPosition: position});
            this.setState({draggingEdge: true});
        }
    };

    createNewEdge() {
        const {canCreateEdge, nodeKey, onCreateEdge} = this.props;
        const {edgesMap, edgeEndNode, hoveredNodeData} = this.state;

        if (!hoveredNodeData) {
            return;
        }

        GraphUtils.removeElementFromDom('edge-custom-container');

        if (edgeEndNode) {
            const mapId1 = `${hoveredNodeData[nodeKey]}_${edgeEndNode[nodeKey]}`;
            const mapId2 = `${edgeEndNode[nodeKey]}_${hoveredNodeData[nodeKey]}`;

            if (
                edgesMap &&
                hoveredNodeData !== edgeEndNode &&
                canCreateEdge &&
                canCreateEdge(hoveredNodeData, edgeEndNode) &&
                !edgesMap[mapId1] &&
                !edgesMap[mapId2]
            ) {
                this.setState({
                    componentUpToDate: false,
                    draggedEdge: null,
                    draggingEdge: false,
                });

                onCreateEdge(hoveredNodeData, edgeEndNode);
            } else {
                this.setState({
                    edgeEndNode: null,
                    draggingEdge: false,
                });
            }
        }
    }

    handleNodeUpdate = (position: any, nodeId: string, shiftKey: boolean) => {
        const {onUpdateNode, readOnly} = this.props;

        if (readOnly) {
            return;
        }

        if (shiftKey) {
            this.createNewEdge();
        } else {
            const nodeMap = this.getNodeById(nodeId);

            if (nodeMap) {
                Object.assign(nodeMap.node, position);
                onUpdateNode(nodeMap.node);
            }
        }

        this.setState({
            componentUpToDate: false,
            focused: true,
            hoveredNode: false,
            svgClicked: true,
        });
    };

    handleNodeMouseEnter = (event: any, data: any, hovered: boolean) => {
        if (hovered && !this.state.hoveredNode) {
            this.setState({
                hoveredNode: true,
                hoveredNodeData: data,
            });
        } else if (!hovered && this.state.hoveredNode && this.state.draggingEdge) {
            this.setState({
                edgeEndNode: data,
            });
        } else {
            this.setState({
                hoveredNode: true,
                hoveredNodeData: data,
            });
        }
    };

    handleNodeMouseLeave = (event: any, data: any) => {
        if (
            (d3.event &&
                d3.event.toElement &&
                GraphUtils.findParent(d3.event.toElement, '.node')) ||
            (event &&
                event.relatedTarget &&
                GraphUtils.findParent(event.relatedTarget, '.node')) ||
            (d3.event && d3.event.buttons === 1) ||
            (event && event.buttons === 1)
        ) {
            return;
        }

        if (event && event.relatedTarget) {
            if (event.relatedTarget.classList.contains('edge-overlay-path')) {
                return;
            }

            this.setState({hoveredNode: false, edgeEndNode: null});
        }
    };

    handleNodeSelected = (
        node: INode,
        nodeId: string,
        creatingEdge: boolean,
        event?: any
    ) => {
        const newState = {
            componentUpToDate: false,
            selectedNodeObj: {
                nodeId,
                node,
            },
        };

        this.setState(newState);

        if (!creatingEdge) {
            this.props.onSelectNode(node, event);
        }
    };

    isArrowClicked(edge: IEdge | null) {
        const {edgeArrowSize} = this.props;
        const eventTarget = d3.event.sourceEvent.target;
        const arrowSize = edgeArrowSize || 0;

        if (!edge || eventTarget.tagName !== 'path') {
            return false;
        }

        const xycoords = d3.mouse(eventTarget);

        if (!edge.target) {
            return false;
        }

        const source = {
            x: xycoords[0],
            y: xycoords[1],
        };
        const edgeCoords = Edge.parsePathToXY(
            Edge.getEdgePathElement(edge, this.viewWrapper.current)
        );

        return (
            source.x < edgeCoords.target.x + arrowSize &&
            source.x > edgeCoords.target.x - arrowSize &&
            source.y < edgeCoords.target.y + arrowSize &&
            source.y > edgeCoords.target.y - arrowSize
        );
    }

    zoomFilter() {
        if (d3.event.button || d3.event.ctrlKey) {
            return false;
        }

        return true;
    }

    containZoom() {
        const stop = d3.event.button || d3.event.ctrlKey;

        if (stop) {
            d3.event.stopImmediatePropagation(); // stop zoom
        }
    }

    handleZoomStart = (event: any) => {

        const sourceEvent = event.sourceEvent;

        if (
            this.props.readOnly ||
            !sourceEvent ||
            (sourceEvent && !sourceEvent.buttons) ||
            (sourceEvent &&
                !sourceEvent.target.classList.contains('edge-overlay-path'))
        ) {
            return false;
        }
        const {target} = sourceEvent;
        const edgeId = target.id;
        const edge =
            this.state.edgesMap && this.state.edgesMap[edgeId]
                ? this.state.edgesMap[edgeId].edge
                : null;

        if (!this.isArrowClicked(edge) || !edge) {
            return false;
        }

        this.removeEdgeElement(edge.source, edge.target);
        this.setState({draggingEdge: true, draggedEdge: edge});
        this.dragEdge(edge, d3.mouse);
    };

    getMouseCoordinates(mouse: typeof d3.mouse) {
        let mouseCoordinates = [0, 0];

        if (this.selectedView && mouse) {
            mouseCoordinates = mouse(this.selectedView.node());
        }

        return mouseCoordinates;
    }

    dragEdge(draggedEdge?: IEdge, mouse: typeof d3.mouse) {
        const {nodeSize, nodeKey} = this.props;

        draggedEdge = draggedEdge || this.state.draggedEdge;

        if (!draggedEdge) {
            return;
        }

        const mouseCoordinates = this.getMouseCoordinates(mouse);
        const targetPosition = {
            x: mouseCoordinates[0],
            y: mouseCoordinates[1],
        };

        const off = Edge.calculateOffset(
            nodeSize,
            (this.getNodeById(draggedEdge.source): any).node,
            targetPosition,
            nodeKey
        );

        targetPosition.x += off.xOff;
        targetPosition.y += off.yOff;
        this.syncRenderEdge({source: draggedEdge.source, targetPosition});
        this.setState({
            draggedEdge,
            draggingEdge: true,
        });
    }

    handleZoom = (event: any) => {
        const {draggingEdge} = this.state;
        const transform: IViewTransform = event.transform;

        if (!draggingEdge) {
            d3.select(this.view).attr('transform', transform);

            if (this.state.viewTransform !== transform) {
                this.setState(
                    {
                        viewTransform: transform,
                        draggedEdge: null,
                        draggingEdge: false,
                    },
                    () => {
                        this.renderGraphControls();
                    }
                );
            }
        } else if (draggingEdge) {
            this.dragEdge(undefined, d3.mouse);

            return false;
        }
    };

    handleZoomEnd = () => {
        const {draggingEdge, draggedEdge, edgeEndNode} = this.state;

        const {nodeKey} = this.props;

        if (!draggingEdge || !draggedEdge) {
            if (draggingEdge && !draggedEdge) {
                this.setState({
                    draggingEdge: false,
                });
            }

            return;
        }

        const draggedEdgeCopy = {...this.state.draggedEdge};
        GraphUtils.removeElementFromDom('edge-custom-container');
        this.setState(
            {
                draggedEdge: null,
                draggingEdge: false,
                hoveredNode: false,
            },
            () => {
                const sourceNodeById = this.getNodeById(draggedEdge.source);
                const targetNodeById = this.getNodeById(draggedEdge.target);

                if (!sourceNodeById || !targetNodeById) {
                    return;
                }

                const sourceNode = sourceNodeById.node;

                if (
                    edgeEndNode &&
                    !this.getEdgeBySourceTarget(
                        draggedEdge.source,
                        edgeEndNode[nodeKey]
                    ) &&
                    this.canSwap(sourceNode, edgeEndNode, draggedEdge)
                ) {
                    draggedEdgeCopy.target = edgeEndNode[nodeKey];
                    this.syncRenderEdge(draggedEdgeCopy);
                    this.props.onSwapEdge(sourceNodeById.node, edgeEndNode, draggedEdge);
                } else {
                    this.syncRenderEdge(draggedEdge);
                }
            }
        );
    };

    handleZoomToFit = () => {
        const entities = d3.select(this.entities).node();

        if (!entities) {
            return;
        }

        const viewBBox = entities.getBBox ? entities.getBBox() : null;

        if (!viewBBox) {
            return;
        }

        this.handleZoomToFitImpl(viewBBox, this.props.zoomDur);
    };

    handleZoomToFitImpl = (viewBBox: IBBox, zoomDur: number = 0) => {
        if (!this.viewWrapper.current) {
            return;
        }

        const parent = d3.select(this.viewWrapper.current).node();
        const width = parent.clientWidth;
        const height = parent.clientHeight;
        const minZoom = this.props.minZoom || 0;
        const maxZoom = this.props.maxZoom || 2;

        const next = {
            k: (minZoom + maxZoom) / 2,
            x: 0,
            y: 0,
        };

        if (viewBBox.width > 0 && viewBBox.height > 0) {
            const dx = viewBBox.width;
            const dy = viewBBox.height;
            const x = viewBBox.x + viewBBox.width / 2;
            const y = viewBBox.y + viewBBox.height / 2;

            next.k = 0.9 / Math.max(dx / width, dy / height);

            if (next.k < minZoom) {
                next.k = minZoom;
            } else if (next.k > maxZoom) {
                next.k = maxZoom;
            }

            next.x = width / 2 - next.k * x;
            next.y = height / 2 - next.k * y;
        }

        this.setZoom(next.k, next.x, next.y, zoomDur);
    };

    modifyZoom = (
        modK: number = 0,
        modX: number = 0,
        modY: number = 0,
        dur: number = 0
    ) => {
        const parent = d3.select(this.viewWrapper.current).node();
        const center = {
            x: parent.clientWidth / 2,
            y: parent.clientHeight / 2,
        };
        const extent = this.zoom.scaleExtent();
        const viewTransform: any = this.state.viewTransform;

        const next = {
            k: viewTransform.k,
            x: viewTransform.x,
            y: viewTransform.y,
        };

        const targetZoom = next.k * (1 + modK);

        next.k = targetZoom;

        if (targetZoom < extent[0] || targetZoom > extent[1]) {
            return false;
        }

        const translate0 = {
            x: (center.x - next.x) / next.k,
            y: (center.y - next.y) / next.k,
        };

        const l = {
            x: translate0.x * next.k + next.x,
            y: translate0.y * next.k + next.y,
        };

        next.x += center.x - l.x + modX;
        next.y += center.y - l.y + modY;
        this.setZoom(next.k, next.x, next.y, dur);

        return true;
    };

    setZoom(k: number = 1, x: number = 0, y: number = 0, dur: number = 0) {
        const t = d3.zoomIdentity.translate(x, y).scale(k);
        d3.select(this.viewWrapper.current)
            .select('svg')
            .transition()
            .duration(dur)
            .call(this.zoom.transform, t);
    }

    renderView() {
        this.selectedView.attr('transform', this.state.viewTransform);
        clearTimeout(this.renderNodesTimeout);
        this.renderNodesTimeout = setTimeout(this.renderNodes);
    }

    getNodeComponent = (id: string, node: INode) => {
        const {
            nodeTypes,
            nodeSubtypes,
            nodeSize,
            renderNode,
            renderNodeText,
            nodeKey,
            maxTitleChars,
        } = this.props;

        return (
            <Node
                key={id}
                id={id}
                data={node}
                nodeTypes={nodeTypes}
                nodeSize={nodeSize}
                nodeKey={nodeKey}
                nodeSubtypes={nodeSubtypes}
                onNodeMouseEnter={this.handleNodeMouseEnter}
                onNodeMouseLeave={this.handleNodeMouseLeave}
                onNodeMove={this.handleNodeMove}
                onNodeUpdate={this.handleNodeUpdate}
                onNodeSelected={this.handleNodeSelected}
                renderNode={renderNode}
                renderNodeText={renderNodeText}
                isSelected={this.state.selectedNodeObj.node === node}
                layoutEngine={this.layoutEngine}
                viewWrapperElem={this.viewWrapper.current}
                centerNodeOnMove={this.props.centerNodeOnMove}
                maxTitleChars={maxTitleChars}
            />
        );
    };

    renderNodes = () => {
        if (!this.entities) {
            return;
        }

        this.state.nodes.forEach((node, i) => {
            this.asyncRenderNode(node);
        });
    };

    asyncRenderNode(node: INode) {
        const nodeKey = this.props.nodeKey;
        const timeoutId = `nodes-${node[nodeKey]}`;

        cancelAnimationFrame(this.nodeTimeouts[timeoutId]);
        this.nodeTimeouts[timeoutId] = requestAnimationFrame(() => {
            this.syncRenderNode(node);
        });
    }

    syncRenderNode(node: INode) {
        const nodeKey = this.props.nodeKey;
        const id = `node-${node[nodeKey]}`;
        const element: any = this.getNodeComponent(id, node);
        const nodesMapNode = this.getNodeById(node[nodeKey]);

        this.renderNode(id, element);

        if (nodesMapNode) {
            this.renderConnectedEdgesFromNode(nodesMapNode);
        }
    }

    renderNode(id: string, element: Element) {
        if (!this.entities) {
            return null;
        }

        const containerId = `${id}-container`;
        let nodeContainer: HTMLElement | Element | null = document.getElementById(
            containerId
        );

        if (!nodeContainer) {
            nodeContainer = document.createElementNS(
                'http://www.w3.org/2000/svg',
                'g'
            );
            nodeContainer.id = containerId;
            this.entities.appendChild(nodeContainer);
        }
        const anyElement: any = element;
        ReactDOM.render(anyElement, nodeContainer);
    }

    renderConnectedEdgesFromNode(
        node: INodeMapNode,
        nodeMoving: boolean = false
    ) {
        if (this.state.draggingEdge) {
            return;
        }

        node.incomingEdges.forEach(edge => {
            this.asyncRenderEdge(edge, nodeMoving);
        });
        node.outgoingEdges.forEach(edge => {
            this.asyncRenderEdge(edge, nodeMoving);
        });
    }

    isEdgeSelected = (edge: IEdge) => {
        return (
            !!this.state.selectedEdgeObj &&
            !!this.state.selectedEdgeObj.edge &&
            this.state.selectedEdgeObj.edge.source === edge.source &&
            this.state.selectedEdgeObj.edge.target === edge.target
        );
    };

    getEdgeComponent = (edge: IEdge | any) => {
        const sourceNodeMapNode = this.getNodeById(edge.source);
        const sourceNode = sourceNodeMapNode ? sourceNodeMapNode.node : null;
        const targetNodeMapNode = this.getNodeById(edge.target);
        const targetNode = targetNodeMapNode ? targetNodeMapNode.node : null;
        const targetPosition = edge.targetPosition;
        const {edgeTypes, edgeHandleSize, nodeSize, nodeKey} = this.props;

        return (
            <Edge
                data={edge}
                edgeTypes={edgeTypes}
                edgeHandleSize={edgeHandleSize}
                nodeSize={nodeSize}
                sourceNode={sourceNode}
                targetNode={targetNode || targetPosition}
                nodeKey={nodeKey}
                viewWrapperElem={this.viewWrapper.current}
                isSelected={this.isEdgeSelected(edge)}
                rotateEdgeHandle={this.props.rotateEdgeHandle}
            />
        );
    };

    renderEdge = (
        id: string,
        element: any,
        edge: IEdge,
        nodeMoving: boolean = false
    ) => {
        if (!this.entities) {
            return null;
        }

        let containerId = `${id}-container`;
        const customContainerId = `${id}-custom-container`;
        const {draggedEdge} = this.state;
        const {afterRenderEdge} = this.props;
        let edgeContainer = document.getElementById(containerId);

        if (nodeMoving && edgeContainer) {
            edgeContainer.style.display = 'none';
            containerId = `${id}-custom-container`;
            edgeContainer = document.getElementById(containerId);
        } else if (edgeContainer) {
            const customContainer = document.getElementById(customContainerId);

            edgeContainer.style.display = '';

            if (customContainer) {
                customContainer.remove();
            }
        }

        if (!edgeContainer && edge !== draggedEdge) {
            const newSvgEdgeContainer = document.createElementNS(
                'http://www.w3.org/2000/svg',
                'g'
            );

            newSvgEdgeContainer.id = containerId;
            this.entities.appendChild(newSvgEdgeContainer);
            edgeContainer = newSvgEdgeContainer;
        }

        if (edgeContainer) {
            ReactDOM.render(element, edgeContainer);

            if (afterRenderEdge) {
                return afterRenderEdge(
                    id,
                    element,
                    edge,
                    edgeContainer,
                    this.isEdgeSelected(edge)
                );
            }
        }
    };

    asyncRenderEdge = (edge: IEdge, nodeMoving: boolean = false) => {
        if (!edge.source || !edge.target) {
            return;
        }

        const timeoutId = `edges-${edge.source}-${edge.target}`;

        cancelAnimationFrame(this.edgeTimeouts[timeoutId]);
        this.edgeTimeouts[timeoutId] = requestAnimationFrame(() => {
            this.syncRenderEdge(edge, nodeMoving);
        });
    };

    syncRenderEdge(edge: IEdge | any, nodeMoving: boolean = false) {
        if (!edge.source) {
            return;
        }

        const idVar = edge.target ? `${edge.source}-${edge.target}` : 'custom';
        const id = `edge-${idVar}`;
        const element = this.getEdgeComponent(edge);

        this.renderEdge(id, element, edge, nodeMoving);
    }

    renderEdges = () => {
        const {edges, draggingEdge} = this.state;

        if (!this.entities || draggingEdge) {
            return;
        }

        for (let i = 0; i < edges.length; i++) {
            this.asyncRenderEdge(edges[i]);
        }
    };

    renderGraphControls() {
        const {showGraphControls, minZoom, maxZoom} = this.props;
        const {viewTransform} = this.state;

        if (!showGraphControls || !this.viewWrapper) {
            return;
        }

        const graphControlsWrapper = this.viewWrapper.current.ownerDocument.getElementById(
            'react-digraph-graph-controls-wrapper'
        );

        if (!graphControlsWrapper) {
            return;
        }

        ReactDOM.render(
            <WorkflowControls
                ref={this.graphControls}
                minZoom={minZoom}
                maxZoom={maxZoom}
                zoomLevel={viewTransform ? viewTransform.k : 1}
                zoomToFit={this.handleZoomToFit}
                modifyZoom={this.modifyZoom}
            />,
            graphControlsWrapper
        );
    }

    render() {
        const {
            edgeArrowSize,
            gridSpacing,
            gridDotSize,
            nodeTypes,
            nodeSubtypes,
            edgeTypes,
            renderDefs,
            gridSize,
            backgroundFillId,
            renderBackground,
        } = this.props;

        return (
            <div className="view-wrapper" ref={this.viewWrapper}>
                <svg className="graph" ref={this.graphSvg}>
                    <Definitions
                        edgeArrowSize={edgeArrowSize}
                        gridSpacing={gridSpacing}
                        gridDotSize={gridDotSize}
                        nodeTypes={nodeTypes}
                        nodeSubtypes={nodeSubtypes}
                        edgeTypes={edgeTypes}
                        renderDefs={renderDefs}
                    />
                    <g className="view" ref={el => (this.view = el)}>
                        <Background
                            gridSize={gridSize}
                            backgroundFillId={backgroundFillId}
                            renderBackground={renderBackground}
                        />

                        <g className="entities" ref={el => (this.entities = el)}/>
                    </g>
                </svg>
                <div
                    id="react-digraph-graph-controls-wrapper"
                    className="graph-controls-wrapper"
                />
            </div>
        );
    }

    panToEntity(entity: IEdge | INode, zoom: boolean) {
        const {viewTransform} = this.state;
        const parent = this.viewWrapper.current;
        const entityBBox = entity ? entity.getBBox() : null;
        const maxZoom = this.props.maxZoom || 2;

        if (!parent || !entityBBox) {
            return;
        }

        const width = parent.clientWidth;
        const height = parent.clientHeight;

        const next = {
            k: viewTransform.k,
            x: 0,
            y: 0,
        };

        const x = entityBBox.x + entityBBox.width / 2;
        const y = entityBBox.y + entityBBox.height / 2;

        if (zoom) {
            next.k =
                0.9 / Math.max(entityBBox.width / width, entityBBox.height / height);

            if (next.k > maxZoom) {
                next.k = maxZoom;
            }
        }

        next.x = width / 2 - next.k * x;
        next.y = height / 2 - next.k * y;

        this.setZoom(next.k, next.x, next.y, this.props.zoomDur);
    }

    panToNode(id: string, zoom: boolean = false) {
        if (!this.entities) {
            return;
        }

        const node = this.entities.querySelector(`#node-${id}-container`);

        this.panToEntity(node, zoom);
    }

    panToEdge(source: string, target: string, zoom: boolean = false) {
        if (!this.entities) {
            return;
        }

        const edge = this.entities.querySelector(
            `#edge-${source}-${target}-container`
        );

        this.panToEntity(edge, zoom);
    }
}

export default WorkflowView;
