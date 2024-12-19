using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace lab56
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random(); //инициализация переменной для ДСЧ 

            int minLen = 1, maxLen = System.Array.MaxLength; //мнимальная и максимальная длина возможного массива
            int strings = 1, columns = 0; //строки и столбцы массива
            int command; //номер для выбора
            int k; //номер удаления/добавления столбцов/строк
            int[,] arr1 = new int[0, 0]; //инициализация двумерного массива
            int[][] arr2 = new int[0][]; //инициализация рваного массива
            string message = ""; //основная строка для работы со строками

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
                        arr2 = CreateTornArr(arr2, ref maxLen, minLen, rnd); //создание рваного массива
                        break;
                    case 4:
                        arr2 = AddStrings(arr2, ref maxLen, minLen, ref strings, rnd); //добавление строк в рваный массив
                        break;
                    case 5:
                        bool isFiveOrSix = false; //флаг для функции проверки строки
                        message = CreateString(ref isFiveOrSix); //создание строки
                        break;
                    case 6:
                        isFiveOrSix = true;
                        message = DeleteFirstLastWord(ref isFiveOrSix, message); //удаление первого и последнего слова в каждом предложении
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
                case 2:
                    arr = InputArr1(arr); //формирование вводом с клавиатуры
                    break;
                default:
                    break;
            }
            return arr;
        }

        static int[,] DeleteColumn(int[,] arr, out int k) //удаление столбца в матрице
        {
            if (CheckEmpty(arr))
            {
                int[,] tempArr = new int[arr.GetLength(0), arr.GetLength(1) - 1];
                Console.WriteLine($"Выберите столбец, который нужно удалить в массиве. От 1 до {arr.GetLength(1)}");
                k = ChooseNum(1, arr.GetLength(1));
                for (int i = 0; i < arr.GetLength(0); i++)
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (j == k - 1) continue;
                        if (j < k - 1)
                            tempArr[i, j] = arr[i, j];
                        else
                            tempArr[i, j - 1] = arr[i, j];
                    }
                for (int i = 0; i < tempArr.GetLength(0); i++)
                {
                    for (int j = 0; j < tempArr.GetLength(1); j++)
                        Console.Write(tempArr[i, j] + " ");
                    Console.WriteLine();
                }
                if (tempArr.Length == 0)
                    Console.WriteLine("Массив пустой");
                return tempArr;   
            }
            k = 0;
            return arr;
        }

        static int[][] CreateTornArr(int[][] arr, ref int maxLen, int minLen, Random rnd) //создание рваного массива
        {
            maxLen /= 2;
            int strings = CheckSizeArr($"Введите количество строк от 1 до {maxLen}", maxLen, minLen);
            arr = new int[strings][];
            Console.WriteLine("Как вы хотите сформировать элементы массива?\n1. С помощью ДСЧ\n2. Ввод с клавиатуры");
            int chooseWay = ChooseNum(1, 2); //выбор формирования элементов
            switch (chooseWay)
            {
                case 1:
                    arr = RandomArr2(arr, ref maxLen, minLen, strings, rnd); //формирование с помощью ДСЧ
                    break;
                case 2:
                    arr = InputArr2(arr, ref maxLen, minLen, strings); //формирование вводом с клавиатуры
                    break;
                default:
                    Console.WriteLine("Неверный ввод команды. Введите целое число - команду");
                    break;
            }
            return arr;        
        }

        static int[][] AddStrings(int[][] arr, ref int maxLen, int minLen, ref int strings, Random rnd) //добавление строк в рваный массив
        {
            int sumElem = 0;
            for (int i = 0; i < arr.GetLength(0); i++)
                sumElem += (arr[i].Length + 1);
            if (sumElem == System.Array.MaxLength)
                Console.WriteLine("Массив полностью заполнен. Добавление строк невозможно");
            else
            {
                maxLen /= 2; //уменьшение макс длины в 2 раза для вмещения минимум по одному элементу в каждой строке
                strings = 0;
                Console.WriteLine($"Введите количество строк, которое вы хотите добавить в конец массива от 1 до {maxLen}");
                int k = ChooseNum(minLen, maxLen); //кол-во строк для ввода
                int[][] tempArr = new int[arr.GetLength(0) + k][]; //вспомогательный рваный массив
                strings = k;
                for (int i = 0; i < arr.GetLength(0); i++) //копирование элементов первоначального массива
                {
                    tempArr[i] = new int[arr[i].Length];
                    arr[i].CopyTo(tempArr[i], 0);
                }
                Console.WriteLine("Как вы хотите сформировать элементы новых строк массива?\n1. С помощью ДСЧ\n2. Ввод с клавиатуры");
                int chooseWay = ChooseNum(1, 2); //выбор формирования элементов
                switch (chooseWay)
                {
                    case 1: //формирование с помощью ДСЧ
                        tempArr = RandomArr2(tempArr, ref maxLen, minLen, strings, rnd);
                        break;
                    case 2: //формирование вводом с клавиатуры
                        tempArr = InputArr2(tempArr, ref maxLen, minLen, strings);
                        break;
                    default:
                        Console.WriteLine("Неверный ввод команды. Введите целое число - команду");
                        break;
                }
                return tempArr;
            }
            return arr;
        }

        static string CreateString(ref bool isFiveOrSix) //создание строки
        {
            string inputString = null;
            string[] stringArr1 = { "  a", "b", "c", "сидели ", "на", "   трубе." };
            string[] stringArr2 = { "1233вававп."};
            string[] stringArr3 = { "static void,, ", "PrintUpper; ", "info1213", "сидели ", " ", "1223info" };
            string[] stringArr4 = { " 11quen", "12quen", "121dsf", "сидели "};
            string[] stringArr5 = { "Я", "люблю", "чипсеки", "#1" };
            string[] stringArr6 = { "....", "   ", "c", "сидели ", "на", "   трубе!!!!" };
            string[] stringArr7 = { "В", "лесу", "родилась", "елочка!", "В", "лесу", "она", "росла.", "Зимой", "и", "летом", "стройная", "зеленая", "была?" }; //тестовые массивы
            string[] stringArr8 = { "Владлена", "Дмитриевна", "лучший", "преподавтель", "!"};
            Console.WriteLine($"Как вы хотите сформировать строку\n1. Ввод с клавиатуры\n2. Работа с заранее подготовленным массивом");
            int chooseWay = ChooseNum(1, 2);
            switch (chooseWay)
            {
                case 1:
                    inputString = CheckString(ref isFiveOrSix, inputString); //ввод с клавиатуры
                    Console.WriteLine(inputString);
                    break;
                case 2: //выбор массива
                    isFiveOrSix = true;
                    Console.WriteLine("Какой тестовый массив вы хотите выбрать\nНекоторые массивы демонстрируются, чтобы вы поняли какие строки вызывают ошибку");
                    Console.WriteLine(@"Выберете массив от 1 до 8
1. { ""  a"", ""b"", ""c"", ""сидели "", ""на"", ""   трубе."" } - верный
2. { ""1233вававп.""} - неверный
3. { ""static void,, "", ""PrintUpper; "", ""info1213"", ""сидели "", "" "", ""1223info"" } - неверный
4. { "" 11quen"", ""12quen"", ""121dsf"", ""сидели ""} - неверный
5. { ""Я"", ""люблю"", ""чипсеки"", ""#1"" } - неверный
6. { ""...."", ""   "", ""c"", ""сидели "", ""на"", ""   трубе!!!!"" } - неверный
7. { ""В"", ""лесу"", ""родилась"", ""елочка!"", ""В"", ""лесу"", ""она"", ""росла."", ""Зимой"", ""и"", ""летом"", ""стройная"", ""зеленая"", ""была?"" } - верный
8.{ ""Владлена"", ""Дмитриевна"", ""лучший"", ""преподавтель"", ""!""} - верный");
                    int chooseArr = ChooseNum(1, 8);
                    switch (chooseArr) //вывод и проверка массивов на соответствие шаблону (массивы выводятся в любом случае, чтобы пользователь могу увидеть)
                    {
                        case 1:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr1).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 2:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr2).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 3:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr3).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 4:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr4).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 5:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr5).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 6:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr6).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 7:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr7).Trim());
                            Console.WriteLine(inputString);
                            break;
                        case 8:
                            inputString = CheckString(ref isFiveOrSix, string.Join(" ", stringArr8).Trim());
                            Console.WriteLine(inputString);
                            break;
                        default:
                            break;  
                    }
                    break;
                default:
                    break;
            }
            Console.WriteLine("Строка распечатана");
            return inputString;
        }

        static string DeleteFirstLastWord(ref bool isFiveOrSix, string message)
        {
            string[] stringArr = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries); //создание массива с разделителем space
            string[] tempStrArr = new string[stringArr.Length];
            if (message == "") //если строка пустая
            {
                Console.WriteLine("Пустая строка невозможно действие. Создайте сначала строку.");
                return "";
            }
            else
            {
                if (stringArr.Length == 1) //если строка - одно слово(буква)
                {
                    Console.WriteLine("В массиве одно слово, невозможно удалить первое и последнее слово. Создайте новую строку");
                }
                else
                {
                    int count = 2;
                    bool isUpCase = false;
                    if (!stringArr[0].Contains(".") & !stringArr[0].Contains("!") & !stringArr[0].Contains("?"))
                        tempStrArr[0] = " "; //так как первое слово в предложениии точно надо будет удалить, то это происходит без цикла
                    else
                    {
                        tempStrArr[0] = stringArr[0];
                        char[] charArr3 = tempStrArr[0].ToCharArray(); //преобразования чтобы сделать первую букву первого слова заглавной
                        charArr3[0] = char.ToUpper(charArr3[0]);
                        tempStrArr[0] = String.Concat(charArr3);
                    }
                    for (int i = 1; i < stringArr.Length; i++)
                    {
                        if (stringArr[i - 1].Contains(".") || stringArr[i - 1].Contains("!") || stringArr[i - 1].Contains("?")) //если прошлый элемент массиваа строк содержит знак окончания предложения
                        {
                            if (i != stringArr.Length - 1 || (!(stringArr[i].Contains(".") & !stringArr[i].Contains("!") & !stringArr[i].Contains("?"))))
                            {
                                tempStrArr[i] = " ";
                                char[] charArr1 = stringArr[i + 1].ToCharArray();
                                charArr1[0] = char.ToUpper(charArr1[0]);
                                stringArr[i + 1] = String.Concat(charArr1); //слово после окончания предложения - с прописной буквы
                                isUpCase = true; //флаг строки выше
                            }
                        }
                        if (stringArr[i].Contains(".") || stringArr[i].Contains("!") || stringArr[i].Contains("?")) //если элемент массиваа строк содержит знак окончания предложения
                        {
                            if (tempStrArr[i - 1].Contains(",") || tempStrArr[i - 1].Contains(":") || tempStrArr[i - 1].Contains(";")) //если предыдущее слово содержит знак препинания
                                tempStrArr[i - 1] = tempStrArr[i - 1].Remove(tempStrArr[i - 1].Length - 1, 1);
                            if (!(tempStrArr[i - 1] == (" ")) & !tempStrArr[i - 1].Contains(".") & !tempStrArr[i - 1].Contains("!") & !tempStrArr[i - 1].Contains("?")) //если предыдущее слово не пустое
                            {
                                char[] charArr2 = stringArr[i].ToCharArray();
                                tempStrArr[i - 1] += charArr2[^1].ToString();
                                tempStrArr[i] = " ";
                            }
                            else
                            {
                                
                                if (stringArr[i - 1].Contains(".") || stringArr[i - 1].Contains("!") || stringArr[i - 1].Contains("?"))
                                {
                                    Console.WriteLine($"В {count++} предложении одно слово, удаление невозможно");
                                    char[] charArr3 = stringArr[i].ToCharArray(); //преобразования чтобы сделать первую букву первого слова заглавной
                                    charArr3[0] = char.ToUpper(charArr3[0]);
                                    tempStrArr[i] = String.Concat(charArr3);
                                }
                                else
                                    tempStrArr[i] = " ";
                                //Console.WriteLine("Строка стала пустой");
                            }
                        }
                        else //если прошлый элемент массива был знаком окончания предложения, то ничего не происходит, иначе присваиваем временному массиву элеент первоначального
                        {
                            if (tempStrArr[i] == null || (!isUpCase & tempStrArr[i][0] < 'A'))
                                tempStrArr[i] = stringArr[i]; //если слово не первое в предложении
                            isUpCase = false;
                        }
                    }
                }
                if (stringArr.Length > 1)
                {
                    message = string.Join(" ", tempStrArr); //преобразование в строку
                    message = string.Join(" ", message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
                    if (message == "") Console.WriteLine("Строка стала пустой");
                    else //делаем первую букву предложения заглавной
                    {

                        char[] charArr = message.ToCharArray();
                        charArr[0] = char.ToUpper(charArr[0]);
                        message = String.Concat(charArr);
                    }
                    Console.WriteLine(message);
                } 
            }
            return message;
        }
        
        static int CheckVar(string message) //проверка вводимого числа
        {
            int num;
            bool isRight;
            Console.WriteLine(message);
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
            } while (!isRight || !(size <= maxLen && size >= minLen));
            return size;
        }

        static int[,] RandomArr1(int[,] arr, Random rnd) //создание массива с помощью ДСЧ
        { 
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = rnd.Next(-100, 100);
                }
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                    Console.Write(arr[i, j] + " ");
                Console.WriteLine();
            }    
            Console.WriteLine("Массив сформирован и распечатан\n");
            return arr;
        }

        static int[,] InputArr1(int[,] arr) //создание массива вводом с клавиатуры
        {
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = CheckVar($"Введите {i * arr.GetLength(1) + j + 1} элемент двумерного массива");
                }
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                    Console.Write(arr[i, j] + " ");
                Console.WriteLine();
            }        
            Console.WriteLine("Массив сформирован и распечатан\n");
            return arr;
        }

        static int ChooseNum(int minCommand, int maxCommand) // выбор номера (команды)
        {
            int command; //номер команды
            bool isRight;
            do
            {
                isRight = Int32.TryParse(Console.ReadLine(), out command);
                if (!isRight || (command > maxCommand || command < minCommand))
                    Console.WriteLine("Неверный ввод. Введите целое число");
            } while (!isRight || (command > maxCommand || command < minCommand));
            return command;
        }

        static bool CheckEmpty(int[,] arr) // проверка на пустоту массива
        {
            bool isRight = false;
            if (arr.GetLength(1) == 0)
            {
                Console.WriteLine("В столбцах нет значений, создайте новый массив");
            }
            else
                isRight = true;
            return isRight;
        }

        static int[][] RandomArr2(int[][] arr, ref int maxLen, int minLen, int strings, Random rnd) //создание массива с помощью ДСЧ
        {
            for (int i = arr.GetLength(0) - strings; i < arr.GetLength(0); i++)
            {
                int columns = CheckSizeArr($"Введите количество столбцов {i + 1} строки от 1 до {maxLen - (arr.GetLength(0) - i - 1)}", maxLen - (arr.GetLength(0) - i - 1), minLen);
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
            int count = 0; // счётчик для подсказки пользователю
            for (int i = arr.GetLength(0) - strings; i < arr.GetLength(0); i++)
            {
                int columns = CheckSizeArr($"Введите количество столбцов {i + 1} строки от 1 до {maxLen - (arr.GetLength(0) - i - 1)}", maxLen - (arr.GetLength(0) - i - 1), minLen);
                arr[i] = new int[columns];
                for (int j = 0; j < columns; j++)
                {
                    arr[i][j] = CheckVar($"Введите {++count} элемент двумерного массива");
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

       static string CheckString(ref bool isFiveOrSix, string inputString = "") //проверка строки на правильность ввода и отсутствие пустой строки
        {    
            bool isCorrect;
            if (string.IsNullOrEmpty(inputString)) //если строка пустая и пользователь выбрал ввод с клавиатуры
            {
                Console.WriteLine("Введите не пустую строку предложений(не нажимайте клавишу Enter, пока не введёте строку до конца)" + SentenceRules());
                do
                {
                    inputString = Console.ReadLine();
                    inputString = inputString.Trim();
                    if (inputString.Length == 0 || !(CheckPattern(inputString)))
                    {
                        isCorrect = false;
                        Console.WriteLine("Неверный ввод. Введите не пустую строку предложений(не нажимайте клавишу Enter, пока не введёте строку до конца)");
                    }
                    else
                        isCorrect = true;
                } while (!isCorrect);
            }
            else
            {
                if (isFiveOrSix) //если строка только создаётся и не происходит работа со строкой
                {
                    inputString = inputString.Trim(); //работа с массивом
                    if (!(CheckPattern(inputString)))
                        Console.WriteLine("Массив составлен неверно. Введите новую строку или напишите другой массив, if ты разработчик или выберете другой массив, если вы пользователь\nДальнейшая работа с массивом невозможна)");
                }
            }
            string[] stringArr = inputString.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            inputString = string.Join(" ", stringArr);
            return inputString;
        }

        static string SentenceRules()
        {
            Console.WriteLine(@"Условия:
Предложение должно начинаться с заглавной буквы (желательно)!
Слова не должны начинаться с цифр!
После знаков препинания должен стоять пробел!
В качестве знаков окончания предложения - (.?!)
В качестве знаков препинания в предложении (не в конце) - (,:;)
Предложения оформляются по всем правилам письма!
Предложения строятся на русском или английском языке
После знаков препинания (,;:) должно идти слово
Перед знаками (.?!) не должно быть проблелов
Множество пробелов преобразовывается в один
При нескольких предложениях должен идти пробел после знака препинания");
            return "";
        }

        static bool CheckPattern(string inputString)
        {
            string pattern = @"((^([_|A-Za-zА-Яа-я])+\w*(\s+\w*|,(\s)+\w+|;\s+\w+|:\s+\w+)?([a-zA-Zа-яА-Я]+\w*(\s+\w+|,\s+\w+|;\s+\w+|:\s+\w+)?)*[.+|!+|?+])+)"; //шаблон вводимого предложения
            return Regex.IsMatch(inputString, pattern);
        }
    }
}
