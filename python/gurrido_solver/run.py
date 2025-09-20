from itertools import permutations
from typing import List, Tuple, TYPE_CHECKING
from all_numbers import AllNumbers
from cell import Cell
from soil_function import SoilFunction
if TYPE_CHECKING:
    from row import Row


class Run:
    def __init__(self, soil_function: SoilFunction, name: str, row: 'Row', cells: List[Cell]):
        if 1 > len(cells) or len(cells) > 9:
            raise ValueError('A run must have between 1 and 9 cells.')
        self._soil_function = soil_function
        self.name = name
        self.cells = cells
        for cell in cells:
            cell.runs.append(self)
        self.row = row
        length = len(cells)
        self.hard_min = 10 - length
        self.hard_max = 0 + length
        self.permutations = [
            permutation
            for soft_min in range(1, 11 - length)
            for permutation in permutations(range(soft_min, soft_min + length))
            if self._is_valid_permutation(permutation)
        ]
        self.soil()

    def clean(self) -> None:
        self.trim_permutations()

        # Flatten all permutations.
        flat_permutations = [set() for _ in range(self.length)]
        for i in range(self.length):
            flat_cell = flat_permutations[i]
            for permutation in self.permutations:
                flat_cell.add(permutation[i])

        # Exclude numbers that don't fit the flat permutation.
        for i in range(self.length):
            flat_cell = flat_permutations[i]
            cell = self.cells[i]
            for number in AllNumbers():
                if number not in flat_cell:
                    cell.eliminate(number)

        # Calculate the soft and hard limits of this run.
        soft_min = min(number for flat_cell in flat_permutations for number in flat_cell)
        soft_max = max(number for flat_cell in flat_permutations for number in flat_cell)
        hard_min = soft_max - self.length + 1
        hard_max = soft_min + self.length - 1

        # Did something change?
        if self.hard_min != hard_min or self.hard_max != hard_max:
            self.hard_min = hard_min
            self.hard_max = hard_max
            # Remove hard numbers from cells in this row but not in this run.
            if hard_min <= hard_max:
                min_str = f'{soft_min} ~ {hard_min}' if soft_min < hard_min else f'{hard_min}'
                max_str = f'{hard_max} ~ {soft_max}' if hard_max < soft_max else f'{hard_max}'
                print(f'{min_str} ≤ {self.name} ≤ {max_str}')
                hard_numbers = list(range(hard_min, hard_max + 1))
                for cell in self.row.cells:
                    if cell not in self.cells:
                        for number in hard_numbers:
                            cell.eliminate(number)
                # Update the row now that this run has new hard limits.
                self.row.soil()

    @property
    def is_decided(self) -> bool:
        return all(cell.is_decided for cell in self.cells)

    def _is_valid_permutation(self, permutation: Tuple[int, ...]) -> bool:
        return all(self.cells[i].could_be(permutation[i]) for i in range(self.length))

    @property
    def length(self) -> int:
        return len(self.cells)

    def soil(self) -> None:
        self._soil_function(self)

    def trim_permutations(self) -> None:
        new_permutations = list(filter(self._is_valid_permutation, self.permutations))
        if len(new_permutations) == len(self.permutations):
            return
        self.permutations = new_permutations
        self.soil()
