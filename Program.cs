using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Options:
//"--bin",
//"--qml",
//"--help"
//"--name"

namespace qmldeploy
{
    class Command
    {
        public Dictionary<string, string> optionsmap;

        public Command()
        {
        }

        public Command(string[] args)
        {
        }
    }

    class Utilities
    {
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

            if(options.ContainsKey("--help"))
            {
                throw new Exception("Generate help");
            }
            if(!options.ContainsKey("--bin"))
            {
            }
            if (!options.ContainsKey("--qml"))
            {
                throw new Exception("Incomplete command");
            }
            if (!options.ContainsKey("--name"))
            {
                throw new Exception("Incomplete command");
            }

            string command = options["--bin"] + "\\windeployqt.exe";
            string option = " --qmldir " + options["--qml"] + " ";
            option = option + options["--name"];
            res.Add("command", command);
            res.Add("option", option);

            return res;
        }
    }

    class Program
    {
        static private Command command;

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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Utilities.PrintHelp();
            }

            if(command.Count > 0)
            {
                Process p = new Process();
                // Redirect the output stream of the child process. 
                p.StartInfo.FileName = command["command"];
                p.StartInfo.Arguments = command["option"];
                p.Start();
            }

            Console.Read();
        }
    }
}
