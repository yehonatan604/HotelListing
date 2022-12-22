using AutoMapper.Internal;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelDbContext _db;
        public GenericRepository(IDbContextFactory<HotelDbContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        // GET
        public async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

        // GET {Id}
        public async Task<T> GetAsync(int? id)
        {
            return (id is not null ? await _db.Set<T>().FindAsync(id) : null)!;
        }

        // POST
        public async Task<T> AddAsync(T entity)
        {
            await _db.AddAsync(entity);
            await Save();
            return entity;
        }

        // PUT
        public async Task UpdateAsync(T entity)
        {
            _db.Update(entity);
            await Save();
        }

        // DELETE {Id}
        public async Task DeleteAsync(int? id)
        {
            var entity = await GetAsync(id);
            _db.Set<T>().Remove(entity);
            await Save();
        }

        // Assistant Methods
        public async Task<bool> Exists(int id)
        {
            return await GetAsync(id) != null;
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
