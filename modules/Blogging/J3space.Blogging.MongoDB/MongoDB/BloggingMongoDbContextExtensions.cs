using System;
using J3space.Blogging.Posts;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.MongoDB
{
    public static class BloggingMongoDbContextExtensions
    {
        public static void ConfigureBlogging(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BloggingMongoModelBuilderConfigurationOptions(
                BloggingDbProperties.DbTablePrefix
            );

            optionsAction?.Invoke(options);

            builder.Entity<Post>(b => { b.CollectionName = options.CollectionPrefix + "Posts"; });

            builder.Entity<Tags.Tag>(b => { b.CollectionName = options.CollectionPrefix + "Tags"; });
        }
    }
}