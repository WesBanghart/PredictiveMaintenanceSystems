// @flow

import { type IEdge } from '../../components/Edge';
import { type INode } from '../../components/Node';
import Transformer, { type IGraphInput } from './Transformers';

//Change for the nodes are shown
export default class BwdlTransformer extends Transformer {
    //Transform to new state
    static transform(input: any) {
        if (!input.States) {
            return {
                edges: [],
                nodes: [],
            };
        }

        const nodeNames = Object.keys(input.States);

        const nodes: INode[] = [];
        const edges: IEdge[] = [];

        nodeNames.forEach(name => {
            const currentNode = input.States[name];

            if (!currentNode) {
                return;
            }

            const nodeToAdd: INode = {
                title: name,
                type: currentNode.Type,
                x: currentNode.x || 0,
                y: currentNode.y || 0,
            };

            if (name === input.StartAt) {
                nodes.unshift(nodeToAdd);
            } else {
                nodes.push(nodeToAdd);
            }

            // create edges
            if (currentNode.Type === 'Choice') {
                // multiple edges
                currentNode.Choices.forEach(choice => {
                    if (input.States[choice.Next]) {
                        edges.push({
                            source: name,
                            target: choice.Next,
                        });
                    }
                });
                if (currentNode.Default) {
                    edges.push({
                        source: name,
                        target: currentNode.Default,
                    });
                }
            } else if (currentNode.Next) {
                if (input.States[currentNode.Next]) {
                    // single edge
                    edges.push({
                        source: name,
                        target: currentNode.Next,
                    });
                }
            }
        });

        return {
            edges,
            nodes,
        };
    }

    static revert(graphInput: IGraphInput) {
        return graphInput;
    }
}
