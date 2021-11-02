using MongoDB.Driver;
using NotesAPI.Models;
using NotesAPI.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IMongoCollection<Owner> _owners;

        public OwnerService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _owners = database.GetCollection<Owner>(settings.OwnerName);
        }

        public async Task<List<Owner>> GetAll()
        {
            var result = await _owners.FindAsync(owner => true);
            return result.ToList();
        }

        public async Task<bool> Create(Owner owner)
        {
            await _owners.InsertOneAsync(owner);
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var result = await _owners.DeleteOneAsync(owner => owner.Id == id);
            if (!result.IsAcknowledged && result.DeletedCount == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<Owner> Get(Guid id)
        {
            return (await _owners.FindAsync(owner => owner.Id == id)).FirstOrDefault();
        }

        public async Task<bool> Update(Guid id, Owner owner)
        {
            owner.Id = id;
            var result = await _owners.ReplaceOneAsync(owner => owner.Id == id, owner);
            if (!result.IsAcknowledged && result.ModifiedCount == 0)
            {
                await _owners.InsertOneAsync(owner);
                return false;
            }

            return true;
        }

        public async Task<List<Owner>> GetOwnersById(Guid Id)
        {

            return (await _owners.FindAsync(owner => owner.Id == Id)).ToList();
        }
    }

}

