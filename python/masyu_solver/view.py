import builtins
from contextlib import contextmanager
from math import floor
from typing import Generator

from colour import *
from map import Map

_indent = 0

def print(map: Map) -> None:
    result = ''
    for y in range(2, len(map.data) - 2):
        row = map.data[y]
        for x in range(2, len(row) - 2):
            result += str(row[x])
        result += NATURAL
        if y % 2 == 1:
            result += f' {y:2}'
        result += '\n'
    result += '\n'
    for x in range(2, len(map.data[0]) - 2):
        if x >= 10 and x % 2 == 1:
            result += str(floor(x / 10))
        else:
            result += ' '
    result += '\n'
    for x in range(2, len(map.data[0]) - 2):
        if x % 2 == 1:
            result += str(x % 10)
        else:
            result += ' '

    builtins.print(result)


@contextmanager
def indent(message: str = None) -> Generator[None, None, None]:
    global _indent
    if message: log(message)
    _indent += 2
    yield
    _indent -= 2

def log(message: str) -> None: builtins.print(' '*_indent + message)
