using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private readonly GloboTicketDbContext _dbContext;
    
    public OrderRepository(GloboTicketDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetPagedOrdersForMonth(DateTime date, int page, int size)
    {
        return await _dbContext.Orders.Where(o => o.OrderPlaced.Month == date.Month && o.OrderPlaced.Year == date.Year)
            .Skip((page - 1) * size)
            .Take(size)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<int> GetTotalCountOfOrdersForMonth(DateTime date)
    {
        return await _dbContext.Orders.CountAsync(o =>
            o.OrderPlaced.Month == date.Month && o.OrderPlaced.Year == date.Year);
    }
}