using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace Training_Practice__1
{
    class Program
    {
        static void Main(string[] args) // Точка входа в приложение 
        {
            HeadMain();
        }

        static void PrintMenu(string[] items) // Печать меню 
        {
            int i;
            int positionX = 3, positionY = 4;

            // Программа вывода меню 
            //Начальный вывод пунктов меню.
            for (i = 0; i < items.Length; i++)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + i;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[i]);
            }
        }

        static int ShowMenu(string[] items) // Функция вывода пунктов меню 
        {
            int currentIndex = 0, previousIndex = 0, i;
            int positionX = 3, positionY = 4;
            bool itemSelected = false;

            // Программа вывода меню 
            //Начальный вывод пунктов меню.
            for (i = 0; i < items.Length; i++)
            {
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + i;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[i]);
            }

            do
            {
                // Вывод предыдущего активного пункта основным цветом.
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + previousIndex;
                Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(items[previousIndex]);


                //Вывод активного пункта.
                Console.CursorLeft = positionX;
                Console.CursorTop = positionY + currentIndex;
                Console.ForegroundColor = ConsoleColor.Black; Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(items[currentIndex]);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                previousIndex = currentIndex;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.DownArrow:
                        currentIndex++;
                        break;
                    case ConsoleKey.UpArrow:
                        currentIndex--;
                        break;
                    case ConsoleKey.Enter:
                        itemSelected = true;
                        break;
                }

                if (currentIndex == items.Length)
                    currentIndex = items.Length - 1;
                else if (currentIndex < 0)
                    currentIndex = 0;
            }
            while (!itemSelected);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray; Console.BackgroundColor = ConsoleColor.Black;
            return currentIndex;
        }

        static void HeadMain() // Функция работы главного меню 
        {
            Console.Clear();
            Console.WriteLine("Учебная практика №1, Власов Виктор");
            Console.WriteLine("ЗАДАЧА №507 Адронный коллайдер");
            string[] items1 = { "1. Открыть файл INPUT.TXT", "2. Пуск", "3. Выход" };
            Console.CursorTop = 3; Console.CursorLeft = 4;

            while (1 > 0)
            {
                switch (ShowMenu(items1))
                {
                    case 0:
                        Process.Start("INPUT.TXT");
                        break;

                    case 1:
                        HadronCollider();
                        break;

                    case 2:
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void HadronCollider() // Основной алгоритм решения задачи
        {
            FileStream CurrFile = new FileStream("INPUT.TXT", FileMode.OpenOrCreate); // Открываем файл
            StreamReader Reader = new StreamReader(CurrFile);

            int NumberOfSpecies = Int32.Parse(Reader.ReadLine()); // Считываем количество типов частиц
            int[] NumberOfParticles = new int[NumberOfSpecies];

            char[] b = new char[NumberOfSpecies];
            Reader.Read(b, 0, NumberOfSpecies);
            for (int i = 0; i < NumberOfSpecies; i++) // Считываем количество частиц каждого типа
            {
                NumberOfParticles[i] = (int)b[i] - 48;
            }
            int[,] ReactionMatrix = new int[NumberOfSpecies, NumberOfSpecies]; // Формируем матрицу реакций частиц каждого типа

            for (int i = 0; i < NumberOfSpecies; i++)
            {
                Reader.ReadLine();
                char[] с = new char[NumberOfSpecies];
                Reader.Read(с, 0, NumberOfSpecies);
                for (int j = 0; j < NumberOfSpecies; j++) // Считываем количество частиц каждого типа
                {
                    ReactionMatrix[i,j] = (int)с[j] - 48;
                }
            }
        }
    }
}
