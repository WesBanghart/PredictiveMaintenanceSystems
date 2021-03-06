// @flow

import {type IGraphViewProps} from '../../components/Workflow-ViewPros';
import {type INode} from '../../components/Node';

export type IPosition = {
    x: number,
    y: number,
    [key: string]: any,
};

//Converts the nodes into a tree view
export default class LayoutEngine {
    graphViewProps: IGraphViewProps;

    constructor(graphViewProps: IGraphViewProps) {
        this.graphViewProps = graphViewProps;
    }

    calculatePosition(node: IPosition) {
        return node;
    }

    adjustNodes(nodes: INode[], nodesMap?: any): INode[] {
        let node = null;

        for (let i = 0; i < nodes.length; i++) {
            node = nodes[i];
            const position = this.calculatePosition({
                x: node.x || 0,
                y: node.y || 0,
            });

            node.x = position.x;
            node.y = position.y;
        }

        return nodes;
    }

    getPositionForNode(node: IPosition): IPosition {
        return this.calculatePosition(node);
    }
}
