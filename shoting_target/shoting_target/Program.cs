using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Resources;
using System.Reflection;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace shoting_target
{
    class Program
    {
        //Библиотека для получения серийного номера
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern int GetVolumeInformation(string strPathName,
          StringBuilder strVolumeNameBuffer,
          int lngVolumeNameSize,
          out int lngVolumeSerialNumber,
          out int lngMaximumComponentLength,
          out int lngFileSystemFlags,
          StringBuilder strFileSystemNameBuffer,
          int lngFileSystemNameSize);

        static void Main(string[] args)
        {

            int serialNum, maxNameLen, flags;
            string pathSetup = System.IO.Directory.GetCurrentDirectory();
            string rootPath = pathSetup.Substring(0, pathSetup.IndexOf(":\\") + 2);
            StringBuilder a = new StringBuilder(), name = new StringBuilder();

            //Создаю ложные переменные
            string serl = "1285349534";

            GetVolumeInformation(rootPath, name, 100, out serialNum, out maxNameLen, out flags, a, 100);

            string serial = serialNum.ToString();

            string chkSerial = "";
            using (StreamReader fstream = new StreamReader($"{pathSetup}\\bin\\obj\\scrSrlFl.log"))
            {
                chkSerial += fstream.ReadToEnd();
            }
            using (StreamReader fstream = new StreamReader($"{pathSetup}\\bin\\dpScnd.ddl"))
            {
                chkSerial += fstream.ReadToEnd();
            }
            using (StreamReader fstream = new StreamReader($"{pathSetup}\\bin\\obj\\lgNmf.log"))
            {
                chkSerial += fstream.ReadToEnd();
            }

            string decodeSerialNum = "";
            //Дешифруем серийный номер
            for (int i = 0; i < chkSerial.Length / 2; i += 1)
            {
                decodeSerialNum += chkSerial[chkSerial.Length / 2 + i];
                decodeSerialNum += chkSerial[i];
            }
            
            //Добавляю лложные проверки
            if (serl == decodeSerialNum)
            {
                System.Console.WriteLine();
            }

            if (decodeSerialNum != serial)
            {
                Console.WriteLine("Нет прав!" );
               // System.Diagnostics.Process.GetCurrentProcess().Kill();
            }



            // Максимальное значение в пределах которого вычисляется координаты выстрела
            short MaxValue = 15;
            // Задержка при вычислении и отображении изменяющихся координат
            short Sleep = 30;
            // Шаг с которым идут круги на мишени
            short Step = 1;
            // Количество секций на мишени (остальное "молоко")
            short MaxScore = 10;

            double X, Y; // Значение выстрела по оси Х и Y
            int Score = 0, allScore = 0, v = 1; // Счёт за один выстрел, общий счёт
            string close; // Переменная для проверки выхода из игры

            Console.WriteLine("Изменить установки игры? (Y - да)");
            if (Console.ReadLine() == "Y")
            {
                do
                {
                    Console.WriteLine("Введите ширину всей мишени [1, 50] (15 по умолчанию):");
                    MaxValue = Convert.ToInt16(Console.ReadLine());

                    Console.WriteLine("Введите ширину одной секции [1, 10] (1 по умолчанию):");
                    Step = Convert.ToInt16(Console.ReadLine());

                    Console.WriteLine("Введите количество секций [1, 50] (10 по умолчанию):");
                    MaxScore = Convert.ToInt16(Console.ReadLine());

                    Console.WriteLine("Введите задержку [10, 300] (30 по умолчанию):");
                    Sleep = Convert.ToInt16(Console.ReadLine());

                } while (Proverka(MaxValue, Step, MaxScore, Sleep) == false);
            }

            do
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("**************************************");
                Console.WriteLine("Выстрел: " + v);
                Console.WriteLine("**************************************");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("Определение X ... Нажмите клавишу ...");
                X = Vistrel(MaxValue, Sleep);
                Console.WriteLine("X = " + X);
                Console.ReadKey(true);
                Console.ReadLine();
                Console.WriteLine("Определение Y ... Нажмите клавишу ...");
                Y = Vistrel(MaxValue, Sleep);
                Console.WriteLine("Y = " + Y);
                Console.ReadKey(true);

                Score = Schet(X, Y, MaxScore, Step, out Score);
                allScore += Score;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("**************************************");
                Console.WriteLine("Выстрел: " + Score + " Общий счёт: " + allScore);
                Console.WriteLine("**************************************");
                Console.ForegroundColor = ConsoleColor.White;
                v++;

                Console.WriteLine("Выйти? (Y - да) ");
                close = Console.ReadLine();
            } while (close != "Y");

        }

        // Проверка введённых данных на совместимость
        public static bool Proverka(short value, short shag, short score, short pause)
        {
            if ((value >= 1 && value <= 50) && (shag >= 1 && shag <= 10) && (score >= 1 && score <= 50) && (pause >= 10 && pause <= 300) && (value >= shag * score))
                return true;
            else
            {
                Console.WriteLine("Вы ввели неправильные значения, повторите ввод");
                Console.WriteLine();
                return false;
            }
        }

        // Выстрел
        public static double Vistrel(short value, short pause)
        {
            double num; // Числа, которые "бегают на экране"
            Random random = new Random();

            num = random.Next(-value, +value);
            while (!Console.KeyAvailable)
            {
                Console.Write(num);
                Console.CursorLeft = 0;
                num += random.NextDouble();

                if (num > value)
                    num = -value + (num - value);

                System.Threading.Thread.Sleep(pause);
            }
            return num;
        }


        // Подсчёт очков
        public static int Schet(double X, double Y, short MaxScore, short shag, out int Score)
        {
            Score = MaxScore;
            if (Math.Abs(X) > MaxScore || Math.Abs(Y) > MaxScore)
            {
                Score = 0;
            }
            else
            {
                while (Math.Abs(X) >= shag || Math.Abs(Y) >= shag)
                {
                    X = Math.Abs(X) - shag;
                    Y = Math.Abs(Y) - shag;
                    Score--;
                }
            }
            return Score;
        }
    }
}
