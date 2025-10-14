using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_1
{
    public class AVLTree<T> : BinarySearchTree<T> where T : IComparable<T>
    {

        protected override BinarySearchTreeNode<T> CreateNode(T data)
        {
            return new AVLNode<T> { Data = data };
        }

        public override BinarySearchTreeNode<T>? Insert(T data)
        {
           AVLNode<T> insertedNode = (AVLNode<T>)base.Insert(data)!;
           return insertedNode;
        }

        public override void Delete(T data)
        {
            base.Delete(data);
        }
    }
}
