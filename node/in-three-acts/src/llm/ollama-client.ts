import { ChatRequest, ChatResponse, Ollama } from "ollama";
import { LlmClient, Message } from "./llm-client";
import { debugLog } from "../logger";

export interface IOllama {
    chat(request: ChatRequest & { stream?: false; }): Promise<ChatResponse>;
    abort(): void;
}

export class OllamaClient implements LlmClient {
    private readonly model: string;
    private readonly ollama: IOllama;

    public constructor(model: string, ollama?: IOllama) {
        this.model = model;
        this.ollama = ollama ?? new Ollama();
    }

    public async chat(messages: Message[]): Promise<Message> {
        const response = await this.ollama.chat({
            keep_alive: '1m',
            model: this.model,
            messages,
            stream: false,
        });
        return response.message;
    }

    public dispose(): void {
        this.ollama.abort();
    }
}
