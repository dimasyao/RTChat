using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatDbContext _context;

        public ChatRepository(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<UserConnection?> GetUserConnectionByConnectionIdAsync(string connectionId)
        {
            return await _context.UserConnections.FirstOrDefaultAsync(c => c.ConnectionId == connectionId);
        }

        public async Task AddUserConnectionAsync(UserConnection userConnection)
        {
            await _context.UserConnections.AddAsync(userConnection);
        }

        public void RemoveUserConnectionAsync(UserConnection userConnection)
        {
            _context.UserConnections.Remove(userConnection);
        }

        public async Task AddChatMessageAsync(ChatMessage chatMessage)
        {
            await _context.ChatMessages.AddAsync(chatMessage);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
