using AcmeCorporation.Core.Entities;
using AcmeCorporation.Core.Interfaces.Services;
using AcmeCorporation.Services.Helpers;
using System.Text.RegularExpressions;

namespace AcmeCorporation.Services
{
    public class DocumentTypeService : IDocumentTypeService
    {
        public DocumentType GetDocumentType(string document)
        {
            DocumentType documentType = DocumentType.UNDEFINED;

            var DOCUMENT_REGEX = new Regex(RegularExpression.DNI_REGEX);

            if (DOCUMENT_REGEX.IsMatch(document)) return DocumentType.DNI;

            DOCUMENT_REGEX = new Regex(RegularExpression.NIF_REGEX);
            if (DOCUMENT_REGEX.IsMatch(document)) return DocumentType.NIF;

            DOCUMENT_REGEX = new Regex(RegularExpression.NIE_REGEX);
            if (DOCUMENT_REGEX.IsMatch(document)) return DocumentType.NIE;

            return documentType;
        }
    }
}