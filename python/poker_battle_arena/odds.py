#!/usr/bin/env python3

from itertools import combinations
import sys
from typing import Dict, Iterable, List, Tuple

HEART = 'x'

NUMBER = 0
SUIT = 1
Joker = (-1, -1)
type Card = Tuple[int, int]
type Cards = Tuple[Card, Card, Card, Card, Card]

class Hand:
    def __init__(self, cards: Cards):
        self.jokers = sum(1 for card in cards if card == Joker)
        self.non_jokers = [card for card in cards if card != Joker]

        self.numbers = set(card[NUMBER] for card in self.non_jokers)
        self.number_counts = sorted(Hand.count_unique(card[NUMBER] for card in self.non_jokers).values(), reverse=True)

        suit_groups = Hand.count_unique(card[SUIT] for card in self.non_jokers)
        self.best_suit = sorted(suit_groups.items(), key=lambda item: item[1], reverse=True)[0][0]
        self.best_suit_count = suit_groups[self.best_suit]

        self.flush_numbers = set(card[NUMBER] for card in self.non_jokers if card[SUIT] == self.best_suit)

    @staticmethod
    def count_unique(items: Iterable[int]) -> Dict[int, int]:
        result: Dict[int, int] = {}
        for item in items:
            result[item] = result.get(item, 0) + 1
        return result

class Army:
    def __init__(self, sample: str, name: str) -> None:
        self.sample = sample
        self.name = name
        self.count = 0

    def is_valid(self, hand: Hand) -> bool:
        raise NotImplementedError()

class Single(Army):
    def __init__(self) -> None:
        super().__init__(' 9', 'Single')

    def is_valid(self, hand: Hand) -> bool:
        return True    

class Kind(Army):
    def __init__(self, name: str, *size: int) -> None:
        sample = ''.join(f' {9-i*2}' * size[i] for i in range(len(size)))
        super().__init__(sample, name)
        self.size = size

    def is_valid(self, hand: Hand) -> bool:
        jokers = hand.jokers
        for i in range(len(self.size)):
            size = self.size[i]
            count = hand.number_counts[i] if i < len(hand.number_counts) else 0
            if count + jokers < size:
                return False
            if count < size:
                jokers -= size - count
        return True

class Flush(Army):
    def __init__(self, name: str, size: int) -> None:
        sample = f'{HEART} ' * size
        super().__init__(sample, name)
        self.size = size

    def is_valid(self, hand: Hand) -> bool:
        return hand.best_suit_count + hand.jokers >= self.size

class Straight(Army):
    def __init__(self, name: str, size: int) -> None:
        sample = ''.join(f' {9-i}' for i in range(size))
        super().__init__(sample, name)
        self.size = size

    def is_valid(self, hand: Hand) -> bool:
        jokers = hand.jokers

        for start in range(14 - self.size):
            missing = 0
            for offset in range(self.size):
                number = start + offset
                if number not in hand.numbers:
                    missing += 1
            if missing <= jokers:
                return True
        return False

class StraightFlush(Army):
    def __init__(self, name: str, size: int) -> None:
        sample = ''.join(f'{HEART}{9-i}' for i in range(size))
        super().__init__(sample, name)
        self.size = size

    def is_valid(self, hand: Hand) -> bool:
        jokers = hand.jokers

        for start in range(14 - self.size):
            missing = 0
            for offset in range(self.size):
                number = start + offset
                if number not in hand.flush_numbers:
                    missing += 1
            if missing <= jokers:
                return True
        return False

def main(jokers: int) -> None:
    cards = [(number, suit) for number in range(13) for suit in range(4)]
    cards.extend(Joker for _ in range(jokers))
    armies: List[Army] = [
        Single(),
        Kind('Pair', 2),
        Kind('Three of a Kind', 3),
        Kind('Four of a Kind', 4),
        Kind('Five of a Kind', 5),
        Kind('Two Pair', 2, 2),
        Kind('Full House', 3, 2),
        Flush('Flush of Three', 3),
        Flush('Flush of Four', 4),
        Flush('Flush of Five', 5),
        Straight('Straight of Three', 3),
        Straight('Straight of Four', 4),
        Straight('Straight of Five', 5),
        StraightFlush('Straight Flush of Three', 3),
        StraightFlush('Straight Flush of Four', 4),
        StraightFlush('Straight Flush of Five', 5),
    ]

    hand_cards: Cards
    for hand_cards in combinations(cards, 5):
        hand = Hand(hand_cards)
        for army in armies:
            if army.is_valid(hand):
                army.count += 1
    
    armies.sort(key=lambda army: army.count, reverse=True)

    rank = 0
    print(' #   Count     Sample    Name')
    for army in armies:
        rank += 1
        print(f'{rank:2}  {army.count:7}  {army.sample:10}  {army.name}')

if __name__ == '__main__':
    jokers = int(sys.argv[1])
    main(jokers)
