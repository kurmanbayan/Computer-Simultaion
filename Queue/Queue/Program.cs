using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    class Program
    {
        static Queue<int> q = new Queue<int>();

        static List<string> arrival = new List<string>();
        static List<string> interArrival = new List<string>();
        static List<string> service = new List<string>();

        static DateTime startTime = DateTime.Now;
        const int n = 10, numOfOpInQueue = 40, numberOfTrFiles = 100;
        static int min = 0, sec = 0, mil = 0;
        static bool state = false;

        static void print()
        {
            Console.Write("P Num | ");
            Console.Write("Arr Time | ");
            Console.Write("IntArr Time | ");
            Console.WriteLine("Ser Time");
        }

        static void Main(string[] args)
        {
            print();

            for (int i = 1; i <= n; i++)
            {
                q.Enqueue(i * 10);
                generator(i * 10);
            }
            Console.ReadKey();
        }

        static void generator(int query)
        {
            if (!state)
            {
                state = true;
                Thread thread = new Thread(queue);
                thread.Start();
            }
            arrival.Add(String.Format("{0}.{1}.{2}", min, sec, mil));

            Thread.Sleep(Convert.ToInt32(query * 1000.0 / numberOfTrFiles));

            TimeSpan time = DateTime.Now - startTime;
            min = time.Minutes; sec = time.Seconds; mil = time.Milliseconds;
            interArrival.Add(String.Format("{0}.{1}.{2}", min, sec, mil));
        }

        static void queue()
        {
            int i = 0;
            while(q.Count != 0)
            {
                DateTime currentTime = DateTime.Now;
                int query = q.Dequeue();
                double delay = query * 1000.0 / numOfOpInQueue;
                Thread.Sleep(Convert.ToInt32(delay));
                TimeSpan time = DateTime.Now - currentTime;
                service.Add(String.Format("{0}.{1}.{2}", time.Minutes, time.Seconds, time.Milliseconds));

                if (i < n)
                {
                    Console.WriteLine("{0}       {1}       {2}       {3}",
                                i + 1, arrival[i], interArrival[i], service[i]);
                    i++;
                }
            }
        }
    }
}
