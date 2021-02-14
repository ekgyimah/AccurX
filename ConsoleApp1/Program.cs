using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        mistype,
        undo,
        redo,
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
                        var foundList = _stackItems.TryPeek(out var currentListItems);
                        if (foundList == false)
                        {
                            _stackItems.Push(ImmutableList.Create(todoListItem));
                            break;
                        }
                        var updatedList = currentListItems.Add(todoListItem);
                        _stackItems.Push(updatedList);
                        break;
                    }
                    case CommandMethod.list:
                    {
                        var stringbuilder = new StringBuilder();
                        foreach (var todoListItem in _stackItems.Peek())
                        {
                            stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status + ", " + String.Join(',', todoListItem.SubList));
                        }
                        Console.WriteLine(stringbuilder);
                        break;
                    }
                    // case CommandMethod.status:
                    // {
                    //     var splitString = command.Instructions.Split(",");
                    //     var foundItem = _todoItems.FirstOrDefault(x => x.Text.Equals(splitString[0]));
                    //     if (foundItem != null)
                    //     {
                    //         foundItem.UpdateStatus(splitString[1]);
                    //     }
                    //     break;
                    // }
                    // case CommandMethod.sublist:
                    // {
                    //     var splitStringSublist = command.Instructions.Split(":");
                    //     var foundItem = _todoItems.FirstOrDefault(x => x.Text.Equals(splitStringSublist[0]));
                    //     if (foundItem != null)
                    //     {
                    //         foundItem.AddToSublist(splitStringSublist[1].Split(","));
                    //     }
                    //
                    //     break;
                    // }
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
                    case CommandMethod.undo:
                    {
                        var topList = _stackItems.Pop();
                        _stackUndoItems.Push(topList);
                        break;
                    }
                    case CommandMethod.redo:
                    {
                        var topList = _stackUndoItems.Pop();
                        _stackItems.Push(topList);
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

        private static Stack<ImmutableList<TodoListItem>> _stackItems = new Stack<ImmutableList<TodoListItem>>();
        private static Stack<ImmutableList<TodoListItem>> _stackUndoItems = new Stack<ImmutableList<TodoListItem>>();
    }

    public class TodoListItem
    {
        public static TodoListItem Create(string text) => new TodoListItem(text, "", ImmutableList<string>.Empty);
        
        private TodoListItem(string text, string status, ImmutableList<string> subList)
        {
            Text = text;
            Status = status;
            SubList = subList;
        }

        public string Text { get;  } // Autoproperty
        public string Status { get;  }
        public ImmutableList<string> SubList { get;  }

        public TodoListItem UpdateStatus(string updatedStatus)
        {
            return new TodoListItem(Text, updatedStatus, SubList);
        }

        public TodoListItem AddToSublist(string[] subList)
        {
            return new TodoListItem(Text, Status, SubList.AddRange(subList));
        }
    }
}