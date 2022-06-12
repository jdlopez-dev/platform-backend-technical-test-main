using AcmeCorporation.Core.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace AcmeCorporation.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPersonRepository PersonRepository { get; }

        Task<int> CommitAsync();
    }
}