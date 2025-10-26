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

    public int CompareTo(Person? other)
    {
        return 0;
    }
}
