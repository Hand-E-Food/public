using Bots.Models;

namespace Bots.Engine.Code;

internal interface IValue<T>
{
    T GetValue();
}

internal class Constant<T>(T value) : IValue<T>
{
    public T GetValue() => value;
}

internal class HasAttribute(IValue<IHasAttributes> variable, IValue<string> attribute) : IValue<bool>
{
    public bool GetValue() => variable.GetValue().Attributes.Contains(attribute.GetValue());
}
