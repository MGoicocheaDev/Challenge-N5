using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Nest;
using Newtonsoft.Json;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Dtos;
using web_api_lib_application.Logic.KafkaEvent;

namespace web_api_lib_application.Logic.Handlers
{
    public class GetPermissionByIdTaskHandler : IRequestHandler<GetAllPermissionByIdTaskQuery, PermissionDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IElasticClient _elasticClient;
        private readonly IProducer<Null, string> _configuration;
        private readonly IConfiguration _config;
        public GetPermissionByIdTaskHandler(IUnitOfWork unitOfWork,
            IElasticClient elasticClient,
            IProducer<Null, string> configuration,
            IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _elasticClient = elasticClient;
            _configuration = configuration;
            _config = config;
        }
        public async Task<PermissionDto> Handle(GetAllPermissionByIdTaskQuery request, CancellationToken cancellationToken)
        {
            var permission = await _unitOfWork.PermissionRepository.FindByIdAsync(request.Id);
            //Save in Elastic
            await _elasticClient.IndexDocumentAsync(permission);

            // Send to Kafka
            await KafkaProducer.SendMessage(_configuration, _config, "get");
            
            // return process

            return new PermissionDto
            {
                Id = permission.Id,
                NombreEmpleado = permission.NombreEmpleado,
                ApellidoEmpleado = permission.ApellidoEmpleado,
                FechaPermiso = permission.FechaPermiso,
                PermissionTypeId = permission.PermissionTypes.Id,
                PermissionTypeName = permission.PermissionTypes.Descripcion
            };
        }
    }
}
