from typing import Dict, Iterable, List

from model.cell import Cell
from model.grid import Grid
from model.group import Group

type Cells = List[List[Cell]]


class GridFactory:
    def __init__(self, stars: int, rows: List[List[str]]) -> None:
        size = len(rows)
        if any(len(row) != size for row in rows):
            raise ValueError('The grid must be square.')
        self.size = size
        self.stars = stars
        self._input = rows
        self.grid = self._create_grid()

    def _create_grid(self) -> Grid:
        grid = Grid()
        grid.cells = self._create_cells()
        grid.quads = list(self._create_adjacencies(grid))
        grid.columns = list(self._create_columns(grid))
        grid.rows = list(self._create_rows(grid))
        grid.areas = list(self._create_areas(grid))
        return grid

    def _create_cells(self) -> Cells:
        return [[Cell(x, y) for x in range(self.size)] for y in range(self.size)]

    def _create_adjacencies(self, grid: Grid) -> Iterable[Group]:
        cells = grid.cells
        for cY in range(self.size - 1):
            for cX in range(self.size - 1):
                group = Group(grid, f'{cX},{cY}', (0, 1), [
                    cells[cY + 0][cX + 0],
                    cells[cY + 0][cX + 1],
                    cells[cY + 1][cX + 0],
                    cells[cY + 1][cX + 1],
                ])
                cells[cY][cX].groups.add(group)
                for target_cell in group.cells:
                    for adjacent_cell in group.cells:
                        if target_cell is not adjacent_cell:
                            target_cell.adjacent_cells.add(adjacent_cell)
                yield group

    def _create_columns(self, grid: Grid) -> Iterable[Group]:
        cells = grid.cells
        for z in range(self.size):
            yield Group(grid, f'col{z:02d}', self.stars, list(cells[z]))

    def _create_rows(self, grid: Grid) -> Iterable[Group]:
        cells = grid.cells
        for z in range(self.size):
            yield Group(grid, f'row{z:02d}', self.stars, [row[z] for row in cells])

    def _create_areas(self, grid: Grid) -> Iterable[Group]:
        cells = grid.cells
        areas: Dict[str, List[Cell]] = {}
        for y in range(self.size):
            for x in range(self.size):
                id = self._input[y][x]
                if id not in areas:
                    areas[id] = []
                areas[id].append(cells[y][x])
        for id, area_cells in areas.items():
            yield Group(grid, id, self.stars, area_cells)
