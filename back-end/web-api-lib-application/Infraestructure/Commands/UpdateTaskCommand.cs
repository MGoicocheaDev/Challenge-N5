using MediatR;
using web_api_lib_data.Models;

namespace web_api_lib_application.Infraestructure.Commands
{
    public record UpdateTaskCommand(int id,string firstName, string lastName, int permissionTypeId): IRequest<Permission>;
}
