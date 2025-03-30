using System.Diagnostics;
using HashSetLibrary;
namespace Console_for_test_and_constants_check
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WarAndPeaceComparison();
            //TakeWarAndPeaceElements();
            //FindingNumbers();
        }


        void DefaultComparison()
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
                Stopwatch sw2 = new Stopwatch();
                List<string> wordsForTest = randDic.ToList();

                sw.Restart();
                for (int i = 0; i < wordsForTest.Count; ++i)
                {
                    MicrosoftDic.Add(wordsForTest[i], wordsForTest[i].Length);
                }
                sw.Stop();
                Console.WriteLine(sw.Elapsed);
                mike += sw.Elapsed.TotalMilliseconds;

                sw.Reset();

                sw.Start();
                for (int i = 0; i < wordsForTest.Count; ++i)
                {
                    ivanDic.Add(wordsForTest[i], wordsForTest[i].Length);
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
        }

        //static void TakeWarAndPeaceElements()
        //{
        //    string[] words;
        //    using (StreamReader sr = new StreamReader("War and peace.txt"))
        //    {
        //        words = sr.ReadToEnd().Split(new char[] {' ', ',','.' ,'!','?',':', ';', '-' }, StringSplitOptions.RemoveEmptyEntries);
        //    }

        //    Dictionary<string, int> microsoft = new Dictionary<string, int>();
        //    IvanHashTableAlternative<string, int> ivanHash = new IvanHashTableAlternative<string, int>();
        //    int outCount;
        //    Stopwatch sw = new Stopwatch();

        //    sw.Start();
        //    foreach(string s in words)
        //    {
        //        if(microsoft.TryGetValue(s, out outCount))
        //        {
        //            microsoft.Remove(s);
        //            microsoft.Add(s, outCount+1);
        //        }
        //        else
        //        {
        //            microsoft.Add(s, 1);
        //        }
        //    }
        //    sw.Stop();
        //    Console.WriteLine($"{sw.Elapsed} Microsoft add War");


        //    sw.Restart();
        //    foreach (string s in words)
        //    {
        //        if (ivanHash.TryGetValue(s, out outCount))
        //        {
        //            ivanHash.Remove(s);
        //            ivanHash.Add(s, outCount + 1);
        //        }
        //        else
        //        {
        //            ivanHash.Add(s, 1);
        //        }
        //    }
        //    sw.Stop();
        //    Console.WriteLine($"{sw.Elapsed} Ivan add War");

        //    List<KeyValuePair<string, int>> l = ivanHash.GetArray().ToList();

        //    sw.Restart();
        //    foreach(KeyValuePair<string, int> pair in l)
        //    {
        //        if(pair.Value > 27)
        //        {
        //            microsoft.Remove(pair.Key);
        //        }
        //    }
        //    sw.Stop();
        //    Console.WriteLine($"{sw.Elapsed} Microsoft rm words");

        //    sw.Restart();
        //    foreach (KeyValuePair<string, int> pair in l)
        //    {
        //        if (pair.Value > 27)
        //        {
        //            ivanHash.Remove(pair.Key);
        //        }
        //    }
        //    sw.Stop();
        //    Console.WriteLine($"{sw.Elapsed} Ivan rm words");


        //    Console.WriteLine(microsoft.Count == ivanHash.Count);

        //    foreach(KeyValuePair<string, int> pair in microsoft)
        //    {
        //        if (!ivanHash.Contains(pair.Key))
        //        {
        //            Console.WriteLine(false); //check for comparison
        //        }
        //    }
        //}

        static void TakeWarAndPeaceElements()
        {
            string[] words = GetWords();

            int testNumber = 10;
            int timeMS = 0, timeID = 0;
            for (int i = 0; i < testNumber; i++)
            {
                Stopwatch swMS = new Stopwatch();
                Stopwatch swID = new Stopwatch();
                Dictionary<string, int> microsoft = new Dictionary<string, int>();
                var ivanHash = new IvanHashTableAlternative<string, int>();
                int outCount;
                swMS.Start();
                foreach (string s in words)
                {
                    if (microsoft.ContainsKey(s))
                    {
                        microsoft[s] += 1;
                    }
                    else
                    {
                        microsoft.Add(s, 1);
                    }
                }
                swMS.Stop();
                timeMS += (int)swMS.ElapsedMilliseconds;



                swID.Start();
                foreach (string s in words)
                {
                    if (ivanHash.Contains(s))
                    {
                        ivanHash[s] += 1;
                    }
                    else
                    {
                        ivanHash.Add(s, 1);
                    }
                }
                swID.Stop();
                timeID += (int)swID.ElapsedMilliseconds;


                List<KeyValuePair<string, int>> l = ivanHash.GetArray().ToList();

                swMS.Restart();
                foreach (KeyValuePair<string, int> pair in l)
                {
                    if (pair.Value > 27)
                    {
                        microsoft.Remove(pair.Key);
                    }
                }
                swMS.Stop();
                timeMS += (int)swMS.ElapsedMilliseconds;

                swID.Restart();
                foreach (KeyValuePair<string, int> pair in l)
                {
                    if (pair.Value > 27)
                    {
                        ivanHash.Remove(pair.Key);
                    }
                }
                swID.Stop();
                timeID += (int)swID.ElapsedMilliseconds;
                Console.WriteLine(microsoft.Count == ivanHash.Count);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Mike {timeMS / testNumber} ms");
            Console.WriteLine($"ivanAlt {timeID / testNumber} ms");
            Console.ForegroundColor = ConsoleColor.Gray;

        }
        private static string[] GetWords()
        {
            string[] words;
            using (StreamReader sr = new StreamReader("voynaEdit.txt"))
            {
                words = sr.ReadToEnd().Split(new char[] { ' ', ',', '.', '!', '?', ':', ';', '-' }, StringSplitOptions.RemoveEmptyEntries);
            }

            return words;
        }


        static void WarAndPeaceComparison()
        {
            string[] all_world;
            using (StreamReader sr = new StreamReader("voynaEdit.txt"))
            {
                all_world = sr.ReadToEnd().Split(new char[] { ' ', '-', ';', '[', ']', '{', '}', '(', ')', '.', '!', '?', '\\', ',', '\r', '\n', '"', '«', '»' }, StringSplitOptions.RemoveEmptyEntries);
            }

            double ivanTime = 0;
            double microsoftTime = 0;
            int amountOfTests = 50;

            for (int i = 0; i < amountOfTests; i++)
            {
                Console.WriteLine("Моя таблица");
                Stopwatch all_time = new Stopwatch();   //класс таймер, позволяющий отсчитывать время от метода старт, до метода стоп
                Stopwatch process = new Stopwatch();
                all_time.Start();
                process.Start();
                IvanHashTableAlternative<string, int> table = new IvanHashTableAlternative<string, int>();
                foreach (string line in all_world)
                {

                    if (table.Contains(line))
                    {
                        table[line]++;
                    }
                    else
                    {
                        table.Add(line, 1);
                    }
                }
                process.Stop();
                all_time.Stop();
                Console.WriteLine($"Время на добавление элементов: {process.Elapsed}");
                List<KeyValuePair<string, int>> list = table.GetArray().ToList();
                process.Restart();
                all_time.Start();

                foreach (var line in list)
                {
                    if (line.Value > 27)
                    {
                        table.Remove(line.Key);
                    }
                }

                process.Stop();
                all_time.Stop();
                Console.WriteLine($"Время на нахождение и удаление элементов больше 27 раз встретившихся: {process.Elapsed}");
                Console.WriteLine($"Общее время: {all_time.Elapsed}");
                ivanTime += all_time.Elapsed.TotalMilliseconds;
                process.Reset();
                all_time.Reset();
                Console.WriteLine("");

                Dictionary<string, int> dic = new Dictionary<string, int>();

                Console.WriteLine("Словарь");

                all_time.Start();
                process.Start();

                foreach (string line in all_world)
                {

                    if (dic.ContainsKey(line))
                    {
                        dic[line]++;
                    }
                    else
                    {
                        dic.Add(line, 1);
                    }
                }
                process.Stop();
                all_time.Stop();
                Console.WriteLine($"Время на добавление элементов: {process.Elapsed}");

                process.Restart();
                all_time.Start();

                foreach (var line in dic)
                {
                    if (line.Value > 27)
                    {
                        dic.Remove(line.Key);
                    }
                }

                process.Stop();
                all_time.Stop();
                Console.WriteLine($"Время на нахождение и удаление элементов больше 27 раз встретившихся: {process.Elapsed}");
                Console.WriteLine($"Общее время: {all_time.Elapsed}");
                microsoftTime += all_time.Elapsed.TotalMilliseconds;
                Console.WriteLine($"Число элементов таблица : словарь = {table.Count} : {dic.Count}");
                Console.WriteLine();
            }

            Console.WriteLine("Общее среднее время выполнения таблица Иван {0} ms", ivanTime / amountOfTests);
            Console.WriteLine("Общее среднее время выполнение таблицы Майрософт {0} ms", microsoftTime / amountOfTests);
            Console.WriteLine($"В {ivanTime / microsoftTime} раз таблица Майкрософт быстрее.");
            Console.ReadKey();
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
                if (c1 > two * 100000)
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
            HashSet<long> set = new HashSet<long>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (long i = 0; i < two; i++)
            {
                set.Add(((c1 * i) % two + c2 * (i * i) % two) % two);
            }
            sw.Stop();
            Console.WriteLine("{0} , {1}, {2}, c1 {3}, c2 {4}", set.Count, two, sw.Elapsed, c1, c2);
            return set.Count == two;
        }
    }
}
