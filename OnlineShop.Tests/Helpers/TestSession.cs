namespace OnlineShop.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class TestSession : ISession
    {
        private Dictionary<string, byte[]> _store
        = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        public bool IsAvailable => true;

        public string Id {get; set;}

        public IEnumerable<string> Keys => _store.Keys;

        public void Clear()
        {
            _store.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }

        public void Remove(string key)
        {
            _store.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _store[key] = value;
        }

        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
        {
            return _store.TryGetValue(key, out value);
        }
    }
}
