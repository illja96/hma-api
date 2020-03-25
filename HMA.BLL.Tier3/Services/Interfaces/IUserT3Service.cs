using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace HMA.BLL.Tier3.Services.Interfaces
{
    public interface IUserT3Service
    {
        Task<ObjectResult> RegisterAsync(CancellationToken cancellationToken = default);

        Task<ObjectResult> GetCurrentAsync(CancellationToken cancellationToken = default);

        Task<ObjectResult> UpdateCurrentAsync(CancellationToken cancellationToken = default);

        Task<StatusCodeResult> DeleteProfileAsync(CancellationToken cancellation = default);
    }
}
