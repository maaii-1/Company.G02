using Company.G02.PL.Models;
using Company.G02.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IScopedService scopedService01;
        private readonly IScopedService scopedService02;
        private readonly ITransientSevrvice transientSevrvice01;
        private readonly ITransientSevrvice transientSevrvice02;
        private readonly ISingleton singleton01;
        private readonly ISingleton singleton02;

        public HomeController(ILogger<HomeController> logger, 
                              IScopedService scopedService01, 
                              IScopedService scopedService02,
                              ITransientSevrvice transientSevrvice01, 
                              ITransientSevrvice transientSevrvice02,
                              ISingleton singleton01,
                              ISingleton singleton02 
                              )
        {
            _logger = logger;
            this.scopedService01 = scopedService01;
            this.scopedService02 = scopedService02;
            this.transientSevrvice01 = transientSevrvice01;
            this.transientSevrvice02 = transientSevrvice02;
            this.singleton01 = singleton01;
            this.singleton02 = singleton02;
        }


        public string TestLifeTime()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"scopedService01 :: {scopedService01.GetGuid()}\n");
            builder.Append($"scopedService02 :: {scopedService02.GetGuid()}\n\n");
            builder.Append($"transientSevrvice01 :: {transientSevrvice01.GetGuid()}\n");
            builder.Append($"transientSevrvice02 :: {transientSevrvice01.GetGuid()}\n\n");
            builder.Append($"singleton01 :: {singleton01.GetGuid()}\n");
            builder.Append($"singleton02 :: {singleton02.GetGuid()}\n\n");

            return builder.ToString();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
