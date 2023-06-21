namespace GameShop.BLL.Pagination.Models
{
    public abstract class PaginationParameters
    {
        private int _pageSize = 20;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }
    }
}
