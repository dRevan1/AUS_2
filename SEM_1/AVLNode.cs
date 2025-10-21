namespace SEM_1;

public class AVLNode<T> : BinarySearchTreeNode<T> where T : IComparable<T>
{
    public int HeightLeft { get; set; }
    public int HeightRight { get; set; }

}
