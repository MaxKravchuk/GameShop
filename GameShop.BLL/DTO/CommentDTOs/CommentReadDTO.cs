namespace GameShop.BLL.DTO.CommentDTOs
{
    public class CommentReadDTO : CommentBaseDTO
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public bool HasQuotation { get; set; }
    }
}
