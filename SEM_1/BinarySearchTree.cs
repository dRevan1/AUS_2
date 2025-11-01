using System.Text;
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
            if (current != null && current.Data.CompareTo(node.Data) > 0)
            {
                return current;
            }
            else
            {
                while (current != null)
                {
                    if (current.Data.CompareTo(node.Data) > 0)
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
        return new BinarySearchTreeNode<T>(data);
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
        BinarySearchTreeNode<T>? replacement = null;
        if (node == Root) // ak je koreň
        {
            if (node.LeftChild == null && node.RightChild == null)
            {
                Root = null;
                return null;
            }
        }

        if (node.LeftChild != null && node.RightChild != null) // ak má 2 deti, nájdeme successora, ten bude mať max 1
        {
            BinarySearchTreeNode<T>? successor = FindSuccessor(node);
            node.Data = successor!.Data;
            node = successor;
            parent = node.Parent;
        }

        if (node.RightChild != null) // ak má vrchol 1 potomka, napr. keď je successor, ale nemusí byť, tak potom musí nahradiť vymazaný vrchol
        {
            replacement = node.RightChild;
        }
        else if (node.LeftChild != null)
        {
            replacement = node.LeftChild;
        }

        if (parent == null) // mazanie koreňa s 1 potomkom
        {
            Root = replacement;
            if (replacement != null)
            {
                replacement.Parent = null;
            }
            return Root;
        }

        if (parent.LeftChild == node) // vymazanie vrcholu, teda priradenie potenciálneho potomka mazaného vrchola (alebo null) na tú stranu parenta, kde sme mazali
        {
            parent.LeftChild = replacement;
        }
        else
        {
            parent.RightChild = replacement;
        }
        if (replacement != null) // nakoniec ak sme nahradzovali mazaný vrchol, tak treba prehodiť parenta toho náhradníka
        {
            replacement.Parent = parent;
            parent = replacement;
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

    public void BuildTreeFromLevelOrder(List<(T data, T parentData, char childPosition)> records)
    {
        if (records.Count == 0)
        {
            return;
        }
        Root = CreateNode(records[0].data);

        if (records.Count == 1)
        {
            return;
        }
        Queue<BinarySearchTreeNode<T>> queue = new Queue<BinarySearchTreeNode<T>>();
        BinarySearchTreeNode<T> parent = Root;

        for (int i = 1; i < records.Count; i++)
        {
            while (parent.Data.CompareTo(records[i].parentData) != 0)
            {
                parent = queue.Dequeue();
            }
            BinarySearchTreeNode<T> newNode = CreateNode(records[i].data);
            newNode.Parent = parent;

            if (records[i].childPosition == 'L')
            {
                parent.LeftChild = newNode;
            }
            else if (records[i].childPosition == 'R')
            {
                parent.RightChild = newNode;
            }

            queue.Enqueue(newNode);
        }
    }

    public List<string> LevelOrderTraversal()
    {
        if (Root == null)
        {
            return new List<string>();
        }
        Queue<BinarySearchTreeNode<T>> queue = new Queue<BinarySearchTreeNode<T>>();
        List<string> treeEntries = new List<string>();

        queue.Enqueue(Root);
        while (queue.Count > 0)
        {
            BinarySearchTreeNode<T> current = queue.Dequeue();
            treeEntries.Add(current.GetNodeString());
            if (current.LeftChild != null)
            {
                queue.Enqueue(current.LeftChild);
            }
            if (current.RightChild != null)
            {
                queue.Enqueue(current.RightChild);
            }
        }
        return treeEntries;
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