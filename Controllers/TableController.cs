using Microsoft.AspNetCore.Mvc;

namespace PremierLeague_Backend.Controllers
{
    [Route("en/tables")]
    public class TableController : Controller
    {
        // GET: TableController
        public ActionResult Index()
        {
            return View();
        }

    }
}
