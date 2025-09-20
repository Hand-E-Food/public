from typing import List, TYPE_CHECKING

if TYPE_CHECKING:
    from group import Group


class Cell:
    def __init__(self, x: int, y: int) -> None:
        self.x: int = x
        self.y: int = y
        self.groups: List[Group] = []
