using AcmeCorporation.Core.Entities;
using System;
using System.Threading.Tasks;

namespace AcmeCorporation.Core.Interfaces.Repository
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<bool> CheckNameExists(String name);
    }
}