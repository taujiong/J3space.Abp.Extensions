using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace J3space.Blogging.Posts
{
    public class Post : FullAuditedAggregateRoot<Guid>
    {
        protected Post()
        {
        }

        public Post(Guid id, [NotNull] string title)
        {
            Id = id;
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));

            Tags = new Collection<PostTag>();
        }

        [NotNull] public virtual string Title { get; protected set; }

        [CanBeNull] public virtual string Content { get; set; }

        [CanBeNull] public virtual string Description { get; set; }

        public virtual Collection<PostTag> Tags { get; protected set; }

        public virtual Post SetTitle([NotNull] string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title));
            return this;
        }

        public virtual void AddTag(Guid tagId)
        {
            Tags.Add(new PostTag(Id, tagId));
        }

        public virtual void RemoveTag(Guid tagId)
        {
            Tags.RemoveAll(t => t.TagId == tagId);
        }
    }
}