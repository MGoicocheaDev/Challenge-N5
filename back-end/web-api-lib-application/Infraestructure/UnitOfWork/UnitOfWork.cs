using Microsoft.EntityFrameworkCore;
using web_api_lib_application.Infraestructure.Repository.Generic;
using web_api_lib_data.Context;
using web_api_lib_data.Models;

namespace web_api_lib_application.Infraestructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebApiContext _context;

        public UnitOfWork(WebApiContext context)
        {
            _context = context;
        }

        public IGenericRepository<PermissionType> _permissionTypeRepository;

        public IGenericRepository<Permission> _permissionRepository;


        public IGenericRepository<PermissionType> PermissionTypeRepository
        {
            get { return _permissionTypeRepository ?? (_permissionTypeRepository = new GenericRepository<PermissionType>(_context)); }
        }

        public IGenericRepository<Permission> PermissionRepository
        {
            get { return _permissionRepository ?? (_permissionRepository = new GenericRepository<Permission>(_context)); }
        }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void RejectChanges()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                      .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }

        }
    }
}
