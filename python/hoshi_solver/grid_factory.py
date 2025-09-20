from typing import List

from cell import Cell
from grid import Grid
from line import Line

type Cells = List[List[Cell]]


class GridFactory:
    def __init__(self, stars: int, rows: List[List[str]]) -> None:
        size = len(rows)
        if any(len(row) != size for row in rows):
            raise ValueError('The grid must be square.')
        self.size = size
        self.stars = stars
        self._input = rows

    def create_grid(self) -> Grid:
        cells = self._create_cells()
        self._create_lines(cells)
        self._create_vicinities(cells)
        return cells

    def _create_cells(self) -> Cells:
        return [[Cell(x, y) for x in range(self.size)] for y in range(self.size)]

    def _create_lines(self, cells: Cells) -> None:
        for z in range(self.size):
            Line(self.stars, list(cells[z]))
            Line(self.stars, [row[z] for row in cells])

    def _create_vicinities(self, cells: Cells) -> None:
        for y in range(self.size):
            for x in range(self.size):
                for 