using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace lab56
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(); //инициализация переменной для ДСЧ 

            int minLen = 1, maxLen = System.Array.MaxLength; //мнимальная и максимальная длина возможного массива
            int strings = 0, columns = 0; //строки и столбцы массива
            int command; //номер для выбора
            int k; //номер удаления/добавления столбцов/строк
            int[,] arr1 = new int[0, 0];
            int[][] arr2 = new int[0][];

            do
            {
                PrintMenu(); //вывод  меню
                command = ChooseNum(1, 7); //выбор команды
                switch (command)
                {
                    case 1:
                        arr1 = CreateTwoDimenArr(maxLen, minLen, rnd); //создание двумерного массива
                        break;
                    case 2:
                        arr1 = DeleteColumn(arr1, out k); //удаление k-го столбца
                        break;
                    case 3:
                        arr2 = CreateTornArr(arr2, ref maxLen, minLen, rnd);
                        break;
                    case 4:
                        arr2 = AddStrings(arr2, ref maxLen, minLen, ref strings, rnd);
                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                }
            } while (command != 7);
            Console.WriteLine("bye");
        }

        static void PrintMenu() // меню
        {
            Console.WriteLine(@"Выберите команду
1. Сформровать и распечатать двумерный массив
2. Удалить столбец из двумерного массива по номеру
3. Сформировать и распечатать рваный массив
4. Добавить определённое количестов строк в конец массива
5. Ввести строку и вывести её на печать
6. Удалить первое и последнее слово в каждом предложении введённой строки и вывести на печать
7. Выход");
        } 

        static int[,] CreateTwoDimenArr(int maxLen, int minLen, Random rnd) //формирование двумерного массива
        {
            int strings = CheckSizeArr($"Введите количество строк от 1 до {maxLen}", maxLen, minLen);
            int [,] arr = new int[strings, CheckSizeArr($"Введите количество столбцов от 1 до {maxLen / strings}", maxLen, minLen)];

            Console.WriteLine("Как вы хотите сформировать элементы массива?\n1. С помощью ДСЧ\n2. Ввод с клавиатуры");
            int chooseWay = ChooseNum(1, 2); //выбор формирования элементов
            switch (chooseWay)
            {
                case 1:
                    arr = RandomArr1(arr, rnd); //формирование с помощью ДСЧ
                    break;
                default:
                    arr = InputArr1(arr); //формирование вводом с клавиатуры
                    break;
            }
            return arr;
        }

        static int[,] DeleteColumn(int[,] arr, out int k)
        {
            if (CheckEmpty(arr))
            {
                int[,] temp = new int[arr.GetLength(0), arr.GetLength(1) - 1];
                Console.WriteLine($"Выберите столбец, который нужно удалить в массиве. От 1 до {arr.GetLength(1)}");
                k = ChooseNum(1, arr.GetLength(1));
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (j == k - 1) continue;
                        if (j < k - 1)
                        {
                            temp[i, j] = arr[i, j];
                            Console.Write(temp[i, j] + " ");
                        }
                        else
                        {
                            temp[i, j - 1] = arr[i, j];
                            Console.Write(temp[i, j - 1] + " ");
                        }
                    }
                Console.WriteLine();
                if (temp.Length == 0)
                    Console.WriteLine("Массив пустой");
                return temp;   
            }
            k = 0;
            return arr;
        }

        static int[][] CreateTornArr(int[][] arr, ref int maxLen, int minLen, Random rnd)
        {
            int strings = CheckSizeArr($"Введите количество строк от 1 до {maxLen}", maxLen, minLen);
            arr = new int[strings][];
            maxLen /= strings;
            strings = 0;
            Console.WriteLine("Как вы хотите сформировать элементы массива?\n1. С помощью ДСЧ\n2. Ввод с клавиатуры");
            int chooseWay = ChooseNum(1, 2); //выбор формирования элементов
            switch (chooseWay)
            {
                case 1:
                    arr = RandomArr2(arr, ref maxLen, minLen, strings, rnd); //формирование с помощью ДСЧ
                    break;
                default:
                    arr = InputArr2(arr, ref maxLen, minLen, strings); //формирование вводом с клавиатуры
                    break;
            }
            return arr;        
        }

        static int[][] AddStrings(int[][] arr, ref int maxLen, int minLen, ref int strings, Random rnd)
        {
            Console.WriteLine($"Введите количество строк, которое вы хотите добавить в конец массива от 1 до {maxLen}");
            int k = ChooseNum(minLen, maxLen);
            int[][] temp = new int[arr.GetLength(0) + k][];
            strings = temp.GetLength(0);
            
            Console.WriteLine(temp[0][3]);
            Console.WriteLine("Как вы хотите сформировать элементы новых строк массива?\n1. С помощью ДСЧ\n2. Ввод с клавиатуры");
            int chooseWay = ChooseNum(1, 2); //выбор формирования элементов
            switch (chooseWay)
            {
                case 1: //формирование с помощью ДСЧ
                    arr = RandomArr2(temp, ref maxLen, minLen, strings, rnd);
                    break;
                default: //формирование вводом с клавиатуры
                    arr = InputArr2(temp, ref maxLen, minLen, strings);
                    break;
            }
            return arr;
        }

        /*static string CreateString(ref string message)
        {

        }

        static string DeleteFirstLastWord(string message)
        {

        }*/
        
        static int CheckVar() //проверка вводимого числа
        {
            int num;
            bool isRight;
            Console.WriteLine("Введите целое число");
            do
            {
                isRight = Int32.TryParse(Console.ReadLine(), out num);
                if (!isRight)
                    Console.WriteLine("Неверный ввод. Введите целое число");
            } while (!isRight);
            return num;
        }

        static int CheckSizeArr(string message, int maxLen, int minLen) //проверка правильности ввода длин размерностей массива
        {
            int size; // длина строки/столбца
            bool isRight;
            Console.WriteLine(message);
            do
            {
                isRight = Int32.TryParse(Console.ReadLine(), out size);
                if (!isRight || !(size <= maxLen && size >=minLen))
                    Console.WriteLine("Неверный ввод. Введите целое число");
            } while (!isRight);
            return size;
        }


        static int[,] RandomArr1(int[,] arr, Random rnd) //создание массива с помощью ДСЧ
        { 
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = rnd.Next(-100, 100);
                    Console.Write(arr[i, j] + " ");
                }
            Console.WriteLine("\nМассив сформирован и распечатан\n");
            return arr;
        }

        static int[,] InputArr1(int[,] arr) //создание массива вводом с клавиатуры
        {
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = CheckVar();
                    Console.Write(arr[i, j] + " ");
                }
            Console.WriteLine("\nМассив сформирован и распечатан\n");
            return arr;
        }

        static int ChooseNum(int minCommand, int maxCommand) // выбор номера (команды)
        {
            int command; //номер команды
            bool isRight;
            do
            {
                isRight = Int32.TryParse(Console.ReadLine(), out command);
                if (!isRight || !(command <= maxCommand && command >= minCommand))
                    Console.WriteLine("Неверный ввод. Введите целое число");
            } while (!isRight);
            return command;
        }

        static bool CheckEmpty(int[,] arr) // проверка на пустоту массива
        {
            bool isRight = false;
            if (arr.GetLength(1) == 0)
            {
                Console.WriteLine("В столбцах нет занчений, создайте новый массив");
            }
            else
                isRight = true;
            return isRight;
        }

        static int[][] RandomArr2(int[][] arr, ref int maxLen, int minLen, int strings, Random rnd) //создание массива с помощью ДСЧ
        {
            for (int i = strings; i < arr.GetLength(0); i++)
            {
                int columns = CheckSizeArr($"Введите количество столбцов {i + 1} строки от 1 до {maxLen}", maxLen, minLen);
                arr[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    arr[i][j] = rnd.Next(-100, 100);
                }
                maxLen -= columns;
            }
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                foreach (int x in arr[i]) Console.Write(x + " ");
                Console.WriteLine();
            }
            Console.WriteLine("Массив сформирован и распечатан\n");
            return arr;
        }

        static int[][] InputArr2(int[][] arr, ref int maxLen, int minLen, int strings) //создание массива вводом с клавиатуры
        {
            for (int i = strings; i < arr.GetLength(0); i++)
            {
                int columns = CheckSizeArr($"Введите количество столбцов {i + 1} строки от 1 до {maxLen}", maxLen, minLen);
                arr[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    arr[i][j] = CheckVar();
                }
                maxLen -= columns;
            }
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                foreach (int x in arr[i]) Console.Write(x + " ");
                Console.WriteLine();
            }
            Console.WriteLine("Массив сформирован и распечатан\n");
            return arr;
        }
    }
}
