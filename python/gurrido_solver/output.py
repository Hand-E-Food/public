from typing import Dict, Optional
from colorama import Back, Fore, Style, Cursor
from map import Map

text_color: Dict[bool, str] = {
    False: Back.BLACK + Fore.WHITE,
    True : Back.WHITE + Fore.BLACK,
}

def print_big(map: Map) -> None:
    output = ''
    for y in range(9):
        output += '┌───┬───┬───┬───┬───┬───┬───┬───┬───┐\n' if y == 0 else '├───┼───┼───┼───┼───┼───┼───┼───┼───┤\n'
        for m in range(1, 10, 3):
            for x in range(9):
                output += '│'
                cell = map.cell(x, y)
                if cell.is_wall:
                    output += text_color[True]
                elif cell.decision:
                    output += Back.BLUE + Fore.WHITE
                else:
                    output += text_color[False]
                for n in range(3):
                    number = m + n
                    if cell.decision:
                        output += f'{cell.decision}' if m == 4 and n == 1 else ' '
                    else:
                        output += f'{number}' if number in cell.numbers else ' '
                output += Style.RESET_ALL
            output += '│\n'
    output += '└───┴───┴───┴───┴───┴───┴───┴───┴───┘\n'
    print(output)

def print_small(map: Map, *, rewind: bool = False) -> None:
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
