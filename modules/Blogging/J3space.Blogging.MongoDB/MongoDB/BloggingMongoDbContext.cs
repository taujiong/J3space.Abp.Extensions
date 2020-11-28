using J3space.Blogging.Posts;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.MongoDB
{
    [ConnectionStringName(BloggingDbProperties.ConnectionStringName)]
    public class BloggingMongoDbContext : AbpMongoDbContext, IBloggingMongoDbContext
    {
        public IMongoCollection<Post> Posts => Collection<Post>();
        public IMongoCollection<Tags.Tag> Tags => Collection<Tags.Tag>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureBlogging();
        }
    }
}