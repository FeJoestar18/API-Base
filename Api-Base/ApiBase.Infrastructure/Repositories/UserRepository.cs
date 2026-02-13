using ApiBase.Domain.Entities;
using ApiBase.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiBase.Infrastructure.Repositories;

public class UserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmail(string email)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task Add(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}