using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Moq;
using Nest;
using web_api_lib_application.Infraestructure.Commands;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Handlers;

namespace web_api_test.Commands
{
    public class CreateTaskCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IElasticClient> _elasticClientMock;
        private readonly Mock<ProducerConfig> _producerConfigMock;

        public CreateTaskCommandHandlerTests()
        {
            _unitOfWorkMock = new();
            _elasticClientMock = new ();
            _producerConfigMock = new ();
        }

        [Test]
        public async Task Handler_Should_CreatePermission()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TopicName", "permission"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var command = new CreateTaskCommand("Manuel", "Goicochea", 1);

            _unitOfWorkMock.Setup(x => 
                x.PermissionTypeRepository.FindByIdAsync(It.IsAny<object>()))
                .ReturnsAsync(new web_api_lib_data.Models.PermissionType { Id = 1, Descripcion = "Read"});

            var handler = new CreateTaskHandler(_unitOfWorkMock.Object, _elasticClientMock.Object, _producerConfigMock.Object, configuration);

            var result = await handler.Handle(command, default);

            Assert.Pass();
        }
    }
}
