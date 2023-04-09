using MediatR;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_data.Models;

namespace web_api_lib_application.Logic.Handlers
{
    public class GetAllPermissionTypeTaskHandler : IRequestHandler<GetAllPermissionTypeTaskQuery, IEnumerable<PermissionType>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPermissionTypeTaskHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
        public async Task<IEnumerable<PermissionType>> Handle(GetAllPermissionTypeTaskQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.PermissionTypeRepository.GetAllAsync();
        }
    }
}
