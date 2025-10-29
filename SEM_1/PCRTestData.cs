namespace SEM_1;

public class PCRTestData
{
    public byte MinuteOfTest { get; set; }
    public byte HourOfTest { get; set; }
    public byte DayOfTest { get; set; }
    public byte MonthOfTest { get; set; }
    public ushort YearOfTest { get; set; }
    public string PersonID { get; set; }
    public uint TestID { get; set; }
    public uint TestSiteID { get; set; } // pracovisko
    public uint RegionID { get; set; } // kraj
    public uint DistrictID { get; set; } // okres
    public bool IsPositive { get; set; } // výsledok testu
    public double TestValue { get; set; } // hodnota testu
    public string Note { get; set; } // poznámka
}
