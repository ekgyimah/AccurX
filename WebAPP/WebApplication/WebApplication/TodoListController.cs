using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace WebApplication
{
        [Route("api/[controller]")]
        [ApiController]
    public class TodoListController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok("Done");
        }
        
        [HttpGet("list")]
        public IActionResult List()
        {
            List<TodoListAdd> f = new List<TodoListAdd>();
            string[] filePath = System.IO.File.ReadAllLines(@"/Users/ekg/RiderProjects/WebApplication/WebApplication/TodoListItem.txt");
            foreach (string line in filePath)
            {
                TodoListAdd item = new TodoListAdd();
                item.Name = line;
                f.Add(item);
            }
            // List all items
            return Ok(f);
        }
        
        [HttpGet("search")]
        public IActionResult Search(string word)
        {
            List<TodoListAdd> items = new List<TodoListAdd>();
            foreach (var todoListItem in ToDo.listItems().Where(x=> x.Name.Contains(word)))
            {
                items.Add(todoListItem);
            }
            return Ok(items);
        }
        
        [HttpPost("add")]
        public IActionResult Add(TodoListAdd todoListAdd)
        {
            using StreamWriter file = new StreamWriter("TodoListItem.txt", append: true);
            file.WriteLine(todoListAdd.Name);
            return Ok("Item added");
        }
        
        
    }


    public class TodoListAdd
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }

    public class ToDo
    {
        public static List<TodoListAdd> listItems(){
            List<TodoListAdd> f = new List<TodoListAdd>();
            string[] filePath = System.IO.File.ReadAllLines(@"/Users/ekg/RiderProjects/WebApplication/WebApplication/TodoListItem.txt");
            foreach (string line in filePath.ToList())
            {
                TodoListAdd item = new TodoListAdd();
                item.Name = line;
                f.Add(item);
            }

            return f;
        }
    }

}