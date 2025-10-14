using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_1
{
    public class BinarySearchTreeNode<T> where T : IComparable<T>
    {
        public T Data { get; set; }
        public BinarySearchTreeNode<T>? Parent { get; set; }
        public BinarySearchTreeNode<T>? LeftChild { get; set; }
        public BinarySearchTreeNode<T>? RightChild { get; set; }

    }
}
