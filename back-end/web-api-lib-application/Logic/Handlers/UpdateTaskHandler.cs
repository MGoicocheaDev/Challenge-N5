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
        private readonly ProducerConfig _configuration;
        private readonly IConfiguration _config;
        public UpdateTaskHandler(IUnitOfWork unitOfWork,
              IElasticClient elasticClient,
            ProducerConfig configuration,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork; 
            _elasticClient = elasticClient;
            _configuration = configuration;
            _config = config;
        }

        public async Task<PermissionDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {

            var permission = await _unitOfWork.PermissionRepository.FindByIdAsync(request.id);
            var permissionType = await _unitOfWork.PermissionTypeRepository.FindByIdAsync(request.permissionTypeId);

            permission.NombreEmpleado = request.firstName;
            permission.ApellidoEmpleado = request.lastName;
            permission.PermissionTypes = permissionType;
            permission.FechaPermiso = DateTime.Now;

            _unitOfWork.PermissionRepository.Update(permission);
            await _unitOfWork.CompleteAsync();

            //Save in Elastic
            await _elasticClient.IndexDocumentAsync(permission);

            // Send to Kafka

            var kafkaMessage = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Operation = "modify" });
            var topic = _config.GetSection("TopicName").Value;

            await KafkaProducer.SendMessage(_configuration, topic, kafkaMessage);
            return new PermissionDto
            {
                NombreEmpleado = permission.NombreEmpleado,
                ApellidoEmpleado = permission.ApellidoEmpleado,
                Id = permission.Id,
                FechaPermiso = permission.FechaPermiso,
                PermissionTypeId = permissionType.Id,
                PermissionTypeName = permissionType.Descripcion
            };

        }
    }
}
