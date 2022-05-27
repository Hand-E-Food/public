from typing import TYPE_CHECKING, Callable, List
from colour import *
from contradiction import Contradiction
from datum import Datum

import view

if TYPE_CHECKING:
    from cell import Cell
    from map import Map

class Edge(Datum):
    BLOCKED = CRIMSON + '×'
    UNKNOWN = NATURAL + ' '

    @property
    def THREAD(cls) -> str: raise NotImplementedError('THREAD')

    def __init__(self, map: 'Map', x: int, y: int):
        super().__init__(map, x, y)

        self.map = map
        '''The map this edge is on.'''

        self.x = x
        '''This edge's X position on the map.'''

        self.y = y
        '''This edge's Y position on the map.'''

        self.state = Edge.UNKNOWN
        '''This edge's current state.'''

        self.listeners: List[Callable[['Edge'], None]] = []
        '''The functions to call when this edge changes state.'''

    def __str__(self) -> str: return self.state

    def add_listener(self, listener: Callable[['Edge'], None]):
        self.listeners.append(listener)
        if not self.is_unknown:
            with view.indent(f'{self.x:02},{self.y:02} existing {"blocked" if self.is_blocked else "threaded"}'):
                listener(self)

    @property
    def is_blocked(self) -> bool: return self.state == Edge.BLOCKED

    @property
    def is_unknown(self) -> bool: return self.state == Edge.UNKNOWN

    @property
    def is_threaded(self) -> bool: return self.state == self.THREAD

    def initialise(self) -> None: pass

    def block(self) -> None:
        if self.is_threaded: raise Contradiction(f'Edge {self.x:02},{self.y:02} is already part of the necklace.')

        if not self.is_blocked:
            self.state = Edge.BLOCKED
            view.log(f'{self.x:02},{self.y:02} blocked')
            self.notify()

    def thread(self) -> None:
        if self.is_blocked: raise Contradiction(f'Edge {self.x:02},{self.y:02} is already blocked.')

        if not self.is_threaded:
            self.state = self.THREAD
            view.log(f'{self.x:02},{self.y:02} threaded')
            self.notify()

        thread_id = max(cell.thread_id for cell in self.adjacent_cells)
        for cell in self.adjacent_cells:
            cell.thread(thread_id)

    @property
    def adjacent_cells(self) -> List['Cell']: raise NotImplementedError('get_adjacent_cells')

    def notify(self) -> None:
        with view.indent():
            for listener in self.listeners:
                listener(self)

    def create_state(self, state: dict) -> None:
        super().create_state(state)
        state['state'] = self.state

    def become_state(self, state: dict) -> None:
        super().become_state(state)
        self.state = state['state']


class HEdge(Edge):
    @Edge.THREAD.getter
    def THREAD(cls) -> str: return YELLOW + '─'

    @Edge.adjacent_cells.getter
    def adjacent_cells(self) -> List['Cell']:
        return [
            self.map.get(self.x - 1, self.y),
            self.map.get(self.x + 1, self.y),
        ]


class VEdge(Edge):
    @Edge.THREAD.getter
    def THREAD(cls) -> str: return YELLOW + '│'

    @Edge.adjacent_cells.getter
    def adjacent_cells(self) -> List['Cell']:
        return [
            self.map.get(self.x, self.y - 1),
            self.map.get(self.x, self.y + 1),
        ]
