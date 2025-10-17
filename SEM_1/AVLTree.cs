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

        public override void Insert(T data)
        {
            AVLNode<T>? insertedNode = InsertNode(data) as AVLNode<T>;
            if (insertedNode == null)
            {
                return;
            }
        }

        public override void Delete(T data)
        {
            AVLNode<T>? nodeToDelete = TryToFindNode(data) as AVLNode<T>;
            if (nodeToDelete == null || nodeToDelete.Data.CompareTo(data) != 0)
            {
                Console.WriteLine("Value not found in the tree.");
                return;
            }

            AVLNode<T>? deletedNodeParent = DeleteNode(nodeToDelete) as AVLNode<T>;
            if (deletedNodeParent == null)
            {
                return;
            }
        }
    }
}