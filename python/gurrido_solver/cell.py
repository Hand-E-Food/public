from typing import List, Optional, TYPE_CHECKING
from all_numbers import AllNumbers
from raw_cell import RawCell
from soil_function import SoilFunction
if TYPE_CHECKING:
    from row import Row
    from run import Run

class Cell:
    def __init__(self, soil_function: SoilFunction, name: str, raw: RawCell):
        self._soil_function = soil_function
        self.name = name
        self.rows: List['Row'] = []
        self.runs: List['Run'] = []
        self.is_wall: bool = raw.is_wall
        self.numbers: List[int]
        if raw.number:
            self.numbers = [raw.number]
            self.soil()
        elif raw.is_wall:
            self.numbers = []
        else:
            self.numbers = AllNumbers()

    def clean(self) -> None:
        for row in self.rows:
            row.soil()
        for run in self.runs:
            run.trim_permutations()

    def could_be(self, number: int) -> bool:
        return number in self.numbers

    @property
    def decision(self) -> Optional[int]:
        return self.numbers[0] if len(self.numbers) == 1 else None

    def eliminate(self, number: int) -> None:
        if self.could_be(number):
            if self.is_wall:
                raise ValueError("Cannot eliminate a wall's own number.")
            if self.decision:
                raise ValueError("Cannot eliminate a cell's last number.")
            print(f'{self.name} is not {number}')
            self.numbers.remove(number)
            self.soil()

    @property
    def is_blank_wall(self) -> bool:
        return self.is_wall and len(self.numbers) == 0

    @property
    def is_decided(self) -> bool:
        return self.is_wall or self.decision is not None

    def soil(self) -> None:
        self._soil_function(self)
