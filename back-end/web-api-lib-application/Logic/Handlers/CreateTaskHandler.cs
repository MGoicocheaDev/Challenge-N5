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
        private readonly ProducerConfig _configuration;
        private readonly IConfiguration _config;
        public CreateTaskHandler(IUnitOfWork unitOfWork,
            IElasticClient elasticClient,
            ProducerConfig configuration,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
            _configuration = configuration;
            _config = config;
        }

        public async Task<PermissionDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
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

            //Save in Elastic
            await _elasticClient.IndexDocumentAsync(permission);
            // Send to Kafka

            var kafkaMessage = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Operation = "request" });
            var topic = _config.GetSection("TopicName").Value;

            await KafkaProducer.SendMessage(_configuration, topic, kafkaMessage);

            return new PermissionDto
            {
                NombreEmpleado = permission.NombreEmpleado,
                ApellidoEmpleado = permission.ApellidoEmpleado,
                Id = permission.Id,
                FechaPermiso = permission.FechaPermiso ,
                PermissionTypeId = permissionType.Id,
                PermissionTypeName = permissionType.Descripcion
            };
        }
    }
}
