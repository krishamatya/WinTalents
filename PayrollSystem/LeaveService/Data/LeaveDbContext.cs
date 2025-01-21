using LeaveService.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LeaveService.Data
{
    public class LeaveDbContext : DbContext
    {
        public LeaveDbContext(DbContextOptions<LeaveDbContext> options) : base(options) { }
        public DbSet<Leave> Leaves { get; set; }
    }
}
