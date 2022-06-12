using AcmeCorporation.Core.Entities;
using AcmeCorporation.Core.Interfaces.Services;
using Moq;
using System;
using Xunit;

namespace AcmeCorporation.Services.Tests
{
    public class DocumentTypeServiceTest
    {
        private readonly DocumentTypeService _documentTypeService;

        public DocumentTypeServiceTest()
        {
            _documentTypeService = new DocumentTypeService();
        }

        [Fact]
        public void Get_DNI_type()
        {
            // Arrage
            string document = "00000000T";

            //Act
            var document_result = _documentTypeService.GetDocumentType(document);

            // Assert
            Assert.Equal(DocumentType.DNI, document_result);
        }

        [Fact]
        public void Get_NIE_type()
        {
            // Arrage
            string document = "X8102676Y";

            //Act
            var document_result = _documentTypeService.GetDocumentType(document);

            // Assert
            Assert.Equal(DocumentType.NIE, document_result);
        }

        [Fact]
        public void Get_NIF_type()
        {
            // Arrage
            string document = "A53475943";

            //Act
            var document_result = _documentTypeService.GetDocumentType(document);

            // Assert
            Assert.Equal(DocumentType.NIF, document_result);
        }
    }
}