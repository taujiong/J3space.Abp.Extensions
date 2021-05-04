using System.Linq;
using J3space.Blogging.Posts;
using Microsoft.EntityFrameworkCore;

namespace J3space.Blogging
{
    public static class BloggingEntityFrameworkCoreQueryableExtensions
    {
        public static IQueryable<Post> IncludeDetails(this IQueryable<Post> queryable, bool include = true)
        {
            return !include ? queryable : queryable.Include(p => p.Tags);
        }
    }
}