﻿namespace Linn.Stores2.Proxy.HttpClients
{
    using System.Collections.Generic;

    public static class HttpHeaders
    {
        public static readonly IDictionary<string, string[]> AcceptJson =
            new Dictionary<string, string[]> { { "Accept", new[] { "application/json" } } };
    }
}
