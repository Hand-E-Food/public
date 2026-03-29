from typing import Iterable, Tuple

from .cell import Cell
from .grid import Grid
from .state import State


class Group:
    def __init__(self, grid: Grid, name: str, stars: int | Tuple[int, int], cells: Iterable[Cell]):
        self.dirty = True
        self.grid = grid
        self.name = name
        self.min_stars = stars if isinstance(stars, int) else stars[0]
        self.max_stars = stars if isinstance(stars, int) else stars[1]
        self.cells = cells if isinstance(cells, set) else set(cells)
        for cell in cells:
            cell.add_group(self)

    def remove_cell(self, cell: Cell) -> None:
        if cell not in self.cells:
            return
        self.cells.remove(cell)
        if cell.state == State.STAR:
            self.max_stars -= 1
            self.min_stars = max(0, self.min_stars - 1)
        self.dirty = True
        self._update_cells()

    def _update_cells(self) -> None:
        if len(self.cells) == 0:
            self.grid.remove_group(self)
        elif self.max_stars == 0:
            for cell in self.cells:
                cell.become(State.EMPTY)
        elif len(self.cells) == self.min_stars:
            for cell in self.cells:
                cell.become(State.STAR)
