from colorama import Back, Fore, init as colorama_init

from model.cell import Cell
from model.grid import Grid
from model.state import State

colorama_init(autoreset=True)

class Painter:
    States = {
        State.UNKNOWN: ' ',
        State.EMPTY: '∙',
        State.STAR: '*',
    }
    Colors = [
        Back.BLUE,
        Back.GREEN,
        Back.CYAN,
        Back.RED,
        Back.MAGENTA,
        Back.YELLOW,
        Back.WHITE,
        Back.LIGHTBLACK_EX,
        Back.LIGHTBLUE_EX,
        Back.LIGHTGREEN_EX,
        Back.LIGHTCYAN_EX,
        Back.LIGHTRED_EX,
        Back.LIGHTMAGENTA_EX,
        Back.LIGHTYELLOW_EX,
        Back.LIGHTWHITE_EX,
        Back.BLACK,
    ]

    def paint(self, grid: Grid):
        color = Back.RESET
        for row in grid.cells:
            text = Fore.BLACK
            for cell in row:
                new_color = self._get_area_color(grid, cell)
                if color != new_color:
                    color = new_color
                    text += color
                text += self._get_char(cell)
            color = Back.RESET + Fore.RESET
            text += color
            print(text)
    
    def _get_area_color(self, grid: Grid, cell: Cell) -> str:
        for index, area in enumerate(grid.areas):
            if cell in area.cells:
                return Painter.Colors[index % len(Painter.Colors)]
        raise ValueError("Cell does not belong to any area.")

    def _get_char(self, cell: Cell) -> str:
        return Painter.States[cell.state]

    def paint_difficulty(self, difficulty: int):
        print()
        print('*' * difficulty)