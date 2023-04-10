using Moq;
using Nest;
using web_api_lib_application.Infraestructure.Queries;
using web_api_lib_application.Infraestructure.UnitOfWork;
using web_api_lib_application.Logic.Handlers;
using web_api_lib_data.Models;

namespace web_api_test.Queries
{
    public class GetAllPermissionTypesTaskHandlerTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public GetAllPermissionTypesTaskHandlerTest()
        {
            _unitOfWorkMock = new();
        }

        [Test]
        public async Task Handler_Should_GetAllPermissionTypesAsync()
        {

            var queryPermissionType = new GetAllPermissionTypeTaskQuery();

            var handler = new GetAllPermissionTypeTaskHandler(_unitOfWorkMock.Object);

            _unitOfWorkMock.Setup(x =>
               x.PermissionTypeRepository.GetAllAsync())
               .ReturnsAsync(new List<PermissionType>
               {
                   new PermissionType
                   {
                       Id = 1,
                       Descripcion = "Read"
                   },
                   new PermissionType {
                       Id = 2,
                       Descripcion = "Read | Write"
                   }
               });

            var result = await handler.Handle(queryPermissionType, default);

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.GreaterThan(1));
        }
    }
}
