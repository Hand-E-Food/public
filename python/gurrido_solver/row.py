from itertools import combinations
from typing import List, Optional
from all_numbers import AllNumbers
from cell import Cell
from raw_cell import RawCell
from run import Run
from soil_function import SoilFunction


class Row:
    def __init__(self, soil_function: SoilFunction, name: str, cells: List[Cell]):
        if len(cells) != 9:
            raise ValueError('A row must have 9 cells.')
        self._soil_function = soil_function
        self.name = name
        self.cells = cells
        for cell in cells:
            cell.rows.append(self)
        self.runs = self._create_runs(soil_function, name, cells)
        self.soil()

    def _create_runs(self, soil_function: SoilFunction, name: str, cells: List[Cell]) -> List[Run]:
        runs: List[Run] = []
        cells = [*cells, Cell(soil_function, 'x', RawCell(number=None, is_wall=True))]
        i1: Optional[int] = None
        for i2 in range(10):
            cell = cells[i2]
            if cell.is_wall:
                if cell.decision:
                    runs.append(Run(soil_function, f'{name}:{i2}', self, [cells[i2]]))
                i1 = None
            else:
                if i1 is None:
                    i1 = i2
                if cells[i2 + 1].is_wall:
                    run_name = f'{name}:{i2}' if i1 == i2 else f'{name}:{i1}-{i2}'
                    runs.append(Run(soil_function, run_name, self, cells[i1:i2 + 1]))
        return runs

    def clean(self) -> None:
        # Find cells in this row where it's certain that all of its numbmers are required in its run.
        known_cells = [
            cell
            for run in self.runs
            for cell in run.cells
            if all(
                run.hard_min <= number <= run.hard_max
                for number in cell.numbers
            )
        ]
        known_cells.sort(key=lambda cell: len(cell.numbers))
        length = 1
        while length < len(known_cells):
            targets = combinations(AllNumbers(), length)
            for target in targets:
                target_cells = [
                    cell
                    for cell in known_cells
                    if all(
                        number in target
                        for number in cell.numbers
                    )
                ]
                if len(target_cells) == len(target):
                    for row_cell in self.cells:
                        if row_cell not in target_cells:
                            for number in target:
                                row_cell.eliminate(number)
                    for target_cell in target_cells:
                        known_cells.remove(target_cell)
            length += 1

    @property
    def is_decided(self) -> bool:
        return all(cell.is_decided for cell in self.cells)

    @property
    def length(self) -> int:
        return len(self.cells)

    def soil(self) -> None:
        self._soil_function(self)
