using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using System.Security;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.KafkaEvent;
using web_api_lib_data.Models;

namespace web_api_lib_application.Logic.Handlers
{
    public class GetPermissionByIdTaskHandler : IRequestHandler<GetAllPermissionByIdTaskQuery, Permission>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;
        private readonly ProducerConfig _configuration;
        private readonly IConfiguration _config;
        public GetPermissionByIdTaskHandler(IUnitOfWork unitOfWork,
            IElasticClient elasticClient,
            ProducerConfig configuration,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
            _configuration = configuration;
            _config = config;
        }
        public async Task<Permission> Handle(GetAllPermissionByIdTaskQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.PermissionRepository.FindByIdAsync(request.Id); 
            //Save in Elastic
            await _elasticClient.IndexDocumentAsync(permission);

            // Send to Kafka

            var kafkaMessage = JsonConvert.SerializeObject(new { Id = Guid.NewGuid(), Operation = "get" });
            var topic = _config.GetSection("TopicName").Value;

            await KafkaProducer.SendMessage(_configuration, topic, kafkaMessage);

            return permission;        }
    }
}
