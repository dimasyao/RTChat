using Domain.Entities;
using Infrastructure.Services.Interfaces;
using Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using RTChat.Hubs.Interfaces;
using UserConnection = Domain.Entities.UserConnection;

namespace RTChat.Hubs
{
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IChatRepository _chatRepository;
        private readonly ITextAnalysisService _textAnalysisService;

        public ChatHub(IChatRepository chatRepository, ITextAnalysisService textAnalysisService)
        {
            _chatRepository = chatRepository;
            _textAnalysisService = textAnalysisService;
        }

        public async Task JoinChat(Models.UserConnection userConnection)
        {
            if (userConnection == null || string.IsNullOrWhiteSpace(userConnection.UserName) || string.IsNullOrWhiteSpace(userConnection.ChatRoom))
            {
                throw new ArgumentException("Invalid user connection data.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, userConnection.ChatRoom);

            var existingConnection = await _chatRepository.GetUserConnectionByConnectionIdAsync(Context.ConnectionId);

            if (existingConnection == null)
            {
                var newConnection = new UserConnection
                {
                    ConnectionId = Context.ConnectionId,
                    ChatRoom = userConnection.ChatRoom,
                    UserName = userConnection.UserName
                };

                await _chatRepository.AddUserConnectionAsync(newConnection);
                await _chatRepository.SaveChangesAsync();
            }

            await Clients.Group(userConnection.ChatRoom).ReceiveMessage("Admin", $"{userConnection.UserName} has joined the chat room.", "System");
        }

        public async Task SendMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("Message cannot be empty.");
            }

            var connection = await _chatRepository.GetUserConnectionByConnectionIdAsync(Context.ConnectionId);

            if (connection != null)
            {
                var sentiment = await _textAnalysisService.AnalyzeSentimentAsync(message);

                var chatMessage = new ChatMessage
                {
                    UserName = connection.UserName,
                    ChatRoom = connection.ChatRoom,
                    Message = message,
                    Sentiment = sentiment,
                    Timestamp = DateTime.UtcNow
                };

                await _chatRepository.AddChatMessageAsync(chatMessage);
                await _chatRepository.SaveChangesAsync();

                await Clients.Group(connection.ChatRoom).ReceiveMessage(connection.UserName, message, sentiment);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connection = await _chatRepository.GetUserConnectionByConnectionIdAsync(Context.ConnectionId);

            if (connection != null)
            {
                _chatRepository.RemoveUserConnectionAsync(connection);
                await _chatRepository.SaveChangesAsync();

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

                await Clients.Group(connection.ChatRoom).ReceiveMessage("Admin", $"{connection.UserName} leave the chat room.", "System");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
