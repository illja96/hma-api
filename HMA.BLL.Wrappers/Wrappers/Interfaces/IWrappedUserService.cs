using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HMA.BLL.Wrappers.Wrappers.Interfaces
{
    public interface IWrappedUserService
    {
        Task<ObjectResult> RegisterAsync(CancellationToken cancellationToken = default);

        Task<ObjectResult> GetCurrentAsync(CancellationToken cancellationToken = default);

        Task<ObjectResult> UpdateCurrentAsync(CancellationToken cancellationToken = default);

        Task<StatusCodeResult> DeleteProfileAsync(CancellationToken cancellation = default);
    }
}
