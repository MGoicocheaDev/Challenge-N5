using MediatR;
using web_api_lib_application.Logic.Dtos;

namespace web_api_lib_application.Infraestructure.Queries
{
    public record GetAllPermissionByIdTaskQuery(int Id): IRequest<PermissionDto>;
}
