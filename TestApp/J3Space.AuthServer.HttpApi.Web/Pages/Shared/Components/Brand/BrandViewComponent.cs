using Microsoft.AspNetCore.Mvc;

namespace J3space.AuthServer.Pages.Shared.Components.Brand
{
    public class BrandViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // TODO: 把 AppName 提取到配置文件
            var brand = new BrandViewModel
            {
                AppName = "J3space.AuthServer",
                AppLogoUrl = "/image/logo.svg"
            };
            return View("~/Pages/Shared/Components/Brand/Default.cshtml", brand);
        }
    }
}