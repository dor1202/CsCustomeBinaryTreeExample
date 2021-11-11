namespace CollectionsGeneric
{
    public class DoubleLinkedListNode<T>
    {
        public T data;
        public DoubleLinkedListNode<T> next;
        public DoubleLinkedListNode<T> prev;
        public DoubleLinkedListNode(T data)
        {
            this.data = data;
            next = null;
            prev = null;
        }
    }
}
