using System;
using Volo.Abp.Domain.Repositories;

namespace J3space.Blogging.Posts
{
    public interface IPostRepository : IBasicRepository<Post, Guid>
    {
    }
}