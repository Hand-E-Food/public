from typing import Iterable, List

from cell import Cell
from group import Group


class Line(Group):
    def __init__(self, stars: int, cells: List[Cell]) -> None:
        super().__init__(stars, stars, cells)

    def clean(self) -> Iterable[Group]:
        return super().clean()
