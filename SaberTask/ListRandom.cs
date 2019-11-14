using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SaberTask
{
    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public ListRandom()
        {
            Count = 0;
        }

        public void addToHead(string newData)
        {
            if (Head == null && Tail == null)
            {
                Head = new ListNode(newData);
                Head.Random = Head;
                Tail = Head;
            }
            else
            {
                Head.Previous = new ListNode(newData);
                Head.Previous.Next = Head;
                Head = Head.Previous;
                Head.Random = GetRandom(Head);
            }
            Count++;
        }

        public void addToTail(string newData)
        {
            if (Head == null && Tail == null)
            {
                Tail = new ListNode(newData);
                Tail.Random = Tail;
                Head = Tail;
            }
            else
            {
                Tail.Next = new ListNode(newData);
                Tail.Next.Previous = Tail;
                Tail = Tail.Next;
                Tail.Random = GetRandom(Head);
            }
            Count++;
        }

        private ListNode GetRandom(ListNode node)
        {
            int i = 0;
            ListNode[] arr = new ListNode[Count];

            void ToUP(ListNode part)
            {
                if (part.Next == null) return;
                else { arr[i] = part; i++; ToUP(part.Next); }
            }

            ToUP(Head);
            Random rand = new Random((int)DateTime.Now.Ticks);

            return arr[rand.Next(0, arr.Length)];
        }

        public void Serialize(Stream s) // выходной поток   // Count + node1.size + node1.data +  node2.size + node2.data... ...+ node1.randomNum + node2.randomNum + node3.randomNum...
        {
            if (this.Head == null || this.Tail == null) return;
            Queue<byte[]> allList = new Queue<byte[]>();
            byte[] listPart;

            listPart = BitConverter.GetBytes(Count);        // первые 4 байта будет количество элементов списка
            if (BitConverter.IsLittleEndian) Array.Reverse(listPart);
            allList.Enqueue(listPart);


            ListNode[] arr = new ListNode[Count];
            ListNode buf = Head;
            int number = 0;
            while(true)
            {
                arr[number] = buf;
                if (buf.Next == null) break;
                number++;
                buf = buf.Next;
            } 

            byte[] sizeBuf;

            foreach (ListNode item in arr)
            {
                listPart = Encoding.ASCII.GetBytes(item.Data);
                if (BitConverter.IsLittleEndian) Array.Reverse(listPart);
                sizeBuf = BitConverter.GetBytes(listPart.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(listPart);
                allList.Enqueue(sizeBuf);
                allList.Enqueue(listPart);
            }

            for (int i = 0; i < arr.Length; i++)
            {
                for (int k = 0; k < arr.Length; k++)
                {
                    if (arr[i].Random == arr[k])
                    {
                        listPart = BitConverter.GetBytes(k);        // потом поочередно запоминаем на какой элемент ссылается очередной объект из списка
                        if (BitConverter.IsLittleEndian) Array.Reverse(listPart);
                        allList.Enqueue(listPart);
                        break;
                    }
                }
            }

            while (allList.Count > 0) s.Write(allList.Dequeue()); // записываем в поток
            Console.WriteLine("Serializ Done!");
        }

        public void Deserialize(Stream s) // входной поток
        {
            byte[] obj = new byte[4];
            s.Seek(0, SeekOrigin.Begin);
            s.Read(obj, 0, obj.Length);
            if (BitConverter.IsLittleEndian) Array.Reverse(obj);
            int elmnts = BitConverter.ToInt32(obj, 0);
            if (elmnts == 0) return;

            int dataSize;
            for (int i = 0; i < elmnts; i++)
            {
                obj = new byte[4];
                s.Read(obj, 0, obj.Length);
                //if (BitConverter.IsLittleEndian) Array.Reverse(obj);
                dataSize = BitConverter.ToInt32(obj, 0);
                obj = new byte[dataSize];
                s.Read(obj, 0, dataSize);
                //if (BitConverter.IsLittleEndian) Array.Reverse(obj);
                this.addToTail(Encoding.ASCII.GetString(obj));
            }

            int[] randNumb = new int[elmnts];
            obj = new byte[4];
            for (int i = 0; i < elmnts; i++)
            {
                s.Read(obj, 0, obj.Length);
                if (BitConverter.IsLittleEndian) Array.Reverse(obj);
                randNumb[i] = BitConverter.ToInt32(obj, 0);
            }

            ListNode mainSeek = Head;
            ListNode seek = Head;
            for (int i = 0; i < elmnts; i++)
            {
                for (int k = 0; k < randNumb[i]; k++) { seek = seek.Next;}
                mainSeek.Random = seek;
                mainSeek = mainSeek.Next;
                seek = Head;
            }
            Console.WriteLine("Deserialize Done!");
        }

    }
}
