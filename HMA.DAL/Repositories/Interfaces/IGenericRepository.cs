using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HMA.DTO.Models.Base;
using MongoDB.Driver;

namespace HMA.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T>
        where T : BaseDalModel
    {
        Task<List<T>> FindAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);

        Task<T> FindOneAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);

        Task<T> InsertAsync(
            T model,
            CancellationToken cancellationToken = default);

        Task<List<T>> InsertManyAsync(
            List<T> models,
            CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(
            FilterDefinition<T> filterDefinition,
            UpdateDefinition<T> updateDefinition,
            CancellationToken cancellationToken = default);

        Task<long> DeleteAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);

        Task<long> ClearAsync(CancellationToken cancellationToken = default);

        Task<long> CountAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);
    }
}
