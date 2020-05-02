// @flow

import * as React from 'react';
import GraphUtils from '../utilities/Workflow-Utils';
import {type INode} from './Node';

type INodeTextProps = {
    data: INode,
    nodeTypes: any,
    isSelected: boolean,
    maxTitleChars: number,
};

//Allows the nodes to have text
class NodeText extends React.Component<INodeTextProps> {
    getTypeText(data: INode, nodeTypes: any) {
        if (data.type && nodeTypes[data.type]) {
            return nodeTypes[data.type].typeText;
        } else if (nodeTypes.emptyNode) {
            return nodeTypes.emptyNode.typeText;
        } else {
            return null;
        }
    }

    //Render the node with relevant text
    render() {
        const {data, nodeTypes, isSelected, maxTitleChars} = this.props;
        const lineOffset = 18;
        const title = data.title;
        const className = GraphUtils.classNames('node-text', {
            selected: isSelected,
        });
        const typeText = this.getTypeText(data, nodeTypes);

        return (
            <text className={className} textAnchor="middle">
                {!!typeText && <tspan opacity="0.5">{typeText}</tspan>}
                {title && (
                    <tspan x={0} dy={lineOffset} fontSize="10px">
                        {title.length > maxTitleChars
                            ? title.substr(0, maxTitleChars)
                            : title}
                    </tspan>
                )}
                {title && <title>{title}</title>}
            </text>
        );
    }
}

export default NodeText;
