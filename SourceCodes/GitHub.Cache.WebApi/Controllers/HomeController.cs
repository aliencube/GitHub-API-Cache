using Aliencube.GitHub.Cache.WebApi.Models.Home;
using System.Web.Mvc;

namespace Aliencube.GitHub.Cache.WebApi.Controllers
{
    /// <summary>
    /// This represents the /home controller.
    /// </summary>
    public class HomeController : BaseMvcController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var vm = new HomeIndexViewModel();
            return View(vm);
        }
    }
}