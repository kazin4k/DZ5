using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tumakov
{

    internal class Program
    {
        static void CountVowelsAndConsonants(char[] content, out int vowelCount, out int consonantCount)
        {
            vowelCount = 0;
            consonantCount = 0;
            List<char> characters = new List<char>(content);
            HashSet<char> vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u', 'y', 'A', 'E', 'I', 'O', 'U', 'Y' };

            foreach (char c in characters)
            {
                if (char.IsLetter(c))
                {
                    if (vowels.Contains(c))
                    {
                        vowelCount++;
                    }
                    else
                    {
                        consonantCount++;
                    }
                }
            }
        }
        public static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        public static int[,] MultiplyMatrices(int[,] matrix1, int[,] matrix2)
        {
            int rows1 = matrix1.GetLength(0);
            int cols1 = matrix1.GetLength(1);
            int rows2 = matrix2.GetLength(0);
            int cols2 = matrix2.GetLength(1);

            if (cols1 != rows2)
            {
                throw new ArgumentException("Размер матрицы не подходит под вычесления\r\n");
            }

            int[,] result = new int[rows1, cols2];

            for (int i = 0; i < rows1; i++)
            {
                for (int j = 0; j < cols2; j++)
                {
                    int sum = 0;

                    for (int k = 0; k < cols1; k++)
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }

                    result[i, j] = sum;
                }
            }

            return result;
        }
        static double[] CalculateAverageTemperatures(Dictionary<string, double[]> temperatureData)
        {
            double[] averageTemperatures = new double[temperatureData.Count];
            int index = 0;

            foreach (var entry in temperatureData)
            {
                double sum = entry.Value.Sum();
                averageTemperatures[index] = sum / entry.Value.Length;
                index++;
            }

            return averageTemperatures;
        }
        static void Main(string[] args)
        {
            //Задание 6.1
            Console.WriteLine("Задание 6.1");
            if (args.Length == 0)
            {
                Console.WriteLine("Пожалуйста, укажите имя файла в качестве аргумента\r\n");
                return;
            }

            string fileName = "Text.txt";

            try
            {
                char[] content = File.ReadAllText(fileName).ToCharArray();
                CountVowelsAndConsonants(content, out int vowelCount, out int consonantCount);
                Console.WriteLine($"Количество гласных букв: {vowelCount}");
                Console.WriteLine($"Количество согласных букв: {consonantCount}\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            // Задание 6.2 
            Console.WriteLine("Задание 6.2");
            int[,] matrix1 =
        {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9}
        };
            int[,] matrix2 =
        {
            {10, 15, 20},
            {34, 19, 13},
            {86, 27, 54}
        };
            int[,] result = MultiplyMatrices(matrix1, matrix2);
            Console.WriteLine("Матрица 1:");
            PrintMatrix(matrix1);
            Console.WriteLine("Матрица 2:");
            PrintMatrix(matrix2);
            Console.WriteLine("Результат умножения:");
            PrintMatrix(result);
            // Задание 6.3 
            Console.WriteLine("Задание 6.3");
            Random random = new Random();
            int months = 12;
            int days = 30;
            double[,] temperatures = new double[months, days];

            for (int i = 0; i < months; i++)
            {
                for (int j = 0; j < days; j++)
                {
                    temperatures[i, j] = random.Next(-10, 35);
                }
            }

            string[] monthNames = {
            "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь",
            "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"
        };

            Dictionary<string, double[]> temperatureData = new Dictionary<string, double[]>();

            for (int i = 0; i < months; i++)
            {
                double[] dailyTemperatures = new double[days];
                for (int j = 0; j < days; j++)
                {
                    dailyTemperatures[j] = temperatures[i, j];
                }
                temperatureData[monthNames[i]] = dailyTemperatures;
            }

            double[] averageTemperatures = CalculateAverageTemperatures(temperatureData);

            var sortedAverageTemperatures = averageTemperatures
                .Select((value, index) => new { Month = monthNames[index], Average = value })
                .OrderBy(x => x.Average)
                .ToList();

            Console.WriteLine("Средние температуры по месяцам:");
            foreach (var item in sortedAverageTemperatures)
            {
                Console.WriteLine($"{item.Month}: {item.Average:F2}°C");
            }
        }






    }

}













