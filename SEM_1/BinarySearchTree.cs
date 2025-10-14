using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_1
{
   public class BinarySearchTree<T> where T : IComparable<T>
    {
       public BinarySearchTreeNode<T>? Root { get; set; }

        private void DeleteNode(BinarySearchTreeNode<T> node)
        {
            BinarySearchTreeNode<T>? parent = node.Parent;
            if (node.LeftChild == null && node.RightChild == null) // bez potomkov/synov
            {
                if (parent == null) // ak je koreň
                {
                    Root = null;
                    return;
                }
                if (parent.LeftChild == node)
                {
                    parent.LeftChild = null;
                }
                else
                {
                    parent.RightChild = null;
                }
            }
            else if (node.LeftChild != null && node.RightChild != null) // má oboch synov
            {
                BinarySearchTreeNode<T> successor = FindSuccessor(node)!;
                T deleteData = node.Data;
                node.Data = successor.Data;
                successor.Data = deleteData;
                DeleteNode(successor);
            }
            else
            {
                BinarySearchTreeNode<T> childToPromote = (node.LeftChild != null) ? node.LeftChild! : node.RightChild!;

                if (parent == null) // ak je koreň
                {
                    Root = childToPromote;
                    Root.Parent = null;
                    return;
                }


                if (parent.LeftChild == node)
                {
                    parent.LeftChild = childToPromote;
                }
                else
                {
                    parent.RightChild = childToPromote;
                }
                childToPromote.Parent = parent;
            }
        }

        private BinarySearchTreeNode<T>? TryToFindNode(T data)
        {
            if (Root == null)
            {
                return null;
            }
            BinarySearchTreeNode<T>? current = Root;
            BinarySearchTreeNode<T> parent = Root;

            while (current != null)
            {
                parent = current;
                if (current.Data.CompareTo(data) < 0)
                {
                    current = current.RightChild;
                }
                else if (current.Data.CompareTo(data) > 0)
                {
                    current = current.LeftChild;
                }
                else
                {
                    return current;
                }
            }
            return parent;
        }

        private BinarySearchTreeNode<T>? FindSuccessor(BinarySearchTreeNode<T> node)
        {
            if (node.RightChild == null)
            {
                return null;
            }
            BinarySearchTreeNode<T> current = node.RightChild!;
            while (current.LeftChild != null)
            {
                current = current.LeftChild;
            }
            return current;
        }

        public BinarySearchTreeNode<T>? Find(T data)
        {
            BinarySearchTreeNode<T>? node = TryToFindNode(data);
            if (node != null && node.Data.CompareTo(data) == 0)
            {
                return node;
            }

            Console.WriteLine("Value not found in the tree.");
            return null;
        }

        public void Insert(T data)
        {
            BinarySearchTreeNode<T>? parent = TryToFindNode(data);
            if (parent == null) // strom je prázdny - uložíme do koreňa
            {
                Root = new BinarySearchTreeNode<T> { Data = data };
                return;
            }
            if (parent.Data.CompareTo(data) == 0)
            {
                Console.WriteLine("Value is already in the tree.");
                return;
            }

            BinarySearchTreeNode<T> nodeToInsert = new BinarySearchTreeNode<T> { Data = data, Parent = parent };
            if (parent.Data.CompareTo(data) < 0)
            {
                parent.RightChild = nodeToInsert;
            }
            else
            {
                parent.LeftChild = nodeToInsert;
            }
        }

        public void Delete(T data)
        {
            BinarySearchTreeNode<T>? nodeToDelete = Find(data);
            if (nodeToDelete == null)
            {
                Console.WriteLine("Value not found in the tree.");
                return;
            }
            DeleteNode(nodeToDelete);
        }
    }
}
