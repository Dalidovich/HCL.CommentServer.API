using HCL.CommentServer.API.Domain.Enums;

namespace HCL.CommentServer.API.Domain.InnerResponse
{
    public abstract class BaseResponse<T>
    {
        public virtual T Data { get; set; }
        public virtual StatusCode StatusCode { get; set; }
        public virtual string Message { get; set; }
    }
}