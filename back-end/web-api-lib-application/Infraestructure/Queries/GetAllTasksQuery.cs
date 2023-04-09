using MediatR;
using web_api_lib_data.Models;

namespace web_api_lib_application.Infraestructure.Queries
{
    public record GetAllPermissionTaskQuery: IRequest<IEnumerable<Permission>>;
    public record GetAllPermissionTypeTaskQuery : IRequest<IEnumerable<PermissionType>>;
}
