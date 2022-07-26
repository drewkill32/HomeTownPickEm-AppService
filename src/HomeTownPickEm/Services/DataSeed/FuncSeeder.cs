using HomeTownPickEm.Abstract.Interfaces;
using HomeTownPickEm.Data;

namespace HomeTownPickEm.Services.DataSeed;

public class FuncSeeder : ISeeder
{
    private readonly ApplicationDbContext _context;
    private readonly Func<ApplicationDbContext, CancellationToken, Task> _execute;


    public FuncSeeder(ApplicationDbContext context, Func<ApplicationDbContext, CancellationToken, Task> execute)
    {
        _context = context;
        _execute = execute;
    }

    public Task Seed(CancellationToken cancellationToken)
    {
        return _execute(_context, cancellationToken);
    }
}