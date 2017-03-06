using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bf
{
    class Interpreter
    {
        /// <summary>
        /// The only characters that are allowed in bf source
        /// </summary>
        static HashSet<char> commands = new HashSet<char>("<>+-.,[]");

        string source;
        int programCounter = 0;
        int dataCounter = 0;

        // The program's memory. It's a dictionary so that we allocate exactly as much memory as we need
        DefaultDictionary<int, char> memory = new DefaultDictionary<int, char>();

        // Dictionaries used to quickly move the program counter around the code
        Dictionary<int, int> matchingClosedBrackets = new Dictionary<int, int>();
        Dictionary<int, int> matchingOpenBrackets = new Dictionary<int, int>();

        /// <summary>
        /// Initializes a new intepreter from the source.
        /// Throws InvalidProgramException if the source is not valid.
        /// </summary>
        public Interpreter(string source)
        {
            this.source = Interpreter.StripComments(source);
            this.FindMatchingBrackets(this.source);
        }

        /// <summary>
        /// Runs the whole source to the end.
        /// </summary>
        public void Run()
        {
            programCounter = 0;
            while (programCounter < this.source.Length)
                Execute(this.source[programCounter]);
        }

        /// <summary>
        /// Executes a single instruction from the source.
        /// </summary>
        /// <param name="code"></param>
        public void Execute(char code)
        {
            #region Conditional jumps
            if (code == '[' && memory[dataCounter] == 0)
                programCounter = matchingClosedBrackets[programCounter];

            if (code == ']' && memory[dataCounter] != 0)
                programCounter = matchingOpenBrackets[programCounter];
            #endregion

            switch (code)
            {
                case '>':
                    dataCounter++;
                    break;
                case '<':
                    dataCounter--;
                    break;
                case '+':
                    memory[dataCounter]++;
                    break;
                case '-':
                    memory[dataCounter]--;
                    break;
                case '.':
                    Console.Write(memory[dataCounter]);
                    break;
                case ',':
                    memory[dataCounter] = (char)Console.Read();
                    break;

            }

            programCounter++;
        }

        /// <summary>
        /// Removes every character from the string that is not a valid brainfuck instruction
        /// </summary>
        public static string StripComments(string source)
        {
            return new string(source.Where(c => commands.Contains(c)).ToArray());
        }

        /// <summary>
        /// Populates the dictionaries with the indices of the corresponding open and closed brackets
        /// </summary>
        protected void FindMatchingBrackets(string source)
        {
            for (int idx = 0; idx < source.Length; idx++)
            {
                if (source[idx] == '[')
                    this.matchingClosedBrackets[idx] = this.FindMatchingBracketIdx(source, idx);
            }

            // Reverse the dictionary so that we don't have to do a reverse lookup every time
            foreach (KeyValuePair<int, int> tuple in this.matchingClosedBrackets)
            {
                int openIdx = tuple.Key;
                int closedIdx = tuple.Value;
                this.matchingOpenBrackets[closedIdx] = openIdx;
            }
        }

        /// <summary>
        /// Finds the index of a matching bracket starting from an index
        /// </summary>
        protected int FindMatchingBracketIdx(string source, int startFrom, char openBracket='[', char closedBracket=']')
        {
            int counter = 0;
            for (int idx = startFrom; idx < source.Length; idx++)
            {
                if (source[idx] == openBracket)
                    counter++;
                else if (source[idx] == closedBracket)
                    counter--;

                if (counter == 0)
                    return idx;
            }

            throw new InvalidProgramException("Could not find a matching bracket starting from position " + startFrom);

        }
    }
}
