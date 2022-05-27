from colour import *
from datum import Datum


class Grid(Datum):
    def __str__(self) -> str: return SLATE + '·'

    def initialise(self) -> None: pass
