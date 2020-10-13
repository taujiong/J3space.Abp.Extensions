using J3space.Sample.Users;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace J3space.Sample.MongoDb
{
    [ConnectionStringName("Default")]
    public class SampleMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<AppUser> Users => Collection<AppUser>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<AppUser>(b => { b.CollectionName = "AbpUsers"; });
        }
    }
}