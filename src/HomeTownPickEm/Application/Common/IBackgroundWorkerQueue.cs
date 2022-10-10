using MediatR;

namespace HomeTownPickEm.Application.Common;

public interface IBackgroundWorkerQueue
{
    bool Queue(string key, IRequest request);
}