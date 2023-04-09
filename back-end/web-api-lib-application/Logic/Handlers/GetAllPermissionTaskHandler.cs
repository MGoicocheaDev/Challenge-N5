using MediatR;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_data.Models;

namespace web_api_lib_application.Logic.Handlers
{
    public class GetAllPermissionTaskHandler : IRequestHandler<GetAllPermissionTaskQuery, IEnumerable<Permission>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllPermissionTaskHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<IEnumerable<Permission>> Handle(GetAllPermissionTaskQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PermissionRepository.GetAllAsync();
        }
    }
}
