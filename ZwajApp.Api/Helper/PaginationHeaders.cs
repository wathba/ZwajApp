namespace ZwajApp.Api.Helper
{
    public class PaginationHeaders
    {
        public int CurrentPage{ get; set; }
        public int ItemPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages{ get; set; }
        public PaginationHeaders(int currentPage,int itemPerPage, int totalItems,int totalPages)
        {
   this.CurrentPage = currentPage;
   this.ItemPerPage = itemPerPage;
   this.TotalItems = totalItems;
   this.TotalPages = totalPages;
  }
    }
}