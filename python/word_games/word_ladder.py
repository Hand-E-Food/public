import sys
from typing import Dict, Iterable, List

MAX_STEPS = 6 # Including target word

def load_words(filename: str, word_length: int) -> Iterable[str]:
  print('Loading words')
  with open(filename, 'r') as file:
    for word in file.readlines():
      word = word.strip()
      if len(word) == word_length:
        yield word.upper()

class Node:
  def __init__(self, word: str, links: List[str]):
    self.distance = 1000000
    self.linked_nodes: List[Node] = []
    self.linked_words = links
    self.word = word

  def __repr__(self) -> str:
    return self.word

class Solver:
  def __init__(self, words: List[str]):
    print('Linking words')
    self.word_links: Dict[str, List[str]] = { word: [] for word in words }
    for index1 in range(len(words)):
      word1 = words[index1]
      for index2 in range(index1 + 1, len(words)):
        word2 = words[index2]
        if Solver.is_link(word1, word2):
          self.word_links[word1].append(word2)
          self.word_links[word2].append(word1)    

  @staticmethod
  def is_link(word1: str, word2: str) -> bool:
    diff = False
    for i in range(len(word1)):
      if word1[i] != word2[i]:
        if diff: return False
        diff = True
    return diff

  def solve(self, start_word: str, target_word: str) -> Iterable[List[str]]:
    print('Building graph')
    nodes = self.build_graph(target_word)
    print('Finding paths')
    node = nodes.get(start_word, None)
    if node:
      return self.walk(node)
    print('No solution')
    return []

  def build_graph(self, target_word) -> Dict[str, Node]:
    nodes = { word: Node(word, links) for word, links in self.word_links.items() }
    node = nodes[target_word]
    node.distance = 0
    node_queue = [node]
    for node1 in node_queue:
      distance = node1.distance + 1
      for word2 in node1.linked_words:
        node2 = nodes[word2]
        node1.linked_nodes.append(node2)
        if node2.distance <= distance: continue
        node2.distance = distance
        node_queue.append(node2)
    return nodes
  
  def walk(self, node1: Node) -> Iterable[List[str]]:
    if node1.distance == 0:
      yield [node1.word]
    else:
      for node2 in node1.linked_nodes:
        if node1.distance <= node2.distance: continue
        for path in self.walk(node2):
          yield [node1.word, *path]

def main(start_word: str = 'KEVIN', target_word: str = 'BACON', word_filename: str = 'words.txt') -> int:
  start_word = start_word.upper()
  target_word = target_word.upper()

  word_length = len(start_word)
  if len(target_word) != word_length:
    print('Words are different lengths.')
    return 1

  print(start_word + ' > ' + ('.' * word_length) + ' > ' + target_word)

  words = list(load_words(word_filename, word_length))
  for word in (start_word, target_word):
    if word not in words: words.append(word)

  solver = Solver(words)
  for solution in solver.solve(start_word, target_word):
    print(' > '.join(solution))

  return 0

if __name__ == '__main__': sys.exit(main(*sys.argv[1:]))
