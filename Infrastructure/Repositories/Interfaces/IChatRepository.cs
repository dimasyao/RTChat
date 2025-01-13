using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IChatRepository
    {
        Task<UserConnection?> GetUserConnectionByConnectionIdAsync(string connectionId);
        Task AddUserConnectionAsync(UserConnection userConnection);
        void RemoveUserConnectionAsync(UserConnection userConnection);
        Task AddChatMessageAsync(ChatMessage chatMessage);
        Task SaveChangesAsync();
    }
}
