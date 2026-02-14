# Rules:
# There are a set of letters. One is the target letter.
# Make as many words as possible using at least 4 letters.
# Each letter may only be used once.
# The target letter must be used in every word.
#
# Input:
# The letters in lower case, with the first letter being the target letter.
#
# Example:
# python target_time.py wetaibrrd

from itertools import combinations, permutations
import sys
from typing import Iterable, Set

def load_words() -> Iterable[str]:
    with open('words.txt', 'r') as file:
        return (line.strip() for line in file.readlines())

input = sys.argv[1]
maximum_length = len(input)
minimum_length = 4
target_letter = input[0]
other_letters = input[1:]

dictionary_words = set(load_words())
answers: Set[str] = set()
for count in range(minimum_length - 1, maximum_length):
    for subset_letters in combinations(other_letters, count):
        for guess_letters in permutations(target_letter + ''.join(subset_letters), count + 1):
            guess_word = ''.join(guess_letters)
            if guess_word in dictionary_words:
                answers.add(guess_word)

for answer in answers:
    print(answer)
