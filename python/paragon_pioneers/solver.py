from building import Buildings
from goods import Goods
from limits_solver import LimitsSolver
from production_solver import ProductionSolver


class Solver:
    '''Calculates how many of each building are required to ensure a continuous supply chain for a
    specific set of buildings.'''


    def __init__(self, maximum_deposits: Goods, requested_buildings: Buildings) -> None:
        '''Initialises a new request to solve.
        
        Parameters
        ----------
        maximum_deposits: Goods
            A dictionary of deposits and maximum number of each that are available.

        requested_buildings: Buildings
            A dictionary of buildings and the minimum number of each that are required. It is
            assumed the full production output of each building specified is wanted, even if its
            production could supply another requested building.
        '''

        self.maximum_deposits: Goods = { good: quantity for good, quantity in maximum_deposits.items() if quantity is not None }
        '''A dictionary of deposits and the maximum number of each that are available.'''

        self.requested_buildings: Buildings = requested_buildings
        '''A dictionary of buildings and the minimum number of each that are required.'''
        
        self.maximum_buildings: Buildings = None
        '''The maximum number that can be built of each building given production chain
        limitations.'''

        self.built_buildings: Buildings = None
        '''The quantities of each building that must be built.'''

        self.net_production: Goods = None
        '''The net production of goods by the built buildings.'''


    def solve(self) -> None:
        limits_solver = LimitsSolver(self.maximum_deposits)
        self.maximum_buildings = limits_solver.solve()
        production_solver = ProductionSolver(self.requested_buildings, self.maximum_buildings)
        production_solver.solve()
        self.built_buildings = production_solver.built_buildings
        self.net_production = production_solver.net_production
