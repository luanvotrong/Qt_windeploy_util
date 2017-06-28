using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;

//Options:
//"--bin",
//"--qml",
//"--help"
//"--name"

namespace qmldeploy
{
    class Utilities
    {
        static private List<string> template_options = new List<string>
        {
            "--help",
            "--bin",
            "--qml",
            "--name"
        };

        static public void PrintHelp()
        {
            Console.WriteLine("------------HELP-------------");
        }

        static public Dictionary<string, string> ProcessCommand(string[] args)
        {
            Dictionary<string, string> optionsmap = new Dictionary<string, string>();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains("--"))
                    optionsmap.Add(args[i], args[i + 1]);
            }

            return optionsmap;
        }

        static public Dictionary<string, string> GenerateCommand(Dictionary<string, string> options)
        {
            Dictionary<string, string> res = new Dictionary<string, string>();

            if (options.ContainsKey("--help"))
            {
                throw new Exception("Generate help");
            }
            for(int i=1; i< template_options.Count; i++)
            {
                if(!options.ContainsKey(template_options[i]))
                {
                    throw new Exception("Incomplete command");
                }
            }

            string command = options["--bin"] + "\\windeployqt.exe";
            string option = " --qmldir " + options["--qml"] + " ";
            option = option + options["--name"];
            res.Add("command", command);
            res.Add("option", option);

            return res;
        }

        static public void CleanUp(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string p in dirs)
            {
                CleanUp(p);
                try
                {
                    Directory.Delete(p);
                }
                catch (Exception e)
                {
                    Console.WriteLine("folder using: " + p);
                    continue; //Dirty hack
                }
                Console.WriteLine("Deleted folder: " + p);
            }
            string[] files = Directory.GetFiles(path);
            foreach (string p in files)
            {
                FileInfo info = new FileInfo(p);
                if (info.Extension.ToLower() == ".dll")
                {
                    try
                    {
                        File.Delete(p);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("file using: " + p);
                        continue; //Dirty hack.
                    }

                    Console.WriteLine("Deleted file: " + info.Name + " " + info.Extension);
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();
            try
            {
                options = Utilities.ProcessCommand(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Utilities.PrintHelp();
            }

            Dictionary<string, string> command = new Dictionary<string, string>();
            try
            {
                command = Utilities.GenerateCommand(options);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Utilities.PrintHelp();
            }

            if (command.Count > 0)
            {
                Process p = new Process();
                // Redirect the output stream of the child process. 
                p.StartInfo.FileName = command["command"];
                p.StartInfo.Arguments = command["option"];
                p.Start();
                p.WaitForExit();

                p.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\" + options["--name"];
                p.StartInfo.Arguments = "";
                p.Start();
                System.Threading.Thread.Sleep(1000);
                Utilities.CleanUp(Directory.GetCurrentDirectory());
            }

            Console.Read();
        }
    }
}
