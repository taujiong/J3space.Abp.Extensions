using System;
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
        }
    }
}