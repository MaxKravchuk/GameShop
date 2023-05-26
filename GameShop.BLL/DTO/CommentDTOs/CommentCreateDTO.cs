namespace GameShop.BLL.DTO.CommentDTOs
{
    public class CommentCreateDTO : CommentBaseDTO
    {
        public string GameKey { get; set; }

        public int? ParentId { get; set; } = null;

        public bool HasQuotation { get; set; } = false;
    }
}
