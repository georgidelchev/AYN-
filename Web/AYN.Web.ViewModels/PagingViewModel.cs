using System;

namespace AYN.Web.ViewModels
{
    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage
            => this.PageNumber > 1;

        public bool HasNextPage
            => this.PageNumber < this.PagesCount;

        public int PreviousPageNumber
            => this.PageNumber - 1;

        public int NextPageNumber
            => this.PageNumber + 1;

        public int Count { get; set; }

        public int ItemsPerPage { get; set; }

        public int PagesCount
            => (int)Math.Ceiling((double)this.Count / this.ItemsPerPage);

        public string Search { get; set; }

        public string OrderBy { get; set; }

        public string Town { get; set; }

        public int? CategoryId { get; set; }

        public int PagedId { get; set; }
    }
}
