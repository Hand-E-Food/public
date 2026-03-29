from typing import List, TYPE_CHECKING, Set

from .state import State

if TYPE_CHECKING:
    from .group import Group


class Cell:
    def __init__(self, x: int, y: int) -> None:
        self.adjacent_cells: Set[Cell] = set() 
        self.groups: List[Group] = []
        self.state: State = State.UNKNOWN
        self.x: int = x
        self.y: int = y
