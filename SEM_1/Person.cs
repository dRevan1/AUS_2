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

    public int CompareTo(Person? other)
    {
        if (other == null)         {
            return 1;
        }
        uint personID = uint.Parse(PersonID), otherPersonID = uint.Parse(other.PersonID);
        return personID.CompareTo(otherPersonID);
    }
}
