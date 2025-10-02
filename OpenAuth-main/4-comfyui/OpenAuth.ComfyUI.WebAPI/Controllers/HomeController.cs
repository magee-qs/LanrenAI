using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OpenAuth.ComfyUI.WebAPI.Controllers
{
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("/index")]
        public AjaxResult Index()
        {
            return AjaxResult.OK("start!");
        }
    }
}
