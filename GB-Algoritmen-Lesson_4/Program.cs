using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryFastDecisions;
using static System.Console;
using System.Reflection;

namespace GB_Algoritmen_Lesson_4
{
    class Program
    {
        static Dictionary<string, Act> dict = new Dictionary<string, Act>
        {
            { "1", new Act },
        };

        static void Main(string[] args)
        {
            var ex = new Extension();
            var q = new Questions();
            var n = "";
            WriteLine("С# - Алгоритмы и структуры данных. Задание 4.");
            WriteLine("Кузнецов");
            while (n != "0")
            {
                WriteLine("Введите номер интересующей вас задачи:" + Environment.NewLine +
                    "1. *Количество маршрутов с препятствиями. Реализовать чтение массива с препятствием и нахождение количество маршрутов." + Environment.NewLine +
                    "   Карта для примера:" + Environment.NewLine +
                    "       3 3" + Environment.NewLine +
                    "       1 1 1" + Environment.NewLine +
                    "       0 1 0" + Environment.NewLine +
                    "       0 1 0" + Environment.NewLine +
                    "2.Решить задачу о нахождении длины максимальной подпоследовательности с помощью матрицы." + Environment.NewLine +
                    "3. * **Требуется обойти конём шахматную доску размером N × M, пройдя через все поля доски по одному разу.Здесь алгоритм решения такой же, как и в задаче о 8 ферзях.Разница только в проверке положения коня." + Environment.NewLine +
                    "0. Нажмите для выхода из программы.");

                n = q.Question<int>("Введите ", new HashSet<char>() { '0', '1', '2', '3', '4' }, true);
                if (n == "0") break;
                dict[n].Work();
            }

            Console.ReadKey();
        }
    }

    abstract class Act
    {
        public abstract void Work();
    }
}
