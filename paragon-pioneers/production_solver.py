import math
from typing import List, Union

from building import AllBuildings, Building, Buildings
from goods import AllGoods, Good, Goods, GoodType


class InsufficientProductionError(Exception):
    '''Raised when it is impossible to build enough buildings to produce the required goods.'''
    ...


class ProductionSolver:
    '''Calculates how many of each building are required to ensure a continuous supply chain for a
    specific set of buildings.'''


    def __init__(self, requested_buildings: Buildings, maximum_buildings: Buildings = {}) -> None:
        '''Initialises a solver.
        
        Parameters
        ----------
        requested_buildings: Buildings
            A dictionary of buildings and the minimum number of each that are required. It is
            assumed the full production output of each building specified is wanted, even if its
            production could supply another requested building.

        maximum_buildings: Buildings
            The maximum number that can be built of each building. Any building not listed is
            assumed to have no limit.
        '''

        self.net_production: Goods = { good: math.inf if good.type is GoodType.infinite else 0 for good in AllGoods }
        '''The net production of goods by the built buildings.'''

        self.built_buildings: Buildings = { building: 0 for building in AllBuildings }
        '''The quantities of each building that must be built.'''

        self.maximum_buildings = maximum_buildings
        '''The maximum number that can be built of each building. Any building not listed is
        assumed to have no limit.'''

        for building in requested_buildings:
            self._build(building, requested_buildings[building])


    def solve(self) -> None:
        '''Generates a solution that ensures a continuous supply chain for the required buildings.

        Raises
        ------
        InsufficientProductionError
            It is impossible to build enough buildings to produce the required goods.
        '''
        while True:
            good = self._find_defecit()
            if good is None:
                break
            producers = self._get_producers_of(good)
            if not producers:
                raise InsufficientProductionError(good.name)
            producer = producers[0]
            maximum = self.maximum_buildings.get(producer, math.inf) - self.built_buildings[producer]
            quantity = min(maximum, -self.net_production[good] / producer.production[good])
            self._build(producer, quantity)

        self._round_up()


    def _find_defecit(self) -> Union[Good, None]:
        '''Finds a cyclic good that has a negative net production.
        
        Returns
        -------
        Union[Good, None]
            A cyclic good with a negative net production.
            `None` if all goods have a balanced or positive net production.
        '''
        return next((good for good in self.net_production if self.net_production[good] < 0 and good.type is GoodType.cyclic), None)


    def _get_producers_of(self, good: Good) -> List[Building]:
        '''Gets the buildings that produce the specified good, in order of fastest production. Any
        building that has already had their maximum number built are excluded from the results.
        
        Parameters
        ----------
        good: Good
            The good to produce.
        
        Returns
        -------
        List[Building]
            The buildings that produce the good, in order of fastest production.
        '''
        return list(sorted(
            (
                building
                for building
                in AllBuildings
                if building.produces(good)
                and self.maximum_buildings.get(building, math.inf) > self.built_buildings[building]
            ),
            key=lambda building: building.production[good],
            reverse=True
        ))


    def _round_up(self) -> None:
        '''Ensures an integral amount of each building is being built.'''
        for building in self.built_buildings:
            quantity = self.built_buildings[building]
            quantity = math.ceil(quantity) - quantity
            self._build(building, quantity)


    def _build(self, building: Building, quantity: float) -> None:
        '''Builds a quantity of a building.
        
        Parameters
        ----------
        bulding: Building
            The building to build.
        
        quantity: float
            The quantity to build of the building.
        '''

        self.built_buildings[building] += quantity
        for good in building.production:
            self.net_production[good] += building.production[good] * quantity
