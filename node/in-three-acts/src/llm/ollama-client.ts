import { ChatRequest, ChatResponse, Ollama, Options } from "ollama";
import { LlmClient, Message } from "./llm-client";

const keep_alive = '10m';
const options: Partial<Options> = {
    repeat_penalty: 1.2,
};

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

    public async warmup(): Promise<void> {
        await this.ollama.chat({
            keep_alive,
            messages: [{ role: 'system', content: 'Do not respond.' }],
            model: this.model,
            stream: false,
        });
    }

    public async chat(messages: Message[]): Promise<Message> {
        const response = await this.ollama.chat({
            keep_alive,
            messages,
            model: this.model,
            options,
            stream: false,
        });
        return response.message;
    }

    public dispose(): void {
        this.ollama.abort();
    }
}
