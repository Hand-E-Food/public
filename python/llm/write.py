#!/usr/bin/env python3
import sys
from os.path import dirname, join
from typing import Optional

from llm import Llm, read_file, write_file

name = sys.argv[1] if len(sys.argv) > 1 else 'story'
dir = dirname(__file__)
output: Optional[str] = None
llm = Llm()
system_prompt = read_file(join(dir, '_system.md'))
llm.new_session(system_prompt)

def chat(*filenames: str) -> Optional[str]:
    prompt = '\n'.join(read_file(join(dir, filename)) for filename in filenames)
    return llm.chat(prompt)

chat('_warmup.md', f'{name}.md')
output = chat('_edit.md')
if output:
    write_file(join(dir, f'{name}.out.md'), output)
