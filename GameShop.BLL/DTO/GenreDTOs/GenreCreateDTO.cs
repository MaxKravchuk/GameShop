namespace GameShop.BLL.DTO.GenreDTOs
{
    public class GenreCreateDTO : GenreBaseDTO
    {
        public int? ParentGenreId { get; set; }
    }
}
