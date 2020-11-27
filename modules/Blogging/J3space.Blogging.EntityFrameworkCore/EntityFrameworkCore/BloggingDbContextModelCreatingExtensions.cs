using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace J3space.Blogging.EntityFrameworkCore
{
    public static class BloggingDbContextModelCreatingExtensions
    {
        public static void ConfigureBlogging(
            this ModelBuilder builder,
            Action<BloggingModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BloggingModelBuilderConfigurationOptions(
                BloggingDbProperties.DbTablePrefix,
                BloggingDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            /* Configure all entities here. Example:

            builder.Entity<Question>(b =>
            {
                //Configure table & schema name
                b.ToTable(options.TablePrefix + "Questions", options.Schema);
            
                b.ConfigureByConvention();
            
                //Properties
                b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);
                
                //Relations
                b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

                //Indexes
                b.HasIndex(q => q.CreationTime);
            });
            */
        }
    }
}