using System;
using System.Collections.Generic;
using System.Text;

namespace Homework_6
{
    public enum Command
    {
        AddStudent,
        RemoveStudent
    }

    public static class Commands
    {
        public static Dictionary<Command, ConsoleKey> buttons = new Dictionary<Command, ConsoleKey>
        {
            [Command.AddStudent] = ConsoleKey.A,
            [Command.RemoveStudent] = ConsoleKey.R,
        };
    }
    
}
