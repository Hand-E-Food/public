#!/usr/bin/env python3

import random
from typing import Any, Callable, Dict, Iterable, List, Optional


def load_words() -> Iterable[str]:
    with open('words.txt', 'r') as file:
        return (line.strip() for line in file.readlines())

def sort_criteria(word: str) -> Any:
    return len(word)

def create_puzzle() -> List[str]:
    letters = [letter for letter in 'abcdefghijklmnbopqrstuvwxyz']
    random.shuffle(letters)
    letters = ''.join(letters[:12])
    return [letters[n:n+3] for n in range(0, 12, 3)]

def is_valid_word(puzzle: List[str], word: str) -> bool:
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

def chain_words(puzzle_letters: str, words_starting_with: Dict[str, List[str]], chains: List[List[str]] = [[]]) -> List[List[str]]:
    solutions = list(filter(is_solution_for(puzzle_letters), chains))
    if solutions:
        return solutions
    extended_chains: List[List[str]] = []
    count = len(chains[0]) + 1
    print(f'Trying {count} {'word' if count == 1 else 'words'}...')
    for chain in chains:
        words: Iterable[str] = words_starting_with[chain[-1][-1]] \
            if chain else \
            iter(word for words in words_starting_with.values() for word in words)
        for word in words:
            extended_chains.append([*chain, word])
    return chain_words(puzzle_letters, words_starting_with, extended_chains)

def is_solution_for(puzzle_letters: str) -> Callable[[List[str]], bool]:
    def is_solution(chain: List[str]) -> bool:
        return puzzle_letters == to_letters(chain)
    return is_solution

def to_letters(words: List[str]) -> str:
    letters = set()
    for word in words:
        for letter in word:
            letters.add(letter)
    return ''.join(sorted(letters))

def solve(puzzle: List[str]) -> List[List[str]]:
    words = load_words()
    valid_words = sorted((word for word in words if is_valid_word(puzzle, word)), key=sort_criteria)
    puzzle_letters = to_letters(puzzle)
    if puzzle_letters != to_letters(valid_words):
        return []
    words_starting_with = group_by_starting_letter(puzzle_letters, valid_words)
    return chain_words(puzzle_letters, words_starting_with)

def main(puzzle: Optional[List[str]] = None) -> None:
    if not puzzle:
        puzzle = create_puzzle()
    print(' / '.join(puzzle))
    solutions = solve(puzzle)
    if solutions:
        for solution in solutions:
            print(' - '.join(solution))
    else:
        print('No solution')

if __name__ == '__main__':
    main()
