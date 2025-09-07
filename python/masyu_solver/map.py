from typing import Any, List

import view
from black_pearl import BlackPearl
from black_with_diagonal_whites import BlackWithDiagonalWhites
from border_cell import BorderCell
from cell import Cell
from contradiction import Contradiction
from dual_black_pearls import DualBlackPearls
from dual_white_pearls import DualWhitePearls
from edge import HEdge, VEdge
from empty_cell import EmptyCell
from grid import Grid
from white_pearl import WhitePearl


_PATTERNS = [
    BlackWithDiagonalWhites,
    DualBlackPearls,
    DualWhitePearls,
]


class Map:

    def __init__(self, input: List[str]):
        input = self.validate_input(input)

        data: List[List[Any]] = [None] * (len(input) * 2 + 5)
        for y in range(len(data)):
            data[y] = [None] * (len(input[0]) * 2 + 5)
        self.data = data
        '''The data for the map, including cells and edges.'''

        self.pearls: List[Cell] = []
        '''All pearls on the map.'''

        for y in range(0, len(data), 2): # Even rows
            row = data[y]
            for x in range(0, len(row), 2): # Even column
                row[x] = Grid(self, x, y)
            for x in range(1, len(row), 2): # Odd columns
                row[x] = VEdge(self, x, y)

        for y in range(1, len(data), 2): # Odd rows
            row = data[y]
            for x in range(0, len(row), 2): # Even columns
                row[x] = HEdge(self, x, y)
            for x in [1, len(row) - 2]: # Outer odd columns
                row[x] = BorderCell(self, x, y)

        for y in [1, len(data) - 2]: # Outer odd rows
            row = data[y]
            for x in range(3, len(row) - 2, 2): # Inner odd columns
                row[x] = BorderCell(self, x, y)

        for input_y in range(len(input)): # Inner odd rows
            map_y = input_y * 2 + 3
            map_row = data[map_y]
            input_row = input[input_y]
            for input_x in range(len(input_row)): # Inner odd columns
                map_x = input_x * 2 + 3
                char = input_row[input_x]
                if char == 'b':
                    cell = BlackPearl(self, map_x, map_y)
                    self.pearls.append(cell)
                elif char == 'w':
                    cell = WhitePearl(self, map_x, map_y)
                    self.pearls.append(cell)
                else:
                    cell = EmptyCell(self, map_x, map_y)
                map_row[map_x] = cell

    def validate_input(self, input):
        if input:
            width = len(input[0])
            if all(len(line) == width for line in input):
                height = len(input)
                if height <= width:
                    return input

                transposed = []
                for x in range(width):
                    line = ''
                    for y in range(height):
                        line += input[y][x]
                    transposed.append(line)
                return transposed

        raise ValueError('input must be a list of equal length strings')

    def solve(self, guess: bool = True) -> None:
        data = self.data
        height = len(data)
        width = len(data[0])

        for y in range(1, height, 2): # Inner odd rows
            row = data[y]
            for x in range(1, width, 2): # Inner odd columns
                row[x].initialise()
                for pattern in _PATTERNS:
                    pattern.detect(self, x, y)

        while guess:
            guess = False
            for y in range(height):
                row = data[y]
                for x in range(1 - (y % 2), width, 2):
                    edge = row[x]
                    if edge.is_unknown:
                        self.save_state()
                        try:
                            with view.indent('Saved state'):
                                edge.thread()
                            view.log('Reverted state')
                        except Contradiction:
                            self.revert_state()
                            edge.block()
                            guess = True
                        else:
                            self.revert_state()

    def get(self, x: int, y: int) -> Any:
        try:
            return self.data[y][x]
        except IndexError:
            return None

    def save_state(self) -> None:
        for row in self.data:
            for cell in row:
                cell.save_state()

    def revert_state(self) -> None:
        for row in self.data:
            for datum in row:
                datum.revert_state()

    @property
    def all_pearls_threaded_together(self) -> bool:
        thread_id = self.pearls[0].thread_id
        return all(pearl.thread_id == thread_id for pearl in self.pearls)
