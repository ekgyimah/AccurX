using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

namespace ConsoleApp1
{

    public class CommandText // Class responsible for splitting method from instruction
    {
        public CommandText(CommandMethod method, string instructions)
        {
            Method = method;
            Instructions = instructions;
        }

        public CommandMethod Method { get; }
        public string Instructions { get; }
    }

    public enum CommandMethod //Enum class 
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
        search,
        next
    }
    
    class Program
    {
        private static CommandText _GetFromReadline(string readline) // input parser
        {
            var splitOnSpace = readline.Split(" ");
            var success = Enum.TryParse<CommandMethod>(splitOnSpace[0], out var command); //Check if command exists in Enum. Create command var
            if (!success)
            {
                var successToLower = Enum.TryParse<CommandMethod>(splitOnSpace[0].ToLower(), out var correctType);
                if (successToLower)
                {
                    return new CommandText(CommandMethod.mistype, $"It looks like you've mistyped a command, did you mean to type {correctType}?");
                }
                
                return new CommandText(CommandMethod.unknown, "It looks like you've not typed a command we recognise");
            }
            var commandText = new CommandText(command, readline.Replace(splitOnSpace[0], "")); // create command instance with split command and instruction. Removes command from instruction
            return commandText; // => commandText.instructions = [1] [2] ....
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
                        TodoListItem.AddItem(todoListItem, _stackItems);
                        break;
                    }
                    case CommandMethod.list:
                    {
                        TodoListItem.ListItems(_stackItems);
                        break;
                    }
                    case CommandMethod.status:
                    {
                        // command status items,newStatus
                        var splitString = command.Instructions.Split(",");
                        var itemList = _stackItems.Peek(); // find list
                        TodoListItem.SetStatus(splitString, itemList, _stackItems);
                        break;
                    }
                    case CommandMethod.search:
                    {
                        var splitString = command.Instructions.Split(" ");
                        var itemList = _stackItems.Peek();
                        TodoListItem.Search(splitString, itemList);
                        break;
                    }
                    case CommandMethod.next:
                    {
                        var itemList = _stackItems.Peek();
                        TodoListItem.Pagination(itemList, pagination);
                        break;
                    }
                    case CommandMethod.sublist:
                    {
                        var splitStringSublist = command.Instructions.Split(":");
                        var itemList = _stackItems.Peek();
                        TodoListItem.AddToSubList(splitStringSublist, itemList, _stackItems);
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
        }

        private static Stack<ImmutableList<TodoListItem>> _stackItems = new Stack<ImmutableList<TodoListItem>>(); //immutable list
        private static Stack<ImmutableList<TodoListItem>> _stackUndoItems = new Stack<ImmutableList<TodoListItem>>();
    }

    public class TodoListItem
    {
        public static TodoListItem Create(string text) => new TodoListItem(text, "", ImmutableList<string>.Empty); //default constructor. return empty sublist
        
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

        public static void Search(String[] splitString, ImmutableList<TodoListItem> itemList )
        {
            // var splitString = command.Instructions.Split(" ");
            var stringbuilder = new StringBuilder();
            // var itemList = _stackItems.Peek();
                        
            foreach (var todoListItem in itemList.Where(x=> x.Text.Contains(splitString[1])))
            {
                stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status);
            }
            Console.WriteLine(stringbuilder); //results
        }

        public static void SetStatus(String[] splitString, ImmutableList<TodoListItem> itemList, Stack<ImmutableList<TodoListItem>> _stackItems)
        {
            var foundItem = itemList.FirstOrDefault(x => x.Text.Equals(splitString[0]));
            if (foundItem != null)
            {
                foundItem = foundItem.UpdateStatus(splitString[1]); // delete old value
            }
            var updatedList = itemList.Add(foundItem); //add item to list
            _stackItems.Push(updatedList);
        }

        public static void Pagination(ImmutableList<TodoListItem> itemList, int pagination)
        {
            var stringbuilder = new StringBuilder();
            foreach (var todoListItem in itemList.GetRange(pagination, 2))
            {
                stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status);
            }
            Console.WriteLine(stringbuilder);
            pagination = pagination + 2;
        }

        public static void AddToSubList(String[] splitStringSublist, ImmutableList<TodoListItem> itemList, Stack<ImmutableList<TodoListItem>> _stackItems)
        {
            var foundItem = itemList.FirstOrDefault(x => x.Text.Equals(splitStringSublist[0]));
            if (foundItem != null)
            {
                foundItem = foundItem.AddToSublist(splitStringSublist[1].Split(","));
            }
            var updatedList = itemList.Add(foundItem); //add item to list
            _stackItems.Push(updatedList);
        }

        public static void ListItems(Stack<ImmutableList<TodoListItem>> _stackItems)
        {
            var stringbuilder = new StringBuilder();
            foreach (var todoListItem in _stackItems.Peek())
            {
                stringbuilder.AppendLine(todoListItem.Text + ", " + todoListItem.Status + ", " + String.Join(',', todoListItem.SubList));
            }
            Console.WriteLine(stringbuilder);
        }

        public static void AddItem( TodoListItem todoListItem, Stack<ImmutableList<TodoListItem>> _stackItems)
        {
            var foundList = _stackItems.TryPeek(out var currentListItems); //check if list exists. return the list
            if (foundList == false)
            {
                _stackItems.Push(ImmutableList.Create(todoListItem));
                return;
            }
            var updatedList = currentListItems.Add(todoListItem); //add item to list
            _stackItems.Push(updatedList);
        }
    }
}