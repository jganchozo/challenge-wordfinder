using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WordFinderChallenge
{
    public class WordFinder : IWordFinder
    {
        readonly IWordFinderValidator _wordFinderValidator;
        private readonly char[][] matrix;

        public WordFinder(IEnumerable<string> matrix)
        {
            //Using the WordFinderValidator class for validations. Because, I consider that validations are not the responsibility of the WordFinder class
            _wordFinderValidator = new WordFinderValidator();
            this.matrix = ConvertListToMatrix(matrix);
        }

        public char[][] ConvertListToMatrix(IEnumerable<string> matrix)
        {
            int rowCount = matrix.Count();
            int colCount = matrix.First().Length;
            char[][] charMatrix = new char[rowCount][];

            for (int i = 0; i < rowCount; i++)
            {
                if (_wordFinderValidator.ValidateSizesBetweenTwoValues(matrix.ElementAt(i).Length, colCount))
                {
                    throw new ArgumentException("Number of characters in matrix are different");
                }

                charMatrix[i] = matrix.ElementAt(i).ToCharArray();
            }

            return charMatrix;
        }

        public IEnumerable<string> Find(IEnumerable<string> wordstream)
        {
            Dictionary<string, int> wordCounts = new Dictionary<string, int>();
            Task<int>[] tasks = new Task<int>[2];

            foreach (string word in wordstream)
            {
                int count = 0;

                if (!wordCounts.ContainsKey(word))
                {
                    //Using Task to run vertical and horizontal searches in parallel
                    tasks[0] = Task.Run(() => SearchHorizontally(word));
                    tasks[1] = Task.Run(() => SearchVertically(word));

                    Task.WaitAll(tasks);

                    foreach (var task in tasks)
                    {
                        count += task.Result;
                    }

                    if (count > 0)
                    {
                        wordCounts[word] = count;
                    }
                }
            }

            return wordCounts.OrderByDescending(x => x.Value).Select(x => x.Key).Take(10);
        }

        /// <summary>
        /// Method to search horizontally
        /// </summary>
        /// <param name="word">value to search</param>
        /// <returns></returns>
        private int SearchHorizontally(string word)
        {
            int count = 0;

            for (int row = 0; row < matrix.Length; row++)
            {
                string rowString = new string(matrix[row]);
                count += CountWordOccurrences(rowString, word);
            }

            return count;
        }

        /// <summary>
        /// Method to search vertically
        /// </summary>
        /// <param name="word">value to search</param>
        /// <returns></returns>
        private int SearchVertically(string word)
        {
            int count = 0;

            for (int col = 0; col < matrix[0].Length; col++)
            {
                string colString = new string(matrix.Select(row => row[col]).ToArray());
                count += CountWordOccurrences(colString, word);
            }

            return count;
        }

        /// <summary>
        /// Method to count occurrences
        /// </summary>
        /// <param name="input">string to search specific word</param>
        /// <param name="word">value to find</param>
        /// <returns></returns>
        private int CountWordOccurrences(string input, string word)
        {
            int count = 0;
            int index = 0;
            int start = 0;
            int end = input.Length;

            while ((start <= end) && (index > -1))
            {
                index = input.IndexOf(word, start);

                if (index == -1)
                    break;

                start = index + 1;
                count++;
            }

            return count;
        }
    }
}
