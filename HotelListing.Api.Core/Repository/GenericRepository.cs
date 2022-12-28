using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using HotelListing.Api.ViewModels.Page;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly HotelDbContext _db;
        private readonly IMapper _mapper;
        public GenericRepository(IDbContextFactory<HotelDbContext> dbFactory, IMapper mapper)
        {
            _db = dbFactory.CreateDbContext();
            _mapper = mapper;
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

        public async Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters)
        {
            var totalSize = await _db.Set<T>().CountAsync();
            var items = await _db.Set<T>()
                .Skip(queryParameters.StartIndex)
                .Take(queryParameters.PageSize)
                .ProjectTo<TResult>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PageResult<TResult>
            {
                Items = items,
                PageNumber = queryParameters.StartIndex,
                RecordNumber= queryParameters.PageSize,
                TotalCount= totalSize
            };
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
