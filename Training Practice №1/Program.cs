using System;
using System.Collections;
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
            int NumberOfSpecies = 0;
            int[] NumberOfParticles = new int[0];
            int[,] ReactionMatrix = new int[0, 0];
            Console.Clear();
            Console.WriteLine("Учебная практика №1, Власов Виктор");
            Console.WriteLine("ЗАДАЧА №507 Адронный коллайдер");
            Console.WriteLine();
            WriteFileInput(ref NumberOfSpecies, ref NumberOfParticles, ref ReactionMatrix);
            HadronCollider(NumberOfParticles, ReactionMatrix);
            Console.ReadLine();
        }

        static void WriteFileInput(ref int NumberOfSpecies, ref int[] NumberOfParticles, ref int[,] ReactionMatrix) // Чтение INPUT.TXT 
        {
            FileStream CurrFile = new FileStream("INPUT.TXT", FileMode.OpenOrCreate); // Открываем файл
            StreamReader Reader = new StreamReader(CurrFile);

            NumberOfSpecies = Int32.Parse(Reader.ReadLine()); // Считываем количество типов частиц
            NumberOfParticles = new int[NumberOfSpecies];

            char[] b = new char[NumberOfSpecies];
            Reader.Read(b, 0, NumberOfSpecies);
            for (int i = 0; i < NumberOfSpecies; i++) // Считываем количество частиц каждого типа
            {
                NumberOfParticles[i] = (int)b[i] - 48;
            }
            ReactionMatrix = new int[NumberOfSpecies, NumberOfSpecies]; // Формируем матрицу реакций частиц каждого типа

            int Count = 0;
            for (int i = 0; i < NumberOfSpecies; i++)
            {
                Reader.ReadLine();
                char[] с = new char[NumberOfSpecies];
                Reader.Read(с, 0, NumberOfSpecies);
                for (int j = 0; j < NumberOfSpecies; j++)
                {
                    ReactionMatrix[i, j] = (int)с[j] - 48;
                    if (ReactionMatrix[i, j] == 1) { Count++; }
                }
            }
            CurrFile.Close();
            Reader.Close();
        }

        static void HadronCollider(int[] NumberOfParticles, int[,] ReactionMatrix) // Основной алгоритм решения задачи 
        {
            List<List<int>> AdjacencyMatrix = new List<List<int>>();
            AdjacencyMatrix = CreateGrafSequences(NumberOfParticles, ReactionMatrix);
        }

        static List<List<int>> CreateGrafSequences(int[] NumberOfParticles, int[,] ReactionMatrix) // Создание матрицы смежности для графа описывающего всевозможные варианты 
        {
            List<List<int>> AdjacencyMatrix = new List<List<int>>(); // Создаем динамический двумерный массив
            List<int> NewLine = new List<int>();
            AdjacencyMatrix.Add(NewLine);
            AdjacencyMatrix[0].Add(0);
            AdjacencyMatrix[0].Add(0);
            for (int i = 0; i < NumberOfParticles.Length; i++)
            {
                AdjacencyMatrix[0][1] += (NumberOfParticles[i] + 1) * Numeral(NumberOfParticles.Length, i); // Добавляем единицу, так как отсутсвие частицы у нас значит 1, а не 0
            }
            AdjacencyMatrix = Vertex(AdjacencyMatrix[0][1], AdjacencyMatrix, ReactionMatrix);
            return AdjacencyMatrix;
        }

        static List<List<int>> Vertex(int Vert, List<List<int>> AdjacencyMatrix, int[,] ReactionMatrix) // Новая вершина 
        {
            List<int> NewLine = new List<int>(); // Создаем строку для новой вершины
            AdjacencyMatrix.Add(NewLine); // Добавляем ее в массив строк
            AdjacencyMatrix[AdjacencyMatrix.Count - 1].Add(AdjacencyMatrix[0][AdjacencyMatrix.Count - 1]); // Создаем для нее столбец
            AddNulls(ref AdjacencyMatrix); // Добавляем недостающие нули

            int VertCount = Vert.ToString().Length; // Вычисление количества видов частиц 
            for (int i = 0; i < VertCount; i++)
            {
                for (int j = 0; j < VertCount; j++)
                {
                    if (ReactionMatrix[i, j] == 1) // Если частицы реагируют между собой
                    {
                        if (i != j) // Если эти частицы разного вида 
                        {
                            if (GetNumeral(Vert, i) != 1 && GetNumeral(Vert, j) != 1) // Проверка на существование каждой частицы
                            {
                                int NewVert = Vert - Numeral(VertCount, j); // Вычитаем убийцу}
                                if (Check(AdjacencyMatrix, NewVert)) // Проверяем была ли такая вершина в графе
                                {
                                    AdjacencyMatrix[0].Add(NewVert); // Если нет - добавляем
                                    AddUnits(ref AdjacencyMatrix, Vert);
                                    AdjacencyMatrix = Vertex(NewVert, AdjacencyMatrix, ReactionMatrix); // Создаем новую вершину и идем дальше с помощью рекурсии
                                }
                            }
                        }
                        else // если частицы одинакового вида, то 
                        {
                            if (GetNumeral(Vert, i) == 3) // Нужно минимум две для реакции(поправка на единицу)
                            {
                                int NewVert = Vert - Numeral(VertCount, j); // Вычитаем убийцу
                                if (Check(AdjacencyMatrix, NewVert)) // Проверяем была ли такая вершина в графе
                                {
                                    AdjacencyMatrix[0].Add(NewVert); // Если нет - добавляем
                                    AddUnits(ref AdjacencyMatrix, Vert);
                                    AdjacencyMatrix = Vertex(NewVert, AdjacencyMatrix, ReactionMatrix); // Создаем новую вершину и идем дальше с помощью рекурсии
                                }
                            }
                        }
                    }
                }
            }
            return AdjacencyMatrix;
        }

        static int Numeral(int BiggestDischarge, int discharge) // Вычисление десятка 
        {
            return (int)Math.Pow(10, ((BiggestDischarge) - discharge - 1));
        }

        static int GetNumeral(int BiggestDischarge, int discharge) // Вычисление цифры 
        {
            return Int32.Parse(BiggestDischarge.ToString()[discharge].ToString());
        }

        static void AddNulls(ref List<List<int>> AdjacencyMatrix) // Добаление недостающих нулей 
        {
            for (int i = 1; i < AdjacencyMatrix.Count; i++)
            {
                for (int j = AdjacencyMatrix[i].Count; j < AdjacencyMatrix[0].Count; j++)
                {
                    AdjacencyMatrix[i].Add(0);
                }
            }
        }

        static void AddUnits(ref List<List<int>> AdjacencyMatrix, int OldVert) // Добаление недостающих нулей 
        {
            for (int i = 1; i < AdjacencyMatrix.Count; i++)
            {
                if (OldVert == AdjacencyMatrix[i][0]) { AdjacencyMatrix[i].Add(1); }
            }
        }

        static bool Check(List<List<int>> AdMatrix, int NewVert) // Проверка существовала ли такая вершина уже 
        {
            for (int i = 1; i < AdMatrix[0].Count; i++)
            {
                if (AdMatrix[0][i] == NewVert) return false;
            }
            return true;
        }
    }
}
