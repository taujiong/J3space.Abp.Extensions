﻿using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace J3space.Blogging.Tags
{
    public class Tag : BasicAggregateRoot<Guid>
    {
        public Tag(Guid id, [NotNull] string name, int usageCount = 0)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            UsageCount = usageCount;
        }

        public virtual string Name { get; protected set; }

        public virtual int UsageCount { get; protected internal set; }

        public virtual void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public virtual void IncreaseUsageCount(int number = 1)
        {
            UsageCount += number;
        }

        public virtual void DecreaseUsageCount(int number = 1)
        {
            if (UsageCount <= 0)
            {
                return;
            }

            if (UsageCount - number <= 0)
            {
                UsageCount = 0;
                return;
            }

            UsageCount -= number;
        }
    }
}