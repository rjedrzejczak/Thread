using System;
using System.Threading;

namespace TasksInConsole
{
    class Program
    {
        private static event Action EventFinished = () => { };

        static void Main(string[] args)
        {
            //
            // What is Asynchronous
            //
            // Asynchronous is if you start something, and don't wait while its happening. 
            // It literally means to not occur at the same time.
            //
            // This means not that our code returns early, but rather it doesn't sit there
            // blocking the code while it waits (doesn't block the thread)
            //

            //
            // Issues with Threads
            // 
            // Threads are asynchronous, as they naturally do something while the calling thread 
            // that made it doesn't wait for it.
            //

            #region Threads are asynchronous

            Console.WriteLine("Before first thread");

            new Thread(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Inside thread");
            }).Start();

            Console.WriteLine("After first thread");

            #endregion

            // What's the issue with Threads? 
            // 
            //  1. Expensive to make
            //  2. Not natural to be able to resume after a thread has finished to do something 
            //     related to the thread that created it
            // 
            // Issue 1 was solved with a ThreadPool. However issue 2 is still an issue for threads, 
            // and is one reason why Tasks were made.In order to resume work after some 
            // asynchronous operation has occurred we could with a Thread:
            // 
            //  1. Block your code waiting for it (no better than just doing it on same thread)
            //

            #region Blocking Thread

            Console.WriteLine("Before blocking thread");

            var blockingThread = new Thread(() =>
            {
                Thread.Sleep(500);
                Console.WriteLine("Inside blocking thread");
            });

            blockingThread.Start();

            //Block and wait
            blockingThread.Join();

            Console.WriteLine("After blocking thread");

            #endregion

            // 2. Constantly poll for completion, waiting for a bool flag to say done (inefficient, slow)

            #region Polling thread

            Console.WriteLine("Before polling thread");

            var poolComplete = false;

            var poolingThread = new Thread(() =>
            {
                Console.WriteLine("Inside polling thread");
                Thread.Sleep(500);

                //set pool complete
                poolComplete = true;
            });

            poolingThread.Start();

            // Poll for completion
            while (!poolComplete)
            {
                Thread.Sleep(10);
                Console.WriteLine("Polling...");
            }

            Console.WriteLine("After polling thread");

            #endregion

            // 3. Event-based callback
            #region Event-based Wait

            Console.WriteLine("Before event thread");

            var eventThread = new Thread(() =>
            {
                Console.WriteLine("Inside event thread");
                Thread.Sleep(500);

                // Fire completed event
                EventFinished();

            });

            // Hook into callback event
            EventFinished += () =>
            {
                //Called back from thread
                Console.WriteLine("Event thread callback on complete");
            };

            eventThread.Start();


            Console.WriteLine("After event thread");
            
            #endregion

            #region Event-based Wait Method
            
            Console.WriteLine("Before event method thread");
            // Call event callback style method
            EventThreadCallbackMethod(() =>
            {
                Console.WriteLine("Event thread callback on complete");
                
            });
            
            Console.WriteLine("After event method thread");
            
            // Wait for work to finish
            Thread.Sleep(1000);

            #endregion

            Console.ReadKey();
        }

        private static void EventThreadCallbackMethod(Action completed)
        {
            new Thread(() =>
            {
                Console.WriteLine("Inside event thread method");
                
                Thread.Sleep(500);

                // Fire completed event
                completed();

            }).Start();
        }
    }
}