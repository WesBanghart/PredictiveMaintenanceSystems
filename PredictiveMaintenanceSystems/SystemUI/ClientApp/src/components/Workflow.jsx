// @flow

import * as React from 'react';
import {GraphView, type IEdgeType as IEdge, type INodeType as INode, type LayoutEngineType,} from '../';
import GraphConfig, {
    COMPLEX_CIRCLE_TYPE,
    edgeTypes,
    EMPTY_EDGE_TYPE,
    EMPTY_TYPE,
    NODE_KEY,
    nodeTypes,
    POLY_TYPE,
    SKINNY_TYPE,
    SPECIAL_CHILD_SUBTYPE,
    SPECIAL_EDGE_TYPE,
    SPECIAL_TYPE,
} from './Workflow-Config';

type IGraph = {
    nodes: INode[],
    edges: IEdge[],
};

const sample: IGraph = {
    edges: [
        {
            handleText: '5',
            handleTooltipText: '5',
            source: 'start1',
            target: 'a1',
            type: SPECIAL_EDGE_TYPE,
        },
        {
            handleText: '5',
            handleTooltipText: 'This edge connects Node A and Node B',
            source: 'a1',
            target: 'a2',
            type: SPECIAL_EDGE_TYPE,
        },
        {
            handleText: '54',
            source: 'a2',
            target: 'a4',
            type: EMPTY_EDGE_TYPE,
        },
        {
            handleText: '54',
            source: 'a1',
            target: 'a3',
            type: EMPTY_EDGE_TYPE,
        },
        {
            handleText: '54',
            source: 'a3',
            target: 'a4',
            type: EMPTY_EDGE_TYPE,
        },
        {
            handleText: '54',
            source: 'a1',
            target: 'a5',
            type: EMPTY_EDGE_TYPE,
        },
        {
            handleText: '54',
            source: 'a4',
            target: 'a1',
            type: EMPTY_EDGE_TYPE,
        },
        {
            handleText: '54',
            source: 'a1',
            target: 'a6',
            type: EMPTY_EDGE_TYPE,
        },
        {
            handleText: '24',
            source: 'a1',
            target: 'a7',
            type: EMPTY_EDGE_TYPE,
        },
    ],
    nodes: [
        {
            id: 'start1',
            title: 'Start (0)',
            type: SPECIAL_TYPE,
        },
        {
            id: 'a1',
            title: 'Node A (1)',
            type: SPECIAL_TYPE,
            x: 258.3976135253906,
            y: 331.9783248901367,
        },
        {
            id: 'a2',
            subtype: SPECIAL_CHILD_SUBTYPE,
            title: 'Node B (2)',
            type: EMPTY_TYPE,
            x: 593.9393920898438,
            y: 260.6060791015625,
        },
        {
            id: 'a3',
            title: 'Node C (3)',
            type: EMPTY_TYPE,
            x: 237.5757598876953,
            y: 61.81818389892578,
        },
        {
            id: 'a4',
            title: 'Node D (4)',
            type: EMPTY_TYPE,
            x: 600.5757598876953,
            y: 600.81818389892578,
        },
        {
            id: 'a5',
            title: 'Node E (5)',
            type: null,
            x: 50.5757598876953,
            y: 500.81818389892578,
        },
        {
            id: 'a6',
            title: 'Node E (6)',
            type: SKINNY_TYPE,
            x: 300,
            y: 600,
        },
        {
            id: 'a7',
            title: 'Node F (7)',
            type: POLY_TYPE,
            x: 0,
            y: 300,
        },
        {
            id: 'a8',
            title: 'Node G (8)',
            type: COMPLEX_CIRCLE_TYPE,
            x: -200,
            y: 400,
        },
    ],
};

function generateSample(totalNodes) {
    const generatedSample: IGraph = {
        edges: [],
        nodes: [],
    };
    let y = 0;
    let x = 0;

    const numNodes = totalNodes ? totalNodes : 0;

    for (let i = 1; i <= numNodes; i++) {
        if (i % 20 === 0) {
            y++;
            x = 0;
        } else {
            x++;
        }

        generatedSample.nodes.push({
            id: `a${i}`,
            title: `Node ${i}`,
            type: nodeTypes[Math.floor(nodeTypes.length * Math.random())],
            x: 0 + 200 * x,
            y: 0 + 200 * y,
        });
    }

    for (let i = 1; i < numNodes; i++) {
        generatedSample.edges.push({
            source: `a${i}`,
            target: `a${i + 1}`,
            type: edgeTypes[Math.floor(edgeTypes.length * Math.random())],
        });
    }

    return generatedSample;
}

type IGraphProps = {};

type IGraphState = {
    graph: any,
    selected: any,
    totalNodes: number,
    copiedNode: any,
    layoutEngineType?: LayoutEngineType,
};

class Workflow extends React.Component<IGraphProps, IGraphState> {
    GraphView;

    constructor(props: IGraphProps) {
        super(props);

        this.state = {
            copiedNode: null,
            graph: sample,
            layoutEngineType: undefined,
            selected: null,
            totalNodes: sample.nodes.length,
        };

        this.GraphView = React.createRef();
    }

    getNodeIndex(searchNode: INode | any) {
        return this.state.graph.nodes.findIndex(node => {
            return node[NODE_KEY] === searchNode[NODE_KEY];
        });
    }

    getEdgeIndex(searchEdge: IEdge) {
        return this.state.graph.edges.findIndex(edge => {
            return (
                edge.source === searchEdge.source && edge.target === searchEdge.target
            );
        });
    }

    getViewNode(nodeKey: string) {
        const searchNode = {};

        searchNode[NODE_KEY] = nodeKey;
        const i = this.getNodeIndex(searchNode);

        return this.state.graph.nodes[i];
    }

    makeItLarge = () => {
        const graph = this.state.graph;
        const generatedSample = generateSample(this.state.totalNodes);

        graph.nodes = generatedSample.nodes;
        graph.edges = generatedSample.edges;
        this.setState(this.state);
    };

    addStartNode = () => {
        const graph = this.state.graph;

        graph.nodes = [
            {
                id: Date.now(),
                title: 'Node A',
                type: SPECIAL_TYPE,
                x: 0,
                y: 0,
            },
            ...this.state.graph.nodes,
        ];
        this.setState({
            graph,
        });
    };
    deleteStartNode = () => {
        const graph = this.state.graph;

        graph.nodes.splice(0, 1);
        graph.nodes = [...this.state.graph.nodes];
        this.setState({
            graph,
        });
    };

    handleChange = (event: any) => {
        this.setState(
            {
                totalNodes: parseInt(event.target.value || '0', 10),
            },
            this.makeItLarge
        );
    };

    onUpdateNode = (viewNode: INode) => {
        const graph = this.state.graph;
        const i = this.getNodeIndex(viewNode);

        graph.nodes[i] = viewNode;
        this.setState({graph});
    };

    onSelectNode = (viewNode: INode | null) => {
        this.setState({selected: viewNode});
    };

    onSelectEdge = (viewEdge: IEdge) => {
        this.setState({selected: viewEdge});
    };

    onCreateNode = (x: number, y: number) => {
        const graph = this.state.graph;
        const type = Math.random() < 0.25 ? SPECIAL_TYPE : EMPTY_TYPE;
        const viewNode = {
            id: Date.now(),
            title: '',
            type,
            x,
            y,
        };

        graph.nodes = [...graph.nodes, viewNode];
        this.setState({graph});
    };

    onDeleteNode = (viewNode: INode, nodeId: string, nodeArr: INode[]) => {
        const graph = this.state.graph;
        const newEdges = graph.edges.filter((edge, i) => {
            return (
                edge.source !== viewNode[NODE_KEY] && edge.target !== viewNode[NODE_KEY]
            );
        });

        graph.nodes = nodeArr;
        graph.edges = newEdges;

        this.setState({graph, selected: null});
    };

    onCreateEdge = (sourceViewNode: INode, targetViewNode: INode) => {
        const graph = this.state.graph;
        const type =
            sourceViewNode.type === SPECIAL_TYPE
                ? SPECIAL_EDGE_TYPE
                : EMPTY_EDGE_TYPE;

        const viewEdge = {
            source: sourceViewNode[NODE_KEY],
            target: targetViewNode[NODE_KEY],
            type,
        };

        if (viewEdge.source !== viewEdge.target) {
            graph.edges = [...graph.edges, viewEdge];
            this.setState({
                graph,
                selected: viewEdge,
            });
        }
    };

    onSwapEdge = (
        sourceViewNode: INode,
        targetViewNode: INode,
        viewEdge: IEdge
    ) => {
        const graph = this.state.graph;
        const i = this.getEdgeIndex(viewEdge);
        const edge = JSON.parse(JSON.stringify(graph.edges[i]));

        edge.source = sourceViewNode[NODE_KEY];
        edge.target = targetViewNode[NODE_KEY];
        graph.edges[i] = edge;
        graph.edges = [...graph.edges];

        this.setState({
            graph,
            selected: edge,
        });
    };

    onDeleteEdge = (viewEdge: IEdge, edges: IEdge[]) => {
        const graph = this.state.graph;

        graph.edges = edges;
        this.setState({
            graph,
            selected: null,
        });
    };

    onUndo = () => {
        console.warn('Undo not implemented yet.')
    };

    onCopySelected = () => {
        if (this.state.selected.source) {
            console.warn('Cannot copy selected edges, try selecting a node instead.');

            return;
        }

        const x = this.state.selected.x + 10;
        const y = this.state.selected.y + 10;

        this.setState({
            copiedNode: {...this.state.selected, x, y},
        });
    };

    onPasteSelected = () => {
        if (!this.state.copiedNode) {
            console.warn(
                'No node is currently in the copy queue. Try selecting a node and copying it with Ctrl/Command-C'
            );
        }

        const graph = this.state.graph;
        const newNode = {...this.state.copiedNode, id: Date.now()};

        graph.nodes = [...graph.nodes, newNode];
        this.forceUpdate();
    };

    handleChangeLayoutEngineType = (event: any) => {
        this.setState({
            layoutEngineType: (event.target.value: LayoutEngineType | 'None'),
        });
    };

    onSelectPanNode = (event: any) => {
        if (this.GraphView) {
            this.GraphView.panToNode(event.target.value, true);
        }
    };

    render() {
        const {nodes, edges} = this.state.graph;
        const selected = this.state.selected;
        const {NodeTypes, NodeSubtypes, EdgeTypes} = GraphConfig;

        return (
            <div id="graph">
                <div className="graph-header">
                    <button onClick={this.addStartNode}>Add Node</button>
                    <button onClick={this.deleteStartNode}>Delete Node</button>
                    <input
                        className="total-nodes"
                        type="number"
                        onBlur={this.handleChange}
                        placeholder={this.state.totalNodes.toString()}
                    />
                    <div className="layout-engine">
                        <span>Layout Engine:</span>
                        <select
                            name="layout-engine-type"
                            onChange={this.handleChangeLayoutEngineType}
                        >
                            <option value={undefined}>None</option>
                            <option value={'SnapToGrid'}>Snap to Grid</option>
                            <option value={'VerticalTree'}>Vertical Tree</option>
                        </select>
                    </div>
                    <div className="pan-list">
                        <span>Pan To:</span>
                        <select onChange={this.onSelectPanNode}>
                            {nodes.map(node => (
                                <option key={node[NODE_KEY]} value={node[NODE_KEY]}>
                                    {node.title}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>
                <GraphView
                    ref={el => (this.GraphView = el)}
                    nodeKey={NODE_KEY}
                    nodes={nodes}
                    edges={edges}
                    selected={selected}
                    nodeTypes={NodeTypes}
                    nodeSubtypes={NodeSubtypes}
                    edgeTypes={EdgeTypes}
                    onSelectNode={this.onSelectNode}
                    onCreateNode={this.onCreateNode}
                    onUpdateNode={this.onUpdateNode}
                    onDeleteNode={this.onDeleteNode}
                    onSelectEdge={this.onSelectEdge}
                    onCreateEdge={this.onCreateEdge}
                    onSwapEdge={this.onSwapEdge}
                    onDeleteEdge={this.onDeleteEdge}
                    onUndo={this.onUndo}
                    onCopySelected={this.onCopySelected}
                    onPasteSelected={this.onPasteSelected}
                    layoutEngineType={this.state.layoutEngineType}
                />
            </div>
        );
    }
}

export default Workflow;
