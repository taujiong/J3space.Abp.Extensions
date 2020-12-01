using System;
using J3space.Blogging.Posts;
using J3space.Blogging.Tags;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

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

            builder.Entity<Post>(b =>
            {
                b.ToTable(options.TablePrefix + "Posts", options.Schema);

                b.ConfigureByConvention();

                b.Property(p => p.Title)
                    .IsRequired()
                    .HasMaxLength(PostConstant.MaxTitleLength)
                    .HasColumnName(nameof(Post.Title));
                b.Property(p => p.Description)
                    .HasMaxLength(PostConstant.MaxDescriptionLength)
                    .HasColumnName(nameof(Post.Description));
                b.Property(p => p.Content)
                    .IsRequired()
                    .HasMaxLength(PostConstant.MaxContentLength)
                    .HasColumnName(nameof(Post.Content));

                b.HasMany(p => p.Tags).WithOne().HasForeignKey(pt => pt.PostId);
            });

            builder.Entity<Tag>(b =>
            {
                b.ToTable(options.TablePrefix + "Tags", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(TagConstant.MaxNameLength)
                    .HasColumnName(nameof(Tag.Name));
                b.Property(x => x.UsageCount).HasColumnName(nameof(Tag.UsageCount));

                b.HasMany<PostTag>().WithOne().HasForeignKey(pt => pt.TagId);
            });

            builder.Entity<PostTag>(b =>
            {
                b.ToTable(options.TablePrefix + "PostTags", options.Schema);

                b.ConfigureByConvention();

                b.Property(x => x.PostId).HasColumnName(nameof(PostTag.PostId));
                b.Property(x => x.TagId).HasColumnName(nameof(PostTag.TagId));

                b.HasKey(x => new {x.PostId, x.TagId});
            });
        }
    }
}