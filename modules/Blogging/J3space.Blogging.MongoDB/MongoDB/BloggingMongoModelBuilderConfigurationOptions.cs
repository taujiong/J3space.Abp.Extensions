using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace J3space.Blogging.MongoDB
{
    public class BloggingMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public BloggingMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}