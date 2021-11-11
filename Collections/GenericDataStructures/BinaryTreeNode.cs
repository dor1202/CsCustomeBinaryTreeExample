namespace CollectionsGeneric
{
    public class BinaryTreeNode<T>
    {
        public T data;
        public BinaryTreeNode<T> right;
        public BinaryTreeNode<T> left;
        public BinaryTreeNode(T data)
        {
            this.data = data;
            left = right = null;
        }
    }
}
