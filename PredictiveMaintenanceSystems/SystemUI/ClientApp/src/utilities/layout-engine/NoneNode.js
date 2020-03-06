// @flow

import LayoutEngine, { type IPosition } from './Layout-Engine';

class NoneNode extends LayoutEngine {
  calculatePosition(node: IPosition) {
    return node;
  }
}

export default NoneNode;
