namespace RTChat.Hubs.Interfaces
{
    public interface IChatClient
    {
        public Task ReceiveMessage(string user, string message, string sentiment);
    }
}
