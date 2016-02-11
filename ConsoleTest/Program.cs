using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace IterableDictionary
{
    class Program
    {
        static IterableDict<int, int> _dict;
        static int len = 10;
        static void Main(string[] args)
        {
            _dict = new IterableDict<int, int>();
            for (int i = 0; i < len; i++)
            {
                _dict.AddOrUpdate(i, i);
            }

            var t1 = Task.Run(() => Manipulate());
            var t2 = Task.Run(() => Read());

            //Task.WaitAll(t1, t2);
            Console.ReadLine();
        }

        static void Manipulate()
        {
            Task.Delay(510).Wait();
            try
            {
                for (int i = 0; i < len - 1; i++)
                {
                    Console.WriteLine($"Changing {i}");
                    _dict.AddOrUpdate(i, i * 10);
                    //Task.Delay(1).Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        static void Read()
        {
            var cursor = _dict.GetCursor();
            while (true)
            {
                if (!cursor.MoveNext())
                {
                    Console.WriteLine("no more");
                    Task.Delay(1000).Wait();
                    continue;
                }
                var item = cursor.GetCurrentNode();
                Console.WriteLine($"{item.Value}");
                Task.Delay(500).Wait();
            }
        }
    }
}