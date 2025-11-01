using System.Collections.Generic;

namespace SEM_1;

public class PCRTestDatabase
{
    public AVLTree<Person> People { get; set; } = new AVLTree<Person>();
    public AVLTree<RegionID> RegionDistrictIDs { get; set; } = new AVLTree<RegionID>();
    public AVLTree<uint> PCRTestIDs { get; set; } = new AVLTree<uint>();
    public AVLTree<Region> Regions { get; set; } = new AVLTree<Region>();
    public AVLTree<District> Districts { get; set; } = new AVLTree<District>();
    public AVLTree<TestSite> TestSites { get; set; } = new AVLTree<TestSite>();
    public AVLTree<PCRTest> PositiveTests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTest> AllTests { get; set; } = new AVLTree<PCRTest>();
    public AVLTree<PCRTestByID> AllTestsByID { get; set; } = new AVLTree<PCRTestByID>();


    private PCRTest GetMinForInterval(PCRTest max, int daysFromTest)
    {
        DateTime minDate = new DateTime(max.TestData.YearOfTest, max.TestData.MonthOfTest, max.TestData.DayOfTest);
        minDate = minDate.AddDays(-daysFromTest);
        PCRTest min = new PCRTest(new PCRTestData
        {
            TestID = 0,
            PersonID = "",
            YearOfTest = (ushort)minDate.Year,
            MonthOfTest = (byte)minDate.Month,
            DayOfTest = (byte)minDate.Day,
            HourOfTest = 0,
            MinuteOfTest = 0,
            IsPositive = true
        });
        return min;
    }

    private List<Person> GetSickPeopleList(List<PCRTest> tests)
    {
        List<Person> sickPeopleCopy = new List<Person>();

        foreach (PCRTest test in tests)
        {
            Person? person = new Person { PersonID = test.TestData.PersonID };
            person = People.Find(person);
            if (person == null)
            {
                continue;
            }
            sickPeopleCopy.Add(new Person
            {
                Name = person.Name,
                Surname = person.Surname,
                DayOfBirth = person.DayOfBirth,
                MonthOfBirth = person.MonthOfBirth,
                YearOfBirth = person.YearOfBirth,
                PersonID = person.PersonID
            });
        }
        return sickPeopleCopy;
    }

    private void InsertPCRTest(PCRTestData testData)
    {
        PCRTest newTest = new PCRTest(testData);
        PCRTestByID newTestByID = new PCRTestByID(testData);
        Person? person = People.Find(new Person { PersonID = testData.PersonID });
        Region? region = Regions.Find(new Region { ID = testData.RegionID });
        District? district = Districts.Find(new District { ID = testData.DistrictID });
        TestSite? testSite = TestSites.Find(new TestSite { ID = testData.TestSiteID });

        person!.Tests.Insert(newTest);  // pcr test do osoby a všeobecných zoznamov 
        person.TestsByID.Insert(newTestByID);
        AllTests.Insert(newTest);
        AllTestsByID.Insert(newTestByID);

        region!.AllTests.Insert(newTest); // do kraju, okresu a testovacieho miesta  
        district!.AllTests.Insert(newTest);
        testSite!.Tests.Insert(newTest);  

        if (testData.IsPositive) // ešte ak je pozitívny tak sa pridá do pozitívnych stromov
        {
            region.PositiveTests.Insert(newTest);
            district.PositiveTests.Insert(newTest);
            PositiveTests.Insert(newTest);
        }
    }

    private void DeletePCRTest(PCRTestByID testByID, bool personWasDeleted)
    {
        PCRTest test = new PCRTest(testByID.TestData);
        Person? person = People.Find(new Person { PersonID = testByID.TestData.PersonID });
        Region? region = Regions.Find(new Region { ID = testByID.TestData.RegionID });
        District? district = Districts.Find(new District { ID = testByID.TestData.DistrictID });
        TestSite? testSite = TestSites.Find(new TestSite { ID = testByID.TestData.TestSiteID });

        AllTests.Delete(test);  // mažeme vo všeobecných, potom v stromoch pre kraje, okresy, test miesta
        AllTestsByID.Delete(testByID);
        region!.AllTests.Delete(test);
        district!.AllTests.Delete(test);
        testSite!.Tests.Delete(test);

        if (testByID.TestData.IsPositive) // ak je pozitívny, musíme vymzať aj tu
        {
            region.PositiveTests.Delete(test);
            district.PositiveTests.Delete(test);
            PositiveTests.Delete(test);
        }

        if (!personWasDeleted) // ak sa delete volal z metódy, kde sa maže podľa ID, tak ešte vymažeme z osoby, inak sa osoba maže po vymazaní jej testov všade, takže tu už netreba mazať
        {
            person!.Tests.Delete(test);
            person.TestsByID.Delete(testByID);
        }
    }

    // # 1
    public void AddPCRTest(PCRTestData testData)
    {
        Person? person = new Person { PersonID = testData.PersonID };
        person = People.Find(person);
        if (person == null)
        {
            return;
        }
        RegionID? regionID = RegionDistrictIDs.Find(new RegionID(testData.RegionID));
        if (regionID == null)
        {
            return;
        }
        else
        {
            DistrictID? districtID = regionID.DistrictIDs.Find(new DistrictID(testData.DistrictID));
            if (districtID == null)
            {
                return;
            }
        }
        TestSite? testSite = TestSites.Find(new TestSite { ID = testData.TestSiteID });
        if (testSite == null)
        {
            return;
        }
        InsertPCRTest(testData);
    }

    // # 2 - má byť výsledok testu
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

    // # 3
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

    // # 4
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

    // # 5
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

    // # 6
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

    // # 7
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

    // # 8
    public List<PCRTest> GetAllPositiveTests(PCRTest min, PCRTest max)
    {
        List<PCRTest> positiveTestsInRange = PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 9
    public List<PCRTest> GetAllTests(PCRTest min, PCRTest max)
    {
        List<PCRTest> testsInRange = AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 10
    public List<Person>? GetSickPeopleByDistrict(uint districtID, PCRTest max, int daysFromTest)
    {
        District? district = new District { ID = districtID };
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }

        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = district.PositiveTests.IntervalSearch(min, max);

        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 11
    public List<Person>? GetSickPeopleByDistrictSorted(uint districtID, PCRTest max, int daysFromTest)
    {
        District? district = new District { ID = districtID };
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }

        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = district.PositiveTests.IntervalSearch(min, max);
        positiveTestsInRange = positiveTestsInRange.OrderBy(test => test.TestData.TestValue).ToList();

        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 12
    public List<Person>? GetSickPeopleByRegion(uint regionID, PCRTest max, int daysFromTest)
    {
        Region? region = new Region { ID = regionID };
        region = Regions.Find(region);
        if (region == null)
        {
            return null;
        }

        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = region.PositiveTests.IntervalSearch(min, max);

        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 13
    public List<Person> GetAllSickPeople(PCRTest max, int daysFromTest)
    {
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = PositiveTests.IntervalSearch(min, max);
        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 14
    public List<Person> GetSickPersonFromEveryDistrict(PCRTest max, int daysFromTest)
    {
        List<District> districts = Districts.InOrderTraversal();
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<Person> sickPeople = new List<Person>();

        foreach (District district in districts)
        {
            List<PCRTest> positiveTests = district.PositiveTests.IntervalSearch(min, max);
            PCRTest currentMax = positiveTests[0];
            uint currrentValue = (uint)currentMax.TestData.TestValue * 1_000_000;

            foreach (PCRTest test in positiveTests)
            {
                uint testValue = (uint)test.TestData.TestValue * 1_000_000;
                if (testValue > currrentValue)
                {
                    currrentValue = testValue;
                    currentMax = test;
                }
            }
            Person? person = People.Find(new Person { PersonID = currentMax.TestData.PersonID });

            if (person != null)
            {
                sickPeople.Add(new Person
                {
                    Name = person.Name,
                    Surname = person.Surname,
                    DayOfBirth = person.DayOfBirth,
                    MonthOfBirth = person.MonthOfBirth,
                    YearOfBirth = person.YearOfBirth,
                    PersonID = person.PersonID
                });
            }
        }

        return sickPeople;
    }

    // # 15
    public List<(District district, uint sickPeople)> GetDistrictsBySickPeople(PCRTest max, int daysFromTest)
    {
        List<District> districts = Districts.InOrderTraversal();
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<(District district, uint sickPeople)> districtAndPatientCount = new List<(District district, uint sickPeople)>();

        foreach (District district in districts)
        {
            List<PCRTest> positiveTests = district.PositiveTests.IntervalSearch(min, max);
            districtAndPatientCount.Add((district, (uint)positiveTests.Count));
        }

        districtAndPatientCount = districtAndPatientCount.OrderByDescending(district => district.sickPeople).ToList();
        return districtAndPatientCount;
    }

    // # 16
    public List<(Region region, uint sickPeople)> GetRegionbBySickPeople(PCRTest max, int daysFromTest)
    {
        List<Region> regions = Regions.InOrderTraversal();
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<(Region region, uint sickPeople)> regionAndPatientCount = new List<(Region region, uint sickPeople)>();

        foreach (Region region in regions)
        {
            List<PCRTest> positiveTests = region.PositiveTests.IntervalSearch(min, max);
            regionAndPatientCount.Add((region, (uint)positiveTests.Count));
        }

        regionAndPatientCount = regionAndPatientCount.OrderByDescending(region => region.sickPeople).ToList();
        return regionAndPatientCount;
    }

    // # 17
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

    // # 18
    public PCRTest? GetPCRTest(uint testID)
    {
        PCRTestData testData = new PCRTestData { TestID = testID };
        PCRTestByID? test = new PCRTestByID(testData);
        test = AllTestsByID.Find(test);
        if (test == null)
        {
            return null;
        }
        return new PCRTest(test.TestData);
    }

    // # 19
    public void AddPerson(Person person)
    {
        People.Insert(person);
    }

    // # 20
    public void DeletePCRTestByID(uint testID)
    {
        PCRTestByID? testToDelete = AllTestsByID.Find(new PCRTestByID(new PCRTestData { TestID = testID }));
        if (testToDelete == null)
        {
            return;
        }
        DeletePCRTest(testToDelete, false);
    }

    // # 21
    public void DeletePerson(string personID)
    {
        Person? person = People.Find(new Person { PersonID = personID });
        if (person == null)
        {
            return;
        }
        List<PCRTestByID> testsToDelete = person.TestsByID.InOrderTraversal();

        foreach (PCRTestByID test in testsToDelete)
        {
            DeletePCRTest(test, true);
        }
        People.Delete(person);
    }
}