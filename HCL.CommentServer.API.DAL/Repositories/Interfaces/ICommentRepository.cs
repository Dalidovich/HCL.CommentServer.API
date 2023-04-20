using HCL.CommentServer.API.Domain.Entities;

namespace HCL.CommentServer.API.DAL.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        public Task<Comment> AddAsync(Comment entity);
        public Comment Update(Comment entity);
        public bool Delete(Comment entity);
        public IQueryable<Comment> GetAsync();
        public Task<bool> SaveAsync();
    }
}