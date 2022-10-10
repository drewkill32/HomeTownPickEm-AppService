using System.Collections.Concurrent;
using HomeTownPickEm.Application.Common;
using MediatR;

namespace HomeTownPickEm.Services;

public class BackgroundWorkerQueue : IDisposable, IBackgroundWorkerQueue
{
    private readonly ConcurrentQueue<string> _keyQueue = new();
    private readonly ConcurrentDictionary<string, IRequest> _queue = new();
    private readonly SemaphoreSlim _semaphore = new(0);
    private readonly object _locker = new();

    public static BackgroundWorkerQueue Instance { get; } = new();

    public async Task DequeueAsync(Func<IRequest, CancellationToken, Task> process, CancellationToken cancellationToken)
    {
        if (process == null)
        {
            throw new ArgumentNullException(nameof(process));
        }

        await _semaphore.WaitAsync(cancellationToken);
        _keyQueue.TryDequeue(out var key);
        _queue.TryGetValue(key, out var request);
        try
        {
            await process(request, cancellationToken);
        }
        finally
        {
            _queue.TryRemove(key, out _);
        }
        
    }

    public bool Queue(string key, IRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        lock (_locker)
        {
            if (!_queue.TryAdd(key, request))
            {
                return false;
            }

            _keyQueue.Enqueue(key);
            _semaphore.Release();
        }

        return true;
    }

    public void Dispose()
    {
        _semaphore?.Dispose();
    }
}