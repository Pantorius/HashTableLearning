using System.Diagnostics;
using HashSetLibrary;
namespace Console_for_test_and_constants_check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double mike = 0;
            double ivanAlt = 0;

            for (int p = 0; p < 10; p++)
            {
                
                HashSet<string> randDic = new HashSet<string>();
                Dictionary<string, int> MicrosoftDic = new Dictionary<string, int>();
                IvanHashTableAlternative<string, int> ivanDic = new IvanHashTableAlternative<string, int>();

                Random rnd = new Random();
                for (int i = 0; i < 1e6; i++)
                {
                    randDic.Add((i + rnd.Next()).ToString());
                }
                Stopwatch sw = new Stopwatch();

                List<string> wordsForTest = randDic.ToList();

                sw.Restart();
                foreach (string s in wordsForTest)
                {
                    MicrosoftDic.Add(s, s.Length);
                }
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
                mike += sw.Elapsed.TotalMilliseconds;

                sw.Restart();
                foreach (string s in wordsForTest)
                {
                    ivanDic.Add(s, s.Length);
                }
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
                ivanAlt += sw.Elapsed.TotalMilliseconds;
                
                Console.WriteLine(MicrosoftDic.Count == ivanDic.Count);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Mike {mike / 10} ms");
            Console.WriteLine($"ivanAlt {ivanAlt / 10} ms");
            Console.ForegroundColor = ConsoleColor.Gray;

            TakeWarAndPeaceElements();


            //FindingNumbers();
        }


        static void TakeWarAndPeaceElements()
        {
            string[] words;
            using (StreamReader sr = new StreamReader("War and peace.txt"))
            {
                words = sr.ReadToEnd().Split(new char[] {' ', ',','.' ,'!','?',':', ';', '-' }, StringSplitOptions.RemoveEmptyEntries);
            }

            Dictionary<string, int> microsoft = new Dictionary<string, int>();
            IvanHashTableAlternative<string, int> ivanHash = new IvanHashTableAlternative<string, int>();
            int outCount;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            foreach(string s in words)
            {
                if(microsoft.TryGetValue(s, out outCount))
                {
                    microsoft.Remove(s);
                    microsoft.Add(s, outCount+1);
                }
                else
                {
                    microsoft.Add(s, 1);
                }
            }
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} Microsoft add War");


            sw.Restart();
            foreach (string s in words)
            {
                if (ivanHash.TryGetValue(s, out outCount))
                {
                    ivanHash.Remove(s);
                    ivanHash.Add(s, outCount + 1);
                }
                else
                {
                    ivanHash.Add(s, 1);
                }
            }
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} Ivan add War");

            List<KeyValuePair<string, int>> l = ivanHash.GetArray().ToList();
    
            sw.Restart();
            foreach(KeyValuePair<string, int> pair in l)
            {
                if(pair.Value > 27)
                {
                    microsoft.Remove(pair.Key);
                }
            }
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} Microsoft rm words");

            sw.Restart();
            foreach (KeyValuePair<string, int> pair in l)
            {
                if (pair.Value > 27)
                {
                    ivanHash.Remove(pair.Key);
                }
            }
            sw.Stop();
            Console.WriteLine($"{sw.Elapsed} Ivan rm words");


            Console.WriteLine(microsoft.Count == ivanHash.Count);

            foreach(KeyValuePair<string, int> pair in microsoft)
            {
                if (!ivanHash.Contains(pair.Key))
                {
                    Console.WriteLine(false); //check for comparison
                }
            }
        }

        


        static void FindingNumbers()
        {
            (int, int) t;
            for (int i = 2; i < int.MaxValue / 2; i *= 2)
            {
                t = FindConstantsForNumber(i);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}, {1}, {2}", t.Item1, t.Item2, i);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        static (int, int) FindConstantsForNumber(long two)
        {
            int c2 = 25578;
            int c1 = 1361;
            while (!BijectionCheck(c1, c2, two) && c1 < int.MaxValue / 1000)
            {
                c2 += 2;
                if(c1 > two * 100000)
                {
                    c2 = 10;
                    c1 += 2;
                    Console.WriteLine($"{c1}");
                }
            }
            return (c1, c2);
        }

        static bool BijectionCheck(long c1, long c2, long two)
        {
            HashSet <long> set = new HashSet<long>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for(long i = 0; i < two; i++)
            {
                set.Add(((c1 * i) % two + c2 * (i * i) % two) % two);
            }
            sw.Stop();
            Console.WriteLine("{0} , {1}, {2}, c1 {3}, c2 {4}", set.Count, two, sw.Elapsed, c1, c2);
            return set.Count == two;
        }
    }
}
