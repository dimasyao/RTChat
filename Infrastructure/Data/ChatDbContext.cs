using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) 
        { 
        }

        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
