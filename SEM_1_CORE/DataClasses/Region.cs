namespace SEM_1.Core;

public class Region : IComparable<Region>
{
    public uint ID { get; set; }
    public AVLTree<PCRTest> PositiveTests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTest> AllTests { get; set; } = new AVLTree<PCRTest>();

    public Region(uint id)
    {
        ID = id;
    }
    public override string ToString()
    {
        return $"{ID}\n";
    }

    public int CompareTo(Region? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
