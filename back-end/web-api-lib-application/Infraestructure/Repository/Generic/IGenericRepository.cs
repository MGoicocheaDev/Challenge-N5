using System.Linq.Expressions;

namespace web_api_lib_application.Infraestructure.Repository.Generic
{
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// List all specific entities
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// List all specific entities ASYNC
        /// </summary>
        /// <returns></returns> 
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// List all specific entities ASYNC
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);


        /// <summary>
        /// List all specific entities by filters
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<T> GetAllByExpression(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// List all specific entities by filters ASYNC
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllByExpressionAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// List all specific entities by filters ASYNC
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllByExpressionAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Page listall specific entities
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        IEnumerable<T> PageAll(int skip, int take);

        /// <summary>
        /// Page listall specific entities ASYNC
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> PageAllAsync(int skip, int take);

        /// <summary>
        /// Page listall specific entities ASYNC
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> PageAllAsync(CancellationToken cancellationToken, int skip, int take);

        /// <summary>
        ///  Get specifci entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T FindById(object id);

        /// <summary>
        ///  Get specifci entity by id ASYNC
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync(object id);

        /// <summary>
        ///  Get specifci entity by id ASYNC
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> FindByIdAsync(CancellationToken cancellationToken, object id);

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// Create data
        /// </summary>
        /// <param name="entity"></param>
        void Add(T entity);

        /// <summary>
        /// Create data Async
        /// </summary>
        /// <param name="entity"></param>
        Task AddAsync(T entity);

        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);

    }
}
