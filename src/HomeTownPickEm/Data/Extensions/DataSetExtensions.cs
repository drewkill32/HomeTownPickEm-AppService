using System.Linq.Expressions;
using HomeTownPickEm.Application.Common;
using HomeTownPickEm.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeTownPickEm.Data.Extensions;

public static class DataSetExtensions
{
    public static void AddOrUpdateRange(this DbSet<Calendar> set,
        IEnumerable<Calendar> entities, Expression<Func<Calendar, bool>> whereClause)
    {
        var dbCalendars = set.Where(whereClause).ToArray();
        var comparer = new CalendarEqualityComparer();
        var existing = dbCalendars.Where(x => entities.Any(y => comparer.Equals(y, x))).ToArray();
        var @new = entities.Where(x => !dbCalendars.Any(y => comparer.Equals(y, x))).ToArray();
        if (existing.Any())
        {
            set.UpdateRange(existing);
        }

        if (@new.Any())
        {
            set.AddRange(@new);
        }
    }


    public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> set,
        IEnumerable<TEntity> entities)
        where TEntity : class, IHasId
    {
        set.AddOrUpdateRange(entities, x => x);
    }

    public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> set,
        IEnumerable<TEntity> entities, Func<IQueryable<TEntity>, IQueryable<TEntity>> query)
        where TEntity : class, IHasId
    {
        var dbEntities = query(set).ToArray();

        var ids = dbEntities.Select(x => x.Id).ToArray();
        var existing = entities.Where(x => ids.Contains(x.Id)).ToArray();
        var @new = entities.Where(x => !ids.Contains(x.Id)).ToArray();
        if (existing.Any())
        {
            set.UpdateRange(existing);
        }

        if (@new.Any())
        {
            set.AddRange(@new);
        }
    }
}