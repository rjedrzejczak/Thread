using System;
using System.Dynamic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace Threads
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //Foreground
            new Thread(() =>
            {
                Console.WriteLine($"This is a new thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(0);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            }).Start();
            
            new Thread(() =>
            {
                Console.WriteLine($"This is a new thread {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(0);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            }).Start();
            
//            var thread = new Thread(()=> Console.WriteLine("blee"));
//            thread.IsBackground = true;
//            thread.Start();
            
            //Background
            new Thread(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("nah");
            }){IsBackground = true}.Start();

            Console.WriteLine("Hello World!");

            await Test(1);
            await Test(2);
            await Test(3);
            await Test(4);

        }

        public static async Task Test(int i)
        {
            await Task.Run(()=> Console.WriteLine($"Thread {i} {Thread.CurrentThread.ManagedThreadId}"));
        }
    }
}