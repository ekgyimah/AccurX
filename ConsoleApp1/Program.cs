using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{

    public class CommandText
    {
        public CommandText(CommandMethod method, string instructions)
        {
            Method = method;
            Instructions = instructions;
        }

        public CommandMethod Method { get; }
        public string Instructions { get; }
    }

    public enum CommandMethod
    {
        unknown,
        stop,
        add,
        list,
        sublist,
        status,
        mistype
    }
    
    class Program
    {
        private static CommandText _GetFromReadline(string readline)
        {
            var splitOnSpace = readline.Split(" ");
            var success = Enum.TryParse<CommandMethod>(splitOnSpace[0], out var command);
            if (!success)
            {
                var successToLower = Enum.TryParse<CommandMethod>(splitOnSpace[0].ToLower(), out var correctType);
                if (successToLower)
                {
                    return new CommandText(CommandMethod.mistype, $"It looks like you've mistyped a command, did you mean to type {correctType}?");
                }
                
                return new CommandText(CommandMethod.unknown, "It looks like you've not typed a command we recognise");
            }
            var commandText = new CommandText(command, readline.Replace(splitOnSpace[0], ""));
            return commandText;
        }
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var initialReadline = Console.ReadLine();
            //var command = "";
            var pagination = 0;
            var historyCommandPointer = 0;

            var command = _GetFromReadline(initialReadline);
            while (command.Method != CommandMethod.stop)
            {
                switch (command.Method)
                {
                    case CommandMethod.add:
                    {
                        var todoListItem = TodoListItem.Create(command.Instructions);
                        _todoItems.Add(todoListItem);
                        break;
                    }
                    case CommandMethod.list:
                    {
                        var stringbuilder = new StringBuilder();
                        foreach (var todoListItem in _todoItems)
                        {
                            stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status + ", " + String.Join(',', todoListItem.SubList));
                        }
                        Console.WriteLine(stringbuilder);
                        break;
                    }
                    case CommandMethod.status:
                    {
                        var splitString = command.Instructions.Split(",");
                        var foundItem = _todoItems.FirstOrDefault(x => x.Text.Equals(splitString[0]));
                        if (foundItem != null)
                        {
                            foundItem.UpdateStatus(splitString[1]);
                        }
                        break;
                    }
                    case CommandMethod.sublist:
                    {
                        var splitStringSublist = command.Instructions.Split(":");
                        var foundItem = _todoItems.FirstOrDefault(x => x.Text.Equals(splitStringSublist[0]));
                        if (foundItem != null)
                        {
                            foundItem.AddToSublist(splitStringSublist[1].Split(","));
                        }

                        break;
                    }
                    case CommandMethod.mistype:
                    {
                        Console.WriteLine(command.Instructions);
                        break;
                    }
                    case CommandMethod.unknown:
                    {
                        Console.WriteLine(command.Instructions);
                        break;
                    }
                }

                var newInstructions = Console.ReadLine();
                command = _GetFromReadline(newInstructions);
            }
            
                // se if (readline.StartsWith("search"))
                // {
                //     var splitString = readline.Split(" ");
                //     var stringbuilder = new StringBuilder();
                //     
                //     foreach (var todoListItem in _todoItems.Where(x=> x.Text.Contains(splitString[1])))
                //     {
                //         stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status);
                //     }
                //     Console.WriteLine(stringbuilder); //results
                // } else if (readline.Equals("next")) //pagination
                // {
                //     var stringbuilder = new StringBuilder();
                //
                //     foreach (var todoListItem in _todoItems.GetRange(pagination, 2))
                //     {
                //         stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status);
                //     }
                //     Console.WriteLine(stringbuilder);
                //     pagination = pagination + 2;
                // } else if (readline.Equals("undo"))
                // {
                //     historyCommandPointer = historyCommandPointer - 1;
                //     // historyCommandPointer;
                //     string undoCommand = _commandHistory[historyCommandPointer];
                //     Console.WriteLine(undoCommand);
                //     Console.ReadLine();
                // } else if (readline.Equals("redo"))
                // {
                //     if (historyCommandPointer < _commandHistory.Count)
                //     {
                //         historyCommandPointer = historyCommandPointer + 1;
                //         string redoCommand = _commandHistory[historyCommandPointer];
                //         Console.WriteLine(redoCommand);
                //         Console.ReadLine();
                //     }
                //     else
                //     {
                //         Console.ReadLine();
                //     }
                // }
                //
                // if (!readline.Contains("undo")&&!readline.Contains("redo"))
                // {
                //     _commandHistory.Add(readline);
                //     historyCommandPointer = historyCommandPointer + 1;
                //     readline = Console.ReadLine();
                // }

                //write command
                //save command in array
                //undo -> (go down array)
                //redo -> (go up array)
            //}
            // adding complexity to thetodo
            //remove code. if else to switch
            //make them in methods
        }

        private static List<String> _commandHistory = new List<string>();
        private static List<TodoListItem> _todoItems = new List<TodoListItem>();
    }

    public class TodoListItem
    {
        public static TodoListItem Create(string text) => new TodoListItem(text, "", new List<string>());
        
        private TodoListItem(string text, string status, List<string> subList)
        {
            Text = text;
            Status = status;
            SubList = subList;
        }

        public string Text { get; private set; } // Autoproperty
        public string Status { get; private set;  }
        public List<string> SubList { get; private set; }

        public void UpdateStatus(string updatedStatus) => Status = updatedStatus;
        public void AddToSublist(string[] subList) => SubList.AddRange(subList);
    }
}