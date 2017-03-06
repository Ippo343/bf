using System;
using System.IO;

namespace bf
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open each filename given as argument and run it
            foreach (string filename in args)
            {
                try
                {
                    string source = File.ReadAllText(filename);
                    Interpreter interpreter = new Interpreter(source);
                    interpreter.Run();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("Cannot open file " + filename);
                }
            }
        }
    }
}
