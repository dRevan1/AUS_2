namespace SEM_1;

public class AVLTree<T> : BinarySearchTree<T> where T : IComparable<T>
{

    private void RotateRight(AVLNode<T> pivot) // zaobaľuje pravú rotáciu, vyberá, či spraviť jednoduchú, alebo ľavo-pravú
    {
        AVLNode<T>? leftChild = pivot.LeftChild as AVLNode<T>;
        if (leftChild!.HeightLeft >= leftChild.HeightRight)
        {
            RotateSimpleRight(pivot);
            UpdateHeights(pivot); // aktualizácia výšok podstromov po rotácii, je upravený pivot vrchol a ten, ktorý ho nahradil
        }
        else
        {
            RotateLeftRight(leftChild!, pivot);
        }
    }

    private void RotateLeft(AVLNode<T> pivot) // zaobaľuje ľavú rotáciu, vyberá, či spraviť jednoduchú, alebo pravo-ľavú
    {
        AVLNode<T>? rightChild = pivot.RightChild as AVLNode<T>;
        if (rightChild!.HeightRight >= rightChild.HeightLeft)
        {
            RotateSimpleLeft(pivot);
            UpdateHeights(pivot);
        }
        else
        {
            RotateRightLeft(pivot, rightChild!);
        }
    }

    protected override BinarySearchTreeNode<T> CreateNode(T data) // vytvára vrchol, aby bol typu AVLNode
    {
        return new AVLNode<T> { Data = data, HeightLeft = 0, HeightRight = 0 }; 
    }

    protected void RotateLeftRight(AVLNode<T> leftPivot, AVLNode<T> rightPivot) // ľavo-pravá rotácia
    {
        RotateSimpleLeft(leftPivot);
        UpdateHeights(leftPivot);
        RotateSimpleRight(rightPivot);
        UpdateHeights(rightPivot);
    }

    protected void RotateRightLeft(AVLNode<T> leftPivot, AVLNode<T> rightPivot) // pravo-ľavá rotácia
    {
        RotateSimpleRight(rightPivot);
        UpdateHeights(rightPivot);
        RotateSimpleLeft(leftPivot);
        UpdateHeights(leftPivot);
    }

    private void UpdateHeights(AVLNode<T> pivot) // pivot je už posunutý dole vpravo/vľavo
    {
        AVLNode<T>? pivotParent = pivot.Parent as AVLNode<T>;
        if (pivotParent == null)
        {
            return;
        }
        AVLNode<T>? childNode;

        if (pivotParent.LeftChild == pivot) // ak sme rotovali do ľava
        {
            childNode = pivot.RightChild as AVLNode<T>;
            pivot.HeightRight = childNode != null ? Math.Max(childNode.HeightLeft, childNode.HeightRight) + 1 : 0;
            pivotParent.HeightLeft = Math.Max(pivot.HeightLeft, pivot.HeightRight) + 1;
        }
        else // ak sme rotovali do prava
        {
            childNode = pivot.LeftChild as AVLNode<T>;
            pivot.HeightLeft = childNode != null ? Math.Max(childNode.HeightLeft, childNode.HeightRight) + 1 : 0;
            pivotParent.HeightRight = Math.Max(pivot.HeightLeft, pivot.HeightRight) + 1;
        }
    }

    public override void Insert(T data)
    {
        AVLNode<T>? node = InsertNode(data) as AVLNode<T>; // insertnutý node
        if (node == null || node == Root)
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
            if (node == parentNode.LeftChild)
            {
                parentNode.HeightLeft = Math.Max(node.HeightLeft, node.HeightRight) + 1;
            }
            else
            {
                parentNode.HeightRight = Math.Max(node.HeightLeft, node.HeightRight) + 1;
            }

            int balanceFactor = parentNode.HeightLeft - parentNode.HeightRight;
            if (balanceFactor > 1) // ľavý podstrom je dlhší ako pravý
            {
                RotateRight(parentNode);
                break;
            }
            else if (balanceFactor < -1) // pravý podstrom je dlhší ako ľavý
            {
                RotateLeft(parentNode);
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

        AVLNode<T>? parentNode = DeleteNode(nodeToDelete) as AVLNode<T>; // parent vymazaaného vrcholu
        if (parentNode == null)
        {
            return;
        }
        AVLNode<T>? node = parentNode.LeftChild as AVLNode<T>; // úprava výšok otca
        parentNode.HeightLeft = node != null ? Math.Max(node.HeightLeft, node.HeightRight) + 1 : 0;
        node = parentNode.RightChild as AVLNode<T>;
        parentNode.HeightRight = node != null ? Math.Max(node.HeightLeft, node.HeightRight) + 1 : 0;

        if (parentNode.LeftChild != null ^ parentNode.RightChild != null) // XOR, znemaná, že sa vymazal jeden z 2 potomkov parenta
        {
            if (parentNode.HeightLeft == 1 ^ parentNode.HeightRight == 1) // ak ten druhý potomok, čo zostal, nemá potomka, tak sme nezmenili výšku v podstrome, iba pre parenta
            {
                return;
            }
        } // inak bude mať určite druhú výšku 2 alebo 0, teda musíme strom vyvážiť

        while (parentNode != null)
        {
            int balanceFactor = parentNode.HeightLeft - parentNode.HeightRight;
            if (balanceFactor > 1) // ľavý podstrom je dlhší ako pravý
            {
                RotateRight(parentNode);
            }
            else if (balanceFactor < -1) // pravý podstrom je dlhší ako ľavý
            {
                RotateLeft(parentNode);
            }

            if (balanceFactor > 1 || balanceFactor < -1)
            {
                node = parentNode.Parent as AVLNode<T>;
                parentNode = node!.Parent as AVLNode<T>;
            }
            else
            {
                node = parentNode;
                parentNode = parentNode.Parent as AVLNode<T>;
            }

            if (parentNode != null)
            {
                if (node == parentNode.LeftChild)
                {
                    parentNode.HeightLeft -= 1;
                }
                else
                {
                     parentNode.HeightRight -= 1;
                }
            }
        }
    }

    public void InOrderBalanceCheck() // testovacia metóda na skontrolovanie vyváženia stromu
    {
        AVLNode<T>? node = Root as AVLNode<T>;
        if (Root == null)
        {
            return;
        }
        Stack<BinarySearchTreeNode<T>> nodeStack = new();

        while (nodeStack.Count != 0 || node != null)
        {
            if (node != null)
            {
                nodeStack.Push(node);
                node = node.LeftChild as AVLNode<T>;
            }
            else
            {
                node = nodeStack.Pop() as AVLNode<T>;
                if (node.HeightLeft >= 0 && node.HeightRight >= 0)
                {
                    if ((node.HeightLeft - node.HeightRight > 1) || (node.HeightRight - node.HeightLeft > 1))
                    {
                        Console.WriteLine($"Unbalanced node: Left height = {node.HeightLeft}, Right height = {node.HeightRight}");
                        return;
                    }
                }
                node = node.RightChild as AVLNode<T>;
            }
        }
        Console.WriteLine("AVL tree is balanced.");
    }
}