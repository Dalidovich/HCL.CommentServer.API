using HCL.CommentServer.API.Domain.Enums;

namespace HCL.CommentServer.API.Domain.DTO
{
    public class CommentDTO
    {
        public string Content { get; set; }
        private CommentMark? _mark;
        public CommentMark? Mark
        {
            get
            {
                return this._mark;
            }
            set
            {
                switch ((int)value) 
                {
                    case 1:
                        this._mark = CommentMark.Good;
                        break;
                    case 2:
                        this._mark = CommentMark.Bad;
                        break;
                    default:
                        this._mark = CommentMark.Normal;
                        break;
                }
            }
        }
    }
}