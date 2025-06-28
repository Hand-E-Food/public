import { Console } from './console';
import { BookChapter, Chapter, ChapterDecks, Goal, MaxGoals, Player, PublicKnowledge, Suit, Suits } from "./model";
import { View } from "./view";
import color from "cli-color";

export class ConsoleView implements View {
    public player?: Player = undefined;

    public setPlayer(player: Player): void {
        this.player = player;
    }

    public showGame(publicKnowledge: PublicKnowledge): void {
        Console.write('\n');
        Console.write(color.whiteBright('P1     P2      Next Chapter') + '\n');
        for (const suit of Suits) {
            const player1 = this.formatPlayer(suit, publicKnowledge, 0);
            const player2 = this.formatPlayer(suit, publicKnowledge, 1);
            const nextChapter = this.formatChapter(publicKnowledge.nextChapters[suit], true);
            Console.write(this.colorize(suit, `${player1}   ${player2}   `) + nextChapter + '\n');
        }
        Console.write('\n');
        for (let i = 0; i < MaxGoals; i++) {
            let result = '';
            for (let j = 0; j < publicKnowledge.players.length; j++) {
                const goals = publicKnowledge.players[j].id === this.player?.id
                    ? this.player.goals
                    : publicKnowledge.players[j].goals;
                if (goals.length > i) result = result.padEnd(j * 35) + this.formatGoal(goals[i], true);
            }
            Console.write(result + '\n');
        }
        Console.write('\n');
    }

    public async chooseChapter(chapters: Chapter[]): Promise<Chapter> {
        const suits: string[] = [];
        for (const chapter of chapters) {
            suits.push(chapter.suit);
            Console.write(this.formatChapter(chapter, true) + '\n');
        };
        const answer = await this.question("Write which chapter", suits);
        return chapters[suits.indexOf(answer)];
    }

    public chooseSuit(suits: Suit[]): Promise<Suit> {
        return this.question("Write as which suit", suits);
    }

    public showBookChapter(chapter: BookChapter): void {
        const MAX_WIDTH = 80;
        Console.write('\n');
        Console.write(`${this.colorize(chapter.chapter.suit, `${chapter.number}. ${chapter.chapter.name}`)}\n`);
        let text = chapter.text;
        const lines: string[] = [];
        while (text.length > MAX_WIDTH) {
            const line = text.substring(0, MAX_WIDTH);
            const lastSpace = line.lastIndexOf(' ');
            if (lastSpace > 0) {
                lines.push(line.substring(0, lastSpace));
                text = text.substring(lastSpace + 1);
            } else {
                lines.push(line);
                text = text.substring(MAX_WIDTH);
            }
        }
        lines.push(text);
        for (const line of lines)
            Console.write(line + '\n');
    }

    public showWinner(player: Player): void {
        Console.write(`${player.name} wins!\n`);
    }

    private formatPlayer(suit: Suit, publicKnowledge: PublicKnowledge, index: 0 | 1): string {
        const my = publicKnowledge.players[index];
        const your = publicKnowledge.players[1 - index];
        const chapters = this.formatChapterCount(suit, my.chapters);
        const goals = this.player?.id === your.id ? this.player.goals : your.goals;
        const goal = this.formatGoalCount(suit, goals);
        return chapters + goal;
    }

    private formatChapterCount(suit: Suit, chapters: ChapterDecks) {
        const wild = chapters[suit].some(chapter => chapter.wild) ? '*' : ' ';
        const count = chapters[suit].length;
        return `${wild}${count}`
    }

    private formatGoalCount(suit: Suit, goals: Goal[]) {
        const goal = goals.reduce((total, goal) => total + (goal.suitCounts[suit] ?? 0), 0);
        return goal ? `/${goal}` : '  ';
    }

    private formatChapter(chapter: Chapter, showName?: boolean): string {
        let result = `${chapter.wild ? '*' : ' '}${this.formatSuit(chapter.suit)} > ${this.formatSuit(chapter.inspires[0])}${this.formatSuit(chapter.inspires[1])}`;
        if (showName) result += ' ' + chapter.name;
        return result;
    }

    private formatGoal(goal: Goal, showName?: boolean): string {
        const suitCounts = goal.suitCounts;
        let result = Object.keys(suitCounts)
            .map(suit => `${this.formatSuit(suit)}:${suitCounts[suit]}`, this)
            .join(' ');
        if (showName) result += ' - ' + goal.name;
        return result;
    }

    private formatSuit(suit: Suit): string {
        return this.colorize(suit, suit);
    }

    private colorize(suit: Suit, text: string): string {
        switch (suit) {
            case 'a': return color.greenBright(text);
            case 'h': return color.yellowBright(text);
            case 'j': return color.magentaBright(text);
            case 'r': return color.redBright(text);
            case 's': return color.blueBright(text);
            default : return text;
        }
    }

    private async question(prompt: string, valid: string[]): Promise<string> {
        let answer: string;
        while(true) {
            answer = await Console.question(`${prompt} [${valid.join('')}]: `);
            if (valid.includes(answer)) break;
            Console.moveCursor(0, -1).clearLine(0);
        }
        return answer;
    }

    public dispose(): void {
        Console.close();
    }
}
