using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace J3space.Blogging.Posts
{
    public class PostTag : CreationAuditedEntity
    {
        protected PostTag()
        {
        }

        public PostTag(Guid postId, Guid tagId)
        {
            PostId = postId;
            TagId = tagId;
        }

        public virtual Guid PostId { get; protected set; }

        public virtual Guid TagId { get; protected set; }

        public override object[] GetKeys()
        {
            return new object[] {PostId, TagId};
        }
    }
}