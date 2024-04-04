using System.Text;

namespace Bots.Engine.Code;
public class TokenReader : IDisposable
{
    private readonly TextReader reader;

    private char c;
    private int column = 0;
    private int i;
    private int line = 1;

    public TokenReader(TextReader reader)
    {
        this.reader = reader;
        ReadChar();
    }

    public bool EndOfStream { get; private set; }

    private void ReadChar()
    {
        column++;
        i = reader.Read();
        if (i >= 0)
        {
            c = (char)i;
        }
        else
        {
            c = '\0';
            EndOfStream = true;
        }
    }

    public IEnumerable<Token> ReadAllTokens()
    {
        Token token;
        do
        {
            token = ReadToken();
            yield return token;
        }
        while (token.Type != TokenType.EndOfStream);
    }

    public Token ReadToken()
    {
        Token token = new()
        {
            Column = column,
            Line = line,
        };

        if (EndOfStream)
        {
            token.Type = TokenType.EndOfStream;
        }
        else if ("\r\n".Contains(c))
        {
            token.Type = TokenType.NewLine;
            token.Text = ReadNewLineToken();
            column = 0;
            line++;
        }
        else if (char.IsAsciiDigit(c))
        {
            token.Type = TokenType.Number;
            token.Text = ReadNumberToken();
        }
    }

    private string ReadNewLineToken()
    {
        StringBuilder text = new(2);
        text.Append(c);
        if (c == '\r')
        {
            ReadChar();
            if (c == '\n')
            {
                text.Append(c);
                ReadChar();
            }
        }
        else
        {
            ReadChar();
        }
        return text.ToString();
    }
    
    private bool isDisposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed) return;
        if (disposing)
        {
            reader.Dispose();
        }
        isDisposed = true;
    }
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
