using System;
using System.Linq;
using ConsoleTables;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Homework_6.Context.Entities;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace Homework_6
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            
            AppState currentState = AppState.Start;
            
            while (true)
            {
                switch (currentState)
                {
                    case AppState.Initialization:
                        {
                            goto case AppState.Start;
                        }

                    case AppState.Start:
                        {
                            Console.Clear();

                            using (AppContext db = new AppContext())
                            {
                                var groups = db.Groups.ToList();

                                foreach (var g in groups)
                                {
                                    var students = db.Students.Where(s => s.GroupRefId == g.Id).OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();

                                    var namingTable = new ConsoleTable(g.Name, g.Number, $"({students.Count()})");
                                    namingTable.Write(Format.Alternative);

                                    var studentsTable = new ConsoleTable("Студенты");
                                    foreach (var student in students)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        sb.AppendJoin(" ", student.LastName, student.FirstName, student.MiddleName);

                                        studentsTable.AddRow(sb.ToString());
                                    }

                                    studentsTable.Write(Format.Minimal);
                                }
                            }

                            goto case AppState.Action;
                        }

                    case AppState.Action:
                        {
                            var menu = new ConsoleTable("<Add Student>", "<Remove Student>");
                            menu.AddRow(Commands.buttons[Command.AddStudent].ToString(), Commands.buttons[Command.RemoveStudent].ToString());

                            menu.Write(Format.MarkDown);

                            while (true)
                            {
                                var key = Console.ReadKey().Key;

                                if (key == Commands.buttons[Command.AddStudent])
                                {
                                    goto case AppState.AddStudent;
                                }
                                else if (key == Commands.buttons[Command.RemoveStudent])
                                {
                                    goto case AppState.RemoveStudent;
                                }
                            }
                        }

                    case AppState.AddStudent:
                        {
                            Console.Clear();

                            List<Group> groups = new List<Group>();
                            using (AppContext db = new AppContext())
                            {
                                groups = db.Groups.ToList();

                                if (groups.Count() == 0)
                                {
                                    Console.WriteLine("There are no groups");
                                    System.Threading.Thread.Sleep(2000);
                                    goto case AppState.Start;
                                }

                                List<string> groupString = new List<string>();

                                foreach (var group in groups)
                                {
                                    groupString.Add($"<{group.Number}> - <{group.Name}>");
                                }

                                Console.WriteLine("Выберете номер группы: " + string.Join(",", groupString));
                            }

                            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                            while (true)
                            {
                                // No SQL injection prevention
                                using (AppContext db = new AppContext())
                                {
                                    Console.WriteLine();
                                    string number = Console.ReadLine();

                                    if (number == null)
                                    {
                                        Console.WriteLine("Неверный номер");
                                        continue;
                                    }
                                    number = number.Trim();
                                    number = textInfo.ToTitleCase(number);

                                    var group = db.Groups.Where(g => g.Number == number).ToList();

                                    if (group.Count() == 1)
                                    {
                                        Console.WriteLine("Введите ФИО студента: ");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный номер");
                                        continue;
                                    }

                                    bool isSaved = false;
                                    while (!isSaved)
                                    {
                                        Console.WriteLine();
                                        string fullName = Console.ReadLine();

                                        if ((fullName == "") || (fullName == null) || (fullName.Replace(" ", null).Any(char.IsDigit)))
                                        {
                                            Console.WriteLine("Неверные ФИО");
                                            continue;
                                        }
                                        fullName = fullName.Trim();
                                        fullName = textInfo.ToTitleCase(fullName);

                                        List<string> fullNameList = fullName.Split(' ').ToList();
                                        
                                        if (fullNameList.Count() == 3)
                                        {
                                            
                                            db.Students.Add(new Student { LastName = fullNameList[0], FirstName = fullNameList[1], MiddleName = fullNameList[2], GroupRefId = group[0].Id });
                                            db.SaveChanges();
                                            isSaved = true;
                                        }
                                        else if (fullNameList.Count() == 2)
                                        {
                                            db.Students.Add(new Student { LastName = fullNameList[0], FirstName = fullNameList[1], GroupRefId = group[0].Id });
                                            db.SaveChanges();
                                            isSaved = true;
                                        }
                                    }
                                }
                                goto case AppState.Start;
                            }
                        }

                    case AppState.RemoveStudent:
                        {
                            Console.Clear();
                            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;

                            while (true)
                            {
                                using (AppContext db = new AppContext())
                                {
                                    var groups = db.Groups.ToList();

                                    if (groups.Count() == 0)
                                    {
                                        Console.WriteLine("Нет групп");
                                        System.Threading.Thread.Sleep(3000);
                                        goto case AppState.Start;
                                    }

                                    foreach (var g in groups)
                                    {
                                        Console.WriteLine(g.Name + " : " + g.Number);
                                    }

                                    Console.WriteLine("Введите номер группы");
                                    string number = Console.ReadLine();

                                    if (number == null)
                                    {
                                        Console.WriteLine("Неверный номер");
                                        continue;
                                    }
                                    number = number.Trim();
                                    number = textInfo.ToTitleCase(number);

                                    var group = db.Groups.Where(g => g.Number == number).ToList();

                                    if (group.Count() == 1)
                                    {
                                        Console.WriteLine("Введите номер студента: ");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Неверный номер");
                                        continue;
                                    }

                                    bool isSaved = false;
                                    while (!isSaved)
                                    {
                                        var students = db.Students.Where(s => s.GroupRefId == group[0].Id).OrderBy(s => s.Id);

                                        foreach (var s in students)
                                        {
                                            Console.WriteLine(s.Id + " : " + s.LastName + " " + s.FirstName + " " + s.MiddleName);
                                        }

                                        Console.WriteLine();
                                        int id = 0;
                                        while (true)
                                        {
                                            try
                                            {
                                                id = Int32.Parse(Console.ReadLine());
                                            }
                                            catch (Exception)
                                            {
                                                continue;
                                            }
                                            break;
                                        }

                                        try
                                        {
                                            db.Remove(db.Students.Single(s => s.Id == id));
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        db.SaveChanges();
                                        isSaved = true;
                                    }
                                    goto case AppState.Start;
                                }
                            }
                           
                        }

                    default:
                        Console.Clear();
                        goto case AppState.Start;
                }
            }
        }
    }
}
