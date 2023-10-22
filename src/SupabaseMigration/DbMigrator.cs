using System.Linq.Expressions;
using HomeTownPickEm.Data;
using Microsoft.EntityFrameworkCore;

public class DbMigrator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DbMigrator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    public int BatchSize { get; set; } = 1000;

    public async Task Migrate<TEntity,TResult>(Expression<Func<TEntity, TResult>> selector, 
        Func<IQueryable<TEntity>,IQueryable<TEntity>>? filter = null, Func<IEnumerable<TEntity>, PostgreSqlAppDbContext, Task>? savePostgres = null) where TEntity : class where TResult : notnull
    {
       
        var func = selector.Compile();
        
        //create the dbcontexts
        using var scope = _scopeFactory.CreateScope();
        var postgreSqlDbContext = scope.ServiceProvider.GetRequiredService<PostgreSqlAppDbContext>();
        var sqlLiteDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        
        var queryableFunc = filter ?? (q => q);
        var entities =  await queryableFunc(sqlLiteDbContext.Set<TEntity>())
            .ToDictionaryAsync(func);

        savePostgres ??= (_, context) => MigratePostgres(selector, context, entities);
        await savePostgres(entities.Values, postgreSqlDbContext);
        
    }

    private async Task MigratePostgres<TEntity, TResult>(Expression<Func<TEntity, TResult>> selector,
        PostgreSqlAppDbContext postgreSqlDbContext, Dictionary<TResult, TEntity> entities)
        where TEntity : class where TResult : notnull
    {
        var dbSet = postgreSqlDbContext.Set<TEntity>();

        var batchSize = BatchSize;
        var existingIds = (await dbSet.Select(selector).ToArrayAsync()).ToHashSet();
        var count = 0;

        foreach (var entity in entities)
        {
            if (existingIds.Contains(entity.Key))
            {
                dbSet.Update(entity.Value);
            }
            else
            {
                dbSet.Add(entity.Value);
            }

            count++;
            if (count % batchSize == 0)
            {
                await postgreSqlDbContext.SaveChangesAsync();
            }
        }

        await postgreSqlDbContext.SaveChangesAsync();
    }
}