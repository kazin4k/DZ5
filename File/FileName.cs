using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Filу
{

    class Program
    {
        static void AddEdge(Dictionary<int, List<int>> graph, int u, int v)
        {
            if (!graph.ContainsKey(u))
                graph[u] = new List<int>();
            if (!graph.ContainsKey(v))
                graph[v] = new List<int>();

            graph[u].Add(v);
            graph[v].Add(u);
        }

        static void DFS(Dictionary<int, List<int>> graph, int start, int target)
        {
            HashSet<int> visited = new HashSet<int>();
            List<int> path = new List<int>();
            bool found = DFSUtil(graph, start, target, visited, path);

            if (found)
            {
                Console.WriteLine("Путь (DFS): " + string.Join(" -> ", path));
            }
            else
            {
                Console.WriteLine("Путь не найден (DFS).");
            }
        }

        static bool DFSUtil(Dictionary<int, List<int>> graph, int current, int target, HashSet<int> visited, List<int> path)
        {
            visited.Add(current);
            path.Add(current);

            if (current == target)
                return true;

            foreach (var neighbor in graph[current])
            {
                if (!visited.Contains(neighbor))
                {
                    if (DFSUtil(graph, neighbor, target, visited, path))
                        return true;
                }
            }

            path.RemoveAt(path.Count - 1);
            return false;
        }

        static void BFS(Dictionary<int, List<int>> graph, int start, int target)
        {
            Queue<int> queue = new Queue<int>();
            Dictionary<int, int> parent = new Dictionary<int, int>();
            HashSet<int> visited = new HashSet<int>();

            queue.Enqueue(start);
            visited.Add(start);
            parent[start] = -1;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == target)
                {
                    PrintPath(start, target, parent);
                    return;
                }

                foreach (var neighbor in graph[current])
                {
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                        parent[neighbor] = current;
                    }
                }
            }

            Console.WriteLine("Путь не найден (BFS).");
        }

        static void PrintPath(int start, int target, Dictionary<int, int> parent)
        {
            List<int> path = new List<int>();
            for (int at = target; at != -1; at = parent[at])
            {
                path.Add(at);
            }
            path.Reverse();
            Console.WriteLine("Путь (BFS): " + string.Join(" -> ", path));
        }
        static void InputBabushkas(List<string> babushkaNames, List<int> babushkaAges, List<List<string>> babushkaDiseases, List<List<string>> babushkaMedicines, int maxBabushkas)
        {
            for (int i = 0; i < maxBabushkas; i++)
            {
                Console.Write($"Введите имя бабушки (или 'exit' для выхода): ");
                string name = Console.ReadLine();
                if (name.ToLower() == "exit") break;

                Console.Write("Введите возраст бабушки: ");
                int age = int.Parse(Console.ReadLine());

                Console.Write("Введите болезни бабушки (через запятую): ");
                List<string> diseases = new List<string>(Console.ReadLine().Split(','));

                Console.Write("Введите лекарства бабушки (через запятую): ");
                List<string> medicines = new List<string>(Console.ReadLine().Split(','));

                babushkaNames.Add(name);
                babushkaAges.Add(age);
                babushkaDiseases.Add(diseases);
                babushkaMedicines.Add(medicines);
            }
        }

        static void DistributeBabushkas(List<string> babushkaNames, List<int> babushkaAges, List<List<string>> babushkaDiseases, List<string> hospitalNames, List<List<string>> hospitalDiseases, List<int> hospitalCapacities, List<int> hospitalCurrentPatients)
        {
            for (int i = 0; i < babushkaNames.Count; i++)
            {
                string currentBabushka = babushkaNames[i];
                Console.WriteLine($"\nБабушка {currentBabushka} поступила на лечение.");

                if (babushkaDiseases[i].Count == 0)
                {
                    for (int j = 0; j < hospitalNames.Count; j++)
                    {
                        if (hospitalCurrentPatients[j] < hospitalCapacities[j])
                        {
                            hospitalCurrentPatients[j]++;
                            Console.WriteLine($"{currentBabushka} попала в {hospitalNames[j]} для консультации.");
                            break;
                        }
                    }
                    continue;
                }

                bool admitted = false;
                for (int j = 0; j < hospitalNames.Count; j++)
                {
                    int treatedCount = 0;
                    foreach (var disease in babushkaDiseases[i])
                    {
                        if (hospitalDiseases[j].Contains(disease.Trim()))
                        {
                            treatedCount++;
                        }
                    }

                    if (treatedCount > 0 && treatedCount * 2 > babushkaDiseases[i].Count && hospitalCurrentPatients[j] < hospitalCapacities[j])
                    {
                        hospitalCurrentPatients[j]++;
                        admitted = true;
                        Console.WriteLine($"{currentBabushka} попала в {hospitalNames[j]} на лечение.");
                        break;
                    }
                }

                if (!admitted)
                {
                    Console.WriteLine($"{currentBabushka} не нашла больницу для лечения и осталась на улице плакать.");
                }
            }
        }

        static void PrintBabushkas(List<string> babushkaNames, List<int> babushkaAges, List<List<string>> babushkaDiseases)
        {
            Console.WriteLine("\nСписок бабушек:");
            for (int i = 0; i < babushkaNames.Count; i++)
            {
                Console.WriteLine($"Имя: {babushkaNames[i]}, Возраст: {babushkaAges[i]}, Болезни: {string.Join(", ", babushkaDiseases[i])}");
            }
        }

        static void PrintHospitals(List<string> hospitalNames, List<List<string>> hospitalDiseases, List<int> hospitalCurrentPatients)
        {
            Console.WriteLine("\nСписок больниц:");
            for (int j = 0; j < hospitalNames.Count; j++)
            {
                Console.WriteLine($"Название: {hospitalNames[j]}, Болезни: {string.Join(", ", hospitalDiseases[j])}, Текущие пациенты: {hospitalCurrentPatients[j]}");
            }
        }
        static List<Tuple<string, string, int, string, int>> LoadStudentsFromFile(string filePath)
        {
            List<Tuple<string, string, int, string, int>> students = new List<Tuple<string, string, int, string, int>>();

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 5)
                    {
                        students.Add(new Tuple<string, string, int, string, int>(
                            parts[0],
                            parts[1],
                            int.Parse(parts[2]),
                            parts[3],
                            int.Parse(parts[4])
                        ));
                    }
                }
            }
            return students;
        }

        static void AddNewStudent(List<Tuple<string, string, int, string, int>> students)
        {
            Console.Write("Введите фамилию: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите имя: ");
            string firstName = Console.ReadLine();
            Console.Write("Введите год рождения: ");
            int birthYear = int.Parse(Console.ReadLine());
            Console.Write("Введите экзамен: ");
            string exam = Console.ReadLine();
            Console.Write("Введите баллы: ");
            int score = int.Parse(Console.ReadLine());

            students.Add(new Tuple<string, string, int, string, int>(lastName, firstName, birthYear, exam, score));

            Console.WriteLine("Студент добавлен.");
        }

        static void RemoveStudent(List<Tuple<string, string, int, string, int>> students)
        {
            Console.Write("Введите фамилию студента: ");
            string lastName = Console.ReadLine();
            Console.Write("Введите имя студента: ");
            string firstName = Console.ReadLine();

            var studentToRemove = students.FirstOrDefault(s => s.Item1.Equals(lastName, StringComparison.OrdinalIgnoreCase) &&
                                                                s.Item2.Equals(firstName, StringComparison.OrdinalIgnoreCase));

            if (studentToRemove != null)
            {
                students.Remove(studentToRemove);
                Console.WriteLine("Студент удален.");
            }
            else
            {
                Console.WriteLine("Студент не найден.");
            }
        }

        static void SortStudents(List<Tuple<string, string, int, string, int>> students)
        {
            students.Sort((s1, s2) => s1.Item5.CompareTo(s2.Item5));
            Console.WriteLine("Студенты отсортированы по баллам.");
        }

        static void PrintStudents(List<Tuple<string, string, int, string, int>> students)
        {
            Console.WriteLine("\nСписок студентов:");
            foreach (var student in students)
            {
                Console.WriteLine($"Фамилия: {student.Item1}, Имя: {student.Item2}, Год рождения: {student.Item3}, Экзамен: {student.Item4}, Баллы: {student.Item5}");
            }
        }
        static List<string> CreateAndShuffleImageList()
        {
            List<string> images = new List<string>();

            for (int i = 1; i <= 32; i++)
            {
                string imagePath = $"image_{i}.jpg";
                images.Add(imagePath);
                images.Add(imagePath);
            }

            return images;
        }

        static void ShuffleList(List<string> list)
        {
            Random rng = new Random();
            List<string> shuffled = list.OrderBy(x => rng.Next()).ToList();
            list.Clear();
            list.AddRange(shuffled);
        }
        static void Main(string[] args)
        {
            //Задание 1
            Console.WriteLine("Задание 1");
            List<string> shuffledImages = CreateAndShuffleImageList();

            Console.WriteLine("Оригинальные индексы:");
            for (int i = 0; i < shuffledImages.Count; i++)
            {
                Console.WriteLine($"Индекс {i}: {shuffledImages[i]}");
            }

            Console.WriteLine("\nПеремешанные индексы:");
            List<string> originalImages = new List<string>(shuffledImages);
            ShuffleList(originalImages);
            for (int i = 0; i < originalImages.Count; i++)
            {
                Console.WriteLine($"Индекс {i}: {originalImages[i]}");
            }

            //Задание 2
            Console.WriteLine("Задание 2");
            List<Tuple<string, string, int, string, int>> students = LoadStudentsFromFile("C:\\Users\\Amir\\source\\repos\\DZ5\\File\\students.txt");
            string command;

            do
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Новый студент");
                Console.WriteLine("2. Удалить");
                Console.WriteLine("3. Сортировать");
                Console.WriteLine("4. Вывести список студентов");
                Console.WriteLine("5. Выход");
                Console.Write("Введите команду: ");
                command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        AddNewStudent(students);
                        break;
                    case "2":
                        RemoveStudent(students);
                        break;
                    case "3":
                        SortStudents(students);
                        break;
                    case "4":
                        PrintStudents(students);
                        break;
                    case "5":
                        Console.WriteLine("Выход из программы.");
                        break;
                    default:
                        Console.WriteLine("Некорректная команда. Попробуйте снова.");
                        break;
                }
            } while (command != "5");

            //Задание 3
            Console.WriteLine("Задание 3");
            const int maxBabushkas = 10;
            List<string> babushkaNames = new List<string>();
            List<int> babushkaAges = new List<int>();
            List<List<string>> babushkaDiseases = new List<List<string>>();
            List<List<string>> babushkaMedicines = new List<List<string>>();

            List<string> hospitalNames = new List<string> { "Больница №1", "Больница №2", "Больница №3" };
            List<List<string>> hospitalDiseases = new List<List<string>>
        {
            new List<string> { "грипп", "простуда" },
            new List<string> { "артрит", "грипп" },
            new List<string> { "диабет", "грипп" }
        };
            List<int> hospitalCapacities = new List<int> { 2, 2, 2 };
            List<int> hospitalCurrentPatients = new List<int> { 0, 0, 0 };

            InputBabushkas(babushkaNames, babushkaAges, babushkaDiseases, babushkaMedicines, maxBabushkas);
            DistributeBabushkas(babushkaNames, babushkaAges, babushkaDiseases, hospitalNames, hospitalDiseases, hospitalCapacities, hospitalCurrentPatients);
            PrintBabushkas(babushkaNames, babushkaAges, babushkaDiseases);
            PrintHospitals(hospitalNames, hospitalDiseases, hospitalCurrentPatients);

            //Задание 4
            Console.WriteLine("Задание 4");
            Dictionary<int, List<int>> graph = new Dictionary<int, List<int>>();

            AddEdge(graph, 0, 1);
            AddEdge(graph, 0, 2);
            AddEdge(graph, 1, 3);
            AddEdge(graph, 1, 4);
            AddEdge(graph, 2, 5);
            AddEdge(graph, 3, 6);
            AddEdge(graph, 4, 6);
            AddEdge(graph, 5, 6);

            int start = 0;
            int target = 6;

            Console.WriteLine("Обход в глубину:");
            DFS(graph, start, target);

            Console.WriteLine("\nОбход в ширину:");
            BFS(graph, start, target);

        }
    }
}
