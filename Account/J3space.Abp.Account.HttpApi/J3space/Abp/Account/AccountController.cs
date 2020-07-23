using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace J3space.Abp.Account
{
    [RemoteService(Name = AccountRemoteServiceConstants.RemoteServiceName)]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : AbpController, IAccountAppService
    {
        protected IAccountAppService AccountAppService { get; }

        public AccountController(IAccountAppService accountAppService)
        {
            AccountAppService = accountAppService;
        }

        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            return AccountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("login")]
        public Task<AbpLoginResult> Login(UserLoginInfo login)
        {
            return AccountAppService.Login(login);
        }

        [HttpGet]
        [Route("logout")]
        public Task Logout()
        {
            return AccountAppService.Logout();
        }

        [HttpPost]
        [Route("checkPassword")]
        public Task<AbpLoginResult> CheckPassword(UserLoginInfo login)
        {
            return AccountAppService.CheckPassword(login);
        }
    }
}
