namespace Domain.Entities
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string ChatRoom { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Sentiment { get; set; } // Опционально
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
