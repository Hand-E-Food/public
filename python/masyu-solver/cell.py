from typing import TYPE_CHECKING, List

from datum import Datum

if TYPE_CHECKING:
    from edge import Edge
    from map import Map


class Cell(Datum):
    def __init__(self, map: 'Map', x: int, y: int):
        super().__init__(map, x, y)

        self.thread_id = x * 100 + y
        '''The ID of the thread passing through this cell.'''
    
    def initialise(self) -> None: pass

    @property
    def adjacent_edges(self) -> List['Edge']:
        return [
            self.map.get(self.x, self.y - 1),
            self.map.get(self.x + 1, self.y),
            self.map.get(self.x, self.y + 1),
            self.map.get(self.x - 1, self.y),
        ]

    def thread(self, thread_id: int) -> None:
        map = self.map
        x = self.x
        y = self.y

        if self.thread_id < thread_id:
            # Note: Use `self.thread_id` going forward as its value may change again mid loop.
            self.thread_id = thread_id
            edges = self.adjacent_edges
            for edge in edges:
                if edge.is_threaded:
                    edge.thread()
            
            for edge in edges:
                if edge.is_unknown:
                    dx = edge.x - x
                    dy = edge.y - y
                    cell = map.get(x + 2 * dx, y + 2 * dy)
                    if cell.thread_id == self.thread_id:
                        if map.all_pearls_threaded_together:
                            edge.thread()
                        else:
                            edge.block()

    def create_state(self, state: dict) -> None:
        super().create_state(state)
        state['thread_id'] = self.thread_id

    def become_state(self, state: dict) -> None:
        super().become_state(state)
        self.thread_id = state['thread_id']
