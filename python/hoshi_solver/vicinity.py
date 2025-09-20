from typing import Iterable, List

from cell import Cell
from group import Group


class Row(Group):
    def __init__(self, cells: List[Cell]) -> None:
        super().__init__(0, 1, cells)

    def clean(self) -> Iterable[Group]:
        return super().clean()
