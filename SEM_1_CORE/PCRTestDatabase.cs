using static System.Net.Mime.MediaTypeNames;

namespace SEM_1.Core;

public class PCRTestDatabase
{
    public AVLTree<Person> People { get; set; } = new AVLTree<Person>();
    public AVLTree<RegionID> RegionDistrictIDs { get; set; } = new AVLTree<RegionID>();
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
        PCRTest min = new PCRTest(new PCRTestData(0, 0, (byte)minDate.Day, (byte)minDate.Month, (ushort)minDate.Year, "", 0, 0, 0, 0, false, 0.0, ""));
        return min;
    }

    private List<Person> GetSickPeopleList(List<PCRTest> tests)
    {
        List<Person> sickPeopleCopy = new List<Person>();

        foreach (PCRTest test in tests)
        {
            Person? person = new Person(test.TestData.PersonID, "", "", 0, 0, 0);
            person = People.Find(person);
            if (person == null)
            {
                continue;
            }
            sickPeopleCopy.Add(new Person(person.PersonID, person.Name, person.Surname, person.DayOfBirth, person.MonthOfBirth, person.YearOfBirth));
        }
        return sickPeopleCopy;
    }

    private void InsertPCRTest(PCRTestData testData)
    {
        PCRTest newTest = new PCRTest(testData);
        PCRTestByID newTestByID = new PCRTestByID(testData);
        Person? person = People.Find(new Person (testData.PersonID, "", "", 0, 0, 0));
        Region? region = Regions.Find(new Region(testData.RegionID));
        District? district = Districts.Find(new District(testData.DistrictID));
        TestSite? testSite = TestSites.Find(new TestSite(testData.TestSiteID));

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
        Person? person = People.Find(new Person (testByID.TestData.PersonID, "", "", 0, 0, 0));
        Region? region = Regions.Find(new Region(testByID.TestData.RegionID));
        District? district = Districts.Find(new District(testByID.TestData.DistrictID));
        TestSite? testSite = TestSites.Find(new TestSite (testByID.TestData.TestSiteID));

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

    private void SaveListToFile(List<string> dataList, string filePath)
    {
        StreamWriter sw = new StreamWriter(filePath);
        foreach (string line in dataList)
        {
            sw.Write(line);
        }
        sw.Close();
    }

    private List<Person> LoadPeople(string[] lines)
    {
        List<Person> peopleList = new List<Person>();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] columns = line.Split(';');
            Person person = new Person(
                columns[0],
                columns[1],
                columns[2],
                byte.Parse(columns[3]),
                byte.Parse(columns[4]),
                ushort.Parse(columns[5])
            );
            peopleList.Add(person);
        }
        return peopleList;
    }

    private List<PCRTest> LoadPCRTests(string[] lines)
    {
        List<PCRTest> testsList = new List<PCRTest>();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] columns = line.Split(';');
            PCRTestData testData = new PCRTestData(
                byte.Parse(columns[5]),
                byte.Parse(columns[4]),
                byte.Parse(columns[3]),
                byte.Parse(columns[2]),
                ushort.Parse(columns[1]),
                columns[6],
                uint.Parse(columns[0]),
                uint.Parse(columns[7]),
                uint.Parse(columns[8]),
                uint.Parse(columns[9]),
                bool.Parse(columns[10]),
                double.Parse(columns[11]),
                columns[12]
            );
            testsList.Add(new PCRTest(testData));
        }

        return testsList;
    }

    public void PopulateDatabase(uint regionsCount, uint testSitesCount, uint peopleCount, uint testsCount)
    {
        List<uint> regionIDs = new List<uint>();
        List<uint> districtIDs = new List<uint>();
        List<uint> testSiteIDs = new List<uint>();
        List<uint> personIDs = new List<uint>();
        List<uint> testIDs = new List<uint>();

        for (uint i = 1; i <= regionsCount; i++)
        {
            Region region = new Region(i);
            RegionID regionID = new RegionID(i);
            Regions.Insert(region);
            RegionDistrictIDs.Insert(regionID);
            regionIDs.Add(region.ID);

            for (uint j = 1; j <= 5; j++)
            {
                District district = new District(j + ((i - 1) * 5));
                Districts.Insert(district);
                regionID.DistrictIDs.Insert(new DistrictID(district.ID));
                districtIDs.Add(district.ID);
            }
        }

        for (uint i = 1; i <= testSitesCount; i++)
        {
            TestSite testSite = new TestSite(i);
            TestSites.Insert(testSite);
            testSiteIDs.Add(testSite.ID);
        }
        Random rand = new Random();

        for (uint i = 1; i <= peopleCount; i++)
        {
            string personID = i.ToString();
            Person person = new Person(personID, $"Name{i}", $"Surname{i}", (byte)rand.Next(1, 29), (byte)rand.Next(1, 13), (ushort)rand.Next(1945, 2021));
            People.Insert(person);
            personIDs.Add(i);
        }

        for (uint i = 1; i <= testsCount; i++)
        {
            string testID = i.ToString();
            uint regionID = regionIDs[rand.Next(regionIDs.Count)];
            uint testSiteID = testSiteIDs[rand.Next(testSiteIDs.Count)];
            string personID = personIDs[rand.Next(personIDs.Count)].ToString();
            RegionID region = RegionDistrictIDs.Find(new RegionID(regionID))!;
            List<DistrictID> listOfDistrictIDs = region.DistrictIDs.InOrderTraversal();
            uint districtID = listOfDistrictIDs[rand.Next(listOfDistrictIDs.Count)].ID;

            PCRTestData testData = new PCRTestData(
                (byte)rand.Next(0, 60),
                (byte)rand.Next(0, 24),
                (byte)rand.Next(1, 29),
                (byte)rand.Next(1, 13),
                (ushort)rand.Next(2023, 2025),
                personID,
                i,
                testSiteID,
                regionID,
                districtID,
                false,
                Math.Round(rand.NextDouble() * 100, 2),
                $"Note for test {testID}"
            );

            AddPCRTest(testData);
        }
    }

    public void ExportDatabase()
    {
        List<string> people = People.LevelOrderTraversal();
        List<string> regionDistrictIDs = RegionDistrictIDs.LevelOrderTraversal();
        List<string> regions = Regions.LevelOrderTraversal();
        List<string> districts = Districts.LevelOrderTraversal();
        List<string> testSites = TestSites.LevelOrderTraversal();
        List<string> allTests = AllTests.LevelOrderTraversal();

        string pathPeople = "people_export2.csv", pathRegionDistrictIDs = "region_district_ids_export2.csv", pathRegions = "regions_export2.csv",
               pathDistricts = "districts_export2.csv", pathTestSites = "test_sites_export2.csv", pathAllTests = "all_tests_export2.csv";
        
        SaveListToFile(people, pathPeople);
        SaveListToFile(regionDistrictIDs, pathRegionDistrictIDs);
        SaveListToFile(regions, pathRegions);
        SaveListToFile(districts, pathDistricts);
        SaveListToFile(testSites, pathTestSites);
        SaveListToFile(allTests, pathAllTests);
    }

    public void ImportDatabase()
    {
        string[] lines;

        lines = File.ReadAllLines("people_export2.csv");
        List<Person> people = LoadPeople(lines);
        People.BuildTreeFromLevelOrder(people);

        lines = File.ReadAllLines("regions_export2.csv");
        List<Region> regions = new List<Region>();
        foreach (string line in lines)
        {
            uint regionID = uint.Parse(line);
            Region region = new Region(regionID);
            regions.Add(region);
        }
        Regions.BuildTreeFromLevelOrder(regions);

        lines = File.ReadAllLines("districts_export2.csv");
        List<District> districts = new List<District>();
        foreach (string line in lines)
        {
            uint districtID = uint.Parse(line);
            District district = new District(districtID);
            districts.Add(district);
        }
        Districts.BuildTreeFromLevelOrder(districts);

        lines = File.ReadAllLines("test_sites_export2.csv");
        List<TestSite> testSites = new List<TestSite>();
        foreach (string line in lines)
        {
            uint testSiteID = uint.Parse(line);
            TestSite testSite = new TestSite(testSiteID);
            testSites.Add(testSite);
        }
        TestSites.BuildTreeFromLevelOrder(testSites);

        lines = File.ReadAllLines("region_district_ids_export2.csv");
        List<RegionID> regionDistrictIDs = new List<RegionID>();
        int i = 0;
        while (i < lines.Length)
        {
            string line = lines[i];
            string[] columns = line.Split(';');
            uint regionID = uint.Parse(columns[0]);
            RegionID regionDistrictID = new RegionID(regionID);
            uint districtsCount = uint.Parse(columns[1]);
            List<DistrictID> districtIDs = new List<DistrictID>();

            for (int j = 0; j < districtsCount; j++)
            {
                i++;
                line = lines[i];
                columns = line.Split(';');
                uint districtID = uint.Parse(columns[0]);
                districtIDs.Add(new DistrictID(districtID));
            }
            regionDistrictID.DistrictIDs.BuildTreeFromLevelOrder(districtIDs);
            regionDistrictIDs.Add(regionDistrictID);
            i++;
        }
        RegionDistrictIDs.BuildTreeFromLevelOrder(regionDistrictIDs);

        lines = File.ReadAllLines("all_tests_export2.csv");
        List<PCRTest> tests = LoadPCRTests(lines);
        foreach (PCRTest test in tests)
        {
            AddPCRTest(test.TestData);
        }
    }

    public Person? FindPerson(string personID)
    {
        Person? person = new Person(personID, "", "", 0, 0, 0);
        person = People.Find(person);

        if (person == null)
        {
            return null;
        }

        return person;
    }

    // # 1
    public string AddPCRTest(PCRTestData testData)
    {
        Person? person = new Person (testData.PersonID, "", "", 0, 0, 0);
        person = People.Find(person);
        if (person == null)
        {
            return "Person with this ID doesn't exist!";
        }
        RegionID? regionID = RegionDistrictIDs.Find(new RegionID(testData.RegionID));
        if (regionID == null)
        {
            return "This region doesn't exist!";
        }
        else
        {
            DistrictID? districtID = regionID.DistrictIDs.Find(new DistrictID(testData.DistrictID));
            if (districtID == null)
            {
                return "Region doesn't contain such district!";
            }
        }
        TestSite? testSite = TestSites.Find(new TestSite(testData.TestSiteID));
        if (testSite == null)
        {
            return "This test site doesn't exist";
        }

        if (testData.TestValue > 50.0)
        {
            testData.IsPositive = true;
        }

        Random rand = new Random();
        if (testData.TestID == 0)
        {
            testData.TestID = (uint)rand.Next(200_000_000);
        }
        PCRTestByID? testByID = new PCRTestByID(testData);

        while (AllTestsByID.Find(testByID) != null)
        {
            testData.TestID = (uint)rand.Next(200_000_000);
        }

        InsertPCRTest(testData);
        return "";
    }

    // # 2 - má byť výsledok testu
    public PCRTestByID? FindPCRTest(string personID, uint testID)
    {
        Person? person = FindPerson(personID);
        if (person == null)
        {
            return null;
        }
        PCRTestData testData = new PCRTestData(0, 0, 0, 0, 0, personID, testID, 0, 0, 0, false, 0.0, "");
        PCRTestByID? test = new PCRTestByID(testData);
        test = person.TestsByID.Find(test);
        if (test == null)
        {
            return null;
        }
        return test;
    }

    // # 3
    public List<PCRTest> GetAllTestsByPerson(string personID)
    {
        Person? person = new Person(personID, "", "", 0, 0, 0);
        person = People.Find(person);
        if (person == null)
        {
            return new List<PCRTest>();
        }

        List<PCRTest> personTests = person.Tests.InOrderTraversal();
        List<PCRTest> testsCopy = personTests.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 4
    public List<PCRTest>? GetAllPositiveTestsByDistrict(uint districtID, PCRTest min, PCRTest max)
    {
        District? district = new District(districtID);
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> positiveTestsInRange = district.PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 5
    public List<PCRTest>? GetAllTestsByDistrict(uint districtID, PCRTest min, PCRTest max)
    {
        District? district = new District(districtID);
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> testsInRange = district.AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 6
    public List<PCRTest>? GetAllPositiveTestsByRegion(uint regionID, PCRTest min, PCRTest max)
    {
        Region? region = new Region(regionID);
        region = Regions.Find(region);
        if (region == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> positiveTestsInRange = region.PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 7
    public List<PCRTest>? GetAllTestsByRegion(uint regionID, PCRTest min, PCRTest max)
    {
        Region? region = new Region(regionID);
        region = Regions.Find(region);
        if (region == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> testsInRange = region.AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 8
    public List<PCRTest> GetAllPositiveTests(PCRTest min, PCRTest max)
    {
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> positiveTestsInRange = PositiveTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = positiveTestsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 9
    public List<PCRTest> GetAllTests(PCRTest min, PCRTest max)
    {
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> testsInRange = AllTests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 10
    public List<Person>? GetSickPeopleByDistrict(uint districtID, PCRTest max, int daysFromTest)
    {
        District? district = new District(districtID);
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;

        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = district.PositiveTests.IntervalSearch(min, max);

        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 11
    public List<Person>? GetSickPeopleByDistrictSorted(uint districtID, PCRTest max, int daysFromTest)
    {
        District? district = new District(districtID);
        district = Districts.Find(district);
        if (district == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;

        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = district.PositiveTests.IntervalSearch(min, max);
        positiveTestsInRange = positiveTestsInRange.OrderBy(test => test.TestData.TestValue).ToList();

        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 12
    public List<Person>? GetSickPeopleByRegion(uint regionID, PCRTest max, int daysFromTest)
    {
        Region? region = new Region(regionID);
        region = Regions.Find(region);
        if (region == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = region.PositiveTests.IntervalSearch(min, max);

        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 13
    public List<Person> GetAllSickPeople(PCRTest max, int daysFromTest)
    {
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<PCRTest> positiveTestsInRange = PositiveTests.IntervalSearch(min, max);
        return GetSickPeopleList(positiveTestsInRange);
    }

    // # 14
    public List<Person> GetSickPersonFromEveryDistrict(PCRTest max, int daysFromTest)
    {
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<District> districts = Districts.InOrderTraversal();
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<Person> sickPeople = new List<Person>();

        foreach (District district in districts)
        {
            List<PCRTest> positiveTests = district.PositiveTests.IntervalSearch(min, max);
            if (positiveTests.Count == 0)
            {
                return sickPeople;
            }
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
            Person? person = People.Find(new Person(currentMax.TestData.PersonID, "", "", 0, 0, 0));

            if (person != null)
            {
                sickPeople.Add(new Person(person.PersonID, person.Name, person.Surname, person.DayOfBirth, person.MonthOfBirth, person.YearOfBirth));
            }
        }

        return sickPeople;
    }

    // # 15
    public List<District> GetDistrictsBySickPeople(PCRTest max, int daysFromTest)
    {
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<District> districts = Districts.InOrderTraversal();
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<(District district, uint sickPeople)> districtAndPatientCount = new List<(District district, uint sickPeople)>();

        foreach (District district in districts)
        {
            List<PCRTest> positiveTests = district.PositiveTests.IntervalSearch(min, max);
            districtAndPatientCount.Add((district, (uint)positiveTests.Count));
        }

        districtAndPatientCount = districtAndPatientCount.OrderByDescending(district => district.sickPeople).ToList();
        List<District> sortedDistricts = new List<District>();
        foreach ((District district, uint sickPeople) districtCount in districtAndPatientCount)
        {
            sortedDistricts.Add(districtCount.district);
        }
        return sortedDistricts;
    }

    // # 16
    public List<Region> GetRegionBySickPeople(PCRTest max, int daysFromTest)
    {
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<Region> regions = Regions.InOrderTraversal();
        PCRTest min = GetMinForInterval(max, daysFromTest);
        List<(Region region, uint sickPeople)> regionAndPatientCount = new List<(Region region, uint sickPeople)>();

        foreach (Region region in regions)
        {
            List<PCRTest> positiveTests = region.PositiveTests.IntervalSearch(min, max);
            regionAndPatientCount.Add((region, (uint)positiveTests.Count));
        }

        regionAndPatientCount = regionAndPatientCount.OrderByDescending(region => region.sickPeople).ToList();
        List<Region> sortedRegions = new List<Region>();
        foreach ((Region region, uint sickPeople) districtCount in regionAndPatientCount)
        {
            sortedRegions.Add(districtCount.region);
        }
        return sortedRegions;
    }

    // # 17
    public List<PCRTest>? GetAllTestsBySite(uint siteID, PCRTest min, PCRTest max)
    {
        TestSite? testSite = new TestSite(siteID);
        testSite = TestSites.Find(testSite);
        if (testSite == null)
        {
            return null;
        }
        max.TestData.TestID = AllTestsByID.FindMax()!.TestData.TestID + 1;
        List<PCRTest> testsInRange = testSite.Tests.IntervalSearch(min, max);
        List<PCRTest> testsCopy = testsInRange.Select(test => new PCRTest(test.TestData)).ToList();
        return testsCopy;
    }

    // # 18
    public PCRTest? GetPCRTest(uint testID)
    {
        PCRTestData testData = new PCRTestData(0, 0, 0, 0, 0, "", testID, 0, 0, 0, false, 0.0, "");
        PCRTestByID? test = new PCRTestByID(testData);
        test = AllTestsByID.Find(test);
        if (test == null)
        {
            return null;
        }
        return new PCRTest(test.TestData);
    }

    // # 19
    public string AddPerson(Person person)
    {
        Person? found = People.Find(person);
        if (found != null)
        {
            return "This person already exists in the database!";
        }
        People.Insert(person);
        return "Person successfully added.";
    }

    // # 20
    public string DeletePCRTestByID(uint testID)
    {
        PCRTestByID? testToDelete = AllTestsByID.Find(new PCRTestByID(new PCRTestData(0, 0, 0, 0, 0, "", testID, 0, 0, 0, false, 0.0, "")));
        if (testToDelete == null)
        {
            return "Test with this ID wasn't found in the database!";
        }
        DeletePCRTest(testToDelete, false);
        return "Test was successfully deleted.";
    }

    // # 21
    public string DeletePerson(string personID)
    {
        Person? person = People.Find(new Person(personID, "", "", 0, 0, 0));
        if (person == null)
        {
            return "Person with this ID wasn't found in the database!";
        }
        List<PCRTestByID> testsToDelete = person.TestsByID.InOrderTraversal();

        foreach (PCRTestByID test in testsToDelete)
        {
            DeletePCRTest(test, true);
        }
        People.Delete(person);
        return "Person was successfully deleted along with all tests.";
    }
}