using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace J3space.Abp.Account
{
    public interface IAccountAppService : IApplicationService
    {
        public Task<IdentityUserDto> RegisterAsync(RegisterDto input);
        public Task<AbpLoginResult> Login(LoginDto login);
        public Task Logout();
        public Task<AbpLoginResult> CheckPassword(LoginDto login);
    }
}