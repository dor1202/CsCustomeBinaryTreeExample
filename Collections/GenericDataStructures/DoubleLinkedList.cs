using System;
using System.Text;

namespace CollectionsGeneric
{
    public class DoubleLinkedList<T> where T : IComparable<T>
    {
        // Properties:
        public DoubleLinkedListNode<T> start { get;private set; }
        public DoubleLinkedListNode<T> end { get; set; }
        public DoubleLinkedListNode<T> prev { get; private set; }

        // Ctor:
        public DoubleLinkedList()
        {
            start = null;
            end = null;
            prev = null;
        }

        // Methods:
        public void AddFirst(T value)
        {
            DoubleLinkedListNode<T> n = new DoubleLinkedListNode<T>(value);
            n.next = start;
            if(n.next != null)
                n.next.prev = n;
            prev = start;
            start = n;
            if (end == null)
            {
                end = n;
                end.prev = prev;
            }
        }

        public bool RemoveLast()
        {
            if (end == null) return false;
            end = end.prev;
            end.next = null;
            if (end == null) start = null;
            return true;
        }

        public void RemoveNode(T item)
        {
            DoubleLinkedListNode<T> tmp = start;
            while (tmp != null)
            {
                if (tmp == null) return;
                if (tmp.data.CompareTo(item) == 0)
                {
                    if(tmp.next != null)
                        tmp.next.prev = tmp.prev;
                    if(tmp.prev != null)
                        tmp.prev.next = tmp.next;
                    if (tmp == end)
                        end = end.prev;
                    if (tmp == start)
                        start = start.next;
                    break;
                }
                tmp = tmp.next;
            }
        }

        public bool Contain(ref T item)
        {
            DoubleLinkedListNode<T> tmp = start;
            while (tmp != null)
            {
                if (tmp == null) return false;
                if (tmp.data.CompareTo(item) == 0) return true;
                tmp = tmp.next;
            }
            return false;
        }

        public void MoveToStart(T item)
        {
            DoubleLinkedListNode<T> tmp = start;
            while (tmp != null)
            {
                if (tmp.data.CompareTo(item) == 0) 
                {
                    if (tmp.prev == null) break; // first in the collection.
                    if (tmp != end)
                        tmp.next.prev = tmp.prev;
                    else // The last item.
                        end = end.prev;
                    if(tmp != start)
                        tmp.prev.next = tmp.next;
                    start.prev = tmp;
                    tmp.next = start;
                    start = tmp;
                    start.prev = null;
                }
                tmp = tmp.next;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            DoubleLinkedListNode<T> tmp = start;
            while (tmp != null)
            {
                sb.AppendLine($"{tmp.data.ToString()}");
                tmp = tmp.next;
            }
            return sb.ToString();
        }
    }
}
