using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;

namespace DA_RTS
{
    public class Miner
    {
        private static Mutex _mutex = new Mutex();
        private static int goldInBank = 0;
        private static int minerCount = 0;
        private bool running = true;
        private Thread minerThread;

        public Miner()
        {
            minerThread = new Thread(MineGold);
            minerThread.Start();
        }


        private void MineGold()
        {
            while (running)
            {
                Thread.Sleep(2000);

                _mutex.WaitOne();
                try
                {
                    goldInBank += 10;
                    Console.WriteLine($"Miner {Thread.CurrentThread.ManagedThreadId} mined 10 gold. Total: {goldInBank}");

                    if (goldInBank >= 50)
                    {
                        goldInBank -= 50;
                        new Miner();
                        Console.WriteLine("New miner spawned!");
                    }
                }
                finally
                {
                    _mutex.ReleaseMutex();
                }
            }
        }

        public void Stop()
        {
            running = false;
            minerThread.Join();
        }
    }
}
