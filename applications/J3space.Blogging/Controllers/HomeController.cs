using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace J3space.Blogging.Controllers
{
    public class HomeController : AbpController
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}