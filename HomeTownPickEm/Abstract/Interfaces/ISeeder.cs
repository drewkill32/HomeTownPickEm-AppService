using System.Threading;
using System.Threading.Tasks;

namespace HomeTownPickEm.Abstract.Interfaces
{
    public interface ISeeder
    {
        Task Seed(CancellationToken cancellationToken);
    }
}