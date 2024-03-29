﻿using System.Diagnostics;
using System.Reflection;
using HomeTownPickEm.Application.Attributes;
using HomeTownPickEm.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace HomeTownPickEm.Services;

public class BackgroundWorker : BackgroundService
{
    private readonly BackgroundWorkerQueue _queue;
    private readonly IHubContext<CacheHub, ICacheClient> _hubContext;
    private readonly IServiceScopeFactory _scopeFactory;

    public BackgroundWorker(BackgroundWorkerQueue queue,
        IHubContext<CacheHub, ICacheClient> hubContext,
        IServiceScopeFactory scopeFactory)
    {
        _queue = queue;
        _hubContext = hubContext;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _queue.DequeueAsync(ProcessQueue, stoppingToken);
        }
    }

    private async Task ProcessQueue(IRequest workItem, CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BackgroundWorker>>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var type = workItem.GetType().Name;
        try
        {
            logger.LogInformation("Starting Background Task {WorkItem}", workItem);
            var sw = Stopwatch.StartNew();
            await mediator.Send(workItem, stoppingToken);
            sw.Stop();
            logger.LogInformation("Completed Background Task {WorkItem} in {Elapsed} seconds", type,
                sw.Elapsed.TotalSeconds.ToString("N"));
            if (workItem.GetType().GetCustomAttribute<CacheRefreshAttribute>() != null)
            {
                await _hubContext.Clients.All.RefreshCache();
            }
  
         
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error in Background Task {WorkItem}. {ErrorMessage}", type,
                e.Message);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _queue.Dispose();
    }
}