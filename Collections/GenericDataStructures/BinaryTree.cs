using System;
using System.Text;

namespace CollectionsGeneric
{
    public class BinaryTree<T> where T : IComparable<T>
    {
        // Properties:
        public BinaryTreeNode<T> Root { get;private set; }

        // Ctor:
        public BinaryTree()
        {
            Root = null;
        }

        // Methods:
        public void Add(T item)
        {
            if (Root == null) // empty tree.
            {
                Root = new BinaryTreeNode<T>(item);
                return;
            }
            BinaryTreeNode<T> tmp = Root;
            BinaryTreeNode<T> parent = null;
            while (tmp != null)
            {
                parent = tmp;
                if (item.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }
            if (item.CompareTo(parent.data) < 0)
                parent.left = new BinaryTreeNode<T>(item);
            else
                parent.right = new BinaryTreeNode<T>(item);
        }

        public T Contain(T item)
        {
            if (Root == null) // empty tree.
                return default(T);
            BinaryTreeNode<T> tmp = Root;
            BinaryTreeNode<T> parent = null;
            while (tmp != null)
            {
                if (item.CompareTo(tmp.data) == 0)
                    return tmp.data;
                parent = tmp;
                if (item.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }
            return default(T);
        }

        public void Remove(T item) => Root = deleteRecursion(Root, item);
        private BinaryTreeNode<T> deleteRecursion(BinaryTreeNode<T> root, T item)
        {
            if (root == null) return root;
            if (item.CompareTo(root.data) < 0)
                root.left = deleteRecursion(root.left, item);
            else if (item.CompareTo(root.data) > 0)
                root.right = deleteRecursion(root.right, item);
            // if item is same as root's item then This is the node to be deleted 
            else
            {
                // node with only one child or no child  
                if (root.left == null)
                    return root.right;
                else if (root.right == null)
                    return root.left;
                // node with two children, Get the smallest in the right subtree
                root.data = minValue(root.right);
                root.right = deleteRecursion(root.right, root.data);
            }
            return root;
        }
        private T minValue(BinaryTreeNode<T> root)
        {
            T minv = root.data;
            while (root.left != null)
            {
                minv = root.left.data;
                root = root.left;
            }
            return minv;
        }

        public void FindBestMatch(T searchForBestX, out T bestNode)
        {
            BinaryTreeNode<T> node = new BinaryTreeNode<T>(default);
            try
            {
                BinaryTreeNode<T> tmp = Root;
                bestNode = FindBestMatch(tmp, searchForBestX).data;
            }
            catch(NullReferenceException exc)
            {
                throw exc;
            }
        }
        private BinaryTreeNode<T> FindBestMatch(BinaryTreeNode<T> root, T SearchedNode)
        {
            //If the tree empty return null
            if (root is null) return null;

            if (root.data.CompareTo(SearchedNode) < 0)
                return FindBestMatch(root.right, SearchedNode);
            else if (root.data.CompareTo(SearchedNode) >= 0)
            {
                BinaryTreeNode<T> returnVal = FindBestMatch(root.left, SearchedNode);

                //If return value = null then return root
                if (returnVal is null) return root;

                //Find Best match on RetrunVal
                return returnVal;
            }

            return null;
        }

        public void PrintInOrder()
        {
            BinaryTreeNode<T> tmp = Root;
            Recursion(tmp);
        }
        private void Recursion(BinaryTreeNode<T> tmp)
        {
            if (tmp == null) return;
            Recursion(tmp.left);
            Console.WriteLine(tmp.data);
            Recursion(tmp.right);
        }

        public string SaveStringInOrder()
        {
            StringBuilder sb = new StringBuilder(); ;
            BinaryTreeNode<T> tmp = Root;
            StringRecursion(tmp, ref sb);
            return sb.ToString();
        }
        private void StringRecursion(BinaryTreeNode<T> tmp , ref StringBuilder sb)
        {
            if (tmp == null) return;
            StringRecursion(tmp.left, ref sb);
            sb.AppendLine(tmp.data.ToString());
            StringRecursion(tmp.right, ref sb);
        }

        public int GetDepth()
        {
            BinaryTreeNode<T> tmp = Root;
            return GetDept1(tmp);
        }
        private int GetDept1(BinaryTreeNode<T> tmp)
        {
            if (tmp == null) return 0;
            int leftDepth = GetDept1(tmp.left);
            int rightDepth = GetDept1(tmp.right);
            return Math.Max(leftDepth, rightDepth) + 1;
            // shorter option:
            //if (tmp == null) return 0;
            //return Math.Max(GetDept1(tmp.left), GetDept1(tmp.right)) + 1;
        }

        public T MaxVal()
        {
            BinaryTreeNode<T> tmp = Root;
            return maxVal(tmp);
        }
        private T maxVal(BinaryTreeNode<T> root)
        {
            T maxv = root.data;
            while (root.right != null)
            {
                maxv = root.right.data;
                root = root.right;
            }
            return maxv;
        }
    }
}
