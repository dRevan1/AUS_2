namespace SEM_1;

public class AVLNode<T> : BinarySearchTreeNode<T> where T : IComparable<T>
{
    public int BalanceFactor { get; set; }

    public AVLNode(T value, int balanceFactor) : base(value)
    {
        BalanceFactor = balanceFactor;
    }

    public override string GetNodeString()
    {
        string data = base.GetNodeString();  // zoberie info o data classe a otcovi, už sa len pridá balance factor
        data.Insert(data.IndexOf('\n'), $",{BalanceFactor}");

        return data;
    }
}
