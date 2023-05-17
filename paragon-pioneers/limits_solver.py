import math
from typing import List

from building import AllBuildings, Building, Buildings, CoalMine, RockSaltMine, SaltWorks
from goods import Good, Goods


class LimitsSolver:
    '''Calculates the maximum number of buildings that can usefully consume a limited number of
    deposits.'''


    def __init__(self, maximum_deposits: Goods) -> None:
        '''Creates a new Limits Solver.
        
        Parameters
        ----------
        maximum_deposits: Goods
            The maximum number of each deposit that exists. `None` values are assumed to be
            unlimited.
        '''

        self.maximum_deposits: Goods = { good: quantity for good, quantity in maximum_deposits.items() if quantity is not None }
        '''The maximum number of each deposit that exists.'''

        self.maximum_buildings: Buildings = {}
        '''The maximum number that can be usefully built of each building.'''


    def solve(self) -> Buildings:
        '''Solve the maximum number of buildings that can usefully consume a limited number of
        deposits.

        Returns
        -------
        Buildings
            The maximum number of useful buildings that can be built due to the limited production
            chian.
        '''
        for good in self.maximum_deposits:
            maximum = self.maximum_deposits[good]
            self._restrict_good(good, maximum)
        return self.maximum_buildings


    def _restrict_good(self, good: Good, maximum: float) -> None:
        '''Restricts how much of a godd can be produced and calculates down-stream effects.

        Parmaeters
        ----------
        good: Good
            The good to restrict.

        maximum: float
            The maximum quantity that can be produced.
        '''
        if maximum == math.inf:
            return
        consumers = self._get_consumers_of(good)
        for consumer in consumers:
            self._restrict_building(consumer, maximum / -consumer.production[good])


    def _restrict_building(self, building: Building, maximum: float) -> None:
        '''Restricts how many of a building can be built and calculates down-stream effects.

        Parameters
        ----------
        building: Building
            The building to restrict.

        maximum: float
            The maximum quantity of this building that can be built.
        '''
        if self.maximum_buildings.get(building, math.inf) < maximum:
            return
        self.maximum_buildings[building] = maximum
        production = [ good for good in building.production if building.production[good] > 0 ]
        for good in production:
            self._assess_good(good)


    def _assess_good(self, good: Good) -> None:
        '''Calculates the maximum possible production of the specified good and applies
        restrictions.

        Parameters
        ----------
        good: Good
            The good to assess.
        '''
        producers = self._get_producers_of(good)
        if not producers:
            return
        maximum = sum(producer.production[good] * self.maximum_buildings.get(producer, math.inf) for producer in producers)
        self._restrict_good(good, maximum)


    def _get_consumers_of(self, good: Good) -> List[Building]:
        '''Gets all buildings that consume the specified good.

        Parameters
        ----------
        good: Good
            The good to consume.

        Returns
        -------
        List[Building]
            The buildings that consume the good.
        '''
        return [ building for building in AllBuildings if building.consumes(good) ]


    def _get_producers_of(self, good: Good) -> List[Building]:
        '''Gets all buildings that produce the specified good.

        Parameters
        ----------
        good: Good
            The good to produce.

        Returns
        -------
        List[Building]
            The buildings that produce the good.
        '''
        return [ building for building in AllBuildings if building.produces(good) ]
