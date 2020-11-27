using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace J3space.Blogging.EntityFrameworkCore
{
    [ConnectionStringName(BloggingDbProperties.ConnectionStringName)]
    public interface IBloggingDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
    }
}