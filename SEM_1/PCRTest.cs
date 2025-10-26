namespace SEM_1;

public class PCRTest : IComparable<PCRTest>
{
    public byte MinuteOfTest { get; set; }
    public byte HourOfTest { get; set; }
    public byte DayOfTest { get; set; }
    public byte MonthOfTest { get; set; }
    public ushort YearOfTest { get; set; }
    public string PersonID { get; set; }
    public int TestID { get; set; }
    public int TestSiteID { get; set; } // pracovisko
    public int RegionID { get; set; } // kraj
    public int DistrictID { get; set; } // okres
    public bool IsPositive { get; set; } // výsledok testu
    public double TestValue { get; set; } // hodnota testu
    public string Note { get; set; } // poznámka

    public int CompareTo(PCRTest? other)
    {
        return 0;
    }
}
