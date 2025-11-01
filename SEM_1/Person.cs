using System.Text;

namespace SEM_1;

public class Person : IComparable<Person>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public byte DayOfBirth { get; set; }
    public byte MonthOfBirth { get; set; }
    public ushort YearOfBirth { get; set; }
    public string PersonID { get; set; }
    public AVLTree<PCRTest> Tests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTestByID> TestsByID { get; set; } = new AVLTree<PCRTestByID>();

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{PersonID},{Name},{Surname},{DayOfBirth},{MonthOfBirth},{YearOfBirth}");
        List<string> testsList = Tests.LevelOrderTraversal();
        List<string> testsByIDList = TestsByID.LevelOrderTraversal();

        sb.AppendLine($",{testsList.Count},{testsByIDList.Count}");
        foreach (string test in testsList)
        {
            sb.Append(test);
        }
        foreach (string testByID in testsByIDList)
        {
            sb.Append(testByID);
        }

        return sb.ToString();
    }

    public int CompareTo(Person? other)
    {
        if (other == null)         {
            return 1;
        }
        uint personID = uint.Parse(PersonID), otherPersonID = uint.Parse(other.PersonID);
        return personID.CompareTo(otherPersonID);
    }
}
