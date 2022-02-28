using System;
using System.Diagnostics;

namespace PrimeSieve
{
    /// <summary>
    /// Sieve of Eratosthenes
    /// </summary>
    public class PrimeSieve
    {
        UInt32[] m_primes;

        int m_maxPrimes;

        /// <summary>
        /// Removes a number for the sieve
        /// </summary>
        /// <param name="number"></param>
        void RemoveNumber(int number)
        {
            int byteNumber = number / 32;
            int bitNumber = number % 32;
            uint one = 1;
            m_primes[byteNumber] |= one << bitNumber;
        }

        /// <summary>
        /// Checks a bit in the sieve
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        bool CheckNumber(int number)
        {
            int byteNumber = number / 32;
            int bit = number % 32;
            return ((m_primes[byteNumber] >> bit) & 0x1) == 1;
        }

        public void Test()
        {
            // Create a bunch of bits
            m_primes = new UInt32[11];
            for (int i = 1; i < 10 * 32; i++)
            {
                RemoveNumber(i);
                Console.WriteLine($"{i} = {m_primes[0]:X}");
                if (!CheckNumber(i))
                {
                    Console.WriteLine($"Failed at bit {i}");
                }
            }
        }

        /// <summary>
        /// Process a number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="max"></param>
        void ProcessNumber(int number, int max)
        {
            if (CheckNumber(number))
            {
                // Console.WriteLine($"Skipping {number}");
                return;
            }

            // Get all multiples of number
            for (int i = 2; i <= max / number; i++)
            {
                // Console.WriteLine($"Multiplying {i} with {number}");
                var multiple = number * i;
                RemoveNumber(multiple);
            }
        }

        /// <summary>
        /// Run the sieve
        /// </summary>
        /// <param name="maxNumber"></param>
        public void RunSieve(int maxNumber)
        {
            Console.WriteLine($"Looking for primes below {maxNumber} with C#");
            m_maxPrimes = maxNumber;

            // Create a bunch of bits
            m_primes = new UInt32[maxNumber / 32 + 1];

            // Remove 1
            RemoveNumber(1);

            for (int i = 2; i < maxNumber; i++)
            {
                ProcessNumber(i, maxNumber);
            }

            // Task[] tasks = new Task[4];            
            // // Try to run it on 4 threads
            // for (int t = 0; t < 4; t++)
            // {
            //     tasks[t] = Task.Run(() =>
            //     {
            //         for (int i = 2 + t; i < maxNumber; i += 4)
            //         {
            //             ProcessNumber(i, maxNumber);
            //         }
            //     });
            // }
            //
            // for (int t = 0; t < 4; t++)
            // {
            //     Task.WaitAll(tasks);
            // }

            PrintPrimes();
        }

        /// <summary>
        /// Print the primes
        /// </summary>
        /// <returns>Number of primes found</returns>
        public int PrintPrimes()
        {
            int found = 0;
            for (int i = 1; i < m_maxPrimes; i++)
            {
                var bit = CheckNumber(i);
                if (!bit)
                {
                    found++;
                    // Console.WriteLine(i);
                }
            }

            return found;
        }
    }

    /// <summary>
    /// Main app class
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var sieve = new PrimeSieve();
            // sieve.Test();

            var sw = Stopwatch.StartNew();
            sieve.RunSieve(1_000_000_000);
            Console.WriteLine($"Processing took {sw.ElapsedMilliseconds}ms");
            Console.WriteLine($"Found {sieve.PrintPrimes()} prime numbers");
        }
    }
}