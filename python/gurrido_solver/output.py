from typing import Dict, Optional
from colorama import Back, Fore, Style, Cursor
from map import Map

text_color: Dict[bool, str] = {
    False: Back.BLACK + Fore.WHITE,
    True : Back.WHITE + Fore.BLACK,
}

def print_map(map: Map, *, rewind: bool = False) -> None:
    output = Cursor.UP(9) if rewind else ''
    in_wall: Optional[bool] = None
    for y in range(9):
        for x in range(9):
            cell = map.cell(x, y)
            is_wall = cell.is_wall
            if is_wall != in_wall:
                in_wall = is_wall
                output += text_color[in_wall]
            output += str(cell.decision or (' ' if is_wall else '.'))
        in_wall = None
        output += Style.RESET_ALL
        output += '\n'
    print(output, end='')
