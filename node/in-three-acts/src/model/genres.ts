export class Genres {
    public static readonly All: string[] = [
        'action adventure',
        'biblical',
        'comedy',
        'coming-of-age',
        'espionage',
        'fantasy',
        'folklore',
        'ghost',
        'historical',
        'modern-day',
        'noir',
        'pirate',
        'romance (but not erotic)',
        'samurai',
        'science fiction',
        'superhero',
        'thriller',
    ];

    public static random(): string {
        const index = Math.floor(Math.random() * Genres.All.length);
        return Genres.All[index];
    }
}
