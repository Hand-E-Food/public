#!/usr/bin/env python3

import sys
from typing import List, Optional

from ollama import ChatResponse, Message, Options
import ollama

block_delimiter = '---\n# '

class Block:
    def __init__(self, title: str, content: Optional[str] = None) -> None:
        self.title = title
        self.content = content

class _ChatRequest:
    def __init__(self) -> None:
        self.keep_alive: str = '30m'
        self.messages: List[Message] = []
        self.model: str = ''
        self.options: Options = Options()
        self.stream: bool = False
        self.think: bool = False
    
    def keys(self):
        return self.__dict__.keys()
    
    def __getitem__(self, key):
        return getattr(self, key)

class _Chatbot:
    def __init__(self, inputs: List[Block]) -> None:
        self.chat_request = _ChatRequest()
        self.inputs: List[Block] = inputs
        self.messages: List[Message] = []
        self.model: Optional[ollama.ShowResponse] = None
        self.outputs: List[Block] = []
        self.reset_assistant: bool = False
        self.user_messages_count: int = 0
        self.user_messages_total: int = len([input for input in inputs if input.title == 'user' and input.content])

        self.chat_request.messages = self.messages

    def converse(self) -> List[Block]:
        do_block = {
            'assistant': self._do_assistant_block,
            '//': self._do_comment_block,
            'config': self._do_config_block,
            'copy': self._do_copy_block,
            'reset': self._do_reset_block,
            'system': self._do_system_block,
            'thinking': self._do_thinking_block,
            'user': self._do_user_block,
        }
        for input in self.inputs:
            do_block.get(input.title, self._do_unknown_block)(input)
        self._respond_to_previous_user_message()
        if self.messages and self.messages[-1].role != 'user':
            self.outputs.append(Block('user'))

        print('Added empty user message for user to continue the conversation')
        return self.outputs

    def _do_assistant_block(self, input: Block):
        if self.messages[-1].role != 'user':
            raise ValueError('An assistant message must come immediately after a user message.')
        if self.reset_assistant: return

        self.messages.append(Message(role=input.title, content=input.content))
        self.outputs.append(input)
        print('Added existing assistant message')

    def _do_comment_block(self, input: Block):
        self._respond_to_previous_user_message()
        self.outputs.append(input)

    def _do_config_block(self, input: Block):
        self._respond_to_previous_user_message()

        if not input.content:
            raise ValueError('Config block must specify the model to use.')

        model = input.content.strip()
        self.chat_request.model = model
        self.model = ollama.show(model)
        self.chat_request.think = think = any(
            indicator in (self.model.capabilities or [])
            for indicator in ["reasoning", "thinking", "chain-of-thought", "step-by-step", "cot", "o1", "reflect"])

        self.outputs.append(input)
        print(f'Using {model} {'with' if think else 'without'} thinking')

    def _do_copy_block(self, input: Block):
        count = int(input.content) if input.content else 1
        messages: List[str] = [
            output.content or ''
            for output in self.outputs
            if output.title == 'assistant'
        ][-count:]
        concatenated_message = '\n\n'.join(messages)

        self.messages.append(Message(role='assistant', content=concatenated_message))
        self.outputs.append(input)
        print("Copied assistant's last " + ("message" if count == 1 else f"{count} messages"))

    def _do_reset_block(self, input: Block):
        self.reset_assistant = True

        self.outputs.append(input)
        print('Resetting following assistant messages')

    def _do_system_block(self, input: Block):
        self._respond_to_previous_user_message()

        self.reset_assistant = False
        if input.content:
            self.messages.clear()
            self.messages.append(Message(role=input.title, content=input.content))
            print('Started a new conversation with a new system prompt')
        elif self.messages and self.messages[0].role == 'system':
            self.messages = self.messages[:1]
            print('Started a new conversation with the same system prompt')
        else:
            self.messages = []
            print('Started a new conversation with no system prompt')

        self.outputs.append(input)

    def _do_thinking_block(self, input: Block):
        if self.reset_assistant: return

        last_message = self.messages[-1] if self.messages else Message(role='')
        if last_message.role != 'assistant':
            raise ValueError('A thinking message must come immediately after an assistant message.')

        last_message.thinking = input.content.strip() if input.content else None
        self.outputs.append(input)
        print('Added existing thinking')

    def _do_user_block(self, input: Block):
        self._respond_to_previous_user_message()

        self.user_messages_count += 1
        self.messages.append(Message(role=input.title, content=input.content))
        self.outputs.append(input)
        print('Added user message')

    def _do_unknown_block(self, input: Block):
        raise ValueError(f'Unexpected block "{input.title}"')

    def _respond_to_previous_user_message(self) -> None:
        last_message = self.messages[-1] if self.messages else Message(role='')
        if last_message.role != 'user' or not last_message.content:
            return

        if not self.chat_request.model:
            raise ValueError('A config message must come before all user messages.')

        print(f'Asking assistant for response to user message {self.user_messages_count} of {self.user_messages_total}')

        content: Optional[str]
        if self.chat_request.stream:
            # Stream the response so this can respond to Ctrl+C immediately.
            responses = ollama.chat(**self.chat_request)

            content = ''
            for response in responses:
                content += response.message.content or ''
                if response.done:
                    print(f'Successfully streamed response: {response.done_reason}')
                    break
            
            content = content.strip()
            message = Message(role='assistant', content=content)
        else:
            # Get the full response at once.
            response: ChatResponse = ollama.chat(**self.chat_request)
            if response.done:
                print(f'Successful response: {response.done_reason}')
            else:
                print(f'Unsuccessful response.')

            content = response.message.content
            message = response.message

        self.messages.append(message)
        self.outputs.append(Block('assistant', content=content))
        if message.thinking:
            self.outputs.append(Block('thinking', content=message.thinking.strip()))

def converse(inputs: List[Block]) -> List[Block]:
    return _Chatbot(inputs).converse()

def converse_in_file(path: str) -> None:
    inputs: List[Block] = []
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
        texts.append(f'{block_delimiter}{output.title}')
        if output.content:
            texts.append(f'{output.content}')
    texts.append('')
    with open(path, 'w') as file:
        file.write('\n\n'.join(texts))

def _parse_message(text: str) -> Block:
    if text.startswith(block_delimiter):
        try: end = text.index('\n', len(block_delimiter))
        except ValueError: end = len(text)
        title = text[len(block_delimiter):end].strip().lower()
    else:
        title = 'config'
        end = -1
    content = text[end+1:].strip()
    return Block(title, content)

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
    converse_in_file(path)
