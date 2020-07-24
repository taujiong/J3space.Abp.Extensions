using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Identity;

namespace J3space.Abp.Account
{
    [RemoteService(Name = AbpAccountRemoteServiceConstants.RemoteServiceName)]
    [Area("account")]
    [Route("api/account")]
    public class AccountController : AccountControllerBase, IAccountAppService
    {
        private readonly IAccountAppService _accountAppService;

        public AccountController(IAccountAppService accountAppService)
        {
            _accountAppService = accountAppService;
        }

        [HttpPost]
        [Route("register")]
        public virtual Task<IdentityUserDto> RegisterAsync(RegisterDto input)
        {
            return _accountAppService.RegisterAsync(input);
        }

        [HttpPost]
        [Route("login")]
        public Task<AbpLoginResult> Login(LoginDto login)
        {
            return _accountAppService.Login(login);
        }

        [HttpGet]
        [Route("logout")]
        public Task Logout()
        {
            return _accountAppService.Logout();
        }

        [HttpPost]
        [Route("checkPassword")]
        public Task<AbpLoginResult> CheckPassword(LoginDto login)
        {
            return _accountAppService.CheckPassword(login);
        }
    }
}