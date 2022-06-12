using AcmeCorporation.Core.Entities;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Interfaces.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AcmeCorporation.Services.Tests
{
    public class PersonServiceTest
    {
        private readonly Mock<IDocumentTypeService> _documentTypeService;
        private readonly PersonService _personService;
        private readonly Mock<IUnitOfWork> _unitOfWork;

        public PersonServiceTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _documentTypeService = new Mock<IDocumentTypeService>();
            _personService = new PersonService(_unitOfWork.Object, _documentTypeService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAListOfPersons()
        {
            // Arrange
            var vehicleAdministrations = new List<Person>()
            {
                new Person
                {
                    Id = 1,
                    Name = "TEST PERSONA",
                    Document = "000000T",
                    Age = 18,
                    DocumentType = DocumentType.DNI,
                }
            };
            _unitOfWork.Setup(x => x.PersonRepository.GetAllAsync()).ReturnsAsync(vehicleAdministrations);

            // Act
            var result = await _personService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Person>>(result);
        }
    }
}