using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using web_api_lib_application.Infraestructure.Commands;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_data.Models;

namespace web_api_permision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermisionsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        /// <summary>
        /// Get All permisions
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IEnumerable<Permission>> GetAll()
        {
            return await _mediator.Send(new GetAllPermissionTaskQuery());
        }

        /// <summary>
        /// Get permission by Id
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("{id}")]
        public async Task<Permission> Get(int id)
        {
            return await _mediator.Send(new GetAllPermissionByIdTaskQuery(id));
        }


        /// <summary>
        /// Modify Permision
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<Permission>> Put(UpdateTaskCommand command)
        {
            var request = await _mediator.Send(command);
            return Ok(request);
        }

        /// <summary>
        /// Request Permision
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Permission>> Post(CreateTaskCommand command)
        {
            var request = await _mediator.Send(command);
            return Ok(request);
        }
    }
}
