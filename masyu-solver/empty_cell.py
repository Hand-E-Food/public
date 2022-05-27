from typing import TYPE_CHECKING, Dict

import view
from cell import Cell
from colour import *
from contradiction import Contradiction
from edge import Edge

if TYPE_CHECKING:
    from map import Map

class EmptyCell(Cell):
    '''A cell with no pearl.'''

    UNKNOWN = NATURAL + ' '
    CONTRADICTION = YELLOW + '@'
    LOOSE = YELLOW + '∙'
    LINE = {
        'NS': YELLOW + '│',
        'EW': YELLOW + '─',
        'NE': YELLOW + '└',
        'NW': YELLOW + '┘',
        'SE': YELLOW + '┌',
        'SW': YELLOW + '┐',
    }

    def __init__(self, map: 'Map', x: int, y: int):
        super().__init__(map, x, y)
        self.state = EmptyCell.UNKNOWN

    def __str__(self) -> str: return self.state

    def initialise(self) -> None:
        super().initialise()

        map = self.map
        x = self.x
        y = self.y
        with view.indent(f'{x:02},{y:02} empty cell initialising'):

            triggers: Dict[str, Edge] = {
                'N': map.get(x, y - 1),
                'S': map.get(x, y + 1),
                'E': map.get(x + 1, y),
                'W': map.get(x - 1, y),
            }

            self.triggers = triggers

            for edge in triggers.values():
                edge.add_listener(self.edge_changed)

    def edge_changed(self, edge) -> None:
        triggers = self.triggers
        with view.indent(f'{self.x:02},{self.y:02} empty cell reacts'):

            blocked = ''
            threaded = ''
            for direction, edge in triggers.items():
                if edge.is_blocked:
                    blocked += direction
                elif edge.is_threaded:
                    threaded += direction

            if len(blocked) == 2 and len(threaded) == 1:
                for edge in triggers.values():
                    if edge.is_unknown:
                        edge.thread()

            elif len(blocked) == 3 or len(threaded) == 2:
                for edge in triggers.values():
                    if edge.is_unknown: edge.block()

            # Refresh before selecting state
            blocked = ''
            threaded = ''
            for direction, edge in triggers.items():
                if edge.is_blocked:
                    blocked += direction
                elif edge.is_threaded:
                    threaded += direction

            if len(threaded) == 0:
                self.state = EmptyCell.UNKNOWN
            elif len(threaded) == 1:
                if len(blocked) == 3:
                    self.state = EmptyCell.CONTRADICTION
                    raise Contradiction(f'Cell {self.x:02},{self.y:02} has a loose thread!')
                else:
                    self.state = EmptyCell.LOOSE
            elif len(threaded) == 2:
                self.state = EmptyCell.LINE[threaded]
            else:
                self.state = EmptyCell.CONTRADICTION
                raise Contradiction(f'Cell {self.x:02},{self.y:02} has more than 2 threads!')

    def create_state(self, state: dict) -> None:
        super().create_state(state)
        state['state'] = self.state

    def become_state(self, state: dict) -> None:
        super().become_state(state)
        self.state = state['state']
