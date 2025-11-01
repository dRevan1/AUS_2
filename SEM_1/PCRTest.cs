namespace SEM_1;

public class PCRTest : IComparable<PCRTest>
{
    public PCRTestData TestData { get; set; }


    public PCRTest(PCRTestData testData)
    {
        TestData = testData;
    }

    public override string ToString()
    {
        return TestData.ToString()! + "\n";
    }

    public int CompareTo(PCRTest? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (other.TestData.YearOfTest < TestData.YearOfTest || other.TestData.YearOfTest > TestData.YearOfTest) // podľa roku
        {
            return TestData.YearOfTest.CompareTo(other.TestData.YearOfTest);
        }

        if (other.TestData.MonthOfTest < TestData.MonthOfTest || other.TestData.MonthOfTest > TestData.MonthOfTest) // podľa mesiaca
        {
            return TestData.MonthOfTest.CompareTo(other.TestData.MonthOfTest);
        }

        if (other.TestData.DayOfTest < TestData.DayOfTest || other.TestData.DayOfTest > TestData.DayOfTest) // podľa dňa
        {
            return TestData.DayOfTest.CompareTo(other.TestData.DayOfTest);
        }

        if (other.TestData.HourOfTest < TestData.HourOfTest || other.TestData.HourOfTest > TestData.HourOfTest) // podľa hodiny
        {
            return TestData.HourOfTest.CompareTo(other.TestData.HourOfTest);
        }

        if (other.TestData.MinuteOfTest < TestData.MinuteOfTest || other.TestData.MinuteOfTest > TestData.MinuteOfTest) // podľa minúty
        {
            return TestData.MinuteOfTest.CompareTo(other.TestData.MinuteOfTest);
        }
        return TestData.TestID.CompareTo(other.TestData.TestID); // podľa ID
    }
}
