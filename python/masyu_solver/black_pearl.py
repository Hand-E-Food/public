from typing import List

import view
from callback import Callback
from cell import Cell
from colour import *
from edge import Edge

NEAR = 0
FAR = 1
LEFT = 2
RIGHT = 3


class BlackPearl(Cell):
    '''A cell with a black peral.'''

    def __str__(self) -> str: return BLUE + '●'

    def initialise(self) -> None:
        super().initialise()
        map = self.map
        x = self.x
        y = self.y

        with view.indent(f'{x:02},{y:02} black pearl initialising'):

            triggers: List[List[List[Edge]]] = [
                [
                    [
                        map.get(x    , y - 1),
                        map.get(x    , y - 3),
                        map.get(x - 1, y - 2),
                        map.get(x + 1, y - 2),
                    ],
                    [
                        map.get(x    , y + 1),
                        map.get(x    , y + 3),
                        map.get(x + 1, y + 2),
                        map.get(x - 1, y + 2),
                    ],
                ],
                [
                    [
                        map.get(x - 1, y    ),
                        map.get(x - 3, y    ),
                        map.get(x - 2, y + 1),
                        map.get(x - 2, y - 1),
                    ],
                    [
                        map.get(x + 1, y    ),
                        map.get(x + 3, y    ),
                        map.get(x + 2, y - 1),
                        map.get(x + 2, y + 1),
                    ],
                ],
            ]

            self.triggers = triggers

            for orientation in range(2):
                for direction in range(2):
                    for relation in range(4):
                        edge = triggers[orientation][direction][relation]
                        callback = Callback(self.edge_changed, orientation, direction, relation, edge)
                        edge.add_listener(callback)

    def edge_changed(self, orientation: int, direction: int, relation: int, changed_edge: Edge) -> None:
        triggers = self.triggers
        with view.indent(f'{self.x:02},{self.y:02} black pearl reacts to [{[["N","S"],["W","E"]][orientation][direction]},{["NEAR","FAR","LEFT","RIGHT"][relation]}]'):

            if changed_edge.is_blocked:

                if relation == FAR:
                    triggers[orientation][direction][NEAR].block()

                elif relation == NEAR:
                    triggers[orientation][1 - direction][NEAR].thread()
            
            elif changed_edge.is_threaded:

                if relation == NEAR:
                    triggers[orientation][direction][FAR].thread()
                    triggers[orientation][1 - direction][NEAR].block()

                if relation in [LEFT, RIGHT]:
                    triggers[orientation][direction][NEAR].block()
