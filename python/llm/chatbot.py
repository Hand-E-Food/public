#!/usr/bin/env python3

import sys
from typing import List, Optional

import ollama

def _read_file(path: str) -> List[str]:
    with open(path, 'r') as file:
        return [block.strip() for block in file.read().split('\n---')]

def _is_valid_inputs(blocks: List[str]) -> bool:
    count = len(blocks)
    if count >= 3: return True
    while count < 3:
        blocks.append('')
        count += 1
    if not blocks[1]: blocks[1] = '{ADD SYSTEM PROMPT HERE}'
    if not blocks[2]: blocks[2] = '{ADD USER MESSAGE HERE}'
    return False

def pull_model(model: str) -> None:
    previous: Optional[str] = None
    for response in ollama.pull(model, stream=True):
        current = response.digest or response.status
        if previous != current:
            previous = current
            print('')
        if response.completed is not None and response.total is not None and response.total > 0:
            print(f'{response.status} {100 * response.completed / response.total:.1f}%', end='\r')
        else:
            print(f'{response.status}', end='\r')
    print('')

def initialise_model(model: Optional[str]) -> str:
    if model:
        if not any(m for m in ollama.list().models if m.model == model):
            pull_model(model)
    else:
        models = [m for m in ollama.list().models if m.model]
        if not models:
            raise ValueError('No models available. Please pull a model first.')
        models.sort(key=lambda m: m.size or 10**12)
        model = models[0].model
    return model # type: ignore

def converse(model: str, inputs: List[str]) -> Optional[str]:
    '''
    Perform a conversation with the model using the provided inputs.
    @param model: The model to use for the conversation.
    @param inputs: The conversation inputs, starting with the system prompt, followed by alternating user and assistant messages.
    @return: The model's response as a string, or None if no response is generated.
    '''

    if not model:
        raise ValueError('Model must be specified.')
    if not inputs or len(inputs) < 2:
        raise ValueError('At least a system prompt and one user message must be specified.')

    messages: List[ollama.Message] = [ollama.Message(role='system', content=inputs[0])]
    messages.extend(
        ollama.Message(role=('user' if i % 2 else 'assistant'), content=inputs[i])
        for i in range(1, len(inputs))
    )
    response = ollama.chat(model, messages)
    return response.message.content

def _sanitize_output(output: str) -> str:
    output = output.strip().replace('\n---', '\n ---')
    if output.startswith('---'):
        output = ' ' + output
    return output

def _write_file(path: str, blocks: List[str]) -> None:
    delimiter = '\n\n---\n\n'
    content = delimiter.join(block for block in blocks)
    with open(path, 'w') as file:
        file.write(content)

def main(path: str) -> None:
    blocks = _read_file(path)
    if _is_valid_inputs(blocks):
        blocks[0] = initialise_model(blocks[0] or None)
        output = converse(blocks[0], blocks[1:])
        if output:
            output = _sanitize_output(output)
            blocks.append(output.strip())
            blocks.append('')
    _write_file(path, blocks)

if __name__ == '__main__':
    args = sys.argv[1:]
    if len(args) < 1: raise ValueError('Provide a path to the chat file.')
    path = args.pop(0)
    if len(args) > 0: raise ValueError(f'Unexpected argument: {args[0]}')
    main(path)
