using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var readline = Console.ReadLine();
            while (!readline.Equals("stop"))
            {
                if (readline.Equals("add"))
                {
                    readline = Console.ReadLine();
                    
                    var todoListItem = new TodoListItem();
                    
                    todoListItem.text = readline;
                    todoItems.Add(todoListItem);
                } else if (readline.Equals("list")) {
                    var stringbuilder = new StringBuilder();
                    foreach (var todoListItem in todoItems)
                    {
                        stringbuilder.AppendLine(todoListItem.text + ", " + todoListItem.status);
                    }
                    Console.WriteLine(stringbuilder);
                } else if (readline.Equals("status")){
                    readline = Console.ReadLine();
                    var splitString = readline.Split(",");

                    var foundItem = todoItems.FirstOrDefault(x => x.text.Equals(splitString[0]));

                    if (foundItem != null)
                    {
                        foundItem.status = splitString[1];
                    }
                } else if (readline.Equals("search"))
                {
                    readline = Console.ReadLine();
                    var stringbuilder = new StringBuilder();
                    foreach (var todoListItem in todoItems.Where(x=> x.text.Contains(readline)))
                    {
                        stringbuilder.AppendLine(todoListItem.text + ", " + todoListItem.status);
                    }
                    Console.WriteLine(stringbuilder); //results
                }

                readline = Console.ReadLine();
            }

     
        }

        private static List<TodoListItem> todoItems = new List<TodoListItem>();
    }

    class TodoListItem
    {
        public string text { get; set; } // Autoproperty
        public string status { get; set;  }
        
    }
}