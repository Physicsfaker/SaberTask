using System;
using System.IO;
using System.Text;

namespace SaberTask
{
    class Program
    {
        private static Random random = new Random((int)DateTime.Now.Ticks);

        static void Main(string[] args)
        {
            ListRandom listOut = new ListRandom();
            ListRandom listIn = new ListRandom();

            int listSize = random.Next(5, 5);

            for (int i = 0; i < listSize; i++)
            {
                if (random.Next(100) < 50) listOut.addToHead(RandomString());
                else listOut.addToTail(RandomString());
            }

            //listOut.addToHead("one - 1");
            //listOut.addToTail("two - 2");
            //listOut.addToTail("three - 3");

            using (MemoryStream str = new MemoryStream())
            {
                ShowObject(listOut);
                listOut.Serialize(str);

                listIn.Deserialize(str);
                ShowObject(listIn);

                Console.WriteLine("");
            }

            Console.ReadLine();
        }

        static private string RandomString()
        {
            int size = random.Next(1, 10);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
        static private void ShowObject(ListRandom l) 
        {
            Console.WriteLine($"\n************START************");
            Console.WriteLine($"\nCount of list elm-ts: {l.Count}\n");
            var x = l.Head;
            int count = 1;
            while (true)
            {
                Console.WriteLine($"list node {count}:\n   node.Data = {x.Data};\n   node.Random.Data = {x.Random.Data};\n"); 
                if (x.Next == null) break;
                x = x.Next;
                count++;
            }
            Console.WriteLine($"");
            Console.WriteLine($"*************END*************\n\n");
        }
    }
}
