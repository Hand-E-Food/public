#!/usr/bin/env python3

import random
from typing import Any, Callable, Dict, Iterable, List, Optional


class Chain:
    def __init__(self, words: List[str]) -> None:
        self.last_letter = words[-1][-1]
        self.length = sum(len(word) for word in words)
        self.used_letters = get_unique_letters(words)
        self.words = words
        self.key = f'{self.last_letter}:{self.used_letters}'

    def __add__(self, word: str) -> 'Chain':
        return Chain([*self.words, word])

def load_words() -> Iterable[str]:
    with open('words.txt', 'r') as file:
        return (line.strip() for line in file.readlines())

def sort_key(word: str) -> Any:
    return len(word)

def is_valid_word(puzzle: List[str], word: str) -> bool:
    if len(word) < 3: return False
    prev_side = ''
    for letter in word:
        try:
            this_side = next(side for side in puzzle if side != prev_side and letter in side)
            prev_side = this_side
        except StopIteration:
            return False
    return True

def group_by_starting_letter(letters: str, words: Iterable[str]) -> Dict[str, List[str]]:
    words_starting_with: Dict[str, List[str]] = {letter: [] for letter in letters}
    for word in words:
        words_starting_with[word[0]].append(word)
    return words_starting_with

def chain_words(puzzle_letters: str, words_starting_with: Dict[str, List[str]], chains: List[Chain]) -> List[Chain]:
    if not chains:
        return []
    solutions = list(filter(is_solution_for(puzzle_letters), chains))
    if solutions:
        return solutions
    extended_chains: Dict[str, Chain] = {}
    count = len(chains[0].words) + 1
    print(f'Trying {count} words...')
    for chain in chains:
        words: Iterable[str] = words_starting_with[chain.last_letter]
        for word in words:
            extended_chain = chain + word
            key = extended_chain.key
            if key not in extended_chains or extended_chains[key].length > extended_chain.length:
                extended_chains[key] = extended_chain
    return chain_words(puzzle_letters, words_starting_with, list(extended_chains.values()))

def is_solution_for(puzzle_letters: str) -> Callable[[Chain], bool]:
    def is_solution(chain: Chain) -> bool:
        return puzzle_letters == chain.used_letters
    return is_solution

def get_unique_letters(words: List[str]) -> str:
    letters = set()
    for word in words:
        for letter in word:
            letters.add(letter)
    return ''.join(sorted(letters))

def solve(puzzle: List[str]) -> List[Chain]:
    words = load_words()
    valid_words = sorted((word for word in words if is_valid_word(puzzle, word)), key=sort_key)
    puzzle_letters = get_unique_letters(puzzle)
    if puzzle_letters != get_unique_letters(valid_words):
        return []
    words_starting_with = group_by_starting_letter(puzzle_letters, valid_words)
    chains = [Chain([word]) for word in valid_words]
    print(f'Trying 1 word...')
    return chain_words(puzzle_letters, words_starting_with, chains)

def create_puzzle() -> List[str]:
    side_count = 4
    side_length = 3
    letter_count = side_count * side_length
    letters = [letter for letter in 'abcdefghijklmnbopqrstuvwxyz']
    random.shuffle(letters)
    letters = ['y', *letters]
    letters = ''.join(letters[:letter_count])
    return [letters[n:n+side_length] for n in range(0, letter_count, side_length)]

def main(puzzle: Optional[List[str]] = None) -> None:
    if not puzzle:
        puzzle = create_puzzle()
    print(' / '.join(puzzle))
    solutions = solve(puzzle)
    if solutions:
        for solution in solutions:
            print(' - '.join(solution.words))
    else:
        print('No solution')

if __name__ == '__main__':
    main()
