namespace SEM_1;

public class TestSite : IComparable<TestSite>
{
    public uint ID { get; set; }
    public AVLTree<PCRTest> Tests { get; set; } = new AVLTree<PCRTest>();

    public int CompareTo(TestSite? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
