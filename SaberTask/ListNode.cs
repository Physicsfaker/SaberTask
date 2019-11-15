using System;
using System.Collections.Generic;
using System.Text;

namespace SaberTask
{
    public class ListNode //как я понял его тоже нужно было реализовывать без сторонних библиотек
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка (ссылка на любой элемент в списке??)
        public string Data;
        public ListNode(string newData) 
        {
            Data = newData;
        }
    }
}
