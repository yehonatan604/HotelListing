using HotelListing.Api.ViewModels.Page;

namespace HotelListing.Api.Contracts
{
    public interface IGenericRepository<T> where T: class
    {
        // GET
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int? id);
        Task<PageResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);

        // POST
        Task<T> AddAsync(T entity);

        //PUT
        Task UpdateAsync(T entity);

        // DELETE
        Task DeleteAsync(int? id);

        //
        // Assistant Methods
        Task<bool> Exists(int id);
    }
}
