namespace HCL.CommentServer.API.Domain.DTO.SignalRDTO
{
    public class ChatAccount
    {
        private readonly List<ChatConnection> _connections;
        public string Login { get; }

        public ChatAccount(string login)
        {
            Login = login;
            _connections = new List<ChatConnection>();
        }

        public DateTime? ConnectedAt
        {
            get
            {
                if (Connections.Any())
                {

                    return Connections
                        .OrderByDescending(x => x.ConnectedAt)
                        .Select(x => x.ConnectedAt)
                        .First();
                }

                return null;
            }
        }

        public IEnumerable<ChatConnection> Connections => _connections;

        public void AppendConnection(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var connection = new ChatConnection
            {
                ConnectedAt = DateTime.UtcNow,
                ConnectionId = connectionId
            };

            _connections.Add(connection);
        }

        public void RemoveConnection(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var connection = _connections.SingleOrDefault(x => x.ConnectionId.Equals(connectionId));
            if (connection == null)
            {

                return;
            }

            _connections.Remove(connection);
        }
    }
}
