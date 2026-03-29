from typing import TYPE_CHECKING, Set

from .state import State

if TYPE_CHECKING:
    from .group import Group


class Cell:
    def __init__(self, x: int, y: int) -> None:
        self.adjacent_cells: Set[Cell] = set() 
        self._groups: Set[Group] = set()
        self.state: State = State.UNKNOWN
        self.x: int = x
        self.y: int = y

    def add_group(self, group: Group) -> None:
        if group in self._groups:
            return
        self._groups.add(group)

    def become(self, state: State) -> None:
        self.state = state
        for group in self._groups:
            group.remove_cell(self)
        self._groups.clear()
