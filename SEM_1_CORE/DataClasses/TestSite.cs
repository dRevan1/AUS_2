namespace SEM_1.Core;

public class TestSite : IComparable<TestSite>
{
    public uint ID { get; set; }
    public AVLTree<PCRTest> Tests { get; set; } = new AVLTree<PCRTest>();

    public TestSite(uint id)
    {
        ID = id;
    }

    public override string ToString()
    {
        return $"{ID}\n";
    }

    public int CompareTo(TestSite? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
