using AcmeCorporation.Core.Entities;

namespace AcmeCorporation.Core.Interfaces.Services
{
    public interface IDocumentTypeService
    {
        DocumentType GetDocumentType(string document);
    }
}