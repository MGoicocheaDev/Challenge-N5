using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Moq;
using Nest;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Dtos;
using web_api_lib_application.Logic.Handlers;
using web_api_lib_data.Models;

namespace web_api_test.Queries
{
    public class GetPermissionTaskHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<ProducerConfig> _producerConfigMock;
        public GetPermissionTaskHandlerTest()
        {
            _unitOfWorkMock = new();
            _elasticClientMock = new();
            _producerConfigMock = new();
        }

        [Test]
        public async Task Handler_should_GetAllPermissions()
        {
            var queryPermission = new GetAllPermissionTaskQuery();
            var handle = new GetAllPermissionTaskHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(x =>
               x.PermissionRepository.GetAllAsync())
               .ReturnsAsync(new List<Permission>
               {
                   new Permission
                   {
                       Id = 1,
                       NombreEmpleado = "Manuel",
                       ApellidoEmpleado = "Goicochea",
                       FechaPermiso = DateTime.Now,
                       PermissionTypes = new PermissionType
                       {
                           Id = 1,
                           Descripcion = "Read"
                       }
                   },
                   new Permission
                   {
                       Id = 1,
                       NombreEmpleado = "Other Name",
                       ApellidoEmpleado = "Other Last Name",
                       FechaPermiso = DateTime.Now,
                       PermissionTypes = new PermissionType
                       {
                           Id = 1,
                           Descripcion = "Read | Write"
                       }
                   }
               });

            var result = await handle.Handle(queryPermission, default);

            Assert.IsNotNull(result);
            Assert.That(result.Count, Is.GreaterThan(1));

        }

        [Test]
        public async Task Handler_should_GetPermissionById()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TopicName", "permission"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var queryPermission = new GetAllPermissionByIdTaskQuery(1);
            var handle = new GetPermissionByIdTaskHandler(_unitOfWorkMock.Object,
                _elasticClientMock.Object,
                _producerConfigMock.Object,
                configuration);

            _unitOfWorkMock.Setup(x =>
               x.PermissionRepository.FindByIdAsync(It.IsAny<object>()))
               .ReturnsAsync(new Permission
               {
                   Id = 1,
                   NombreEmpleado = "Manuel",
                   ApellidoEmpleado = "Goicochea",
                   FechaPermiso = DateTime.Now,
                   PermissionTypes = new PermissionType
                   {
                       Id = 1,
                       Descripcion = "Read"
                   }
               });

            /// Need to cread mock for Kafka component
            //_producerConfigMock.Setup(x =>
            //    x.)

            var result = await handle.Handle(queryPermission, default);

            Assert.IsNotNull(result);

        }
    }
}
