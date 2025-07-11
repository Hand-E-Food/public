export function wrapLines(width: number, text: string): string[] {
    const lines: string[] = [];
    while (text.length > width) {
        const line = text.substring(0, width);
        const lastSpace = line.lastIndexOf(' ');
        if (lastSpace > 0) {
            lines.push(line.substring(0, lastSpace));
            text = text.substring(lastSpace + 1);
        } else {
            lines.push(line);
            text = text.substring(width);
        }
    }
    lines.push(text);

    return lines;
}
