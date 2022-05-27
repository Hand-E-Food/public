import view
from cell import Cell


class BorderCell(Cell):
    '''A cell outside the bounds of the map.'''

    def initialise(self) -> None:
        super().initialise()

        with view.indent(f'{self.x:02},{self.y:02} border initialising'):
            for edge in self.adjacent_edges:
                edge.block()

    def __str__(self) -> str: return '█'
