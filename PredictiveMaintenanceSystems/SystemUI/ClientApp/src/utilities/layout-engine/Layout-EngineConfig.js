//@flow

import NoneNode from './NoneNode';
import SnapToGrid from './SnapToGrid';
import VerticalTree from './Vertical-Tree';
import HorizontalTree from './Horizontal-Tree';

export type LayoutEngine = NoneNode | SnapToGrid | VerticalTree | HorizontalTree;

//Optional settings
export const LayoutEngines = {
    None: NoneNode,
    SnapToGrid,
    VerticalTree,
    HorizontalTree,
};
