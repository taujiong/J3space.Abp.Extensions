using System;
using System.Collections.Generic;
using J3space.Blogging.Tags.Dto;
using Volo.Abp.Application.Dtos;

namespace J3space.Blogging.Posts.Dto
{
    public class PostWithDetailDto : AuditedEntityDto<Guid>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public List<TagDto> Tags { get; set; }
    }
}