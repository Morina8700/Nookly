using BookMyHome.Domain.Models;
using BookMyHome.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace BookMyHome.Persistence.Repositories;

public class UserRepository
{
    private readonly BookMyHomeDbContext _context;

    public UserRepository(BookMyHomeDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Set<User>()
            .SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Set<User>()
            .AnyAsync(u => u.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _context.Set<User>().Add(user);
        await _context.SaveChangesAsync();
    }
}