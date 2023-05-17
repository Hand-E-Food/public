from typing import Dict


class GoodType:
    cyclic: 'GoodType' = 'cyclic'
    infinite: 'GoodType' = 'infinite'
    resident: 'GoodType' = 'resident'


class Good:
    '''A good that can be produced, consumed, or simply exists.'''

    def __init__(self, name: str, type: GoodType) -> None:
        '''Initialises a new good.
        
        Parameters
        ----------
        name: str
            This good's name.
        
        is_cyclic: bool
            `True` if this good is produced and consumed cycicly.
            `False` if this good exists perpetually.
        '''

        self.name = name
        '''This good's name.'''

        self.type: GoodType = type
        '''
        "cyclic" if this good is produced and consumed cyclicly.
        "infinite" if this good can be reused infinitely.
        "resident" if this good exists perpetually.
        '''

    def __str__(self) -> str: return self.name
    def __repr__(self) -> str: return self.name


Goods = Dict[Good, float]
'''A tally of various goods.'''


def civilian(name: str) -> Good: return Good(name, GoodType.resident)
def crop(name: str) -> Good: return Good(name, GoodType.cyclic)
def deposit(name: str) -> Good: return Good(name, GoodType.infinite)
def good(name: str) -> Good: return Good(name, GoodType.cyclic)
def soldier(name: str) -> Good: return Good(name, GoodType.cyclic)


Sapling = crop('sapling')
Tree = crop('tree')
FishSchool = crop('fish school')
SheepHeard = crop('sheep heard')
HorseHeard = crop('horse heard')
CattleHeard = crop('cattle heard')
SaltWater = crop('salt water')

PotatoCrop = crop('potato crop')
LinseedCrop = crop('linseed crop')
WheatCrop = crop('wheat crop')
TobaccoCrop = crop('tobacco crop')
HopCrop = crop('hops crop')
CottonCrop = crop('cotton crop')
SilkCrop = crop('silk crop')
IndigoCrop = crop('indigo crop')
HoneyCrop = crop('honey crop')
GrapeCrop = crop('grape crop')

CopperDeposit = deposit('copper deposit')
ClayDeposit = deposit('clay deposit')
CoalDeposit = deposit('coal deposit')
HaliteDeposit = deposit('halite deposit')
IronDeposit = deposit('iron deposit')
GoldDeposit = deposit('gold deposit')
MarbleDeposit = deposit('marble deposit')
GemstoneDeposit = deposit('gemstone deposit')
Reef = deposit('reef deposit')
SaltpeterDeposit = deposit('saltpeter deposit')

Coin = good('coin')
Favor = good('favor')

Wood = good('wood')
Fish = good('fish')
Plank = good('plank')
Schnapps = good('schnapps')
Stone = good('stone')
Linseed = good('linseed')
Linen = good('linen')
Bow = good('bow')
Ropes = good('ropes')
CopperOre = good('copper ore')
CopperIngot = good('copper ingot')
CopperSword = good('copper sword')
Yarn = good('yarn')
Fabric = good('fabric')
Wheat = good('wheat')
Flour = good('flour')
Bread = good('bread')
Tobacco = good('tobacco')
Cigar = good('cigar')
Sail = good('sail')
Horse = good('horse')
Longbow = good('longbow')
Clay = good('clay')
Brick = good('brick')
Clothes = good('clothes')
Malt = good('malt')
Hops = good('hops')
Beer = good('beer')
Cattle = good('cattle')
RockSalt = good('rock salt')
Salt = good('salt')
Meat = good('meat')
Coal = good('coal')
GoldOre = good('gold ore')
GoldIngot = good('gold ingot')
GoldJewelry = good('gold jewelry')
LinseedOil = good('linseed oil')
IronOre = good('iron ore')
IronIngot = good('iron ingot')
IronSword = good('iron sword')
Tools = good('tools')
Crossbow = good('crossbow')
Marble = good('marble')
Silk = good('silk')
SilkCloth = good('silk cloth')
Dye = good('dye')
Garment = good('garment')
Gemstone = good('gemstone')
Goblet = good('goblet')
Honey = good('honey')
Candle = good('candle')
CandleHolder = good('candle holder')
Liqueur = good('liqueur')
Lobster = good('lobster')
Feast = good('feast')
Grapes = good('grapes')
Barrel = good('barrel')
Wine = good('wine')
Paper = good('paper')
Book = good('book')
Coke = good('coke')
SteelIngot = good('steel ingot')
ArmoredHorse = good('armored horse')
Saltpeter = good('saltpeter')
Gunpowder = good('gunpowder')
Cannon = good('cannon')
MetalCuttings = good('metal cuttings')
Fireworks = good('fireworks')

Malitia = soldier('malitia')
Archer = soldier('archer')
Footsoldier = soldier('footsoldier')
Cavalry = soldier('cavalry')
LongbowArcher = soldier('longbow archer')
Knight = soldier('knight')
Crossbowman = soldier('crossbowman')
Cuirassier = soldier('cuirassier')
Cannoneer = soldier('cannoneer')

Pioneer = civilian('pioneer')
Colonist = civilian('colonist')
Townsman = civilian('townsman')
Merchant = civilian('merchant')
Paragon = civilian('paragon')

AllGoods = [
    Sapling, Tree, FishSchool, SheepHeard, HorseHeard, CattleHeard, SaltWater,
    PotatoCrop, LinseedCrop, WheatCrop, TobaccoCrop, HopCrop, CottonCrop, SilkCrop, IndigoCrop, HoneyCrop, GrapeCrop,
    CopperDeposit, ClayDeposit, CoalDeposit, HaliteDeposit, IronDeposit, GoldDeposit, MarbleDeposit, GemstoneDeposit, Reef, SaltpeterDeposit,
    Coin, Favor,
    Wood, Fish, Plank, Schnapps,
    Stone, Linseed, Linen, Bow, Ropes, CopperOre, CopperIngot, CopperSword,
    Yarn, Fabric, Wheat, Flour, Bread, Tobacco, Cigar, Sail, Horse, Longbow, Clay, Brick,
    Clothes, Malt, Hops, Beer, Cattle, RockSalt, Salt, Meat, Coal, GoldOre, GoldIngot, GoldJewelry, LinseedOil, IronOre, IronIngot, IronSword, Tools, Crossbow, Marble,
    Silk, SilkCloth, Dye, Garment, Gemstone, Goblet, Honey, Candle, CandleHolder, Liqueur, Lobster, Feast, Grapes, Barrel, Wine, Paper, Book, Coke, SteelIngot, ArmoredHorse, Saltpeter, Gunpowder, Cannon, MetalCuttings, Fireworks,
    Malitia, Archer, Footsoldier, Cavalry, LongbowArcher, Knight, Crossbowman, Cuirassier, Cannoneer,
    Pioneer, Colonist, Townsman, Merchant, Paragon,
]


__all__ = [
    "GoodType", "Good", "Goods", "AllGoods",
    "Sapling", "Tree", "FishSchool", "SheepHeard", "HorseHeard", "CattleHeard", "SaltWater",
    "PotatoCrop", "LinseedCrop", "WheatCrop", "TobaccoCrop", "HopCrop", "CottonCrop", "SilkCrop", "IndigoCrop", "HoneyCrop", "GrapeCrop",
    "CopperDeposit", "ClayDeposit", "CoalDeposit", "HaliteDeposit", "IronDeposit", "GoldDeposit", "MarbleDeposit", "GemstoneDeposit", "Reef", "SaltpeterDeposit",
    "Coin", "Favor",
    "Wood", "Fish", "Plank", "Schnapps",
    "Stone", "Linseed", "Linen", "Bow", "Ropes", "CopperOre", "CopperIngot", "CopperSword",
    "Yarn", "Fabric", "Wheat", "Flour", "Bread", "Tobacco", "Cigar", "Sail", "Horse", "Longbow", "Clay", "Brick",
    "Clothes", "Malt", "Hops", "Beer", "Cattle", "RockSalt", "Salt", "Meat", "Coal", "GoldOre", "GoldIngot", "GoldJewelry", "LinseedOil", "IronOre", "IronIngot", "IronSword", "Tools", "Crossbow", "Marble",
    "Silk", "SilkCloth", "Dye", "Garment", "Gemstone", "Goblet", "Honey", "Candle", "CandleHolder", "Liqueur", "Lobster", "Feast", "Grapes", "Barrel", "Wine", "Paper", "Book", "Coke", "SteelIngot", "ArmoredHorse", "Saltpeter", "Gunpowder", "Cannon", "MetalCuttings", "Fireworks",
    "Malitia", "Archer", "Footsoldier", "Cavalry", "LongbowArcher", "Knight", "Crossbowman", "Cuirassier", "Cannoneer",
    "Pioneer", "Colonist", "Townsman", "Merchant", "Paragon",
]