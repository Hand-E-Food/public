export class AuthorStyles {
    public static readonly All: string[] = [
        "childrens'",
        'classical',
        'dark',
        'modern',
        'pulp fiction',
        'satirical',
        'whimsical',
    ];

    public static random(): string {
        const index = Math.floor(Math.random() * AuthorStyles.All.length);
        return AuthorStyles.All[index];
    }
}
