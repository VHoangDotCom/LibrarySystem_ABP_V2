using System.Collections.Generic;

namespace LibrarySystem.CoreDependencies.Paging
{
    public class PagingResult<T>
    {
        public long TotalItems { get; set; }
        public List<T> Items { get; set; }
    }
}
