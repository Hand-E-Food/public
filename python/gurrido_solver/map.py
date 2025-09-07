from typing import List
from cell import Cell
from raw_cell import RawCell
from row import Row
from soil_function import SoilFunction


class Map:
    def __init__(self, soil_function: SoilFunction, cells: List[RawCell]):
        if len(cells) != 81:
            raise ValueError('A map must have 81 cells.')
        self.cells = [Cell(soil_function, f's{x}{y}', cells[y*9+x]) for y in range(9) for x in range(9)]
        self.rows = [
            *(Row(soil_function, f'r{y}', [self.cell(x, y) for x in range(9)]) for y in range(9)),
            *(Row(soil_function, f'c{x}', [self.cell(x, y) for y in range(9)]) for x in range(9)),
        ]
        self.runs = [run for row in self.rows for run in row.runs]

    def cell(self, x, y) -> Cell:
        if 0 <= x <= 9:
            if 0 <= y <= 9:
                return self.cells[y * 9 + x]
            else:
                raise IndexError('y')
        else:
            raise IndexError('x')

    @property
    def is_decided(self) -> bool:
        return all(cell.is_decided for cell in self.cells)
