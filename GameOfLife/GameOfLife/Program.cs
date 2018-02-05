using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife
{ 
    class Program
    {
        const int n = 20;

        static char[,] primary = new char[n + 1, n + 1];
        static char[,] secondary = new char[n + 1, n + 1];
        static bool[,] used = new bool[n + 1, n + 1];

        static int[] dx = new int[] {0, 0, -1, 1, -1, -1, 1, 1};
        static int[] dy = new int[] {-1, 1, 0, 0, -1, 1, -1, 1};

        static bool check(int x, int y)
        {
            if (x >= 1 && x <= n && y >= 1 && y <= n)
                return true;
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("You are given a field with a not alive cells");
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    Console.Write('.' + " ");
                    primary[i, j] = '.';
                    secondary[i, j] = '.';
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Enter number queries");
            int queries = Convert.ToInt32(Console.ReadLine());
            while(queries != 0)
            {
                Console.Write("Please enter x y: ");
                string input = Console.ReadLine();
                string[] splitted = input.Split(' ');
                int x = Convert.ToInt32(splitted[0]);
                int y = Convert.ToInt32(splitted[1]);
                if (check(x, y))
                {
                    if (!used[x, y])
                    {
                        primary[x, y] = 'o';
                        used[x, y] = true;
                        queries--;
                        print();
                    }
                    else
                    {
                        Console.WriteLine("This coordinates have already entered");
                    }
                }
                else
                {
                    Console.WriteLine("x or y value exceeds restricted range from 1 to {0}", n);
                }                
            }

            Console.Clear();
            print();
            while(true)  
            {
                Thread fill = new Thread(thread1);
                fill.Start();
                Thread delete = new Thread(thread2);
                delete.Start();
                Thread.Sleep(600);

                fillPoints();
                assign();
            }

            Console.ReadKey();
        }
                                                                                                                                                                                                 
        static void fillPoints()
        {
            for (int i = 1; i <= n; i++)
            { 
                for (int j = 1; j <= n; j++)
                {
                    if (primary[i, j] != secondary[i, j] || (primary[i, j] == secondary[i, j] && secondary[i, j] == 'o'))
                    {
                        Console.SetCursorPosition((j - 1) * 2, i - 1);
                        Console.Write(secondary[i, j]);   
                    }
                }
            }
        }

        static void thread1()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (primary[i, j] == '.')
                    {
                        checkCell(i, j, false);
                    }                                                                                                                                 
                }
            }
        }

        static void thread2()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (primary[i, j] == 'o')
                    {
                        checkCell(i, j, true);
                    }
                }
            }
        }

        static void assign()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    primary[i, j] = secondary[i, j];
                    secondary[i, j] = '.';
                }
            }
        }
        
        static void checkCell(int x, int y, bool alive) 
        {
            int cnt = 0;
            for (int i = 0; i < 8; i++)
            {
                int qx = x + dx[i], qy = y + dy[i];
                if (check(qx, qy) && primary[qx, qy] == 'o') 
                {
                    cnt++;
                }
            }
            secondary[x, y] = primary[x, y];
            if (alive && (cnt <= 1 || cnt >= 4))
            { 
                secondary[x, y] = '.';
            }
            if (!alive && cnt == 3)
            {
                secondary[x, y] = 'o';
            }
        }

        static void print()
        {
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    Console.Write(primary[i, j] + " ");
                }
                Console.WriteLine("");
            }
        }
    }
}
