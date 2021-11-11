using System;
using System.Text;
using CollectionsGeneric;

namespace DataModels
{
    public class Box : IComparable<Box>
    {
        public int Size { get; set; }
        public BinaryTree<BoxHightDetails> BinaryTreeHight { get; set; }
        public Box(int size)
        {
            Size = size;
            BinaryTreeHight = new BinaryTree<BoxHightDetails>();
        }

        public int CompareTo(Box other)
        {
            if (other != null)
                return this.Size.CompareTo(other.Size);
            else
                throw new ArgumentException("Object isn't DataX");
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Base side size: {Size}.");
            sb.AppendLine(BinaryTreeHight.SaveStringInOrder());
            return sb.ToString();
        }

    }
}
