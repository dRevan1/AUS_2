namespace SEM_1.Core;

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

    public PCRTestData(byte minuteOfTest, byte hourOfTest, byte dayOfTest, byte monthOfTest, ushort yearOfTest,
        string personID, uint testID, uint testSiteID, uint regionID, uint districtID,
        bool isPositive, double testValue, string note)
    {
        MinuteOfTest = minuteOfTest;
        HourOfTest = hourOfTest;
        DayOfTest = dayOfTest;
        MonthOfTest = monthOfTest;
        YearOfTest = yearOfTest;
        PersonID = personID;
        TestID = testID;
        TestSiteID = testSiteID;
        RegionID = regionID;
        DistrictID = districtID;
        IsPositive = isPositive;
        TestValue = testValue;
        Note = note;
    }

    public override string ToString()
    {
        return $"{TestID};{YearOfTest};{MonthOfTest};{DayOfTest};{HourOfTest};{MinuteOfTest};{PersonID};{TestSiteID};{RegionID};{DistrictID};{IsPositive};{TestValue};{Note}";
    }
}
