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
        private readonly Mock<IProducer<Null, string>> _producerMock;
        public GetPermissionTaskHandlerTest()
        {
            _unitOfWorkMock = new();
            _elasticClientMock = new();
            _producerMock = new();
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

            var produce = _producerMock.Object;
            var queryPermission = new GetAllPermissionByIdTaskQuery(1);
            var handle = new GetPermissionByIdTaskHandler(_unitOfWorkMock.Object,
                _elasticClientMock.Object,
                produce,
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

            int produceCount = 0;
            int flushCount = 0;
            _producerMock.Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<Message<Null,string>>(), It.IsAny<Action<DeliveryReport<Null,string>>>()))
                .Callback<string, Message<Null, string>, Action<DeliveryReport<Null, string>>>((topic, message,action) 
                => {
                    var result = new DeliveryReport<Null, string>
                    {
                        Topic = topic,
                        Partition = 0,
                        Offset = 0,
                        Error = new Error(ErrorCode.NoError),
                        Message = message,
                    };

                    action.Invoke(result);

                    produceCount++;
                });

            _producerMock.Setup(x => x.Flush(It.IsAny<TimeSpan>())).Returns(0).Callback(() => flushCount++);
            

            var result = await handle.Handle(queryPermission, default);

            Assert.IsNotNull(result);

        }
    }
}
