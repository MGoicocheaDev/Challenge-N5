using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using web_api_lib_application.Infraestructure.Commands;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Dtos;
using web_api_lib_application.Logic.KafkaEvent;
using web_api_lib_data.Models;

namespace web_api_lib_application.Logic.Handlers
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;
        private readonly IProducer<Null, string> _configuration;
        private readonly IConfiguration _config;
        public CreateTaskHandler(IUnitOfWork unitOfWork,
            IElasticClient elasticClient,
            IProducer<Null, string> configuration,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
            _configuration = configuration;
            _config = config;
        }

        public async Task<PermissionDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            // Save Request
            var resultSavePermission =await SaveNewRequest(request);

            //Save in Elastic
            await _elasticClient.IndexDocumentAsync(resultSavePermission);
            // Send to Kafka
            await KafkaProducer.SendMessage(_configuration, _config, "request");

            // return process
            return new PermissionDto
            {
                NombreEmpleado = resultSavePermission.NombreEmpleado,
                ApellidoEmpleado = resultSavePermission.ApellidoEmpleado,
                Id = resultSavePermission.Id,
                FechaPermiso = resultSavePermission.FechaPermiso ,
                PermissionTypeId = resultSavePermission.PermissionTypes.Id,
                PermissionTypeName = resultSavePermission.PermissionTypes.Descripcion
            };
        }

        private async Task<Permission> SaveNewRequest(CreateTaskCommand request)
        {
            var permissionType = await _unitOfWork.PermissionTypeRepository.FindByIdAsync(request.permissionTypeId);

            Permission permission = new Permission
            {
                NombreEmpleado = request.firstName,
                ApellidoEmpleado = request.lastName,
                FechaPermiso = DateTime.Now,
                PermissionTypes = permissionType
            };

            await _unitOfWork.PermissionRepository.AddAsync(permission);

            await _unitOfWork.CompleteAsync();

            return permission;
        }
    }
}
