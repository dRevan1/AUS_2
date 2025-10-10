using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_1
{
   public class BinarySearchTree<T> where T : IComparable<T>
    {
       public BinarySearchTreeNode<T> Root { get; set; }

        public void Find(T data)
        {
            BinarySearchTreeNode<T> current = Root;
            while (current != null) {
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
                    break;
                }
            }

           Console.WriteLine(current != null ? ("Found: " + current.Data) : "Not Found");
        }

        public void Insert(T data)
        {
            BinarySearchTreeNode<T> current = Root;
            if (Root == null)
            {
                Root = new BinarySearchTreeNode<T> { Data = data };
                return;
            }
            while (current != null)
            {
                if (current.Data.CompareTo(data) < 0)
                {
                    if (current.RightChild == null)
                    {
                        current.RightChild = new BinarySearchTreeNode<T> { Data = data, Parent = current };
                        return;
                    }
                    else
                    {
                        current = current.RightChild;
                    }
                }
                else if (current.Data.CompareTo(data) > 0)
                {
                    if (current.LeftChild == null)
                    {
                        current.LeftChild = new BinarySearchTreeNode<T> { Data = data, Parent = current };
                        return;
                    }
                    else
                    {
                        current = current.LeftChild;
                    }
                }
                else
                {
                    Console.WriteLine("Value is already in the tree.");
                    break;
                }
            }
        }
    }
}
