using AcmeCorporation.Core.Entities;
using AcmeCorporation.Services.Helpers;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AcmeCorporation.Services.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage(ErrorMessages.NAME_REQUIRED);
            RuleFor(p => p.Age).NotEmpty().WithMessage(ErrorMessages.AGE_REQUIRED);
            RuleFor(p => p.Document).NotEmpty().WithMessage(ErrorMessages.DOCUMENT_REQUIERED);

            RuleFor(p => p.Name).MaximumLength(50).WithMessage(ErrorMessages.NAME_LENGTH);

            RuleFor(p => p.Document).MaximumLength(9).WithMessage(ErrorMessages.DOCUMENT_LENGTH);
            RuleFor(p => p.Document).Must(HasValidDocument).WithMessage(ErrorMessages.DOCUMENT_INVALID);

            RuleFor(p => p.Age).GreaterThanOrEqualTo(18).WithMessage(ErrorMessages.AGE_VALID);
        }

        private bool HasValidDocument(string document)
        {
            var DNI_REGEX = new Regex(RegularExpression.DNI_REGEX);
            var NIF_REGEX = new Regex(RegularExpression.NIF_REGEX);
            var NIE_REGEX = new Regex(RegularExpression.NIE_REGEX);

            return DNI_REGEX.IsMatch(document) || NIF_REGEX.IsMatch(document) || NIE_REGEX.IsMatch(document);
        }
    }
}