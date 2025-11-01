using System.Text;

namespace SEM_1;

public class TestSite : IComparable<TestSite>
{
    public uint ID { get; set; }
    public AVLTree<PCRTest> Tests { get; set; } = new AVLTree<PCRTest>();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{ID}");
        List<string> tests = Tests.LevelOrderTraversal();
        sb.AppendLine($",{tests.Count}");

        foreach (string test in tests)
        {
            sb.Append(test);
        }

        return sb.ToString();
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
