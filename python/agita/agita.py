#!/usr/bin/env python3

from itertools import combinations
from typing import Iterable, List, Literal, Optional, get_args

from colorama import Back, Style

REQUIRED_ENDING_COUNT = 2

Suit = Literal['a', 'b', 'c', 'd', 'e']
Ending = str

class Player:
    '''The current state of a Player.'''

    def __init__(self, color: str, suits: List[Suit], endings: List[Ending]):
        '''Creates a new Player object.
        
        Parameters
        ----------
        color: str
            The color used to draw this player. Also used as a unique identifier.

        suits: List[Suit]
            The suits this player can write to trigger Act 3.
            Default is all suits.

        endings: List[Ending]
            The endings this player has chosen.
            Default is an empty list.
        '''

        self.color = color
        '''The color used to draw this player. Also used as a unique identifier.'''

        self.suits = suits
        '''The suits this player can write to trigger Act 3.'''

        self.endings = endings
        '''The endings this player has chosen.'''

    def act3(self, suit: Suit, ending: Optional[Ending] = None) -> 'Player':
        '''Triggers Act 3.

        Parameters
        ----------
        suit: Suit
            The suit that triggered Act 3.

        ending: Ending | None
            The ending to take. Omit or `''` or `None` if no ending could be taken.

        Returns
        -------
        Player
            A new Player object that is a copy of this object with the specified changes.
        '''
        if suit not in self.suits: raise ValueError('Player has already triggered Act 3 with this suit.')
        if ending:
            if not self.needs_an_ending: raise ValueError('Player already has enough endings.')
            if suit in ending: raise ValueError('Player cannot take an ending matching the suit used to trigger Act 3.')
            if not self.can_take_ending(ending): raise ValueError('Player cannot take an ending that conflicts with their existing endings.')
            endings = [*self.endings, ending]
        else:
            endings = self.endings
        suits = remove(self.suits, suit)
        return Player(self.color, suits, endings)

    @property
    def needs_an_ending(self) -> bool:
        '''Checks whether this player needs an ending.

        Returns
        -------
        bool
            True if this player needs an ending.
            False if this player has their required endings.
        '''
        return len(self.endings) < REQUIRED_ENDING_COUNT

    def can_take_ending(self, ending: Ending) -> bool:
        '''Checks whether the specified ending conflicts with an ending already chosen by this player.
        
        Parameters
        ----------
        ending: Ending
            The ending to check.
        
        Returns
        -------
        bool
            True if the specified ending can be chosen.
            False if the specified ending conflicts with one of this player's endings.
        '''
        return not has_conflict(''.join(self.endings), ending)

class Game:
    '''The current state of a game.'''

    def __init__(self, endings: List[Ending], players: List[Player]):
        '''Creates a new Game object.
        
        Parameters
        ----------
        endings: List[Ending]
            The available endings.
        
        players: List[Player]
            The current state of each player.
            Default is two players in their initial state.
        '''

        self.endings = endings
        '''The available endings.'''

        self.players = players
        '''The current state of each player.'''

    def act3(self, player: Player, suit: Suit, ending: Optional[Ending] = None) -> 'Game':
        '''Triggers Act 3 for a player.

        Parameters
        ----------
        player: Player
            The player triggering Act 3.

        suit: Suit
            The suit that triggered Act 3.

        ending: Ending | None
            The ending the player chose.
            Default is the player cannot choose an ending.
        '''
        if ending:
            if ending not in self.endings: raise ValueError('Ending is not available.')
            endings = remove(self.endings, ending)
        else:
            endings = self.endings
        players = list(self.players)
        try: id = players.index(player)
        except ValueError: raise ValueError('Player is not in this game.')
        players[id] = players[id].act3(suit, ending)
        return Game(endings, players)

class Result:
    '''A game result.'''

    def successful(game: Game) -> 'Result':
        '''Creates a successful result.

        Parameters
        ----------
        game: Game
            The game state that succeeded.

        Returns
        -------
        Result
            A successful result.
        '''
        return Result(game, None)
    
    def failed(game: Game, player: Player) -> 'Result':
        '''Creates a failed result.

        Parameters
        ----------
        game: Game
            The game state that failed.

        player: Player
            The player who cannot choose enough endings.

        Returns
        -------
        Result
            A failed result.
        '''
        if (player not in game.players): raise ValueError('Player is not in the game.')
        return Result(game, player)

    def __init__(self, game: Game, player: Optional[Player]):
        '''Creates a new Result object.

        Parameters
        ----------
        game: Game
            The game state that produced this result.

        player: Player
            The player who was unable to chose enough endings, if any.
        '''

        self.message = ' '.join([ending.ljust(3).rjust(6) for ending in game.endings])
        '''This result's message.'''

        self.player = player
        '''The player who was unable to chose enough endings, if any.'''

    @property
    def is_successful(self) -> bool:
        '''True if this result is successful. False if this result is failed.'''
        return self.player is None

    def prefix(self,
        player: Player,
        suit: Suit,
        ending: Optional[Ending] = None
    ) -> 'Result':
        '''Prefixes this result's message with a player's Act 3 action.
        
        Parameters
        ----------
        player: Player
            The player who triggered Act 3.
        
        suit: Suit
            The suit that triggered Act 3.
            
        endings: Ending
            The ending the player chose, if any.
        
        Returns
        -------
        Result
            This result.
        '''
        if ending:
            self.message = f'{player.color} {suit}:{ending.ljust(2)} {Back.BLACK} {self.message}'
        else:
            self.message = f'{player.color} {suit}: {Back.BLACK}   {self.message}'
        return self

class Solver:
    def __init__(self, player_count: int, ending_count: int) -> None:
        suits = get_args(Suit)
        endings: List[Ending] = [*suits, *[suits[n] + suits[(n + 2) % len(suits)] for n in range(len(suits))]]
        def get_ending_combinations():
            return combinations(endings, ending_count)
        self._get_ending_combinations = get_ending_combinations
        PLAYER_COLORS = [Back.RED, Back.GREEN, Back.BLUE, Back.YELLOW, Back.MAGENTA, Back.CYAN, Back.WHITE]
        self._initial_players = [Player(PLAYER_COLORS[n], suits, []) for n in range(player_count)]

    def main(self) -> None:
        '''Checks all combinations of endings for the possibility of a player not being able to choose
        enough endings.
        '''
        fail_count = 0
        total = 0
        for endings in self._get_ending_combinations():
            total += 1
            game = Game(endings, self._initial_players)
            result = self._can_fail_player(game, game.players[0])
            if not result.is_successful:
                fail_count += 1
            print(result.message + Style.RESET_ALL)
        print(f'{fail_count}/{total} = {100 * fail_count / total}%')

    def _can_fail_game(self, game: Game) -> Result:
        players_needing_an_ending = [player for player in game.players if player.needs_an_ending]
        if not players_needing_an_ending:
            return Result.successful(game)
        for player in players_needing_an_ending:
            if all(not player.can_take_ending(e) for e in game.endings):
                return Result.failed(game)
            result = self._can_fail_player(game, player)
            if not result.is_successful:
                return result
        return result

    def _can_fail_player(self, game: Game, player: Player) -> Result:
        triggerable_suits = player.suits
        if not triggerable_suits:
            return Result.failed(game)
        for suit in triggerable_suits:
            result = self._can_fail_suit(game, player, suit)
            if not result.is_successful:
                return result
        return result

    def _can_fail_suit(self, game: Game, player: Player, suit: Suit) -> Result:
        endings = [ending for ending in game.endings if suit not in ending and player.can_take_ending(ending)] or [None]
        for ending in endings:
            result = self._can_fail_game(game.act3(player, suit, ending))
            result.prefix(player, suit, ending)
            if result.is_successful or result.player.color != player.color:
                return result
        return result

def has_conflict(suits1: Iterable[Suit], suits2: Iterable[Suit]) -> bool:
    '''Checks if the two sets of suits contain any common suits.

    Parameters
    ----------
    suits1: Iterable[Suit]
        One set of suits. May be a single suit or an ending.

    suits2: Iterable[Suit]
        Another set of suits. May be a single suit or an ending.

    Returns
    -------
    bool
        True if the sets have a suit in common.
        False if the sets contain only unique suits.
'''
    return any(s in suits1 for s in suits2)

def remove(array: list, *items: list) -> list:
    '''Removes one or more items from a list and returns a new list.
    
    Parameters
    ----------
    array: list
        The list to manipulate.

    items: list
        The items to remove. Each item is removed once only.

    Returns
    -------
    list
        A new list that contains all elements of `array` except for those in `items`.
    '''
    array = list(array)
    for item in items:
        array.remove(item)
    return array

if __name__ == '__main__':
    Solver(
        player_count=2,
        ending_count=6,
    ).main()
