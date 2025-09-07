from typing import Callable, TYPE_CHECKING
if TYPE_CHECKING:
    from item import Item


type SoilFunction = Callable[[Item], None]
