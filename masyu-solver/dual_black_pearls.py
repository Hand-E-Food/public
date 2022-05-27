from typing import TYPE_CHECKING

import view
from black_pearl import BlackPearl

if TYPE_CHECKING:
    from map import Map


class DualBlackPearls:
    def detect(map: 'Map', x: int, y: int):
        if not isinstance(map.get(x, y), BlackPearl): return

        if isinstance(map.get(x, y - 2), BlackPearl):
            with view.indent(f'{x:02},{y - 1:02} dual black perals initialising'):
                map.get(x, y - 1).block()

        if isinstance(map.get(x - 2, y), BlackPearl):
            with view.indent(f'{x - 1:02},{y:02} dual black perals initialising'):
                map.get(x - 1, y).block()
