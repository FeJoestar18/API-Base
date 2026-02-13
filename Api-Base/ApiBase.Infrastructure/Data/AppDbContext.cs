using ApiBase.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiBase.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}