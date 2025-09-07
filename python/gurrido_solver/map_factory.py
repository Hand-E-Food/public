from typing import Dict, List

from raw_cell import RawCell

_convert: Dict[str, RawCell] = {
    '1': RawCell(1, False),
    '2': RawCell(2, False),
    '3': RawCell(3, False),
    '4': RawCell(4, False),
    '5': RawCell(5, False),
    '6': RawCell(6, False),
    '7': RawCell(7, False),
    '8': RawCell(8, False),
    '9': RawCell(9, False),
    '.': RawCell(None, False),
    'a': RawCell(1, True),
    'b': RawCell(2, True),
    'c': RawCell(3, True),
    'd': RawCell(4, True),
    'e': RawCell(5, True),
    'f': RawCell(6, True),
    'g': RawCell(7, True),
    'h': RawCell(8, True),
    'i': RawCell(9, True),
    'x': RawCell(None, True),
}

def parse(lines: List[str]) -> List[RawCell]:
    if len(lines) != 9:
        raise ValueError('There must be 9 lines.')
    cells: List[RawCell] = []
    for y in range(9):
        line = lines[y]
        if len(line) != 9:
            raise ValueError('All lines must have 9 characters.')
        line = line.lower()
        for x in range(9):
            char = line[x]
            if char not in _convert:
                raise ValueError('All characters must be one of: ' + ''.join(_convert.keys()))
            cells.append(_convert[char])
    return cells
