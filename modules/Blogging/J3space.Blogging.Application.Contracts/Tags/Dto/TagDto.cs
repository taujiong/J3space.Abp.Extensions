﻿using System;
using Volo.Abp.Application.Dtos;

namespace J3space.Blogging.Tags.Dto
{
    public class TagDto : EntityDto<Guid>
    {
        public string Name { get; set; }
    }
}