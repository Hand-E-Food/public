from typing import TYPE_CHECKING, List

import view
from black_pearl import BlackPearl
from edge import Edge
from white_pearl import WhitePearl

if TYPE_CHECKING:
    from map import Map


class DualWhitePearls:
    '''A pattern of two adjacent white pearls.'''

    @staticmethod
    def detect(map: 'Map', x: int, y: int):
        if not isinstance(map.get(x, y), WhitePearl): return

        for dx, dy in ((0, 1), (1, 0)):
            # Position is the edge between the two white pearls.
            mid_x = x - dx
            mid_y = y - dy
            def map_position(r: int): return map.get(mid_x + r * dx, mid_y + r * dy)

            if isinstance(map_position(-1), WhitePearl):
                with view.indent(f'{mid_x:02},{mid_y:02} dual white pearls initialising'):

                    # Three or more white pearls in a row cannot be threaded directly.
                    if isinstance(map_position(-3), WhitePearl):
                        map_position(0).block()
                    else:
                        # A black pearl two spaces away cannot thread directly with a pair of white pearls.
                        for d in (-1, +1):
                            if isinstance(map_position(5 * d), BlackPearl):
                                map_position(4 * d).block()
                        # A thread through two white pearls in a row must turn at each end.
                        DualWhitePearls(
                            triggers=[ map_position(4 * d) for d in (-1, +1)],
                            response=map_position(0),
                        )

    def __init__(self, triggers: List[Edge], response: Edge):
        self.response = response
        '''The edge to block if either trigger is threaded.'''

        for edge in triggers:
            edge.add_listener(self.edge_changed)
    
    def edge_changed(self, changed_edge: Edge):
        with view.indent(f'{self.response.x:02},{self.response.y:02} dual white pearls reacts'):
            if changed_edge.is_threaded:
                self.response.block()
