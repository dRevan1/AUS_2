using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_1
{
    public class AVLNode<T> : BinarySearchTreeNode<T> where T : IComparable<T>
    {
        public int HeightLeft { get; set; }
        public int HeightRight { get; set; }

    }
}
