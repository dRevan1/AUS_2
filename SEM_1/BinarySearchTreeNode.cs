namespace SEM_1;

public class BinarySearchTreeNode<T> where T : IComparable<T>
{
    public T Data { get; set; }
    public BinarySearchTreeNode<T>? Parent { get; set; }
    public BinarySearchTreeNode<T>? LeftChild { get; set; }
    public BinarySearchTreeNode<T>? RightChild { get; set; }

    public BinarySearchTreeNode(T data)
    {
        Data = data;
        Parent = null;
        LeftChild = null;
        RightChild = null;
    }

    public virtual string GetNodeString()
    {
        string data = Data.ToString()!, childrenInfo;
        childrenInfo = LeftChild != null ? ",1" : ",0";
        childrenInfo += RightChild != null ? ",1" : ",0";

        data.Insert(data.IndexOf('\n'), childrenInfo);

        return data;
    }
}
