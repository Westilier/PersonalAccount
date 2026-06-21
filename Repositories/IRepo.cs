using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public interface IRepo<TModel>
    where TModel : Model, new()
{
    // CREATE
    Task AddAsync(TModel model);

    // READ
    Task<TModel?> GetByIdAsync(int id);
    Task<List<TModel>> GetAllAsync();
    Task<bool> AnyAsync();
    Task<bool> ContainsByIdAsync(int id);

    //DELETE
    Task DeleteByIdAsync(int id);
}