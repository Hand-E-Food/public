from typing import Dict, List, Optional, Set, Type
from cell import Cell
from item import Item
from map import Map
from raw_cell import RawCell
from row import Row
from run import Run


class Solver:
    def __init__(self, cells: List[RawCell]) -> None:
        self.dirty_items: Dict[Type, Set[Item]] = {
            Cell: set(),
            Row: set(),
            Run: set(),
        }
        self.map = Map(self.soil, cells)

    def _pop_dirty_item(self) -> Optional[Item]:
        for items in self.dirty_items.values():
            if len(items) > 0:
                return items.pop()
        return None

    def soil(self, item: Item) -> None:
        self.dirty_items[type(item)].add(item)

    def solve(self) -> None:
        while True:
            item = self._pop_dirty_item()
            if not item:
                break
            print(item.name)
            item.clean()
