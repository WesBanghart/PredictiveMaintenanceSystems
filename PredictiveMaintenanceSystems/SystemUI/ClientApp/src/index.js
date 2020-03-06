//@flow

import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import ReactDOM from 'react-dom';
import {BrowserRouter} from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';
import GV from './components/Workflow-View';
import { type LayoutEngine as LayoutEngineConfigTypes } from './utilities/layout-engine/Layout-EngineConfig';
import type { IEdge } from './components/Edge';
import type { INode } from './components/Node';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

export { default as GraphViewFast } from './components/Workflow-View';
export { default as Edge } from './components/Edge';
export type IEdgeType = IEdge;
export { default as GraphUtils } from './utilities/Workflow-Utils';
export { default as Node } from './components/Node';
export type INodeType = INode;
export { default as BwdlTransformer } from './utilities/transformers/Bwdl-Transformers';
export { GV as GraphView };
export type LayoutEngineType = LayoutEngineConfigTypes;
export default GV;

ReactDOM.render(
    <BrowserRouter basename={baseUrl}>
        <App/>
    </BrowserRouter>,
    rootElement);

registerServiceWorker();

