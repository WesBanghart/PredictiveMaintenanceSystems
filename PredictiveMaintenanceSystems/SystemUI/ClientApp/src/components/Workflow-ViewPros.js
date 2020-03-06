// @flow

import {type LayoutEngineType} from '../utilities/layout-engine/Layout-EngineTypes';
import {type IEdge} from './Edge';
import {type INode} from './Node';

export type IBBox = {
    x: number,
    y: number,
    width: number,
    height: number,
};

export type IGraphViewProps = {
    backgroundFillId?: string,
    edges: any[],
    edgeArrowSize?: number,
    edgeHandleSize?: number,
    edgeTypes: any,
    gridDotSize?: number,
    gridSize?: number,
    gridSpacing?: number,
    layoutEngineType?: LayoutEngineType,
    maxTitleChars?: number,
    maxZoom?: number,
    minZoom?: number,
    nodeKey: string,
    nodes: any[],
    nodeSize?: number,
    nodeHeight?: number,
    nodeWidth?: number,
    nodeSpacingMultiplier?: number,
    nodeSubtypes: any,
    nodeTypes: any,
    readOnly?: boolean,
    selected: any,
    showGraphControls?: boolean,
    zoomDelay?: number,
    zoomDur?: number,
    canCreateEdge?: (startNode?: INode, endNode?: INode) => boolean,
    canDeleteEdge?: (selected: any) => boolean,
    canDeleteNode?: (selected: any) => boolean,
    onBackgroundClick?: (x: number, y: number, event: any) => void,
    onCopySelected?: () => void,
    onCreateEdge: (sourceNode: INode, targetNode: INode) => void,
    onCreateNode: (x: number, y: number, event: any) => void,
    onDeleteEdge: (selectedEdge: IEdge, edges: IEdge[]) => void,
    onDeleteNode: (selected: any, nodeId: string, nodes: any[]) => void,
    onPasteSelected?: () => void,
    onSelectEdge: (selectedEdge: IEdge) => void,
    onSelectNode: (node: INode | null, event: any) => void,
    onSwapEdge: (sourceNode: INode, targetNode: INode, edge: IEdge) => void,
    onUndo?: () => void,
    onUpdateNode: (node: INode) => void,
    renderBackground?: (gridSize?: number) => any,
    renderDefs?: () => any,
    renderNode?: (
        nodeRef: any,
        data: any,
        id: string,
        selected: boolean,
        hovered: boolean
    ) => any,
    afterRenderEdge?: (
        id: string,
        element: any,
        edge: IEdge,
        edgeContainer: any,
        isEdgeSelected: boolean
    ) => void,
    renderNodeText?: (data: any, id: string | number, isSelected: boolean) => any,
    rotateEdgeHandle?: boolean,
    centerNodeOnMove?: boolean,
    initialBBox: IBBox,
};
