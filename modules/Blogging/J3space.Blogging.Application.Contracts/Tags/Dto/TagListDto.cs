using System;
using Volo.Abp.Application.Dtos;

namespace J3space.Blogging.Tags.Dto
{
    public class TagListDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public int UsageCount { get; set; }
    }
}