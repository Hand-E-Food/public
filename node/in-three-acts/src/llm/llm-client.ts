export interface Message {
    role: string;
    content: string;
}

export interface LlmClient {
    /**
     * Sends a message to the LLM and returns the response.
     * @param messages The messages to send to the LLM.
     * @returns The response from the LLM.
     */
    chat(messages: Message[]): Promise<Message>;

    /**
     * Ensures all resources are correctly disposed.
     */
    dispose(): void;
}
