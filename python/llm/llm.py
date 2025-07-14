#!/usr/bin/env python3
import os
import sys
from typing import List, Optional

import ollama
from ollama import Message

MODEL = 'llama3.2:latest'
TOKEN = 'THE STORY:'

def pop_arg(args: List[str], filename: str) -> str:
    return args.pop(0) if len(args) > 0 else os.path.join(os.path.dirname(__file__), filename)

def get_prompts(system_prompt_path: str, user_prompt_path: str) -> List[Message]:
    with open(system_prompt_path, 'r') as file:
        system_prompt = file.read()
    with open(user_prompt_path, 'r') as file:
        user_prompt = file.read()

    return [
        Message(role='system', content=system_prompt),
        Message(role='user', content=user_prompt),
    ]

def call_llm(messages: List[Message]) -> Optional[str]:
    messages = messages.copy()
    response = ollama.chat(MODEL, messages)
    messages.append(response.message)
    output = response.message.content
    if not output: return None

    index = output.find(TOKEN)
    if index != -1: output = output[index + len(TOKEN):]
    output = output.strip() + '\n'
    return output

def save_output(output_path: str, output: Optional[str]):
    if output:
        with open(output_path, 'w') as file:
            file.write(output)

def print_output(output: str, width: int = 40) -> None:
    while output:
        index = output.rindex(' ', 0, width)
        line = output[:index]
        output = output[index + 1:]
        print(line)
    print(output)

def main(args: List[str]) -> None:
    system_prompt_path = pop_arg(args, 'system.md')
    user_prompt_path = pop_arg(args, 'user.md')
    output_path = pop_arg(args, 'output.txt')
    messages = get_prompts(system_prompt_path, user_prompt_path)
    output = call_llm(messages)
    save_output(output_path, output)

if __name__ == '__main__':
    main(sys.argv[1:])
