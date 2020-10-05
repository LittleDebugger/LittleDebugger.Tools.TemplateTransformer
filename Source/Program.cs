using System;
using System.IO;

namespace LittleDebugger.Tools.TemplateTransformer
{
    class Program
    {
        private static string _from;
        private static string _to;

        static void Main(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
            {
                Console.WriteLine("from agrument is invalid");
            }

            if (string.IsNullOrEmpty(to))
            {
                Console.WriteLine("to agrument is invalid");
            }

            _from = from;
            _to = to;

            var directory = Directory.GetCurrentDirectory();
            HandleDirectory(directory);
        }

        private static void HandleDirectory(string currentDirectory)
        {
            Console.WriteLine(currentDirectory);
            var subdirectories = Directory.GetDirectories(currentDirectory);
            foreach (var directory in subdirectories)
            {
                // TODO rename the directory

                HandleDirectory(currentDirectory);
            }

            foreach(var file in Directory.GetFiles(currentDirectory))
            {
                // TODO rename the filename
                // TODO check file contents
            }
        } 
    }
}
