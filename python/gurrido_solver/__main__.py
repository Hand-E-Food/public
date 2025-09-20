from typing import List
from raw_cell import RawCell
from map_factory import parse
import output
from solver import Solver

def load_map(path: str) -> List[RawCell]:
    with open(path, 'r') as file:
        return parse([line.strip() for line in file.readlines()])

cells = load_map('map2.txt')
solver = Solver(cells)
output.print_small(solver.map, rewind=False)
print()
solver.solve()
print()
output.print_big(solver.map)
