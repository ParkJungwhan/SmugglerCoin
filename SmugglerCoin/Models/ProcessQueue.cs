using System.Collections.Concurrent;
using System.Diagnostics;

namespace SmugglerCoin.Models
{
    public sealed class ProcessQueue<T, TPrio> where TPrio : IComparable<TPrio> where T : class
    {
        private readonly PriorityQueue<T, TPrio> _pq = new();
        private readonly object _gate = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(T item, TPrio priority)
        {
            lock (_gate) _pq.Enqueue(item, priority);
            _signal.Release();
        }

        public async ValueTask<T> DequeueAsync(CancellationToken ct = default)
        {
            while (true)
            {
                lock (_gate)
                {
                    if (_pq.TryDequeue(out var item, out _))
                        return item;
                }
                await _signal.WaitAsync(ct);
            }
        }

        public int Count
        { get { lock (_gate) return _pq.Count; } }
    }
}