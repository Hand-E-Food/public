from typing import Iterable, List

from .cells import Cells
from .group import Group


class Grid:
    def __init__(self) -> None:
        self.cells: Cells
        self.areas: List[Group]
        self.columns: List[Group]
        self.rows: List[Group]
        self.adjacencies: List[Group]

    @property
    def all_groups(self) -> Iterable[Group]:
        yield from self.star_groups
        yield from self.adjacencies

    @property
    def star_groups(self) -> Iterable[Group]:
        yield from self.columns
        yield from self.rows
        yield from self.areas
