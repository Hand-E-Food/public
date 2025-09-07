from typing import TYPE_CHECKING, List

if TYPE_CHECKING:
    from map import Map


class Datum:
    def __init__(self, map: 'Map', x: int, y: int) -> None:
        self._states: List[dict] = []
        '''This datum's saved states.'''

        self.map = map
        '''The map this datum is on.'''

        self.x = x
        '''This datum's x position.'''

        self.y = y
        '''This datum's y position.'''

    def save_state(self):
        state = {}
        self.create_state(state)
        self._states.append(state)

    def create_state(self, state: dict) -> None: pass

    def revert_state(self):
        self.become_state(self._states.pop())

    def become_state(self, state: dict) -> None: pass
