using MediatR;
using web_api_lib_application.Logic.Dtos;
using web_api_lib_data.Models;

namespace web_api_lib_application.Infraestructure.Queries
{
    public record GetAllPermissionTaskQuery: IRequest<IEnumerable<PermissionDto>>;
    public record GetAllPermissionTypeTaskQuery : IRequest<IEnumerable<PermissionType>>;
}
