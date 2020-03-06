// @flow

import * as dagre from 'dagre';
import {type INode} from '../../components/Node';
import SnapToGrid from './SnapToGrid';

class HorizontalTree extends SnapToGrid {
    adjustNodes(nodes: INode[], nodesMap?: any): INode[] {
        const {
            nodeKey,
            nodeSize,
            nodeHeight,
            nodeWidth,
            nodeSizeOverridesAllowed,
            nodeSpacingMultiplier,
            nodeLocationOverrides,
            graphConfig,
        } = this.graphViewProps;
        const spacing = nodeSpacingMultiplier || 1.5;
        const size = (nodeSize || 1) * spacing;
        const g = new dagre.graphlib.Graph();
        const height = nodeHeight ? nodeHeight * spacing : size;
        const width = nodeWidth ? nodeWidth * spacing : size;

        g.setGraph(
            Object.assign(
                {
                    rankdir: 'LR',
                },
                graphConfig
            )
        );
        g.setDefaultEdgeLabel(() => ({}));

        nodes.forEach(node => {
            if (!nodesMap) {
                return;
            }

            const {
                sizeOverrides: {width: widthOverride, height: heightOverride} = {},
            } = node;

            const nodeId = node[nodeKey];
            const nodeKeyId = `key-${nodeId}`;
            const nodesMapNode = nodesMap[nodeKeyId];
            if (
                nodesMapNode.incomingEdges.length === 0 &&
                nodesMapNode.outgoingEdges.length === 0
            ) {
                return;
            }

            g.setNode(nodeKeyId, {
                width:
                    nodeSizeOverridesAllowed && widthOverride ? widthOverride : width,
                height:
                    nodeSizeOverridesAllowed && heightOverride ? heightOverride : height,
            });
            nodesMapNode.outgoingEdges.forEach(edge => {
                g.setEdge(nodeKeyId, `key-${edge.target}`);
            });
        });

        dagre.layout(g);

        if (nodeLocationOverrides) {
            for (const gNodeId in nodeLocationOverrides) {
                if (nodeLocationOverrides.hasOwnProperty(gNodeId)) {
                    const nodeKeyId = `key-${gNodeId}`;
                    const gNode = g.node(nodeKeyId);
                    const locationOverride = nodeLocationOverrides[gNodeId];

                    if (gNode && locationOverride) {
                        gNode.x = locationOverride.x;
                        gNode.y = locationOverride.y;
                    }
                }
            }
        }

        g.nodes().forEach(gNodeId => {
            const nodesMapNode = nodesMap[gNodeId];
            const gNode = g.node(gNodeId);
            nodesMapNode.node.x = gNode.x;
            nodesMapNode.node.y = gNode.y;
        });

        return nodes;
    }
}

export default HorizontalTree;
