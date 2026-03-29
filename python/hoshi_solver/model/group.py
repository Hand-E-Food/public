from typing import Iterable, List, Tuple

from .cell import Cell
from .state import State


class Group:
    def __init__(self, name: str, stars: int | Tuple[int, int], cells: Iterable[Cell]):
        self.name = name
        self.min_stars = stars if isinstance(stars, int) else stars[0]
        self.max_stars = stars if isinstance(stars, int) else stars[1]
        self.cells = cells if isinstance(cells, list) else list(cells)
        for cell in cells:
            cell.groups.append(self)

    @property
    def unknowns(self) -> int:
        return sum(1 for cell in self.cells if cell.state == State.UNKNOWN)

    @property
    def empties(self) -> int:
        return sum(1 for cell in self.cells if cell.state == State.EMPTY)

    @property
    def stars(self) -> int:
        return sum(1 for cell in self.cells if cell.state == State.STAR)

    @property
    def is_valid(self) -> bool:
        return self.min_stars <= self.unknowns + self.stars \
            and self.stars <= self.max_stars
    
    def simplified(self) -> 'Group':
        cells = [cell for cell in self.cells if cell.state == State.UNKNOWN]
        stars = self.max_stars - self.stars
        return Group(self.name + "_simplified", stars, cells)
