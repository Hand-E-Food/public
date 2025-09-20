from grid import Grid
from grid_factory import GridFactory

class Parser:
    def __init__(self, grid_factory: GridFactory) -> None:
        self._grid_factory = grid_factory

    def load(self, path: str) -> Grid:
        with open(path, 'r') as f:
            stars = int(f.readline().strip())
            rows = [[cell for cell in line.strip()] for line in f.readlines()]
        return self._grid_factory.create_grid(stars, rows)
