using System;
using Volo.Abp.Application.Dtos;

namespace J3space.Blogging.Tags.Dto
{
    public class TagDto : EntityDto<Guid>
    {
        // TODO: 区分 tag 页面和 Post 列表页面 TagDto 的不同
        public string Name { get; set; }
        public string Description { get; set; }
        public int UsageCount { get; set; }
    }
}