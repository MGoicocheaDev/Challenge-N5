using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Moq;
using Nest;
using web_api_lib_application.Infraestructure.Commands;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Handlers;
using web_api_lib_data.Models;

namespace web_api_test.Commands
{
    public class UpdateTaskCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<IProducer<Null, string>> _producerMock;

        public UpdateTaskCommandHandlerTests()
        {
            _unitOfWorkMock = new();
            _elasticClientMock = new();
            _producerMock = new();
        }

        [Test]
        public async Task Handler_Should_UpdatePermission()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TopicName", "permission"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var command = new UpdateTaskCommand(1,"Manuel", "Goicochea", 2);
            var produce = _producerMock.Object;

            _unitOfWorkMock.Setup(x =>
                x.PermissionRepository.FindByIdAsync(It.IsAny<object>()))
                .ReturnsAsync(new Permission { Id = 1, NombreEmpleado = "Manuel", ApellidoEmpleado = "Goicochea", PermissionTypes = new PermissionType { Id = 1, Descripcion = "Read" } });

            _unitOfWorkMock.Setup(x =>
                x.PermissionTypeRepository.FindByIdAsync(It.IsAny<object>()))
                .ReturnsAsync(new PermissionType { Id = 2, Descripcion = "Read|Write" });

            //_unitOfWorkMock.Setup(x =>
            //    x.PermissionRepository.Update(It.IsAny<Permission>()))
            //    .Callback<Permission>(permission =>
            //    {
            //        permission.Id = 1;
            //        permission.FechaPermiso = DateTime.Now;
            //    });

            var handler = new UpdateTaskHandler(_unitOfWorkMock.Object, _elasticClientMock.Object, produce, configuration);



            int produceCount = 0;
            int flushCount = 0;
            _producerMock.Setup(x => x.Produce(It.IsAny<string>(), It.IsAny<Message<Null, string>>(), It.IsAny<Action<DeliveryReport<Null, string>>>()))
                .Callback<string, Message<Null, string>, Action<DeliveryReport<Null, string>>>((topic, message, action)
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

            var result = await handler.Handle(command, default);

            _unitOfWorkMock.Verify(x => x.PermissionRepository.Update(It.Is<Permission>(mbox => mbox.Id == result.Id)), Times.Once);


            Assert.IsNotNull(result);
        }
    }
}
