from typing import Dict, List
from goods import *


class Building:
    '''A building that consumes and produces goods.'''

    def __init__(self, name: str, cost: Goods = {}, duration: float = None, production: Goods = {}) -> None:
        '''Initialises a new building.

        Parameters
        ----------
        name: str
            This building's name.

        cost: Dict[Good, float]
            The goods required to build this building.

        duration: float
            The number of seconds taken for one production cycle of this building.

        production: Dict[Good, float]
            The goods produced and consumed by this building each production cycle.
        '''

        self.name = name
        '''This building's name.'''

        self.cost = cost
        '''The goods required to build this building.'''

        cyclic_rate = 60 / duration if duration else None
        self.production = { good: (quantity * cyclic_rate if good.type is GoodType.cyclic else quantity) for good, quantity in production.items() }
        '''The goods produced and consumed by this building each minute.'''

    def __str__(self) -> str: return self.name
    def __repr__(self) -> str: return self.name
    
    def consumes(self, good: Good) -> bool:
        '''Gets a value indicating whether or not this building consumes the specified good.
        
        Parameters
        ----------
        good: Good
            The good to consume.
            
        Returns
        -------
        bool
            `True` if this building consumes the specified good. Otherwise, `False`.
        '''
        return self.production.get(good, 0) < 0
    
    def produces(self, good: Good) -> bool:
        '''Gets a value indicating whether or not this building produces the specified good.
        
        Parameters
        ----------
        good: Good
            The good to produce.
            
        Returns
        -------
        bool
            `True` if this building produces the specified good. Otherwise, `False`.
        '''
        return self.production.get(good, 0) > 0


Buildings = Dict[Building, float]
'''A tally of various buildings.'''


Forest = Building("Forest", {}, 2*6*12, { Sapling: -2, Tree: 2 })
Lumberjack = Building("Lumberjack", { Wood: 5 }, 12, { Tree: -1, Wood: 1 })
Forester = Building("Forester", { Wood: 10 }, 6, { Sapling: 1 })
Warehouse1 = Building("Warehouse I", { Wood: 10 })
Well = Building("Well", { Wood: 10 })
FishField = Building("Fish Field", {}, 4*80, { FishSchool: 1 })
FishermansHut = Building("Fisherman's Hut", { Wood: 10 }, 80, { FishSchool: -1, Fish: 1 })
Sawmill = Building("Sawmill", { Wood: 20 }, 12, { Wood: -1, Plank: 1 })
PotatoField = Building("Potato Field", {}, 4*60, { PotatoCrop: 1 })
PotatoFarm = Building("Potato Farm", { Wood: 10 }, 60, { PotatoCrop: -1, Schnapps: 1 })

Tavern = Building("Tavern", { Plank: 40 })
Warehouse2 = Building("Warehouse II", { Plank: 20 })
Garrison1 = Building("Garrison I", { Plank: 40 })
LinseedField = Building("Linseed Field", {}, 8*120, { LinseedCrop: 1 })
LinseedFarm = Building("Linseed Farm", { Plank: 20 }, 120, { LinseedCrop: -1, Linseed: 1 })
LinenWeaver = Building("Linen Weaver", { Plank: 40 }, 120, { Linseed: -2, Linen: 1 })
Bowyer = Building("Bowyer", { Plank: 60 }, 120, { Linseed: -1, Wood: -5, Bow: 1 })
ArcheryRange = Building("Archery Range", { Plank: 60 }, 600, { Bow: -1, Malitia: -1, Archer: 1 })
Ropery = Building("Ropery", { Plank: 40 }, 120, { Linseed: -1, Ropes: 1 })
SmallShipyard = Building("Small Shipyard", { Plank: 100 }, 240, { Plank: -10, Ropes: -10 })
Stonecutter = Building("Stonecutter", { Plank: 20 }, 420, { Stone: 1 })
CopperMine = Building("Copper Mine", { Plank: 60, Stone: 30 }, 240, { CopperDeposit: -1, CopperOre: 1 })
CopperSmelter = Building("Copper Smelter", { Plank: 40, Stone: 20 }, 480, { Wood: -10, CopperOre: -1, CopperIngot: 1 })
CopperArmory = Building("Copper Armory", { Plank: 60, Stone: 30 }, 480, { Wood: -10, CopperIngot: -1, CopperSword: 1 })
Barracks = Building("Barracks", { Plank: 60, Stone: 30 }, 600, { CopperSword: -1, Malitia: -1, Footsoldier: 1 }) # Coin: -1

SheepField = Building("Sheep Field", {}, 8*120, { SheepHeard: 1 })
SheepFarm = Building("Sheep Farm", { Coin: 20, Plank: 20, Stone: 20 }, 120, { SheepHeard: -1, Yarn: 1 })
Weaver = Building("Weaver", { Coin: 20, Plank: 40, Stone: 40 }, 120, { Yarn: -2, Fabric: 1 })
Warehouse3 = Building("Warehouse III", { Coin: 20, Plank: 40, Stone: 40 })
Garrison2 = Building("Garrison II", { Plank: 40, Stone: 40 })
WheatField = Building("Wheat Field", { Coin: 5 }, 8*120, { WheatCrop: 1 })
WheatFarm = Building("Wheat Farm", { Coin: 20, Plank: 20, Stone: 20 }, 120, { WheatCrop: -1, Wheat: 1 })
Windmill = Building("Windmill", { Coin: 20, Plank: 40, Stone: 40 }, 120, { Wheat: -2, Flour: 1 })
Bakery = Building("Bakery", { Coin: 20, Plank: 60, Stone: 60 }, 240, { Flour: -1, Bread: 1 })
HorseField = Building("Horse Field", {}, 20*360, { HorseHeard: 1 })
HorseBreeder = Building("Horse Breeder", { Coin: 20, Plank: 40, Stone: 40 }, 360, { HorseHeard: -1, Horse: 1 })
RidingArena = Building("Riding Arena", { Coin: 40, Plank: 60, Stone: 60 }, 600, { Horse: -1, Malitia: -1, Cavalry: 1 }) # Coin: -1
TobaccoField = Building("Tobacco Field", { Coin: 5 }, 8*120, { TobaccoCrop: 1 })
TobaccoFarm = Building("Tobacco Farm", { Coin: 20, Plank: 20, Stone: 20 }, 120, { TobaccoCrop: -1, Tobacco: 1 })
CigarManufacturer = Building("Cigar Manufacturer", { Coin: 20, Plank: 40, Stone: 40 }, 120, { Tobacco: -2, Cigar: 1 })
School = Building("School", { Coin: 40, Plank: 120, Stone: 120 })
Sailmaker = Building("Sailmaker", { Coin: 20, Plank: 60, Stone: 60 }, 240, { Ropes: -1, Yarn: -2, Sail: 1 })
Shipyard = Building("Shipyard", { Coin: 40, Plank: 120, Stone: 120 }, 240, { Plank: -10, Sail: -5 })
CharcoalKiln = Building("Charcoal Kiln", { Coin: 20, Plank: 40, Stone: 40 }, 240, { Wood: -20, Coal: 1 })
ClayPit = Building("Clay Pit", { Coin: 20, Plank: 40, Stone: 40 }, 120, { ClayDeposit: -1, Clay: 1 })
Brickyard = Building("Brickyard", { Coin: 20, Plank: 60, Stone: 60 }, 240, { Coal: -1, Clay: -1, Brick: 1 })
Longbowyer = Building("Longbowyer", { Coin: 40, Plank: 60, Stone: 60 }, 120, { Linen: -2, Wood: -10, Longbow: 1 })
LongbowArcheryRange = Building("Longbow Archery Range", { Coin: 40, Plank: 120, Stone: 120 }, 600, { Longbow: -1, Malitia: -1, LongbowArcher: 1 }) # Coin: -2

CottonField = Building("Cotton Field", { Coin: 25 }, 8*60, { CottonCrop: 1 })
CottonPlantation = Building("Cotton Plantation", { Coin: 100, Brick: 20 }, 60, { CottonCrop: -1, Yarn: 1 })
TextileMill = Building("Textile Mill", { Coin: 100, Brick: 40 }, 80, { Yarn: -2, Fabric: 1 })
Tailor = Building("Tailor", { Coin: 100, Brick: 60 }, 240, { Fabric: -4, Clothes: 1 })
CoalMine = Building("Coal Mine", { Coin: 100, Brick: 60 }, 120, { CoalDeposit: -1, Coal: 1 })
HopField = Building("Hop Field", { Coin: 25 }, 8*120, { HopCrop: 1 })
HopFarm = Building("Hop Farm", { Coin: 100, Brick: 20 }, 120, { HopCrop: -1, Hops: 1 })
Malthouse = Building("Malthouse", { Coin: 100, Brick: 40 }, 120, { Wheat: -2, Malt: 1 })
Brewery = Building("Brewery", { Coin: 100, Brick: 60 }, 240, { Hops: -3, Malt: -1, Beer: 2 })
LinseedOilPress = Building("Linseed Oil Press", { Coin: 100, Brick: 40 }, 180, { Linseed: -2, LinseedOil: 1 })
Medicus = Building("Medicus", { Coin: 200, Brick: 120 }, 60, { LinseedOil: -1 })
IronMine = Building("Iron Mine", { Coin: 100, Brick: 60 }, 240, { IronDeposit: -1, IronOre: 1 })
Warehouse4 = Building("Warehouse IV", { Coin: 100, Brick: 60, Tools: 20 })
IronSmelter = Building("Iron Smelter", { Coin: 100, Brick: 40 }, 960, { Coal: -1, IronOre: -2, IronIngot: 2 })
Toolmaker = Building("Toolmaker", { Coin: 100, Brick: 60 }, 960, { Coal: -1, IronIngot: -2, Tools: 4 })
IronArmory = Building("Iron Armory", { Coin: 100, Brick: 60, Tools: 20 }, 960, { Coal: -1, IronIngot: -2, IronSword: 2 })
KnightBarracks = Building("Knight Barracks", { Coin: 200, Brick: 120, Tools: 40 }, 600, { IronSword: -1, Malitia: -1, Knight: 1 }) # Coin: -4
Garrison3 = Building("Garrison III", { Brick: 120 })
CattleField = Building("Cattle Field", {}, 8*240, { CattleHeard: 1 })
CattleRanch = Building("Cattle Ranch", { Coin: 100, Brick: 20, Tools: 5 }, 240, { CattleHeard: -1, Cattle: 1 })
RockSaltMine = Building("Rock Salt Mine", { Coin: 100, Brick: 60, Tools: 20 }, 240, { HaliteDeposit: -1, RockSalt: 1 })
SaltWorks = Building("Salt Works", { Coin: 100, Brick: 40, Tools: 10 }, 480, { Coal: -1, RockSalt: -2, Salt: 6 })
SaltField = Building("Salt Field", {}, 2*480, { SaltWater: 1 })
Saltern = Building("Saltern", { Coin: 100, Brick: 40, Tools: 10 }, 480, { SaltWater: -1, Salt: 1 })
ButchersShop = Building("Butcher's Shop", { Coin: 100, Brick: 60, Tools: 20 }, 240, { Salt: -1, Cattle: -2, Meat: 4 })
GoldMine = Building("Gold Mine", { Coin: 100, Brick: 60, Tools: 20 }, 240, { GoldDeposit: -1, GoldOre: 1 })
GoldSmelter = Building("Gold Smelter", { Coin: 100, Brick: 40, Tools: 10 }, 960, { Coal: -1, GoldOre: -2, GoldIngot: 2 })
Goldsmith = Building("Goldsmith", { Coin: 100, Brick: 60, Tools: 20 }, 960, { Coal: -1, GoldIngot: -2, GoldJewelry: 2 })
CrossbowMaker = Building("Crossbow Maker", { Coin: 200, Brick: 120, Tools: 40 }, 240, { Linen: -2, IronIngot: -1, Crossbow: 1 })
CrossbowShootingRange = Building("Crossbow Shooting Range", { Coin: 200, Brick: 120, Tools: 40 }, 600, { Crossbow: -1, Malitia: -1, Crossbowman: 1 }) # Coin: -4
GreatShipyard = Building("Great Shipyard", { Coin: 200, Brick: 120, Tools: 40 }, 240, { Plank: -10, Sail: -5, Tools: -5 })
Bathhouse = Building("Bathhouse", { Coin: 200, Brick: 120, Tools: 40 })
MarbleQuarry = Building("Marble Quarry", { Coin: 100, Brick: 60, Tools: 20 }, 240, { MarbleDeposit: -1, Marble: 1 })

MulberryTrees = Building("Mulberry Trees", { Coin: 100 }, 8*120, { SilkCrop: 1 })
SilkPlantation = Building("Silk Plantation", { Coin: 300, Marble: 20, Tools: 10 }, 120, { SilkCrop: -1, Silk: 1 })
SilkTwineMill = Building("Silk Twine Mill", { Coin: 300, Marble: 40, Tools: 20 }, 120, { Silk: -2, SilkCloth: 1 })
IndigoField = Building("Indigo Field", { Coin: 100 }, 8*120, { IndigoCrop: 1 })
IndigoPlantation = Building("Indigo Plantation", { Coin: 300, Marble: 20, Tools: 10 }, 120, { IndigoCrop: -1, Dye: 1 })
NobleTailor = Building("Noble Tailor", { Coin: 300, Marble: 60, Tools: 30 }, 240, { Dye: -1, SilkCloth: -1, Garment: 1 })
GemstoneMine = Building("Gemstone Mine", { Coin: 300, Marble: 60, Tools: 60 }, 180, { GemstoneDeposit: -1, Gemstone: 1 })
GobletManufacturer = Building("Goblet Manufacturer", { Coin: 300, Marble: 40, Tools: 20 }, 480, { Gemstone: -1, GoldIngot: -1, Goblet: 2 })
Garrison4 = Building("Garrison IV", { Marble: 240, Tools: 0 })
Warehouse5 = Building("Warehouse V", { Coin: 300, Marble: 60, Tools: 30 })
Tiltyard = Building("Tiltyard", { Coin: 600, Marble: 120, Tools: 60 }, 180, { Horse: -1 })
HoneyField = Building("Honey Field", {}, 8*240, { HoneyCrop: 1 })
Apiary = Building("Apiary", { Coin: 300, Marble: 20, Tools: 10 }, 240, { HoneyCrop: -1, Honey: 1 })
Chandler = Building("Chandler", { Coin: 300, Marble: 40, Tools: 20 }, 240, { Honey: -2, Linseed: -1, Candle: 2 })
FineForge = Building("Fine Forge", { Coin: 300, Marble: 60, Tools: 30 }, 240, { CopperIngot: -1, Candle: -3, CandleHolder: 3 })
ConcertHall = Building("Concert Hall", { Coin: 600, Marble: 120, Tools: 60 })
Cokery = Building("Cokery", { Coin: 300, Marble: 40, Tools: 20 }, 240, { Coal: -2, Coke: 1 })
SteelFurnace = Building("Steel Furnace", { Coin: 300, Marble: 40, Tools: 20 }, 240, { Coke: -1, IronIngot: -1, SteelIngot: 1 })
Armorsmith = Building("Armorsmith", { Coin: 300, Marble: 40, Tools: 20 }, 240, { Horse: -1, SteelIngot: -1, ArmoredHorse: 1 })
CuirassierAcademy = Building("Cuirassier Academy", { Coin: 600, Marble: 120, Tools: 60 }, 600, { ArmoredHorse: -1, Malitia: -1, Cuirassier: 1 }) # Coin: -5
Distillery = Building("Distillery", { Coin: 300, Marble: 40, Tools: 20 }, 120, { Honey: -1, Schnapps: -2, Liqueur: 1 })
Lobsterer = Building("Lobsterer", { Coin: 300, Marble: 20, Tools: 10 }, 40, { Reef: -1, Lobster: 1 })
NobleKitchen = Building("Noble Kitchen", { Coin: 300, Marble: 60, Tools: 30 }, 80, { Liqueur: -1, Lobster: -2, Feast: 1 })
Vineyard = Building("Vineyard", { Coin: 100 }, 12*80, { GrapeCrop: 1 })
Winery = Building("Winery", { Coin: 300, Marble: 20, Tools: 10 }, 80, { GrapeCrop: -1, Grapes: 1 })
Cooper = Building("Cooper", { Coin: 300, Marble: 40, Tools: 20 }, 240, { IronIngot: -1, Plank: -16, Barrel: 3 })
Winepress = Building("Winepress", { Coin: 300, Marble: 60, Tools: 30 }, 240, { Barrel: -2, Grapes: -4, Wine: 3 })
PaperMill = Building("Paper Mill", { Coin: 300, Marble: 40, Tools: 20 }, 120, { Wood: -16, Paper: 1 })
Bookbinder = Building("Bookbinder", { Coin: 300, Marble: 40, Tools: 20 }, 480, { Dye: -2, Paper: -4, Book: 1 })
University = Building("University", { Coin: 600, Marble: 120, Tools: 60 }, 240, { Book: -1 })
NitrateMaker = Building("Nitrate Maker", { Coin: 300, Marble: 20, Tools: 10 }, 240, { SaltpeterDeposit: -1, Saltpeter: 1 })
PowderMill = Building("Powder Mill", { Coin: 300, Marble: 40, Tools: 20 }, 240, { Coal: -1, Saltpeter: -1, Gunpowder: 1 })
CannonFoundry = Building("Cannon Foundry", { Coin: 300, Marble: 60, Tools: 30 }, 480, { Gunpowder: -1, SteelIngot: -1, Cannon: 1 })
CannoneersSchool = Building("Cannoneer's School", { Coin: 600, Marble: 120, Tools: 60 }, 600, { Cannon: -1, Malitia: -1, Cannoneer: 1 }) # Coin: -5
AdvancedShipyard = Building("Advanced Shipyard", { Coin: 900, Marble: 120, Tools: 60 }, 240, { Plank: -10, Sail: -10, SteelIngot: -5 })
PlaningMill = Building("Planing Mill", { Coin: 300, Marble: 40, Tools: 20 }, 240, { IronIngot: -1, CopperIngot: -1, MetalCuttings: 2 })
Gunsmith = Building("Gunsmith", { Coin: 300, Marble: 60, Tools: 30 }, 240, { Gunpowder: -2, MetalCuttings: -2, Fireworks: 1 })

Kontor1 = Building("Kontor", { Wood: 40 })
Kontor2 = Building("Kontor II", { Coin: 20, Plank: 50 })
Kontor3 = Building("Kontor III", { Coin: 100, Plank: 100, Stone: 100 })
Kontor4 = Building("Kontor IV", { Coin: 300, Brick: 200, Tools: 100 })
Kontor5 = Building("Kontor V", { Coin: 900, Marble: 240, Tools: 240 })

PalaceFoundation = Building("Palace Foundation", { Coin: 10000, Plank: 1000, Tools: 1000 })
Palace1 = Building("Palace I", { Coin: 20000, Plank: 2000, Stone: 2000, Tools: 2000 })
Palace2 = Building("Palace II", { Coin: 20000, Brick: 2000, Marble: 2000, Tools: 2000 })
Palace3 = Building("Palace III", { Coin: 30000, Brick: 3000, Marble: 3000, Tools: 3000 })
Palace4 = Building("Palace IV", { Coin: 40000, GoldIngot: 4000, SteelIngot: 4000, Tools: 4000 })
Palace5 = Building("Palace V", { Fireworks: 1000 })

PioneersHut = Building("Pioneer's Hut", { Wood: 10 }, 3600, {
    Pioneer: 10,
    Fish: -7.5,
    Schnapps: -5,
    Malitia: 1.5,
})
ColonistsHouse = Building("Colonist's House", { Plank: 20 }, 3600, {
    Colonist: 15,
    Fish: -22.5,
    Schnapps: -20,
    Linen: -5,
    Coin: 1.6875,
})
TownsmensHouse = Building("Townsmen's House", { Plank: 30, Stone: 30 }, 3600, {
    Townsman: 20,
    Fish: -30,
    Schnapps: -30,
    Linen: -7.5,
    Fabric: -5,
    Bread: -4.8,
    Cigar: -3.2,
    Coin: 10,
})
MerchantsMansion = Building("Merchant's Mansion", { Brick: 40 }, 3600, {
    Merchant: 25,
    Fish: -37.5,
    Schnapps: -37.5,
    Cigar: -4,
    Bread: -6,
    Clothes: -3,
    Beer: -6,
    Meat: -12,
    GoldJewelry: -2,
    Coin: 50,
})
ParagonsResidence = Building("Paragon's Residence", { Marble: 40, Tools: 20 }, 3600, {
    Paragon: 30,
    Cigar: -4.8,
    Clothes: -3.6,
    Beer: -7.2,
    Meat: -14.4,
    Garment: -7.06,
    Goblet: -3.53,
    CandleHolder: -5.29,
    Feast: -10.59,
    Wine: -7.94,
    Favor: 2.808,
})

AllBuildings: List[Building] = [
    Forest, Lumberjack, Forester, Warehouse1, Well, FishField, FishermansHut, Sawmill, PotatoField, PotatoFarm,
    Tavern, Warehouse2, Garrison1, LinseedField, LinseedFarm, LinenWeaver, Bowyer, ArcheryRange, Ropery, SmallShipyard, Stonecutter, CopperMine, CopperSmelter, CopperArmory, Barracks,
    SheepField, SheepFarm, Weaver, Warehouse3, Garrison2, WheatField, WheatFarm, Windmill, Bakery, HorseField, HorseBreeder, RidingArena, TobaccoField, TobaccoFarm, CigarManufacturer, School, Sailmaker, Shipyard, CharcoalKiln, ClayPit, Brickyard, Longbowyer, LongbowArcheryRange,
    CottonField, CottonPlantation, TextileMill, Tailor, CoalMine, HopField, HopFarm, Malthouse, Brewery, LinseedOilPress, Medicus, IronMine, Warehouse4, IronSmelter, Toolmaker, IronArmory, KnightBarracks, Garrison3, CattleField, CattleRanch, RockSaltMine, SaltWorks, SaltField, Saltern, ButchersShop, GoldMine, GoldSmelter, Goldsmith, CrossbowMaker, CrossbowShootingRange, GreatShipyard, Bathhouse, MarbleQuarry,
    MulberryTrees, SilkPlantation, SilkTwineMill, IndigoField, IndigoPlantation, NobleTailor, GemstoneMine, GobletManufacturer, Garrison4, Warehouse5, Tiltyard, HoneyField, Apiary, Chandler, FineForge, ConcertHall, Cokery, SteelFurnace, Armorsmith, CuirassierAcademy, Distillery, Lobsterer, NobleKitchen, Vineyard, Winery, Cooper, Winepress, PaperMill, Bookbinder, University, NitrateMaker, PowderMill, CannonFoundry, CannoneersSchool, AdvancedShipyard, PlaningMill, Gunsmith,
    Kontor1, Kontor2, Kontor3, Kontor4, Kontor5,
    PioneersHut, ColonistsHouse, TownsmensHouse, MerchantsMansion, ParagonsResidence,
    PalaceFoundation, Palace1, Palace2, Palace3, Palace4, Palace5,
]

__all__ = [
    "Building", "Buildings", "AllBuildings",
    "Forest", "Lumberjack", "Forester", "Warehouse1", "Well", "FishField", "FishermansHut", "Sawmill", "PotatoField", "PotatoFarm",
    "Tavern", "Warehouse2", "Garrison1", "LinseedField", "LinseedFarm", "LinenWeaver", "Bowyer", "ArcheryRange", "Ropery", "SmallShipyard", "Stonecutter", "CopperMine", "CopperSmelter", "CopperArmory", "Barracks",
    "SheepField", "SheepFarm", "Weaver", "Warehouse3", "Garrison2", "WheatField", "WheatFarm", "Windmill", "Bakery", "HorseField", "HorseBreeder", "RidingArena", "TobaccoField", "TobaccoFarm", "CigarManufacturer", "School", "Sailmaker", "Shipyard", "CharcoalKiln", "ClayPit", "Brickyard", "Longbowyer", "LongbowArcheryRange",
    "CottonField", "CottonPlantation", "TextileMill", "Tailor", "CoalMine", "HopField", "HopFarm", "Malthouse", "Brewery", "LinseedOilPress", "Medicus", "IronMine", "Warehouse4", "IronSmelter", "Toolmaker", "IronArmory", "KnightBarracks", "Garrison3", "CattleField", "CattleRanch", "RockSaltMine", "SaltWorks", "SaltField", "Saltern", "ButchersShop", "GoldMine", "GoldSmelter", "Goldsmith", "CrossbowMaker", "CrossbowShootingRange", "GreatShipyard", "Bathhouse", "MarbleQuarry",
    "MulberryTrees", "SilkPlantation", "SilkTwineMill", "IndigoField", "IndigoPlantation", "NobleTailor", "GemstoneMine", "GobletManufacturer", "Garrison4", "Warehouse5", "Tiltyard", "HoneyField", "Apiary", "Chandler", "FineForge", "ConcertHall", "Cokery", "SteelFurnace", "Armorsmith", "CuirassierAcademy", "Distillery", "Lobsterer", "NobleKitchen", "Vineyard", "Winery", "Cooper", "Winepress", "PaperMill", "Bookbinder", "University", "NitrateMaker", "PowderMill", "CannonFoundry", "CannoneersSchool", "AdvancedShipyard", "PlaningMill", "Gunsmith",
    "Kontor1", "Kontor2", "Kontor3", "Kontor4", "Kontor5",
    "PioneersHut", "ColonistsHouse", "TownsmensHouse", "MerchantsMansion", "ParagonsResidence",
    "PalaceFoundation", "Palace1", "Palace2", "Palace3", "Palace4", "Palace5",
]
