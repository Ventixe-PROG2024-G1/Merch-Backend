using Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Data.Context;

public class MerchDbContext(DbContextOptions<MerchDbContext> options) : DbContext(options)
{
    public DbSet<MerchEntity> MerchEntitySet { get; set; }
}
