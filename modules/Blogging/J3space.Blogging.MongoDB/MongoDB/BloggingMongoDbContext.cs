using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.MongoDB
{
    [ConnectionStringName(BloggingDbProperties.ConnectionStringName)]
    public class BloggingMongoDbContext : AbpMongoDbContext, IBloggingMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureBlogging();
        }
    }
}