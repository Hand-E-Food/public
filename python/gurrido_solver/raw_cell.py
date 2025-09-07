from typing import Optional


class RawCell:
    def __init__(self, number: Optional[int], is_wall: bool):
        self.number = number
        self.is_wall = is_wall
