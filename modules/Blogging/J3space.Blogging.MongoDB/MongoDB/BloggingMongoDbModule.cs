using J3space.Blogging.Posts;
using J3space.Blogging.Tags;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.MongoDB
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(AbpMongoDbModule)
    )]
    public class BloggingMongoDbModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddMongoDbContext<BloggingMongoDbContext>(options =>
            {
                options.AddRepository<Post, MongoPostRepository>();
                options.AddRepository<Tag, MongoTagRepository>();
            });
        }
    }
}