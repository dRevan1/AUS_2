namespace SEM_1;

public class BinarySearchTree<T> where T : IComparable<T>
{
    public BinarySearchTreeNode<T>? Root { get; set; }


    private BinarySearchTreeNode<T>? FindSuccessor(BinarySearchTreeNode<T> node)
    {
        BinarySearchTreeNode<T>? current = node.RightChild;

        if (current == null) // ak nie je pravý syn, tak ak existuje successor, bude to prvý rodič, ktorý má vyššiu hodnotu
        {
            current = node.Parent;
            if (current != null && current.Data.CompareTo(node.Data) == 1)
            {
                return current;
            }
            else
            {
                while (current != null)
                {
                    if (current.Data.CompareTo(node.Data) == 1)
                    {
                        break;
                    }
                    current = current.Parent;
                }
                return current;
            }
        }

        while (current.LeftChild != null)
        {
            current = current.LeftChild;
        }
        return current;
    }

    public BinarySearchTreeNode<T>? FindMinNode() // pre in order/interval
    {
        if (Root == null)
        {
            return null;
        }
        BinarySearchTreeNode<T> current = Root;
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

    protected BinarySearchTreeNode<T>? InsertNode(T data) // vráti potom vrchol, čo sa uložil, použijeme v AVL aby sme sa mohli posúvať smerom ku koreňu
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

    protected BinarySearchTreeNode<T>? DeleteNode(BinarySearchTreeNode<T> node) // vráti rodiča vymazaného vrcholu, použijeme v AVL aby sme sa mohli posúvať smerom ku koreňu
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
            parent = DeleteNode(successor);
        }
        else
        {
            BinarySearchTreeNode<T> childToPromote = (node.LeftChild != null) ? node.LeftChild! : node.RightChild!;

            if (parent == null) // ak je koreň
            {
                Root = childToPromote;
                Root.Parent = null;
                return Root;
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
            //Console.WriteLine("The tree is empty.");
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
            //Console.WriteLine("The tree is empty.");
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

        //Console.WriteLine("Value not found in the tree.");
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
            //Console.WriteLine("Value not found in the tree.");
            return;
        }
        DeleteNode(nodeToDelete);
    }

    public List<T> InOrderTraversal()
    {
        if (Root == null)
        {
            return new List<T>();
        }
        List<T> dataList = new List<T>();
        BinarySearchTreeNode<T>? node = FindMinNode();

        while (node != null)
        {
            dataList.Add(node.Data);
            node = FindSuccessor(node);
        }
        return dataList;
    }

    public List<T> IntervalSearch(T min, T max)
    {
        if (Root == null || min.CompareTo(max) > 0)
        {
            return new List<T>();
        }
        BinarySearchTreeNode<T>? node = TryToFindNode(min);
        List<T> dataList = new List<T>();

        while (node != null)
        {
            if (node.Data.CompareTo(max) == 1)
            {
                break;
            }
            dataList.Add(node.Data);
            node = FindSuccessor(node);
        }

        return dataList;
    }

    protected void RotateSimpleRight(BinarySearchTreeNode<T> pivot)
    {
        BinarySearchTreeNode<T>? pivotParent = pivot.Parent;
        BinarySearchTreeNode<T>? pivotLeftChild = pivot.LeftChild;
        if (pivotLeftChild == null)
        {
            return;
        }

        pivot.LeftChild = pivotLeftChild.RightChild;  // výmena pravý syn ľavého syna pivotu -> pivotov ľavý syn
        if (pivotLeftChild.RightChild != null)
        {
            pivotLeftChild.RightChild.Parent = pivot;
        }

        pivotLeftChild.Parent = pivotParent; // výmena rodiča pivotu -> rodič ľavého syna pivotu
        if (pivotParent == null)
        {
            Root = pivotLeftChild;
        }
        else if (pivotParent.LeftChild == pivot)
        {
            pivotParent.LeftChild = pivotLeftChild;
        }
        else
        {
            pivotParent.RightChild = pivotLeftChild;
        }

        pivotLeftChild.RightChild = pivot; // výmena pivot -> pravý syn ľavého syna pivotu
        pivot.Parent = pivotLeftChild;
    }

    protected void RotateSimpleLeft(BinarySearchTreeNode<T> pivot)
    {
        BinarySearchTreeNode<T>? pivotParent = pivot.Parent;
        BinarySearchTreeNode<T>? pivotRightChild = pivot.RightChild;
        if (pivotRightChild == null)
        {
            return;
        }

        pivot.RightChild = pivotRightChild.LeftChild;  // výmena ľavý syn pravého syna pivotu -> pivotov pravý syn
        if (pivotRightChild.LeftChild != null)
        {
            pivotRightChild.LeftChild.Parent = pivot;
        }

        pivotRightChild.Parent = pivotParent; // výmena rodiča pivotu -> rodič pravého syna pivotu
        if (pivotParent == null)
        {
            Root = pivotRightChild;
        }
        else if (pivotParent.LeftChild == pivot)
        {
            pivotParent.LeftChild = pivotRightChild;
        }
        else
        {
            pivotParent.RightChild = pivotRightChild;
        }

        pivotRightChild.LeftChild = pivot; // výmena pivot -> ľavý syn pravého syna pivotu
        pivot.Parent = pivotRightChild;
    }
}