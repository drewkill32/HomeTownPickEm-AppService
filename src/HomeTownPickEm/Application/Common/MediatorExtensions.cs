using HomeTownPickEm.Services;
using MediatR;

namespace HomeTownPickEm.Application.Common;

public static class MediatorExtensions
{
    public static bool Enqueue(this IMediator _, string key, IRequest request)
    {
        return BackgroundWorkerQueue.Instance.Queue(key, request);
    }
}