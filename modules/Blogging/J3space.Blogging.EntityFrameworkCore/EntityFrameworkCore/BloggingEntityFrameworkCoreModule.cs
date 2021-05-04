using J3space.Blogging.Posts;
using J3space.Blogging.Tags;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace J3space.Blogging.EntityFrameworkCore
{
    [DependsOn(
        typeof(BloggingDomainModule),
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class BloggingEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<BloggingDbContext>(options =>
            {
                options.AddRepository<Post, EfCorePostRepository>();
                options.AddRepository<Tag, EfCoreTagRepository>();
            });
        }
    }
}