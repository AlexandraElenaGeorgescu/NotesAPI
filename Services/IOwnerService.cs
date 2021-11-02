using NotesAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Services
{
    public interface IOwnerService : ICollectionService<Owner>
    {
        Task<List<Owner>> GetOwnersById(Guid Id);
    }
}
