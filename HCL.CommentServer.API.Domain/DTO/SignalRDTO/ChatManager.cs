using System.Xml.Linq;

namespace HCL.CommentServer.API.Domain.DTO.SignalRDTO
{
    public class ChatManager
    {
        public List<ChatAccount> Accounts { get; } = new();

        public void ConnectAccount(string accountLogin, string connectionId)
        {
            var accountAlreadyExists = GetConnectedAccountByName(accountLogin);
            if (accountAlreadyExists != null)
            {
                accountAlreadyExists.AppendConnection(connectionId);

                return;
            }

            var account = new ChatAccount(accountLogin);
            account.AppendConnection(connectionId);
            Accounts.Add(account);
        }

        public bool DisconnectAccount(string connectionId)
        {
            var accountExists = GetConnectedAccountById(connectionId);
            if (accountExists == null || !accountExists.Connections.Any())
            {

                return false;
            }

            var connectionExists = accountExists.Connections
                .Select(x => x.ConnectionId)
                .First()
                .Equals(connectionId);
            if (!connectionExists)
            {

                return false;
            }

            if (accountExists.Connections.Count() == 1)
            {
                Accounts.Remove(accountExists);

                return true;
            }

            accountExists.RemoveConnection(connectionId);

            return false;
        }

        private ChatAccount? GetConnectedAccountById(string connectionId)
        {
            var Account = Accounts
                .FirstOrDefault(x => x.Connections
                .Select(c => c.ConnectionId)
                .Contains(connectionId));

            return Account;
        }

        private ChatAccount? GetConnectedAccountByName(string userName)
        {
            var Account = Accounts.FirstOrDefault(x => string.Equals(
                    x.Login,
                    userName,
                    StringComparison.CurrentCultureIgnoreCase));

            return Account;
        }
    }
}
