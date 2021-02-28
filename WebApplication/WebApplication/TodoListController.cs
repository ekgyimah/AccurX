using Microsoft.AspNetCore.Mvc;

namespace WebApplication
{
    [Route("api/[controller]")]
    public class TodoListController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok("Hello world 2");
        }
    }
}