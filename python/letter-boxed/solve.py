#!/usr/bin/env python3

import random
from typing import Dict, Iterable, List, Optional


def main(puzzle: Optional[List[str]] = None) -> None:
    if not puzzle:
        puzzle = create_puzzle()
    print(' / '.join(puzzle))
    words = load_words()
    solutions = Solver(puzzle, words).solve()
    if solutions:
        for solution in solutions:
            print(' - '.join(solution.words))
    else:
        print('No solution')

def create_puzzle() -> List[str]:
    side_count = 4
    side_length = 3
    letter_count = side_count * side_length
    letters = [letter for letter in 'abcdefghijklmnbopqrstuvwxyz']
    random.shuffle(letters)
    letters = ['y', *letters]
    letters = ''.join(letters[:letter_count])
    return [letters[n:n+side_length] for n in range(0, letter_count, side_length)]

def load_words() -> Iterable[str]:
    with open('words.txt', 'r') as file:
        return (line.strip() for line in file.readlines())

class Chain:
    def __init__(self, words: List[str]) -> None:
        self.last_letter = words[-1][-1]
        self.length = sum(len(word) for word in words)
        self.used_letters = get_unique_letters(words)
        self.words = words
        self.key = f'{self.last_letter}:{self.used_letters}'

    def __add__(self, word: str) -> 'Chain':
        return Chain([*self.words, word])

class Solver:
    def __init__(self, puzzle: List[str], words: Iterable[str]) -> None:
        self.puzzle = puzzle
        self.puzzle_letters = get_unique_letters(puzzle)
        if len(self.puzzle_letters) < sum(len(side) for side in puzzle):
            raise ValueError('puzzle must contain only unique letters')
        self.words = sorted((word for word in words if self.is_valid_word(word)), key=lambda word: len(word))
        self.words_starting_with = self._group_by_starting_letter()

    def is_valid_word(self, word: str) -> bool:
        if len(word) < 3: return False
        prev_side = ''
        for letter in word:
            try:
                this_side = next(side for side in self.puzzle if side != prev_side and letter in side)
                prev_side = this_side
            except StopIteration:
                return False
        return True

    def _group_by_starting_letter(self) -> Dict[str, List[str]]:
        words_starting_with: Dict[str, List[str]] = {letter: [] for letter in self.puzzle_letters}
        for word in self.words:
            words_starting_with[word[0]].append(word)
        return words_starting_with

    def solve(self) -> List[Chain]:
        if self.puzzle_letters != get_unique_letters(self.words):
            return []
        chains = [Chain([word]) for word in self.words]
        print(f'Trying 1 word...')
        return self._chain_words(chains)

    def _chain_words(self, chains: List[Chain]) -> List[Chain]:
        if not chains:
            return []
        solutions = list(sorted(filter(self.is_solution, chains), key=lambda solution: solution.length))
        if solutions:
            return solutions
        extended_chains: Dict[str, Chain] = {}
        count = len(chains[0].words) + 1
        print(f'Trying {count} words...')
        for chain in chains:
            words: Iterable[str] = self.words_starting_with[chain.last_letter]
            for word in words:
                extended_chain = chain + word
                key = extended_chain.key
                if key not in extended_chains or extended_chains[key].length > extended_chain.length:
                    extended_chains[key] = extended_chain
        return self._chain_words(list(extended_chains.values()))

    def is_solution(self, chain: Chain) -> bool:
        return self.puzzle_letters == chain.used_letters

def get_unique_letters(words: List[str]) -> str:
    letters = set()
    for word in words:
        for letter in word:
            letters.add(letter)
    return ''.join(sorted(letters))

if __name__ == '__main__':
    puzzle = None
    #puzzle = ['yye', 'jmi', 'ncr', 'wxs']
    #puzzle = ['ymz', 'jfc', 'ohl', 'rtb']
    #puzzle = ['yzl', 'ufj', 'yhx', 'ces']
    main(puzzle)
