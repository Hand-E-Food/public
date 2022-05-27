from typing import List

import view
from callback import Callback
from cell import Cell
from colour import *
from edge import Edge

FAR1 = 0
NEAR1 = 1
NEAR2 = 2
FAR2 = 3

class WhitePearl(Cell):
    '''A cell with a white pearl.'''

    def __str__(self) -> str: return WHITE + '●'

    def initialise(self) -> None:
        super().initialise()
        map = self.map
        x = self.x
        y = self.y

        with view.indent(f'{x:02},{y:02} white pearl initialising'):

            triggers: List[List[Edge]] = [
                [
                    map.get(x, y - 3),
                    map.get(x, y - 1),
                    map.get(x, y + 1),
                    map.get(x, y + 3),
                ],
                [
                    map.get(x - 3, y),
                    map.get(x - 1, y),
                    map.get(x + 1, y),
                    map.get(x + 3, y),
                ],
            ]

            self.triggers = triggers
            self.orientation: int = None

            cells: List[List[Cell]] = [
                [
                    map.get(x, y - 2),
                    map.get(x, y + 2),
                ],
                [
                    map.get(x - 2, y),
                    map.get(x + 2, y),
                ],
            ]
            for orientation in range(2):
                if all(isinstance(cell, WhitePearl) for cell in cells[orientation]):
                    triggers[orientation][NEAR1].block()
                    triggers[orientation][NEAR2].block()

            for orientation in range(2):
                for relation in range(4):
                    edge = triggers[orientation][relation]
                    callback = Callback(self.edge_changed, orientation, relation, edge)
                    edge.add_listener(callback)

    def edge_changed(self, orientation: int, relation: int, changed_edge: Edge) -> None:
        triggers = self.triggers
        with view.indent(f'{self.x:02},{self.y:02} white pearl reacts to [{["V","H"][orientation]},{["FAR1","NEAR1","NEAR2","FAR2"][relation]}]'):

            if self.orientation is None:

                if changed_edge.is_blocked:
                    if relation in [ NEAR1, NEAR2 ] or (triggers[orientation][FAR1].is_blocked and triggers[orientation][FAR2].is_blocked):
                        self.orientation = 1 - orientation

                elif changed_edge.is_threaded:
                    if relation in [ NEAR1, NEAR2 ]:
                        self.orientation = orientation

            if self.orientation is not None:
                triggers[self.orientation][NEAR1].thread()
                triggers[self.orientation][NEAR2].thread()
                triggers[1 - self.orientation][NEAR1].block()
                triggers[1 - self.orientation][NEAR2].block()
                if triggers[self.orientation][FAR1].is_threaded:
                    triggers[self.orientation][FAR2].block()
                elif triggers[self.orientation][FAR2].is_threaded:
                    triggers[self.orientation][FAR1].block()

    def create_state(self, state: dict) -> None:
        super().create_state(state)
        state['orientation'] = self.orientation
    
    def become_state(self, state: dict) -> None:
        super().become_state(state)
        self.orientation = state['orientation']
