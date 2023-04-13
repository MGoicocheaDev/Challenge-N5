using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using web_api_lib_application.Infraestructure.Commands;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Dtos;
using web_api_lib_application.Logic.KafkaEvent;
using web_api_lib_data.Models;

namespace web_api_lib_application.Logic.Handlers
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;
        private readonly IProducer<Null, string> _configuration;
        private readonly IConfiguration _config;
        public UpdateTaskHandler(IUnitOfWork unitOfWork,
              IElasticClient elasticClient,
            IProducer<Null, string> configuration,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork; 
            _elasticClient = elasticClient;
            _configuration = configuration;
            _config = config;
        }

        public async Task<PermissionDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            //Update permission
            var resultUpdatePermission = await UpdatePermission(request);

            //Save in Elastic
            await _elasticClient.IndexDocumentAsync(resultUpdatePermission);

            // Send to Kafka
            await KafkaProducer.SendMessage(_configuration, _config, "modify");

            // return process
            return new PermissionDto
            {
                NombreEmpleado = resultUpdatePermission.NombreEmpleado,
                ApellidoEmpleado = resultUpdatePermission.ApellidoEmpleado,
                Id = resultUpdatePermission.Id,
                FechaPermiso = resultUpdatePermission.FechaPermiso,
                PermissionTypeId = resultUpdatePermission.PermissionTypes.Id,
                PermissionTypeName = resultUpdatePermission.PermissionTypes.Descripcion
            };
        }

        private async Task<Permission> UpdatePermission(UpdateTaskCommand request)
        {
            var permission = await _unitOfWork.PermissionRepository.FindByIdAsync(request.id);
            var permissionType = await _unitOfWork.PermissionTypeRepository.FindByIdAsync(request.permissionTypeId);

            permission.NombreEmpleado = request.firstName;
            permission.ApellidoEmpleado = request.lastName;
            permission.PermissionTypes = permissionType;
            permission.FechaPermiso = DateTime.Now;

            _unitOfWork.PermissionRepository.Update(permission);
            await _unitOfWork.CompleteAsync();

            return permission;
        }
    }
}
