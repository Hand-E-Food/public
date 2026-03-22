from typing import List


words = {
    3: 'Fizz',
    5: 'Buzz',
    7: 'Pink',
    11: 'Oink',
}

def format(n: int) -> str:
    return ''.join([word for m, word in words.items() if n % m == 0]) or str(n)

for n in range(1, 3*5*7*11+1):
    print(format(n))
