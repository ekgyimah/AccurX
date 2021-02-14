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
            String command = "";
            int pagination = 0;
            int historyCommandPointer = 0;
            while (!readline.Equals("stop"))
            {
                if (readline.StartsWith("add")) 
                {
                    var splitString = readline.Split(" ");
                    
                    var todoListItem = new TodoListItem();
                    
                    todoListItem.text = splitString[1];
                    todoItems.Add(todoListItem);
                } else if (readline.Equals("list")) 
                {
                    var stringbuilder = new StringBuilder();
                    foreach (var todoListItem in todoItems)
                    {
                        stringbuilder.AppendLine(todoListItem.text + ", " + todoListItem.status + ", " + String.Join(',', todoListItem.subList));
                    }
                    Console.WriteLine(stringbuilder);
                } else if (readline.StartsWith("status")) //set status
                {
                    var splitString = readline.Split(",");
                    
                    var foundItem = todoItems.FirstOrDefault(x => x.text.Equals(splitString[0].Split(" ")[1]));
                
                    if (foundItem != null)
                    {
                        foundItem.status = splitString[1];
                    }
                } else if(readline.StartsWith("sublist"))
                {
                    var splitString = readline.Split(" ");
                    var splitStringSublist = splitString[1].Split(":");
                    
                    var foundItem = todoItems.FirstOrDefault(x => x.text.Equals(splitStringSublist[0]));
                    
                    if (foundItem != null)
                    {
                        foundItem.subList = splitStringSublist[1].Split(",");
                    }

                } else if (readline.StartsWith("search"))
                {
                    var splitString = readline.Split(" ");
                    var stringbuilder = new StringBuilder();
                    
                    foreach (var todoListItem in todoItems.Where(x=> x.text.Contains(splitString[1])))
                    {
                        stringbuilder.AppendLine(todoListItem.text + ", " + todoListItem.status);
                    }
                    Console.WriteLine(stringbuilder); //results
                } else if (readline.Equals("next")) //pagination
                {
                    var stringbuilder = new StringBuilder();

                    foreach (var todoListItem in todoItems.GetRange(pagination, 2))
                    {
                        stringbuilder.AppendLine(todoListItem.text + ", " + todoListItem.status);
                    }
                    Console.WriteLine(stringbuilder);
                    pagination = pagination + 2;
                } else if (readline.Equals("undo"))
                {
                    historyCommandPointer = historyCommandPointer - 1;
                    // historyCommandPointer;
                    string undoCommand = commandHistory[historyCommandPointer];
                    Console.WriteLine(undoCommand);
                    Console.ReadLine();
                } else if (readline.Equals("redo"))
                {
                    if (historyCommandPointer < commandHistory.Count)
                    {
                        historyCommandPointer = historyCommandPointer + 1;
                        string redoCommand = commandHistory[historyCommandPointer];
                        Console.WriteLine(redoCommand);
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.ReadLine();
                    }
                }

                if (!readline.Contains("undo")&&!readline.Contains("redo"))
                {
                    commandHistory.Add(readline);
                    historyCommandPointer = historyCommandPointer + 1;
                    readline = Console.ReadLine();
                }

                //write command
                //save command in array
                //undo -> (go down array)
                //redo -> (go up array)
            }
            // adding complexity to thetodo
            //remove code. if else to switch
            //make them in methods
        }

        private static List<String> commandHistory = new List<string>();
        private static List<TodoListItem> todoItems = new List<TodoListItem>();
    }

    class TodoListItem
    {
        public string text { get; set; } // Autoproperty
        public string status { get; set;  }
        public string[] subList { get; set; }
        
    }
}