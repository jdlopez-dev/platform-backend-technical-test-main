using AcmeCorporation.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcmeCorporation.Core.Interfaces.Services
{
    public interface IPersonService
    {
        Task<Person> CreatePerson(Person newPerson);

        Task DeletePerson(int personId);

        Task<IEnumerable<Person>> GetAll();

        Task<Person> GetPersonById(int id);

        Task<Person> UpdatePerson(int personToBeUpdatedId, Person newPersonValues);
    }
}