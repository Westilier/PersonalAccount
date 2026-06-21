using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.Data;
using PersonalAccount.Data.Entities;
using PersonalAccount.Mappers;
using PersonalAccount.Models;

namespace PersonalAccount.Repositories;

public abstract class Repo<TEntity, TModel>(
    AppDbContext ctx,
    IMapper<TEntity, TModel> mapper,
    Func<AppDbContext, DbSet<TEntity>> tableSelector
) : IRepo<TModel>
    where TEntity : Entity, new()
    where TModel : Model, new()
{
    protected AppDbContext Ctx { get; } = ctx;
    protected IMapper<TEntity, TModel> Mapper { get; } = mapper;

    protected DbSet<TEntity> Table => tableSelector(Ctx);

    public async Task AddAsync(TModel model)
    {
        await Table.AddAsync(Mapper.ToEntity(model));
        await Ctx.SaveChangesAsync();
    }

    public async Task<TModel?> GetByIdAsync(int id)
    {
        var entity = await Table.FindAsync(id);
        return entity == null ? null : Mapper.ToModel(entity);
    }

    public async Task<List<TModel>> GetAllAsync() =>
        await Table
            .AsNoTracking()
            .Select(entity => Mapper.ToModel(entity))
            .ToListAsync();

    public async Task<bool> AnyAsync() => await Table.AnyAsync();
    
    protected async Task<TModel?> GetByAsync(Expression<Func<TEntity, bool>> predicate)
    {
        var entity = await Table
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate);
        return entity == null ? null : Mapper.ToModel(entity);
    }


    protected async Task<List<TModel>> GetAllByAsync(Expression<Func<TEntity, bool>> predicate) =>
        await Table
            .AsNoTracking()
            .Where(predicate)
            .Select(entity => Mapper.ToModel(entity))
            .ToListAsync();


    public async Task<bool> ContainsByIdAsync(int id)
    {
        var entity = await Table.FindAsync(id);
        return entity != null;
    }

    protected async Task UpdateByIdAsync(int id, Action<TEntity> updateAction)
    {
        var entity = await Table.FindAsync(id);
        if (entity == null) throw new KeyNotFoundException();
        updateAction(entity);
        await Ctx.SaveChangesAsync();
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await Table.FindAsync(id) ?? throw new KeyNotFoundException();
        Table.Remove(entity);
        await Ctx.SaveChangesAsync();
    }
}