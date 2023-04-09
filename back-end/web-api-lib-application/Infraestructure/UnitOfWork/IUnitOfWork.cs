using web_api_lib_application.Infraestructure.Repository.Generic;
using web_api_lib_data.Models;

namespace web_api_lib_application.Infraestructure.UnitOfWork
{
    public interface IUnitOfWork
    {

        #region Repository References

        IGenericRepository<PermissionType> PermissionTypeRepository { get; }
        IGenericRepository<Permission> PermissionRepository { get; }
        #endregion

        /// <summary>
        /// Commit all changes
        /// </summary>
        int Complete();

        /// <summary>
        /// Commit all changes ASYNC
        /// </summary>
        Task<int> CompleteAsync();

        /// <summary>
        /// Commit all changes ASYNC
        /// </summary>
        Task<int> CompleteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();

        /// <summary>
        /// clean context
        /// </summary>
        void Dispose();
    }
}
