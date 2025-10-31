﻿namespace SEM_1;

public class PCRTestByID : IComparable<PCRTestByID>
{
    public PCRTestData TestData { get; set; }
    public PCRTestByID(PCRTestData testData)
    {
        TestData = testData;
    }

    public int CompareTo(PCRTestByID? other)
    {
        if (other == null)
        {
            return 1;
        }
        return TestData.TestID.CompareTo(other.TestData.TestID);
    }
}
