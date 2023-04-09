using MediatR;
using Microsoft.AspNetCore.Mvc;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_data.Models;

namespace web_api_permision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionTypesController(IMediator mediator)
        {
            _mediator = mediator;   
        }

        /// <summary>
        /// Get all permission types
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<PermissionType>> GetAll()
        {
            return await _mediator.Send(new GetAllPermissionTypeTaskQuery());
        }
    }
}
