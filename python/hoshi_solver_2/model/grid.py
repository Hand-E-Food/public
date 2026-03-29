from typing import Iterable, List

from .cells import Cells
from .group import Group


class Grid:
    def __init__(self) -> None:
        self.cells: Cells
        self.areas: List[Group]
        self.columns: List[Group]
        self.rows: List[Group]
        self.quads: List[Group]
        

    @property
    def all_groups(self) -> Iterable[Group]:
        yield from self.star_groups
        yield from self.quads

    @property
    def star_groups(self) -> Iterable[Group]:
        yield from self.columns
        yield from self.rows
        yield from self.areas

    def

    def remove_group(self, group: Group) -> None:
        if group in self.quads:
            self.quads.remove(group)
        if group in self.columns:
            self.columns.remove(group)
        if group in self.rows:
            self.rows.remove(group)
        if group in self.areas:
            self.areas.remove(group)
