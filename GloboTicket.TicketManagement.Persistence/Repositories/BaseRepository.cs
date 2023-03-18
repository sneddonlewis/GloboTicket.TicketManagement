using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : class
{
    private readonly GloboTicketDbContext _dbContext;

    public BaseRepository(GloboTicketDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<T>()
            .FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _dbContext.Set<T>()
            .ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbContext.Set<T>()
            .AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity)
            .State = EntityState.Modified;
        return _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>()
            .Remove(entity);
        return _dbContext.SaveChangesAsync();
    }
}