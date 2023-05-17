#!/usr/bin/env python3
import math
from building import *
from goods import ClayDeposit, CoalDeposit, CopperDeposit, GemstoneDeposit, GoldDeposit, GoodType, Goods, HaliteDeposit, IronDeposit, MarbleDeposit, Reef, SaltpeterDeposit
from solver import Solver


solver = Solver(
    maximum_deposits = {
        ClayDeposit: None,
        CoalDeposit: 0,
        CopperDeposit: None,
        GemstoneDeposit: None,
        GoldDeposit: None,
        HaliteDeposit: 0,
        IronDeposit: None,
        MarbleDeposit: None,
        Reef: None,
        SaltpeterDeposit: None,
    },
    requested_buildings = {
        PioneersHut: 0,
        ColonistsHouse: 0,
        TownsmensHouse: 0,
        MerchantsMansion: 18,
        ParagonsResidence: 39,
        Well: 0,
        Tavern: 0,
        School: 5,
        Medicus: 2,
        Bathhouse: 4,
        ConcertHall: 1,
        Tiltyard: 1,
        University: 1,
    },
)


def print_maximum_buildings(maximum_buildings: Buildings) -> None:
    for building in maximum_buildings:
        quantity = maximum_buildings[building]
        if quantity != 0:
            print(f'{quantity:8.0f} x {building.name}')
    print()


def print_built_buildings(built_buildings: Buildings) -> None:
    for building in built_buildings:
        quantity = math.ceil(built_buildings[building])
        if quantity != 0:
            print(f'{quantity:8} x {building.name}')
    print()


def print_net_production(net_production: Goods) -> None:
    for good in net_production:
        if good.type is GoodType.infinite:
            continue
        elif good.type is GoodType.cyclic:
            unit = '/m'
        elif good.type is GoodType.resident:
            unit = '  '
        else:
            raise ValueError('good.type')
        quantity = net_production[good]
        if quantity != 0:
            print(f'{quantity:+8.2f}{unit} {good.name}')
    print()


solver.solve()
#print_maximum_buildings(solver.maximum_buildings)
print_built_buildings(solver.built_buildings)
#print_net_production(solver.net_production)
