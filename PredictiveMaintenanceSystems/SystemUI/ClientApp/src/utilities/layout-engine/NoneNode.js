// @flow

import LayoutEngine, { type IPosition } from './Layout-Engine';

//There is no node that exists for the current position
class NoneNode extends LayoutEngine {
  calculatePosition(node: IPosition) {
    return node;
  }
}

export default NoneNode;
