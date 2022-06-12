using AcmeCorporation.Core.Entities;
using AcmeCorporation.Core.Interfaces.Repository;
using AcmeCorporation.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AcmeCorporation.Infrastructure.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckNameExists(string name)
        {
            return await dbSet.AnyAsync(p => p.Name == name);
        }
    }
}