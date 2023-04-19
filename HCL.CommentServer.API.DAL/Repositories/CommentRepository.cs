using HCL.CommentServer.API.DAL.Repositories.Interfaces;
using HCL.CommentServer.API.Domain.Entities;
using System.Security.Cryptography.X509Certificates;

namespace HCL.CommentServer.API.DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentAppDBContext _db;

        public CommentRepository(CommentAppDBContext db)
        {
            _db = db;
        }

        public async Task<Comment> AddAsync(Comment entity)
        {
            var createdEntity = await _db.Comments.AddAsync(entity);

            return createdEntity.Entity;
        }

        public bool Delete(Comment entity)
        {
            _db.Comments.Remove(entity);

            return true;
        }

        public IQueryable<Comment> GetAsync()
        {

            return _db.Comments.AsQueryable();
        }

        public async Task<bool> SaveAsync()
        {
            await _db.SaveChangesAsync();

            return true;
        }

        public Comment Update(Comment entity)
        {
            var updatedEntity = _db.Comments.Update(entity);

            return updatedEntity.Entity;
        }
    }
}