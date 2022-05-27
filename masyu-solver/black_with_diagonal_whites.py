from typing import TYPE_CHECKING

import view
from black_pearl import BlackPearl
from white_pearl import WhitePearl

if TYPE_CHECKING:
    from map import Map

class BlackWithDiagonalWhites:
    @staticmethod
    def detect(map: 'Map', x: int, y: int):
        if not isinstance(map.get(x, y), BlackPearl): return

        diagonals = []
        for dx, dy in ((-2, -2), (+2, -2), (+2, +2), (-2, +2)):
            diagonal = map.get(x + dx, y + dy)
            if isinstance(diagonal, WhitePearl):
                diagonals.append(diagonal)
            else:
                diagonals.append(None)

        count = sum(diagonal is not None for diagonal in diagonals)
        if count >= 2:
            with view.indent(f'{x:02},{y:02} black with diagonal whites initialising'):
                if diagonals[0] and diagonals[1]: map.get(x, y - 1).block()
                if diagonals[1] and diagonals[2]: map.get(x + 1, y).block()
                if diagonals[2] and diagonals[3]: map.get(x, y + 1).block()
                if diagonals[3] and diagonals[0]: map.get(x - 1, y).block()
