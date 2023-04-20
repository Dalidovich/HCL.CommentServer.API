using HCL.CommentServer.API.Domain.DTO;

namespace HCL.CommentServer.API.BLL.Hubs.Interfaces
{
    public interface ICommentHub
    {
        Task RemoveConnectionInGroup(string connectionId, string groupId);
        Task SendCommentInGroupAsync(CommentDTO message, string groupId);
        Task SetConnectionInGroup(string connectionId, string groupId);
    }
}
