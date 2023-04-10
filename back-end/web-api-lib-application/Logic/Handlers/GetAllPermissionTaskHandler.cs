using MediatR;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Dtos;
using web_api_lib_data.Models;

namespace web_api_lib_application.Logic.Handlers
{
    public class GetAllPermissionTaskHandler : IRequestHandler<GetAllPermissionTaskQuery, IEnumerable<PermissionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllPermissionTaskHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public async Task<IEnumerable<PermissionDto>> Handle(GetAllPermissionTaskQuery request, CancellationToken cancellationToken)
        {
            var permissions =  await _unitOfWork.PermissionRepository.GetAllAsync();

            return permissions
                .Select(x =>
                new PermissionDto
                {
                    NombreEmpleado = x.NombreEmpleado,
                    ApellidoEmpleado = x.ApellidoEmpleado,
                    Id = x.Id,
                    FechaPermiso = x.FechaPermiso,
                    PermissionTypeId = x.PermissionTypes.Id,
                    PermissionTypeName = x.PermissionTypes.Descripcion
                }).ToList();
        }
    }
}
