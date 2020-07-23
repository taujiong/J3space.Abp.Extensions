using J3space.AuthServer.Users;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace J3space.AuthServer.MongoDb
{
    [ConnectionStringName("Default")]
    public class AuthServerMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<AppUser> Users => Collection<AppUser>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<AppUser>(b => { b.CollectionName = "AbpUsers"; });
        }
    }
}
