#!/usr/bin/env python3
from typing import List, Optional

import ollama
from ollama import Message


def read_file(path: str) -> str:
    with open(path, 'r') as file:
        return file.read()

def write_file(path: str, content: str) -> None:
    with open(path, 'w') as file:
        file.write(content)

def print_wrapped(text: str, width: int = 40) -> None:
    while text:
        index = text.rindex(' ', 0, width)
        line = text[:index]
        text = text[index + 1:]
        print(line)
    print(text)

def get_text_after(token: str, text: Optional[str]) -> Optional[str]:
    if not text: return None
    index = text.find(token)
    if index == -1: return None
    return text[index + len(token):].strip()

def get_text_between(start_token: str, end_token: str, text: Optional[str]) -> Optional[str]:
    if not text: return None
    start_index = text.find(start_token)
    if start_index == -1: return None
    start_index += len(start_token)
    end_index = text.find(end_token, start_index)
    if end_index == -1: end_index = len(text)
    return text[start_index:end_index].strip()

class Llm:
    def __init__(self, model: Optional[str] = None):
        if model:
            self._model: str = model
            if not any(m for m in ollama.list().models if m.model == model):
                print(f'Pulling model {model}...')
                ollama.pull(model)
        else:
            models = [m for m in ollama.list().models if m.model]
            if not models:
                raise ValueError('No models available. Please pull a model first.')
            models.sort(key=lambda m: m.size or 10**12)
            self._model: str = models[0].model # type: ignore
        self._messages: List[Message] = []

    def new_session(self, system_prompt: str) -> None:
        self._messages.clear()
        self._messages.append(Message(role='system', content=system_prompt))

    def chat(self, user_prompt: str) -> Optional[str]:
        if len(self._messages) == 0: raise ValueError('Call "new_session(system_prompt)" first.')
        self._messages.append(Message(role='user', content=user_prompt))
        response = ollama.chat(self._model, self._messages)
        self._messages.append(response.message)
        return response.message.content
