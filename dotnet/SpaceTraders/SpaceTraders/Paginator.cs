using Microsoft.Extensions.Logging;
using SpaceTraders.Api;
using System.Reflection;

namespace SpaceTraders
{
    public class Paginator
    {
        private readonly ILogger logger;

        public Paginator(ILogger<Paginator> logger)
        {
            this.logger = logger;
        }

        public Task<List<TModel>> GetAll<TResponse, TModel>(
            Func<int?, int?, CancellationToken, Task<TResponse>> apiCall,
            Func<TResponse, IEnumerable<TModel>> dataSelector
        ) => GetAll(apiCall, dataSelector, CancellationToken.None);
        
        public async Task<List<TModel>> GetAll<TResponse, TModel>(
            Func<int?, int?, CancellationToken, Task<TResponse>> apiCall,
            Func<TResponse, IEnumerable<TModel>> dataSelector,
            CancellationToken cancellationToken
        ) {
            Func<TResponse, Meta> metaSelector = CreateMetaSelector<TResponse>();
            List<TModel>? models = null;
            var meta = new Meta { Page = 0, Limit = 0, Total = 1 };
            while (meta.Page * meta.Limit < meta.Total)
            {
                TResponse response = await apiCall(meta.Page + 1, null, cancellationToken);
                meta = metaSelector(response);
                var totalPages = (meta.Total + meta.Limit - 1) / meta.Limit;
                logger.LogDebug("Loaded page {Page} of {TotalPages}.", meta.Page, totalPages);
                models ??= new(meta.Total);
                models.AddRange(dataSelector(response));
            }
            return models ?? new(0);
        }

        private static Func<TResponse, Meta> CreateMetaSelector<TResponse>()
        {
            var property = typeof(TResponse).GetProperty("Meta", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            if (property == null) throw new InvalidOperationException($"Type {typeof(TResponse).Name} does not have a getable 'Meta' property.");
            if (property.PropertyType != typeof(Meta)) throw new InvalidCastException($"'{typeof(TResponse).Name}.Meta' must be of type '{typeof(Meta)}'.");
            return response => (Meta)property.GetValue(response)!;
        }
    }
}
