using AcmeCorporation.Core.Entities;
using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Interfaces.Services;
using AcmeCorporation.Services.Helpers;
using AcmeCorporation.Services.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeCorporation.Services
{
    public class PersonService : IPersonService
    {
        private readonly IDocumentTypeService _documentTypeService;
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork, IDocumentTypeService documentTypeService)
        {
            _unitOfWork = unitOfWork;
            _documentTypeService = documentTypeService;
        }

        public async Task<Person> CreatePerson(Person newPerson)
        {
            PersonValidator validator = new();
            var validationResult = await validator.ValidateAsync(newPerson);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(string.Join(";", validationResult.Errors));
            }

            var checkName = await _unitOfWork.PersonRepository.CheckNameExists(newPerson.Name);

            if (checkName)
            {
                throw new ArgumentException(ErrorMessages.NAME_EXISTS);
            }

            var documentType = _documentTypeService.GetDocumentType(newPerson.Document);
            newPerson.DocumentType = documentType;

            await _unitOfWork.PersonRepository.AddAsync(newPerson);
            await _unitOfWork.CommitAsync();

            return newPerson;
        }

        public async Task DeletePerson(int personId)
        {
            var vehicleAdministration = await _unitOfWork.PersonRepository.GetByIdAsync(personId);
            _unitOfWork.PersonRepository.Remove(vehicleAdministration);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<Person>> GetAll()
        {
            return await _unitOfWork.PersonRepository.GetAllAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await _unitOfWork.PersonRepository.GetByIdAsync(id);
        }

        public Task<Person> UpdatePerson(int personToBeUpdatedId, Person newPersonValues)
        {
            throw new NotImplementedException();
        }
    }
}