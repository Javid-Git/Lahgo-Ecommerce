using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LAHGO.Service.ViewModels
{
    public class PageNatedList<T> : List<T>
    {
        public int Page { get; }
        public int PageCount { get; }
        public int ItemCount { get; }
        public bool HasPrev { get; }
        public bool HasNext { get; }
        public PageNatedList(IQueryable<T> query, int page, int pagecount, int itemcount)
        {
            Page = page;
            PageCount = pagecount;
            HasPrev = page > 1;
            HasNext = page < pagecount;
            ItemCount = itemcount;
            this.AddRange(query);
        }

        public static PageNatedList<T> Create(int page, IQueryable<T> query, int itemcount)
        {
            int pagecount = (int)Math.Ceiling((decimal)query.Count() / itemcount);
            page = page < 1 || page > pagecount ? 1 : page;
            query = query.Skip((page - 1) * itemcount).Take(itemcount);

            return new PageNatedList<T>(query, page, pagecount, itemcount);
        }
    }
}
