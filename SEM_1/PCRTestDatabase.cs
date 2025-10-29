namespace SEM_1;

public class PCRTestDatabase
{
    public AVLTree<Person> People { get; set; } = new AVLTree<Person>();
    public AVLTree<Person> RegionDistrictIDs { get; set; } = new AVLTree<Person>();
    public AVLTree<uint> PCRTestIDs { get; set; } = new AVLTree<uint>();
    public AVLTree<Region> Regions { get; set; } = new AVLTree<Region>();
    public AVLTree<District> Districts { get; set; } = new AVLTree<District>();
    public AVLTree<TestSite> TestSites { get; set; } = new AVLTree<TestSite>();
    public AVLTree<PCRTest> PositiveTests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTest> AllTests { get; set; } = new AVLTree<PCRTest>();


    // #1
    public void AddPCRTest(PCRTestData testData)
    {
        Person? person = new Person { PersonID = testData.PersonID };
        person = People.Find(person);
        if (person == null)
        {
            return;
        }
        person.Tests.Insert(new PCRTest(testData));
    }

    // #2 - má byť výsledok testu
    public PCRTest? FindPCRTest(string personID, uint testID)
    {
        Person? person = new Person { PersonID = personID };
        person = People.Find(person);
        if (person == null)
        {
            return null;
        }
        PCRTestData testData = new PCRTestData { PersonID = personID, TestID = testID };
        PCRTest? test = new PCRTest(testData);

        if (test == null)
        {
            return null;
        }
        return test;
    }

    // #3
    public List<PCRTest> GetAllTestsByPerson(string personID)
    {
        Person? person = new Person { PersonID = personID };
        person = People.Find(person);
        if (person == null)
        {
            return new List<PCRTest>();
        }

        List<PCRTest> personTests = person.Tests.InOrderTraversal();
        List<PCRTest> testsCopy = personTests.Select(test => new PCRTest(test.TestData)).ToList(); // AI otázka na kopírovanie
        return testsCopy;
    }

    // #4
    public List<PCRTest>? GetAllPositiveTestsByDistrict(uint districtID, PCRTest min, PCRTest max)
    {
        District? district = new District { ID = districtID };
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }
        List<PCRTest> positiveTestsInRange = district.PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #5
    public List<PCRTest>? GetAllTestsByDistrict(uint districtID, PCRTest min, PCRTest max)
    {
        District? district = new District { ID = districtID };
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }
        List<PCRTest> testsInRange = district.AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #6
    public List<PCRTest>? GetPositiveTestsByRegion(uint regionID, PCRTest min, PCRTest max)
    {
        Region? region = new Region { ID = regionID };
        region = Regions.Find(region);
        if (region == null)
        {
            return null;
        }
        List<PCRTest> positiveTestsInRange = region.PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #7
    public List<PCRTest>? GetAllTestsByRegion(uint regionID, PCRTest min, PCRTest max)
    {
        Region? region = new Region { ID = regionID };
        region = Regions.Find(region);
        if (region == null)
        {
            return null;
        }
        List<PCRTest> testsInRange = region.AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #8
    public List<PCRTest> GetAllPositiveTests(PCRTest min, PCRTest max)
    {
        List<PCRTest> positiveTestsInRange = PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #9
    public List<PCRTest> GetAllTests(PCRTest min, PCRTest max)
    {
        List<PCRTest> testsInRange = AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #17
    public List<PCRTest>? GetAllTestsBySite(uint siteID, PCRTest min, PCRTest max)
    {
        TestSite? testSite = new TestSite { ID = siteID };
        testSite = TestSites.Find(testSite);
        if (testSite == null)
        {
            return null;
        }
        List<PCRTest> testsInRange = testSite.Tests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // #18
    public PCRTest? GetPCRTest(uint testID)
    {
        PCRTestData testData = new PCRTestData { TestID = testID, DayOfTest = 0 };
        PCRTest? test = new PCRTest(testData);
        test = AllTests.Find(test);
        if (test == null)
        {
            return null;
        }
        return new PCRTest(test.TestData);
    }

    // #19
    public void AddPerson(Person person)
    {
        People.Insert(person);
    }

    // #21
    public void DeletePerson(string personID)
    {
        Person person = new Person { PersonID = personID };
        People.Delete(person);
    }
}
