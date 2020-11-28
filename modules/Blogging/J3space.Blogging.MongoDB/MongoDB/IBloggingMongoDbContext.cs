using J3space.Blogging.Posts;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.MongoDB
{
    [ConnectionStringName(BloggingDbProperties.ConnectionStringName)]
    public interface IBloggingMongoDbContext : IAbpMongoDbContext
    {
        IMongoCollection<Post> Posts { get; }
        IMongoCollection<Tags.Tag> Tags { get; }
    }
}