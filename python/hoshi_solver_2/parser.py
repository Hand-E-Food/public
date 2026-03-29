from model.grid import Grid
from grid_factory import GridFactory

class Parser:
    @staticmethod
    def load(path: str) -> Grid:
        with open(path, 'r') as f:
            stars = int(f.readline().strip())
            rows = [[cell for cell in line.strip()] for line in f.readlines()]
        return GridFactory(stars, rows).grid
