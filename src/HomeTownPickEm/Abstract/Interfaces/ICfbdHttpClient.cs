using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HomeTownPickEm.Services.Cfbd;

namespace HomeTownPickEm.Abstract.Interfaces
{
    public interface ICfbdHttpClient
    {
        Task<IEnumerable<GameResponse>> GetGames(GameRequest request, CancellationToken cancellationToken);
    }
}