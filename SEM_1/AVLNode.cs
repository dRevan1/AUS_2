namespace SEM_1;

public class AVLNode<T> : BinarySearchTreeNode<T> where T : IComparable<T>
{
    public int BalanceFactor { get; set; }

    public AVLNode(T value, int balanceFactor) : base(value)
    {
        BalanceFactor = balanceFactor;
    }
}
