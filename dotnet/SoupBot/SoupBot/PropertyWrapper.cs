using System.Linq.Expressions;
using System.Reflection;

namespace SoupBot;

/// <inheritdoc cref="PropertyWrapper{TValue}"/>
internal class PropertyWrapper
{
    /// <inheritdoc cref="PropertyWrapper{TValue}" path="/typeparam"/>
    /// <inheritdoc cref="PropertyWrapper{TValue}.For{TInstance}(TInstance, Expression{Func{TInstance, TValue}})"/>
    public static PropertyWrapper<TValue> For<TInstance, TValue>(TInstance instance, Expression<Func<TInstance, TValue>> target)
        where TInstance : notnull =>
        PropertyWrapper<TValue>.For(instance, target);
}

/// <summary>
/// Wraps a property's getter and setter.
/// </summary>
/// <typeparam name="TValue">The property's value type.</typeparam>
internal class PropertyWrapper<TValue>
{
    private readonly object instance;
    private readonly PropertyInfo property;

    /// <summary>
    /// The wrapped property's name.
    /// </summary>
    public string Name => property.Name;

    /// <summary>
    /// The property's value.
    /// </summary>
    public TValue Value
    {
        get => (TValue)property.GetValue(instance)!;
        set => property.SetValue(instance, value);
    }

    private PropertyWrapper(object instance, PropertyInfo propertyInfo)
    {
        this.instance = instance;
        property = propertyInfo;
    }

    /// <summary>
    /// Creates a new <see cref="PropertyWrapper{TValue}"/>.
    /// </summary>
    /// <typeparam name="TInstance">The type of <paramref name="instance"/>.</typeparam>
    /// <param name="instance">The instance owning the property.</param>
    /// <param name="target">The property to wrap.</param>
    /// <returns>A <see cref="PropertyWrapper{TValue}"/>.</returns>
    public static PropertyWrapper<TValue> For<TInstance>(TInstance instance, Expression<Func<TInstance, TValue>> target)
        where TInstance : notnull
    {
        var instanceType = instance.GetType();
        if (target.Body is not MemberExpression memberExpression
            || memberExpression.Member is not PropertyInfo propertyInfo
            || (
                propertyInfo.ReflectedType != null
                && instanceType != propertyInfo.ReflectedType
                && !instanceType.IsSubclassOf(propertyInfo.ReflectedType)
            )
        )
        {
            throw new ArgumentException($"{nameof(target)} must point to a property of {nameof(instance)}.");
        }

        return new(instance, propertyInfo);
    }
}
