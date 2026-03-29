from itertools import combinations
from typing import Callable, Iterable, List, Optional, Set, Tuple

from model.cell import Cell
from model.grid import Grid
from model.group import Group
from model.state import State


class Solver:
    def __init__(self, grid: Grid) -> None:
        self._difficulty = 0
        self.grid = grid
        self.on_changed: List[Callable[[], None]] = []

    @property
    def difficulty(self) -> int:
        return self._difficulty
    @difficulty.setter
    def difficulty(self, value: int) -> None:
        if (self._difficulty < value):
            self._difficulty = value

    def solve(self) -> None:
        strategies = [
            (0, self._solved),
            (1, self._ground_rules),
            (1, self._illegal_fields),
            (1, self._small_regions),
            (1, self._illegal_neighbours),
            (2, self._four_fields_rule),
            (3, self._cooupied_regions_a),
            (4, self._occupied_regions_b),
            (5, self._trial_and_error),
        ]
        changed: int = -1
        while changed:
            self.difficulty = changed
            for callback in self.on_changed:
                callback()
            changed = next((difficulty for difficulty, strategy in strategies if strategy()), 0)


    def _solved(self) -> bool:
        for group in self.grid.star_groups:
            if group.stars < group.max_stars:
                return False
        return True


    def _ground_rules(self) -> bool:
        changed = False
        for group in self.grid.all_groups:
            if group.stars == group.max_stars:
                for cell in group.cells:
                    if cell.state == State.UNKNOWN:
                        cell.state = State.EMPTY
                        changed = True
        return changed


    def _small_regions(self) -> bool:
        return any([self._small_regions_for_area(area.simplified()) for area in self.grid.areas])

    def _small_regions_for_area(self, area: Group) -> bool:
        if area.max_stars == 0:
            return False
        if len(area.cells) > area.max_stars * 3 - 2:
            return False
        solutions = [star_cells for star_cells, _ in self._find_legal_star_layouts(area)]
        if len(solutions) == 0:
            raise RuntimeError('No solution found for area.')
        changed = False
        for star_cell in area.cells:
            if all(star_cell in solution for solution in solutions):
                star_cell.state = State.STAR
                changed = True
        return changed
    

    def _illegal_fields(self) -> bool:
        return any([self._illegal_fields_for_area(area.simplified()) for area in self.grid.areas])

    def _illegal_fields_for_area(self, area: Group) -> bool:
        if area.max_stars <= 1:
            return False
        changed = False
        for cell in area.cells:
            remaining_cells = set(area.cells)
            remaining_cells.remove(cell)
            for adjacent_cell in cell.adjacent_cells:
                remaining_cells.discard(adjacent_cell)
            if not any(self._find_legal_star_layouts(Group('illegal_fields', area.max_stars - 1, remaining_cells))):
                cell.state = State.EMPTY
                changed = True
            
        return changed


    def _illegal_neighbours(self) -> bool:
        return any([self._illegal_neighbours_for_area(area.simplified()) for area in self.grid.areas])

    def _illegal_neighbours_for_area(self, area: Group) -> bool:
        changed = False
        area_adjacent_cells: Set[Cell] = set()
        for cell in area.cells:
            for adjacent_cell in cell.adjacent_cells:
                if adjacent_cell.state == State.UNKNOWN:
                    area_adjacent_cells.add(adjacent_cell)
        for cell in area.cells:
            area_adjacent_cells.discard(cell)
        for cell in area_adjacent_cells:
            remaining_cells = set(area.cells)
            for adjacent_cell in cell.adjacent_cells:
                remaining_cells.discard(adjacent_cell)
            if not any(self._find_legal_star_layouts(Group('illegal_neighbours', area.max_stars, remaining_cells))):
                cell.state = State.EMPTY
                changed = True
        return changed


    def _four_fields_rule(self) -> bool:
        return False


    def _cooupied_regions_a(self) -> bool:
        return False


    def _occupied_regions_b(self) -> bool:
        return False


    def _trial_and_error(self) -> bool:
        return False


    def _find_legal_star_layouts(self, group: Group) -> Iterable[Tuple[Tuple[Cell, ...], Set[Cell]]]:
        '''
        Find all legal star layouts for a group.

        :param group: The group to find legal star layouts for.
        :returns: An iterable of tuples, each containing:
            - a tuple of cells containing stars
            - a set of remaining unknown cells
        '''
        for star_cells in combinations(group.cells, group.max_stars):
            skip = False
            remaining_cells = set(group.cells)
            for star_cell in star_cells:
                if star_cell not in remaining_cells:
                    skip = True
                else:
                    remaining_cells.remove(star_cell)
                    for adjacent_cell in star_cell.adjacent_cells:
                        remaining_cells.discard(adjacent_cell)
            if skip:
                continue
            yield star_cells, remaining_cells
