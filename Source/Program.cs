using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LittleDebugger.Tools.TemplateTransformer
{
    class Program
    {
        private static string _fromPascal;
        private static string _fromCamel;
        private static string _fromLower;
        private static string _fromUpper;

        private static string _toPascal;
        private static string _toCamel;
        private static string _toLower;
        private static string _toUpper;

        /// <summary>
        /// Please use PascalCase.
        /// Will try to support PascalCase, camelCase, all lower and ALL UPPER
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        static async Task Main(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
            {
                Console.WriteLine("from argument is invalid");
            }

            if (string.IsNullOrEmpty(to))
            {
                Console.WriteLine("to argument is invalid");
            }

            _fromPascal = from;
            _fromCamel = from[0] + string.Join("", from.Skip(1));
            _fromLower = from.ToLower();
            _fromUpper = from.ToUpper();

            _toPascal = to;
            _toCamel = to[0] + string.Join("", to.Skip(1));
            _toLower = to.ToLower();
            _toUpper = to.ToUpper();

            var directory = Directory.GetCurrentDirectory();

            if (directory.Contains(_fromCamel, StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Please run from a path which does not include the translation key.");
                return;
            }
            await HandleDirectory(directory);
            Console.WriteLine("Done");
        }

        private static async Task HandleDirectory(string currentDirectory)
        {
            var subdirectories = Directory.GetDirectories(currentDirectory);
            foreach (var directory in subdirectories)
            {
                var translatedDirectory = Translate(directory);
                if (translatedDirectory.changes)
                {
                    Console.WriteLine($"Moving directory: [{directory}] -> [{translatedDirectory}]");
                    Directory.Move(directory, translatedDirectory.translation);
                }

                await HandleDirectory(translatedDirectory.translation);
            }

            foreach(var fileName in Directory.GetFiles(currentDirectory))
            {
                var translatedFileName = Translate(fileName);
                if (translatedFileName.changes)
                {
                    Console.WriteLine($"renaming file: [{fileName}] -> [{translatedFileName}]");
                    File.Move(fileName, translatedFileName.translation);
                }

                var fileContents = (await File.ReadAllLinesAsync(translatedFileName.translation))
                    .ToArray();

                var changes = false;
                for(var i = 0; i < fileContents.Length; i++)
                {
                    var transatedLine = Translate(fileContents[i]);
                    if (transatedLine.changes)
                    {
                        if (!changes)
                        {
                            Console.WriteLine($"Changes in file [{translatedFileName.translation}]:");
                            changes = true;
                        }

                        Console.WriteLine(fileContents[i]);
                        Console.WriteLine(transatedLine.translation);
                        Console.WriteLine("----------------------------");
                        fileContents[i] = transatedLine.translation;
                    }
                }

                if (changes)
                {
                    await File.WriteAllLinesAsync(translatedFileName.translation, fileContents);
                }
            }
        }

        private static (string translation, bool changes) Translate(string line, bool changes = false)
        {
            if (!line.Contains(_fromPascal, StringComparison.InvariantCultureIgnoreCase))
            {
                return (line, changes);
            }

            if (line.Contains(_fromPascal))
            {
                return Translate(line.Replace(_fromPascal, _toPascal), true);
            }

            if (line.Contains(_fromUpper))
            {
                return Translate(line.Replace(_fromUpper, _toUpper), true);
            }

            if (line.Contains(_toLower))
            {
                return Translate(line.Replace(_fromLower, _toLower), true);
            }

            return Translate(line.Replace(_fromPascal, _toPascal, StringComparison.InvariantCultureIgnoreCase), true);
        }
    }
}
