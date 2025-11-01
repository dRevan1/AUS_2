using System.Text;

namespace SEM_1;

public class District : IComparable<District>
{
    public uint ID { get; set; }
    public AVLTree<PCRTest> PositiveTests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTest> AllTests { get; set; } = new AVLTree<PCRTest>();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{ID}");
        List<string> positiveTests = PositiveTests.LevelOrderTraversal();
        List<string> allTests = AllTests.LevelOrderTraversal();
        sb.AppendLine($",{positiveTests.Count},{allTests.Count}");

        foreach (string test in positiveTests)
        {
            sb.Append(test);
        }
        foreach (string test in allTests)
        {
            sb.Append(test);
        }

        return sb.ToString();
    }

    public int CompareTo(District? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
