from typing import Iterable


def load_words() -> Iterable[str]:
    with open('words.txt', 'r') as file:
        return (line.strip() for line in file.readlines())


words = set(load_words())
for forward_word in words:
    reverse_word = ''.join(reversed(forward_word))
    if forward_word == reverse_word:
        print(forward_word)
    elif forward_word < reverse_word and reverse_word in words:
        print(forward_word, '<->', reverse_word)
