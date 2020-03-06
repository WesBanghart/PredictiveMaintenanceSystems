// @flow

import * as dagre from 'dagre';
import {type INode} from '../../components/Node';
import SnapToGrid from './SnapToGrid';

class VerticalTree extends SnapToGrid {
    adjustNodes(nodes: INode[], nodesMap?: any): INode[] {
        const {
            nodeKey,
            nodeSize,
            nodeHeight,
            nodeWidth,
            nodeSpacingMultiplier,
        } = this.graphViewProps;
        const g = new dagre.graphlib.Graph();

        g.setGraph({});
        g.setDefaultEdgeLabel(() => ({}));

        const spacing = nodeSpacingMultiplier || 1.5;
        const size = (nodeSize || 1) * spacing;
        let height;
        let width;

        if (nodeHeight) {
            height = nodeHeight * spacing;
        }

        if (nodeWidth) {
            width = nodeWidth * spacing;
        }

        nodes.forEach(node => {
            if (!nodesMap) {
                return;
            }

            const nodeId = node[nodeKey];
            const nodeKeyId = `key-${nodeId}`;
            const nodesMapNode = nodesMap[nodeKeyId];

            if (
                nodesMapNode.incomingEdges.length === 0 &&
                nodesMapNode.outgoingEdges.length === 0
            ) {
                return;
            }

            g.setNode(nodeKeyId, {width: width || size, height: height || size});
            nodesMapNode.outgoingEdges.forEach(edge => {
                g.setEdge(nodeKeyId, `key-${edge.target}`);
            });
        });

        dagre.layout(g);

        g.nodes().forEach(gNodeId => {
            const nodesMapNode = nodesMap[gNodeId];

            const gNode = g.node(gNodeId);

            nodesMapNode.node.x = gNode.x;
            nodesMapNode.node.y = gNode.y;
        });

        return nodes;
    }
}

export default VerticalTree;
