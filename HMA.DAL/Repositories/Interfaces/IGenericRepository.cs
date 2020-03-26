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
        /// <summary>
        /// Find models
        /// </summary>
        /// <param name="filterDefinition">Filter definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<T>> FindAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find models
        /// </summary>
        /// <typeparam name="TOut">Output model type</typeparam>
        /// <param name="pipelineDefinition">Pipeline definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<TOut>> FindAsync<TOut>(
            PipelineDefinition<T, TOut> pipelineDefinition,
            CancellationToken cancellationToken = default)
            where TOut : class;

        /// <summary>
        /// Find model
        /// </summary>
        /// <param name="filterDefinition">Filter definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> FindOneAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Find model
        /// </summary>
        /// <typeparam name="TOut">Output model type</typeparam>
        /// <param name="pipelineDefinition">Pipeline definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TOut> FindOneAsync<TOut>(
            PipelineDefinition<T, TOut> pipelineDefinition,
            CancellationToken cancellationToken = default)
            where TOut : class;

        /// <summary>
        /// Insert model
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> InsertAsync(
            T model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Insert models
        /// </summary>
        /// <param name="models">Models</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<T>> InsertManyAsync(
            List<T> models,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update model
        /// </summary>
        /// <param name="filterDefinition">Filter definition</param>
        /// <param name="updateDefinition">Update definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<T> UpdateOneAsync(
            FilterDefinition<T> filterDefinition,
            UpdateDefinition<T> updateDefinition,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete model
        /// </summary>
        /// <param name="filterDefinition">Filter definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> DeleteAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Clear models
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> ClearAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Count models
        /// </summary>
        /// <param name="filterDefinition">Filter definition</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> CountAsync(
            FilterDefinition<T> filterDefinition,
            CancellationToken cancellationToken = default);
    }
}
