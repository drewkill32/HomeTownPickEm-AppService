﻿using HomeTownPickEm.Application.Common;

namespace HomeTownPickEm.Services;

public class DevSystemDate : ISystemDate
{
    private DateTimeOffset? _now;

    public void SetNow(DateTimeOffset now)
    {
        _now = now;
    }

    public DateTimeOffset Now => _now ?? DateTimeOffset.Now;

    public DateTimeOffset UtcNow => _now ?? DateTimeOffset.UtcNow;
    public string Year => _now.HasValue ? _now.Value.Year.ToString() : DateTimeOffset.Now.Year.ToString();
}