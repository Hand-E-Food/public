from typing import Iterable, List

from cell import Cell


class Group:
    def __init__(self, min_stars: int, max_stars: int, cells: List[Cell]):
        self.min_stars = min_stars
        self.max_stars = max_stars
        self.cells = cells

    def clean(self) -> Iterable['Group']:
        return []
