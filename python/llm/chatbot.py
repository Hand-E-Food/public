#!/usr/bin/env python3

import sys
from typing import List, Optional

from ollama import Message
import ollama

block_delimiter = '---\n# '

class _Chatbot:
    def __init__(self, inputs: List[Message]) -> None:
        self.inputs: List[Message] = inputs
        self.messages: List[Message] = []
        self.model: str
        self.outputs: List[Message] = []
        self.reset_assistant: bool = False

    def converse(self) -> List[Message]:
        do_block = {
            'assistant': self._do_assistant_block,
            'config': self._do_config_block,
            'copy': self._do_copy_block,
            'reset': self._do_reset_block,
            'system': self._do_system_block,
            'user': self._do_user_block,
        }
        for input in self.inputs:
            do_block.get(input.role, self._do_unknown_block)(input)
        self._respond_to_previous_user_message()
        if self.messages and self.messages[-1].role != 'user':
            self.outputs.append(Message(role='user'))

        print('Added empty user message for user to continue thew conversation')
        return self.outputs

    def _do_assistant_block(self, input: Message):
        if self.messages[-1].role != 'user':
            raise ValueError('An assistant message must come immediately after a user message.')
        if self.reset_assistant: return

        self.messages.append(input)
        self.outputs.append(input)
        print('Added existing assistant message')

    def _do_config_block(self, input: Message):
        self._respond_to_previous_user_message()

        if input.content:
            self.model = input.content.strip()
        else:
            input.content = self.model = get_smallest_model()

        if not self.model in (m.model for m in ollama.list().models):
            pull_model(self.model)

        self.outputs.append(input)
        print('Set config')

    def _do_copy_block(self, input: Message):
        count = int(input.content) if input.content else 1
        messages: List[str] = [
            message.content
            for message in reversed(self.outputs)
            if message.role == 'assistant'
        ][:count] # type: ignore
        concatenated_message = '\n\n'.join(messages)

        self.messages.append(Message(role='assistant', content=concatenated_message))
        self.outputs.append(input)
        print("Copied assistant's last " + ("message" if count == 1 else f"{count} messages"))

    def _do_reset_block(self, input: Message):
        self.reset_assistant = True

        self.outputs.append(input)
        print('Resetting following assistant messages')

    def _do_system_block(self, input: Message):
        self._respond_to_previous_user_message()

        self.reset_assistant = False
        self.messages.clear()

        self.messages.append(input)
        self.outputs.append(input)
        print('Started new conversation with a system prompt')

    def _do_user_block(self, input: Message):
        if not self.messages:
            raise ValueError('A system message must come before all user messages.')

        self._respond_to_previous_user_message()

        self.messages.append(input)
        self.outputs.append(input)
        print('Added user message')

    def _do_unknown_block(self, input: Message):
        raise ValueError(f'Unexpected block "{input.role}"')

    def _respond_to_previous_user_message(self) -> None:
        if not self.messages or self.messages[-1].role != 'user' or not self.messages[-1].content:
            return

        if not self.model:
            raise ValueError('A config message must come before all user messages.')

        print('Asking assistant for response')
        response = ollama.chat(self.model, self.messages)

        message = response.message
        self.messages.append(message)
        self.outputs.append(message)

def converse(inputs: List[Message]) -> List[Message]:
    return _Chatbot(inputs).converse()

def use_file(path: str) -> None:
    inputs: List[Message] = []
    with open(path, 'r') as file:
        text = file.read()
    while text:
        try: end = text.index(block_delimiter, 1)
        except ValueError: end = len(text)
        inputs.append(_parse_message(text[:end].strip()))
        text = text[end:]

    outputs = converse(inputs)

    texts: List[str] = []
    for output in outputs:
        texts.append(f'{block_delimiter}{output.role}')
        if output.content:
            texts.append(f'{output.content}')
    texts.append('')
    with open(path, 'w') as file:
        file.write('\n\n'.join(texts))

def _parse_message(text: str) -> Message:
    if text.startswith(block_delimiter):
        try: end = text.index('\n', len(block_delimiter))
        except ValueError: end = len(text)
        role = text[len(block_delimiter):end].strip().lower()
    else:
        role = 'config'
        end = -1
    content = text[end+1:].strip()
    return Message(role=role, content=content)

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

def get_smallest_model() -> str:
    models = [model for model in ollama.list().models if model.model]
    if not models:
        raise ValueError('No models available. Pull a model first.')
    max_size = 1 + max((model.size for model in models if model.size is not None), default=0)
    models.sort(key=lambda model: model.size or max_size)
    return models[0].model # type: ignore

if __name__ == '__main__':
    args = sys.argv[1:]
    if len(args) < 1: raise ValueError('Provide a path to the chat file.')
    path = args.pop(0)
    if len(args) > 0: raise ValueError(f'Unexpected argument: {args[0]}')
    use_file(path)
