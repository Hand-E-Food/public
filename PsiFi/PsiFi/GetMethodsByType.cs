using System.Reflection;

namespace PsiFi
{
    /// <summary>
    /// Gets a dictionary of types that map to methods that take that type as a parameter.
    /// </summary>
    public static class GetMethodsByType
    {
        /// <summary>
        /// Gets a dictionary that maps types to <see cref="Action{BaseType}"/> deletgates that
        /// take a more specific type of <typeparamref name="BaseType"/> as a parameter.
        /// </summary>
        /// <typeparam name="BaseType">The type of parameter shared by these methods.</typeparam>
        /// <param name="obj">The instance that owns the methods.</param>
        /// <param name="methodName">The name of the methods. All methods must have the same name.
        /// </param>
        /// <returns>A dictionary that maps types extending from <typeparamref name="BaseType"/> to
        /// <see cref="Action{BaseType}"/> delegates that take that same type as their only
        /// parameter.</returns>
        /// <remarks>Public and private instance methods are found </remarks>
        public static Dictionary<Type, Action<BaseType>> Action<BaseType>(object obj, string methodName)
        {
            var result = new Dictionary<Type, Action<BaseType>>();
            var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (method.Name != methodName) continue;
                if (method.ReturnParameter.ParameterType != typeof(void)) continue;
                var parameters = method.GetParameters();
                if (parameters.Length != 1) continue;
                var parameterType = parameters[0].ParameterType;
                if (parameterType != typeof(BaseType) && !parameterType.IsAssignableTo(typeof(BaseType))) continue;
                result.Add(parameterType, (BaseType a) => method.Invoke(obj, new object?[] { a }));
            }
            return result;
        }

        public static Dictionary<Type, Func<BaseType, ReturnType>> Func<BaseType, ReturnType>(object obj, string methodName)
        {
            var result = new Dictionary<Type, Func<BaseType, ReturnType>>();
            var methods = obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods)
            {
                if (method.Name != methodName) continue;
                if (method.ReturnParameter.ParameterType != typeof(ReturnType)) continue;
                var parameters = method.GetParameters();
                if (parameters.Length != 1) continue;
                var parameterType = parameters[0].ParameterType;
                if (parameterType != typeof(BaseType) && !parameterType.IsAssignableTo(typeof(BaseType))) continue;
                result.Add(parameterType, (BaseType a) => (ReturnType)method.Invoke(obj, new object?[] { a })!);
            }
            return result;
        }
    }
}
