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
            return new AVLNode<T> { Data = data, HeightLeft = 0, HeightRight = 0 }; 
        }

        protected void RotateLeftRight(AVLNode<T> leftPivot, AVLNode<T> rightPivot)
        {
            // 
        }

        protected void RotateRightLeft(AVLNode<T> leftPivot, AVLNode<T> rightPivot)
        {
            // 
        }

        private void UpdateHeights(AVLNode<T> pivot) // pivot je už posunutý dole vpravo/vľavo
        {
            AVLNode<T>? childNode;
            if (pivot.Parent!.LeftChild == pivot)
            {
                childNode = pivot.RightChild as AVLNode<T>;
                pivot.HeightRight = childNode != null ? Math.Max(childNode.HeightLeft, childNode.HeightRight) + 1 : 0;
            }
            else
            {
                childNode = pivot.LeftChild as AVLNode<T>;
                pivot.HeightLeft = childNode != null ? Math.Max(childNode.HeightLeft, childNode.HeightRight) + 1 : 0;
            }
        }

        public override void Insert(T data)
        {
            AVLNode<T>? node = InsertNode(data) as AVLNode<T>; // insertnutý node
            if (node == null || Root == node)
            {
                return;
            }
            AVLNode<T>? parentNode = node.Parent as AVLNode<T>;

            if (parentNode!.LeftChild != null && parentNode.RightChild != null) // určite bude mať parenta, určite nebude root (preto ! v insertNode parent), keď je potomok aj na druhej strane
            {
                if (parentNode.LeftChild == node)
                {
                    parentNode.HeightLeft = 1;
                }
                else
                {
                    parentNode.HeightRight = 1;
                }
                return;
            }

            while (parentNode != null)
            {
                if (parentNode!.LeftChild == node)
                {
                    parentNode.HeightLeft += 1;
                }
                else
                {
                    parentNode.HeightRight += 1;
                }

                int balanceFactor = parentNode.HeightLeft - parentNode.HeightRight;
                if (balanceFactor > 1)
                {
                    AVLNode<T> leftChild = parentNode.LeftChild as AVLNode<T>;
                    if (leftChild!.HeightLeft >= leftChild.HeightRight)
                    {
                        RotateRight(parentNode);
                        UpdateHeights(parentNode);
                    }
                    else
                    {
                        RotateLeftRight(leftChild!, parentNode);
                    }

                    break;
                }
                else if (balanceFactor < -1)
                {
                    AVLNode<T> rightChild = parentNode.RightChild as AVLNode<T>;
                    if (rightChild != null && rightChild.HeightRight >= rightChild.HeightLeft)
                    {
                        RotateLeft(parentNode);
                    }
                    else
                    {
                        RotateRightLeft(parentNode, rightChild!);
                    }

                    break;
                }
                node = parentNode;
                parentNode = parentNode.Parent as AVLNode<T>;
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