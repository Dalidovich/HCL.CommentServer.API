namespace HCL.CommentServer.API.Domain.DTO.SignalRDTO
{
    public class ChatConnection
    {
        public DateTime ConnectedAt { get; set; }
        public string ConnectionId { get; set; } = null!;
    }
}
