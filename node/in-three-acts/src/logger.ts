import * as fs from 'fs';

export function debugLog(message: string): void {
    fs.appendFileSync('debug.log', message + '\n---\n', 'utf8');
}
