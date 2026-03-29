import sys
from painter import Painter
from parser import Parser
from solver import Solver


grid = Parser().load(sys.argv[1])

painter = Painter()
def paint() -> None:
    painter.paint(grid)
    painter.paint_difficulty(solver.difficulty)
    input("")

solver = Solver(grid)
solver.on_changed.append(paint)
solver.solve()
