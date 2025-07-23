using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TestProcessing_1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            IEnumerable<string> fileLines;
            // Loop until a file is successfully read or the user exits.
            while(true)
            {
                Console.Write("Please enter a file path (or type 'exit' to exit): "); // An instructive prompt for the user.
                var input = Console.ReadLine();
                if (input == null || input.ToLower().Trim() == "exit")
                {
                    return;
                }
                // TryFileReadLines returns a boolean value indicating success,
                // stores the read lines in fileLines,
                // and stores any caught exception message in errorMessage.
                if (TryFileReadLines(input, out fileLines, out var errorMessage))
                {
                    break;
                }
                Console.WriteLine($"[!] {errorMessage}");
            }
            var wordCount = CountWordsInLines(fileLines);
            LogWordCount(wordCount);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        
        private static bool TryFileReadLines(string filePath, out IEnumerable<string> fileLines, out string errorMessage)
        {
            try
            {
                // ReadLines reads and loads each line into memory as they are called for one at a time
                // unlike ReadAllLines which loads all lines into memory upfront which consumes more memory.
                fileLines = File.ReadLines(filePath);
                errorMessage = null;
                return true;
            }
            // Introduced error handling, storing the message in errorMessage.
            catch (Exception e)
            {
                fileLines = null;
                errorMessage = e.Message;
                return false;
            }
        }
        
        private static Dictionary<string, int> CountWordsInLines(IEnumerable<string> fileLines)
        {
            var wordCount = new Dictionary<string, int>();
            foreach (var fileLine in fileLines) // Reduced boilerplate code compared to a for-loop.
            {
                // Remove non-word and non-whitespace characters, and convert to lowercase.
                var cleanedLine = Regex.Replace(fileLine, @"[^\w\s]", "").ToLower();
                // Split the line into words between whitespace characters.
                var lineWords = Regex.Split(cleanedLine, @"\s+");
                foreach (var word in lineWords) // Given meaningful variable names.
                {
                    if (string.IsNullOrEmpty(word)) continue; // Defensive programming - Protection against cleaned and empty lines.
                    
                    if (wordCount.TryGetValue(word, out var count)) // Avoids double lookup in the dictionary by assigning count value for reuse.
                    {
                        wordCount[word] = count + 1;
                    }
                    else
                    {
                        wordCount[word] = 1; // Matched for consistency.
                    }
                }
            }
            return wordCount;
        }

        private static void LogWordCount(Dictionary<string, int> wordCount)
        {
            Console.WriteLine("Results:");
            foreach (var wordKVPair in wordCount)
            {
                Console.WriteLine($"  {wordKVPair.Key}: {wordKVPair.Value}");
            }
            Console.WriteLine($"Total unique words: {wordCount.Count}");
        }
    }
}
