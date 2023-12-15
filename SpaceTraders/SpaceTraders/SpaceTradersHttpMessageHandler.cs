namespace SpaceTraders
{
    internal class SpaceTradersHttpMessageHandler : DelegatingHandler
    {
        private readonly object rateLimitResetLock = new();

        private DateTime rateLimitReset = DateTime.MinValue;

        public SpaceTradersHttpMessageHandler()
            : base(new HttpClientHandler())
        { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await WaitForRateLimitReset(cancellationToken);
            var response = await base.SendAsync(request, cancellationToken);
            UpdateRateLimitReset(response);
            return response;
        }

        private async Task WaitForRateLimitReset(CancellationToken cancellationToken)
        {
            TimeSpan delay;

            lock (rateLimitResetLock)
                delay = rateLimitReset - DateTime.UtcNow;

            if (delay > TimeSpan.Zero)
                await Task.Delay(delay, cancellationToken);
        }

        private void UpdateRateLimitReset(HttpResponseMessage response)
        {
            if (!HasRateLimitRemaining(response))
            {
                lock (rateLimitResetLock)
                {
                    rateLimitReset = GetRateLimitReset(response);
                }
            }
        }

        private static bool HasRateLimitRemaining(HttpResponseMessage response)
        {
            var remaining = false;
            if (response.Headers.TryGetValues("x-ratelimit-remaining", out var values))
            {
                var list = values.ToList();
                if (list.Count == 1 && int.TryParse(list[0], out var value))
                {
                    remaining = value > 0;
                }
            }
            return remaining;
        }

        private static DateTime GetRateLimitReset(HttpResponseMessage response)
        {
            var rateLimit = DateTime.MinValue;
            if (response.Headers.TryGetValues("x-ratelimit-reset", out var values))
            {
                var list = values.ToList();
                if (list.Count == 1 && DateTime.TryParse(list[0], out var value))
                {
                    rateLimit = value.ToUniversalTime();
                }
            }
            return rateLimit;
        }
    }
}