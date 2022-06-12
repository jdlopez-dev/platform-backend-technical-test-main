using AcmeCorporation.Core.Interfaces;
using AcmeCorporation.Core.Interfaces.Repository;
using AcmeCorporation.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace AcmeCorporation.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private PersonRepository _personRepository;

        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
        }

        public IPersonRepository PersonRepository => _personRepository ??= new PersonRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}