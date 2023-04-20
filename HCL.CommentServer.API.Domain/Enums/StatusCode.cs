namespace HCL.CommentServer.API.Domain.Enums
{
    public enum StatusCode
    {
        EntityNotFound = 0,

        CommentCreate = 1,
        CommentUpdate = 2,
        CommentDelete = 3,
        CommentRead = 4,

        OK = 200,
        OKNoContent = 204,
        InternalServerError = 500,
    }
}