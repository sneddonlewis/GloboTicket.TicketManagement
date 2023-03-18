using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GloboTicket.TicketManagement.Persistence.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    private readonly GloboTicketDbContext _dbContext;
    
    public CategoryRepository(GloboTicketDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Category>> GetCategoriesWithEvents(bool includePastEvents)
    {
        var allCategories = await _dbContext.Categories.Include(c => c.Events)
            .ToListAsync();

        if (!includePastEvents)
        {
            allCategories.ForEach(c => c.Events.ToList().RemoveAll(e => e.Date < DateTime.Today));
        }

        return allCategories;
    }
}