using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_1
{
   public class BinarySearchTree<T> where T : IComparable<T>
    {
       public BinarySearchTreeNode<T>? Root { get; set; }


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


        protected virtual BinarySearchTreeNode<T> CreateNode(T data)
        {
            return new BinarySearchTreeNode<T> { Data = data };
        }

        // Vráti node ak nájde (napr. pri insert a delete kontrolujeme, či existuje),
        // inak vráti rodiča, kde by mal byť node s hľadanými dátami (použitie pri insert)
        // Používa sa aj pri hľadaní hraníc pre intervalové vyhľadávanie, na rovnakom princípe
        // vráti vhodný vrchol, od ktorého sa dá určiť hranica (min/max)
        protected BinarySearchTreeNode<T>? TryToFindNode(T data) 
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

        protected BinarySearchTreeNode<T>? InsertNode(T data)
        {
            BinarySearchTreeNode<T>? parent = TryToFindNode(data);
            if (parent == null) // strom je prázdny - uložíme do koreňa
            {
                Root = CreateNode(data);
                return Root;
            }
            if (parent.Data.CompareTo(data) == 0)
            {
                Console.WriteLine("Value is already in the tree.");
                return null;
            }

            BinarySearchTreeNode<T> nodeToInsert = CreateNode(data);
            nodeToInsert.Parent = parent;
            if (parent.Data.CompareTo(data) < 0)
            {
                parent.RightChild = nodeToInsert;
            }
            else
            {
                parent.LeftChild = nodeToInsert;
            }
            return nodeToInsert;
        }

        protected BinarySearchTreeNode<T>? DeleteNode(BinarySearchTreeNode<T> node)
        {
            BinarySearchTreeNode<T>? parent = node.Parent;
            if (node.LeftChild == null && node.RightChild == null) // bez potomkov/synov
            {
                if (parent == null) // ak je koreň
                {
                    Root = null;
                    return null;
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
                node.Data = successor.Data;
                DeleteNode(successor);
            }
            else
            {
                BinarySearchTreeNode<T> childToPromote = (node.LeftChild != null) ? node.LeftChild! : node.RightChild!;

                if (parent == null) // ak je koreň
                {
                    Root = childToPromote;
                    Root.Parent = null;
                    return null;
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
            return parent;
        }


        public T? FindMin()
        {
            if (Root == null)
            {
                Console.WriteLine("The tree is empty.");
                return default;
            }
            BinarySearchTreeNode<T> current = Root;
            while (current.LeftChild != null)
            {
                current = current.LeftChild;
            }
            return current.Data;
        }

        public T? FindMax()
        {
            if (Root == null)
            {
                Console.WriteLine("The tree is empty.");
                return default;
            }
            BinarySearchTreeNode<T> current = Root;
            while (current.RightChild != null)
            {
                current = current.RightChild;
            }
            return current.Data;
        }

        public T? Find(T data)
        {
            BinarySearchTreeNode<T>? node = TryToFindNode(data);
            if (node != null && node.Data.CompareTo(data) == 0)
            {
                return node.Data;
            }

            Console.WriteLine("Value not found in the tree.");
            return default;
        }

        public virtual void Insert(T data)
        {
            BinarySearchTreeNode<T>? insertedNode = InsertNode(data);
            if (insertedNode == null)
            {
                return;
            }
        }

        public virtual void Delete(T data)
        {
            BinarySearchTreeNode<T>? nodeToDelete = TryToFindNode(data);
            if (nodeToDelete == null || nodeToDelete.Data.CompareTo(data) != 0)
            {
                Console.WriteLine("Value not found in the tree.");
                return;
            }
            DeleteNode(nodeToDelete);
        }

        public LinkedList<T> InOrderTraversal()
        {
            BinarySearchTreeNode<T>? node = Root;
            if (Root == null) { 
                return new LinkedList<T>(); 
            }
            Stack<BinarySearchTreeNode<T>> nodeStack = new();
            LinkedList<T> dataList = new LinkedList<T>();

            while (nodeStack.Count != 0 || node != null) // AI - dokumentácia
            {
                if (node != null)
                {
                    nodeStack.Push(node);
                    node = node.LeftChild;
                }
                else
                {
                    node = nodeStack.Pop();
                    dataList.AddLast(node.Data);
                    node = node.RightChild;
                }
            }
            return dataList;
        }

        public LinkedList<T> IntervalSearch(T min, T max)
        {
            BinarySearchTreeNode<T>? node = Root;
            LinkedList<T> dataList = new LinkedList<T>();
            if (Root == null || min.CompareTo(max) > 0) {
                return dataList; 
            }
            Stack<BinarySearchTreeNode<T>> nodeStack = new();

            while (nodeStack.Count != 0 || node != null)
            {
                if (node != null)
                {
                    if (node.Data.CompareTo(min) >= 0)
                    {
                        nodeStack.Push(node);
                        node = node.LeftChild;
                    }
                    else
                    {
                        node = node.RightChild;
                    }
                }
                else
                {
                    node = nodeStack.Pop();
                    if (node.Data.CompareTo(max) > 0)
                    {
                        break;
                    }
                    if (node.Data.CompareTo(min) >= 0)
                    {
                        dataList.AddLast(node.Data);
                    }
                    node = node.RightChild;
                }
            }
            return dataList;
        }
    }
}