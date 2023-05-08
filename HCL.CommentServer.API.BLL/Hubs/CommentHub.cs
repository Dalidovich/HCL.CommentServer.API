using HCL.CommentServer.API.BLL.Hubs.Interfaces;
using HCL.CommentServer.API.BLL.Interfaces;
using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.DTO.SignalRDTO;
using HCL.CommentServer.API.Domain.Entities;
using HCL.CommentServer.API.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HCL.CommentServer.API.BLL.Hubs
{
    [Authorize]
    public class CommentHub : Hub<ICommentHub>
    {
        private readonly ChatManager _chatManager;
        private readonly ICommentService _commentService;

        public CommentHub(ChatManager chatManager, ICommentService commentService)
        {
            _chatManager = chatManager;
            _commentService = commentService;
        }

        public override async Task OnConnectedAsync()
        {
            var AccountLogin = Context.User?.Identity?.Name;
            if (AccountLogin != null)
            {
                var connectionId = Context.ConnectionId;
                _chatManager.ConnectAccount(AccountLogin, connectionId);
                await base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var isUserRemoved = _chatManager.DisconnectAccount(Context.ConnectionId);
            if (!isUserRemoved)
            {
                await base.OnDisconnectedAsync(exception);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendCommentInGroupAsync(CommentDTO commentDTO, string groupId)
        {
            await Clients.OthersInGroup(groupId).SendCommentInGroupAsync(commentDTO, groupId);
            Guid accountId= new(Context.User.Identities.First().FindFirst(CustomClaimType.AccountId).Value);
            await _commentService.CreateComment(new Comment(commentDTO, accountId, groupId));
        }

        public async Task SetConnectionInGroup(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }

        public async Task RemoveConnectionInGroup(string groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupId);
        }
    }
}
