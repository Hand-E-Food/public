# Introduction

Chatbot runs an Ollama LLM against a conversation. The conversation is a sequence of messages and commands. A conversation is stored in a single Markdown file.

The intent is to use a featureful text editor (e.g. VSCode) to either continue the conversation, or edit and rerun an in-progress conversation.

Assistant responses may be reused without calling the LLM. If you include a **reset** block, then any following **assistant** blocks are instead ignored.

The system prompt may be changed part-way through the conversation. Doing so clears all messages from memory. The **copy** command may be used to copy **assistant** responses from the end of the previous conversation into the new conversation.


# Usage

python3 ./chatbot.py (path)

path    The path to the conversation file to use.


# Conversation File Syntax

This conversation file is a sequence of blocks written in Markdown format. Each block starts with a horizontal line with a level 1 heading on the next line. The text of the heading indicates the command this block executes. The headings are not case-sensitive.

## Commands

### config

This block contains a single word which is the name of the LLM model to use. The model must be installed.
This must be the first block in the file. It may be specified again later in the file to change that model that is used form that point onward.

### system

Starts a new conversation with a system prompt.
This must be specified before any **user** or **assistant** blocks.

### user

Adds a user message to the conversation.
If mulitple **user** blocks are specified sequentially, then the LLM will be called and the **assistant** message inserted between each pair of **user** blocks.

### assistant

The response from the LLM. If the conversation file contains this when it is run, this block is used instead of calling the LLM.
This block may only follow a **user** block.

### reset

This block does not have any content.
It instructs the engine to ignore any **assistant** block from this point forward, calling the LLM instead of reusing its previous responses.
A subsequent **system** block resets this behaviour.

### copy

This block is a command to copy the previous n assistant blocks. This is used to copy a previous response after a **system** block has reset the conversation.
This block contains a single number, or may be empty. If it is empty, it defaults to "1".

## Example

```
---
# config

llama3.2:latest

---
# system

You are a friendly conversationalist.

---
# user

Hello, how are you.

```
