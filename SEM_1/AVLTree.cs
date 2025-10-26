namespace SEM_1;

public class AVLTree<T> : BinarySearchTree<T> where T : IComparable<T>
{

    private void RotateRight(AVLNode<T> pivot) // zaobaľuje pravú rotáciu, vyberá, či spraviť jednoduchú, alebo ľavo-pravú
    {
        AVLNode<T>? leftChild = pivot.LeftChild as AVLNode<T>;
        if (pivot.BalanceFactor == -2 && leftChild!.BalanceFactor <= 0)
        {
            RotateSimpleRight(pivot);
            if (leftChild!.BalanceFactor == 0)
            {
                pivot.BalanceFactor = -1;
                leftChild.BalanceFactor = 1;
            }
            else
            {
                pivot.BalanceFactor = 0;
                leftChild.BalanceFactor = 0;
            }     
        }
        else
        {
            RotateLeftRight(leftChild!, pivot);
        }
    }

    private void RotateLeft(AVLNode<T> pivot) // zaobaľuje ľavú rotáciu, vyberá, či spraviť jednoduchú, alebo pravo-ľavú
    {
        AVLNode<T>? rightChild = pivot.RightChild as AVLNode<T>;
        if (pivot.BalanceFactor == 2 && rightChild!.BalanceFactor >= 0)
        {
            RotateSimpleLeft(pivot);
            if (rightChild!.BalanceFactor == 0)
            {
                pivot.BalanceFactor = 1;
                rightChild.BalanceFactor = -1;
            }
            else
            {
                pivot.BalanceFactor = 0;
                rightChild.BalanceFactor = 0;
            }     
        }
        else
        {
            RotateRightLeft(pivot, rightChild!);
        }
    }

    protected override BinarySearchTreeNode<T> CreateNode(T data) // vytvára vrchol, aby bol typu AVLNode
    {
        return new AVLNode<T> { Data = data, BalanceFactor = 0 }; 
    }

    protected void RotateLeftRight(AVLNode<T> leftPivot, AVLNode<T> rightPivot) // ľavo-pravá rotácia - left pivot je ten, kde rotujeme doľava, a pravý naopak
    {
        AVLNode<T>? leftPivotChild = leftPivot.RightChild as AVLNode<T>;
        RotateSimpleLeft(leftPivot);
        leftPivot.BalanceFactor = (leftPivotChild!.BalanceFactor == 1) ? -1 : 0;
        leftPivotChild.BalanceFactor = -1;
        RotateRight(rightPivot);
    }

    protected void RotateRightLeft(AVLNode<T> leftPivot, AVLNode<T> rightPivot) // pravo-ľavá rotácia, platí to isté, ako LeftRight pre pivotov
    {
        AVLNode<T>? rightPivotChild = rightPivot.LeftChild as AVLNode<T>;
        RotateSimpleRight(rightPivot);
        rightPivot.BalanceFactor = (rightPivotChild!.BalanceFactor == -1) ? 1 : 0;
        rightPivotChild.BalanceFactor = 1;
        RotateLeft(leftPivot);
    }


    public override void Insert(T data)
    {
        AVLNode<T>? childNode = InsertNode(data) as AVLNode<T>; // insertnutý node
        if (childNode == null || childNode == Root)
        {
            return;
        }
        AVLNode<T>? node = childNode.Parent as AVLNode<T>;

        while (node != null)
        {
            if (childNode == node.LeftChild)
            {
                node.BalanceFactor -= 1; 
            }
            else
            {
                node.BalanceFactor += 1;
            }

            if (node.BalanceFactor == 0)
            {
                break;
            }
            if (node.BalanceFactor == -2) // ľavý podstrom je dlhší ako pravý
            {
                RotateRight(node);
                break;
            }
            else if (node.BalanceFactor == 2) // pravý podstrom je dlhší ako ľavý
            {
                RotateLeft(node);
                break;
            }
            childNode = node;
            node = node.Parent as AVLNode<T>;
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

        AVLNode<T>? node = DeleteNode(nodeToDelete) as AVLNode<T>; // parent vymazaaného vrcholu
        if (node == null)
        {
            return;
        }

        AVLNode<T>? childNode = null;
        if (node.RightChild == null && node.LeftChild == null)
        {
            node.BalanceFactor = 0;
            childNode = node;
            node = node.Parent as AVLNode<T>;
        }

        while (node != null)
        {
            if (childNode == node.LeftChild)
            {
                node.BalanceFactor += 1; 
            }
            else
            {
                node.BalanceFactor -= 1;
            }

            if (node.BalanceFactor == 1 || node.BalanceFactor == -1)
            {
                break;
            }
            if (node.BalanceFactor == -2) // ľavý podstrom je dlhší ako pravý
            {
                RotateRight(node);
                childNode = node.Parent as AVLNode<T>;
                node = childNode!.Parent as AVLNode<T>;
            }
            else if (node.BalanceFactor == 2) // pravý podstrom je dlhší ako ľavý
            {
                RotateLeft(node);
                childNode = node.Parent as AVLNode<T>;
                node = childNode!.Parent as AVLNode<T>;
            }
            else
            {
                childNode = node;
                node = node.Parent as AVLNode<T>;
            }
        }
    }

    public void InOrderBalanceCheck() // testovacia metóda na skontrolovanie vyváženia stromu
    {
        return;
    }
}