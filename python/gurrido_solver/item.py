from typing import Union, TYPE_CHECKING
if TYPE_CHECKING:
    from cell import Cell
    from row import Row
    from run import Run


type Item = Union['Cell', 'Row', 'Run']
