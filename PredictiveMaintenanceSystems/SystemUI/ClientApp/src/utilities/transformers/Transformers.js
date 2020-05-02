//@flow

import {type IEdge} from '../../components/Edge';
import {type INode} from '../../components/Node';

export type IGraphInput = {
    nodes: INode[],
    edges: IEdge[],
};

//Holds the transformations of the graph properties
export default class Transformers {
    static transform(input: any): IGraphInput {
        return {
            edges: [],
            nodes: [],
        };
    }

    static revert(graphInput: IGraphInput): any {
        return graphInput;
    }
}
