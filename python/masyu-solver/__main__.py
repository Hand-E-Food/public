import os.path
import sys
from typing import List

import view
from map import Map


def main(argv: List[str]) -> int:
    with open(os.path.join('puzzles', argv[0]), 'r') as file:
        lines = [ line.rstrip('\n') for line in file.readlines() ]
        lines = [ line for line in lines if line ]
    map = Map(lines)
    try:
        map.solve(guess=True)
    finally:
        view.print(map)

    return 0


if __name__ == '__main__':
    sys.exit(main(sys.argv[1:]))
