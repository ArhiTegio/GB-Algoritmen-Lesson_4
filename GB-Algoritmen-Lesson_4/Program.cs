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
            { "1", new Barriers() },
            { "2", new Matrix() },
            { "3", new Horse() },
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
                    "3. * **Требуется обойти конём шахматную доску размером N x M, пройдя через все поля доски по одному разу.Здесь алгоритм решения такой же, как и в задаче о 8 ферзях.Разница только в проверке положения коня." + Environment.NewLine +
                    "0. Нажмите для выхода из программы.");

                n = q.Question<int>("Введите ", new HashSet<char>() { '0', '1', '2', '3', }, true);
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



    class Barriers : Act
    {
        Stack<Point> points;
        int[,] map = new int[,]
        {
            {  0, 0, 0, 0, 0, 0 },
            {  0, 0, 0, 0, 0, 0 },
            {  0,-1,-1,-1, 0, 0 },
            {  0,-1, 0, 0, 0, 0 },
            {  0,-1, 0, 0, 0, 0 },
        };
        List<Tuple<int, int>> directions;


        public override void Work()
        {
            points = new Stack<Point>();
            map[0, 0] = 1;
            points.Push(new Point(0, 0));
            directions = new List<Tuple<int, int>>() { Tuple.Create(1, 0), Tuple.Create(0, 1), };
            MapPath();

            for (int i = 0; i < map.GetLength(0); ++i)
            {
                for (int j = 0; j < map.GetLength(1); ++j)
                    Write($"{map[i, j]} ");
                Write($"{Environment.NewLine}");
            }
        }

        private void MapPath()
        {
            var point = new Point(0, 0);
            while (points.Count != 0)
            {
                point = points.Pop();
                var sum = 0;

                foreach (var e in directions)
                {
                    if (point.X - e.Item1 >= 0 &&
                        point.Y - e.Item2 >= 0)
                        sum += map[point.X - e.Item1, point.Y - e.Item2];

                    if (point.X + e.Item1 <= map.GetLength(0) - 1 &&
                        point.Y + e.Item2 <= map.GetLength(1) - 1 &&
                        map[point.X + e.Item1, point.Y + e.Item2] >= 0)
                        points.Push(new Point(point.X + e.Item1, point.Y + e.Item2));
                }
                if (sum > 0) map[point.X, point.Y] = sum;
            }
        }
    }

    class Matrix : Act
    {
        Stack<Point> stack;
        int[] sequence1;
        int[] sequence2;
        int[,] matrix;
        List<Point> list;
        int posY;

        public override void Work()
        {
            sequence1 = new int[] { 1, 5, 3, 2, 5, 6, 9, 7, };
            sequence2 = new int[] { 1, 5, 2, 4, 9, 7, 8 };
            matrix = new int[sequence1.Length + 1, sequence2.Length + 1];
            stack = new Stack<Point>();
            list = new List<Point>();
            stack.Push(new Point(1, 1));
            FoundInMatrix();
            FillingMatrix();

            for (int j = 0; j < sequence1.Length; ++j)
                Write($"{sequence1[j]} ");
            Write($"{Environment.NewLine}");

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)                
                    Write($"{matrix[i, j]} ");
                
                Write($"{Environment.NewLine}");
            }
        }

        private void FillingMatrix()
        {
            var x = 0;
            var y = 0;
            var e = new Point(0, 0);
            var oldPoint = new Point(0, 0);
            var st = new Queue<Point>();

            foreach (var f in list)
                st.Enqueue(f);

            while(st.Count != 0)
            {
                e = st.Dequeue();
                x = e.X;
                y = e.Y;


                while (true)
                {
                    x++;
                    if (x < matrix.GetLength(0))
                    {
                        if (matrix[x, y] != 0)
                            break;
                        else
                            matrix[x, y] = matrix[e.X, e.Y];
                    }
                    else
                        break;
                }

                x = e.X;
                y = e.Y;
                while (true)
                {
                    y++;
                    if (y < matrix.GetLength(1))
                    {
                        if (matrix[x, y] != 0)
                            break;
                        else
                            matrix[x, y] = matrix[e.X, e.Y];
                    }
                    else
                        break;
                }


                if (oldPoint.X + 1 != e.X)                
                    for (int x1 = oldPoint.X; x1 < e.X; ++x1)                    
                        st.Enqueue(new Point(x1, e.Y));
                                    
                if (oldPoint.Y + 1 != e.Y)
                    for (int y1 = oldPoint.Y; y1 < e.Y; ++y1)                    
                        st.Enqueue(new Point(e.X, y1));
                
                oldPoint = e;
            }
        }

        private void FoundInMatrix()
        {
            var newPoint = new Point(0, 0);
            while (stack.Count != 0)
            {
                newPoint = stack.Pop();

                if (newPoint.X < matrix.GetLength(0) && newPoint.Y < matrix.GetLength(1) &&
                    sequence1[newPoint.X - 1] == sequence2[newPoint.Y - 1])
                {
                    list.Add(new Point(newPoint.X, newPoint.Y));
                    posY = newPoint.Y;
                    if (newPoint.X < matrix.GetLength(0) && newPoint.Y < matrix.GetLength(1))
                    {
                        matrix[newPoint.X, newPoint.Y] = list.Count;
                        stack.Push(new Point(newPoint.X + 1, newPoint.Y + 1));
                    }
                    else
                        break;
                }
                else if (newPoint.Y < matrix.GetLength(1))
                {
                    stack.Push(new Point(newPoint.X, newPoint.Y + 1));
                }
                else if (newPoint.X < matrix.GetLength(0))
                {
                    stack.Push(new Point(newPoint.X + 1, posY));
                }
                else
                    break;
            }

        }

    }



    class Horse : Act
    {
        Point[,] field;
        Stack<Field> visitingPoints;
        HashSet<Point> hashPoint;
        int goal;


        public override void Work()
        {
            var sizeChessboard = 0;
            while (sizeChessboard < 5)
                sizeChessboard = int.Parse((new Questions()).Question<int>("Введите размер доски для шахмат (не менее 5):", new HashSet<char>() { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' }, true));
            GetField(sizeChessboard, sizeChessboard);
            try
            {
                FoundPathForHorse();

                var step = visitingPoints.Count;
                foreach (var e in visitingPoints)
                {
                    WriteLine($"Ход {step--} - {e.Point.X + 1}:{e.Point.Y + 1}");
                }
            }
            catch
            {
                WriteLine($"Нет решения!");
            }
        }

        private void GetField(int x, int y)
        {
            goal = x * y;
            var listStep = new List<Tuple<int, int>>
            {
                Tuple.Create(-1, -2), Tuple.Create(+1, -2), Tuple.Create(-1, +2), Tuple.Create(+1, +2),
                Tuple.Create(+2, -1), Tuple.Create(+2, +1), Tuple.Create(-2, -1), Tuple.Create(-2, +1),
            };
            field = new Point[x, y];
            for (int n = 0; n < x; ++n)
                for (int m = 0; m < y; ++m)                
                    field[n, m] = new Point(n, m);
            var n2 = 0;
            var m2 = 0;
            for (int n = 0; n < x; ++n)
                for (int m = 0; m < y; ++m)                
                    foreach(var e in listStep)
                    {
                        n2 = n + e.Item1;
                        m2 = m + e.Item2;
                        if (n2 <= x - 1 && n2 >= 0 &&  m2 <= y - 1 && m2 >= 0)
                            field[n, m].ListPoint.Add(field[n2, m2]);
                    }

            hashPoint = new HashSet<Point>();
            visitingPoints = new Stack<Field>();            
            visitingPoints.Push(new Field(field[0, 0], SortByInserts(field[0, 0].ListPoint.ToArray())));
            hashPoint.Add(field[0, 0]);
        }

        private void FoundPathForHorse()
        {
            var newPoint = new Point(0,0);
            while (goal != hashPoint.Count)
            {
                if (visitingPoints.Peek().List.Count == 0)
                {
                    hashPoint.Remove(visitingPoints.Peek().Point);
                    visitingPoints.Pop();
                }
                else
                {
                    newPoint = visitingPoints.Peek().List.Dequeue();
                    hashPoint.Add(newPoint);
                    visitingPoints.Push(new Field(newPoint, SortByInserts(newPoint.ListPoint.ToArray())));
                }
            }
        }



        public Queue<Point> SortByInserts(Point[] mass)
        {
            for (int i = 0; i < mass.Length; i++)
            {
                int temp = mass[i].ListPoint.Count;
                int j = i;
                while (j > 0 && mass[j - 1].ListPoint.Count > temp)
                {
                    TwoValuesExchange(mass, j, j - 1);
                    j--;
                }
            }
            var stack = new Queue<Point>();
            foreach (var e in mass)
            {
                if (!hashPoint.Contains(e))
                    stack.Enqueue(e);
            }
            return stack;
        }

        static void TwoValuesExchange<T>(T[] x, int i1, int i2) => (x[i1], x[i2]) = (x[i2], x[i1]);
    }

    class Field
    {
        Point point;
        Queue<Point> list = new Queue<Point>();

        public Field(Point point, Queue<Point> list)
        {
            this.Point = point;
            this.List = list;
        }

        internal Point Point { get => point; set => point = value; }
        internal Queue<Point> List { get => list; set => list = value; }
    }

    class Point
    {
        int x;
        int y;
        List<Point> listPoint = new List<Point>();

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public List<Point> ListPoint { get => listPoint; set => listPoint = value; }
    }
}
