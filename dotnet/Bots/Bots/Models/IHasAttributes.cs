namespace Bots.Models;
internal interface IHasAttributes
{
    public IEnumerable<string> Attributes { get; }
}
