namespace SEM_1;

public class Region : IComparable<Region>
{
    public uint ID { get; set; }
    public AVLTree<PCRTest> PositiveTests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTest> AllTests { get; set; } = new AVLTree<PCRTest>();

    public int CompareTo(Region? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
